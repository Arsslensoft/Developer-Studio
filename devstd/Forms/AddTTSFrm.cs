using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace devstd
{
    public partial class AddTTSFrm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public AddTTSFrm()
        {
            InitializeComponent();
        }
        public bool Said = false;
        private void buttonX1_Click(object sender, EventArgs e)
        {

            Said = true;
            this.Close();
        }

        private void AddTTSFrm_Load(object sender, EventArgs e)
        {

        }
    }
}