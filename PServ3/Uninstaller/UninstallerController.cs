using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Xml;
using Microsoft.Win32;

namespace pserv3.Uninstaller
{
    public class UninstallerController : IServiceController
    {
        private readonly List<IServiceObject> Items = new List<IServiceObject>();

        public UninstallerController(ListView listView)
            : base(listView)
        {
            Columns.Add(new StandardColumn(
                IDS.Uninstaller_Column_Application,
                (int)UninstallerItemTypes.Application));
            Columns.Add(new StandardColumn(
                IDS.Uninstaller_Column_Path,
                (int)UninstallerItemTypes.Path));
            Columns.Add(new StandardColumn(
                IDS.Uninstaller_Column_Key,
                (int)UninstallerItemTypes.Key));
            Columns.Add(new StandardColumn(
                IDS.Uninstaller_Column_Version,
                (int)UninstallerItemTypes.Version));
            Columns.Add(new StandardColumn(
                IDS.Uninstaller_Column_Publisher,
                (int)UninstallerItemTypes.Publisher));
            Columns.Add(new StandardColumn(
                IDS.Uninstaller_Column_HelpLink,
                (int)UninstallerItemTypes.HelpLink));
            Columns.Add(new StandardColumn(
                IDS.Uninstaller_Column_AboutLink,
                (int)UninstallerItemTypes.AboutLink));
            Columns.Add(new StandardColumn(
                IDS.Uninstaller_Column_Action,
                (int)UninstallerItemTypes.Action));

        }

        public override string CreatePrintDocument()
        {
            return "";
        }

        private IEnumerable<UninstallerObject> SelectedUninstallers
        {
            get
            {
                foreach (int selectedIndex in Control.SelectedIndices)
                {
                    yield return MainForm.Instance.CurrentObjects[selectedIndex] as UninstallerObject;
                }
            }
        }

        public override void OnShowProperties()
        {
            foreach (UninstallerObject uo in SelectedUninstallers)
            {
                UninstallerProperties dialog = new UninstallerProperties(uo, this);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    
                }
            }
            ServiceView.UpdateDisplay();
        }

        #region IServiceController Members

        public override string GetCaption()
        {
            return "Uninstaller";
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
            DefineContextMenu(cm, "Properties", (x, y) => OnShowProperties());
            return cm;
            */
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
            Items.Clear();
            RefreshEntries(Registry.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            RefreshEntries(Registry.CurrentUser, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
        }

        private void RefreshEntries(RegistryKey rootKey, string keyName)
        {
            using(RegistryKey hkKey = rootKey.OpenSubKey(keyName,false))
            {
                foreach (string subKeyName in hkKey.GetSubKeyNames())
                {
                    UninstallerObject uo = new UninstallerObject(rootKey, string.Format("{0}\\{1}", keyName, subKeyName), subKeyName);
                    if( uo.IsValid )
                        Items.Add(uo);
                }
            }
        }

        public override IEnumerable<IServiceObject> GetObjects()
        {
            return Items;
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
                xtw.WriteStartElement("uninstaller");
                xtw.WriteAttributeString("version", "2.0");
                foreach (ListViewItem item in Control.Items)
                {
                    UninstallerObject wo = item.Tag as UninstallerObject;
                    xtw.WriteStartElement("uninstall");
                    xtw.WriteAttributeString("id", wo.GetText((int)UninstallerItemTypes.Key));

                    xtw.WriteElementString("name", wo.GetText((int)UninstallerItemTypes.Application));
                    xtw.WriteElementString("path", wo.GetText((int)UninstallerItemTypes.Path));
                    xtw.WriteElementString("version", wo.GetText((int)UninstallerItemTypes.Version));
                    xtw.WriteElementString("publisher", wo.GetText((int)UninstallerItemTypes.Publisher));
                    xtw.WriteElementString("help", wo.GetText((int)UninstallerItemTypes.HelpLink));
                    xtw.WriteElementString("about", wo.GetText((int)UninstallerItemTypes.AboutLink));

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
