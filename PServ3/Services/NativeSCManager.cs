using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using GSharpTools;
using System.Text;

namespace pserv3.Services
{
    public class NativeSCManager : IDisposable
    {
        public IntPtr Handle;

        public NativeSCManager()
            : this(ServicesController.MachineName)
        {
        }

        public NativeSCManager(string machineName)
        {
            if (string.IsNullOrEmpty(machineName))
                machineName = Environment.MachineName;

            Handle = NativeServiceFunctions.OpenSCManager(
                machineName,
                NativeServiceFunctions.SERVICES_ACTIVE_DATABASE,
                (uint)(ACCESS_MASK.STANDARD_RIGHTS_READ | ACCESS_MASK.GENERIC_READ));
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!NativeServiceFunctions.CloseServiceHandle(Handle))
            {
                Trace.WriteLine("Warning, unable to close ServiceControlManager.Handle");
            }
        }

        #endregion

        public void Refresh(List<IServiceObject> Services, SC_SERVICE_TYPE ServiceType)
        {
            // Quote from MSDN: Windows Server 2003 and Windows XP:  
            // The maximum size of this array is 64K bytes. This limit
            // was increased as of Windows Server 2003 SP1 and Windows XP SP2.

            const int BytesAllocated = 63 * 1024;

            IntPtr lpServices = Marshal.AllocHGlobal((int)BytesAllocated);
            int cbBytesNeeded = 0;
            int ServicesReturned = 0;
            int ResumeHandle = 0;

            while (true)
            {
                if (NativeServiceFunctions.EnumServicesStatusEx(Handle,
                        SC_ENUM_TYPE.SC_ENUM_PROCESS_INFO,
                        ServiceType,
                        SC_QUERY_SERVICE_STATUS.SERVICE_STATE_ALL,
                        lpServices,
                        BytesAllocated,
                        ref cbBytesNeeded,
                        ref ServicesReturned,
                        ref ResumeHandle,
                        null))
                {
                    Trace.TraceInformation("Got {0} services in last chunk", ServicesReturned);
                    Refresh(Services, lpServices, ServicesReturned);
                    break;
                }
                else
                {
                    int LastError = Marshal.GetLastWin32Error();
                    if (LastError == NativeServiceFunctions.ERROR_MORE_DATA)
                    {
                        Trace.TraceInformation("Got {0} services in this chunk", ServicesReturned);
                        Refresh(Services, lpServices, ServicesReturned);
                    }
                    else
                    {
                        Trace.TraceInformation("ERROR {0}, unable to query list", LastError);
                        break;
                    }
                }
            }
        }

        public void Refresh(List<IServiceObject> Services, IntPtr lpServices, int ServicesReturned)
        {
            int iPtr = lpServices.ToInt32();
            for (int i = 0; i < ServicesReturned; i++)
            {
                ENUM_SERVICE_STATUS_PROCESS essp = (ENUM_SERVICE_STATUS_PROCESS)
                    Marshal.PtrToStructure(
                        new IntPtr(iPtr),
                        typeof(ENUM_SERVICE_STATUS_PROCESS));

                ServiceObject so = new ServiceObject();
                Services.Add(so);

                so.Objects[(int)ServiceItemTypes.DisplayName] = essp.DisplayName;
                so.Objects[(int)ServiceItemTypes.ServiceName] = essp.ServiceName;
                so.Objects[(int)ServiceItemTypes.Status] = NativeServiceFunctions.DescribeRuntimeStatus(essp.CurrentState);

                so.CurrentState = essp.CurrentState;
                so.ControlsAccepted = essp.ControlsAccepted;

                so.Objects[(int)ServiceItemTypes.Type] = ServiceObject.DescribeServiceType(essp.ServiceType);
                so.Objects[(int)ServiceItemTypes.PID] = essp.ProcessID;

                if (essp.CurrentState == SC_RUNTIME_STATUS.SERVICE_RUNNING)
                {
                    so.ForegroundColor = Color.Blue;
                }

                so.RefreshConfig(this);

                iPtr += Marshal.SizeOf(essp);
            }
        }

    }
}
