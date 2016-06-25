using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using JSS;
using System.Threading;
using System.Diagnostics;
using devstd.utils;

namespace devstd
{
    public partial class Checkerfrm : MetroForm
    {
        string exec;
        public Checkerfrm(string exe)
        {
            exec = exe;
            InitializeComponent();
        }
        string url = "";
        private void Checkerfrm_Shown(object sender, EventArgs e)
        {
            circularProgress1.Visible = true;
            circularProgress1.Value = 0;
            labelX1.Text = "                 Checking file...             ";
            backgroundWorker1.RunWorkerAsync();

        }
        string vir = "";
        bool r = false;
        void Check()
        {
            try
            {
                circularProgress1.IsRunning = true;
                VirusTotal.APIKey = SettingsManager.GetString("VTAPI");
                VirusTotal.Initialize(SettingsManager.GetString("VTAPI"));
                VT.Init();
                result = VirusTotal.Scan(exec);
           r= VT.Check(result,out vir);
           url = "https://www.virustotal.com/file/" + result.Split('-')[0].Replace("\r\n", "").Replace(" ", "") + "/analysis/";
            }
            catch(Exception ex)
            {
                ELog.LogEx(ex);
            }
    }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
 
            Thread thr = new Thread(new ThreadStart(Check));
            thr.Start();
            thr.Join();
        }
        string result = null;
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (r)
            {
                circularProgress1.IsRunning = false;
                circularProgress1.Value = 100;
                circularProgress1.ProgressColor = Color.Red;
                linkLabel1.Visible = true;
                labelX1.Text = "  File is not safe : " +vir;
            }
            else
            {
                circularProgress1.IsRunning = false;
                circularProgress1.Value = 100;
                circularProgress1.ProgressColor = Color.Green;
                labelX1.Text = "  File is safe           ";
            }
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {

            }
        }

    }
}
