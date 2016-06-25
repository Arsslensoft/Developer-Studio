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
    public partial class ThreadCtrl : UserControl
    {
        public ThreadCtrl()
        {
            InitializeComponent();
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
        private void addMessageToList(Thread message)
        {
            try
            {
                ListViewItem item = messagesListView.Items.Add(new ListViewItem(message.ID.ToString()));

                item.SubItems.Add(message.Name);
                item.SubItems.Add(message.Priority.ToString());
                if (message.CurrentException != null)
                    item.SubItems.Add(message.CurrentExceptionType.ToString());
                else
                    item.SubItems.Add("NO");
                item.SubItems.Add(message.Suspended.ToString());
                item.SubItems.Add(message.HasExited.ToString());
                item.Tag = message;

                if (message.Suspended)
                    item.ImageIndex = 1;

                else if (message.HasExited)
                    item.ImageIndex = 2;
                else
                    item.ImageIndex = 0;
            }
            catch{
            }
            
        }
        private delegate void addMessageDelegate(Thread message);
        void AddThread(Thread e)
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

        public void FillThreadCtrl(List<Thread> threads)
        {
            try
            {
                messagesListView.Items.Clear();
                foreach (Thread thr in threads)
                    AddThread(thr);

                
            }
            catch
            {

            }
        }
    }
}
