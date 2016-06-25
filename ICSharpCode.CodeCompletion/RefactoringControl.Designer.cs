namespace ICSharpCode.CodeCompletion
{
    partial class RefactoringControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RefactoringControl));
            this.messagesListView = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.desccol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sevcol1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.stcol1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.encol1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.desccol,
            this.sevcol1,
            this.stcol1,
            this.encol1});
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
            this.messagesListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.messagesListView_MouseDoubleClick);
            // 
            // desccol
            // 
            this.desccol.Text = "Description";
            this.desccol.Width = 392;
            // 
            // sevcol1
            // 
            this.sevcol1.Text = "Action Severity";
            this.sevcol1.Width = 127;
            // 
            // stcol1
            // 
            this.stcol1.Text = "Start Offset";
            this.stcol1.Width = 91;
            // 
            // encol1
            // 
            this.encol1.Text = "End Offset";
            this.encol1.Width = 85;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "toggle.png");
            // 
            // RefactoringControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.messagesListView);
            this.Name = "RefactoringControl";
            this.Size = new System.Drawing.Size(696, 303);
            this.Resize += new System.EventHandler(this.messagesListView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx messagesListView;
        private System.Windows.Forms.ColumnHeader desccol;
        internal System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ColumnHeader sevcol1;
        private System.Windows.Forms.ColumnHeader stcol1;
        private System.Windows.Forms.ColumnHeader encol1;
    }
}
