using Debugger.AL;
namespace MoreComplexPopup
{
    partial class ComplexPopup
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
            this.debugTreeNode1 = new DebugTreeNode();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            ((System.ComponentModel.ISupportInitialize)(this.debugTreeNode1)).BeginInit();
            this.SuspendLayout();
            // 
            // debugTreeNode1
            // 
            this.debugTreeNode1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.debugTreeNode1.AllowDrop = true;
            this.debugTreeNode1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.debugTreeNode1.BackgroundStyle.Class = "TreeBorderKey";
            this.debugTreeNode1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.debugTreeNode1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugTreeNode1.Location = new System.Drawing.Point(9, 9);
            this.debugTreeNode1.Name = "debugTreeNode1";
            this.debugTreeNode1.NodesConnector = this.nodeConnector1;
            this.debugTreeNode1.NodeStyle = this.elementStyle1;
            this.debugTreeNode1.PathSeparator = ";";
            this.debugTreeNode1.Size = new System.Drawing.Size(407, 49);
            this.debugTreeNode1.Styles.Add(this.elementStyle1);
            this.debugTreeNode1.TabIndex = 0;
            this.debugTreeNode1.Text = "debugTreeNode1";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // ComplexPopup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.debugTreeNode1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ComplexPopup";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.Size = new System.Drawing.Size(425, 67);
            ((System.ComponentModel.ISupportInitialize)(this.debugTreeNode1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        internal DebugTreeNode debugTreeNode1;


    }
}
