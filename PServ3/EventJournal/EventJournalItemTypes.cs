using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pserv3.EventJournal
{
    public enum EventJournalItemTypes : int
    {
        Index,
        TimeWritten,
        Message,
        ReplacementStrings,
        Category,
        EntryType,
        Source,
        UserName,
        Machine,
        MAX
    }
}
