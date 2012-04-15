using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace GPrint
{
    public class TextPrinter : IPrinter
    {
        private string[] PrintLines;
        private readonly int LinesPerPage = 76;
        private int PageNumber;
        private readonly Font PrintFont = new Font("Courier New", 8);
        private readonly PrintDocument printDoc = new PrintDocument();
        private readonly PageSettings pgSettings = new PageSettings();
        private readonly PrinterSettings prtSettings = new PrinterSettings();
        private readonly PrintDialog printDialog1 = new PrintDialog();

        public TextPrinter()
        {
            printDoc.PrintPage += printDoc_PrintPage;
            printDialog1.UseEXDialog = true;
        }

        private void printDoc_PrintPage(Object sender, PrintPageEventArgs e)
        {
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;

            float linesPerPage = e.MarginBounds.Height / PrintFont.GetHeight(e.Graphics);
            float yPosition = 0;
            int count = 0;

            SolidBrush myBrush = new SolidBrush(Color.Black);

            int i = 0;
            for (i = 0; i < LinesPerPage && (i + (PageNumber * LinesPerPage) < PrintLines.Length); ++i)
            {
                string line = PrintLines[i + (PageNumber * LinesPerPage)];

                // calculate the next line position based on the height of the font according to the printing device
                yPosition = topMargin + (count * PrintFont.GetHeight(e.Graphics));

                // draw the next line in the rich edit control
                e.Graphics.DrawString(line, PrintFont, myBrush, leftMargin, yPosition, new StringFormat());
                count++;
            }
            e.HasMorePages = (i + (PageNumber * LinesPerPage) < PrintLines.Length);
            ++PageNumber;
            myBrush.Dispose();
        }


        #region IPrinter Members

        void IPrinter.SetDocument(string content)
        {
            PrintLines = content.Split('\n');
            PageNumber = 0;
        }

        void IPrinter.PrintFile(bool showDialog)
        {
            printDoc.DefaultPageSettings = pgSettings;
            printDialog1.PrinterSettings = prtSettings;
            printDialog1.Document = printDoc;

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        void IPrinter.ShowPrintPreview()
        {
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = printDoc;
            dlg.ShowDialog();
        }

        void IPrinter.ShowPageSettings()
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.PageSettings = pgSettings;
            pageSetupDialog.PrinterSettings = prtSettings;
            pageSetupDialog.AllowOrientation = true;
            pageSetupDialog.AllowMargins = true;
            pageSetupDialog.ShowDialog();
        }

        #endregion
    }
}
