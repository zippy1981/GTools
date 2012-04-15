using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;

namespace pserv3.EventJournal
{
    public class EventJournalObject : IServiceObject
    {
        private readonly object[] Objects = new object[(int)EventJournalItemTypes.MAX];
        private string ToolTipText;
        private Color ForegroundColor = Color.Black;

        public EventJournalObject(EventLogEntryCollection entryCollection, int itemIndex)
        {
            EventLogEntry Entry = entryCollection[itemIndex];

            Objects[(int)EventJournalItemTypes.Index] = Entry.Index;

            Objects[(int)EventJournalItemTypes.TimeWritten] = Entry.TimeWritten;
            ToolTipText = Entry.Message;
            Objects[(int)EventJournalItemTypes.Message] = Entry.Message;
            Objects[(int)EventJournalItemTypes.ReplacementStrings] = GSharpTools.Tools.Join(Entry.ReplacementStrings, ",");
            Objects[(int)EventJournalItemTypes.Category] = Entry.Category;
            Objects[(int)EventJournalItemTypes.EntryType] = Entry.EntryType;
            Objects[(int)EventJournalItemTypes.Source] = Entry.Source;
            Objects[(int)EventJournalItemTypes.UserName] = Entry.UserName;
            Objects[(int)EventJournalItemTypes.Machine] = Entry.MachineName;

            if (Entry.EntryType == EventLogEntryType.Error)
                ForegroundColor = Color.Blue;
            else if (Entry.EntryType == EventLogEntryType.Information)
                ForegroundColor = Color.Gray;
        }
        
        #region IServiceObject Members

        public object GetObject(int nID)
        {
            return Objects[nID];
        }

        public string GetText(int nID)
        {
            object o = Objects[nID];
            return (o == null) ? "" : o.ToString();
        }

        public string GetToolTipText()
        {
            return ToolTipText;
        }

        public Color GetForegroundColor()
        {
            return ForegroundColor;
        }

        #endregion
    }
}
