namespace devstd
{
    partial class ProjNameFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjNameFrm));
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.textBoxX1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxEx1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.fx2 = new DevComponents.Editors.ComboItem();
            this.fx3 = new DevComponents.Editors.ComboItem();
            this.fx35 = new DevComponents.Editors.ComboItem();
            this.fx4 = new DevComponents.Editors.ComboItem();
            this.fx45 = new DevComponents.Editors.ComboItem();
            this.SuspendLayout();
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(283, 112);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.buttonX1.TabIndex = 0;
            this.buttonX1.Text = "Create";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // textBoxX1
            // 
            this.textBoxX1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            // 
            // 
            // 
            this.textBoxX1.Border.Class = "TextBoxBorder";
            this.textBoxX1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX1.DisabledBackColor = System.Drawing.Color.Black;
            this.textBoxX1.ForeColor = System.Drawing.Color.Black;
            this.textBoxX1.Location = new System.Drawing.Point(12, 31);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(328, 20);
            this.textBoxX1.TabIndex = 1;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 2);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(314, 23);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "Name";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 57);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(328, 23);
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "Target Framework Version";
            // 
            // comboBoxEx1
            // 
            this.comboBoxEx1.DisplayMember = "Text";
            this.comboBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx1.FormattingEnabled = true;
            this.comboBoxEx1.ItemHeight = 14;
            this.comboBoxEx1.Items.AddRange(new object[] {
            this.fx2,
            this.fx3,
            this.fx35,
            this.fx4,
            this.fx45});
            this.comboBoxEx1.Location = new System.Drawing.Point(12, 86);
            this.comboBoxEx1.Name = "comboBoxEx1";
            this.comboBoxEx1.Size = new System.Drawing.Size(328, 20);
            this.comboBoxEx1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.comboBoxEx1.TabIndex = 5;
            this.comboBoxEx1.Text = "FRAMEWORK 2.0";
            // 
            // fx2
            // 
            this.fx2.Text = "FRAMEWORK 2.0";
            // 
            // fx3
            // 
            this.fx3.Text = "FRAMEWORK 3.0";
            // 
            // fx35
            // 
            this.fx35.Text = "FRAMEWORK 3.5";
            // 
            // fx4
            // 
            this.fx4.Text = "FRAMEWORK 4";
            // 
            // fx45
            // 
            this.fx45.Text = "FRAMEWORK 4.5";
            // 
            // ProjNameFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 144);
            this.Controls.Add(this.comboBoxEx1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.textBoxX1);
            this.Controls.Add(this.buttonX1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjNameFrm";
            this.Text = "Create Project";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        private DevComponents.Editors.ComboItem fx2;
        private DevComponents.Editors.ComboItem fx3;
        private DevComponents.Editors.ComboItem fx35;
        private DevComponents.Editors.ComboItem fx4;
        private DevComponents.Editors.ComboItem fx45;
    }
}