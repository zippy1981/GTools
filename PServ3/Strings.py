#! -*- Encoding: Latin-1 -*-

import sys
import create_csharp_strings

if __name__ == "__main__":
    create_csharp_strings.reader().read("strings.xml", "pserv3")
