using DevComponents.DotNetBar;
namespace alfrmdesign
{
	partial class ErrorList
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._errorsList = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._detailedText = new System.Windows.Forms.TextBox();
            this.saveButton = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._errorsList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(737, 492);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 0;
            // 
            // _errorsList
            // 
            this._errorsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._errorsList.FormattingEnabled = true;
            this._errorsList.HorizontalScrollbar = true;
            this._errorsList.IntegralHeight = false;
            this._errorsList.Location = new System.Drawing.Point(0, 0);
            this._errorsList.Name = "_errorsList";
            this._errorsList.Size = new System.Drawing.Size(737, 160);
            this._errorsList.TabIndex = 3;
            this._errorsList.SelectedIndexChanged += new System.EventHandler(this.errorsList_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._detailedText);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.saveButton);
            this.splitContainer2.Size = new System.Drawing.Size(737, 328);
            this.splitContainer2.SplitterDistance = 296;
            this.splitContainer2.TabIndex = 0;
            // 
            // _detailedText
            // 
            this._detailedText.Dock = System.Windows.Forms.DockStyle.Fill;
            this._detailedText.Location = new System.Drawing.Point(0, 0);
            this._detailedText.Multiline = true;
            this._detailedText.Name = "_detailedText";
            this._detailedText.ReadOnly = true;
            this._detailedText.Size = new System.Drawing.Size(737, 296);
            this._detailedText.TabIndex = 2;
            // 
            // saveButton
            // 
            this.saveButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(640, 2);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(94, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save log to file";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // ErrorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ErrorList";
            this.Size = new System.Drawing.Size(737, 492);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.TextBox _detailedText;
		private ButtonX saveButton;
		private System.Windows.Forms.ListBox _errorsList;
	}
}
