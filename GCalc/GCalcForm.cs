using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using GSharpTools;
using GSharpTools.Calculator;

namespace GCalc
{
    public partial class GCalcForm : Form
    {
        private readonly GSharpTools.Calculator.Interpreter m_runtime = new GSharpTools.Calculator.Interpreter();
        private readonly GSharpTools.Calculator.Parser m_parser = new GSharpTools.Calculator.Parser();
        private readonly GPrint.IPrinter Printer = new GPrint.TextPrinter();
        private string FileName;

        public GCalcForm()
        {
            InitializeComponent();
            m_input.MouseWheel += new MouseEventHandler(this.onMouseWheel);
            m_output.MouseWheel += new MouseEventHandler(this.onMouseWheel);

            GCalc.Properties.Settings settings = new GCalc.Properties.Settings();
            m_input.Font = new Font(settings.FontFace, settings.FontHeight);
            FileName = settings.FileName;
            m_output.Font = m_input.Font;
            Text = string.Format("GCALC {0}", AppVersion.Get());
        }

        private void onMouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                float fontheight = m_input.Font.SizeInPoints;
                if (e.Delta > 0)
                {
                    fontheight += (float)1.0;
                }
                else
                {
                    if (fontheight > 4.0)
                    {
                        fontheight -= (float)1.0;
                    }
                }
                m_input.Font = new Font(m_input.Font.Name, (float)fontheight);
                m_output.Font = m_input.Font;

            }
        }

        private void saveConfiguration()
        {
            GCalc.Properties.Settings settings = new GCalc.Properties.Settings();
            settings.FontFace = m_input.Font.Name;
            settings.FontHeight = m_input.Font.SizeInPoints;
            settings.FileName = FileName;
            saveFile(FileName, true);
            settings.Save();
        }

        private bool saveFile(string filename, bool quiet)
        {
            if ((filename != null) && (filename.Length > 0))
            {
                try
                {
                    StreamWriter s = File.CreateText(filename);
                    s.Write(m_input.Text);
                    s.Close();
                    FileName = filename;
                    return true;
                }
                catch (IOException ex)
                {
                    if (!quiet)
                    {
                        MessageBox.Show(this, String.Format("ERROR, unable to save file {0}.\r\nThe error code is {1}", filename, ex.Message));
                    }
                }
            }
            return false;
        }

        private bool loadFile(string filename, bool quiet)
        {
            if ((filename != null) && (filename.Length > 0))
            {
                try
                {
                    FileStream f = File.OpenRead(filename);
                    if (f.Length > 0)
                    {
                        char[] data = new char[f.Length];
                        StreamReader s = new StreamReader(f);
                        int len = s.Read(data, 0, (int)f.Length);
                        m_input.Text = new String(data, 0, len);
                        FileName = filename;
                        s.Close();
                        f.Close();
                        return true;
                    }
                }
                catch (IOException ex)
                {
                    if (!quiet)
                    {
                        MessageBox.Show(this, String.Format("ERROR, unable to load file {0}.\r\nThe error code is {1}", filename, ex.Message));
                    }
                }
            }
            return false;
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to create a new file?", "Question",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_output.Text = m_input.Text = "";
                m_input.Focus();
            }
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            string filename = FileName;
            if (filename.Length > 0)
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.InitialDirectory = filename;
                ofd.FileName = filename;
                ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (loadFile(ofd.FileName, false))
                    {
                        FileName = ofd.FileName;
                    }
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string filename = FileName;
            ofd.InitialDirectory = filename;
            ofd.CheckFileExists = false;
            ofd.Title = "Save as";
            ofd.FileName = filename;
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (saveFile(ofd.FileName, false))
                {
                    FileName = ofd.FileName;
                }
            }
        }

        private void fontToolStripButton_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = m_input.Font;
            if (fd.ShowDialog() != DialogResult.Cancel)
            {
                m_output.Font = fd.Font;
                m_input.Font = fd.Font;
            }
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            if (m_input.SelectedText != "")
            {
                m_input.Cut();
            }
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            if (m_input.SelectionLength > 0)
            {
                m_input.Copy();
            }
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                m_input.Paste();
            }
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            Process.Start("http://p-nand-q.com/download/pcalc.html"); 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveConfiguration();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (FileName != null && FileName.Length > 0)
            {
                loadFile(FileName, true);
            }
        }

        private void m_input_TextChanged(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            StringBuilder display = new StringBuilder();
            foreach (string line in m_input.Text.Split('\n'))
            {
                string temp = line.Trim();
                try
                {
                    if (temp != "")
                    {
                        Operation node = m_parser.Parse(temp);
                        Trace.TraceInformation("PARSED AS {0}", node);
                        
                        Value result = node.Evaluate(m_runtime);
                        display.AppendLine(result.ToString());
                    }
                    else
                    {
                        display.AppendLine("");
                    }
                }
                catch (Exception e)
                {
                    display.AppendLine(e.Message.ToString());
                }
            }
            SetOutput(display.ToString());
        }
        private void SetOutput(string value)
        {
            Trace.TraceInformation(value);
            m_output.Text = value;
        }

        private string GetPrintDocument()
        {
            StringBuilder PrintDocument = new StringBuilder();

            foreach (string line in m_input.Text.Split('\n'))
            {
                string temp = line.Trim();
                PrintDocument.AppendFormat("Expression: {0}\n", temp);
                try
                {
                    if (temp != "")
                    {
                        Operation node = m_parser.Parse(temp);
                        Value result = node.Evaluate(m_runtime);

                        PrintDocument.AppendFormat("    Result: {0}\n", result.ToString());
                        PrintDocument.AppendLine();
                    }
                }
                catch (Exception e)
                {
                    PrintDocument.AppendFormat("    Result: {0}\n", e.ToString());                    
                }
            }
            return PrintDocument.ToString();
        }

        private void tsmFile_Print_Click(object sender, EventArgs e)
        {
            Printer.SetDocument(GetPrintDocument());
            Printer.PrintFile(true);
        }

        private void tsmFile_PrintPreview_Click(object sender, EventArgs e)
        {
            Printer.SetDocument(GetPrintDocument());
            Printer.ShowPrintPreview();
        }

        private void tsmFile_PageSetup_Click(object sender, EventArgs e)
        {
            Printer.ShowPageSettings();
        }

        private void tsmFile_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
