using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace DLLUsage
{
    enum DisplayMode 
    {
        ListByDLLName,
        ListByDLLPath,
        ListByProcessName,
        ListByProcessPath
    }

    class ProcessList
    {
        public DisplayMode CurrentDisplayMode = DisplayMode.ListByDLLName;
        private readonly List<ProcessInfo> Items = new List<ProcessInfo>();
        private String FilterString = "";
        private readonly TreeView Control;

        public ProcessList(TreeView control)
        {
            Control = control;
            Refresh();
        }

        public void SaveAsXML(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            settings.IndentChars = "\t";
            settings.NewLineChars = "\r\n";

            StringBuilder buffer = new StringBuilder();
            using (XmlWriter xtw = (filename == null) ? XmlWriter.Create(buffer, settings) : XmlWriter.Create(filename, settings))
            {
                xtw.WriteStartDocument();

                if (IsProcessList)
                {
                    xtw.WriteStartElement("processes");
                    xtw.WriteAttributeString("version", "2.0");
                    foreach (ProcessInfo pi in Items)
                    {
                        xtw.WriteStartElement("process");
                        xtw.WriteAttributeString("name", pi.FileName);
                        xtw.WriteAttributeString("path", pi.Directory);
                        foreach (string module in pi.Dependencies)
                        {
                            xtw.WriteStartElement("module");
                            xtw.WriteAttributeString("name", module);
                            xtw.WriteEndElement();
                        }
                        xtw.WriteEndElement();
                    }
                }
                else
                {
                    xtw.WriteStartElement("modules");
                    xtw.WriteAttributeString("version", "2.0");

                    foreach (ProcessInfo pi in Items)
                    {
                        xtw.WriteStartElement("module");
                        xtw.WriteAttributeString("name", pi.FileName);
                        xtw.WriteAttributeString("path", pi.Directory);

                        foreach (string process in pi.Dependencies)
                        {
                            xtw.WriteStartElement("process");
                            xtw.WriteAttributeString("name", process);
                            xtw.WriteEndElement();
                        }
                        xtw.WriteEndElement();
                    }
                }

                xtw.WriteEndElement();
                xtw.Close();
            }
            

        }

        private static string GetSafeProcessName(Process p)
        {
            string result = p.ProcessName;
            try
            {
                if (p.Id >= 10)
                    result = p.MainModule.FileName;
            }
            catch
            {

            }
            if (result.StartsWith("\\??\\"))
                result = result.Substring(4);
            return result;
        }

        private static string GetSafeModuleName(ProcessModule m)
        {
            try
            {
                return m.FileName;
            }
            catch
            {

            }
            return m.ToString();
        }

        public bool IsProcessList
        {
            get
            {
                return (CurrentDisplayMode == DisplayMode.ListByProcessName) || (CurrentDisplayMode == DisplayMode.ListByProcessPath);
            }
        }

        public void Refresh()
        {
            if (IsProcessList)
            {
                RefreshByProcess();
            }
            else
            {
                RefreshByModule();
            }
            InsertNodes();
        }

        public void ApplyFilter(string filter)
        {
            FilterString = filter;
            InsertNodes();
        }

        private bool IsFiltered(string text)
        {
            if (FilterString.Equals(""))
                return false;

            return text.ToLower().IndexOf(FilterString) < 0;
        }
        public void SwitchTo(DisplayMode dm)
        {
            CurrentDisplayMode = dm;
            FilterString = "";
            Refresh();
        }

        private void InsertNodes()
        {
            Items.Sort();
            Control.Nodes.Clear();
            foreach (ProcessInfo pi in Items)
            {
                if (!IsFiltered(pi.PathName))
                {
                    TreeNode node = Control.Nodes.Add(pi.DisplayName);
                    node.Tag = pi;
                    pi.Dependencies.Sort();
                    foreach (string module in pi.Dependencies)
                    {
                        TreeNode item = node.Nodes.Add(module);
                        item.Tag = module;                        
                    }
                }
            }
        }

        private void RefreshByModule()
        {
            Dictionary<string, ProcessInfo> dict = new Dictionary<string, ProcessInfo>(StringComparer.CurrentCultureIgnoreCase);
            foreach(Process p in Process.GetProcesses())
            {
                string procname = GetSafeProcessName(p);

                ProcessModuleCollection pmc = null;
                try
                {
                     pmc = p.Modules;
                }
                catch
                {
                    continue;                
                }
                if (pmc != null)
                {
                    string FormattedProcessName = string.Format("{0} [{1}]", procname, p.Id);
                    foreach (ProcessModule m in pmc)
                    {
                        string key = GetSafeModuleName(m);
                        ProcessInfo pi = null;
                        try
                        {
                            if (dict.ContainsKey(key))
                            {
                                pi = dict[key];
                            }
                        }
                        catch
                        {
                            
                        }
                        if (pi == null)
                        {
                            pi = new ProcessInfo(key, CurrentDisplayMode);
                            dict[key] = pi;
                        }
                        pi.Dependencies.Add(FormattedProcessName);
                    }
                }
            }

            Items.Clear();
            foreach (ProcessInfo pi in dict.Values)
                Items.Add(pi);
        }

        private void RefreshByProcess()
        {
            Items.Clear();

            foreach(Process p in Process.GetProcesses())
            {
                ProcessInfo pi = new ProcessInfo(GetSafeProcessName(p), p.Id, CurrentDisplayMode);
                Items.Add(pi);
                ProcessModuleCollection pmc = null;
                try
                {
                     pmc = p.Modules;
                }
                catch
                {
                    continue;
                }
                foreach (ProcessModule m in pmc)
                {
                    pi.Dependencies.Add(GetSafeModuleName(m));
                }
            }
        }
    }
}
