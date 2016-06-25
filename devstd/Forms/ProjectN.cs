using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using alproj;
using System.IO;

namespace devstd
{
    public partial class ProjectN : MetroForm
    {
        MainForm frm;
        string projdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DS15\Projects\";
        public bool AddProject = false;

        public ProjectN(MainForm f)
        {
            InitializeComponent();
            frm = f;
        }

        private void buttonItem13_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AddProject)
                {
                    if (frm.CSol != null)
                    {
                        ProjNameFrm pf = new ProjNameFrm();
                        pf.ShowDialog();
                        if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                        {
                            Directory.CreateDirectory(projdir + pf.ProjectName);


                            ALProject proj = alproj.ALProject.CreateConsole(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                            frm.solfile = frm.CSol.FileName;
                            frm.UnloadSolutionNew();
                        }
                        else
                        {
                            if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                            {

                                ALProject proj = alproj.ALProject.CreateConsole(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                                frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                                frm.solfile = frm.CSol.FileName;
                                frm.UnloadSolutionNew();
                            }
                        }
                    }
                    else
                    {
                        ProjNameFrm pf = new ProjNameFrm();
                        pf.ShowDialog();

                        if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                        {
                            Directory.CreateDirectory(projdir + pf.ProjectName);
                            ALProject proj = alproj.ALProject.CreateConsole(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                            frm.OpenSolution();
                        }
                        else
                        {
                            if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                            {
                                ALProject proj = alproj.ALProject.CreateConsole(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                                frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                                frm.OpenSolution();
                            }
                        }
                    }
                }
                else
                {

                    ProjNameFrm pf = new ProjNameFrm();
                    pf.ShowDialog();

                    if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                    {
                        Directory.CreateDirectory(projdir + pf.ProjectName);
                        ALProject proj = alproj.ALProject.CreateConsole(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                        frm.CSol.AddEditProject(proj, (byte)frm.CSol.Projects.Count, true);
                            frm.LoadSolutionControl();
                    }
                    else
                    {
                        if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                        {
                            ALProject proj = alproj.ALProject.CreateConsole(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol.AddEditProject(proj, (byte)frm.CSol.Projects.Count, true);
                            frm.LoadSolutionControl();
                        }
                    }


                }
            }
            catch
            {

            }
            finally
            {
                this.Close();
            }
        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AddProject)
                {
                    if (frm.CSol != null)
                    {
                        ProjNameFrm pf = new ProjNameFrm();
                        pf.ShowDialog();
                        if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                        {
                            Directory.CreateDirectory(projdir + pf.ProjectName);
                            ALProject proj = alproj.ALProject.CreateForm(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                            frm.solfile = frm.CSol.FileName;
                            frm.UnloadSolutionNew();
                        }
                        else
                        {
                            if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                            {
                                ALProject proj = alproj.ALProject.CreateForm(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                                frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                                frm.solfile = frm.CSol.FileName;
                                frm.UnloadSolutionNew();
                            }
                        }
                    }
                    else
                    {
                        ProjNameFrm pf = new ProjNameFrm();
                        pf.ShowDialog();

                        if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                        {
                            Directory.CreateDirectory(projdir + pf.ProjectName);
                            ALProject proj = alproj.ALProject.CreateForm(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                            frm.OpenSolution();
                        }
                        else
                        {
                            if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                            {
                                ALProject proj = alproj.ALProject.CreateForm(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                                frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                                frm.OpenSolution();
                            }
                        }
                    }
                }
                else
                {

                    ProjNameFrm pf = new ProjNameFrm();
                    pf.ShowDialog();

                    if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                    {
                        Directory.CreateDirectory(projdir + pf.ProjectName);
                        ALProject proj = alproj.ALProject.CreateForm(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                        frm.CSol.AddEditProject(proj, (byte)frm.CSol.Projects.Count, true);
                        frm.LoadSolutionControl();
                    }
                    else
                    {
                        if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                        {
                            ALProject proj = alproj.ALProject.CreateForm(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol.AddEditProject(proj, (byte)frm.CSol.Projects.Count, true);
                            frm.LoadSolutionControl();
                        }
                    }


                }
            }
            catch
            {

            }
            finally
            {
                this.Close();
            }
        }
        private void buttonItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AddProject)
                {
                    if (frm.CSol != null)
                    {
                        ProjNameFrm pf = new ProjNameFrm();
                        pf.ShowDialog();
                        if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                        {
                            Directory.CreateDirectory(projdir + pf.ProjectName);

                            ALProject proj = alproj.ALProject.CreateLibrary(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                            frm.solfile = frm.CSol.FileName;
                            frm.UnloadSolutionNew();
                        }
                        else
                        {
                            if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                            {

                                ALProject proj = alproj.ALProject.CreateLibrary(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                                frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                                frm.solfile = frm.CSol.FileName;
                                frm.UnloadSolutionNew();
                            }
                        }
                    }
                    else
                    {
                        ProjNameFrm pf = new ProjNameFrm();
                        pf.ShowDialog();

                        if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                        {
                            Directory.CreateDirectory(projdir + pf.ProjectName);
                            ALProject proj = alproj.ALProject.CreateLibrary(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                            frm.OpenSolution();
                        }
                        else
                        {
                            if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                            {
                                ALProject proj = alproj.ALProject.CreateLibrary(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                                frm.CSol = DSolution.CreateSolutionForProject(proj, frm.SolVersion);
                                frm.OpenSolution();
                            }
                        }
                    }
                }
                else
                {

                    ProjNameFrm pf = new ProjNameFrm();
                    pf.ShowDialog();

                    if (checkBoxItem1.Checked && !Directory.Exists(projdir + pf.ProjectName))
                    {
                        Directory.CreateDirectory(projdir + pf.ProjectName);
                        ALProject proj = alproj.ALProject.CreateLibrary(projdir + pf.ProjectName, pf.ProjectName, pf.ProjectTarget);
                        frm.CSol.AddEditProject(proj, (byte)frm.CSol.Projects.Count, true);
                        frm.LoadSolutionControl();
                    }
                    else
                    {
                        if (frm.folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(pf.ProjectName))
                        {
                            ALProject proj = alproj.ALProject.CreateLibrary(frm.folderBrowserDialog1.SelectedPath, pf.ProjectName, pf.ProjectTarget);
                            frm.CSol.AddEditProject(proj, (byte)frm.CSol.Projects.Count, true);
                            frm.LoadSolutionControl();
                        }
                    }


                }
            }
            catch
            {

            }
            finally
            {
                this.Close();
            }
        }
    }
}
