using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using pserv3.Services;
using GSharpTools;

namespace pserv3.Services
{
    partial class ChangeServiceStatus : Form
    {
        private readonly ServiceStateRequest SSR;
        private readonly List<ServiceObject> Services = new List<ServiceObject>();
        private NativeSCManager SCM;
        private double PercentPerService;
        private double CurrentPercentage;
        private readonly IServiceView ServiceView;
        private bool bCancelled = false;
        private delegate void StatusMessageDelegate(ServiceObject so, SC_RUNTIME_STATUS state);

        private void StatusMessage(ServiceObject so, SC_RUNTIME_STATUS state)
        {
            if (so != null)
            {
                stServiceName.Text = so.GetText((int)ServiceItemTypes.DisplayName);
                stOldStatus.Text = NativeServiceFunctions.DescribeRuntimeStatus(so.CurrentState);
            }
            stNewStatus.Text = NativeServiceFunctions.DescribeRuntimeStatus(state);
        }

        public ChangeServiceStatus(IServiceView serviceView, IEnumerable<ServiceObject> selectedItems, ServiceStateRequest ssr)
        {
            Trace.TraceInformation("ChangeServiceStatus() dialog created for {0}", ssr); 
            SSR = ssr;
            ServiceView = serviceView;
            int nItem = 0;
            foreach (ServiceObject so in selectedItems)
            {
                Services.Add(so);
                Trace.TraceInformation("- Item {0}: {1}", nItem++, so);
            }
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs args)
        {
            if (Services.Count > 0)
            {
                CurrentPercentage = 0.0;
                PercentPerService = 100.0 / Services.Count;

                ACCESS_MASK ServiceAccessMask = SSR.GetServiceAccessMask() | ACCESS_MASK.STANDARD_RIGHTS_READ | ACCESS_MASK.SERVICE_QUERY_STATUS;
                
                using (SCM = new NativeSCManager())
                {
                    int ServiceIndex = 0;
                    foreach (ServiceObject so in Services)
                    {
                        try
                        {
                            Trace.TraceInformation("Invoke StatusMessage '{0}', {1}", so, so.CurrentState);
                            Invoke(new StatusMessageDelegate(StatusMessage), so, so.CurrentState);
                            CurrentPercentage = ServiceIndex * (100.0 / Services.Count);
                            using (NativeService ns = new NativeService(SCM, so.GetText((int)ServiceItemTypes.ServiceName), ServiceAccessMask))
                            {
                                backgroundWorker1_Process(ns, so);
                            }
                            ++ServiceIndex;
                        }
                        catch (Exception e)
                        {
                            GSharpTools.Tools.DumpException(e, "ChangeServiceStatus.backgroundWorker1_DoWork");
                        }
                        if (bCancelled)
                            break;
                    }                    
                }
            }
            //backgroundWorker1.ReportProgress(100);
        }

        private void backgroundWorker1_Process(NativeService ns, ServiceObject so)
        {
            bool requestedStatusChange = false;

            Trace.TraceInformation("BEGIN backgroundWorker1_Process for {0}", ns.Description);

            using (ServiceStatus ss = new ServiceStatus(ns))
            {
                for (int i = 0; i < 100; ++i)
                {
                    if (bCancelled)
                        break;
                    
                    double RelativePercentage = (PercentPerService * (double)i) / 10.0;
                    double Percentage = CurrentPercentage + RelativePercentage;

                    int p = (int)Percentage;
                    if (p >= 100)
                        p = 99;
                    backgroundWorker1.ReportProgress(p);

                    if (!ss.Refresh())
                        break;

                    Trace.TraceInformation("Refresh #{0}: Status is {1}", i, ss.Status.CurrentState);
                    Invoke(new StatusMessageDelegate(StatusMessage), null, ss.Status.CurrentState);
                    if (SSR.HasSuccess(ss.Status.CurrentState))
                    {
                        Trace.WriteLine("Reached target status, done...");
                        break; // TODO: reached 100% of this service' status reqs. 
                    }

                    // if we haven't asked the service to change its status yet, do so now. 
                    if (!requestedStatusChange)
                    {
                        requestedStatusChange = true;
                        Trace.TraceInformation("Ask {0} to issue its status request on {1}", SSR, ss);
                        if (!SSR.Request(ss))
                            break;
                    }
                    else if (SSR.HasFailed(ss.Status.CurrentState))
                    {
                        Trace.TraceInformation("ERROR, target state is one of the failed ones :(");
                        break;
                    }
                    Thread.Sleep(500);
                }
                ss.Modify(so);
                Trace.TraceInformation("END backgroundWorker1_Process");
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Trace.TraceInformation("backgroundWorker1_RunWorkerCompleted called");
            Close();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            bCancelled = true;
        }

        private void ChangeServiceStatus_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
