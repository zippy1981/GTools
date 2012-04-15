using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace GSharpTools.InputFormats
{
    public class InputFormatInteger : IInputFormat
    {
        private readonly TextBox Control;
        private readonly string Name;
        private readonly IInputFormatEventSink EventSink;

        public InputFormatInteger(string name, TextBox tb, IInputFormatEventSink es)
        {
            Control = tb;
            EventSink = es;
            Name = name;
            tb.TextAlign = HorizontalAlignment.Right;
            tb.KeyDown += new KeyEventHandler(IntegerControl_OnKeyDown);
            tb.KeyPress += new KeyPressEventHandler(IntegerControl_OnKeyPress);
            tb.Leave += new EventHandler(IntegerControl_OnLeave);
        }

        public int? Value
        {
            get
            {
                if (Control.Text == "")
                    return null;

                return int.Parse(Control.Text);
            }
            set
            {
                if (value.HasValue)
                    Control.Text = string.Format("{0}", value.Value);
                else
                    Control.Text = "";
            }
        }

        private void IntegerControl_OnLeave(object sender, EventArgs e)
        {
            if (Control.Text != "")
            {
                try
                {
                    Control.Text = int.Parse(Control.Text).ToString();
                }
                catch
                {
                    EventSink.ReportFailure(Control, 
                        string.Format("Keine gültige Zahl im Feld {0}", Name));
                }
            }
        }

        // Boolean flag used to determine when a character other than a number is entered.
        private bool m_nonNumberEntered = false;

        // Handle the KeyDown event to determine the type of character entered into the control.
        private void IntegerControl_OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Initialize the flag to false.
            m_nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        m_nonNumberEntered = true;
                    }
                }
            }
        }

        // This event occurs after the KeyDown event and can be used to prevent
        // characters from entering the control.
        private void IntegerControl_OnKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (m_nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

    }
}
