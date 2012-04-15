using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Diagnostics;

namespace GPrint
{
    public class XmlPrinter : IPrinter
    {
        private XmlDocStruct Document = null;

        public void SetDocument(string content)
        {
            Document = new XmlDocStruct(content);
        }

        public void PrintFile(bool showDialog)
        {
        }

        public void ShowPrintPreview()
        {
        }
        public void ShowPageSettings()
        {
        }
    }
}
