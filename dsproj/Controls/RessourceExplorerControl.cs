using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using alproj;
using System.IO;
using DevComponents.DotNetBar;

namespace alfrmdesign
{
    public partial class RessourceExplorerControl : UserControl
    {

        ALProject project;
        public event ThumbnailImageEventHandler OnImageSizeChanged;
        private ImageDialog m_ImageDialog;

        private ImageViewer m_ActiveImageViewer;
        public void Init(ALProject proj)
        {
            try
            {
                project = proj;
                dataGridViewX1.Rows.Clear();
                dataGridViewX2.Rows.Clear();
                dataGridViewX3.Rows.Clear();
                dataGridViewX4.Rows.Clear();
                thumbnailFlowLayoutPanel1.Controls.Clear();
                itemPanel3.SubItems.Clear();
                flowLayoutPanelMain.Controls.Clear();

                m_ImageDialog = new ImageDialog();


            foreach (KeyValuePair<string, RessourceItem> p in project.Ressources)
                {
                    if (p.Value.Type == RessourceType.String)
                    {
                        dataGridViewX1.Rows.Add(p.Key, p.Value.Value);
                    }
                    else if (p.Value.Type == RessourceType.Integer)
                    {
                        dataGridViewX2.Rows.Add(p.Key, p.Value.Value);
                    }
                    else if (p.Value.Type == RessourceType.Boolean)
                    {
                        dataGridViewX3.Rows.Add(p.Key, p.Value.Value);
                    }
                    else if (p.Value.Type == RessourceType.Real)
                    {
                        dataGridViewX4.Rows.Add(p.Key, p.Value.Value);
                    }
                    else if (p.Value.Type == RessourceType.Icon)
                    {
                        MemoryStream ms = new MemoryStream(Convert.FromBase64String(p.Value.Value));
                        ImageViewer imageViewer = new ImageViewer();
                        imageViewer.Dock = DockStyle.Bottom;
                        imageViewer.LoadImage(new Icon(ms), 256, 256);
                        imageViewer.Width = 128;
                        imageViewer.Height = 128;
                        imageViewer.Name = p.Key;
                        imageViewer.IsThumbnail = true;
                        imageViewer.MouseClick += new MouseEventHandler(imageViewer_MouseClick_icon);
                     
                         this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);

                     thumbnailFlowLayoutPanel1 .Controls.Add(imageViewer);
                    }
                    else if (p.Value.Type == RessourceType.Image)
                    {
                        ImageViewer imageViewer = new ImageViewer();
                        imageViewer.Dock = DockStyle.Bottom;
                        MemoryStream ms = new MemoryStream(Convert.FromBase64String(p.Value.Value));
                        imageViewer.LoadImage(Image.FromStream(ms), 256, 256);
                        imageViewer.Width = 128;
                        imageViewer.Height = 128;
                        imageViewer.IsThumbnail = true;
                        imageViewer.Name = p.Key;
                        imageViewer.MouseClick += new MouseEventHandler(imageViewer_MouseClick_image);
                      
                        this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);

                        this.flowLayoutPanelMain.Controls.Add(imageViewer);
                    }
                    else
                    {
                        ButtonItem it = new ButtonItem();
                        it.ImageFixedSize = new Size(48, 48);
                        it.Name = p.Key;
                        it.Text = p.Key;
                      
                        itemPanel3.SubItems.Add(it);
                    }

                }
            foreach (string keyword in File.ReadAllLines(Application.StartupPath + @"\Data\AKeywords.dat"))
            {
                if (keyword.StartsWith(","))
                    KEYWORDS.Add(keyword.Remove(0, 1).ToUpper());
                else
                    KEYWORDS.Add(keyword.ToUpper());
            }
            }
            catch
            {

            }
        }
        List<string> KEYWORDS = new List<string>();

        public RessourceExplorerControl()
        {
            InitializeComponent();                
           
        }
        bool IsValidName(string name)
        {
            bool val = (!KEYWORDS.Contains(name.ToUpper().Replace(" ", "")) && !name.Contains(" "));
            if (val == true)
            {
                foreach (char s in name)
                {

                    if (!(char.IsLetterOrDigit(s) || s == '_'))
                    {
                        val = false;
                        MessageBoxEx.Show("Please choose a valid name." + Environment.NewLine + "The name should not be an AL keyword", "Ressource Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
                }
            }
            return val;
        }
        bool IsValid(string file)
        {
            bool val = IsValidName(Path.GetFileNameWithoutExtension(file));
            if (val == false)
                MessageBoxEx.Show("Please choose a valid name." + Environment.NewLine + "The name should not be an AL keyword", "Ressource Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                foreach (char s in Path.GetFileNameWithoutExtension(file))
                {
                    if (!(char.IsLetterOrDigit(s) || s == '_'))
                    {
                        val = false;
                        MessageBoxEx.Show("Please choose a valid file name." + Environment.NewLine + "The name should contain only letters, digits or _", "Ressource Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
                }
            }
            return val;
        }
        public event EventHandler OnRessourceModified;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                if (superTabControl1.SelectedTab.Text == "String")
                {
                    ValueAdder frm = new ValueAdder();
                    frm.ShowDialog();
                    if (IsValidName(frm.textBoxX1.Text))
                    {
                        dataGridViewX1.Rows.Add(frm.textBoxX1.Text, frm.textBoxX2.Text);
                        RessourceItem rit = new RessourceItem(frm.textBoxX1.Text, "", RessourceType.String, frm.textBoxX2.Text, frm.comboBoxEx1.Text);
                        if (!project.Ressources.ContainsKey(frm.textBoxX1.Text))
                            project.Ressources.Add(frm.textBoxX1.Text, rit);
                    }
                }
                else if (superTabControl1.SelectedTab.Text == "Integer")
                {
                    ValueAdder frm = new ValueAdder();
                    frm.ShowDialog();
                    if (IsValidName(frm.textBoxX1.Text))
                    {
                        int.Parse(frm.textBoxX2.Text);
                        dataGridViewX2.Rows.Add(frm.textBoxX1.Text, frm.textBoxX2.Text);
                        RessourceItem rit = new RessourceItem(frm.textBoxX1.Text, "", RessourceType.Integer, frm.textBoxX2.Text, frm.comboBoxEx1.Text);
                        if (!project.Ressources.ContainsKey(frm.textBoxX1.Text) )
                            project.Ressources.Add(frm.textBoxX1.Text, rit);
                    }
                }
                else if (superTabControl1.SelectedTab.Text == "Boolean")
                {
                    ValueAdder frm = new ValueAdder();
                    frm.ShowDialog();
                    if (IsValidName(frm.textBoxX1.Text))
                    {
                        bool.Parse(frm.textBoxX2.Text);
                        dataGridViewX3.Rows.Add(frm.textBoxX1.Text, frm.textBoxX2.Text);
                        RessourceItem rit = new RessourceItem(frm.textBoxX1.Text, "", RessourceType.Boolean, frm.textBoxX2.Text, frm.comboBoxEx1.Text);
                        if (!project.Ressources.ContainsKey(frm.textBoxX1.Text))
                            project.Ressources.Add(frm.textBoxX1.Text, rit);
                    }
                }
                else if (superTabControl1.SelectedTab.Text == "Real")
                {
                    ValueAdder frm = new ValueAdder();
                    frm.ShowDialog();
                    if (IsValidName(frm.textBoxX1.Text))
                    {
                        double.Parse(frm.textBoxX2.Text);
                        dataGridViewX4.Rows.Add(frm.textBoxX1.Text, frm.textBoxX2.Text);
                        RessourceItem rit = new RessourceItem(frm.textBoxX1.Text, "", RessourceType.Real, frm.textBoxX2.Text, frm.comboBoxEx1.Text);
                        if (!project.Ressources.ContainsKey(frm.textBoxX1.Text))
                            project.Ressources.Add(frm.textBoxX1.Text, rit);
                    }
                }
                else if (superTabControl1.SelectedTab.Text == "Icon")
                {
                    if (openFileDialog2.ShowDialog() == DialogResult.OK)
                    {
                        if (IsValid(openFileDialog2.FileName))
                        {
                            if (!project.Ressources.ContainsKey(Path.GetFileNameWithoutExtension(openFileDialog2.FileName)))
                            {
                                try
                                {
                                    if (File.Exists(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog2.FileName)))
                                        File.Delete(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog2.FileName));

                                    File.Copy(openFileDialog2.FileName, Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog2.FileName));
                                }
                                catch
                                {

                                }
                                project.ImportRessource(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog2.FileName), RessourceType.Icon);
                                MemoryStream ms = new MemoryStream(Convert.FromBase64String(openFileDialog2.FileName));
                                ImageViewer imageViewer = new ImageViewer();
                                imageViewer.Dock = DockStyle.Bottom;
                                imageViewer.LoadImage(new Icon(ms), 256, 256);
                                imageViewer.Width = 128;
                                imageViewer.Height = 128;
                                imageViewer.Name = Path.GetFileNameWithoutExtension(openFileDialog2.FileName);
                                imageViewer.IsThumbnail = true;
                                imageViewer.MouseClick += new MouseEventHandler(imageViewer_MouseClick_icon);
                                this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);
                              
                            }
                        }
                    }
                }
                else if (superTabControl1.SelectedTab.Text == "Image")
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        if (IsValid(openFileDialog1.FileName))
                        {
                            if (!project.Ressources.ContainsKey(Path.GetFileNameWithoutExtension(openFileDialog1.FileName)))
                            {

                                try
                                {
                                    if (File.Exists(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog1.FileName)))
                                        File.Delete(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog1.FileName));

                                    File.Copy(openFileDialog1.FileName, Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog1.FileName));
                                }
                                catch
                                {

                                }
                                project.ImportRessource(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog1.FileName), RessourceType.Image);

                                ImageViewer imageViewer = new ImageViewer();
                                imageViewer.Dock = DockStyle.Bottom;
                                imageViewer.LoadImage(Image.FromFile(openFileDialog1.FileName), 256, 256);
                                imageViewer.Width = 128;
                                imageViewer.Height = 128;
                                imageViewer.IsThumbnail = true;
                                imageViewer.Name = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                                imageViewer.MouseClick += new MouseEventHandler(imageViewer_MouseClick_image);

                                this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);

                                this.flowLayoutPanelMain.Controls.Add(imageViewer);
                              
                            }
                        }
                    }
                }
                else if (superTabControl1.SelectedTab.Text == "File Data")
                {
                    if (openFileDialog3.ShowDialog() == DialogResult.OK)
                    {
                        if (IsValid(openFileDialog3.FileName))
                        {
                            if (!project.Ressources.ContainsKey(Path.GetFileNameWithoutExtension(openFileDialog3.FileName)))
                            {
                                try
                                {
                                    if (File.Exists(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog3.FileName)))
                                        File.Delete(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog3.FileName));

                                    File.Copy(openFileDialog3.FileName, Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog3.FileName));
                                }
                                catch
                                {

                                }
                                project.ImportRessource(Path.GetDirectoryName(project.FileName) + @"\res\" + Path.GetFileName(openFileDialog3.FileName), RessourceType.Data);
                                ButtonItem item = new ButtonItem();
                                item.ImageFixedSize = new Size(48, 48);
                                item.Name = openFileDialog3.FileName;
                                item.Text = Path.GetFileNameWithoutExtension(openFileDialog3.FileName);
                                itemPanel3.SubItems.Add(item);

                               
                            }
                        }
                    }
                }

                if (OnRessourceModified != null)
                    OnRessourceModified(project, EventArgs.Empty);
            }
            catch
            {

            }
        }
        private void imageViewer_MouseClick_icon(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_ActiveImageViewer != null)
                {
                    m_ActiveImageViewer.IsActive = false;
                }

                m_ActiveImageViewer = (ImageViewer)sender;
                m_ActiveImageViewer.IsActive = true;

                if (m_ImageDialog.IsDisposed) m_ImageDialog = new ImageDialog();
                if (!m_ImageDialog.Visible) m_ImageDialog.Show();

                m_ImageDialog.SetImage(m_ActiveImageViewer.Image);
            }
            catch
            {

            }
        }
        private void imageViewer_MouseClick_image(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_ActiveImageViewer != null)
                {
                    m_ActiveImageViewer.IsActive = false;
                }

                m_ActiveImageViewer = (ImageViewer)sender;
                m_ActiveImageViewer.IsActive = true;

                if (m_ImageDialog.IsDisposed) m_ImageDialog = new ImageDialog();
                if (!m_ImageDialog.Visible) m_ImageDialog.Show();

                m_ImageDialog.SetImage(m_ActiveImageViewer.Image);
            }
            catch
            {

            }
        }
        private void buttonX2_Click(object sender, EventArgs e)
        {
            try
            {
                if (superTabControl1.SelectedTab.Text == "String")
                {
                    string name = dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString();
                    dataGridViewX1.Rows.Remove(dataGridViewX1.SelectedRows[0]);
                    project.Ressources.Remove(name);
                }
                else if (superTabControl1.SelectedTab.Text == "Integer")
                {
                    string name = dataGridViewX2.SelectedRows[0].Cells[0].Value.ToString();
                    dataGridViewX2.Rows.Remove(dataGridViewX2.SelectedRows[0]);
                    project.Ressources.Remove(name);
                }
                else if (superTabControl1.SelectedTab.Text == "Boolean")
                {
                    string name = dataGridViewX3.SelectedRows[0].Cells[0].Value.ToString();
                    dataGridViewX3.Rows.Remove(dataGridViewX3.SelectedRows[0]);
                    project.Ressources.Remove(name);
                }
                else if (superTabControl1.SelectedTab.Text == "Real")
                {
                    string name = dataGridViewX4.SelectedRows[0].Cells[0].Value.ToString();
                    dataGridViewX4.Rows.Remove(dataGridViewX4.SelectedRows[0]);
                    project.Ressources.Remove(name);
                }
            
                if(OnRessourceModified != null)
                    OnRessourceModified(project, EventArgs.Empty);
            }
            catch
            {

            }
        }

        private void superTabControl1_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            try
            {
                if ((superTabControl1.SelectedTab.Text == "Real") || (superTabControl1.SelectedTab.Text == "Boolean") || (superTabControl1.SelectedTab.Text == "Integer") || (superTabControl1.SelectedTab.Text == "String"))
               buttonItem1.Visible = true;
                else
                    buttonItem1.Visible = false;
            }
            catch
            {

            }
        }

        private void comboBoxItem1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string name = "";
                  if (superTabControl1.SelectedTab.Text == "String")
                {
                    name = dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString();
                             

                }
                else if (superTabControl1.SelectedTab.Text == "Integer")
                {
                    name = dataGridViewX2.SelectedRows[0].Cells[0].Value.ToString();
                  
                }
                else if (superTabControl1.SelectedTab.Text == "Boolean")
                {
                    name = dataGridViewX3.SelectedRows[0].Cells[0].Value.ToString();
                  
                }
                else if (superTabControl1.SelectedTab.Text == "Real")
                {
                    name = dataGridViewX4.SelectedRows[0].Cells[0].Value.ToString();
                }
                if(name != "")
                {
                if (comboBoxItem1.SelectedItem != null)
                {
                    if (comboBoxItem1.SelectedItem == comboItem1)
                        project.Ressources[name] = new RessourceItem(project.Ressources[name].Name, project.Ressources[name].DevPath, project.Ressources[name].Type, project.Ressources[name].Value, "Internal");
                    else project.Ressources[name] = new RessourceItem(project.Ressources[name].Name, project.Ressources[name].DevPath, project.Ressources[name].Type, project.Ressources[name].Value, "Public");
                }
                else if (comboBoxItem1.Text == "Internal" || comboBoxItem1.Text == "Public")
                {
                    if (comboBoxItem1.Text == "Internal")
                        project.Ressources[name] = new RessourceItem(project.Ressources[name].Name, project.Ressources[name].DevPath, project.Ressources[name].Type, project.Ressources[name].Value, "Internal");
                    else project.Ressources[name] = new RessourceItem(project.Ressources[name].Name, project.Ressources[name].DevPath, project.Ressources[name].Type, project.Ressources[name].Value, "Public");
                }

                }

            }
            catch
            {

            }
        }

        private void dataGridViewX1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                string name = "";
                if (superTabControl1.SelectedTab.Text == "String")
                {
                    name = dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString();
                             

                }
                else if (superTabControl1.SelectedTab.Text == "Integer")
                {
                    name = dataGridViewX2.SelectedRows[0].Cells[0].Value.ToString();
                  
                }
                else if (superTabControl1.SelectedTab.Text == "Boolean")
                {
                    name = dataGridViewX3.SelectedRows[0].Cells[0].Value.ToString();
                  
                }
                else if (superTabControl1.SelectedTab.Text == "Real")
                {
                    name = dataGridViewX4.SelectedRows[0].Cells[0].Value.ToString();
                }
             
                if (name != "")
                {
                    if (project.Ressources[name].Modifier == "Internal")
                        comboBoxItem1.SelectedItem = comboItem1;
                    else
                        comboBoxItem1.SelectedItem = comboItem2;
                }
            }
            catch
            {

            }
        }

        public void SelectTab(RessourceType type)
        {
            switch (type)
            {
                case RessourceType.Image:
                    superTabControl1.SelectedTab = imagetab;
                    break;
                case RessourceType.String:
                    superTabControl1.SelectedTab = Stringstab;
                    break;
                case RessourceType.Integer:
                    superTabControl1.SelectedTab = integertab;
                    break;
                case RessourceType.Boolean:
                    superTabControl1.SelectedTab = booltab;
                    break;
                case RessourceType.Icon:
                    superTabControl1.SelectedTab = icontab;
                    break;

                case RessourceType.Real:
                    superTabControl1.SelectedTab = realtab;
                    break;

                case RessourceType.Data:
                    superTabControl1.SelectedTab = filetab;
                    break;
            }
        }
    }
}
