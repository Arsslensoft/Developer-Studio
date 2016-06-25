using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Reflection;
using System.Diagnostics;
using DevComponents.DotNetBar.Metro;

namespace devstd
{
    public partial class ReferenceAdder : MetroForm
    {
        public ReferenceAdder()
        {
            InitializeComponent();
        }
        System.Collections.Generic.Dictionary<string, string> GetAssemblies()
        {
            System.Collections.Generic.Dictionary<string, string> Asm = new System.Collections.Generic.Dictionary<string, string>();
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.Arguments = "/l";
            proc.FileName = Application.StartupPath + @"\gacutil.exe";
            proc.RedirectStandardOutput = true;
            proc.UseShellExecute = false;
            proc.CreateNoWindow = true;

            Process p = Process.Start(proc);

            p.WaitForExit(1000);
            //Nombre d'éléments <- Global Assembly Cache 3
            string[] l = p.StandardOutput.ReadToEnd().Replace("\r", "").Split('\n');

            int i = 4;
            while (!l[i].StartsWith("Nombre ") && i < l.Length)
            {
                if (l[i].Contains(",") == true)
                    Asm.Add(l[i], l[i].Remove(0, 2).Split(',')[0]);
                i++;
            }
            return Asm;
        }
        private void ReferenceAdder_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (KeyValuePair<string, string> p in GetAssemblies())
                {
                    ListViewItem item = messagesListView.Items.Add(new ListViewItem(p.Value));
                    item.SubItems.Add(p.Key);
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       public List<Assembly> ToAdd;
       public bool CopyAsm;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                ToAdd = new List<Assembly>();
                foreach (ListViewItem item in messagesListView.SelectedItems)
                {
                    Assembly asm = Assembly.Load(item.SubItems[1].Text);
                    ToAdd.Add(asm);
                }
                CopyAsm = checkBoxX1.Checked;
                this.Close();
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            try
            {
                ToAdd = new List<Assembly>();
                if(openFileDialog1.ShowDialog()== DialogResult.OK)
                {
                    Assembly asm = Assembly.LoadFrom(openFileDialog1.FileName);
                    ToAdd.Add(asm);
              
                CopyAsm = true;
                this.Close();
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

    }
}