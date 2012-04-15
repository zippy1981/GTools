using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace pserv3.Uninstaller
{
    public partial class UninstallerProperties : Form
    {
        private readonly UninstallerController Controller;
        private readonly UninstallerObject Object;

        public UninstallerProperties(UninstallerObject uo, UninstallerController uc)
        {
            InitializeComponent();

            Controller = uc;
            Object = uo;

            tbAboutLink.Text = uo.AboutLink;
            if (string.IsNullOrEmpty(tbAboutLink.Text))
                btAboutLink.Enabled = false;
            tbHelpLink.Text = uo.HelpLink;
            if (string.IsNullOrEmpty(tbHelpLink.Text))
                btHelpLink.Enabled = false;
            tbName.Text = uo.Application;
            tbPath.Text = uo.Path;
            tbPublisher.Text = uo.Publisher;
            tbKey.Text = uo.Key;
            tbAction.Text = uo.Action;
        }

        private void btHelpLink_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(tbHelpLink.Text);
            }
            catch (Exception)
            {

            }
        }

        private void btAboutLink_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(tbAboutLink.Text);
            }
            catch (Exception)
            {

            }            
        }

        private void tbHelpLink_TextChanged(object sender, EventArgs e)
        {
            btHelpLink.Enabled = !string.IsNullOrEmpty(tbHelpLink.Text);
        }

        private void tbAboutLink_TextChanged(object sender, EventArgs e)
        {
            btAboutLink.Enabled = !string.IsNullOrEmpty(tbAboutLink.Text);
        }
    }
}
