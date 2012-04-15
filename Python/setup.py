#! -*- Encoding: Latin-1 -*-

from distutils.core import setup
import py2exe
import sys

# If run without args, build executables, in quiet mode.
if len(sys.argv) == 1:
    sys.argv.append("py2exe")
    sys.argv.append("-q")

class Target:
    def __init__(self, **kw):
        self.__dict__.update(kw)
        self.version = "3.1"
        self.company_name = "http://p-nand-q.com"
        self.copyright = "Freeware"
        self.name = "regdiff"
        
regdiff_py = Target(

    # used for the versioninfo resource
    description = "A registry comparison file",

    # what to build
    script = "regdiff.py",
    dest_base = "regdiff")
    
setup(
    options = {"py2exe": {"compressed": 1,
                          "optimize": 2,
                          "ascii": 0,
                          "bundle_files": 1}},
    zipfile = None,
    console = [regdiff_py],
    )

import os
os.remove("dist\\w9xpopen.exe")