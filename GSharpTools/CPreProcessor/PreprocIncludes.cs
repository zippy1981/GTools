using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace GSharpTools.CPreProcessor
{
    public class PreprocIncludes : List<string>
    {
        public PreprocIncludes()
        {
            Add(Directory.GetCurrentDirectory());
            string includes = Environment.GetEnvironmentVariable("INCLUDE");
            if (includes != null)
            {
                foreach (string incdir in includes.Split(';'))
                {
                    Add(incdir.Trim());
                }
            }

            Trace.TraceInformation("Include path has {0} directories:", Count);
            foreach (string incdir in this)
            {
                Trace.TraceInformation("- {0}", incdir);
            }
        }

        public string Locate(string filename)
        {
            foreach(string directory in this)
            {
                string pathname = Path.Combine(directory, filename);
                if (File.Exists(pathname))
                    return pathname;
            }
            return null;
        }
    }
}
