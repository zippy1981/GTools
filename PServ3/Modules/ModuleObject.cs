using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;
using pserv3.Processes;

namespace pserv3.Modules
{
    public class ModuleObject : IServiceObject
    {
        public readonly object[] Objects = new object[(int)ModuleItemTypes.MAX];
        public string ToolTipText;
        public Color ForegroundColor = Color.Black;

        public ModuleObject(Process p, ProcessModule m, string username)
        {
            Objects[(int)ModuleItemTypes.Name] = Path.GetFileName(m.FileName);
            Objects[(int)ModuleItemTypes.ID] = p.Id;
            Objects[(int)ModuleItemTypes.Path] = Path.GetDirectoryName(m.FileName);
            Objects[(int)ModuleItemTypes.ModuleMemorySize] = m.ModuleMemorySize; // 
            Objects[(int)ModuleItemTypes.FileDescription] = m.FileVersionInfo.FileDescription;
            Objects[(int)ModuleItemTypes.FileVersion] = m.FileVersionInfo.FileVersion;
            Objects[(int)ModuleItemTypes.Product] = m.FileVersionInfo.ProductName;
            Objects[(int)ModuleItemTypes.ProductVersion] = m.FileVersionInfo.ProductVersion;
            ForegroundColor = ProcessObject.GetColorFromName(p, username);
            ToolTipText = m.FileVersionInfo.FileDescription;
        }

        public object GetObject(int nID)
        {
            return Objects[nID];
        }

        private static string GetSafeProcessName(Process p)
        {
            string result = p.ProcessName;
            try
            {
                if (p.Id >= 10)
                    result = p.MainModule.FileName;
            }
            catch
            {

            }
            if (result.StartsWith("\\??\\"))
                result = result.Substring(4);
            return result;
        }

        #region IServiceObject Members

        public string GetText(int nID)
        {
            object o = Objects[nID];
            if (nID == (int)ModuleItemTypes.ModuleMemorySize)
            {
                return Program.BytesToSize((int)o);
            }
            return (o == null) ? "" : o.ToString();
        }

        public string GetToolTipText()
        {
            return ToolTipText;
        }

        public Color GetForegroundColor()
        {
            return ForegroundColor;
        }

        #endregion
    }
}
