namespace devstd
{
    partial class ProjectN
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectN));
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.buttonItem13 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem14 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.checkBoxItem1 = new DevComponents.DotNetBar.CheckBoxItem();
            this.SuspendLayout();
            // 
            // itemPanel1
            // 
            this.itemPanel1.AutoScroll = true;
            this.itemPanel1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.itemPanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemPanel1.DragDropSupport = true;
            this.itemPanel1.ForeColor = System.Drawing.Color.White;
            this.itemPanel1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem13,
            this.buttonItem14,
            this.buttonItem1,
            this.checkBoxItem1});
            this.itemPanel1.Location = new System.Drawing.Point(0, 0);
            this.itemPanel1.MultiLine = true;
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(294, 108);
            this.itemPanel1.TabIndex = 4;
            // 
            // buttonItem13
            // 
            this.buttonItem13.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem13.ForeColor = System.Drawing.Color.Black;
            this.buttonItem13.Image = global::devstd.Properties.Resources.alproj;
            this.buttonItem13.ImagePaddingVertical = 12;
            this.buttonItem13.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem13.Name = "buttonItem13";
            this.buttonItem13.Text = "<span align=\"center\">Console<br/>Application</span>";
            this.buttonItem13.Click += new System.EventHandler(this.buttonItem13_Click);
            // 
            // buttonItem14
            // 
            this.buttonItem14.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem14.ForeColor = System.Drawing.Color.Black;
            this.buttonItem14.Image = global::devstd.Properties.Resources.alproj;
            this.buttonItem14.ImagePaddingVertical = 12;
            this.buttonItem14.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem14.Name = "buttonItem14";
            this.buttonItem14.Text = "<span align=\"center\">Windows Form<br/>Application</span>";
            this.buttonItem14.Click += new System.EventHandler(this.buttonItem14_Click);
            // 
            // buttonItem1
            // 
            this.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem1.ForeColor = System.Drawing.Color.Black;
            this.buttonItem1.Image = global::devstd.Properties.Resources.alproj;
            this.buttonItem1.ImagePaddingVertical = 12;
            this.buttonItem1.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "<span align=\"center\">Dynamic Linked<br/>Library</span>";
            this.buttonItem1.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // checkBoxItem1
            // 
            this.checkBoxItem1.Checked = true;
            this.checkBoxItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxItem1.Name = "checkBoxItem1";
            this.checkBoxItem1.Text = "Save Project Automatically";
            // 
            // ProjectN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 108);
            this.Controls.Add(this.itemPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectN";
            this.Text = "Developer Studio - AL Project";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.DotNetBar.ButtonItem buttonItem13;
        private DevComponents.DotNetBar.ButtonItem buttonItem14;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.CheckBoxItem checkBoxItem1;
    }
}