using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using DevComponents.DotNetBar;

namespace alproj
{
    public partial class ProjectProperties : UserControl
    {
        public ProjectProperties()
        {
            InitializeComponent();
        }
        ALProject project = null;
        public void Init(ALProject proj)
        {
            try
            {
                project = proj;
                ressourceExplorerControl1.Init(proj);
                LoadSets();
            }
            catch
            {

            }
        }
        public void LoadSets()
        {
            try
            {
                checkedcheck.Checked=  project.Properties.Checked ;
                if (project.Properties.Debug)
                    targetbox.Text = "Debug";
                else
                    targetbox.Text = "Release";
                //project.Properties.Debug = (targetbox.Text == "Debug");
                delaysigncheck.Checked = project.Properties.DelaySign;
                docbox.Text = project.Properties.DocFile;
                mainbox.Text = project.Properties.EntryPoint;
                highentropycheck.Checked = project.Properties.HighEntropy;
                execiconbox.Text = project.Properties.IconFile;
                keyfilebox.Text = project.Properties.KeyFile;
                optimizecheck.Checked = project.Properties.Optimize;
                outputbox.Text = project.Properties.Output;
                platformbox.Text = project.Properties.Platform;
                prefbuildbox.Text = project.Properties.PreferedBuildLanguage;
                signcheck.Checked = project.Properties.Sign;
                if(project.Properties.Target == "winexe")
                   outputappbox.SelectedIndex =0;
                else if (project.Properties.Target == "exe")
                    outputappbox.SelectedIndex = 1;
                else
                    outputappbox.SelectedIndex =2;
                warnaserrorcheck.Checked = project.Properties.WarnAsError;
                warnlvlbox.SelectedIndex = project.Properties.WarnLevel;

                string syms = "";
                foreach (string sym in project.Properties.Symbols)
                    syms += sym + ",";
                if (syms.Length > 0)
                    syms = syms.Remove(syms.Length - 1, 1);
                symbox.Text = syms;


                syms = "";
                foreach (string sym in project.Properties.NoWarn)
                    syms += sym + ",";
                if (syms.Length > 0)
                    syms = syms.Remove(syms.Length - 1, 1);
             nowarnbox .Text = syms;

             syms = "";
             foreach (string sym in project.Properties.WarnAsErrorList)
                 syms += sym + ",";
             if (syms.Length > 0)
                 syms = syms.Remove(syms.Length - 1, 1);
          warnaserrorbox.Text = syms;
               
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Project Properties", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Save()
        {
            try
            {
                project.Properties.Checked = checkedcheck.Checked;
                project.Properties.Debug = (targetbox.Text == "Debug");
                project.Properties.DelaySign = delaysigncheck.Checked;
                project.Properties.DocFile = docbox.Text;
                project.Properties.EntryPoint = mainbox.Text;
                project.Properties.HighEntropy = highentropycheck.Checked;
                project.Properties.IconFile = execiconbox.Text;
                project.Properties.KeyFile = keyfilebox.Text;
                project.Properties.Optimize = optimizecheck.Checked;
                project.Properties.Output = outputbox.Text;
                project.Properties.Platform = platformbox.Text;
                project.Properties.PreferedBuildLanguage = prefbuildbox.Text;
                project.Properties.Sign = signcheck.Checked;
                if(outputappbox.SelectedIndex == 0)
                  project.Properties.Target = "winexe";
                else if (outputappbox.SelectedIndex == 1)
                    project.Properties.Target = "exe";
                else project.Properties.Target = "library";

                project.Properties.WarnAsError = warnaserrorcheck.Checked;
                project.Properties.WarnLevel = warnlvlbox.SelectedIndex;

                project.Properties.Symbols.Clear();
                foreach (string sym in symbox.Text.Split(','))
                    if(sym.Length > 0)
                      project.Properties.Symbols.Add(sym);

                project.Properties.NoWarn.Clear();
                foreach (string sym in this.nowarnbox.Text.Split(','))
                    if (sym.Length > 0)
                    project.Properties.NoWarn.Add(sym);

                project.Properties.WarnAsErrorList.Clear();
                foreach (string sym in this.warnaserrorbox.Text.Split(','))
                    if (sym.Length > 0)
                    project.Properties.WarnAsErrorList.Add(sym);

                project.Save();
            }
            catch(Exception ex)
            {
                MessageBoxEx.Show(ex.Message,"Project Properties", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
                execiconbox.Text = openFileDialog2.FileName;
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                execiconbox.Text = openFileDialog1.FileName;
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            try
            {
             Process p  =  Process.Start(Application.StartupPath + @"\MS_SDK\sn.exe"," -k \""+Path.GetDirectoryName(project.FileName)+@"\key.snk"+"\"");
             p.WaitForExit();
                if (File.Exists(Path.GetDirectoryName(project.FileName) + @"\key.snk"))
                    keyfilebox.Text = Path.GetDirectoryName(project.FileName) + @"\key.snk";
                else MessageBoxEx.Show("Key Generation Failed", "Key Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {

            }
        }
        public void SelectRessource()
        {
            this.superTabControl1.SelectedTab = resedittab;
        }
        public void SelectNormal()
        {
            this.superTabControl1.SelectedTab = apptab;
        }
        public event EventHandler OpenInfo;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (OpenInfo != null)
                OpenInfo(project, e);
        }

        public void OpenRessource(RessourceType res)
        {
            ressourceExplorerControl1.SelectTab(res);
        }
    }
 
}
