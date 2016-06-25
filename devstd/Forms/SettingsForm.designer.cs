namespace devstd
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.intelcheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.stmsgcheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX2 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.comboBoxEx1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(384, 211);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(140, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.buttonX1.TabIndex = 0;
            this.buttonX1.Text = "Save";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // intelcheck
            // 
            this.intelcheck.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.intelcheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intelcheck.ForeColor = System.Drawing.Color.White;
            this.intelcheck.Location = new System.Drawing.Point(39, 54);
            this.intelcheck.Name = "intelcheck";
            this.intelcheck.Size = new System.Drawing.Size(248, 23);
            this.intelcheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.intelcheck.TabIndex = 6;
            this.intelcheck.Text = "Intellisense    (Auto-Completition)";
            // 
            // stmsgcheck
            // 
            this.stmsgcheck.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.stmsgcheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.stmsgcheck.ForeColor = System.Drawing.Color.White;
            this.stmsgcheck.Location = new System.Drawing.Point(39, 83);
            this.stmsgcheck.Name = "stmsgcheck";
            this.stmsgcheck.Size = new System.Drawing.Size(248, 23);
            this.stmsgcheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.stmsgcheck.TabIndex = 10;
            this.stmsgcheck.Text = "Startup Message";
            // 
            // checkBoxX1
            // 
            this.checkBoxX1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.checkBoxX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX1.ForeColor = System.Drawing.Color.White;
            this.checkBoxX1.Location = new System.Drawing.Point(39, 25);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(248, 23);
            this.checkBoxX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.checkBoxX1.TabIndex = 18;
            this.checkBoxX1.Text = "Parse code";
            // 
            // checkBoxX2
            // 
            this.checkBoxX2.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.checkBoxX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX2.ForeColor = System.Drawing.Color.White;
            this.checkBoxX2.Location = new System.Drawing.Point(39, 112);
            this.checkBoxX2.Name = "checkBoxX2";
            this.checkBoxX2.Size = new System.Drawing.Size(248, 23);
            this.checkBoxX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.checkBoxX2.TabIndex = 19;
            this.checkBoxX2.Text = "Auto Update";
            // 
            // comboBoxEx1
            // 
            this.comboBoxEx1.DisplayMember = "Text";
            this.comboBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx1.FormattingEnabled = true;
            this.comboBoxEx1.ItemHeight = 14;
            this.comboBoxEx1.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3});
            this.comboBoxEx1.Location = new System.Drawing.Point(39, 170);
            this.comboBoxEx1.Name = "comboBoxEx1";
            this.comboBoxEx1.Size = new System.Drawing.Size(186, 20);
            this.comboBoxEx1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.comboBoxEx1.TabIndex = 20;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "English";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "Français";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "العربية";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(39, 141);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(260, 23);
            this.labelX1.TabIndex = 21;
            this.labelX1.Text = "Default Language";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 241);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.comboBoxEx1);
            this.Controls.Add(this.checkBoxX2);
            this.Controls.Add(this.checkBoxX1);
            this.Controls.Add(this.stmsgcheck);
            this.Controls.Add(this.intelcheck);
            this.Controls.Add(this.buttonX1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Setting Developer Studio Professional Environment";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX intelcheck;
        private DevComponents.DotNetBar.Controls.CheckBoxX stmsgcheck;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}