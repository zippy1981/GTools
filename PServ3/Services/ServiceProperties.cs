using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pserv3.Services
{
    public partial class ServiceProperties : Form
    {
        public readonly ServiceObject SO;
        private readonly string INTERNAL_PASSWORD_TEXT = "\x01\x08\x06\x00";

        public ServiceProperties(ServiceObject so, ServicesController sc)
        {
            InitializeComponent();

            SO = so;

            tbServiceName.Text = SO.ServiceName;
            tbDisplayName.Text = SO.DisplayName;
            tbDescription.Text = SO.Description;

            tbImagePath.Text = SO.ImagePath;
            cbStartupTypes.Items.Add(IDS.SERVICE_BOOT_START);
            cbStartupTypes.Items.Add(IDS.SERVICE_SYSTEM_START);
            cbStartupTypes.Items.Add(IDS.SERVICE_AUTO_START);
            cbStartupTypes.Items.Add(IDS.SERVICE_DEMAND_START);
            cbStartupTypes.Items.Add(IDS.SERVICE_DISABLED);
            cbStartupTypes.SelectedIndex = (int)SO.StartType;

            foreach (string log in sc.GetLoadOrderGroups())
            {
                cbLoadOrderGroup.Items.Add(log);
            }
            cbLoadOrderGroup.Text = SO.LoadOrderGroup;            

            tbPassword1.Text = INTERNAL_PASSWORD_TEXT;
            tbPassword2.Text = INTERNAL_PASSWORD_TEXT;

            
            if (SO.IsSystemAccount)
            {
                btSystemAccount.Checked = true;
                btSystemAccount_Click(null, null);
            }
            else
            {
                btThisAccount.Checked = true;
                tbAccountName.Text = SO.ServiceStartName;
                btThisAccount_Click(null, null);
            }

            if ((SO.ServiceType & SC_SERVICE_TYPE.SERVICE_INTERACTIVE_PROCESS) != 0)
            {
                btInteractWithDesktop.Checked = true;
            }
        }

        private void btAccept_Click(object sender, EventArgs e)
        {
            if (btThisAccount.Checked)
            {
                if (!tbPassword1.Text.Equals(tbPassword2.Text))
                {
                    MessageBox.Show("The new passwords don't match",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    tbPassword1.Text = "";
                    tbPassword2.Text = "";
                    tbPassword1.Focus();
                    return;
                }
                tbPassword2.Text = INTERNAL_PASSWORD_TEXT;
            }

            if (!tbDisplayName.Text.Equals(SO.DisplayName))
                SO.DisplayName = tbDisplayName.Text;
            if (!tbDescription.Text.Equals(SO.Description))
                SO.Description = tbDescription.Text;
            if (cbStartupTypes.SelectedIndex != (int)SO.StartType)
                SO.StartType = (SC_START_TYPE)cbStartupTypes.SelectedIndex;
            if (!tbImagePath.Text.Equals(SO.ImagePath))
                SO.ImagePath = tbImagePath.Text;

            if (btThisAccount.Checked)
            {
                if (!tbPassword1.Text.Equals(INTERNAL_PASSWORD_TEXT))
                {
                    SO.Password = tbPassword1.Text;
                }
                if (!tbAccountName.Text.Equals(SO.ServiceStartName))
                {
                    SO.ServiceStartName = tbAccountName.Text;
                }
            }
            else if (btSystemAccount.Checked)
            {
                if (!SO.IsSystemAccount)
                {
                    SO.ServiceStartName = "LocalSystem";
                    SO.Password = "";
                }
            }

            DialogResult = SO.Modified ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        private void btChooseImagePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            try
            {
                ofd.InitialDirectory = Path.GetDirectoryName(tbImagePath.Text);
                ofd.FileName = Path.GetFileName(tbImagePath.Text);
            }
            catch (Exception)
            {
            }
            ofd.CheckFileExists = true;
            ofd.Title = "Image Path";
            ofd.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {

            }

        }

        private void btThisAccount_Click(object sender, EventArgs e)
        {
            btInteractWithDesktop.Enabled = false;
            tbPassword1.Enabled = true;
            tbPassword2.Enabled = true;
            tbAccountName.Enabled = true;
        }

        private void btSystemAccount_Click(object sender, EventArgs e)
        {
            btInteractWithDesktop.Enabled = true;
            tbPassword1.Enabled = false;
            tbPassword2.Enabled = false;
            tbAccountName.Enabled = false;
        }
    }
}
