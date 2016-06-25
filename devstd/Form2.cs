using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Threading;

namespace devstd
{
    delegate void CloseV();
    public partial class Form2 : Office2007RibbonForm
    {
       
        public Form2()
        {
            
            InitializeComponent();
        }
  
        private void Form2_Load(object sender, EventArgs e)
        {
            if (Program.Initialized)
                this.Close();

            Programs.CloseSplashScreen += new EventHandler(Programs_CloseSplashScreen);
        }
       public void CloseF()
        {
            this.Close();
        }
        void Programs_CloseSplashScreen(object sender, EventArgs e)
        {
       
            this.BeginInvoke(new CloseV(CloseF));

        }
    }
}
