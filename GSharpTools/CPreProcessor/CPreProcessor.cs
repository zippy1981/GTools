using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace GSharpTools.CPreProcessor
{
    public class CPreProcessor
    {
        public bool Feed(string filename)
        {
            if (IsReadOnce(filename))
            {
                Trace.TraceInformation("NOT Reading {0}.", filename);
                return true;
            }
            else
            {
                Trace.TraceInformation("Reading {0}.", filename);
                return new PreprocReader(this).Feed(filename);
            }
        }

        public void AddIncludeDirectory(string directory)
        {
            Includes.Add(directory);
        }

        private bool IsReadOnce(string filename)
        {
            return ReadOnceList.Contains(filename);
        }

        internal readonly Dictionary<string, PreprocMacro> PreProcMacros = new Dictionary<string, PreprocMacro>();
        public readonly PreprocIncludes Includes = new PreprocIncludes();
        internal readonly List<string> ReadOnceList = new List<string>();

    }
}
