using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace pserv3
{
    public interface IServiceColumn
    {
        string GetName();
        int GetID();
        int Compare(IServiceObject a, IServiceObject b);
        HorizontalAlignment GetTextAlign();
    }
}
