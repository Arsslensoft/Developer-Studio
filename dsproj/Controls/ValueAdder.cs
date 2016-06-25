using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar;

namespace alfrmdesign
{
    public partial class ValueAdder : MetroForm
    {
        
        public ValueAdder()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (superValidator1.Validate())
            {
                if (!textBoxX2.Text.Contains("\"") && (comboBoxEx1.Text == "Public" || comboBoxEx1.Text == "Internal"))
                    this.Close();
                else if (textBoxX2.Text.Contains("\"") && (comboBoxEx1.Text == "Public" || comboBoxEx1.Text == "Internal"))
                {
                    MessageBoxEx.Show("Warning : The value you entered contains a quote, this can cause a problem in the code generation", "Value Adder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
                else
                    MessageBoxEx.Show("Wrong Value", "Value Adder", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBoxEx.Show("Wrong Value", "Value Adder", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
