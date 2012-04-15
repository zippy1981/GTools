using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace GSharpTools.InputFormats
{
    public class InputFormatJNText : IInputFormat
    {
        private readonly TextBox Control;
        private readonly string Name;

        public InputFormatJNText(string name, TextBox tb)
        {
            Control = tb;
            Name = name;
            tb.Size = new System.Drawing.Size(25, 13);
            tb.Text = "";
            tb.TextAlign = HorizontalAlignment.Right;
            tb.KeyPress += OnKeyPressed;
        }

        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(Control.Text))
                    return null;

                return Control.Text;
            }
            set
            {
                Control.Text = value;
            }
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            Trace.TraceInformation("OnKeyPressed: {0}", Tools.DumpObject(e.KeyChar));

            if ((e.KeyChar == 'j') || (e.KeyChar == 'n') ||
                (e.KeyChar == 'J') || (e.KeyChar == 'N'))
            {
                e.Handled = true;
                Control.Text = e.KeyChar.ToString().ToLower();
            }
            else if (e.KeyChar == ' ')
            {
                e.Handled = true;
                Control.Text = "";
            }
            else if (e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }
    }
}
