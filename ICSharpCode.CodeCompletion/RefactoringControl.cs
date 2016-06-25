using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit;
using ICSharpCode.NRefactory.AL.Refactoring;
using DevComponents.DotNetBar;

namespace ICSharpCode.CodeCompletion
{
    public partial class RefactoringControl : UserControl
    {
        public RefactoringControl()
        {
            InitializeComponent();
        }
        ALRefactoring.ALRefactoringContext CurrentContext;
        ALRefactoring.Refactoring Refactor;
        private void addMessageToList(RefactoringEntry message)
        {
            try
            {
                if (message.CodeIssueOrAction is CodeAction)
                {
                    CodeAction msg = (CodeAction)message.CodeIssueOrAction;
                    ListViewItem item = messagesListView.Items.Add(new ListViewItem(msg.Description));
                    item.SubItems.Add(msg.Severity.ToString());
                    item.SubItems.Add(message.StartOffset.ToString());
                    item.SubItems.Add(message.EndOffset.ToString());
                    item.ToolTipText = "Double click to repair the code";
                    item.Tag = message;
                    item.ImageIndex = 0;
                }
                else
                {
                     CodeIssue msg = (CodeIssue)message.CodeIssueOrAction;
                  ListViewItem item = messagesListView.Items.Add(new ListViewItem(msg.Description));
       if(msg.Actions.Count > 0)
                    item.SubItems.Add(msg.Actions[0].Severity.ToString());
       else item.SubItems.Add("Tip");
                    item.SubItems.Add(message.StartOffset.ToString());
                    item.SubItems.Add(message.EndOffset.ToString());
                
                    item.Tag = message;
                    item.ImageIndex = 0;
                }
            }
            catch
            {

            }


        }
        private delegate void addMessageDelegate(RefactoringEntry message);
        public void AddEntry(RefactoringEntry e, ALRefactoring.ALRefactoringContext context, ALRefactoring.Refactoring refactor)
        {
            try
            {
                CurrentContext = context;
                Refactor = refactor;
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

                //if (messagesListView.SelectedItems.Count > 0)
                //{

                //    ListViewItem item = messagesListView.SelectedItems[0];
                //    if (CurrentContext != null)
                //    {

                //        RefactoringEntry ent = (RefactoringEntry)item.Tag;
                //        CodeIssue cact = (CodeIssue)ent.CodeIssueOrAction;
                //            RefactorCorrection frm = new RefactorCorrection(cact.Actions.ToArray(), Refactor, CurrentContext);
                //            frm.ShowDialog();
                //            messagesListView.Items.RemoveAt(messagesListView.SelectedIndices[0]);
                //    }
                //}
            }
            catch
            {

            }
        }

        private void messagesListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {

                if (messagesListView.SelectedItems.Count > 0)
                {

                    ListViewItem item = messagesListView.SelectedItems[0];
                    if (CurrentContext != null)
                    {

                        RefactoringEntry ent = (RefactoringEntry)item.Tag;
                        CodeIssue cact = (CodeIssue)ent.CodeIssueOrAction;
                        if (cact.Actions.Count > 0)
                        {
                            RefactorCorrection frm = new RefactorCorrection(cact.Actions.ToArray(), Refactor, CurrentContext);
                            frm.ShowDialog();

                            messagesListView.Items.RemoveAt(messagesListView.SelectedIndices[0]);
                        }
                        else
                            MessageBoxEx.Show("No code action for this issue", "Code Action ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {

            }
        }


   private void messagesListView_Resize(object sender, EventArgs e)
        {
            this.desccol.Width = (int)(0.5608 * this.Width);
        }
        
    }
}
