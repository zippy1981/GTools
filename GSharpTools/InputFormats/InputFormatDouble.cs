using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace GSharpTools.InputFormats
{
    public class InputFormatDouble : IInputFormat
    {
        private readonly TextBox Control;
        private readonly string Name;
        private IInputFormatEventSink EventSink;
        public double? MinValue = null;
        public double? MaxValue = null;
        public bool ForceFraction = false;

        public InputFormatDouble(string name, TextBox tb, IInputFormatEventSink sink)
        {
            Control = tb;
            EventSink = sink;
            Name = name;
            tb.TextAlign = HorizontalAlignment.Right;
            tb.KeyDown += new KeyEventHandler(DoubleControl_OnKeyDown);
            tb.KeyPress += new KeyPressEventHandler(DoubleControl_OnKeyPress);
            tb.Leave += new EventHandler(DoubleControl_OnLeave);
        }

        public static string Format(double value)
        {
            string formatted = string.Format("{0:#,##0.00}", value);
            double decoded = double.Parse(formatted);
            if (decoded != value)
            {
                return value.ToString();
            }
            return formatted;
        }

        public double? Value
        {
            get
            {
                if (Control.Text == "")
                    return null;

                return double.Parse(Control.Text);
            }
            set
            {
                if (value.HasValue)
                {
                    if (!ForceFraction)
                    {
                        Control.Text = InputFormatDouble.Format(value.Value);
                    }
                    else
                    {
                        Control.Text = string.Format("{0:#,##0.00}", value.Value);
                    }
                }
                else
                {
                    Control.Text = "";
                }
            }
        }

        private void DoubleControl_OnLeave(object sender, EventArgs e)
        {
            if (Control.Text != "")
            {
                try
                {
                    double value = Double.Parse(Control.Text);
                    Control.Text = InputFormatDouble.Format(value);
                }
                catch
                {
                    if (EventSink != null)
                    {
                        EventSink.ReportFailure(Control,
                            string.Format("Keine gültige Zahl im Feld {0}", Name));
                    }
                }
            }
        }

        public void ListWarnings(StringBuilder sb) 
        {
            if (Control.Text != "")
            {
                try
                {
                    double value = Double.Parse(Control.Text);
                    if (MinValue.HasValue)
                    {
                        if (value < MinValue)
                        {
                            sb.AppendFormat("- Feld {0} ist zu klein [Minimum: {1}]", Name, MinValue);
                        }
                    }
                    if (MaxValue.HasValue)
                    {
                        if (value > MaxValue)
                        {
                            sb.AppendFormat("- Feld {0} ist zu groß [Maximum: {1}]", Name, MaxValue);
                        }
                    }
                    Control.Text = value.ToString();
                }
                catch
                {
                    sb.AppendFormat("- Keine gültige Zahl im Feld {0}", Name);
                }
            }
        }

        // Boolean flag used to determine when a character other than a number is entered.
        private bool m_nonNumberEntered = false;

        // Handle the KeyDown event to determine the type of character entered into the control.
        private void DoubleControl_OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
                    if ((e.KeyCode != Keys.Back) && (e.KeyCode != Keys.Decimal) && (e.KeyCode != Keys.OemPeriod) && (e.KeyCode != Keys.Oemcomma))
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
        private void DoubleControl_OnKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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
