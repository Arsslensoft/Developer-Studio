using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace devstd.lang
{
  public  class PawnCompletionManager
    {
      public string SourceFile { get; set; }
      public CompletionWindow completionWindow { get; set; }
      internal PawnCodeCompletionProvider completion { get; set; }
      public PawnCompletionManager()
      {
          completion = new PawnCodeCompletionProvider();
      }
      void ShowCompletion(byte type,TextEditor editor)
      {
          // open code completion after the user has pressed dot:
          completionWindow = new CompletionWindow(editor.TextArea);
          // provide AvalonEdit with the data:
          IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
          foreach (MyCompletionData md in completion.GenerateCompletionData(type))
              data.Add(md);
 
          completionWindow.CompletionList.IsFiltering = false;
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

                  if (line.ToLower().Contains("#include") && line.ToLower().Contains("<"))        
                      {
                          ShowCompletion(2, editor);
                      }
                      else if (IsInString(line))
                      {

                      }
                  else if (IsInAssignement(line))
                      ShowCompletion(1, editor);
                  else if (PawnCodeCompletionProvider.IsInType(line))
                      {
                          try
                          {
                              string word = PawnCodeCompletionProvider.GetWordBeforePosition(line);
                              if (word == "new" && !line.Contains(":"))
                                  ShowCompletion(0, editor);
                              else if (!line.Contains(":"))
                                  ShowCompletion(0, editor);
                          }
                          catch
                          {
                              ShowCompletion(0, editor);
                          }
                      }
                    
                      else
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
              else if (!(char.IsLetterOrDigit(e.Text[0]) || e.Text == "_" || e.Text == "#"))
              {
                  
                  // Whenever a non-letter is typed while the completion window is open,
                  // insert the currently selected element.
                  completionWindow.CompletionList.RequestInsertion(e);
              }
          }
          // do not set e.Handled=true - we still want to insert the character that was typed
      }
     public bool IsInType(string line)
     {
         int x = 0;

         int p = line.LastIndexOf(":");
         int semi = line.LastIndexOf(';');

         if (p > 0)
             return (char.IsLetter(line[p - 1]) && (semi < p));
         else return false;
     }
     public bool IsInAssignement(string line)
     {

         if (line.Length > 2)
         {
             int i = line.Length - 1;
             while (i > 0)
             {
                 if (!char.IsWhiteSpace(line[i]))
                 {
                     if (line[i] == '=')
                         return true;
                     else
                         return false;
                 }
                 i--;
             }
             return false;
         }
         else
             return false;
     }
     public bool IsInString(string line)
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
    }
}
