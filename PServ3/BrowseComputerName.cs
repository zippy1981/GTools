using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pserv3
{
    public partial class BrowseComputerName : Form
    {
        public BrowseComputerName()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (CanConnect(tbComputerName.Text))
            {
                Services.ServicesController.MachineName = tbComputerName.Text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(string.Format("Unable to connect to '{0}', try something else",tbComputerName.Text),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                tbComputerName.Focus();
            }

        }

        private bool CanConnect(string name)
        {
            try
            {
                using (Services.NativeSCManager scm = new Services.NativeSCManager(name))
                {
                    return true;
                }
            }
            catch(Exception e)
            {
                GSharpTools.Tools.DumpException(e, "unable to connect to {0}", name);                
            }
            return false;
        }
    }
}
