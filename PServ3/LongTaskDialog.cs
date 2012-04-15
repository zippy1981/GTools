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
    public partial class LongTaskDialog : Form
    {
        private readonly IServiceController Controller;

        public LongTaskDialog(IServiceController controller)
        {
            InitializeComponent();

            Controller = controller;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Controller.Refresh();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Controller.CancelRefresh();
        }
    }
}
