using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pserv3.Processes
{
    public enum ProcessItemTypes : int
    {
        ID,
        Name,
        Path,
        User,
        FileDescription,
        FileVersion,
        Product,
        ProductVersion,
        ThreadCount,
        HandleCount,
        MainWindowHandle,
        MainWindowTitle,
        Responding,
        StartTime,
        TotalRunTime,
        TotalProcessorTime,
        PrivilegedProcessorTime,
        ProcessPriorityClass,
        SessionId,
        StartInfoArguments,
        NonpagedSystemMemorySize64,
        PagedMemorySize64,
        PagedSystemMemorySize64,
        PeakPagedMemorySize64,
        PeakVirtualMemorySize64,
        PeakWorkingSet64,
        PrivateMemorySize64,
        VirtualMemorySize64,
        WorkingSet64,

        MAX
    }
}
