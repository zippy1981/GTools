using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace pserv3.Services
{
    public class ServiceObject : IServiceObject
    {
        public readonly object[] Objects = new object[(int)ServiceItemTypes.MAX];
        private string DescriptionText;
        public Color ForegroundColor = Color.Black;
        public SC_RUNTIME_STATUS CurrentState;
        public SC_CONTROLS_ACCEPTED ControlsAccepted;
        private QUERY_SERVICE_CONFIG Config;
        private bool StartTypeModified;
        private bool LoadOrderGroupModified;
        private bool DisplayNameModified;
        private bool DescriptionModified;
        private bool ServiceTypeModified;
        private bool ErrorControlModified;
        private bool BinaryPathNameModified;
        private bool ServiceStartNameModified;
        private bool PasswordModified;
        private string PasswordText;

        public string Password
        {
            set
            {
                PasswordModified = true;
                PasswordText = value;
            }
        }

        

        public bool IsSystemAccount
        {
            get
            {
                return string.IsNullOrEmpty(ServiceStartName) ||
                        ServiceStartName.Equals("LocalSystem", StringComparison.OrdinalIgnoreCase);
            }
        }

        public SC_START_TYPE StartType
        {
            get
            {
                return Config.StartType;
            }
            set
            {
                Config.StartType = value;
                StartTypeModified = true;
            }
        }

        public SC_SERVICE_TYPE ServiceType
        {
            get
            {
                return Config.ServiceType;
            }
            set
            {
                Config.ServiceType = value;
                ServiceTypeModified = true;
            }
        }

        public SC_ERROR_CONTROL ErrorControl
        {
            get
            {
                return Config.ErrorControl;
            }
            set
            {
                Config.ErrorControl = value;
                ErrorControlModified = true;
            }
        }


        public string Description
        {
            get
            {
                return DescriptionText;
            }
            set
            {
                DescriptionText = value;
                DescriptionModified = true;
            }
        }

        public string ServiceName
        {
            get
            {
                return GetText((int)ServiceItemTypes.ServiceName);
            }
        }

        public string LoadOrderGroup
        {
            get
            {
                return Config.LoadOrderGroup;
            }
            set
            {
                Config.LoadOrderGroup = value;
                LoadOrderGroupModified = true;
            }
        }

        public string ServiceStartName
        {
            get
            {
                return Config.ServiceStartName;
            }
            set
            {
                Config.ServiceStartName = value;
                ServiceStartNameModified = true;
            }
        }

        public string DisplayName
        {
            get
            {
                return GetText((int)ServiceItemTypes.DisplayName);
            }
            set
            {
                Objects[(int)ServiceItemTypes.DisplayName] = value;
                DisplayNameModified = true;
            }
        }

        public string ImagePath
        {
            get
            {
                return Config.BinaryPathName;
            }
            set
            {
                Config.BinaryPathName = value;
                BinaryPathNameModified = true;
            }

        }

        public ServiceObject()
        {
            Modified = false;
        }

        public bool Modified
        {
            get
            {
                return  StartTypeModified || DisplayNameModified || BinaryPathNameModified ||
                        DescriptionModified || ServiceTypeModified || ErrorControlModified ||
                        LoadOrderGroupModified || ServiceStartNameModified || PasswordModified;
            }
            set
            {
                StartTypeModified = value;
                LoadOrderGroupModified = value;
                DisplayNameModified = value;
                BinaryPathNameModified = value;
                DescriptionModified = value;
                PasswordModified = value;
                ServiceStartNameModified = value;
                ErrorControlModified = value;
                ServiceTypeModified = value;
            }
        }

        private static Dictionary<SC_SERVICE_TYPE, string> ServiceTypeStrings = null;

        public static string DescribeServiceType(SC_SERVICE_TYPE type)
        {
            StringBuilder result = new StringBuilder();
            bool first = true;

            if (ServiceTypeStrings == null)
            {
                ServiceTypeStrings = new Dictionary<SC_SERVICE_TYPE, string>();
                ServiceTypeStrings[SC_SERVICE_TYPE.SERVICE_DRIVER] = IDS.SERVICE_DRIVER;
                ServiceTypeStrings[SC_SERVICE_TYPE.SERVICE_WIN32] = IDS.SERVICE_WIN32;
                ServiceTypeStrings[SC_SERVICE_TYPE.SERVICE_FILE_SYSTEM_DRIVER] = IDS.SERVICE_FILE_SYSTEM_DRIVER;
                ServiceTypeStrings[SC_SERVICE_TYPE.SERVICE_KERNEL_DRIVER] = IDS.SERVICE_KERNEL_DRIVER;
                ServiceTypeStrings[SC_SERVICE_TYPE.SERVICE_WIN32_OWN_PROCESS] = IDS.SERVICE_WIN32_OWN_PROCESS;
                ServiceTypeStrings[SC_SERVICE_TYPE.SERVICE_WIN32_SHARE_PROCESS] = IDS.SERVICE_WIN32_SHARE_PROCESS;
                ServiceTypeStrings[SC_SERVICE_TYPE.SERVICE_INTERACTIVE_PROCESS] = IDS.SERVICE_INTERACTIVE_PROCESS;
            }

            foreach(SC_SERVICE_TYPE bitmask in ServiceTypeStrings.Keys)
            {
                if ((type & bitmask) != 0)
                {
                    if (first)
                        first = false;
                    else
                        result.Append("|");

                    result.Append(ServiceTypeStrings[bitmask]);
                }
            }

            return result.ToString();

        }

        public void Uninstall()
        {
            using (NativeSCManager SCM = new NativeSCManager())
            {
                using (NativeService ns = new NativeService(SCM,
                    ServiceName,
                    ACCESS_MASK.STANDARD_RIGHTS_REQUIRED))
                {
                    NativeServiceFunctions.DeleteService(ns.Handle);
                }
            }
        }

        public void RefreshConfig(NativeSCManager scm)
        {
            using (NativeService ns = new NativeService(scm, ServiceName))
            {
                Config = ns.ServiceConfig;

                Objects[(int)ServiceItemTypes.Start] = NativeServiceFunctions.DescribeStartType(Config.StartType);
                Objects[(int)ServiceItemTypes.Path] = Config.BinaryPathName;
                Objects[(int)ServiceItemTypes.LoadOrderGroup] = Config.LoadOrderGroup;
                Objects[(int)ServiceItemTypes.Type] = DescribeServiceType(Config.ServiceType);
                Objects[(int)ServiceItemTypes.ErrorControl] = NativeServiceFunctions.DescribeErrorControl(Config.ErrorControl);
                Objects[(int)ServiceItemTypes.TagId] = Config.TagId.ToString();
                Objects[(int)ServiceItemTypes.Dependencies] = Config.Dependencies;
                Objects[(int)ServiceItemTypes.Description] = ns.Description;
                Objects[(int)ServiceItemTypes.User] = Config.ServiceStartName;
                DescriptionText = ns.Description;
                if (Config.StartType == SC_START_TYPE.SERVICE_DISABLED)
                {
                    ForegroundColor = Color.Gray;
                }
            }
        }

        public bool ApplyChanges()
        {
            if (!Modified)
                return true;

            bool success = true;

            using (NativeSCManager SCM = new NativeSCManager())
            {
                using (NativeService ns = new NativeService(SCM,
                    ServiceName,
                    ACCESS_MASK.SERVICE_CHANGE_CONFIG | ACCESS_MASK.SERVICE_QUERY_STATUS))
                {
                    success = NativeServiceFunctions.ChangeServiceConfig(ns.Handle,
                        ServiceTypeModified ? Config.ServiceType : SC_SERVICE_TYPE.SERVICE_NO_CHANGE,
                        StartTypeModified ? Config.StartType : SC_START_TYPE.SERVICE_NO_CHANGE,
                        ErrorControlModified ? Config.ErrorControl : SC_ERROR_CONTROL.SERVICE_NO_CHANGE,
                        BinaryPathNameModified ? Config.BinaryPathName : null,
                        LoadOrderGroupModified ? Config.LoadOrderGroup : null,
                        null,
                        null,
                        ServiceStartNameModified ? Config.ServiceStartName : null,
                        PasswordModified ? PasswordText : null,
                        DisplayNameModified ? DisplayName : null);
                    if (success)
                    {
                        if (DescriptionModified)
                        {
                            SERVICE_DESCRIPTION sd = new SERVICE_DESCRIPTION();
                            sd.Description = DescriptionText;

                            const int cbBufSize = 8 * 1024;

                            IntPtr lpMemory = Marshal.AllocHGlobal((int)cbBufSize);
                            Marshal.StructureToPtr(sd, lpMemory, false);

                            NativeServiceFunctions.ChangeServiceConfig2(ns.Handle, SC_SERVICE_CONFIG.SERVICE_CONFIG_DESCRIPTION, lpMemory);

                            Marshal.FreeHGlobal(lpMemory);
                        }                       
                    }
                    Modified = false;
                    RefreshConfig(SCM);
                }
            }
            return success;
        }



        #region IServiceObject Members

        public object GetObject(int nID)
        {
            return Objects[nID];
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
            return Description;
        }

        public Color GetForegroundColor()
        {
            return ForegroundColor;
        }

        #endregion
    }
}
