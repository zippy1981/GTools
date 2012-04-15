using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using GSharpTools;

namespace Which
{
    /// <summary>
    /// Find executable files on the PATH environment 
    /// </summary>
    /// <todo>
    /// <ul>
    /// - should understand syntax "which windows.h /env include" [doesn't work because anything after the filename is ignored]
    /// - should understand both /env include and /env: include
    /// </ul>
    /// </todo>
    class Which
    {
        private InputArgs Args;
        private List<string> Filenames;
        private List<string> Directories;

        /// <summary>
        /// This program finds an executable on the PATH. It can also find other stuff on the path, but 
        /// mostly it finds the executable.s
        /// </summary>
        /// <param name="args"></param>
        private void Run(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(Encoding.Default.CodePage);
            Args = new InputArgs(
                "WHICH",
                string.Format( "Version {0}\r\nFreeware written by Gerson Kurz (http://p-nand-q.com)", 
                AppVersion.Get()));

            Args.Add(InputArgType.StringList, "extension", null, Presence.Optional, "search for extension <name>, can be a ; separated list");
            Args.Add(InputArgType.StringList, "dir", null, Presence.Optional, "add directory <name>, can be a ; separated list");
            Args.Add(InputArgType.Flag, "recursive", false, Presence.Optional, "search directories recursively");
            Args.Add(InputArgType.Flag, "single", false, Presence.Optional, "stop after the first find result");
            Args.Add(InputArgType.RemainingParameters, "FILE {FILE}", null, Presence.Required, "one or more files to find");
            Args.Add(InputArgType.Parameter, "env", "PATH", Presence.Optional, "environment variable, defaults to PATH");

            if (Args.Process(args))
            {
                Filenames = Args.GetStringList("FILE {FILE}");
                Directories = Args.FindOrCreateStringList("dir");

                string EnvironmentVariableName = Args.GetString("env");
                if( EnvironmentVariableName != null )
                {
                    foreach (string token in Environment.GetEnvironmentVariable(EnvironmentVariableName, EnvironmentVariableTarget.User).Split(';'))
                    {
                        Directories.Add(token);
                    }
                }

                if( FilenamesAreIncludes() )
                {
                    AddEnvBasedDirectories("INCLUDE");
                }
                else if (FilenamesAreLibs())
                {
                    AddEnvBasedDirectories("LIB");
                }
                else
                {
                    // default: use standard windows lookup
                    Directories.Add( Directory.GetCurrentDirectory());
                    Directories.Add( Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
                    Directories.Add( Environment.GetFolderPath(Environment.SpecialFolder.System));
                    Directories.Add( Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.System)));
                    AddEnvBasedDirectories("PATH");
                }
                
                StringList.MakeUnique(ref Directories, StringComparison.OrdinalIgnoreCase);

                List<string> Extensions = Args.FindOrCreateStringList("extension");
                if (Extensions.Count == 0)
                {
                    foreach (string path in Environment.GetEnvironmentVariable("PATHEXT").Split(';'))
                    {
                        Extensions.Add(path);
                    }
                }

                List<string> FoundItems = new List<string>();
                foreach (string filename in Filenames )
                {
                    foreach (string foundname in Locate(filename))
                    {
                        if (!Contains(FoundItems, foundname))
                        {
                            FileInfo fi = new FileInfo(foundname);
                            
                            Console.WriteLine("{0} [{1}, {2} bytes]", 
                                foundname, fi.LastWriteTime, fi.Length);
                            FoundItems.Add(foundname);
                            if (Args.GetFlag("single"))
                                break;
                        }
                    }
                }
            }
        }
        private void AddEnvBasedDirectories(string name)
        {
            string content = Environment.GetEnvironmentVariable(name);
            if( content != null )
            {
                foreach (string path in content.Split(';'))
                {
                    if( path.Length > 0 )
                        Directories.Add(path);
                }
            }
        }

        private bool FilenamesAreIncludes()
        {
            foreach (string filename in Filenames)
            {
                if (GetExtension(filename).Equals(".h", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private bool FilenamesAreLibs()
        {
            foreach (string filename in Filenames)
            {
                if (GetExtension(filename).Equals(".lib", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Given a filename, return the extension (or '' if no extension given)
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <returns>Extension</returns>
        private string GetExtension(string filename)
        {
            int k = filename.LastIndexOf('.');
            if (k >= 0)
            {
                return filename.Substring(k).ToLower();
            }
            return "";
        }

        /// <summary>
        /// Check if a particular extension is contained within a list of extensions
        /// </summary>
        /// <param name="extensions">List of extensions</param>
        /// <param name="extension">Extension to find</param>
        /// <returns>True if the extension exists</returns>
        private bool Contains(List<string> extensions, string extension)
        {
            foreach (string e in extensions)
            {
                if (e.Equals(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Locate all instances of the given filename 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private IEnumerable<string> Locate(string filename)
        {
            // if the user is looking for a special 
            List<string> ExtensionsToFind = Args.GetStringList("extension");
            string e = GetExtension(filename);
            if (e.Length > 0)
            {
                ExtensionsToFind = new List<string>(); ;
                ExtensionsToFind.Add(e);
                int k = filename.LastIndexOf('.');
                filename = filename.Substring(0, k);                
            }

            string lookfor = filename + ".*";
            SearchOption so = SearchOption.TopDirectoryOnly;
            if (Args.GetFlag("recursive"))
                so = SearchOption.AllDirectories;

            foreach (string directory in Args.GetStringList("dir"))
            {
                string[] files;
                if (!Directory.Exists(directory))
                {
                    continue;
                }
                try
                {
                    files = Directory.GetFiles(directory, lookfor, so);
                }
                catch (ArgumentException ep)
                {
                    Console.WriteLine(ep);
                    Console.WriteLine(ep.StackTrace);
                    Console.WriteLine(string.Format("while parsing '{0}'", directory));
                    Console.WriteLine(string.Format("looking for '{0}'", lookfor));
                    break;
                }
                catch (Exception ep)
                {
                    Console.WriteLine(ep);
                    Console.WriteLine(ep.StackTrace);
                    Console.WriteLine(string.Format("while parsing '{0}'", directory));
                    Console.WriteLine("lookfor: {0}", lookfor);
                    continue;
                }
                foreach (string foundname in files)
                {
                    e = GetExtension(foundname);
                    if (Contains(ExtensionsToFind, e))
                    {
                        yield return foundname;
                    }
                }
            }
        }

        /// <summary>
        /// Main function: defer program logic
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            new Which().Run(args);
        }
    }
}
