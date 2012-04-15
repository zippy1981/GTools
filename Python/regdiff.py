#! -*- Encoding: Latin-1 -*-

import time
import pprint
import sys
import psyco
import warnings
import codecs
import encodings
import gtools.win32.registry
import gtools.dicttools
import optparse

__VERSION__ = "3.5"

unicode_encoding = "utf-16le"
ansi_encoding = "mbcs"                   

def decodeUnicode(s):
    # str => unicode
    assert isinstance(s, str)
    return s.decode(unicode_encoding)

def encodeUnicode(s):
    # str => unicode
    assert isinstance(s, unicode)
    return s.encode(unicode_encoding)

def makeSureIsUnicode(s):
    if isinstance(s, str):
        return s.decode(ansi_encoding)
    assert isinstance(s, unicode)
    return s

def makeSureIsAnsi(s):
    if isinstance(s, unicode):
        return s.encode(ansi_encoding)
    
    if not isinstance(s, str):
        return makeSureIsAnsi(repr(s))
    return s


def decodebinary_enum(data):
    token = 0
    token_valid = False
    for index, char in enumerate(data):
        if char == ",":
            yield chr(token)
            token = 0
            token_valid = False
            
        else:
            token <<= 4
            token |= int(char, 16)
            token_valid = True
    if token_valid:
        yield chr(token)

def decodebinary(data):
    return "".join([t for t in decodebinary_enum(data)])
    
def decodestr(data):
    result = []
    assert data[0] == '"'
    assert data[-1] == '"'
    
    escape = False
    for index, char in enumerate(data[1:-1]):
        if escape:
            if char == '"':
                result.append( '"' )
            elif char == 'n':
                result.append( '\n' )
            elif char == '\\':
                result.append( '\\' )
                
            else:
                print "unknown escape sequence \\%c" % char
                assert False
                result.append( "\\" )
                result.append( char )
            
            escape = False
        
        elif char == '\\':
            escape = True
            
        else:
            result.append( char )

    return "".join(result)

class value(object):
    def __init__(self, name, data):
        if name is None:
            return
        
        try:
            if name == '@':
                self.name = None
            else:
                self.name = decodestr(name)
        except AssertionError, e:
            print repr(name)
            raise e            
        
        if data.startswith("hex:"):
            self.type = gtools.win32.registry.REG_BINARY
            self.data = decodebinary(data[4:])
            
        elif data.startswith("dword:"):
            self.type = gtools.win32.registry.REG_DWORD
            try:
                self.data = int(data[6:], 16)
                if self.data > 0x7FFFFFFFL:
                    self.data = self.data - 0x100000000L
            except ValueError:
                self.data = repr(data)
                                
        elif data.startswith('"'):
            self.type = gtools.win32.registry.REG_SZ
            self.data = decodestr(data)
            
        elif data.startswith("hex("):
            k = data.find(")")
            assert k > 0
            self.type = int(data[4:k], 16)
            self.data = decodebinary(data[k+2:])
                
            if self.type == gtools.win32.registry.REG_MULTI_SZ:
                items = self.data.decode("utf-16le", "ignore").split("\0")
                while items and not items[-1]:
                    del items[-1]
                self.data = items
                
        else:
            print data[:6]
            assert False
    
    def from_regedt32(self, name, regtype, data):
        self.name = name
        if regtype == u"REG_DWORD":
            self.type = gtools.win32.registry.REG_DWORD
            self.data = int(data, 16)
            if self.data > 0x7FFFFFFFL:
                self.data = self.data - 0x100000000L
                
        elif regtype == u"REG_SZ":
            self.type = gtools.win32.registry.REG_SZ
            self.data = data
            
        else:
            print "ERROR, unsupported type %s" % regtype
    
    def __repr__(self):
        return "<value %r:%s:%r>" % (self.name, gtools.win32.registry.TypeAsString[self.type], self.data)

class key(object):
    def __init__(self, name):
        self.name = name
        
    def __repr__(self):
        return "<key %r>" % self.name

class regedit5File(object):
    def __init__(self, filename):
        self.header = '\xff\xfe' + encodeUnicode(u'Windows Registry Editor Version 5.00\r\n\r\n')
        self.output = file(filename, "wb")
    
    def createFrom(self, items, data):
        self.output.write( self.header )
        keynames = items.keys()
        keynames.sort()
        for keyname in keynames:
            self.current_keyname = keyname
            self.write( u'[' + makeSureIsUnicode(keyname) + u']\r\n' )
            valuenames = items[keyname].keys()
            valuenames.sort()
            for valuename in valuenames:
                d = items[keyname][valuename]
                if isinstance(d, value):
                    self.writeValue( d )
                else:
                    try:
                        self.writeValue( data[keyname][valuename] )
                    except KeyError, e:
                        print type(data[keyname])
                        pprint.pprint(data[keyname].keys())
                        
                        raise e
                
            self.write( u'\r\n' )
        self.output.close()
        
    def writeValue(self, value):
        if value.name is not None:
            ds = makeSureIsUnicode(value.name)
            ds = ds.replace(u"\\", u"\\\\")
            ds = ds.replace(u'"', u'\\"')
            name_header = u'"' + ds + u'"='
            
        else:
            name_header = u'@='
            
        self.write( name_header )
        self.current_valuename = name_header

        if value.type == gtools.win32.registry.REG_DWORD:
            if value.data < 0:
                self.write( u'dword:%08x\r\n' % (0x100000000L + value.data ) )
            else:
                self.write( u'dword:%08x\r\n' % value.data )                
            
        elif value.type == gtools.win32.registry.REG_SZ:
            ds = makeSureIsUnicode(value.data)
            ds = ds.replace(u"\\", u"\\\\")
            ds = ds.replace(u'"', u'\\"')
            self.write( u'"' + ds + u'"\r\n' )
            
        elif value.type == gtools.win32.registry.REG_BINARY:  
            self.write_hexblock( u"hex:", value.data, len(name_header) )
            
        elif value.type == gtools.win32.registry.REG_MULTI_SZ:
            assert isinstance(value.data, list)
            data = u"\0".join( value.data ) + u"\0"
            data = encodeUnicode(data)
            self.write_hexblock( u"hex(%x):" % value.type, data, len(data) )
            
        else:
            self.write_hexblock( u"hex(%x):" % value.type, value.data, len(name_header) )

    def write_hexblock(self, header, data, namelen):
        if data is None:
            self.write( u"\r\n" )
            return
            
        if isinstance(data, unicode):
            data = encodeUnicode(data)
        
        if not isinstance(data, str):
            print "ERROR in [%s]: %s has wrong type %s for hexblock. Data is: %r" % (self.current_keyname, self.current_valuename, type(data), data)
            
            self.write( u"\r\n" )
            assert False
            return
        
        data = [u"%02x" % ord(c) for c in data]
        
        t = namelen + len(header)
        
        chars_available = 79 - t
        if chars_available < 0:
            max_bytes_to_write = 25
            
        else:
            max_bytes_to_write = chars_available / 3
            if max_bytes_to_write < 1:
                max_bytes_to_write = 1

        start_index = 0
        max_index = len(data)

        while start_index <= max_index:
            self.write(header)
            header = u"  "
            temp =  u",".join( data[start_index:start_index+max_bytes_to_write] )
            self.write( temp )
            start_index += max_bytes_to_write
            if start_index < max_index:
                self.write( u",\\\r\n" )
            else:
                self.write( u"\r\n" )
                break
            
            max_bytes_to_write = 25

    def write(self, data):
        self.output.write( encodeUnicode(data) )
        
class regedit4File(regedit5File):
    def __init__(self, filename):
        regedit5File.__init__(self, filename)
        self.header = 'REGEDIT4\r\n\r\n'

    def write(self, data):
        self.output.write( makeSureIsAnsi(data) )

class regedit(object):
    def __init__(self, debugmode):
        self.debugMode = debugmode
        
    def read(self, filename):
        print "Reading %s" % filename
        self.statements = []
        
        if self.debugMode:
            t0 = time.time()
            
        self.data = open(filename, "rb").read()
        if self.data[:2] == '\xff\xfe':
            self.data = decodeUnicode(self.data[2:])
            
        else:
            self.data = self.data.decode("latin-1")
            
        assert isinstance(self.data, unicode)
        
        if self.debugMode:
            t1 = time.time()
            print "%.2f seconds to read %d chars" % (t1 - t0, len(self.data))
            t0 = time.time()
        
        self.tokenize( self.data )
        
        if self.debugMode:
            t1 = time.time()
            output = open("tokens.txt","w")
            pprint.pprint(self.tokens, output)
            print "%.2f seconds to create %d tokens" % (t1 - t0, len(self.tokens))
        
        try:
            if self.debugMode:
                t0 = time.time()
                
            if self.is_regedt32_format:
                self.statementize_regedt32( self.tokens )
                
            else:
                self.statementize( self.tokens )
            
        finally:
            if self.debugMode:
                t1 = time.time()
                output = open("statements.txt","w")
                pprint.pprint(self.statements, output)
                
                print "%.2f seconds to create %d statements" % (t1 - t0, len(self.statements))
        
        return self.statements
        
    def registry(self, d):
        if d.keys() == ['']:
            del d['']
            d["HKEY_LOCAL_MACHINE"] = True
            d["HKEY_USERS"] = True
        
        self.result_d = gtools.dicttools.Dict(case_sensitive = False)
        self.registryRecursive(d.keys())
        return self.result_d

    def registryRecursive(self, keynames):
        for keyname in keynames:
            try:
                k = gtools.win32.registry.key(keyname)
            except WindowsError:
                continue
            
            self.result_d[keyname] = gtools.dicttools.Dict(case_sensitive = False)
            temp = self.result_d[keyname]
            keynames = []
            
            try:
                for value_name, value_type, value_data in k.values():
                    v = value(None, "")
                    v.name = value_name
                    v.type = value_type
                    v.data = value_data
                    temp[value_name] = v
                
                keynames = ["%s\\%s" % (keyname, n) for n in k.keys()]
                
            finally:
                k.close()
                
            self.registryRecursive(keynames)

        
    def asDictionary(self):
        result = gtools.dicttools.Dict(case_sensitive = False)
        current = None
        for stmt in self.statements:
            if isinstance(stmt, key):
                result[stmt.name] = gtools.dicttools.Dict(case_sensitive = False)
                current = result[stmt.name]
                
            else:
                current[stmt.name] = stmt

        return result
        
    def statementize_regedt32(self, tokens):
        self.statements = []
        tuple_size = 0
        for index, token in enumerate(tokens):
            if tuple_size > 0:
                tuple_size -= 1
                
            else:
                k = token.find("\\")
                if k >= 0:
                    name = token[:k]
                    if gtools.win32.registry.KeyLookupDict.has_key(name):
                        tuple_size = 2
                        self.statements.append( key( token ) )
                        
                    else:
                        pprint.pprint(subtokens_2)
                        print token
                        assert False
                else:
                    tuple_size = 3
                    v = value(None, None)
                    v.from_regedt32(*tokens[index+1:index+4])
                    self.statements.append( v )
    
        return self.statements
       
    def statementize(self, tokens):
        if "Windows Registry Editor Version 5.00" == " ".join(tokens[:5]):
            startindex = 5
            
        elif tokens[0] == "REGEDIT4":
            startindex = 1
            
        else:
            raise Exception("Invalid file format: %r" % tokens[:5])
            
        data, self.statements = [], []
        continueData = False
        last_keyname = None
        for index, token in enumerate(tokens):
            if index < startindex:
                continue
            
            if token.startswith("["):
                last_keyname = token[1:-1]
                self.statements.append( key( last_keyname ) )
                mode = 1
                data = []
                
            elif mode == 1:
                name = token
                mode = 2
                
            elif mode == 2:
                if token != '=':
                    print "ERROR AT INDEX %d" % index
                    assert token == '='
                mode = 3
                
            elif mode == 3:
                if token.endswith("\\"):
                    data.append( makeSureIsUnicode( token[:-1] ) )
                
                else:
                    data.append( makeSureIsUnicode( token ) )
                    data = u"".join( data )
                    self.statements.append( value( name, data ) )
                    mode = 1
                    data = []
            
        return self.statements
        
    def tokenize(self, data):

        if not data.startswith(u"Windows Registry Editor Version 5.00") and not data.startswith(u"REGEDIT4"):
            return self.tokenize_regedt32(data)

        self.is_regedt32_format = False
        self.tokens = []
        recording = False
        isstring = False
        isbracket = False
        escape = False
        iscomment = False
        
        for index, char in enumerate(data):
            
            if iscomment:
                if char == '\n':
                    iscomment = False
                continue
            
            if isstring:
                if escape:
                    escape = False
                elif char == '\\':
                    escape = True
                elif char == '"':
                    assert recording
                    self.tokens.append(data[startindex:index+1])
                    recording = False
                    isstring = False
                continue
                    
            if isbracket:
                if char == ']':
                    assert recording
                    self.tokens.append(data[startindex:index+1])
                    recording = False
                    isbracket = False
                continue
                
            if char == ';':
                if recording:
                    self.tokens.append(data[startindex:index])
                    recording = False
                iscomment = True
                continue
                    
            elif char in ' \t\n\r':
                if recording:
                    self.tokens.append(data[startindex:index])
                    recording = False
                    
            elif char in "=":
                if recording:
                    self.tokens.append(data[startindex:index])
                    recording = False
                self.tokens.append(char)
                
            elif char == '"':
                if recording:
                    self.tokens.append(data[startindex:index])
                    recording = False
                recording = True
                escape = False
                startindex = index
                isstring = True
                
            elif char == '[':
                if recording:
                    self.tokens.append(data[startindex:index])
                    recording = False
                recording = True
                startindex = index
                isbracket = True
                
            elif not recording:
                recording = True
                startindex = index
                
        if recording:
            self.tokens.append(data[startindex:])
            
        return self.tokens
        
    def tokenize_regedt32(self, data):
        self.is_regedt32_format = True
        self.tokens = []
        recording = False
        ignore_spaces = True
        header_expected = True
        has_any_spaces = False
        
        for index, char in enumerate(data):
            if header_expected and char == u':':
                if recording:
                    recording = False
                    header_expected = False
            
            elif char in u'\r\n':
                if recording:
                    self.tokens.append(data[startindex:index])
                    recording = False
                elif has_any_spaces:
                    self.tokens.append('')
                header_expected = True
                has_any_spaces = False
                    
            elif not recording:
                if ignore_spaces and char in " \t":
                    has_any_spaces = True
                    continue
                startindex = index
                recording = True
                
        if recording:
            self.tokens.append(data[startindex:index])
            recording = False
        elif has_any_spaces:
            self.tokens.append('')            
        return self.tokens       

def uniqueCombinations(items, n):
    if n==0: yield []
    else:
        for i in xrange(len(items)):
            for cc in uniqueCombinations(items[i+1:],n-1):
                yield [items[i]]+cc

class CompareDicts(object):
    def __init__(self, callback):
        self.callback = callback

    def compare(self, data_a, data_b):
        keys_missing_in_a, keys_missing_in_b = [], []
        keys_found = gtools.dicttools.Dict(case_sensitive = False)
        
        for key_a in data_a.keys():
            try:
                existing_in_b = data_b[key_a]
                keys_found[key_a] = True
            except KeyError:
                keys_missing_in_b.append( key_a )
                continue
            
            existing_in_a = data_a[key_a]
            self.callback( key_a, existing_in_a, existing_in_b )
                
        for key_b in data_b.keys():
            try:
                existing = keys_found[key_b]
            except KeyError:
                keys_missing_in_a.append( key_b )
                
        return keys_missing_in_a, keys_missing_in_b

class compare(object):
    def check(self, name_a, data_a, name_b, data_b, diffFile, mergeFile, fileVersion, ignoreCase):
        self.name_a, self.data_a = name_a, data_a
        self.name_b, self.data_b = name_b, data_b
        self.ignoreCase = ignoreCase
        self.values_missing_in_a, self.values_missing_in_b = [], []
        self.type_mismatch = []
        self.data_mismatch = []
        self.keys_missing_in_a, self.keys_missing_in_b = [], []
        self.nrOfDifferences = 0
        
        if not data_a is data_b:
            print "Comparing %s and %s" % (self.name_a, self.name_b)
            print
        
            self.keys_missing_in_a, self.keys_missing_in_b = CompareDicts(self.compareKeys).compare(self.data_a, self.data_b)
            
            self.printMissingKeys(self.keys_missing_in_a, self.name_a)
            self.printMissingKeys(self.keys_missing_in_b, self.name_b)
            self.printMissingValues(self.values_missing_in_a, self.name_a)
            self.printMissingValues(self.values_missing_in_b, self.name_b)
            self.printMismatchedType(self.type_mismatch)
            self.printMismatchedData(self.data_mismatch)
    
            if not self.nrOfDifferences:
                print "No differences detected..."
                
            elif self.nrOfDifferences == 1:
                print "A total of 1 difference detected"
    
            else:
                print "A total of %d differences detected" % self.nrOfDifferences 

        if fileVersion == 4:
            cls = regedit4File
        else:
            cls = regedit5File

        if diffFile:
            self.createFile(diffFile, cls)
            
        if mergeFile:
            self.createFile(mergeFile, cls, self.data_b)    
    
    def createFile(self, filename, regeditFileClass, temp = gtools.dicttools.Dict(case_sensitive = False)):
        print "Creating %s" % filename
        
        for keyname in self.keys_missing_in_b:
            temp[keyname] = gtools.dicttools.Dict(case_sensitive = False)
            existing = temp[keyname]
            for valuename in self.data_a[keyname]:
                existing[valuename] = True       
        
        for itemlist in (self.values_missing_in_b, self.type_mismatch, self.data_mismatch):
            for value in itemlist:
                keyname, valuename = value
                try:
                    existing = temp[keyname]
                except KeyError:
                    temp[keyname] = gtools.dicttools.Dict(case_sensitive = False)
                    existing = temp[keyname]
                existing[valuename] = True
        
        regeditFileClass(filename).createFrom(temp, self.data_a)

    def compareKeys(self, key, data_a, data_b):
        self.context = key
        a_values, b_values = CompareDicts(self.compareValues).compare(data_a, data_b)
        
        for value_a in a_values:
            self.values_missing_in_a.append( (key, value_a) )

        for value_b in b_values:
            self.values_missing_in_b.append( (key, value_b) )
        
    def compareValues(self, key, data_a, data_b):
        assert isinstance(data_a, value)
        assert isinstance(data_b, value)
        
        if data_a.type <> data_b.type:
            self.type_mismatch.append( (self.context, key) )
            return
        
        data_a = data_a.data
        data_b = data_b.data

        if self.ignoreCase:
            try:
                if isinstance(data_a, str) or isinstance(data_a, unicode):
                    data_a = data_a.lower()
            except:
                pass

            try:
                if isinstance(data_b, str) or isinstance(data_b, unicode):
                    data_b = data_b.lower()
            except:
                pass
                
        if data_a <> data_b:
            self.data_mismatch.append( (self.context, key) )

    def printMismatchedType(self, items):
        if items:
            if len(items) == 1:
                print "The following value has a type mismatch"
            else:
                print "The following %d values have a type mismatch" % len(items)
                
            for item in items:
                print "- %s" % "\\".join(item)
                self.nrOfDifferences += 1
                
                keyname, valuename = item
                
                value_a = self.data_a[keyname][valuename]
                print "\tType in %s: %s" % (self.name_a, gtools.win32.registry.TypeAsString[value_a.type])
                
                value_b = self.data_b[keyname][valuename]
                print "\tType in %s: %s" % (self.name_b, gtools.win32.registry.TypeAsString[value_b.type])
            print                

    def printMismatchedData(self, items):
        if items:
            if len(items) == 1:
                print "The following value has a data mismatch"
            else:
                print "The following %d values have a data mismatch" % len(items)
                
            for item in items:
                a, b = item
                print "- %s\\%s" % (makeSureIsAnsi(a), makeSureIsAnsi(b))
                self.nrOfDifferences += 1
                
                keyname, valuename = item
                
                value_a = self.data_a[keyname][valuename]
                data_a = repr(value_a.data)
                if len(data_a) > 100:
                    data_a = data_a[:100] + "..."
                
                print "\tData in %30s: %s: %s" % (self.name_a, gtools.win32.registry.TypeAsString[value_a.type], data_a)
                
                value_b = self.data_b[keyname][valuename]
                data_b = repr(value_b.data)
                if len(data_b) > 100:
                    data_b = data_b[:100] + "..."
                
                print "\tData in %30s: %s: %s" % (self.name_b, gtools.win32.registry.TypeAsString[value_b.type], data_b)
            print


    def printMissingKeys(self, keyslist, keysname):
        if keyslist:
            keyslist.sort()
            if len(keyslist) == 1:
                print "The following key is missing in %s" % (keysname)
            else:
                print "The following %d keys are missing in %s" % (len(keyslist), keysname)
                
            for key in keyslist:
                print "- %s" % makeSureIsAnsi(key)
                self.nrOfDifferences += 1
            print

    def printMissingValues(self, valueslist, valuesname):
        if valueslist:
            valueslist.sort()
            if len(valueslist) == 1:
                print "The following value is missing in %s" % (valuesname)
            else:
                print "The following %d values are missing in %s" % (len(valueslist), valuesname)
                
            for key, name in valueslist:
                print "- %s\\%s" % (makeSureIsAnsi(key), makeSureIsAnsi(name))
                self.nrOfDifferences += 1
            print

def main():
    psyco.full()

    warnings.filterwarnings("ignore", "", Warning) 
    
    print 'REGDIFF %s - Freeware written by Gerson Kurz (http://www.p-nand-q.com)' % __VERSION__
    print
    
    parser = optparse.OptionParser()
    parser.add_option("-r", "--registry",  action="store_true", dest="regcmp", default=False, help="compare against the current registry")
    parser.add_option("-d", "--diff", dest="diff", help="create diff file", metavar="FILE")
    parser.add_option("-m", "--merge", dest="merge", help="create merged file", metavar="FILE")
    parser.add_option("-4", "--regedt32", action="store_true", dest="regedt4", default=5, help="file format from Windows NT 4.0 (default: Unicode)")
    parser.add_option("-i", "--ignore-case", action="store_true", dest="ignore_case", default=False, help="ignore case (default: case sensitive)")
    parser.add_option("-v", "--verbose",  action="store_true", dest="verbose", default=False, help="verbose (not recommended!)")
    parser.add_option("-s", "--selftest", action="store_true", dest="selftest", default=False, help="selftest (not recommended!)")
    
    (options, files) = parser.parse_args()
    
    diff, diffFile = False, options.diff
    if diffFile:
        diff = True
    merge, mergeFile = False, options.merge
    if mergeFile:
        merge = True
    regcmp, fileVersion = options.regcmp, 5
    ignoreCase, debugmode = options.ignore_case, options.verbose
    
    if options.regedt4:
       fileVersion = 4 
    
    if options.selftest:
        return selftest()
    
    if not files:
        return parser.print_help()

    dicts, p = [], regedit(debugmode)    
    for index, filename in enumerate(files):

        found = False
        for k in gtools.win32.registry.KeyLookupDict:
            if filename.startswith(k+"\\") or filename == k:
                dicts.append( p.registry( gtools.dicttools.Dict({filename: True}, case_sensitive = False)) )
                found = True
                break
        
        if found:
            continue
        
        if filename[:4].lower() == "reg:":
            files[index] = "the registry"
            dicts.append( p.registry( gtools.dicttools.Dict({filename[4:]: True}, case_sensitive = False)))
        else:
            p.read(filename)
            dicts.append( p.asDictionary() )
    
    c = compare()
    if regcmp:
        for index in range(len(files)):
            c.check("the registry", p.registry(dicts[index]), files[index], dicts[index], diffFile, mergeFile, fileVersion, ignoreCase)
    
    elif len(files) > 1:
        for a, b in uniqueCombinations(range(len(files)), 2):
            c.check(files[a], dicts[a], files[b], dicts[b], diffFile, mergeFile, fileVersion, ignoreCase)
    else:
        c.check(files[0], dicts[0], files[0], dicts[0], diffFile, mergeFile, fileVersion, ignoreCase)

if __name__ == "__main__":
#    sys.argv = ["regdiff.py", "\\temp\\my-conemu.txt", "\\temp\\my-conemu2.txt", "-v" ]
    main()
    