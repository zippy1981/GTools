using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Xml;

namespace pserv3.Windows
{
    public class WindowsController : IServiceController
    {
        private readonly List<IServiceObject> Windows = new List<IServiceObject>();

        public WindowsController(ListView listView)
            :   base(listView)
        {
            Columns.Add(new StandardColumn(
                IDS.Windows_Column_Handle,
                (int)WindowItemTypes.Handle));
            Columns.Add(new StandardColumn(
                IDS.Windows_Column_Title,
                (int)WindowItemTypes.Title));
            Columns.Add(new StandardColumn(
                IDS.Windows_Column_Class,
                (int)WindowItemTypes.Class));
            Columns.Add(new StandardColumn(
                IDS.Windows_Column_Style,
                (int)WindowItemTypes.Style));
            Columns.Add(new StandardColumn_UInt(
                IDS.Windows_Column_ExStyle,
                (int)WindowItemTypes.ExStyle));
            Columns.Add(new StandardColumn_UInt(
                IDS.Windows_Column_ID,
                (int)WindowItemTypes.ID));
            Columns.Add(new StandardColumn(
                IDS.Windows_Column_Size,
                (int)WindowItemTypes.Size));
            Columns.Add(new StandardColumn(
                IDS.Windows_Column_Position,
                (int)WindowItemTypes.Position));
            Columns.Add(new StandardColumn_UInt(
                IDS.Windows_Column_PID,
                (int)WindowItemTypes.PID));
            Columns.Add(new StandardColumn_UInt(
                IDS.Windows_Column_TID,
                (int)WindowItemTypes.TID));
            Columns.Add(new StandardColumn(
                IDS.Windows_Column_Process,
                (int)WindowItemTypes.Process));
        
        }
        
        #region IServiceController Members

        public override string CreatePrintDocument()
        {
            StringBuilder result = new StringBuilder();

            int index = 0;
            foreach (ListViewItem item in Control.Items)
            {
                ++index;
                WindowObject wo = item.Tag as WindowObject;

                result.AppendFormat("{0:000}/{1:000}: {2} \"{3}\"\r\n", index, Control.Items.Count,
                    wo.GetText((int)WindowItemTypes.Handle),
                    wo.GetText((int)WindowItemTypes.Title));

                result.AppendFormat("  Class: {0}\n",
                    wo.GetText((int)WindowItemTypes.Class));

                result.AppendFormat("  Style: {0}\n",
                    wo.GetText((int)WindowItemTypes.Style));
                result.AppendFormat("ExStyle: {0}\n",
                    wo.GetText((int)WindowItemTypes.ExStyle));

                result.AppendLine();

            }
            return result.ToString();
        }

        public override string GetCaption()
        {
            return string.Format("Windows on \\{0} (this machine)", Environment.MachineName);
        }

        public override ContextMenu CreateContextMenu()
        {
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
        }

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

        private delegate bool ENUM_WINDOWS_PROC(int hwnd, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int EnumWindows(ENUM_WINDOWS_PROC callPtr, int lParam);
        private const int GWL_WNDPROC = -4;
        private const int GWL_HINSTANCE = -6;
        private const int GWL_HWNDPARENT = -8;
        private const int GWL_USERDATA = -21;

        private const int SW_RESTORE = 9;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(int hWnd, int state);

        private bool EnumWindowsProc(int hwnd, int lParam)
        {
            WindowObject wo = new WindowObject(hwnd);
            if (!string.IsNullOrEmpty(wo.Title))
            {
                Windows.Add(wo);
            }
            return true;
        }

        public override void Refresh()
        {
            Windows.Clear();
            EnumWindows(EnumWindowsProc, 0);
        }

        public override IEnumerable<IServiceObject> GetObjects()
        {
            return Windows;
        }

        public override void OnContextRestart()
        {
            OnShowWindow(SW_RESTORE);
        }

        public override void OnContextStart()
        {
            OnShowWindow(SW_SHOW);
        }

        public override void OnContextStop()
        {
            OnShowWindow(SW_HIDE);
        }

        public override void OnContextPause()
        {
            OnShowWindow(SW_MINIMIZE);
        }

        public override void OnContextContinue()
        {
            OnShowWindow(SW_MAXIMIZE);
        }

        private IEnumerable<WindowObject> SelectedWindows
        {
            get
            {
                foreach (int selectedIndex in Control.SelectedIndices)
                {
                    yield return MainForm.Instance.CurrentObjects[selectedIndex] as WindowObject;
                }
            }
        }

        private void OnShowWindow(int state)
        {
            foreach (WindowObject wo in SelectedWindows)
            {
                ShowWindow(wo.Handle, state);
                wo.Refresh();
            }
            ServiceView.UpdateDisplay();
        }


        public override bool IsContextRestartEnabled()
        {
            return true;
        }

        public override bool IsContextStartEnabled()
        {
            return true;
        }

        public override bool IsContextStopEnabled()
        {
            return true;
        }

        public override bool IsContextPauseEnabled()
        {
            return true;
        }

        public override bool IsContextContinueEnabled()
        {
            return true;
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
                xtw.WriteStartElement("windows");
                xtw.WriteAttributeString("version", "2.0");
                foreach (ListViewItem item in Control.Items)
                {
                    WindowObject wo = item.Tag as WindowObject;
                    xtw.WriteStartElement("window");
                    xtw.WriteAttributeString("id", wo.GetText((int)WindowItemTypes.Handle));

                    xtw.WriteElementString("name", wo.GetText((int)WindowItemTypes.Title));
                    xtw.WriteElementString("class", wo.GetText((int)WindowItemTypes.Class));
                    xtw.WriteElementString("size", wo.GetText((int)WindowItemTypes.Size));
                    xtw.WriteElementString("position", wo.GetText((int)WindowItemTypes.Position));
                    xtw.WriteElementString("id", wo.GetText((int)WindowItemTypes.ID));
                    xtw.WriteElementString("style", wo.GetText((int)WindowItemTypes.Style));
                    xtw.WriteElementString("exstyle", wo.GetText((int)WindowItemTypes.ExStyle));

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
