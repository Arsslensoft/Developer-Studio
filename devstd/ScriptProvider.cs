using ALCodeDomProvider;
using alproj;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.CodeCompletion;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using TextView = ICSharpCode.AvalonEdit.Rendering.TextView;

namespace devstd
{

   
        /// <summary>
        /// Represents a text marker.
        /// </summary>
        public interface ITextMarker
        {
            /// <summary>
            /// Gets the start offset of the marked text region.
            /// </summary>
            int StartOffset { get; }

            /// <summary>
            /// Gets the end offset of the marked text region.
            /// </summary>
            int EndOffset { get; }

            /// <summary>
            /// Gets the length of the marked region.
            /// </summary>
            int Length { get; }

            /// <summary>
            /// Deletes the text marker.
            /// </summary>
            void Delete();

            /// <summary>
            /// Gets whether the text marker was deleted.
            /// </summary>
            bool IsDeleted { get; }

            /// <summary>
            /// Event that occurs when the text marker is deleted.
            /// </summary>
            event EventHandler Deleted;

            /// <summary>
            /// Gets/Sets the background color.
            /// </summary>
            Color? BackgroundColor { get; set; }

            /// <summary>
            /// Gets/Sets the foreground color.
            /// </summary>
            Color? ForegroundColor { get; set; }

            /// <summary>
            /// Gets/Sets the font weight.
            /// </summary>
            FontWeight? FontWeight { get; set; }

            /// <summary>
            /// Gets/Sets the font style.
            /// </summary>
            FontStyle? FontStyle { get; set; }

            /// <summary>
            /// Gets/Sets the type of the marker. Use TextMarkerType.None for normal markers.
            /// </summary>
            TextMarkerTypes MarkerTypes { get; set; }

            /// <summary>
            /// Gets/Sets the color of the marker.
            /// </summary>
            Color MarkerColor { get; set; }

            /// <summary>
            /// Gets/Sets an object with additional data for this text marker.
            /// </summary>
            object Tag { get; set; }

            /// <summary>
            /// Gets/Sets an object that will be displayed as tooltip in the text editor.
            /// </summary>
            object ToolTip { get; set; }
        }

        [Flags]
        public enum TextMarkerTypes
        {
            /// <summary>
            /// Use no marker
            /// </summary>
            None = 0x0000,
            /// <summary>
            /// Use squiggly underline marker
            /// </summary>
            SquigglyUnderline = 0x001,
            /// <summary>
            /// Normal underline.
            /// </summary>
            NormalUnderline = 0x002,
            /// <summary>
            /// Dotted underline.
            /// </summary>
            DottedUnderline = 0x004,

            /// <summary>
            /// Horizontal line in the scroll bar.
            /// </summary>
            LineInScrollBar = 0x0100,
            /// <summary>
            /// Small triangle in the scroll bar, pointing to the right.
            /// </summary>
            ScrollBarRightTriangle = 0x0400,
            /// <summary>
            /// Small triangle in the scroll bar, pointing to the left.
            /// </summary>
            ScrollBarLeftTriangle = 0x0800,
            /// <summary>
            /// Small circle in the scroll bar.
            /// </summary>
            CircleInScrollBar = 0x1000
        }

        public interface ITextMarkerService
        {
            /// <summary>
            /// Creates a new text marker. The text marker will be invisible at first,
            /// you need to set one of the Color properties to make it visible.
            /// </summary>
            ITextMarker Create(int startOffset, int length);

            /// <summary>
            /// Gets the list of text markers.
            /// </summary>
            IEnumerable<ITextMarker> TextMarkers { get; }

            /// <summary>
            /// Removes the specified text marker.
            /// </summary>
            void Remove(ITextMarker marker);

            /// <summary>
            /// Removes all text markers that match the condition.
            /// </summary>
            void RemoveAll(Predicate<ITextMarker> predicate);

            /// <summary>
            /// Finds all text markers at the specified offset.
            /// </summary>
            IEnumerable<ITextMarker> GetMarkersAtOffset(int offset);
        }
    

	/// <summary>
	/// Handles the text markers for a code editor.
	/// </summary>
	sealed class TextMarkerService : DocumentColorizingTransformer, IBackgroundRenderer, ITextMarkerService
	{
		TextSegmentCollection<TextMarker> markers;
		TextView textView;
		
		public TextMarkerService(TextView textView)
		{
			if (textView == null)
				throw new ArgumentNullException("textView");
			this.textView = textView;
			textView.DocumentChanged += OnDocumentChanged;
			OnDocumentChanged(null, null);
		}

		void OnDocumentChanged(object sender, EventArgs e)
		{
			if (textView.Document != null)
				markers = new TextSegmentCollection<TextMarker>(textView.Document);
			else
				markers = null;
		}
		
		#region ITextMarkerService
		public ITextMarker Create(int startOffset, int length)
		{
			if (markers == null)
				throw new InvalidOperationException("Cannot create a marker when not attached to a document");
			
			int textLength = textView.Document.TextLength;
			if (startOffset < 0 || startOffset > textLength)
				throw new ArgumentOutOfRangeException("startOffset", startOffset, "Value must be between 0 and " + textLength);
			if (length < 0 || startOffset + length > textLength)
				throw new ArgumentOutOfRangeException("length", length, "length must not be negative and startOffset+length must not be after the end of the document");
			
			TextMarker m = new TextMarker(this, startOffset, length);
			markers.Add(m);
			// no need to mark segment for redraw: the text marker is invisible until a property is set
			return m;
		}
		
		public IEnumerable<ITextMarker> GetMarkersAtOffset(int offset)
		{
			if (markers == null)
				return Enumerable.Empty<ITextMarker>();
			else
				return markers.FindSegmentsContaining(offset);
		}
		
		public IEnumerable<ITextMarker> TextMarkers {
			get { return markers ?? Enumerable.Empty<ITextMarker>(); }
		}
		
		public void RemoveAll(Predicate<ITextMarker> predicate)
		{
			if (predicate == null)
				throw new ArgumentNullException("predicate");
			if (markers != null) {
				foreach (TextMarker m in markers.ToArray()) {
					if (predicate(m))
						Remove(m);
				}
			}
		}
		
		public void Remove(ITextMarker marker)
		{
			if (marker == null)
				throw new ArgumentNullException("marker");
			TextMarker m = (TextMarker)marker  ;
			if (markers != null && markers.Remove(m)) {
				Redraw(m);
				m.OnDeleted();
			}
		}
		
		/// <summary>
		/// Redraws the specified text segment.
		/// </summary>
		internal void Redraw(ISegment segment)
		{
			textView.Redraw(segment, DispatcherPriority.Normal);
			if (RedrawRequested != null)
				RedrawRequested(this, EventArgs.Empty);
		}
		
		public event EventHandler RedrawRequested;
		#endregion
		
		#region DocumentColorizingTransformer
		protected override void ColorizeLine(DocumentLine line)
		{
			if (markers == null)
				return;
			int lineStart = line.Offset;
			int lineEnd = lineStart + line.Length;
			foreach (TextMarker marker in markers.FindOverlappingSegments(lineStart, line.Length)) {
				Brush foregroundBrush = null;
				if (marker.ForegroundColor != null) {
					foregroundBrush = new SolidColorBrush(marker.ForegroundColor.Value);
					foregroundBrush.Freeze();
				}
				ChangeLinePart(
					Math.Max(marker.StartOffset, lineStart),
					Math.Min(marker.EndOffset, lineEnd),
					element => {
						if (foregroundBrush != null) {
							element.TextRunProperties.SetForegroundBrush(foregroundBrush);
						}
						Typeface tf = element.TextRunProperties.Typeface;
						element.TextRunProperties.SetTypeface(new Typeface(
							tf.FontFamily,
							marker.FontStyle ?? tf.Style,
							marker.FontWeight ?? tf.Weight,
							tf.Stretch
						));
					}
				);
			}
		}
		#endregion
		
		#region IBackgroundRenderer
		public KnownLayer Layer {
			get {
				// draw behind selection
				return KnownLayer.Selection;
			}
		}
		
		public void Draw(ICSharpCode.AvalonEdit.Rendering.TextView textView, DrawingContext drawingContext)
		{
			if (textView == null)
				throw new ArgumentNullException("textView");
			if (drawingContext == null)
				throw new ArgumentNullException("drawingContext");
			if (markers == null || !textView.VisualLinesValid)
				return;
			var visualLines = textView.VisualLines;
			if (visualLines.Count == 0)
				return;
			int viewStart = visualLines.First().FirstDocumentLine.Offset;
			int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;
			foreach (TextMarker marker in markers.FindOverlappingSegments(viewStart, viewEnd - viewStart)) {
				if (marker.BackgroundColor != null) {
					BackgroundGeometryBuilder geoBuilder = new BackgroundGeometryBuilder();
					geoBuilder.AlignToWholePixels = true;
					geoBuilder.CornerRadius = 3;
					geoBuilder.AddSegment(textView, marker);
					Geometry geometry = geoBuilder.CreateGeometry();
					if (geometry != null) {
						Color color = marker.BackgroundColor.Value;
						SolidColorBrush brush = new SolidColorBrush(color);
						brush.Freeze();
						drawingContext.DrawGeometry(brush, null, geometry);
					}
				}
				var underlineMarkerTypes = TextMarkerTypes.SquigglyUnderline | TextMarkerTypes.NormalUnderline | TextMarkerTypes.DottedUnderline;
				if ((marker.MarkerTypes & underlineMarkerTypes) != 0) {
					foreach (Rect r in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker)) {
						Point startPoint = r.BottomLeft;
						Point endPoint = r.BottomRight;
						
						Brush usedBrush = new SolidColorBrush(marker.MarkerColor);
						usedBrush.Freeze();
						if ((marker.MarkerTypes & TextMarkerTypes.SquigglyUnderline) != 0) {
							double offset = 2.5;
							
							int count = Math.Max((int)((endPoint.X - startPoint.X) / offset) + 1, 4);
							
							StreamGeometry geometry = new StreamGeometry();
							
							using (StreamGeometryContext ctx = geometry.Open()) {
								ctx.BeginFigure(startPoint, false, false);
								ctx.PolyLineTo(CreatePoints(startPoint, endPoint, offset, count).ToArray(), true, false);
							}
							
							geometry.Freeze();
							
							Pen usedPen = new Pen(usedBrush, 1);
							usedPen.Freeze();
							drawingContext.DrawGeometry(Brushes.Transparent, usedPen, geometry);
						}
						if ((marker.MarkerTypes & TextMarkerTypes.NormalUnderline) != 0) {
							Pen usedPen = new Pen(usedBrush, 1);
							usedPen.Freeze();
							drawingContext.DrawLine(usedPen, startPoint, endPoint);
						}
						if ((marker.MarkerTypes & TextMarkerTypes.DottedUnderline) != 0) {
							Pen usedPen = new Pen(usedBrush, 1);
							usedPen.DashStyle = DashStyles.Dot;
							usedPen.Freeze();
							drawingContext.DrawLine(usedPen, startPoint, endPoint);
						}
					}
				}
			}
		}
		
		IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
		{
			for (int i = 0; i < count; i++)
				yield return new Point(start.X + i * offset, start.Y - ((i + 1) % 2 == 0 ? offset : 0));
		}
		#endregion
	}
	
	sealed class TextMarker : TextSegment, ITextMarker
	{
		readonly TextMarkerService service;
		
		public TextMarker(TextMarkerService service, int startOffset, int length)
		{
			if (service == null)
				throw new ArgumentNullException("service");
			this.service = service;
			this.StartOffset = startOffset;
			this.Length = length;
			this.markerTypes = TextMarkerTypes.None;
		}
		
		public event EventHandler Deleted;
		
		public bool IsDeleted {
			get { return !this.IsConnectedToCollection; }
		}
		
		public void Delete()
		{
			service.Remove(this);
		}
		
		internal void OnDeleted()
		{
			if (Deleted != null)
				Deleted(this, EventArgs.Empty);
		}
		
		void Redraw()
		{
			service.Redraw(this);
		}
		
		Color? backgroundColor;
		
		public Color? BackgroundColor {
			get { return backgroundColor; }
			set {
				if (backgroundColor != value) {
					backgroundColor = value;
					Redraw();
				}
			}
		}
		
		Color? foregroundColor;
		
		public Color? ForegroundColor {
			get { return foregroundColor; }
			set {
				if (foregroundColor != value) {
					foregroundColor = value;
					Redraw();
				}
			}
		}
		
		FontWeight? fontWeight;
		
		public FontWeight? FontWeight {
			get { return fontWeight; }
			set {
				if (fontWeight != value) {
					fontWeight = value;
					Redraw();
				}
			}
		}
		
		FontStyle? fontStyle;
		
		public FontStyle? FontStyle {
			get { return fontStyle; }
			set {
				if (fontStyle != value) {
					fontStyle = value;
					Redraw();
				}
			}
		}
		
		public object Tag { get; set; }
		
		TextMarkerTypes markerTypes;
		
		public TextMarkerTypes MarkerTypes {
			get { return markerTypes; }
			set {
				if (markerTypes != value) {
					markerTypes = value;
					Redraw();
				}
			}
		}
		
		Color markerColor;
		
		public Color MarkerColor {
			get { return markerColor; }
			set {
				if (markerColor != value) {
					markerColor = value;
					Redraw();
				}
			}
		}
		
		public object ToolTip { get; set; }
	}




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
        protected override void ColorizeLine(DocumentLine line )
        {
            if (line.Length == 0)
                return;

            if (line.Offset < StartOffset || line.Offset > EndOffset)
                return;

     
            int start = line.Offset > StartOffset ? line.Offset : StartOffset;
            int end = EndOffset > line.EndOffset ? line.EndOffset : EndOffset;
              SolidColorBrush f = new SolidColorBrush(Color.FromRgb(180, 38, 38));
            if(State == BreakPointState.Hit)
               f = new SolidColorBrush(Color.FromRgb(222, 185, 0));
            else if (State == BreakPointState.Disabled)
                f = new SolidColorBrush(Color.FromRgb(232, 113, 113));

            ChangeLinePart(start, end, element => element.TextRunProperties.SetBackgroundBrush(  f ));
        } 
    }




    public class HighlightDebugLineBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor _editor;
        public int Line = 0;
        public Color color = Color.FromArgb(0x40, 0, 0, 0xFF);
        public HighlightDebugLineBackgroundRenderer(TextEditor editor)
        {
            _editor = editor;
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Background; }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_editor.Document == null)
                return;

            textView.EnsureVisualLines();
            var currentLine = _editor.Document.GetLineByNumber(Line);
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
            {
                drawingContext.DrawRectangle(
                    new SolidColorBrush(color), null,
                    new Rect(rect.Location, new Size(textView.ActualWidth - 32, rect.Height)));
            }
        }
    }
    public class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor _editor;

        public HighlightCurrentLineBackgroundRenderer(TextEditor editor)
        {
            _editor = editor;
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Caret; }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_editor.Document == null)
                return;

            textView.EnsureVisualLines();
            var currentLine = _editor.Document.GetLineByOffset(_editor.CaretOffset);
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
            {
                drawingContext.DrawRectangle(
                    new SolidColorBrush(Color.FromArgb(0x40, 0, 0, 0)), null,
                    new Rect(rect.Location, new Size(textView.ActualWidth - 32, rect.Height)));
            }
        }
    }
    /// <summary>
    /// This is a simple script provider that adds a few using statements to the C# scripts (.csx files)
    /// </summary>
    class ScriptProvider : ICSharpCode.CodeCompletion.ICSharpScriptProvider
    {
        public string GetUsing()
        {
            return "" +
                "using System; " +
                "using System.Collections.Generic; " +
                "using System.Linq; " +
               "using System.Windows.Forms; " +
                "using System.Text; ";
        }


        public string GetVars()
        {
            return null;
        }
    }
}
