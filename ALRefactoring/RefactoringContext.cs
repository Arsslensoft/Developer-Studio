using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Snippets;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.AL.Refactoring;
using ICSharpCode.NRefactory.AL.Resolver;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ALRefactoring
{
    public class ALRefactoringContext : RefactoringContext
    {
        public static bool UseExplict
        {
            get;
            set;
        }

        internal string defaultNamespace;
        public override string DefaultNamespace
        {
            get
            {
                return defaultNamespace;
            }
        }

        internal readonly IDocument doc;
        readonly TextLocation location;
        List<ALRefactoringContext> projectContexts;

        public ALRefactoringContext(IDocument document, TextLocation location, ALAstResolver resolver)
            : base(resolver, CancellationToken.None)
        {
            this.doc = document;
            this.location = location;
            this.UseExplicitTypes = UseExplict;
            this.FormattingOptions = FormattingOptionsFactory.CreateMono();
            UseExplict = false;
            Services.AddService(typeof(NamingConventionService), new TestNameService());
            Services.AddService(typeof(CodeGenerationService), new DefaultCodeGenerationService());
        }

        class TestNameService : NamingConventionService
        {
            public override IEnumerable<NamingRule> Rules
            {
                get
                {
                    return DefaultRules.GetFdgRules();
                }
            }
        }

        public override bool Supports(Version version)
        {
            return this.version == null || this.version.CompareTo(version) >= 0;
        }

        public override TextLocation Location
        {
            get { return location; }
        }

        public Version version;

        public ALFormattingOptions FormattingOptions { get; set; }

        public Script StartScript()
        {
            return new ALScript(this);
        }

      public  sealed class ALScript : DocumentScript
        {
            readonly ALRefactoringContext context;
            public ALScript(ALRefactoringContext context)
                : base(context.doc, context.FormattingOptions, new ICSharpCode.NRefactory.AL.TextEditorOptions())
            {
                this.context = context;
            }

            public override Task Link(params AstNode[] nodes)
            {
                // check that all links are valid.
                foreach (var node in nodes)
                {
                    //  Assert.IsNotNull(GetSegment(node));
                }
                return new Task(() => { });
            }

            public override Task<Script> InsertWithCursor(string operation, InsertPosition defaultPosition, IList<AstNode> nodes)
            {
                EntityDeclaration entity = context.GetNode<EntityDeclaration>();
                if (entity is Accessor)
                {
                    entity = (EntityDeclaration)entity.Parent;
                }

                foreach (var node in nodes)
                {
                    InsertBefore(entity, node);
                }
                var tcs = new TaskCompletionSource<Script>();
                tcs.SetResult(this);
                return tcs.Task;
            }

            public override Task<Script> InsertWithCursor(string operation, ITypeDefinition parentType, Func<Script, RefactoringContext, IList<AstNode>> nodeCallback)
            {
                var unit = context.RootNode;
                var insertType = unit.GetNodeAt<TypeDeclaration>(parentType.Region.Begin);

                var startOffset = GetCurrentOffset(insertType.LBraceToken.EndLocation);
                var nodes = nodeCallback(this, context);
                foreach (var node in nodes.Reverse())
                {
                    var output = OutputNode(1, node, true);
                    if (parentType.Kind == TypeKind.Enum)
                    {
                        InsertText(startOffset, output.Text + (!parentType.Fields.Any() ? "" : ","));
                    }
                    else
                    {
                        InsertText(startOffset, output.Text);
                    }
                    output.RegisterTrackedSegments(this, startOffset);
                }
                var tcs = new TaskCompletionSource<Script>();
                tcs.SetResult(this);
                return tcs.Task;
            }

            void Rename(AstNode node, string newName)
            {
                if (node is ObjectCreateExpression)
                    node = ((ObjectCreateExpression)node).Type;

                if (node is InvocationExpression)
                    node = ((InvocationExpression)node).Target;

                if (node is MemberReferenceExpression)
                    node = ((MemberReferenceExpression)node).MemberNameToken;

                if (node is MemberType)
                    node = ((MemberType)node).MemberNameToken;

                if (node is EntityDeclaration)
                    node = ((EntityDeclaration)node).NameToken;

                if (node is ParameterDeclaration)
                    node = ((ParameterDeclaration)node).NameToken;
                if (node is ConstructorDeclaration)
                    node = ((ConstructorDeclaration)node).NameToken;
                if (node is DestructorDeclaration)
                    node = ((DestructorDeclaration)node).NameToken;
                if (node is VariableInitializer)
                    node = ((VariableInitializer)node).NameToken;
                Replace(node, new IdentifierExpression(newName));
            }

            public override void Rename(ISymbol symbol, string name)
            {
                if (symbol.SymbolKind == SymbolKind.Variable || symbol.SymbolKind == SymbolKind.Parameter)
                {
                    Rename(symbol as IVariable, name);
                    return;
                }

                FindReferences refFinder = new FindReferences();

                foreach (var fileContext in context.projectContexts)
                {
                    using (var newScript = (ALScript)fileContext.StartScript())
                    {
                        refFinder.FindReferencesInFile(refFinder.GetSearchScopes(symbol),
                                                       fileContext.UnresolvedFile,
                                                       fileContext.RootNode as SyntaxTree,
                                                       fileContext.Compilation,
                                                       (n, r) => newScript.Rename(n, name),
                                                       context.CancellationToken);
                    }
                }
            }

            void Rename(IVariable variable, string name)
            {
                FindReferences refFinder = new FindReferences();

                refFinder.FindLocalReferences(variable,
                                              context.UnresolvedFile,
                                              context.RootNode as SyntaxTree,
                                              context.Compilation, (n, r) => Rename(n, name),
                                              context.CancellationToken);
            }

            public override void CreateNewType(AstNode newType, NewTypeContext context)
            {
                var output = OutputNode(0, newType, true);
                InsertText(0, output.Text);
            }

            public override void DoGlobalOperationOn(IEnumerable<IEntity> entities, Action<RefactoringContext, Script, IEnumerable<AstNode>> callback, string operationDescripton)
            {
                foreach (var projectContext in context.projectContexts)
                {
                    DoLocalOperationOn(projectContext, entities, callback);
                }
            }

            void DoLocalOperationOn(ALRefactoringContext localContext, IEnumerable<IEntity> entities, Action<RefactoringContext, Script, IEnumerable<AstNode>> callback)
            {
                List<AstNode> nodes = new List<AstNode>();
                FindReferences refFinder = new FindReferences();
                refFinder.FindCallsThroughInterface = true;
                refFinder.FindReferencesInFile(refFinder.GetSearchScopes(entities),
                                               localContext.UnresolvedFile,
                                               localContext.RootNode as SyntaxTree,
                                               localContext.Compilation,
                                               (node, result) =>
                                               {
                                                   nodes.Add(node);
                                               },
                                               CancellationToken.None);

                using (var script = localContext.StartScript())
                {
                    callback(localContext, script, nodes);
                }
            }
        }

        #region Text stuff

        public override bool IsSomethingSelected { get { return selectionStart > 0; } }

        public override string SelectedText { get { return IsSomethingSelected ? doc.GetText(selectionStart, selectionEnd - selectionStart) : ""; } }

        int selectionStart;
        public override TextLocation SelectionStart { get { return doc.GetLocation(selectionStart); } }

        int selectionEnd;
        public override TextLocation SelectionEnd { get { return doc.GetLocation(selectionEnd); } }

        public override int GetOffset(TextLocation location)
        {
            return doc.GetOffset(location);
        }

        public override TextLocation GetLocation(int offset)
        {
            return doc.GetLocation(offset);
        }

        public override string GetText(int offset, int length)
        {
            return doc.GetText(offset, length);
        }

        public override string GetText(ISegment segment)
        {
            return doc.GetText(segment);
        }

        public override IDocumentLine GetLineByOffset(int offset)
        {
            return doc.GetLineByOffset(offset);
        }
        #endregion
        public string Text
        {
            get
            {
                return doc.Text;
            }
        }


        public string GetSideDocumentText(int index)
        {
            return projectContexts[index].Text;
        }

        internal static void Print(AstNode node)
        {
            var v = new ALOutputVisitor(Console.Out, FormattingOptionsFactory.CreateMono());
            node.AcceptVisitor(v);
        }
    }
}
