using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using System.IO;
using System.Diagnostics;


namespace devstd
{
    public partial class SourceN : MetroForm
    {
        public string FileWrit = null;
        void WriteSample(string code, string filter)
        {
            try
            {

                if (!checkBoxX1.Checked)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = filter;
                    sfd.Title = "Save Program";
                    if (sfd.ShowDialog() == DialogResult.OK)
                        File.WriteAllText(sfd.FileName, code);

                    FileWrit = sfd.FileName;
                }
                else
                {
                    if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DS15"))
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DS15");

               if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DS15\Sources"))
                   Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DS15\Sources");

               if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DS15\Projects"))
                   Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DS15\Projects");

             

                    string srcdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DS15\Sources\";

                    if (!Directory.Exists(srcdir + filter.Split('|')[1].Replace("*.", "").ToUpper()))
                        Directory.CreateDirectory(srcdir + filter.Split('|')[1].Replace("*.", "").ToUpper());

                    string filename = srcdir + filter.Split('|')[1].Replace("*.", "").ToUpper() + @"\" + "SRC"+Directory.GetFiles( srcdir + filter.Split('|')[1].Replace("*.", "").ToUpper() + @"\").Length.ToString() + "."+filter.Split('|')[1].Replace("*.", "");
                       if (!string.IsNullOrEmpty(textBoxX1.Text))
                           filename = srcdir + filter.Split('|')[1].Replace("*.", "").ToUpper() + @"\" + textBoxX1.Text + "."+filter.Split('|')[1].Replace("*.", "");

                    File.WriteAllText(filename, code);
                    FileWrit = filename;
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
    
        public SourceN()
        {
          
            InitializeComponent();
        }

        private void pascalbtn_Click(object sender, EventArgs e)
        {
            WriteSample("program programname; "+ Environment.NewLine + "uses crt;"+Environment.NewLine +Environment.NewLine+ "begin"+Environment.NewLine + Environment.NewLine + "end.", "(*.pas)|*.pas|(*.pascal)|*.pascal|(*.pp)|*.pp");

        }
        private void pascalhbtn_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.PawnCode, "(*.pwn)|*.pwn");
        }
        private void metroTileItem3_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.CScode, "(*.cs)|*.cs");
        }
        private void metroTileItem4_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.VBCode, "(*.vb)|*.vb");
        }
        private void metroTileItem1_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.ASPCode, "(*.asp)|*.asp");
        }
        private void metroTileItem2_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.Ccode, "(*.c)|*.c");
        }
        private void metroTileItem5_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.Ccode, "(*.cpp)|*.cpp");
        }
        private void metroTileItem6_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.ASMcode, "(*.s)|*.s");
        }
        private void metroTileItem7_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.HTMLcode, "(*.html)|*.html");
        }
        private void metroTileItem8_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.jscode, "(*.js)|*.js");
        }
        private void metroTileItem9_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.PHPcode, "(*.php)|*.php");
        }
        private void metroTileItem10_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.Xmlcode, "(*.xml)|*.xml");
        }
        private void metroTileItem11_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.SQLcode, "(*.sql)|*.sql");
        }

        private void metroTileItem12_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.JavaCode, "(*.java)|*.java");
        }

        private void SourceN_Load(object sender, EventArgs e)
        {
            int tc = 15;
   
        }

        private void metroTileItem14_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.Powershellcode, "(*.ps1)|*.ps1");
        }

        private void metroTileItem16_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.ALCode, "(*.al)|*.al");
        }

        private void metroTileItem13_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.CSSCode, "(*.css)|*.css");
        }

        private void metroTileItem15_Click(object sender, EventArgs e)
        {
            WriteSample(devstd.Properties.Resources.BooCode, "(*.boo)|*.boo");
        }
        //void t_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        MetroTileItem it = (MetroTileItem)sender;
        //        WriteSample(it.Tag.ToString(), "*"+it.Name + "|*" + it.Name);

        //    }
        //    catch
        //    {

        //    }
        //}
    }
}
