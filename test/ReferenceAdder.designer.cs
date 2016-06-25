namespace devstd
{
    partial class ReferenceAdder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReferenceAdder));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.messagesListView = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.namec = new System.Windows.Forms.ColumnHeader();
            this.info = new System.Windows.Forms.ColumnHeader();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.buttonX3);
            this.panelEx1.Controls.Add(this.checkBoxX1);
            this.panelEx1.Controls.Add(this.buttonX2);
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx1.Location = new System.Drawing.Point(0, 272);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(625, 33);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX3.Location = new System.Drawing.Point(346, 5);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(75, 23);
            this.buttonX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX3.TabIndex = 3;
            this.buttonX3.Text = "Open";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // checkBoxX1
            // 
            // 
            // 
            // 
            this.checkBoxX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX1.Location = new System.Drawing.Point(12, 7);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(269, 23);
            this.checkBoxX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX1.TabIndex = 2;
            this.checkBoxX1.Text = "Copy Assemblies";
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2.Location = new System.Drawing.Point(538, 5);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(75, 23);
            this.buttonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX2.TabIndex = 1;
            this.buttonX2.Text = "Close";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(447, 5);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 0;
            this.buttonX1.Text = "Add";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // messagesListView
            // 
            this.messagesListView.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.messagesListView.Border.Class = "ListViewBorder";
            this.messagesListView.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.messagesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.namec,
            this.info});
            this.messagesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesListView.ForeColor = System.Drawing.Color.Black;
            this.messagesListView.FullRowSelect = true;
            this.messagesListView.GridLines = true;
            this.messagesListView.HideSelection = false;
            this.messagesListView.Location = new System.Drawing.Point(0, 0);
            this.messagesListView.MultiSelect = false;
            this.messagesListView.Name = "messagesListView";
            this.messagesListView.Size = new System.Drawing.Size(625, 272);
            this.messagesListView.TabIndex = 2;
            this.messagesListView.UseCompatibleStateImageBehavior = false;
            this.messagesListView.View = System.Windows.Forms.View.Details;
            // 
            // namec
            // 
            this.namec.Text = "Name";
            this.namec.Width = 220;
            // 
            // info
            // 
            this.info.Text = "Information";
            this.info.Width = 400;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "DLL Files|*.dll|Executables|*.exe|Net Module|.netmodule";
            // 
            // ReferenceAdder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 305);
            this.Controls.Add(this.messagesListView);
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReferenceAdder";
            this.Text = "Add Reference";
            this.Load += new System.EventHandler(this.ReferenceAdder_Load);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.ListViewEx messagesListView;
        private System.Windows.Forms.ColumnHeader namec;
        private System.Windows.Forms.ColumnHeader info;
        internal DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
        private DevComponents.DotNetBar.ButtonX buttonX3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}