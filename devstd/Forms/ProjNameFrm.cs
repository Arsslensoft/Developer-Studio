using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using alproj;

namespace devstd
{
    public partial class ProjNameFrm : MetroForm
    {
        public ProjNameFrm()
        {
            InitializeComponent();
        }

        public string ProjectName
        {
            get { return textBoxX1.Text; }
        }
        public TargetFramework ProjectTarget
        {
            get
            {
                foreach (TargetFramework fx in Framework.Targets)
                    if (fx.Name == comboBoxEx1.Text)
                        return fx;

                return Framework.Targets[0];
            }
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }
    }
}
