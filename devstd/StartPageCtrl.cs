using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevComponents.DotNetBar;

namespace devstd
{
    public partial class StartPageCtrl : UserControl
    {
        public event EventHandler OnNewSolClick;
        public event EventHandler OnOpenSolClick;
        public event FileWrittenHandler OnFileWritten;
        public StartPageCtrl()
        {
            InitializeComponent();
            newSourceCtrl1.OnFileWritten += newSourceCtrl1_OnFileWritten;
        }
        void newSourceCtrl1_OnFileWritten(string file)
        {
            OnFileWritten(file);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnNewSolClick(sender, EventArgs.Empty);
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnOpenSolClick(sender, EventArgs.Empty);
        }


    }
}
