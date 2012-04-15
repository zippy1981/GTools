using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Drawing;
using Microsoft.Win32;

namespace pserv3.Uninstaller
{
    public class UninstallerObject : IServiceObject
    {
        private readonly object[] Objects = new object[(int)UninstallerItemTypes.MAX];
        public string ToolTipText;
        public Color ForegroundColor = Color.Black;

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Application);
            }
        }

        public string AboutLink
        {
            get
            {
                return GetText((int)UninstallerItemTypes.AboutLink);
            }
        }

        public string HelpLink
        {
            get
            {
                return GetText((int)UninstallerItemTypes.HelpLink);
            }
        }

        public string Application
        {
            get
            {
                return GetText((int)UninstallerItemTypes.Application);
            }
        }

        public string Path
        {
            get
            {
                return GetText((int)UninstallerItemTypes.Path);
            }
        }

        public string Publisher
        {
            get
            {
                return GetText((int)UninstallerItemTypes.Publisher);
            }
        }

        public string Action
        {
            get
            {
                return GetText((int)UninstallerItemTypes.Action);
            }
        }

        public string Key
        {
            get
            {
                return GetText((int)UninstallerItemTypes.Key);
            }
        }

        public UninstallerObject(RegistryKey rootKey, string keyPath, string keyName)
        {
            using (RegistryKey hkKey = rootKey.OpenSubKey(keyPath, false))
            {
                string ApplicationName = hkKey.GetValue("DisplayName") as string;
                if (string.IsNullOrEmpty(ApplicationName))
                {
                    ApplicationName = hkKey.GetValue("QuietDisplayName") as string;
                }
                if (string.IsNullOrEmpty(ApplicationName))
                    ApplicationName = keyName;
                Objects[(int)UninstallerItemTypes.Application] = ApplicationName;
                Objects[(int)UninstallerItemTypes.Path] = hkKey.GetValue("InstallLocation") as string;
                Objects[(int)UninstallerItemTypes.Key] = keyName;
                Objects[(int)UninstallerItemTypes.Version] = hkKey.GetValue("DisplayVersion") as string;
                Objects[(int)UninstallerItemTypes.Publisher] = hkKey.GetValue("Publisher") as string;
                Objects[(int)UninstallerItemTypes.HelpLink] = hkKey.GetValue("HelpLink") as string;
                Objects[(int)UninstallerItemTypes.AboutLink] = hkKey.GetValue("URLInfoAbout") as string;
                ToolTipText = hkKey.GetValue("UninstallString") as string;                
                Objects[(int)UninstallerItemTypes.Action] = ToolTipText;
                if (string.IsNullOrEmpty(ToolTipText))
                {
                    ForegroundColor = Color.Gray;
                }
                else if (!string.IsNullOrEmpty(Path))
                {
                    ForegroundColor = Color.Blue;
                }
            }
        }

        #region IServiceObject Members

        public object GetObject(int nID)
        {
            return Objects[nID]; ;
        }

        public string GetText(int nID)
        {
            object o = Objects[nID];
            if (o == null)
                return "";
            return o.ToString();
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
