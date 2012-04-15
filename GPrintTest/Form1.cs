using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GPrint;

namespace GPrintTest
{
    public partial class Form1 : Form
    {
        private readonly GPrint.IPrinter Printer = new GPrint.XmlPrinter();
        public Form1()
        {
            InitializeComponent();

            scintilla1.Text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<document>
    <h1>Header</h1>
    <p>Hello, World</p>
    <p>This paragraph is auto-formatted to fit in all text</p>
    <p>This paragraph is auto-formatted to fit in all text</p>
    <p>This paragraph is auto-formatted to fit in all text</p>
</document>";
            scintilla1.ConfigurationManager.Language = "xml";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printer.SetDocument(scintilla1.Text);
            Printer.PrintFile(true);
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printer.SetDocument(scintilla1.Text);
            Printer.ShowPrintPreview();
        }

        private void printerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printer.ShowPageSettings();
        }
    }
}
