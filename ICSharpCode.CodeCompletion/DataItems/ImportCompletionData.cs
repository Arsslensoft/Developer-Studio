// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.AL.Refactoring;
using ICSharpCode.NRefactory.AL.Resolver;
using ICSharpCode.NRefactory.AL.TypeSystem;
using ICSharpCode.NRefactory.Completion;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.CodeCompletion.DataItems
{
    /// <summary>
    /// Completion item that introduces a using declaration.
    /// </summary>
    class ImportCompletionData : EntityCompletionData
    {
        string insertUsing;
        string insertionText;

        public ImportCompletionData(ITypeDefinition typeDef, ALTypeResolveContext contextAtCaret, bool useFullName)
            : base(typeDef)
        {
            this.Description = "include " + typeDef.Namespace + ";";
            if (useFullName)
            {
                var astBuilder = new TypeSystemAstBuilder(new ALResolver(contextAtCaret));
                insertionText = astBuilder.ConvertType(typeDef).GetText();
            }
            else
            {
                insertionText = typeDef.Name;
                insertUsing = typeDef.Namespace;
            }
        }
    } //end class ImportCompletionData
}
