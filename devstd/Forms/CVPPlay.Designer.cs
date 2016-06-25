using System.Windows.Forms.Integration;
namespace devstd.Forms
{
    partial class CVPPlay
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
            CVP.CVPPlayer cvpPlayer1 = new CVP.CVPPlayer();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.playerControl1 = new CVP.Controls.PlayerControl();
            this.SuspendLayout();
            // 
            // progressBarX1
            // 
            this.progressBarX1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.progressBarX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarX1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarX1.ForeColor = System.Drawing.Color.Black;
            this.progressBarX1.Location = new System.Drawing.Point(0, 385);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.ProgressType = DevComponents.DotNetBar.eProgressItemType.Marquee;
            this.progressBarX1.Size = new System.Drawing.Size(751, 11);
            this.progressBarX1.TabIndex = 0;
            this.progressBarX1.Text = "progressBarX1";
            // 
            // playerControl1
            // 
            this.playerControl1.BackColor = System.Drawing.Color.Transparent;
            this.playerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerControl1.Location = new System.Drawing.Point(0, 0);
            this.playerControl1.Name = "playerControl1";
            cvpPlayer1.AudioPlayer = null;
            cvpPlayer1.IsPaused = false;
            cvpPlayer1.IsPlaying = false;
            cvpPlayer1.Reader = null;
            this.playerControl1.Player = cvpPlayer1;
            this.playerControl1.Size = new System.Drawing.Size(751, 385);
            this.playerControl1.TabIndex = 1;
            // 
            // CVPPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 396);
            this.Controls.Add(this.playerControl1);
            this.Controls.Add(this.progressBarX1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "CVPPlay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Code Visual Presentation - Player";
            this.Shown += new System.EventHandler(this.CVPPlay_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private CVP.Controls.PlayerControl playerControl1;

    }
}