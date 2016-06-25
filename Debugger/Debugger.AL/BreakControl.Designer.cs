namespace Debugger.AL
{
    partial class BreakControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BreakControl));
            this.messagesListView = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.filecol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.linecol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colcol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
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
            this.filecol,
            this.linecol,
            this.colcol});
            this.messagesListView.DisabledBackColor = System.Drawing.Color.Empty;
            this.messagesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesListView.ForeColor = System.Drawing.Color.Black;
            this.messagesListView.FullRowSelect = true;
            this.messagesListView.Location = new System.Drawing.Point(0, 0);
            this.messagesListView.Name = "messagesListView";
            this.messagesListView.Size = new System.Drawing.Size(696, 303);
            this.messagesListView.SmallImageList = this.imageList;
            this.messagesListView.TabIndex = 4;
            this.messagesListView.UseCompatibleStateImageBehavior = false;
            this.messagesListView.View = System.Windows.Forms.View.Details;
            this.messagesListView.SelectedIndexChanged += new System.EventHandler(this.messagesListView_SelectedIndexChanged);
            // 
            // filecol
            // 
            this.filecol.Text = "File";
            this.filecol.Width = 569;
            // 
            // linecol
            // 
            this.linecol.Text = "Line";
            // 
            // colcol
            // 
            this.colcol.Text = "Enabled";
            this.colcol.Width = 62;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "toggle.png");
            // 
            // BreakControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.messagesListView);
            this.Name = "BreakControl";
            this.Size = new System.Drawing.Size(696, 303);
            this.Resize += new System.EventHandler(this.BreakControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx messagesListView;
        private System.Windows.Forms.ColumnHeader filecol;
        private System.Windows.Forms.ColumnHeader linecol;
        private System.Windows.Forms.ColumnHeader colcol;
        internal System.Windows.Forms.ImageList imageList;
    }
}
