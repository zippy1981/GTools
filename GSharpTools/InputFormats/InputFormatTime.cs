using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace GSharpTools.InputFormats
{
    public class InputFormatTime : IInputFormat
    {
        private readonly TextBox Control;
        private readonly string Name;
        private readonly IInputFormatEventSink EventSink;

        public InputFormatTime(string name, TextBox tb, IInputFormatEventSink es)
        {
            Control = tb;
            EventSink = es;
            Name = name;
        }

        public static DateTime FromString(string text)
        {
            if( string.IsNullOrEmpty(text))
                throw new FormatException();

            string[] tokens = text.Split(',', '.',':');
            DateTime dt = DateTime.Now;
            if (tokens.Length > 3 )
                throw new FormatException();
            
            int hour = int.Parse(tokens[0]);
            int minute = int.Parse(tokens[1]);
            int second = 0;
            if( tokens.Length > 2 )
                second = int.Parse(tokens[2]);
            return new DateTime(1899, 12, 30, hour % 24, minute % 60, second % 60);
        }

        public static string ToString(DateTime dt)
        {
            return string.Format("{0:##00}:{1:##00}:{2:##00}", dt.Hour, dt.Minute, dt.Second);
        }

        public DateTime? Value
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Control.Text))
                        return null;

                    return InputFormatTime.FromString(Control.Text);
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
                    Control.Text = InputFormatTime.ToString(dt);
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
