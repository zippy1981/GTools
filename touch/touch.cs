using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;
using GSharpTools;
using System.Security.Cryptography;

namespace touch
{
    /// <summary>
    /// Find executable files on the PATH environment 
    /// </summary>
    class touch
    {
        private InputArgs Args;
        private DateTime TimeStamp = DateTime.Now;

        /// <summary>
        /// This flag indicates whether subdirectories should be recursed into or not.
        /// </summary>
        private bool Recursive = true;

        private string CalculateMD5(string filename)
        {
            long start = DateTime.Now.Ticks;
            string result = null;

            using (HashAlgorithm hashAlg = MD5.Create())
            {
                using (Stream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    byte[] hash = hashAlg.ComputeHash(file);

                    // Display the hash code of the file to the console.
                    result = BitConverter.ToString(hash).Replace("-", "");
                }
            }
            return result;
        }

        /// <summary>
        /// This program calculates the MD5 hashes for the given input files
        /// </summary>
        /// <param name="args"></param>
        private void Run(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(Encoding.Default.CodePage);
            Args = new InputArgs(
                "touch",
                string.Format("Version {0}\r\nFreeware written by Gerson Kurz (http://p-nand-q.com)",
                AppVersion.Get()));

            Args.Add(InputArgType.Flag, "recursive", false, Presence.Optional, "search directories recursively");
            Args.Add(InputArgType.RemainingParameters, "DIR {DIR}", null, Presence.Optional, "one or more directories to search");

            if (Args.Process(ref args))
            {
                Recursive = Args.GetFlag("recursive");

                List<string> directories = Args.GetStringList("DIR {DIR}");
                if (directories == null)
                {
                    Read(Directory.GetCurrentDirectory());
                }
                else
                {
                    foreach (string directory in directories)
                    {
                        Read(SanitizeInput(directory));
                    }
                }
            }
        }

        private void Read(string directory)
        {
            // if the last part of the directory is actually  awildcard, use that instead
            string search_pattern = "*";

            string optional_filepart = Path.GetFileName(directory);
            if (!string.IsNullOrEmpty(optional_filepart) && IsWildcard(optional_filepart))
            {
                search_pattern = optional_filepart;
                directory = Path.GetDirectoryName(directory);
            }


            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Warning, {0} doesn't exist...", directory);
                return;
            }

            string[] files = null;
            try
            {
                files = Directory.GetFiles(directory, search_pattern);
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry, unable to read directory '{0}': {1}.", directory, e.Message);
                return;
            }
            foreach (string filename in files)
            {
                string readpath = directory.Equals(".") ? filename : Path.Combine(directory, filename);
                Check(readpath);
            }
            if (Recursive)
            {
                foreach (string subdir in Directory.GetDirectories(directory))
                {
                    Read(Path.Combine(directory, subdir));
                }
            }
        }

        private void Check(string filename)
        {
            try
            {
                File.SetLastWriteTime(filename, TimeStamp);
            }
            catch (Exception e)
            {
                Tools.DumpException(e, "SetLastWriteTime({0}) failed", filename);
                Console.WriteLine(">> Error '{0}' while touching '{1}'",
                    e.Message, filename);
            }
        }

        static private string RemoveLastPathPart(string text)
        {
            string[] tokens = text.Split('\\');
            StringBuilder result = new StringBuilder();
            bool first = true;
            for (int i = 0; i < (tokens.Length - 1); ++i)
            {
                if (first)
                    first = false;
                else
                    result.Append("\\");
                result.Append(tokens[i]);
            }
            return result.ToString();
        }

        static private bool IsWildcard(string name)
        {
            return name.Contains("*") || name.Contains("?");
        }

        static private string SanitizeInput(string input)
        {
            bool IsUNCPath = false;
            if (input.StartsWith("\\\\"))
            {
                IsUNCPath = true;
                input = input.Substring(3);
            }

            string[] tokens = input.Split('\\');

            // replace leading "." / ".." tokens             
            for (int i = 0; i < tokens.Length; ++i)
            {
                if (i == 0)
                {
                    if (tokens[0] == ".")
                    {
                        tokens[0] = Directory.GetCurrentDirectory();
                    }
                    else if (tokens[0] == "..")
                    {
                        string current_directory = Directory.GetCurrentDirectory();
                        DirectoryInfo parent = Directory.GetParent(current_directory);
                        if (parent != null)
                            tokens[0] = parent.FullName;
                        else
                            tokens[0] = current_directory;
                    }
                    else if (IsWildcard(tokens[0]))
                    {
                        tokens[0] = string.Format("{0}\\{1}",
                            Directory.GetCurrentDirectory(),
                            tokens[0]);
                    }
                }
                else
                {
                    if (tokens[i] == "..")
                    {
                        Debug.Assert(i > 0);
                        tokens[i] = null;

                        int j = i - 1;
                        while ((j >= 0) && (tokens[j] == null))
                        {
                            --j;
                        }
                        if (j >= 0)
                        {
                            tokens[j] = tokens[j].Contains("\\") ? RemoveLastPathPart(tokens[j]) : null;
                        }
                        if (tokens[0] == null)
                            tokens[0] = Directory.GetCurrentDirectory();

                    }
                    else if (tokens[i] == ".")
                    {
                        tokens[i] = null;
                    }
                }
            }

            StringBuilder result = new StringBuilder();
            bool first = true;
            if (IsUNCPath)
            {
                result.Append("\\\\");
                first = false;
            }
            for (int i = 0; i < tokens.Length; ++i)
            {
                if (tokens[i] != null)
                {
                    if (!first)
                        result.Append("\\");
                    else
                        first = false;

                    result.Append(tokens[i]);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Main function: defer program logic
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            new touch().Run(args);
        }
    }

}
