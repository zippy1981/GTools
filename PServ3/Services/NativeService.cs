using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using GSharpTools;

namespace pserv3.Services
{
    public class NativeService : IDisposable
    {
        public IntPtr Handle;

        public NativeService(NativeSCManager scm, string ServiceName, ACCESS_MASK am)
        {
            Handle = NativeServiceFunctions.OpenService(
                scm.Handle,
                ServiceName,
                (uint)am);
        }

        public NativeService(NativeSCManager scm, string ServiceName)
        {
            Handle = NativeServiceFunctions.OpenService(
                scm.Handle, 
                ServiceName, 
                (uint)(ACCESS_MASK.STANDARD_RIGHTS_READ | ACCESS_MASK.GENERIC_READ));
        }

        public QUERY_SERVICE_CONFIG ServiceConfig
        {
            get
            {
                const int cbBufSize = 8 * 1024;
                int cbBytesNeeded = 0;

                IntPtr lpMemory = Marshal.AllocHGlobal((int)cbBufSize);

                if (NativeServiceFunctions.QueryServiceConfig(Handle, lpMemory, cbBufSize, ref cbBytesNeeded))
                {
                    return (QUERY_SERVICE_CONFIG)
                        Marshal.PtrToStructure(
                            new IntPtr(lpMemory.ToInt32()),
                            typeof(QUERY_SERVICE_CONFIG));
                }
                return null;
            }
        }

        public string Description
        {
            get
            {
                const int cbBufSize = 8 * 1024;
                int cbBytesNeeded = 0;

                IntPtr lpMemory = Marshal.AllocHGlobal((int)cbBufSize);

                if (NativeServiceFunctions.QueryServiceConfig2(
                        Handle, 
                        SC_SERVICE_CONFIG_INFO_LEVEL.SERVICE_CONFIG_DESCRIPTION, 
                        lpMemory, 
                        cbBufSize, 
                        out cbBytesNeeded))
                {
                    SERVICE_DESCRIPTION sd = (SERVICE_DESCRIPTION)
                        Marshal.PtrToStructure(
                            new IntPtr(lpMemory.ToInt32()),
                            typeof(SERVICE_DESCRIPTION));
                    return sd.Description;
                }
                return null;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!NativeServiceFunctions.CloseServiceHandle(Handle))
            {
                Trace.WriteLine("Warning, unable to close NativeService.Handle");
            }
        }

        #endregion
    }
}
