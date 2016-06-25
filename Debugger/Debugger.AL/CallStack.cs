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
    public partial class CallStack : UserControl
    {
        public CallStack()
        {
            InitializeComponent();
        }
        public event GetFileFromTemp OnGetTemp;
        public event EventHandler OnStackSelected;
        private void addMessageToList(StackFrame message)
        {
            try
            {
                ListViewItem item = messagesListView.Items.Add(new ListViewItem(message.MethodInfo.FullName));

                item.SubItems.Add(OnGetTemp(message.NextStatement.Filename));

                item.SubItems.Add(message.NextStatement.StartLine.ToString());
                item.SubItems.Add(message.NextStatement.StartColumn.ToString());
                item.Tag = message;
                item.ImageIndex = 0;
            }
            catch
            {

            }


        }
        private delegate void addMessageDelegate(StackFrame message);
        void AddStack(StackFrame e)
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
        public void FillStackCtrl(Thread threads)
        {
            try
            {
                messagesListView.Items.Clear();
                foreach (StackFrame thr in threads.GetCallstack())
                    AddStack(thr);


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
    }
}
