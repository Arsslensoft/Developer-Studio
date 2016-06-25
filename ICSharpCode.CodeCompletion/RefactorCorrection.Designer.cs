namespace ICSharpCode.CodeCompletion
{
    partial class RefactorCorrection
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
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
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
            this.itemPanel1.Location = new System.Drawing.Point(0, 0);
            this.itemPanel1.MultiLine = true;
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(355, 90);
            this.itemPanel1.TabIndex = 5;
            // 
            // RefactorCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 90);
            this.Controls.Add(this.itemPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RefactorCorrection";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Arsslen Language Refactoring - Correction";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ItemPanel itemPanel1;
    }
}