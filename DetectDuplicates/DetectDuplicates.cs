﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;
using GSharpTools;
using GSharpTools.DBTools;
using GSharpSQLite;
using System.Security.Cryptography;

namespace DetectDuplicates
{
    /// <summary>
    /// Find executable files on the PATH environment 
    /// </summary>
    class DetectDuplicates
    {
        private InputArgs Args;

        /// <summary>
        /// This flag indicates whether subdirectories should be recursed into or not.
        /// </summary>
        private bool Recursive = true;

        /// <summary>
        /// this is a dictionary of the following format:
        /// Cache[File Size][MD5 Hash] = Existing Filename
        /// </summary>
        private Dictionary<long, Dictionary<string, string>> Cache = new Dictionary<long, Dictionary<string, string>>();
        private long HashesRead = 0;
        private long FilesDetected = 0;
        private long BytesDetected = 0;
        private long FilesChecked = 0;
        private long BytesChecked = 0;
        private bool DeleteFiles = false;
        private DateTime StartupTime = DateTime.Now;
        private CachedHashes DatabaseCache = new CachedHashes();
        private long TimeSpentCalculatingMD5s = 0;

        private string CalculateMD5(string filename, long size, Dictionary<string, string> knownHashes)
        {
            long start = DateTime.Now.Ticks;

            string result = DatabaseCache.LookupHash(filename);
            if (result != null)
                return result;

            using (HashAlgorithm hashAlg = MD5.Create())
            {
                using (Stream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    byte[] hash = hashAlg.ComputeHash(file);

                    // Display the hash code of the file to the console.
                    result = BitConverter.ToString(hash);

                    DatabaseCache.Write(result, filename);
                    ++HashesRead;
                }
            }
            TimeSpentCalculatingMD5s += DateTime.Now.Ticks - start;
            return result;
        }
 
        /// <summary>
        /// This program finds an executable on the PATH. It can also find other stuff on the path, but 
        /// mostly it finds the executable.s
        /// </summary>
        /// <param name="args"></param>
        private void Run(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(Encoding.Default.CodePage);
            Args = new InputArgs(
                "DETECTDUPLICATES",
                string.Format("Version {0}\r\nFreeware written by Gerson Kurz (http://p-nand-q.com)",
                AppVersion.Get()));

            Args.Add(InputArgType.Flag, "recursive", false, Presence.Optional, "search directories recursively");
            Args.Add(InputArgType.Parameter, "cache", null, Presence.Optional, "cache file name");
            Args.Add(InputArgType.Flag, "delete", false, Presence.Optional, "delete files (default: list only)");
            Args.Add(InputArgType.RemainingParameters, "DIR {DIR}", null, Presence.Required, "one or more directories to search");

            if (Args.Process(ref args))
            {
                DatabaseCache.Initialize(Args.GetString("cache"), Cache);
                Recursive = Args.GetFlag("recursive");
                DeleteFiles = Args.GetFlag("delete");

                foreach (string directory in Args.GetStringList("DIR {DIR}"))
                {
                    Read(SanitizeInput(directory));
                }
            }
            if (FilesChecked > 0)
            {
                DatabaseCache.Flush();

                TimeSpan elapsed = DateTime.Now - StartupTime;

                Console.WriteLine("____________________________________________________________________________________");
                Console.WriteLine("DetectDuplicates finished after {0}", elapsed);
                
                Console.WriteLine("Checked a total of {0} files using {1}, calculating {2} hashes.",
                    FilesChecked,
                    Tools.BytesAsString(BytesChecked),
                    HashesRead);

                Console.WriteLine("Of these, {0} files [{2:0.00%}] using {1} [{3:0.00%}] were duplicates.",
                    FilesDetected,
                    Tools.BytesAsString(BytesDetected),
                    ((double)FilesDetected) / ((double)FilesChecked),
                    ((double)BytesDetected) / ((double)BytesChecked));
                
            }
        }

        private void Read(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Warning, {0} doesn't exist...", directory);
                return;
            }

            string[] files = null;
            try
            {
                files = Directory.GetFiles(directory);
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

        private void Found(FileInfo fi, string filename, string existing)
        {
            if (!filename.Equals(existing, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("{0} already exists as {1} [{2} bytes]",
                    filename, existing, fi.Length);

                ++FilesDetected;
                BytesDetected += fi.Length;

                if (DeleteFiles)
                {
                    File.SetAttributes(filename, FileAttributes.Normal);
                    File.Delete(filename);
                }
            }
        }

        private void Check(string filename)
        {
            FileInfo fi = null;
            try
            {
                fi = new FileInfo(filename);
            }
            catch (Exception)
            {
                return;
            }

            long size = fi.Length;
            ++FilesChecked;
            BytesChecked += size;

            if (Cache.ContainsKey(size))
            {
                Dictionary<string, string> temp = Cache[size];
                string hash = null;

                if (temp.ContainsKey("\0"))
                {
                    Debug.Assert(temp.Count == 1);

                    string tempname = temp["\0"];

                    string second_hash = CalculateMD5(tempname, size, temp);
                    temp.Remove("\0");
                    temp.Add(second_hash, tempname);
                }

                // this most definitely needs to be calculated

                hash = CalculateMD5(filename, size, temp);

                if (temp.ContainsKey(hash))
                {
                    Found(fi, filename, temp[hash]);
                }
                else
                {
                    
                    temp[hash] = filename;
                }
            }
            else
            {
                // because the file size is new, by definition cache cannot exist yet
                Dictionary<string, string> temp = new Dictionary<string, string>();
                temp["\0"] = filename; // don't read yet, read only when required
                Cache[size] = temp;
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
                }
                else
                {
                    if (tokens[i] == "..")
                    {
                        Debug.Assert(i > 0);
                        tokens[i] = null;

                        int j = i - 1; 
                        while( (j >= 0) && (tokens[j] == null) )
                        {
                            --j;
                        }
                        if (j >= 0)
                        {
                            tokens[j] = tokens[j].Contains("\\") ? RemoveLastPathPart(tokens[j]) : null;
                        }
                        if( tokens[0] == null )
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
            if( IsUNCPath )
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
            new DetectDuplicates().Run(args);
        }
    }

}
