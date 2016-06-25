using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace devstd.network
{
    public partial class AddBCAST : DevComponents.DotNetBar.Metro.MetroForm
    {
        public AddBCAST()
        {
            InitializeComponent();
        }
       
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (superValidator1.Validate())
                this.Close();
        }
    }
}