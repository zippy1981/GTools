
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DLLUsage
{
    public partial class DLLUsageForm : Form
    {
        private ProcessList ProcessList;
        private readonly GPrint.IPrinter Printer = new GPrint.TextPrinter();
        private readonly string DependsExe = null;
        private readonly string CmdExe = null;
        private StringBuilder PrintDocument;
        private static int MaxPrintCharsPerLine = 95;

        public DLLUsageForm()
        {
            InitializeComponent();
            ProcessList = new ProcessList(treeView1);
            treeView1.MouseWheel += new MouseEventHandler(OnMouseWheel);


            dllusage.Properties.Settings settings = new dllusage.Properties.Settings();
            treeView1.Font = new Font(settings.FontFace, settings.FontHeight);


            ContextMenu cm = new ContextMenu();
            DefineContextMenu(cm, "Show dependencies", tsmDependencies_Click);
            DefineContextMenu(cm, "Console", tsmConsole_Click);
            DefineContextMenu(cm, "Explorer", tsmExplorer_Click);
            cm.MenuItems.Add("-");
            DefineContextMenu(cm, "Copy to clipboard", copyToClipboardToolStripMenuItem_Click);
            DefineContextMenu(cm, "Copy to clipboard in quotationmarks", copyToClipboardInquotationmarksToolStripMenuItem_Click);

            if (!FindProgram("depends.exe", ref DependsExe))
            {
                // special case: depends.exe may not reside on the path,
                // but the normal install location is easy enough
                DependsExe = null;
                tsmDependencies.Enabled = false;
                tsbDependencies.Enabled = false;
                cm.MenuItems[0].Enabled = false;
            }

            if (!FindProgram("cmd.exe", ref CmdExe))
            {
                CmdExe = null;
                tsmConsole.Enabled = false;
                tsbConsole.Enabled = false;
                cm.MenuItems[1].Enabled = false;
            }

            treeView1.ContextMenu = cm;

            Text = string.Format("DLLUSAGE {0}", GSharpTools.AppVersion.Get());
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                float fontheight = treeView1.Font.SizeInPoints;
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
                treeView1.Font = new Font(treeView1.Font.Name, (float)fontheight);
            }
        }
        [DllImport("shell32.dll", EntryPoint="FindExecutable")]
        public static extern long FindExecutableW(
            string lpFile, string lpDirectory, StringBuilder lpResult);

        private bool FindProgram(string name, ref string executable)
        {
            StringBuilder objResultBuffer = new StringBuilder(1024);
            long lngResult = 0;

            lngResult = FindExecutableW(name, string.Empty, objResultBuffer);

            if(lngResult >= 32)
            {
                executable = objResultBuffer.ToString();
                Trace.TraceInformation("Mapped '{0}' to '{1}'", name, executable);
                return true;
            }

            Trace.TraceError("Error: ({0})", lngResult);
            return false;
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            ProcessList.Refresh();
        }

        private void btListByProcessName_Click(object sender, EventArgs e)
        {
            ProcessList.SwitchTo(DisplayMode.ListByProcessName);
        }

        private void btListByDLL_Click(object sender, EventArgs e)
        {
            ProcessList.SwitchTo(DisplayMode.ListByDLLName);
        }

        private void btListByDLLPath_Click(object sender, EventArgs e)
        {
            ProcessList.SwitchTo(DisplayMode.ListByDLLPath);
        }

        private void btListByProcessPath_Click(object sender, EventArgs e)
        {
            ProcessList.SwitchTo(DisplayMode.ListByProcessPath);
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            ProcessList.ApplyFilter(toolStripComboBox1.Text);
        }

        private void btExpandAll_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void btCollapseAll_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://p-nand-q.com/download/dllusage.html");
            }
            catch (Exception)
            {
                
            }
        }

        private string GetPath()
        {
            TreeNode n = treeView1.SelectedNode;
            if( n != null )
            {
                if( n.Tag != null )
                {
                    ProcessInfo pi = n.Tag as ProcessInfo;
                    if (pi != null)
                        return pi.PathName;

                    string pathstring_incl_proc = n.Tag as string;
                    if( pathstring_incl_proc != null )
                    {
                        if (!pathstring_incl_proc.EndsWith("]"))
                            return pathstring_incl_proc;
                        int k = pathstring_incl_proc.LastIndexOf("[");
                        if (k >= 0)
                            return pathstring_incl_proc.Substring(0, k - 1);
                    }
                }

            }
            return null;
        }

        private void fontToolStripButton_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = treeView1.Font;
            if (fd.ShowDialog() != DialogResult.Cancel)
            {
                treeView1.Font = fd.Font;
            }
        }

        private void DLLUsageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dllusage.Properties.Settings settings = new dllusage.Properties.Settings();

            settings.FontFace = treeView1.Font.Name;
            settings.FontHeight = treeView1.Font.SizeInPoints;
            settings.Save();
        }

        private void DefineContextMenu(ContextMenu cm, string name, EventHandler eh)
        {
            MenuItem mi = cm.MenuItems.Add(name);
            mi.Click += eh;
        }

        private void FeedLine(string format, params object[] args)
        {
            string text = string.Format(format, args);

            while (text.Length > MaxPrintCharsPerLine)
            {
                PrintDocument.AppendLine(text.Substring(0, MaxPrintCharsPerLine));
                text = "           " + text.Substring(MaxPrintCharsPerLine);
            }
            PrintDocument.AppendLine(text);
        }

        private string GetPrintDocument()
        {
            PrintDocument = new StringBuilder();

            int index = 0;
            foreach (TreeNode node in treeView1.Nodes)
            {
                ++index;

                FeedLine("{0:0000}/{1:0000}: {2}", index, treeView1.Nodes.Count, node.Text);

                foreach (TreeNode subnode in node.Nodes)
                {
                    FeedLine("           " + subnode.Text);
                }
                FeedLine("");
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

        private void tsmFile_SaveAsXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "XML Files|*.xml|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ProcessList.SaveAsXML(ofd.FileName);
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = GetPath();
            if (!string.IsNullOrEmpty(path))
            {
                Clipboard.SetText(path);
            }
        }

        private void copyToClipboardInquotationmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = GetPath();
            if (!string.IsNullOrEmpty(path))
            {
                StringBuilder target = new StringBuilder();
                target.Append('"');
                foreach (char c in path)
                {
                    if (c == '"' || c == '\\')
                    {
                        target.Append('\\');
                    }
                    target.Append(c);
                }
                target.Append('"');
                Clipboard.SetText(target.ToString());
            }
        }

        private void tsmDependencies_Click(object sender, EventArgs e)
        {
            RunCmd(DependsExe);
        }

        private void tsmConsole_Click(object sender, EventArgs e)
        {
            RunCmd(CmdExe);
        }

        private void RunCmd(string cmd)
        {
            string path = GetPath();
            if (!string.IsNullOrEmpty(path))
            {
                ProcessStartInfo psi = new ProcessStartInfo(cmd);
                psi.WorkingDirectory = Path.GetDirectoryName(path);
                psi.FileName = cmd;
                psi.Arguments = Path.GetFileName(path);
                Process.Start(psi);
            }
        }

        private void tsmExplorer_Click(object sender, EventArgs e)
        {
            string path = GetPath();
            if (!string.IsNullOrEmpty(path))
            {
                Process.Start(Path.GetDirectoryName(path));
            }
        }

        private void tsmFile_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
