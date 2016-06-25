using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevComponents.DotNetBar.Metro;

namespace alfrmdesign
{
    public partial class ImageDialog : MetroForm
    {
        public ImageDialog()
        {
            InitializeComponent();
        }


        public void SetImage(Image img)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(SetImageInternI));
            thread.IsBackground = true;
            thread.Start(img);
        }

        private void SetImageInternI(object filename)
        {
            this.imageViewerFull.Image = (Image)filename;
            this.imageViewerFull.Invalidate();
        }


        private void ImageDialog_Resize(object sender, EventArgs e)
        {
            this.imageViewerFull.Invalidate();
        }
    }
}