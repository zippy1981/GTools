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


using LUID = System.Int64;
using HANDLE = System.IntPtr;

namespace pserv3.Processes
{
    public class ProcessObject : IServiceObject
    {
        private readonly object[] Objects = new object[(int)ProcessItemTypes.MAX];
        private readonly string[] Text = new string[(int)ProcessItemTypes.MAX];
        public string ToolTipText;
        public Color ForegroundColor = Color.Black;
        public readonly Process P;

        public ProcessObject(Process p)
        {
            P = p;
            string path = GetSafeProcessName(p);
            ToolTipText = path;
            Objects[(int)ProcessItemTypes.Name] = p.ProcessName;
            Objects[(int)ProcessItemTypes.ID] = p.Id;
            Objects[(int)ProcessItemTypes.Path] = path;
            string username = GetUserInfo(p);
            Objects[(int)ProcessItemTypes.User] = username;
                        
            try
            {
                if (p.Id >= 10)
                {
                    string description = p.MainModule.FileVersionInfo.FileDescription;
                    Objects[(int)ProcessItemTypes.FileDescription] = description;
                    ToolTipText = description;
                    Objects[(int)ProcessItemTypes.FileVersion] = p.MainModule.FileVersionInfo.FileVersion;
                    Objects[(int)ProcessItemTypes.Product] = p.MainModule.FileVersionInfo.ProductName;
                    Objects[(int)ProcessItemTypes.ProductVersion] = p.MainModule.FileVersionInfo.ProductVersion;
                    Objects[(int)ProcessItemTypes.MainWindowHandle] = p.MainWindowHandle;
                    Objects[(int)ProcessItemTypes.MainWindowTitle] = p.MainWindowTitle;
                    Objects[(int)ProcessItemTypes.Responding] = p.Responding;

                    Objects[(int)ProcessItemTypes.StartTime] = p.StartTime.TimeOfDay;
                    Text[(int)ProcessItemTypes.StartTime] = Program.TimeSpanToString(p.StartTime.TimeOfDay);
                    Objects[(int)ProcessItemTypes.TotalRunTime] = DateTime.Now - p.StartTime;
                    Text[(int)ProcessItemTypes.StartTime] = Program.TimeSpanToString(DateTime.Now - p.StartTime);
                    Objects[(int)ProcessItemTypes.TotalProcessorTime] = p.TotalProcessorTime;
                    Text[(int)ProcessItemTypes.StartTime] = Program.TimeSpanToString(p.TotalProcessorTime);
                    Objects[(int)ProcessItemTypes.PrivilegedProcessorTime] = p.PrivilegedProcessorTime;
                    Text[(int)ProcessItemTypes.StartTime] = Program.TimeSpanToString(p.PrivilegedProcessorTime);
                    Objects[(int)ProcessItemTypes.ThreadCount] = p.Threads.Count;
                    Objects[(int)ProcessItemTypes.HandleCount] = p.HandleCount;

                    Objects[(int)ProcessItemTypes.ProcessPriorityClass] = p.PriorityClass;
                    Objects[(int)ProcessItemTypes.SessionId] = p.SessionId;
                    Objects[(int)ProcessItemTypes.StartInfoArguments] = p.StartInfo.Arguments;

                    
                    Objects[(int)ProcessItemTypes.NonpagedSystemMemorySize64] = p.NonpagedSystemMemorySize64;
                    Text[(int)ProcessItemTypes.NonpagedSystemMemorySize64] = Program.BytesToSize(p.NonpagedSystemMemorySize64);
                    Objects[(int)ProcessItemTypes.PagedMemorySize64] = p.PagedMemorySize64;
                    Text[(int)ProcessItemTypes.PagedMemorySize64] = Program.BytesToSize(p.PagedMemorySize64);
                    Objects[(int)ProcessItemTypes.PagedSystemMemorySize64] = p.PagedSystemMemorySize64;
                    Text[(int)ProcessItemTypes.PagedSystemMemorySize64] = Program.BytesToSize(p.PagedSystemMemorySize64);
                    Objects[(int)ProcessItemTypes.PeakPagedMemorySize64] = p.PeakPagedMemorySize64;
                    Text[(int)ProcessItemTypes.PeakPagedMemorySize64] = Program.BytesToSize(p.PeakPagedMemorySize64);
                    Objects[(int)ProcessItemTypes.PeakVirtualMemorySize64] = p.PeakVirtualMemorySize64;
                    Text[(int)ProcessItemTypes.PeakVirtualMemorySize64] = Program.BytesToSize(p.PeakVirtualMemorySize64);
                    Objects[(int)ProcessItemTypes.PeakWorkingSet64] = p.PeakWorkingSet64;
                    Text[(int)ProcessItemTypes.PeakWorkingSet64] = Program.BytesToSize(p.PeakWorkingSet64);
                    Objects[(int)ProcessItemTypes.PrivateMemorySize64] = p.PrivateMemorySize64;
                    Text[(int)ProcessItemTypes.PrivateMemorySize64] = Program.BytesToSize(p.PrivateMemorySize64);
                    Objects[(int)ProcessItemTypes.VirtualMemorySize64] = p.VirtualMemorySize64;
                    Text[(int)ProcessItemTypes.VirtualMemorySize64] = Program.BytesToSize(p.VirtualMemorySize64);
                    Objects[(int)ProcessItemTypes.WorkingSet64] = p.WorkingSet64;
                    Text[(int)ProcessItemTypes.WorkingSet64] = Program.BytesToSize(p.WorkingSet64);
                }
            }
            catch (Exception e)
            {
                GSharpTools.Tools.DumpException(e, "While accessing ProcessObject");
            }
            ForegroundColor = GetColorFromName(p, username);
        }

        public static Color GetColorFromName(Process p, string username)
        {
            if (p.Id < 10)
            {
                return Color.Gray;
            }
            if (username == null)
            {
                return Color.Red;
            }
            if (Environment.UserName.Equals(username))
            {
                return Color.Blue;
            }
            if (username.Equals(IDS.NativeAccount_SYSTEM, StringComparison.OrdinalIgnoreCase) ||
                username.Equals(IDS.LocalizedAccount_SYSTEM, StringComparison.OrdinalIgnoreCase))
            {
                return Color.Gray;
            }
            return Color.Black;
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

        public object GetObject(int nID)
        {
            return Objects[nID];
        }

        public string GetText(int nID)
        {
            if (Text[nID] != null)
                return Text[nID];

            object o = Objects[nID];
            if (o == null)
                return null;
            
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

        // Forward declarations

        public const int TOKEN_QUERY = 0X00000008;

        const int ERROR_NO_MORE_ITEMS = 259;

        enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId
        }

        [StructLayout(LayoutKind.Sequential)]
        struct TOKEN_USER
        {
            public _SID_AND_ATTRIBUTES User;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public int Attributes;
        }

        [DllImport("advapi32")]
        static extern bool OpenProcessToken(
            HANDLE ProcessHandle, // handle to process
            int DesiredAccess, // desired access to process
            ref IntPtr TokenHandle // handle to open access token
        );

        [DllImport("kernel32")]
        static extern HANDLE GetCurrentProcess();

        [DllImport("advapi32", CharSet=CharSet.Auto)]
        static extern bool GetTokenInformation(
            HANDLE hToken,
            TOKEN_INFORMATION_CLASS tokenInfoClass,
            IntPtr TokenInformation,
            int tokeInfoLength,
            ref int reqLength
        );

        [DllImport("kernel32")]
        static extern bool CloseHandle(HANDLE handle);

        [DllImport("advapi32", CharSet=CharSet.Auto)]
        static extern bool LookupAccountSid
        (
            [In,MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, // name of local or remote computer
            IntPtr pSid, // security identifier
            StringBuilder Account, // account name buffer
            ref int cbName, // size of account name buffer
            StringBuilder DomainName, // domain name
            ref int cbDomainName, // size of domain name buffer
            ref int peUse // SID type
            // ref _SID_NAME_USE peUse // SID type
        );

        [DllImport("advapi32", CharSet=CharSet.Auto)]
        static extern bool ConvertSidToStringSid(
            IntPtr pSID,
            [In,Out,MarshalAs(UnmanagedType.LPTStr)] ref string pStringSid);

        public static string GetUserInfo(Process p)
        {
            if (p.Id < 10)
                return IDS.LocalizedAccount_SYSTEM;
            string result = "";
            try
            {
                int Access = TOKEN_QUERY;
                HANDLE procToken = IntPtr.Zero;
                if (OpenProcessToken(p.Handle, Access, ref procToken))
                {
                    result = PerformDump(procToken);
                    CloseHandle(procToken);
                }
            }
            catch (Exception e)
            {
                GSharpTools.Tools.DumpException(e, "EXCEPTION CAUGHT in ProcessObject.GetUserInfo()");
            }
            return result;
        }

        static string PerformDump(HANDLE token)
        {
            StringBuilder sb = new StringBuilder();
            TOKEN_USER tokUser;
            const int bufLength = 256;
            IntPtr tu = Marshal.AllocHGlobal( bufLength );
            int cb = bufLength;
            GetTokenInformation( token, TOKEN_INFORMATION_CLASS.TokenUser, tu, cb, ref cb );
            tokUser = (TOKEN_USER) Marshal.PtrToStructure(tu, typeof(TOKEN_USER) );
            string result = DumpAccountSid(tokUser.User.Sid);
            Marshal.FreeHGlobal( tu );
            return result;
        }
        
        static string DumpAccountSid(IntPtr SID)
        {
            int cchAccount = 0;
            int cchDomain = 0;
            int snu = 0 ;
            string result = "";

            // Caller allocated buffer
            StringBuilder Account= null;
            StringBuilder Domain = null;
            bool ret = LookupAccountSid(null, SID, Account, ref cchAccount, Domain, ref cchDomain, ref snu);
            if (ret == true)
                if (Marshal.GetLastWin32Error() == ERROR_NO_MORE_ITEMS)
                    return "";
            try
            {
                Account = new StringBuilder( cchAccount );
                Domain = new StringBuilder( cchDomain );
                ret = LookupAccountSid(null, SID, Account, ref cchAccount, Domain, ref cchDomain, ref snu);
                if (ret)
                {
                    result = Account.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
            }
            return result;
        }
    }
}
