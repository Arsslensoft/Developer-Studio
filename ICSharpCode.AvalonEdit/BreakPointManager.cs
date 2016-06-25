using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ICSharpCode.AvalonEdit
{
    public delegate void BreakpointEventHandler(BreakPoint bp,string file);
    public class BreakPoint
    {
        public int Line {get;set;}
        public BreakPointMarker Marker { get; set; }
        public BreakPointState State { get; set; }
    }
  public class BreakPointManager
    {
      public event BreakpointEventHandler OnBreakpointAdded;
      public event BreakpointEventHandler OnBreakpointRemoved;
      public event BreakpointEventHandler OnBreakpointModified;
      public List<BreakPoint> BreakPoints { get; set; }
      public string FileName { get; set; }
      public TextEditor Editor;

      public BreakPointManager(TextEditor editor, string file)
      {
          BreakPoints = new List<BreakPoint>();
          Editor = editor;
          FileName = file;
      }

      public int GetIndexOfBreakPoint(int line)
      {
         
          for (int i = 0; i < BreakPoints.Count; i++)
          {
              if (BreakPoints[i].Line == line)
                  return i;
          }
          return -1;
      }
      public bool Contains(int line)
      {
          foreach (BreakPoint b in BreakPoints)
          {
              if (b.Line == line)
                  return true;
          }
          return false;
      }
      public void SetBreakPointState(int Line, BreakPointState ST)
      {
          try
          {
              try
              {
                  int ind = GetIndexOfBreakPoint(Line);
                  if (ind > -1)
                  {
                      BreakPoint b = BreakPoints[ind];
                      b.State = ST;
                      BreakPointMarker bm = null;
                      foreach (IVisualLineTransformer iv in Editor.TextArea.TextView.LineTransformers)
                      {
                          if (iv is BreakPointMarker)
                          {
                             bm = (BreakPointMarker)iv;
                              if (bm.Line == Line)
                                  break;
                          }
                      }
                      if (bm != null)
                          Editor.TextArea.TextView.LineTransformers.Remove(bm);

                      b.Marker.State = ST;

                      if (OnBreakpointModified != null)
                          OnBreakpointModified(b,this.FileName);

                      Editor.TextArea.TextView.LineTransformers.Add(b.Marker);
                      Editor.TextArea.TextView.Redraw();
                  }
              }
              catch
              {

              }
          }
          catch
          {

          }
      }
      public void AddMarker2(int Line)
      {
          try
          {
              if (!Contains(Line) && Editor.Document.LineCount > Line && Line > 0)
              {
                  BreakPoint b = new BreakPoint();
                  b.Line = Line;
                  b.Marker = new BreakPointMarker();
                  b.Marker.Line = Line;
                  b.Marker.Document = Editor.Document;
                  b.Marker.StartOffset = Editor.Document.GetLineByNumber(Line).Offset;
                  b.Marker.EndOffset = Editor.Document.GetLineByNumber(Line).EndOffset;
                  b.Marker.State = BreakPointState.Normal;
                  b.State = BreakPointState.Normal;
                  BreakPoints.Add(b);
                  Editor.TextArea.TextView.LineTransformers.Add(b.Marker);
                  Editor.TextArea.TextView.Redraw();
              }
          }
          catch
          {

          }

      }
      public void RemoveMarker2(int line)
      {
          try
          {

              BreakPointMarker bk = null;
              BreakPoint bmk = null;
              foreach (BreakPoint b in BreakPoints)
              {
                  if (b.Line == line)
                  {
                      bmk = b;
                      break;
                  }
              }
              foreach (IVisualLineTransformer b in Editor.TextArea.TextView.LineTransformers)
              {
                  if (b is BreakPointMarker)
                  {
                      bk = (BreakPointMarker)b;
                      if (bk.Line == line)
                          break;

                  }
              }
              if (bk != null && bmk != null)
              {
                  BreakPoints.Remove(bmk);
                  Editor.TextArea.TextView.LineTransformers.Remove(bk);
               
              }


          }
          catch
          {

          }
      }
      public void AddMarker(int Line)
      {
          try
          {
              if (!Contains(Line) && Editor.Document.LineCount > Line && Line > 0)
              {
                  BreakPoint b = new BreakPoint();
                  b.Line = Line;
                  b.Marker = new BreakPointMarker();
                  b.Marker.Line = Line;
                  b.Marker.Document = Editor.Document;
                  b.Marker.StartOffset = Editor.Document.GetLineByNumber(Line).Offset;
                  b.Marker.EndOffset = Editor.Document.GetLineByNumber(Line).EndOffset;
                  b.Marker.State = BreakPointState.Normal;
                  b.State = BreakPointState.Normal;
                  BreakPoints.Add(b);
                  if(OnBreakpointAdded != null)
                      OnBreakpointAdded(b, this.FileName);
                  Editor.TextArea.TextView.LineTransformers.Add(b.Marker);
                  Editor.TextArea.TextView.Redraw();
              }
          }
          catch
          {

          }
      
      }
      public void RemoveMarker(int line)
      {
          try
          {
            
              BreakPointMarker bk = null;
              BreakPoint bmk = null;
              foreach (BreakPoint b in BreakPoints)
              {
                  if (b.Line == line)
                  {
                      bmk = b;
                      break;
                  }
              }
              foreach (IVisualLineTransformer b in Editor.TextArea.TextView.LineTransformers)
              {
                  if (b is BreakPointMarker)
                  {
                      bk = (BreakPointMarker)b;
                      if (bk.Line == line)
                          break;
                    
                  }
              }
              if (bk != null && bmk != null)
              {
                  BreakPoints.Remove(bmk);
                  Editor.TextArea.TextView.LineTransformers.Remove(bk);
                  if (OnBreakpointRemoved != null)
                      OnBreakpointRemoved(bmk, this.FileName);
              }

           
          }
          catch
          {

          }
      }

      public void LoadBreaks(string dir, string file)
      {
          try
          {
              if (File.Exists(dir + @"\" + file + ".breaks"))
              {
                  foreach (string line in File.ReadAllLines(dir + @"\" + file + ".breaks"))
                  {
                      string[] x = line.Split('=');
                      if (x.Length  == 2)
                      {
                          Editor.PicturePanel.AddaBreakPoint(int.Parse(x[0]));
                          BreakPointState st = (BreakPointState)byte.Parse(x[1]);
                          SetBreakPointState(int.Parse(x[0]), st);
                      }
                  }
              }
          }
          catch
          {

          }
      }
      public void Save(string dir, string file)
      {
          try
          {
              using (StreamWriter str = new StreamWriter(dir + @"\" + file + ".breaks", false))
              {
                  foreach (BreakPoint bp in BreakPoints)
                      str.WriteLine(bp.Line + "=" + ((byte)bp.State).ToString());
                  
              }
          }
          catch
          {

          }
      }
    }
}
