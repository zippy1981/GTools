using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Xml;

namespace pserv3.Processes
{
    public class ProcessesController : IServiceController
    {
        private readonly List<IServiceObject> Processes = new List<IServiceObject>();

        public ProcessesController(ListView listView)
            :   base(listView)
        {
            Columns.Add(new StandardColumn_Int(
                IDS.Processes_Column_ID,
                (int)ProcessItemTypes.ID));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_Name,
                (int)ProcessItemTypes.Name));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_Path,
                (int)ProcessItemTypes.Path));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_User,
                (int)ProcessItemTypes.User));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_FileDescription,
                (int)ProcessItemTypes.FileDescription));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_FileVersion,
                (int)ProcessItemTypes.FileVersion));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_Product,
                (int)ProcessItemTypes.Product));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_ProductVersion,
                (int)ProcessItemTypes.ProductVersion));

            Columns.Add(new StandardColumn_Int(
                IDS.Processes_Column_ThreadCount,
                (int)ProcessItemTypes.ThreadCount));
            Columns.Add(new StandardColumn_Int(
                IDS.Processes_Column_HandleCount,
                (int)ProcessItemTypes.HandleCount));
            Columns.Add(new StandardColumn_Int(
                IDS.Processes_Column_MainWindowHandle,
                (int)ProcessItemTypes.MainWindowHandle));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_MainWindowTitle,
                (int)ProcessItemTypes.MainWindowTitle));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_Responding,
                (int)ProcessItemTypes.Responding));
            Columns.Add(new StandardColumn_TimeSpan(
                IDS.Processes_Column_StartTime,
                (int)ProcessItemTypes.StartTime));
            Columns.Add(new StandardColumn_TimeSpan(
                IDS.Processes_Column_TotalRunTime,
                (int)ProcessItemTypes.TotalRunTime));
            Columns.Add(new StandardColumn_TimeSpan(
                IDS.Processes_Column_TotalProcessorTime,
                (int)ProcessItemTypes.TotalProcessorTime));
            Columns.Add(new StandardColumn_TimeSpan(
                IDS.Processes_Column_PrivilegedProcessorTime,
                (int)ProcessItemTypes.PrivilegedProcessorTime));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_ProcessPriorityClass,
                (int)ProcessItemTypes.ProcessPriorityClass));
            Columns.Add(new StandardColumn_Int(
                IDS.Processes_Column_SessionId,
                (int)ProcessItemTypes.SessionId));
            Columns.Add(new StandardColumn(
                IDS.Processes_Column_StartInfoArguments,
                (int)ProcessItemTypes.StartInfoArguments));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_NonpagedSystemMemorySize64,
                (int)ProcessItemTypes.NonpagedSystemMemorySize64));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_PagedMemorySize64,
                (int)ProcessItemTypes.PagedMemorySize64));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_PagedSystemMemorySize64,
                (int)ProcessItemTypes.PagedSystemMemorySize64));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_PeakPagedMemorySize64,
                (int)ProcessItemTypes.PeakPagedMemorySize64));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_PeakVirtualMemorySize64,
                (int)ProcessItemTypes.PeakVirtualMemorySize64));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_PeakWorkingSet64,
                (int)ProcessItemTypes.PeakWorkingSet64));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_PrivateMemorySize64,
                (int)ProcessItemTypes.PrivateMemorySize64));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_VirtualMemorySize64,
                (int)ProcessItemTypes.VirtualMemorySize64));
            Columns.Add(new StandardColumn_Long(
                IDS.Processes_Column_WorkingSet64,
                (int)ProcessItemTypes.WorkingSet64));
        }

        #region IServiceController Members

        public override string CreatePrintDocument()
        {
            StringBuilder result = new StringBuilder();
            /*
            int index = 0;
            foreach (ListViewItem item in Control.Items)
            {
                ++index;
                ProcessObject wo = item.Tag as ProcessObject;

                result.AppendFormat("{0:000}/{1:000}: {2} \"{3}\"\r\n", index, Control.Items.Count,
                    wo.GetText((int)ProcessItemTypes.Handle),
                    wo.GetText((int)ProcessItemTypes.Title));

                result.AppendFormat("  Class: {0}\n",
                    wo.GetText((int)ProcessItemTypes.Class));

                result.AppendFormat("  Style: {0}\n",
                    wo.GetText((int)ProcessItemTypes.Style));
                result.AppendFormat("ExStyle: {0}\n",
                    wo.GetText((int)ProcessItemTypes.ExStyle));

                result.AppendLine();

            }*/
            return result.ToString();
        }

        public override string GetCaption()
        {
            return string.Format("Processes on \\{0} (this machine)", Environment.MachineName);
        }

        public override ContextMenu CreateContextMenu()
        {
            return null;
            /*
            ContextMenu cm = new ContextMenu();
            cm.Popup += cm_Popup;
            DefineContextMenu(cm, "Show", (x, y) => OnContextStart());
            DefineContextMenu(cm, "Hide", (x, y) => OnContextStop());
            DefineContextMenu(cm, "Restore", (x, y) => OnContextRestart());
            DefineContextMenu(cm, "Minimize", (x, y) => OnContextPause());
            DefineContextMenu(cm, "Maximize", (x, y) => OnContextContinue());
            cm.MenuItems.Add("-");
            //DefineContextMenu(cm, "Startup: Automatically", (x, y) => OnApplyStartupChanges(SC_START_TYPE.SERVICE_AUTO_START));
            //DefineContextMenu(cm, "Startup: Manually", (x, y) => OnApplyStartupChanges(SC_START_TYPE.SERVICE_DEMAND_START));
            //DefineContextMenu(cm, "Startup: Disabled", (x, y) => OnApplyStartupChanges(SC_START_TYPE.SERVICE_DISABLED));
            //cm.MenuItems.Add("-");
            DefineContextMenu(cm, "Properties", (x, y) => OnShowProperties());            
            return cm;
             * */
        }

        /*private void DefineContextMenu(ContextMenu cm, string name, EventHandler eh)
        {
            MenuItem mi = cm.MenuItems.Add(name);
            mi.Click += eh;
        }

        void cm_Popup(object sender, EventArgs e)
        {
            ContextMenu cm = sender as ContextMenu;
           
            // disable start / stop functions
            cm.MenuItems[0].Enabled = IsContextStartEnabled();
            cm.MenuItems[1].Enabled = IsContextStopEnabled();
            cm.MenuItems[2].Enabled = IsContextRestartEnabled();
            cm.MenuItems[3].Enabled = IsContextPauseEnabled();
            cm.MenuItems[4].Enabled = IsContextContinueEnabled();
           
        }
         */

        public override void Refresh()
        {
            Processes.Clear();
            foreach (Process p in Process.GetProcesses())
            {
                Processes.Add(new ProcessObject(p));
            }
        }

        public override IEnumerable<IServiceObject> GetObjects()
        {
            return Processes;
        }

        public override void SaveAsXML(string filename)
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
                xtw.WriteStartElement("processes");
                xtw.WriteAttributeString("version", "2.0");
                foreach (ListViewItem item in Control.Items)
                {
                    ProcessObject wo = item.Tag as ProcessObject;
                    xtw.WriteStartElement("process");
                    xtw.WriteAttributeString("id", wo.GetText((int)ProcessItemTypes.ID));
                    xtw.WriteElementString("name", wo.GetText((int)ProcessItemTypes.Name));
                    xtw.WriteElementString("path", wo.GetText((int)ProcessItemTypes.Path));
                    xtw.WriteElementString("user", wo.GetText((int)ProcessItemTypes.User));

                    xtw.WriteEndElement();
                }
                xtw.WriteEndElement();
                xtw.Close();
            }
            if (filename == null)
            {
                Clipboard.SetText(buffer.ToString());
            }
        }

        #endregion
    }
}
