using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace pserv3
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IDS.AssignStrings(GSharpTools.Tools.ReadStrings(Environment.CurrentDirectory));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        internal static string TimeSpanToString(TimeSpan ts)
        {
            return string.Format("{0}:{1}:{2}",
                ts.Hours.ToString("D").PadLeft(2, '0'),
                ts.Minutes.ToString("D").PadLeft(2, '0'),
                ts.Seconds.ToString("D").PadLeft(2, '0'));
        }        

        internal static string BytesToSize(long bytes)
        {
            if (bytes < 1024)
            {
                return string.Format("{0} Bytes", bytes);
            }

            decimal v = ((decimal)bytes) / 1024.0m;
            if (v < 1024)
            {
                return string.Format("{0} KB", v.ToString("F2"));
            }
            v /= 1024;
            if (v < 1024)
            {
                return string.Format("{0} MB", v.ToString("F2"));
            }
            v /= 1024;
            return string.Format("{0} GB", v.ToString("F2"));
        }
    }
}
