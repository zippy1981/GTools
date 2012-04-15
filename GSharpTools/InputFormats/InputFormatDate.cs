using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace GSharpTools.InputFormats
{
    public class InputFormatDate : IInputFormat
    {
        private readonly TextBox Control;
        private readonly string Name;
        private readonly IInputFormatEventSink EventSink;

        public InputFormatDate(string name, TextBox tb, IInputFormatEventSink es)
        {
            Control = tb;
            EventSink = es;
            Name = name;
        }

        public static DateTime FromString(string text)
        {
            if( string.IsNullOrEmpty(text))
                throw new FormatException();

            string[] tokens = text.Split(',', '.');
            DateTime dt = DateTime.Now;
            if (tokens.Length != 3)
                throw new FormatException();
            
            int day = int.Parse(tokens[0]);
            int month = int.Parse(tokens[1]);
            int year = int.Parse(tokens[2]);
            if (year < 50)
                year += 2000;
            else if (year < 100)
                year += 1900;
            return new DateTime(year, month, day);        
        }

        public static string ToString(DateTime dt)
        {
            return string.Format("{0}.{1}.{2}", dt.Day, dt.Month, dt.Year);
        }

        public DateTime? Value
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Control.Text))
                        return null;

                    return InputFormatDate.FromString(Control.Text);
                }
                catch
                {
                }
                return null;                
            }
            set
            {
                if (value != null)
                {
                    DateTime dt = value.Value;
                    Control.Text = InputFormatDate.ToString(dt);
                }
                else
                {
                    Control.Text = "";
                }
            }
        }

        public void ListWarnings(StringBuilder sb) 
        {
            
        }

    }
}
