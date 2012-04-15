using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pserv3.Services
{
    public enum ServiceItemTypes : int
    {
        DisplayName = 0,
        ServiceName,
        Status,
        Start,
        Type,
        Path,
        PID,
        User,
        LoadOrderGroup,
        ErrorControl,
        TagId,
        Dependencies,
        Description,
        MAX
    }
}
