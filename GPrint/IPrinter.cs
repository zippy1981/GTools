using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPrint
{
    public interface IPrinter
    {
        void SetDocument(string content);
        void PrintFile(bool showDialog);
        void ShowPrintPreview();
        void ShowPageSettings();
    }
}
