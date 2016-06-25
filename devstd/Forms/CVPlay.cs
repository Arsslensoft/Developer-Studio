using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using System.Diagnostics;
using ICSharpCode.CodeCompletion;
using System.Windows.Forms.Integration;
using ICSharpCode.AvalonEdit.Highlighting;
using CVP;

namespace devstd
{
    public partial class CVPlay : DevComponents.DotNetBar.Metro.MetroForm
    {
        CVPReader cvpr;
        public CVPlay(CVPReader cvp)
        {

            InitializeComponent();
            cvpr = cvp;
        }
        CodeTextEditor edit;
        private void CVPlay_Load(object sender, EventArgs e)
        {
            try
            {
                edit = new CodeTextEditor();
                string file = Path.GetTempFileName() + cvpr.SourceName;
                File.WriteAllText(file, " ");
                ElementHost h = new ElementHost();

                h.Dock = DockStyle.Fill;
                h.Child = edit;
                edit.Name = "EDITOR";
                edit.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("AL");
                edit.OpenFile(file);
                this.panelEx2.Controls.Add(h);
                cvpr.OnCVPEvent += new CVPEvent(cvpr_OnCVPEvent);
            }
            catch
            {

            }
        }

        void cvpr_OnCVPEvent(string action, string data,string odata)
        {
            try
            {
                if (action == "PUSH")
                    AppendSRC(edit, data,odata);
            //    else if (action == "SAYS")
            //        Speech.SpeakSync(data);
            //    else if (action == "SAYA")
            //        Speech.SpeakSync(data);
            }
            catch
            {

            }
        }
        delegate void AppendTextEDelegate(CodeTextEditor txt, string text);
        string position;
        void AppendSRC(CodeTextEditor txt, string t, string odata)
        {
            try
            {
                AppendTextEDelegate d = new AppendTextEDelegate(AL);
                position = odata;
                    this.BeginInvoke(d, new object[] { txt, t });
                 


            }
            catch
            {

            }
        }
        void AL(CodeTextEditor txt, string t)
        {
            try
            {
                txt.Text = t;
                txt.JumpTo(int.Parse(position.Split(';')[0]), int.Parse(position.Split(';')[1]) + 1);
                progressBarX1.Value = (int)st.Elapsed.TotalSeconds;
            }
            catch
            {

            }
        }
        Stopwatch st = new Stopwatch();
        private void CVPlay_Shown(object sender, EventArgs e)
        {
            progressBarX1.Maximum = (int)cvpr.Duration;
            progressBarX1.Minimum = 0;
            st.Start();
            cvpr.Play();
        }

        private void progressBarX1_Click(object sender, EventArgs e)
        {

        }

        private void CVPlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            cvpr.Stop();
        }
    }
}