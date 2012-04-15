using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pserv3.Uninstaller
{
    public enum UninstallerItemTypes : int
    {
        Application,
        Path,
        Key,
        Version,
        Publisher,
        HelpLink,
        AboutLink,
        Action,
        MAX
    }
}
