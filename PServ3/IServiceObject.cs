using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace pserv3
{
    public interface IServiceObject
    {
        string GetText(int nID);
        object GetObject(int nID);
        string GetToolTipText();
        Color GetForegroundColor();
    }
}
