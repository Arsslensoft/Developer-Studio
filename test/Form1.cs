using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        devstd.utils.DevStdTask t;
        private void buttonX1_Click(object sender, EventArgs e)
        {
           t = new devstd.utils.DevStdTask();
            t.Name = "EXPLORE";
            t.Description = "Exploring Files...";
            t.Executed += t_Executed;
            t.ShowTaskProgress = true;
            devstd.utils.TaskManager.ExecuteTask(t, new object[2] { "Hello",this });
        }
        delegate void Setz(string t);
        void SetText(string t)
        {
            if (this.InvokeRequired)
            {
                Setz s = new Setz(SetText);
                this.BeginInvoke(s, t);
            }
            else
                this.Text = t;
        }
        void t_Executed(params object[] para)
        {
           SetText( "aaaaa");
            MessageBox.Show(para[0].ToString());
            t.Description = "Part1 Explore";
            for (long i = 1; i < 50000; i++)
            {
                t.Description = i.ToString();
                Console.WriteLine();
            }
            t.Description = "Part2 Explore";
            for (long i = 1; i < 50000; i++)
            {
                t.Description = i.ToString();
                Console.WriteLine();
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            devstd.utils.TaskManager.StopTaskByName("EXPLORE");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100000;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
         BackgroundWorker bg =   (BackgroundWorker)sender;
          
            for (long i = 1; i < 50000; i++)
            {

                bg.ReportProgress((int)i,"alpha");
                Console.WriteLine();
            }

            for (long i = 1; i < 50000; i++)
            {
                bg.ReportProgress((int)i + 50000,"beta");
                Console.WriteLine();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            this.Text = e.UserState.ToString();
        }
    }
}
