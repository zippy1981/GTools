using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GSharpTools.InputFormats
{
    public interface IInputFormatEventSink
    {
        bool ReportFailure(Control ctl, string text);
        bool ReportWarning(Control ctl, string text);
        void ClearFailure();
    }
}
