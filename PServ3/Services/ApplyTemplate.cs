using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace pserv3.Services
{
    public partial class ApplyTemplate : Form
    {
        private readonly List<IServiceObject> Services;
        private readonly Dictionary<string, ServiceActionTemplate> Actions;
        private readonly IServiceView ServiceView;
        private readonly SC_START_TYPE[] DefaultStartTypes = {
            SC_START_TYPE.SERVICE_NO_CHANGE,
            SC_START_TYPE.SERVICE_AUTO_START,
            SC_START_TYPE.SERVICE_BOOT_START,
            SC_START_TYPE.SERVICE_DEMAND_START,
            SC_START_TYPE.SERVICE_DISABLED,
            SC_START_TYPE.SERVICE_SYSTEM_START
        };

        public ApplyTemplate(List<IServiceObject> services, Dictionary<string, ServiceActionTemplate> actions, IServiceView serviceView)
        {
            InitializeComponent();
            Services = services;
            Actions = actions;
            ServiceView = serviceView;

            columnHeader1.Text = IDS.ApplyServiceTemplate_Column_Service;
            columnHeader2.Text = IDS.ApplyServiceTemplate_Column_Action;
            btOK.Text = IDS.ApplyServiceTemplate_Button_OK;
            btCancel.Text = IDS.ApplyServiceTemplate_Button_Cancel;
            label1.Text = IDS.ApplyServiceTemplate_Label_ChooseDefaultAction;
            Text = IDS.ApplyServiceTemplate_Dialog_Caption;

            cbDefaultAction.Sorted = false;
            cbDefaultAction.Items.Add(IDS.ApplyServiceTemplate_DefaultAction_SERVICE_NO_CHANGE);
            cbDefaultAction.Items.Add(IDS.ApplyServiceTemplate_DefaultAction_SERVICE_AUTO_START);
            cbDefaultAction.Items.Add(IDS.ApplyServiceTemplate_DefaultAction_SERVICE_BOOT_START);
            cbDefaultAction.Items.Add(IDS.ApplyServiceTemplate_DefaultAction_SERVICE_DEMAND_START);
            cbDefaultAction.Items.Add(IDS.ApplyServiceTemplate_DefaultAction_SERVICE_DISABLED);
            cbDefaultAction.Items.Add(IDS.ApplyServiceTemplate_DefaultAction_SERVICE_SYSTEM_START);
            cbDefaultAction.SelectedIndex = 0;

            foreach (IServiceObject service in services)
            {
                ServiceObject so = service as ServiceObject;
                if (so != null)
                {
                    string key = so.ServiceName.ToLower();
                    if (actions.ContainsKey(key))
                    {
                        ServiceActionTemplate template = actions[key];
                        Debug.Assert(template != null);

                        if (template.StartType != so.StartType)
                        {
                            ListViewItem lvi = listView1.Items.Add(new ListViewItem(so.DisplayName));
                            lvi.SubItems.Add(template.StartType.ToString());
                            lvi.Tag = so;                            
                        }
                    }
                    else // use default action for this
                    {
                        ListViewItem lvi = listView1.Items.Add(new ListViewItem(so.DisplayName));
                        lvi.SubItems.Add("- apply default action -");
                        lvi.Tag = so;
                    }
                }
            }
        }

        private void ApplySpecificAction(ServiceObject so, SC_START_TYPE st)
        {
        }

        private void ApplyDefaultAction(ServiceObject so)
        {
            ApplySpecificAction(so, DefaultStartTypes[cbDefaultAction.SelectedIndex]);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                ServiceObject so = lvi.Tag as ServiceObject;
                Debug.Assert(so != null);

                string key = so.ServiceName.ToLower();
                if (Actions.ContainsKey(key))
                {
                    ServiceActionTemplate template = Actions[key];
                    Debug.Assert(template != null);
                    Debug.Assert(template.StartType != so.StartType);

                    ApplySpecificAction(so, template.StartType);

                    so.StartType = template.StartType;
                    so.ApplyChanges();
                }
                else 
                {
                    ApplyDefaultAction(so);
                }
            }
            ServiceView.UpdateDisplay();
        }
    }
}
