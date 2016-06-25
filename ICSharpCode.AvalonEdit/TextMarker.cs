using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.NRefactory.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using TextView = ICSharpCode.AvalonEdit.Rendering.TextView;

namespace ICSharpCode.AvalonEdit
{

    public enum BreakPointState
    {
        Normal = 1,
        Hit = 2,
        Disabled = 3
    }
    public class BreakPointMarker : DocumentColorizingTransformer
    {
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
        public TextDocument Document { get; set; }
        public BreakPointState State { get; set; }
        public int Line { get; set; }
        protected override void ColorizeLine(DocumentLine line)
        {
            if (line.Length == 0)
                return;

            if (line.Offset < StartOffset || line.Offset > EndOffset)
                return;

           
            Line = line.LineNumber;
            int start = line.Offset > StartOffset ? line.Offset : StartOffset;
            int end = EndOffset > line.EndOffset ? line.EndOffset : EndOffset;
            SolidColorBrush f = new SolidColorBrush(Color.FromRgb(180, 38, 38));
            if (State == BreakPointState.Hit)
            {
                ChangeLinePart(start, end, element => element.TextRunProperties.SetForegroundBrush(Brushes.Black));
                f = new SolidColorBrush(Color.FromRgb(222, 185, 0));
            }
            else if (State == BreakPointState.Disabled)
                f = new SolidColorBrush(Color.FromRgb(232, 113, 113));
            else
                ChangeLinePart(start, end, element => element.TextRunProperties.SetForegroundBrush(Brushes.White));


            ChangeLinePart(start, end, element => element.TextRunProperties.SetBackgroundBrush(f));
        }
    }

}
