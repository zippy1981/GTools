using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace pserv3.Services
{
    public class ServicesController : IServiceController
    {
        private readonly List<IServiceObject> Services = new List<IServiceObject>();
        private readonly NativeServiceFunctions NativeServiceFunctions = new NativeServiceFunctions();
        protected SC_SERVICE_TYPE ServicesType;
        public static string MachineName = null;

        public ServicesController(ListView listView)
            :   base(listView)
        {
            MachineName = null; // localhost
            ServicesType = SC_SERVICE_TYPE.SERVICE_WIN32;

            Columns.Add(new StandardColumn(
                IDS.Services_Column_DisplayName,
                (int)ServiceItemTypes.DisplayName));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_ServiceName,
                (int)ServiceItemTypes.ServiceName));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_Status,
                (int)ServiceItemTypes.Status));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_Start,
                (int)ServiceItemTypes.Start));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_Type,
                (int)ServiceItemTypes.Type));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_Path,
                (int)ServiceItemTypes.Path));
            Columns.Add(new StandardColumn_Int(
                IDS.Services_Column_PID,
                (int)ServiceItemTypes.PID));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_User,
                (int)ServiceItemTypes.User));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_LoadOrderGroup,
                (int)ServiceItemTypes.LoadOrderGroup));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_ErrorControl,
                (int)ServiceItemTypes.ErrorControl));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_TagId,
                (int)ServiceItemTypes.TagId));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_Dependencies,
                (int)ServiceItemTypes.Dependencies));
            Columns.Add(new StandardColumn(
                IDS.Services_Column_Description,
                (int)ServiceItemTypes.Description));
        }

        #region IServiceController Members

        public override bool CanConnectToRemoteMachine()
        {
            return true;
        }

        public override string GetCaption()
        {
            if( string.IsNullOrEmpty(MachineName) || MachineName.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("Services on \\\\{0} (this machine)", Environment.MachineName);
            }
            else
            {
                return string.Format("Services on \\\\{0}", MachineName);
            }
        }

        public List<string> GetLoadOrderGroups()
        {
            Dictionary<string, bool> temp = new Dictionary<string, bool>();
            foreach (ListViewItem item in Control.Items)
            {
                temp[(item.Tag as ServiceObject).LoadOrderGroup] = true;
            }
            List<string> result = new List<string>();
            foreach (string key in temp.Keys)
                result.Add(key);
            result.Sort();
            return result;
        }

        public override void OnShowProperties()
        {
            foreach (ServiceObject so in SelectedServices)
            {
                ServiceProperties dialog = new ServiceProperties(so, this);
                if( dialog.ShowDialog() == DialogResult.OK )
                {
                    so.ApplyChanges();
                }
            }
            ServiceView.UpdateDisplay();
        }

        public void OnUninstall()
        {
            bool refreshRequired = false;
            foreach (ServiceObject so in SelectedServices)
            {
                if (MessageBox.Show(
                        string.Format("Are you sure you want to remove the service \"{0}\"? Note: this will not uninstall the application, it will just not run as service any longer...", so.DisplayName),
                        "Question", 
                        MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    so.Uninstall();
                    refreshRequired = true;
                }
            }
            if (refreshRequired)
            {
                ServiceView.RefreshDisplay();
            }
        }

        public override ContextMenu CreateContextMenu()
        {
            ContextMenu cm = new ContextMenu();
            cm.Popup += cm_Popup;
            DefineContextMenu(cm, "Start", (x, y) => OnContextStart());
            DefineContextMenu(cm, "Stop", (x, y) => OnContextStop());
            DefineContextMenu(cm, "Restart", (x,y) => OnContextRestart());
            DefineContextMenu(cm, "Pause", (x, y) => OnContextPause());
            DefineContextMenu(cm, "Continue", (x, y) => OnContextContinue());
            cm.MenuItems.Add("-");
            DefineContextMenu(cm, "Startup: Automatically", (x, y) => OnApplyStartupChanges(SC_START_TYPE.SERVICE_AUTO_START));
            DefineContextMenu(cm, "Startup: Manually", (x, y) => OnApplyStartupChanges(SC_START_TYPE.SERVICE_DEMAND_START));
            DefineContextMenu(cm, "Startup: Disabled", (x, y) => OnApplyStartupChanges(SC_START_TYPE.SERVICE_DISABLED));
            cm.MenuItems.Add("-");
            DefineContextMenu(cm, "Uninstall", (x, y) => OnUninstall());
            cm.MenuItems.Add("-");
            DefineContextMenu(cm, "Properties", (x, y) => OnShowProperties());
            return cm;
        }

        private void OnApplyStartupChanges(SC_START_TYPE ExpectedStartType)
        {
            foreach (ServiceObject so in SelectedServices)
            {
                if (so.StartType != ExpectedStartType)
                {
                    so.StartType = ExpectedStartType;
                    so.ApplyChanges();
                }                
            }
            ServiceView.UpdateDisplay();
        }
        
        private void DefineContextMenu(ContextMenu cm, string name, EventHandler eh)
        {
            MenuItem mi = cm.MenuItems.Add(name);
            mi.Click += eh;
        }

        void context_ChangeServiceStatus(ServiceStateRequest ssr)
        {
            new ChangeServiceStatus(ServiceView, SelectedServices, ssr).ShowDialog();

            foreach (ServiceObject so in Services)
            {
                //ServiceView.Update(so);
            }
            ServiceView.UpdateDisplay();
        }

        public override void OnContextRestart()
        {
            context_ChangeServiceStatus(new RequestServiceRestart());
        }

        public override void OnContextStart()
        {
            context_ChangeServiceStatus(new RequestServiceStartup());
        }

        public override void OnContextStop()
        {
            context_ChangeServiceStatus(new RequestServiceShutdown());
        }

        public override void OnContextPause()
        {
            context_ChangeServiceStatus(new RequestServicePause());
        }

        public override void OnContextContinue()
        {
            context_ChangeServiceStatus(new RequestServiceContinue());
        }

        public override bool IsContextRestartEnabled()
        {
            foreach (ServiceObject so in SelectedServices)
            {
                if ((so.CurrentState == SC_RUNTIME_STATUS.SERVICE_RUNNING) ||
                    (so.CurrentState == SC_RUNTIME_STATUS.SERVICE_PAUSED))
                {
                    if ((so.ControlsAccepted & SC_CONTROLS_ACCEPTED.SERVICE_ACCEPT_STOP) != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private IEnumerable<ServiceObject> SelectedServices
        {
            get
            {
                foreach (int selectedIndex in Control.SelectedIndices)
                {
                    yield return MainForm.Instance.CurrentObjects[selectedIndex] as ServiceObject;
                }
            }
        }

        public override bool IsContextStartEnabled()
        {
            foreach(ServiceObject so in SelectedServices)
            {
                if (so.CurrentState == SC_RUNTIME_STATUS.SERVICE_STOPPED)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool IsContextStopEnabled()
        {
            foreach (ServiceObject so in SelectedServices)
            {
                if ((so.CurrentState == SC_RUNTIME_STATUS.SERVICE_RUNNING) ||
                    (so.CurrentState == SC_RUNTIME_STATUS.SERVICE_PAUSED))
                {
                    if ((so.ControlsAccepted & SC_CONTROLS_ACCEPTED.SERVICE_ACCEPT_STOP) != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool IsContextPauseEnabled()
        {
            foreach (ServiceObject so in SelectedServices)
            {
                if (so.CurrentState == SC_RUNTIME_STATUS.SERVICE_RUNNING)
                {
                    if ((so.ControlsAccepted & SC_CONTROLS_ACCEPTED.SERVICE_ACCEPT_PAUSE_CONTINUE) != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool IsContextContinueEnabled()
        {
            foreach (ServiceObject so in SelectedServices)
            {
                if (so.CurrentState == SC_RUNTIME_STATUS.SERVICE_PAUSED)
                {
                    if ((so.ControlsAccepted & SC_CONTROLS_ACCEPTED.SERVICE_ACCEPT_PAUSE_CONTINUE) != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
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

        public override void Refresh()
        {
            Services.Clear();

            // list all local services:
            NativeServiceFunctions.Refresh(Services, ServicesType);
        }

        public override IEnumerable<IServiceObject> GetObjects()
        {
            return Services;
        }

        private enum RecordingTag
        {
            None,
            Id,
            StartType,
            Name
        };

        public override void ApplyTemplate(string filename)
        {
            Dictionary<string, ServiceActionTemplate> templates = new Dictionary<string, ServiceActionTemplate>();

            using (XmlReader reader = XmlReader.Create(filename))
            {
                RecordingTag CurrentRecordingTag = RecordingTag.None;
                ServiceActionTemplate sat = null;

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                    case XmlNodeType.Element:
                        if( reader.Name == "service" )
                        {
                            sat = new ServiceActionTemplate(reader.GetAttribute("id").ToLower());
                            templates[sat.ID] = sat;
                        }
                        else if (reader.Name == "name")
                        {
                            CurrentRecordingTag = RecordingTag.Name;
                        }
                        else if (reader.Name == "start")
                        {
                            CurrentRecordingTag = RecordingTag.StartType;
                        }
                        break;                
                    case XmlNodeType.Text:
                        if (CurrentRecordingTag == RecordingTag.Name)
                        {
                            sat.Name = reader.Value;
                            CurrentRecordingTag = RecordingTag.None;
                        }
                        else if (CurrentRecordingTag == RecordingTag.StartType)
                        {
                            sat.StartType = (SC_START_TYPE) Enum.Parse(typeof(SC_START_TYPE), reader.Value);
                            CurrentRecordingTag = RecordingTag.None;
                        }
                        break;
                    }
                }
            }

            ApplyTemplate dialog = new ApplyTemplate(Services, templates, ServiceView);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // apply each and every individual service action
            }
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
                xtw.WriteStartElement("services");
                xtw.WriteAttributeString("version", "2.0");
                foreach (ListViewItem item in Control.Items)
                {
                    ServiceObject so = item.Tag as ServiceObject;
                    xtw.WriteStartElement("service");
                    xtw.WriteAttributeString("id", so.ServiceName);

                    xtw.WriteElementString("name", so.DisplayName);
                    xtw.WriteElementString("status", so.CurrentState.ToString());
                    xtw.WriteElementString("start", so.StartType.ToString());
                    xtw.WriteElementString("type", so.ServiceType.ToString());
                    xtw.WriteElementString("path", so.ImagePath);
                    xtw.WriteElementString("pid", so.GetText((int)ServiceItemTypes.PID));
                    xtw.WriteElementString("description", so.Description);
                    xtw.WriteElementString("error-control", so.ErrorControl.ToString());

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

        public override string CreatePrintDocument()
        {
            StringBuilder result = new StringBuilder();

            int index = 0;
            foreach (ListViewItem item in Control.Items)
            {
                ++index;
                ServiceObject so = item.Tag as ServiceObject;

                result.AppendFormat("{0:000}/{1:000}: {2}\r\n", index, Control.Items.Count, so.ServiceName);
                result.AppendFormat("Display: {0}\n", so.DisplayName);
                result.AppendFormat("   Type: {0}\n", so.ServiceType);
                result.AppendFormat("  Start: {0}\n", so.StartType);
                result.AppendFormat("  State: {0}\n", so.CurrentState);
                result.AppendFormat("  Image: {0}\n\n", so.ImagePath);
            }
            return result.ToString();
        }
        
        #endregion
    }
}
