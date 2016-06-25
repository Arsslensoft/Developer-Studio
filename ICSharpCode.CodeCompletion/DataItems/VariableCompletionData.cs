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
    internal class VariableCompletionData : CompletionData, IVariableCompletionData
    {
        public VariableCompletionData(IVariable variable)
        {
            if (variable == null) throw new ArgumentNullException("variable");
            Variable = variable;

            IAmbience ambience = new ALAmbience();
            DisplayText = variable.Name;
            Description = ambience.ConvertVariable(variable);
            CompletionText = Variable.Name;
            this.Image = ICSharpCode.AvalonEdit.CodeCompletion.CompletionImage.Field.BaseImage;
        }

        public IVariable Variable { get; private set; }
    } //end class VariableCompletionData

}
