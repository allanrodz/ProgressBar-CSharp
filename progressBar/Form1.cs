using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace progressBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(!bgWorder.IsBusy)
            {
                bgWorder.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please wait until finished", "worker running", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if(bgWorder.IsBusy)
            {
                bgWorder.CancelAsync();
            }
        }

        private void bgWorder_DoWork(object sender, DoWorkEventArgs e)
        {
            int sum = 0;
            for(int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                sum++;
                bgWorder.ReportProgress(i);
                if(bgWorder.CancellationPending)
                {
                    e.Cancel = true;
                    bgWorder.ReportProgress(0);
                    return;
                }

                e.Result = sum;
            }
        }

        private void bgWorder_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
            lblDisplay.Text = e.ProgressPercentage.ToString()+ "%";
        }

        private void bgWorder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled)
            {
                lblDisplay.Text = "Progress Cancelled";
            }
            else if(e.Error != null)
            {
                lblDisplay.Text = e.Error.Message;
            }
            else
            {
                lblDisplay.Text = "Total " + e.Result.ToString() + "%";
            }
        }
    }
}
