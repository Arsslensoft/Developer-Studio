using ICSharpCode.AvalonEdit;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.AL.Refactoring;
using ICSharpCode.NRefactory.AL.Resolver;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ALRefactoring
{
   public class Refactoring
    {
       public List<ICSharpCode.NRefactory.AL.Refactoring.CodeAction> CodeActions { get; set; }
       public List<ICSharpCode.NRefactory.AL.Refactoring.CodeIssue> CodeIssues { get; set; }
       public ALRefactoringContext UsingContext;
       public ALRefactoringContext UpdatingContext;
       public static List<GatherVisitorCodeIssueProvider> Providers { get; set; }

      static void InitProviders()
        {
            Providers = new List<GatherVisitorCodeIssueProvider>();
            Assembly NR_CSharp = typeof(InconsistentNamingIssue).Assembly;
            foreach (var topLevelType in NR_CSharp.GetTypes())
            {
                if (topLevelType.BaseType == typeof(GatherVisitorCodeIssueProvider) && !topLevelType.IsAbstract)
                    Providers.Add((GatherVisitorCodeIssueProvider)Activator.CreateInstance(topLevelType));

            }
        }
       public Refactoring()
       {
           CodeActions = new List<ICSharpCode.NRefactory.AL.Refactoring.CodeAction>();
           CodeIssues = new List<ICSharpCode.NRefactory.AL.Refactoring.CodeIssue>();
           if (Refactoring.Providers == null)
               Refactoring.InitProviders();
       }

       public void UpdateRefactory(IDocument doc, TextLocation location, SyntaxTree tree, IProjectContent proj)
       {
           UpdateRefactory(doc,location, tree, proj.CreateCompilation());
       }
       public void UpdateRefactory(IDocument doc,TextLocation location, SyntaxTree tree, ICompilation compilation)
       {
           try
           {
               CodeIssues.Clear();
               UpdatingContext = new ALRefactoringContext(doc, location, new ALAstResolver(compilation, tree, tree.ToTypeSystem()));
              
               foreach (GatherVisitorCodeIssueProvider prov in Providers)
               {
                   List<CodeIssue> v = prov.GetIssues(UpdatingContext).ToList();
                   CodeIssues.AddRange(v.ToArray());

               }
           

      

           }
           catch
           {

           }
       }
       public void Run(CodeAction act, ALRefactoringContext context)
       {
           try
           {
        act.Run(context.StartScript());
           }
           catch
           {

           }

       }
       public void GetUsingCodeActions(IDocument doc, TextLocation location, SyntaxTree tree, ICompilation compilation)
       {
           try
           {
              CodeActions.Clear();
               UsingContext = new ALRefactoringContext(doc, location, new ALAstResolver(compilation, tree, tree.ToTypeSystem()));
              
               AddUsingAction a = new AddUsingAction();

               CodeActions = a.GetActions(UsingContext).ToList();
             
           }
           catch
           {
           }
       }
    }
}
