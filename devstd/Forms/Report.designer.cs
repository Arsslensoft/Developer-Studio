namespace devstd
{
    partial class Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report));
            this.Panel1 = new System.Windows.Forms.Panel();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Codelb = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblErr = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(198)))));
            this.Panel1.Controls.Add(this.Label1);
            this.Panel1.Controls.Add(this.Label2);
            this.Panel1.Controls.Add(this.Codelb);
            this.Panel1.Controls.Add(this.PictureBox1);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel1.ForeColor = System.Drawing.Color.Black;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(586, 60);
            this.Panel1.TabIndex = 8;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Black;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(143, 18);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(40, 25);
            this.Label1.TabIndex = 4;
            this.Label1.Text = "....";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Black;
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(82, 47);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(66, 13);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Description :";
            // 
            // Codelb
            // 
            this.Codelb.BackColor = System.Drawing.Color.Black;
            this.Codelb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Codelb.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Codelb.ForeColor = System.Drawing.Color.White;
            this.Codelb.Location = new System.Drawing.Point(65, 0);
            this.Codelb.Name = "Codelb";
            this.Codelb.Size = new System.Drawing.Size(521, 60);
            this.Codelb.TabIndex = 2;
            this.Codelb.Text = "Code : ";
            this.Codelb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Black;
            this.PictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.PictureBox1.ForeColor = System.Drawing.Color.Black;
            this.PictureBox1.Image = global::devstd.Properties.Resources.warning;
            this.PictureBox1.Location = new System.Drawing.Point(0, 0);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(65, 60);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // lblErr
            // 
            this.lblErr.BackColor = System.Drawing.Color.Black;
            this.lblErr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblErr.ForeColor = System.Drawing.Color.White;
            this.lblErr.Location = new System.Drawing.Point(12, 63);
            this.lblErr.Multiline = true;
            this.lblErr.Name = "lblErr";
            this.lblErr.Size = new System.Drawing.Size(562, 302);
            this.lblErr.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(340, 368);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            this.btnSend.ForeColor = System.Drawing.Color.Black;
            this.btnSend.Location = new System.Drawing.Point(502, 368);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 9;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242)))));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(421, 368);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 403);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblErr);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.Panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Report";
            this.Text = "Error Report";
            this.Load += new System.EventHandler(this.Report_Load);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.Label Codelb;
        public System.Windows.Forms.TextBox lblErr;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button button1;
    }
}