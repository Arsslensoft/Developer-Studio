using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ICSharpCode.NRefactory.AL.Refactoring;

namespace ICSharpCode.CodeCompletion
{
    public partial class RefactorCorrection : DevComponents.DotNetBar.Metro.MetroForm
    {
        
        public RefactorCorrection(CodeAction[] acts, ALRefactoring.Refactoring refactor, ALRefactoring.ALRefactoringContext context)
        {
            InitializeComponent();
            foreach (CodeAction act in acts)
            {
                ButtonItem b = new ButtonItem();
                b.Text = act.Description;
                b.Text += "\nSeverity : " + act.Severity.ToString();
                b.Name = act.Description;
                b.Click += new EventHandler((sender, e) =>
                  {
                      refactor.Run(act, context);
                      this.Close();
                  });
                itemPanel1.Items.Add(b);
            }
        }
    }
}