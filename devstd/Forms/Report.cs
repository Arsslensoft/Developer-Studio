using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Net;
using DevComponents.DotNetBar.Metro;

namespace devstd
{
    public partial class Report : MetroForm
    {
        public Exception err;
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {

            string[] lines = {"Application : " + Application.ProductName + " " + Application.ProductVersion,
                                 "Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                 "Computer name: " + SystemInformation.ComputerName,
                                  "Internet connexion : " + SystemInformation.Network.ToString(),
                                  "User name: " + SystemInformation.UserName,
                                  "OS: " + Environment.OSVersion.ToString(),
                                  "Culture: " + CultureInfo.CurrentCulture.Name,
                                  "Screen Resolution: " + SystemInformation.PrimaryMonitorSize.ToString(),
                                  "Application up time:       " + (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(),
        
                                 "Monitors : " + SystemInformation.MonitorCount.ToString(),
                                  "Mouse Speed : " + SystemInformation.MouseSpeed.ToString(),
                                  "Mouse Buttons : " + SystemInformation.MouseButtons.ToString(),
                                  "Loaded Modules : "
                             };

            lblErr.Text = err.StackTrace;

            foreach (string line in lines)
                lblErr.Text += Environment.NewLine + line;

        
           
            lblErr.Text += Environment.NewLine + "Error Message : " + err.Message.ToString() + Environment.NewLine +  "Stack Trace: " + err.StackTrace.ToString();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // you believe that you hacked Treco (in your dream)
        public void sendreport(string error)
        {
            string sysinfo = null;
            sysinfo = "Build : " + Environment.Version.Build.ToString() + " / Os version : " + Environment.OSVersion.Platform.ToString() + Environment.OSVersion.ServicePack.ToString() + "/ USERNAME : " + Environment.UserName.ToString() + "/ Machine name : " + Environment.MachineName.ToString() + "/ Fireweb Install path : " + Application.StartupPath + "/ ERROR";
            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("arsslensoft.errservice@gmail.com", "ar5s4r8f6f3e4f469");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            try
            {
                MailAddress maFrom = new MailAddress("arsslensoft.errservice@gmail.com", "Arsslensoft service", Encoding.UTF8);
                MailAddress maTo = new MailAddress("arsslensoft.errservice@gmail.com", "Arsslensoft Report", Encoding.UTF8);
                System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage(maFrom, maTo);
                mmsg.Body = "<html><body><h1>DS Professional IDE got an error</h1><p>" + error + "</p><p>" + sysinfo + "</p></body></html>";
                mmsg.BodyEncoding = Encoding.UTF8;
                mmsg.IsBodyHtml = true;
                mmsg.Subject = "Error";
                mmsg.SubjectEncoding = Encoding.UTF8;

                client.Send(mmsg);
                string a = Application.StartupPath + @"\report.brpt";
                File.WriteAllText(a, error);
                string l = "report" + error.Length.ToString() + ".brpt";
                File.Delete(a);
            }
            catch (Exception ex)
            {
                //throw;
                DevComponents.DotNetBar.MessageBoxEx.Show(ex.ToString() + Environment.NewLine + "try to send e-mail to : report@arsslensoft.tk", ex.Message);

            }

        }
        private void btnSend_Click(object sender, EventArgs e)
        {
sendreport(lblErr.Text);
        }
        public void WriteSoloReport(string member, Exception ex)
        {
            string header = "                                     Arsslensoft Crash Detection System                              ";
            string unheader = "                    Copyright (c) 2010-2013 Arsslensoft. All rights reserved            ";
            string hbody = "********************************************************************************************";
            string body = "SYS_INFO : " + Environment.NewLine + "OS : " + Environment.OSVersion.VersionString + Environment.NewLine + "Machine Name : " + Environment.MachineName + Environment.NewLine + " UserName : " + Environment.UserName + Environment.NewLine + " CLR :" + Environment.Version + Environment.NewLine + "Memory usage : " + Environment.WorkingSet + Environment.NewLine + "System directory : " + Environment.SystemDirectory;
            string ebody = "*********************************************************************************************";
            string reports = "Bug : " + member + Environment.NewLine + ex.StackTrace.ToString() + Environment.NewLine + ex.HelpLink.ToString();
            string[] lines = { header, unheader, hbody, body, ebody, reports,"Error Log : " ,File.ReadAllText(Application.StartupPath + @"\ELog.txt") };
            File.WriteAllLines(Application.StartupPath + @"\Report_" +DateTime.Now.ToFileTimeUtc().ToString()+ ".txt", lines);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
