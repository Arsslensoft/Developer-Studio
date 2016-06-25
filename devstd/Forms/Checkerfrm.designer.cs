namespace devstd
{
    partial class Checkerfrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Checkerfrm));
            this.circularProgress1 = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // circularProgress1
            // 
            // 
            // 
            // 
            this.circularProgress1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circularProgress1.Location = new System.Drawing.Point(197, 12);
            this.circularProgress1.Name = "circularProgress1";
            this.circularProgress1.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Donut;
            this.circularProgress1.Size = new System.Drawing.Size(128, 144);
            this.circularProgress1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circularProgress1.TabIndex = 0;
            this.circularProgress1.Value = 100;
            this.circularProgress1.Visible = false;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(74, 183);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(354, 24);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "Please Wait...";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(207, 225);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(90, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "See the full report";
            this.linkLabel1.Visible = false;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // Checkerfrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 247);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.circularProgress1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Checkerfrm";
            this.Text = "Check Program";
            this.Shown += new System.EventHandler(this.Checkerfrm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.CircularProgress circularProgress1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}