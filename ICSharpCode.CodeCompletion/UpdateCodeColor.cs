using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.CodeCompletion;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.AL.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ICSharpCode.CodeCompletion
{

  public  class UpdateCodeColor
    {
      readonly ICSharpCode.AvalonEdit.Highlighting.HighlightingRuleSet MainRuleSet;
      public UpdateCodeColor(HighlightingRuleSet r)
      {
          MainRuleSet = r;
          Types = new List<string>();
          STypes = new List<string>();
   

      }
      public List<string> Types;
      public List<string> STypes;

      public List<string> NameSpaces = new List<string>();

      public HighlightingRule STypeRule;
      public HighlightingRule TypeRule;
   
      public void UpdateReferencedTypes( ICompilation compilation)
      {
          try
          {
   
             foreach(IAssembly asmb in compilation.Assemblies)
              {
                  foreach (DefaultResolvedTypeDefinition def in asmb.GetAllTypeDefinitions())
                  {
                      if (def.Kind == TypeKind.Enum || def.Kind ==  TypeKind.Interface)
                      {
                          if (!STypes.Contains(def.Name) && NameSpaces.Contains(def.Namespace))
                              STypes.Add(def.Name);
                      }
                      else
                      {
                          if (!Types.Contains(def.Name) && NameSpaces.Contains(def.Namespace))
                              Types.Add(def.Name);
                      }

                    
                  }
              }
          }
          catch
          {

          }
      }
      public void UpdateCurrentFile(SyntaxTree tree)
      {
          try
          {
              NameSpaces.Clear();
              var unresolvedFile = tree.ToTypeSystem();
              foreach (TypeOrNamespaceReference ns in unresolvedFile.RootUsingScope.Usings)
                  NameSpaces.Add(ns.ToString());

              // source types
              foreach (IUnresolvedTypeDefinition def in unresolvedFile.GetAllTypeDefinitions())
              {
                  if (def.Kind == TypeKind.Enum || def.Kind == TypeKind.Interface)
                  {
                      if (!STypes.Contains(def.Name))
                          STypes.Add(def.Name);
                  }
                  else
                  {
                      if (!Types.Contains(def.Name))
                          Types.Add(def.Name);
                  }
              }  
                                   
           
          }
          catch
          {

          }
      }
      public void UpdateProject(IProjectContent al)
      {
          try
          {
              // source types
              foreach (IUnresolvedFile f in al.Files)
              {
                  foreach (IUnresolvedTypeDefinition def in f.GetAllTypeDefinitions())
                  {
                      if (def.Kind == TypeKind.Enum || def.Kind == TypeKind.Interface)
                      {
                          if (!STypes.Contains(def.Name) && NameSpaces.Contains(def.Namespace))
                              STypes.Add(def.Name);
                      }
                      else
                      {
                          if (!Types.Contains(def.Name) && NameSpaces.Contains(def.Namespace))
                              Types.Add(def.Name);
                      }
                  }

              }
              ICompilation compilation = al.CreateCompilation();
                foreach(IAssembly asmb in compilation.Assemblies)
              {
                  
                  foreach (DefaultResolvedTypeDefinition def in asmb.GetAllTypeDefinitions())
                  {
                      if (def.Kind == TypeKind.Enum || def.Kind ==  TypeKind.Interface)
                      {
                          if (!STypes.Contains(def.Name) && NameSpaces.Contains(def.Namespace))
                              STypes.Add(def.Name);
                      }
                      else
                      {
                          if (!Types.Contains(def.Name) && NameSpaces.Contains(def.Namespace))
                              Types.Add(def.Name);
                      }

                    
                  }
              }

          }
          catch
          {

          }
      }
      void CleanNonCurrentRules(ref IList<HighlightingRule> rules ,int defaultsize)
      {
          while (rules.Count > defaultsize)
              rules.Remove(rules[rules.Count - 1]);
      }
      public void UpdateColor(CodeTextEditor editor)
      {
          try
          {
         
         
              // // Dynamic syntax highlighting for your own purpose
              var rules = editor.SyntaxHighlighting.MainRuleSet.Rules;

              CleanNonCurrentRules(ref rules, 23);
              TypeRule = new HighlightingRule();
              TypeRule.Color = new HighlightingColor()
               {
                   Foreground = new CustomizedBrush(System.Windows.Media.Colors.DarkCyan)
               };

              String[] wordList = Types.ToArray(); // Your own logic
              String regex = String.Format(@"\b({0})\w*\b", String.Join("|", wordList));
              TypeRule.Regex = new Regex(regex);
                 if(Types.Count > 0)
                      rules.Add(TypeRule);

              STypeRule = new HighlightingRule();
              STypeRule.Color = new HighlightingColor()
              {
                  Foreground = new CustomizedBrush(System.Windows.Media.Color.FromArgb(255,0,102,102))
              };
            
              wordList = STypes.ToArray(); // Your own logic
              regex = String.Format(@"\b(?>{0})\b", String.Join("|", wordList));
              STypeRule.Regex = new Regex(regex, RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
              if (STypes.Count > 0)
              rules.Add(STypeRule);

              editor.TextArea.TextView.Redraw();

          }
          catch
          {
          }

      }
    }
  internal sealed class CustomizedBrush : HighlightingBrush
  {
      private readonly SolidColorBrush brush;
      public CustomizedBrush(System.Windows.Media.Color color)
      {
          brush = CreateFrozenBrush(color);
      }

      public CustomizedBrush(System.Drawing.Color c)
      {
          var c2 = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
          brush = CreateFrozenBrush(c2);
      }

      public override System.Windows.Media.Brush GetBrush(ITextRunConstructionContext context)
      {
          return brush;
      }

      public override string ToString()
      {
          return brush.ToString();
      }

      private static SolidColorBrush CreateFrozenBrush(System.Windows.Media.Color color)
      {
          SolidColorBrush brush = new SolidColorBrush(color);
          brush.Freeze();
          return brush;
      }
  }
}
