using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ICSharpCode.AvalonEdit
{
    public class RefactoringEntry
    {
        public object CodeIssueOrAction;
        public int StartOffset;
        public int EndOffset;
        public int Line;
        public RefactoringEntry(object cia, int st, int end,int line)
        {
            CodeIssueOrAction = cia;
            StartOffset = st;
            EndOffset = end;
            Line = line;
        }
        
    }

   public class RefactoringManager 
   {

       public TextEditor Ceditor { get; set; }
       public List<RefactoringEntry> Entries { get; set; }
       public RefactoringErrorColorizer Colorizer { get; set; }
       public RefactoringManager(TextEditor edit)
       {
           Ceditor = edit;
           Entries = new List<RefactoringEntry>();
           Colorizer = new RefactoringErrorColorizer();
           Colorizer.Manager = this;

           edit.TextArea.TextView.LineTransformers.Add(Colorizer);
       }
      
       int IndexOf(RefactoringEntry ent)
       {
           for(int i =0; i < Entries.Count; i++)
               if(ent.Line == Entries[i].Line)
                   return i;

           return -1;
       }
       public bool RemoveEntry(RefactoringEntry ent)
       {
           //int i = IndexOf(ent);
           if (Entries.Contains(ent))
           {
     
               Entries.Remove(ent);
           }
           else return false;

           return true;
       }
       public bool AddEntry(RefactoringEntry ent)
       {
           if (!Entries.Contains(ent))
           {
               Entries.Add(ent);
   
           }
           else return false;

           return true;
       }

       public void ShowList(int line)
       {

       }
    }

   public class RefactoringErrorColorizer : DocumentColorizingTransformer
   {
       public RefactoringManager Manager;
       private readonly TextDecorationCollection collection;
       public RefactoringErrorColorizer()
       {
           // Create the Text decoration collection for the visual effect - you can get creative here
           collection = new TextDecorationCollection();
           var dec = new TextDecoration();
           dec.Pen = new Pen { Thickness = 1, DashStyle = DashStyles.DashDot, Brush = new SolidColorBrush(Colors.Red) };
           dec.PenThicknessUnit = TextDecorationUnit.FontRecommended;
           collection.Add(dec);

       }

       protected override void ColorizeLine(DocumentLine line)
       {
           int start = line.Offset;
           int end = line.EndOffset;
           try
           {
               foreach (RefactoringEntry ent in Manager.Entries)
               {
                   if (ent.Line == line.LineNumber && ent.StartOffset  >= line.Offset && ent.EndOffset <= line.EndOffset)
                   {
                       end = ent.EndOffset;
                       start = ent.StartOffset;
                       if (end != start)
                           base.ChangeLinePart(start, end, (VisualLineElement element) => element.TextRunProperties.SetTextDecorations(collection));
                   }
               }
           }
           catch
           {

           }
       }
   }
}
