#! -*- Encoding: Latin-1 -*-

import sys
import xml.parsers.expat
import datetime
import cStringIO
import traceback
import codecs

class reader(object):
    
    def start_element(self, name, attrs):   
        if name == "string":
            id = attrs["id"]
            description = attrs["description"]
            text = attrs["text"]
            self.strings.append( (id, description, text, ) )

    def read(self, filename, classname):
        self.strings = []
        
        p = xml.parsers.expat.ParserCreate() 
        p.StartElementHandler = self.start_element
        p.Parse(open(filename).read())
        
        self.create_strings_cs("strings.cs", classname)
        
    def create_strings_cs(self, filename, classname):
        output = open(filename, "wb")
        output.write(codecs.BOM_UTF8);
        output.write('// THIS IS GENERATED CODE, DO NOT EDIT THIS!!!!\r\n')
        output.write('using System;\r\n')
        output.write('using System.Collections.Generic;\r\n')
        output.write('using System.Text;\r\n')
        output.write('using System.Diagnostics;\r\n')
        output.write('\r\n')
        output.write('namespace %s\r\n' % (classname, ))
        output.write('{\r\n')
        output.write('    internal static class IDS\r\n')
        output.write('    {\r\n')

        for string in self.strings:
            id, description, text = string
            output.write('\r\n')
            output.write('        /// <summary>\r\n')
            output.write('        /// %s\r\n' % (description.replace('\r', '').replace('\n', ''), ) )
            output.write('        /// </summary>\r\n')
            output.write('        internal static string %s { get; private set; }\r\n' % (id, ))
            
        output.write('\r\n')
        output.write('        private static string AssignString(Dictionary<string, string> inputStrings, string key, string defval)\r\n')
        output.write('        {\r\n')
        output.write('            if (inputStrings.ContainsKey(key))\r\n')
        output.write('                return inputStrings[key];\r\n')
        output.write('            return defval;\r\n')
        output.write('        }\r\n')
        output.write('\r\n')
        output.write('        internal static void AssignStrings(Dictionary<string, string> inputStrings)\r\n')
        output.write('        {\r\n')

        for string in self.strings:
            id, description, text = string
            
            output.write('            %s = AssignString(inputStrings, "%s", "%s"); \r\n' % (id.encode("utf-8"), id.encode("utf-8"), text.encode("utf-8"), ))
            
        output.write('        }\r\n')
        output.write('    }\r\n')
        output.write('}\r\n')
        
        output.close()

