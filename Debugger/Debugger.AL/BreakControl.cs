using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Debugger.AL
{
    public partial class BreakControl : UserControl
    {
        public BreakControl()
        {
            InitializeComponent();
        }
        public event GetFileFromTemp OnGetTemp;
        public event EventHandler OnStackSelected;
        private void addMessageToList(Breakpoint message)
        {
            try
            {
                ListViewItem item = messagesListView.Items.Add(new ListViewItem(OnGetTemp(message.FileName)));
                item.SubItems.Add(message.Line.ToString());
                item.SubItems.Add(message.Enabled.ToString());
                item.Tag = message;
                item.ImageIndex = 0;
            }
            catch
            {

            }


        }
        private delegate void addMessageDelegate(Breakpoint message);
       public void AddBreak(Breakpoint e)
        {
            try
            {

                if (messagesListView.InvokeRequired)
                {
                    addMessageDelegate d = new addMessageDelegate(addMessageToList);
                    messagesListView.Invoke(d, e);
                }
                else
                {
                    addMessageToList(e);
                }

            }
            catch (Exception ex)
            {

            }
        }
        public void Clear()
        {
            try
            {
                messagesListView.Items.Clear();
            }
            catch
            {

            }
        }

        private void messagesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (messagesListView.SelectedItems.Count > 0)
                {

                    ListViewItem item = messagesListView.SelectedItems[0];
                    if (OnStackSelected != null)
                        OnStackSelected(item.Tag, e);

                }
            }
            catch
            {

            }
        }

        private void BreakControl_Resize(object sender, EventArgs e)
        {
            this.filecol.Width = (int)(0.8140 * this.Width);
        }
    }
}
