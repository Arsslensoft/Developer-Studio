﻿#region License
// CShell, A Simple C# Scripting IDE
// Copyright (C) 2013  Arnova Asset Management Ltd., Lukas Buhler
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

// This file is based on code from the SharpDevelop project:
//   Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \Doc\sharpdevelop-copyright.txt)
//   This code is distributed under the GNU LGPL (for details please see \Doc\COPYING.LESSER.txt)
#endregion

using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.CodeCompletion.DataItems;
using ICSharpCode.NRefactory.Xml;
using System.ComponentModel;

namespace ICSharpCode.CodeCompletion
{
   public sealed class CSharpInsightItem
    {
        public readonly IParameterizedMember Method;

        public CSharpInsightItem(IParameterizedMember method)
        {
            this.Method = method;
        }

        FlowDocumentScrollViewer header;

        public object Header
        {
            get
            {
                if (header == null)
                {
                    header = GenerateHeader();
                    OnPropertyChanged("Header");
                }
                return header;
            }
        }

        int highlightedParameterIndex = -1;

        public void HighlightParameter(int parameterIndex)
        {
            if (highlightedParameterIndex == parameterIndex)
                return;
            this.highlightedParameterIndex = parameterIndex;
            if (header != null)
                header = GenerateHeader();
            OnPropertyChanged("Header");
            OnPropertyChanged("Content");
        }

        FlowDocumentScrollViewer GenerateHeader()
        {
            ALAmbience ambience = new ALAmbience();
            ambience.ConversionFlags = ConversionFlags.StandardConversionFlags;
            var stringBuilder = new StringBuilder();
            var formatter = new ParameterHighlightingOutputFormatter(stringBuilder, highlightedParameterIndex);
            ambience.ConvertEntity(Method, formatter, FormattingOptionsFactory.CreateSharpDevelop());

            var documentation = XmlDocumentationElement.Get(Method);
            ambience.ConversionFlags = ConversionFlags.ShowTypeParameterList;

            DocumentationUIBuilder b = new DocumentationUIBuilder(ambience);
            string parameterName = null;
            if (Method.Parameters.Count > highlightedParameterIndex)
                parameterName = Method.Parameters[highlightedParameterIndex].Name;
            b.AddSignatureBlock(stringBuilder.ToString(), formatter.parameterStartOffset, formatter.parameterLength, parameterName);

            DocumentationUIBuilder b2 = new DocumentationUIBuilder(ambience);
            b2.ParameterName = parameterName;
            b2.ShowAllParameters = false;

            if (documentation != null)
            {
                foreach (var child in documentation.Children)
                {
                    b2.AddDocumentationElement(child);
                }
            }

            content = new FlowDocumentScrollViewer
            {
                Document = b2.CreateFlowDocument(),
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            var flowDocument = b.CreateFlowDocument();
            flowDocument.PagePadding = new Thickness(0); // default is NOT Thickness(0), but Thickness(Auto), which adds unnecessary space

            return new FlowDocumentScrollViewer
            {
                Document = flowDocument,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
        }

        FlowDocumentScrollViewer content;

        public object Content
        {
            get
            {
                if (content == null)
                {
                    GenerateHeader();
                    OnPropertyChanged("Content");
                }
                return content;
            }
        }

        sealed class ParameterHighlightingOutputFormatter : TextWriterTokenWriter
        {
            StringBuilder b;
            int highlightedParameterIndex;
            int parameterIndex;
            internal int parameterStartOffset;
            internal int parameterLength;

            public ParameterHighlightingOutputFormatter(StringBuilder b, int highlightedParameterIndex)
                : base(new StringWriter(b))
            {
                this.b = b;
                this.highlightedParameterIndex = highlightedParameterIndex;
            }

            public override void StartNode(AstNode node)
            {
                if (parameterIndex == highlightedParameterIndex && node is ParameterDeclaration)
                {
                    parameterStartOffset = b.Length;
                }
                base.StartNode(node);
            }

            public override void EndNode(AstNode node)
            {
                base.EndNode(node);
                if (node is ParameterDeclaration)
                {
                    if (parameterIndex == highlightedParameterIndex)
                        parameterLength = b.Length - parameterStartOffset;
                    parameterIndex++;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
