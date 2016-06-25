﻿// Copyright (c) 2010-2013 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using ICSharpCode.NRefactory.AL.Resolver;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.AL.TypeSystem
{
	/// <summary>
	/// DocumentationComment with C# cref lookup.
	/// </summary>
	sealed class ALDocumentationComment : DocumentationComment
	{
		public ALDocumentationComment(ITextSource xmlDoc, ITypeResolveContext context) : base(xmlDoc, context)
		{
		}
		
		public override IEntity ResolveCref(string cref)
		{
			if (cref.Length > 2 && cref[1] == ':') {
				// resolve ID string
				return base.ResolveCref(cref);
			}
			var documentationReference = new ALParser().ParseDocumentationReference(cref);
			var ALContext = context as ALTypeResolveContext;
			ALResolver resolver;
			if (ALContext != null) {
				resolver = new ALResolver(ALContext);
			} else {
				resolver = new ALResolver(context.Compilation);
			}
			var astResolver = new ALAstResolver(resolver, documentationReference);
			var rr = astResolver.Resolve(documentationReference);
			
			MemberResolveResult mrr = rr as MemberResolveResult;
			if (mrr != null)
				return mrr.Member;
			TypeResolveResult trr = rr as TypeResolveResult;
			if (trr != null)
				return trr.Type.GetDefinition();
			return null;
		}
	}
}