//
// Authors:	 
//	  Jonathan Pobst (monkey@jpobst.com>)
//
// (C) 2007 Jonathan Pobst

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Design;

namespace alfrmdesign 
{
	public class ToolBoxListItem : UserControl 
	{

#region Fields
		private bool hover;
		private Image image;
		private bool selected;
        private DevComponents.DotNetBar.PanelEx panelEx1;
		private ToolboxItem tool_box_item;
#endregion

#region Public Constructor
		public ToolBoxListItem (ToolboxItem toolBoxItem)
		{
			Size = new Size (150, 20);

			SetStyle (ControlStyles.ResizeRedraw, true);
           
         
			Text = toolBoxItem.DisplayName;

			if (toolBoxItem.Bitmap != null)
				image = toolBoxItem.Bitmap;
			else
				image = SystemIcons.Exclamation.ToBitmap ();

			tool_box_item = toolBoxItem;

		}
#endregion

#region Public Properties
		public bool Selected {
			get { return selected;}
			set {
				if (selected != value) {
					selected = value;
					Invalidate ();
				}
			}
		}

		public ToolboxItem ToolBoxItem {
			get { return tool_box_item;}
		}
#endregion

#region Public Methods
		public void Deselect ()
		{
			if (selected) {
				selected = false;
				Invalidate ();
			}
		}
#endregion

#region Protected Methods
		protected override void OnClick (EventArgs e)
		{
			selected = true;
			Invalidate ();

			base.OnClick (e);
		}

		protected override void OnMouseEnter (EventArgs e)
		{
			base.OnMouseEnter (e);

			hover = true;
			Invalidate ();
		}

		protected override void OnMouseLeave (EventArgs e)
		{
			base.OnMouseLeave (e);

			hover = false;
			Invalidate ();
		}

		protected override void OnPaintBackground (PaintEventArgs e)
		{
			base.OnPaintBackground (e);

			if (hover) {
				using (SolidBrush b = new SolidBrush (ProfessionalColors.ButtonPressedGradientMiddle))
				e.Graphics.FillRectangle (b, ClientRectangle);

				using (Pen p = new Pen (ProfessionalColors.ButtonPressedHighlightBorder))
				e.Graphics.DrawRectangle (p, new Rectangle (0, 0, Width - 1, Height - 1));
			} else if (selected) {
				using (SolidBrush b = new SolidBrush (ProfessionalColors.ButtonSelectedGradientMiddle))
				e.Graphics.FillRectangle (b, ClientRectangle);

				using (Pen p = new Pen (ProfessionalColors.ButtonSelectedHighlightBorder))
				e.Graphics.DrawRectangle (p, new Rectangle (0, 0, Width - 1, Height - 1));
			}
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			base.OnPaint (e);

			if (image != null)
				e.Graphics.DrawImage (image, new Rectangle (2, 2, 16, 16));

			TextRenderer.DrawText (e.Graphics, Text, Font, new Rectangle (20, 1, Width - 21, Height - 2), Color.Black, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
		}
#endregion

        private void InitializeComponent()
        {
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(150, 150);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // ToolBoxListItem
            // 
            this.Controls.Add(this.panelEx1);
            this.Name = "ToolBoxListItem";
            this.ResumeLayout(false);

        }
	}
}
