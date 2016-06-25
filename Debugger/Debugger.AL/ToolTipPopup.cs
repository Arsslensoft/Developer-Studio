using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PopupControl;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Documents;

namespace MoreComplexPopup
{
    public partial class ToolTipPopup : System.Windows.Forms.UserControl
    {
        public ToolTipPopup(FlowDocument doc)
        {
            InitializeComponent();
         
            MinimumSize = Size;
            
            MaximumSize = new Size(Size.Width * 3, Size.Height * 3);
            DoubleBuffered = true;
            ResizeRedraw = true;
            try
            {
                FlowDocumentScrollViewer fr = new FlowDocumentScrollViewer();
                fr.Document = doc;
                ElementHost d = new ElementHost();
                d.Dock = DockStyle.Fill;
    

                d.Child = fr;
                this.Controls.Add(d);
            }
            catch
            {

            }
        }

        protected override void WndProc(ref Message m)
        {
            if ((Parent as Popup).ProcessResizing(ref m))
            {
                return;
            }
            base.WndProc(ref m);
        }
    }
}
