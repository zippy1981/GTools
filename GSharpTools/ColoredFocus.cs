using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace GSharpTools
{
    public class ColoredFocus
    {
        public static Color BackColor = Color.Khaki;

        public static void Enable(Control c)
        {
            EnableControls(c.Controls);
        }

        public static void Enable(Form f)
        {
            EnableControls(f.Controls);
        }

        public static void EnableControls(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if ((c is ComboBox) || (c is CheckBox) || (c is TextBox) || (c is Button) || (c is DateTimePicker) || (c is RadioButton))
                {
                    c.Leave += new EventHandler(Control_Leave);
                    c.Enter += new EventHandler(Control_Enter);
                }
                else if (c is TabControl)
                {
                    TabControl tab = c as TabControl;
                    foreach (TabPage page in tab.TabPages)
                    {
                        EnableControls(page.Controls);
                    }
                }
                else if (c is GroupBox)
                {
                    GroupBox group = c as GroupBox;
                    EnableControls(group.Controls);
                }
                else if (!(c is Label))
                {
                    Trace.TraceInformation("Ignoring control type {0}", c);
                }
            }
        }

        private static void Control_Leave(object sender, EventArgs e)
        {
            Control c = sender as Control;

            if (c is Button)
            {
                c.BackColor = Color.White;
            }
            else
            {
                c.BackColor = Color.Empty;
            }
        }

        private static void Control_Enter(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = BackColor;
        }
    }
}
