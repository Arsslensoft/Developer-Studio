using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace devstd.utils
{
    public partial class TaskForm : DevComponents.DotNetBar.Metro.MetroForm
    {
        DevStdTask task ;

        public TaskForm(DevStdTask dtask)
        {
            InitializeComponent();
            task = dtask;
           // task.OnComplete += timer1_Tick;

        }

        void SetText(object control, object val)
        {
            try
            {
                if (this.labelX1.InvokeRequired)
                {
                    this.BeginInvoke(new ControlUpdateDelegate(SetText));
                }
                else
                {
                    labelX1.Text = val.ToString();
                }
            }
            catch
            {

            }
        }

        private void TaskForm_Load(object sender, EventArgs e)
        {
            SetText(labelX1, task.Description);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (task.Done)
                this.Close();
            SetText(labelX1,task.Description);
            
        }

    }
}