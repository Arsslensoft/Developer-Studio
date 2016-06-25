using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace devstd.lang
{
    public class AnyCompletionManager
    {
        public string SourceFile { get; set; }
        public CompletionWindow completionWindow { get; set; }
        internal AnyCodeCompletionProvider completion { get; set; }
        public AnyCompletionManager()
        {
            completion = new AnyCodeCompletionProvider();
        }
        void ShowCompletion(byte type, TextEditor editor)
        {
            // open code completion after the user has pressed dot:
            completionWindow = new CompletionWindow(editor.TextArea);
            completionWindow.CompletionList.IsFiltering = false;
            // provide AvalonEdit with the data:
            IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
            foreach (MyCompletionData md in completion.GenerateCompletionData(editor.Document.FileName))
                data.Add(md);
            completionWindow.CloseAutomatically = true;

            completionWindow.Show();
            completionWindow.Closed += delegate
            {
                completionWindow = null;
            };
        }
        public void TextEntered(object sender, TextCompositionEventArgs e, TextEditor editor)
        {
            try
            {
                if (completionWindow == null)
                {
              
                        string line = editor.Document.GetText(editor.Document.GetLineByNumber(editor.TextArea.Caret.Line));
                        if (!IsInString(line) && !IsInChar(line))
                            ShowCompletion(1, editor);
                    
                }
            }
            catch
            {

            }
        }
        public void TextEntering(object sender, TextCompositionEventArgs e)
        {
   
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (e.Text == " ")
                    completionWindow.Close();
                else if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // do not set e.Handled=true - we still want to insert the character that was typed
        }
        public bool IsInChar(string line)
        {
            int qc = 0;
            // count how much string quotes before position except \'

            int p = line.IndexOf('\'');
            while (p > -1)
            {
                if (p > 0)
                {
                    // check for \
                    if (line[p - 1] != '\\')
                    {
                        qc++;
                        line = line.Remove(p, 1);
                    }
                    else
                        line = line.Remove(p - 1, 2);

                }
                else
                {
                    qc++;
                    line = line.Remove(p, 1);
                }


                p = line.IndexOf('\'');
            }

            return (qc % 2 != 0);
        }
        public bool IsInString(string line)
        {
            int qc = 0;
            // count how much string quotes before position except \'

            int p = line.IndexOf('"');
            while (p > -1)
            {
                if (p > 0)
                {
                    // check for \
                    if (line[p - 1] != '\\')
                    {
                        qc++;
                        line = line.Remove(p, 1);
                    }
                    else
                        line = line.Remove(p - 1, 2);

                }
                else
                {
                    qc++;
                    line = line.Remove(p, 1);
                }


                p = line.IndexOf('"');
            }

            return (qc % 2 != 0);
        }
    }
}
