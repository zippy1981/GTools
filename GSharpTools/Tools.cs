using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Globalization;
using System.Xml;
using System.IO;

namespace GSharpTools
{
    public static class Tools
    {
        public static Dictionary<string, string> ReadStrings(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                directory = Environment.CurrentDirectory;

            Dictionary<string, string> sd = new Dictionary<string, string>();
            string combinedName = Path.Combine(directory, string.Format("strings.{0}.xml", CultureInfo.CurrentUICulture.Name));
            Trace.TraceInformation("Testing if '{0}' exists:", combinedName);
            if (File.Exists(combinedName))
            {
                Trace.TraceInformation("=> It does, read from '{0}'", combinedName);
                ReadStrings(sd, combinedName);
            }
            else
            {
                combinedName = Path.Combine(directory, "strings.xml");
                Trace.TraceInformation("Testing if '{0}' exists:", combinedName);
                if (File.Exists(combinedName))
                {
                    Trace.TraceInformation("=> It does, read from '{0}'", combinedName);
                    ReadStrings(sd, combinedName);
                }
                else
                {
                    Trace.TraceWarning("No valid strings.xml found, using default strings...");
                }
            }
            return sd;
        }

        private static void ReadStrings(Dictionary<string, string> sd, string filename)
        {
            using (XmlTextReader xml = new XmlTextReader(filename))
            {
                while (xml.Read())
                {
                    if (XmlNodeType.Element == xml.NodeType)
                    {
                        if (xml.Name == "string")
                        {
                            string id = xml.GetAttribute("id");
                            string text = xml.GetAttribute("text");
                            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(text))
                            {
                                sd[id] = text.Replace("\\r", "\r").Replace("\\n", "\n").Replace("_", "&").Replace("&lt;", "<").Replace("&gt;", ">");
                            }
                        }
                    }
                }
            }
        }

        public static string BytesAsString(long bytes)
        {
            if (bytes < 1024)
                return string.Format("{0} Bytes", bytes);

            bytes /= 1024;
            if (bytes < 1024)
                return string.Format("{0} KB", bytes);

            double baits = (double)bytes;
            baits /= 1024;
            if (baits < 1024)
                return string.Format("{0:0.00} MB", baits);

            baits /= 1024;
            return string.Format("{0:0.00} GB", baits);
        }

        private static string BackupName(string Filename, int index)
        {
            if (index > 0)
                return string.Format("{0}.{1}", Filename, index);
            return Filename;
        }

        public static string DumpObject(object o)
        {
            if (o == null)
                return "null [null]";

            return string.Format("{0} [{1}]", o.ToString(), o.GetType());
        }

        public static bool DeleteFile(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    File.SetAttributes(filename, FileAttributes.Normal);
                    File.Delete(filename);
                }
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Error while deleting '{0}': {1}", filename, e.Message));
                return false;
            }
        }

        public static void BackupOldFiles(string Filename, int nCount)
        {
            if (nCount > 0)
            {
                string newName, oldName = BackupName(Filename, nCount);
                DeleteFile(oldName);
                for (int index = nCount - 1; index >= 0; --index)
                {
                    newName = BackupName(Filename, index);
                    if (File.Exists(newName))
                    {
                        try
                        {
                            File.Move(newName, oldName);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    oldName = newName;
                }
            }
        }

        public static Dictionary<TValue, TKey> ReverseDict<TKey, TValue>(Dictionary<TKey, TValue> source)
        {
            Dictionary<TValue, TKey> target = new Dictionary<TValue, TKey>();
            foreach (TKey key in source.Keys)
            {
                target[source[key]] = key;
            }
            return target;
        }

        public static void WriteAt<T>(T[] target, int writepos, T[] source)
        {
            for (int index = 0; index < source.Length && writepos < target.Length; ++index)
            {
                target[writepos++] = source[index];
            }
        }

        public static T[] CreateCopy<T>(T[] source) where T : new()
        {
            T[] target = new T[source.Length];
            WriteAt<T>(target, 0, source);
            return target;
        }

        public static string Join(IEnumerable<string> elements, string joiner)
        {
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (string item in elements)
            {
                if (first)
                    first = false;
                else
                    result.Append(joiner);
                result.Append(item);
            }
            return result.ToString();
        }
        
        public static string DumpException(Exception e, string format, params object[] args)
        {
            StringBuilder output = new StringBuilder();
            output.AppendFormat("--- EXCEPTION CAUGHT: {0} ---\r\n", e.Message);
            output.AppendFormat("CONTEXT: {0}\r\n", string.Format(format, args));
            output.AppendFormat("SOURCE: {0}\r\n", e.Source);
            output.AppendLine(e.StackTrace);
            string text = output.ToString();
            Trace.TraceError(text);
            return text;
        }

        public static void DumpEnvironment(string context)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("VARIABLES IN CONTEXT {0}:\r\n", context);

            IDictionary envvars = Environment.GetEnvironmentVariables();
            foreach (string name in envvars.Keys)
            {
                result.AppendFormat("{0,30} = {1}\r\n", name, envvars[name]);
            }

            Trace.TraceInformation(result.ToString());
            result = new StringBuilder();
            result.AppendFormat("ENVIRONMENT IN CONTEXT {0}:\r\n", context);

            result.AppendFormat("{0,30} = {1}\r\n", "CommandLine", Environment.CommandLine);
            result.AppendFormat("{0,30} = {1}\r\n", "CurrentDirectory", Environment.CurrentDirectory);
            result.AppendFormat("{0,30} = {1}\r\n", "MachineName", Environment.MachineName);
            result.AppendFormat("{0,30} = {1}\r\n", "OSVersion", Environment.OSVersion);
            result.AppendFormat("{0,30} = {1}\r\n", "ProcessorCount", Environment.ProcessorCount);
            result.AppendFormat("{0,30} = {1}\r\n", "SystemDirectory", Environment.SystemDirectory);
            result.AppendFormat("{0,30} = {1}\r\n", "UserDomainName", Environment.UserDomainName);
            result.AppendFormat("{0,30} = {1}\r\n", "UserInteractive", Environment.UserInteractive);
            result.AppendFormat("{0,30} = {1}\r\n", "UserName", Environment.UserName);
            result.AppendFormat("{0,30} = {1}\r\n", "Version", Environment.Version);
            result.AppendFormat("{0,30} = {1}\r\n", "UserDomainName", Environment.UserDomainName);

            Trace.TraceInformation(result.ToString());
        }
    }
}
