using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using devstd.utils;

namespace devstd
{
    public partial class SettingsForm : MetroForm
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            SettingsManager.Init();
            if (SettingsManager.GetString("LANG") == "FR")
            {
                comboBoxEx1.SelectedItem = comboItem2;
            }
            else if (SettingsManager.GetString("LANG") == "AR")
            {
                comboBoxEx1.SelectedItem = comboItem3;
            }
            else
            {
                comboBoxEx1.SelectedItem = comboItem1;
            }
           // checkBoxX3.Checked = SettingsManager.GetBool("LOADPLUGIN");
            checkBoxX1.Checked = SettingsManager.GetBool("PARSE");
            intelcheck.Checked = SettingsManager.GetBool("INTELLISENSE");

         
            stmsgcheck.Checked = SettingsManager.GetBool("STARTUPMSG");
            checkBoxX2.Checked = SettingsManager.GetBool("UPDATE");
         //   textBoxX2.Text = SettingsManager.GetString("VTAPI");
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            SettingsManager.SetBool("INTELLISENSE", intelcheck.Checked);
            SettingsManager.SetBool("UPDATE", checkBoxX2.Checked);
         //   SettingsManager.SetBool("LOADPLUGIN", checkBoxX3.Checked);
            SettingsManager.SetBool("STARTUPMSG", stmsgcheck.Checked);
                  SettingsManager.SetBool("PARSE", checkBoxX1.Checked);

          //  SettingsManager.SetString("VTAPI", textBoxX2.Text);
            SettingsManager.SetString("COMPCONFIG", " -d basepath=\"$.curdir$\\Pascal\" -o \"$.curdir$\\Pascal\\bin\\i386-win32\\fpc.cfg\"");

            if (comboBoxEx1.SelectedItem == comboItem2)
            {
                SettingsManager.SetString("LANG", "FR");
            }
            else if (comboBoxEx1.SelectedItem == comboItem3)
            {
                SettingsManager.SetString("LANG", "AR");
            }
            else
            {
                SettingsManager.SetString("LANG", "EN");
            }

            SettingsManager.Save();

            this.Close();

        }

      }
}
