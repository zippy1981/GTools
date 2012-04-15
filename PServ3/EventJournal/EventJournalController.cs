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

namespace pserv3.EventJournal
{
    public class EventJournalController : IServiceController
    {
        private readonly List<IServiceObject> Events = new List<IServiceObject>();
        private readonly string JournalName;
        private bool Cancelled;

        public EventJournalController(ListView listView, string journalName)
            :   base(listView)
        {
            JournalName = journalName;

            Columns.Add(new StandardColumn_Int(
                IDS.EventJournal_Column_Index,
                (int) EventJournalItemTypes.Index));
            Columns.Add(new StandardColumn(
                IDS.EventJournal_Column_TimeWritten,
                (int)EventJournalItemTypes.TimeWritten));
            Columns.Add(new StandardColumn(
                IDS.EventJournal_Column_Message,
                (int)EventJournalItemTypes.Message));
            Columns.Add(new StandardColumn(
                IDS.EventJournal_Column_ReplacementStrings,
                (int)EventJournalItemTypes.ReplacementStrings));
            Columns.Add(new StandardColumn(
                IDS.EventJournal_Column_Category,
                (int)EventJournalItemTypes.Category));
            Columns.Add(new StandardColumn(
                IDS.EventJournal_Column_EntryType,
                (int)EventJournalItemTypes.EntryType));
            Columns.Add(new StandardColumn(
                IDS.EventJournal_Column_Source,
                (int)EventJournalItemTypes.Source));
            Columns.Add(new StandardColumn(
                IDS.EventJournal_Column_Machine,
                (int)EventJournalItemTypes.Machine));
            Columns.Add(new StandardColumn(
                IDS.EventJournal_Column_UserName,
                (int)EventJournalItemTypes.UserName));
        }

        #region IServiceController Members

        /// <summary>
        /// Items are cached
        /// </summary>
        /// <returns>true</returns>
        public override bool ItemDataIsCached()
        {
            return true;
        }

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
                    wo.GetText((int)EventJournalItemTypes.Handle),
                    wo.GetText((int)EventJournalItemTypes.Title));

                result.AppendFormat("  Class: {0}\n",
                    wo.GetText((int)EventJournalItemTypes.Class));

                result.AppendFormat("  Style: {0}\n",
                    wo.GetText((int)EventJournalItemTypes.Style));
                result.AppendFormat("ExStyle: {0}\n",
                    wo.GetText((int)EventJournalItemTypes.ExStyle));

                result.AppendLine();

            }*/
            return result.ToString();
        }

        public override string GetCaption()
        {
            return string.Format("{0} Event Journal on \\{1} (this machine)", JournalName, Environment.MachineName);
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
        */

        public override void Refresh()
        {
            DateTime n = DateTime.Now;
            EventLog el = new EventLog(JournalName, Environment.MachineName);

            EventLogEntryCollection collection = el.Entries;
            Cancelled = false;
            Events.Clear();

            for (int i = collection.Count-1; i >= 0; --i)
            {
                bool isCancelled = false;
                lock (this)
                {
                    isCancelled = Cancelled;
                }
                if (isCancelled)
                    break;
                Events.Add(new EventJournalObject(collection, i));
            }
            Trace.TraceInformation("Took {0} for {1} events", DateTime.Now - n, Events.Count);
        }

        public override void CancelRefresh()
        {
            lock (this)
            {
                Cancelled = true;
            }
        }

        public override IEnumerable<IServiceObject> GetObjects()
        {
            return Events;
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
                /*xtw.WriteStartDocument();
                xtw.WriteStartElement("modules");
                xtw.WriteAttributeString("version", "2.0");
                foreach (ListViewItem item in Control.Items)
                {
                    EventJournalObject wo = item.Tag as EventJournalObject;
                    xtw.WriteStartElement("module");
                    xtw.WriteAttributeString("id", wo.GetText((int)EventJournalItemTypes.ID));

                    xtw.WriteElementString("name", wo.GetText((int)EventJournalItemTypes.Name));
                    xtw.WriteElementString("path", wo.GetText((int)EventJournalItemTypes.Path));
                    

                    xtw.WriteEndElement();
                }
                xtw.WriteEndElement();
                xtw.Close();
                */
            }
            if (filename == null)
            {
                Clipboard.SetText(buffer.ToString());
            }
        }

        #endregion
    }
}
