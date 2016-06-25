using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CVP;
using devstd.utils;
using System.IO;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using System.Windows.Threading;

namespace devstd.Forms
{
    public partial class CVPPlay : DevComponents.DotNetBar.Metro.MetroForm
    {
     
        public CVPPlay()
        {
            InitializeComponent();
        }
        public void Play(string file)
        {
            try
            {
                playerControl1.CVPFile = file;
               this.Text = Path.GetFileNameWithoutExtension(file)+" - Code Visual Presentation";
   
             
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }


        private void CVPPlay_Shown(object sender, EventArgs e)
        {
            playerControl1.Play();
        }
    }
}