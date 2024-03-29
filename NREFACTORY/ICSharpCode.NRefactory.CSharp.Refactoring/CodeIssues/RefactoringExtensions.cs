//
// RefactoringExtensions.cs
//
// Author:
//       Simon Lindgren <simon.n.lindgren@gmail.com>
//
// Copyright (c) 2013 Simon Lindgren
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using System;

namespace ICSharpCode.NRefactory.AL
{
	static class RefactoringExtensions
	{
		/// <summary>
		/// Gets the local variable declaration space, as defined by §3.3 Declarations.
		/// </summary>
		/// <returns>The local variable declaration space.</returns>
		public static AstNode GetLocalVariableDeclarationSpace (this AstNode self)
		{
			var node = self.Parent;
			while (node != null && !CreatesLocalVariableDeclarationSpace(node))
				node = node.Parent;
			return node;
		}
		
		static ISet<Type> localVariableDeclarationSpaceCreators = new HashSet<Type> () {
			typeof(BlockStatement),
			typeof(SwitchStatement),
			typeof(ForeachStatement),
			typeof(ForStatement),
			typeof(UsingStatement),
			typeof(LambdaExpression),
			typeof(AnonymousMethodExpression)
		};

		static bool CreatesLocalVariableDeclarationSpace(AstNode node)
		{
			return localVariableDeclarationSpaceCreators.Contains(node.GetType());
		}
	}
}

