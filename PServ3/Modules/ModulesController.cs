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
using pserv3.Processes;

namespace pserv3.Modules
{
    public class ModulesController : IServiceController
    {
        private readonly List<IServiceObject> Modules = new List<IServiceObject>();

        public ModulesController(ListView listView)
            :   base(listView)
        {
            Columns.Add(new StandardColumn_Int(
                IDS.Modules_Column_ID,
                (int)ModuleItemTypes.ID));
            Columns.Add(new StandardColumn(
                IDS.Modules_Column_Name,
                (int)ModuleItemTypes.Name));
            Columns.Add(new StandardColumn(
                IDS.Modules_Column_Path,
                (int)ModuleItemTypes.Path));
            Columns.Add(new StandardColumn_Int(
                IDS.Modules_Column_ModuleMemorySize,
                (int)ModuleItemTypes.ModuleMemorySize));
            Columns.Add(new StandardColumn(
                IDS.Modules_Column_FileDescription,
                (int)ModuleItemTypes.FileDescription));
            Columns.Add(new StandardColumn(
                IDS.Modules_Column_FileVersion,
                (int)ModuleItemTypes.FileVersion));
            Columns.Add(new StandardColumn(
                IDS.Modules_Column_Product,
                (int)ModuleItemTypes.Product));
            Columns.Add(new StandardColumn(
                IDS.Modules_Column_ProductVersion,
                (int)ModuleItemTypes.ProductVersion));

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
                    wo.GetText((int)ModuleItemTypes.Handle),
                    wo.GetText((int)ModuleItemTypes.Title));

                result.AppendFormat("  Class: {0}\n",
                    wo.GetText((int)ModuleItemTypes.Class));

                result.AppendFormat("  Style: {0}\n",
                    wo.GetText((int)ModuleItemTypes.Style));
                result.AppendFormat("ExStyle: {0}\n",
                    wo.GetText((int)ModuleItemTypes.ExStyle));

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
        /*
        private void DefineContextMenu(ContextMenu cm, string name, EventHandler eh)
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
         * */

        public override void Refresh()
        {
            Modules.Clear();
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    string username = ProcessObject.GetUserInfo(p);
                    foreach (ProcessModule m in p.Modules)
                    {
                        try
                        {
                            Modules.Add(new ModuleObject(p, m, username));
                        }
                        catch (Exception e)
                        {
                            GSharpTools.Tools.DumpException(e, "EXCEPTION CAUGHT in ModulesController.Refresh(1)");
                        }
                    }
                }
                catch (Exception e)
                {
                    GSharpTools.Tools.DumpException(e, "EXCEPTION CAUGHT in ModulesController.Refresh(2)");
                }
            }
        }

        public override IEnumerable<IServiceObject> GetObjects()
        {
            return Modules;
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
                xtw.WriteStartElement("modules");
                xtw.WriteAttributeString("version", "2.0");
                foreach (ListViewItem item in Control.Items)
                {
                    ModuleObject wo = item.Tag as ModuleObject;
                    xtw.WriteStartElement("module");
                    xtw.WriteAttributeString("id", wo.Objects[(int)ModuleItemTypes.ID].ToString());
                    xtw.WriteElementString("name", wo.Objects[(int)ModuleItemTypes.Name].ToString());
                    xtw.WriteElementString("path", wo.Objects[(int)ModuleItemTypes.Path].ToString());
                    

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
