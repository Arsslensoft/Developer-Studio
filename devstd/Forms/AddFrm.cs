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
    public partial class AddFrm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public AddFrm()
        {
            InitializeComponent();
        }
        public string ClassName
        {
            get { return textBoxX1.Text; }
        }
        public string ElementType
        {
            get { return comboBoxEx1.Text; }
        }
        bool isa= false;
        public bool IsAddControl
        {
            get { return isa; }
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            isa = true;
            this.Close();
        }
    }
}