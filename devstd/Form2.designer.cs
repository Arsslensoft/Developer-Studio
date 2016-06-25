namespace devstd
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.labelX648 = new DevComponents.DotNetBar.LabelX();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.reflectionImage1 = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerColorTint = System.Drawing.SystemColors.ActiveCaptionText;
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.VisualStudio2012Light;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154))))));
            // 
            // labelX648
            // 
            this.labelX648.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX648.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX648.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelX648.Location = new System.Drawing.Point(17, 258);
            this.labelX648.Name = "labelX648";
            this.labelX648.Size = new System.Drawing.Size(300, 25);
            this.labelX648.TabIndex = 10;
            this.labelX648.Text = "Copyright © 2010-2014 Arsslensoft. All rights reserved";
            // 
            // progressBarX1
            // 
            this.progressBarX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.progressBarX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarX1.Location = new System.Drawing.Point(17, 221);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.ProgressType = DevComponents.DotNetBar.eProgressItemType.Marquee;
            this.progressBarX1.Size = new System.Drawing.Size(458, 16);
            this.progressBarX1.TabIndex = 9;
            this.progressBarX1.Text = "progressBarX1";
            // 
            // reflectionImage1
            // 
            // 
            // 
            // 
            this.reflectionImage1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage1.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage1.Image = global::devstd.Properties.Resources.devstdpro;
            this.reflectionImage1.Location = new System.Drawing.Point(41, 22);
            this.reflectionImage1.Name = "reflectionImage1";
            this.reflectionImage1.Size = new System.Drawing.Size(386, 230);
            this.reflectionImage1.TabIndex = 11;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.BackgroundImage = global::devstd.Properties.Resources.devstdpro;
            this.ClientSize = new System.Drawing.Size(503, 338);
            this.Controls.Add(this.progressBarX1);
            this.Controls.Add(this.reflectionImage1);
            this.Controls.Add(this.labelX648);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Initializing Developer Studio 2015";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.LabelX labelX648;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage1;
    }
}