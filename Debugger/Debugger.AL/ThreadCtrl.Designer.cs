namespace Debugger.AL
{
    partial class ThreadCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreadCtrl));
            this.messagesListView = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.idcol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.namecol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pricol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.excol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statecol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.exitcol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.idcol,
            this.namecol,
            this.pricol,
            this.excol,
            this.statecol,
            this.exitcol});
            this.messagesListView.DisabledBackColor = System.Drawing.Color.Empty;
            this.messagesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesListView.ForeColor = System.Drawing.Color.Black;
            this.messagesListView.FullRowSelect = true;
            this.messagesListView.Location = new System.Drawing.Point(0, 0);
            this.messagesListView.Name = "messagesListView";
            this.messagesListView.Size = new System.Drawing.Size(945, 239);
            this.messagesListView.SmallImageList = this.imageList;
            this.messagesListView.TabIndex = 3;
            this.messagesListView.UseCompatibleStateImageBehavior = false;
            this.messagesListView.View = System.Windows.Forms.View.Details;
            // 
            // idcol
            // 
            this.idcol.Text = "Thread Id";
            this.idcol.Width = 115;
            // 
            // namecol
            // 
            this.namecol.Text = "Thread Name";
            this.namecol.Width = 200;
            // 
            // pricol
            // 
            this.pricol.Text = "Priority";
            // 
            // excol
            // 
            this.excol.Text = "Exception";
            this.excol.Width = 105;
            // 
            // statecol
            // 
            this.statecol.Text = "Suspended";
            this.statecol.Width = 114;
            // 
            // exitcol
            // 
            this.exitcol.Text = "Has Exited";
            this.exitcol.Width = 98;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "play-26.png");
            this.imageList.Images.SetKeyName(1, "pause-26.png");
            this.imageList.Images.SetKeyName(2, "stop-26.png");
            // 
            // ThreadCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.messagesListView);
            this.Name = "ThreadCtrl";
            this.Size = new System.Drawing.Size(945, 239);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx messagesListView;
        private System.Windows.Forms.ColumnHeader idcol;
        private System.Windows.Forms.ColumnHeader namecol;
        private System.Windows.Forms.ColumnHeader pricol;
        private System.Windows.Forms.ColumnHeader excol;
        private System.Windows.Forms.ColumnHeader statecol;
        private System.Windows.Forms.ColumnHeader exitcol;
        internal System.Windows.Forms.ImageList imageList;

    }
}
