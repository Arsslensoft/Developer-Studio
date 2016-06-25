using ALCodeDomProvider;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace alproj
{
   public delegate void AsyncParserInitializer();
   public enum ParserCommand
   {
       ParseFile,
       ParseRessource
   }
   public class ParserArgument
   {
       public object Editor;
       public string Text;
       public ALProject Project;
       public string FileName;
       public ParserCommand Command;
   }
   public class ProjectParser
    {
       public IProjectContent NativeProject;
       public ALProject Project;
       public ALParser Parser;
       public void UpdateRessources()
       {
           try{
               RessourceManager.RessourcesToCode(Project.Ressources, Project.Name, Project.CodeDomProvider.ProjectDirectory + @"\res\" + Project.Name + ".res.al");
              var source = File.ReadAllText(Project.CodeDomProvider.ProjectDirectory + @"\res\" +Project.Name + ".res.al");
               var asyntaxTree = Parser.Parse(source, Project.CodeDomProvider.ProjectDirectory + @"\res\" + Project.Name + ".res.al");
               asyntaxTree.Freeze();
               var aunresolvedFile = asyntaxTree.ToTypeSystem();
               NativeProject = NativeProject.AddOrUpdateFiles(aunresolvedFile);
              
           }
           catch
           {

           }
       }
       public void UpdateReferencedAssemblies()
       {
           try
           {
               var unresolvedAssemblies = new IUnresolvedAssembly[Project.Asm.Count];
               int i = 0;
               foreach (KeyValuePair<string, ReferencedAssembly> fs in Project.Asm)
               {
                   CecilLoader loader = new CecilLoader();
                   var path = fs.Value.SourcePath;
                   unresolvedAssemblies[i] = loader.LoadAssemblyFile(path);
                   i++;
               }

               NativeProject = NativeProject.AddAssemblyReferences((IEnumerable<IUnresolvedAssembly>)unresolvedAssemblies);
           }
           catch
           {

           }
       }
       public SyntaxTree UpdateSourceFile(string filename)
       {
           try
           {
               var source = File.ReadAllText(filename);
               var asyntaxTree = Parser.Parse(source, filename);
               asyntaxTree.Freeze();
               var aunresolvedFile = asyntaxTree.ToTypeSystem();
               NativeProject = NativeProject.AddOrUpdateFiles(aunresolvedFile);
               return asyntaxTree;
           }
           catch
           {

           }
           return null;
       }
       public void LoadProject()
       {
           try
           {
            

               string source = null;
               foreach (KeyValuePair<string, SourceFile> f in Project.SourceFiles)
               {
                   source = File.ReadAllText(f.Value.SourcePath);
                   var syntaxTree = Parser.Parse(source, f.Value.SourcePath);
                   syntaxTree.Freeze();
                   var unresolvedFile = syntaxTree.ToTypeSystem();
                   NativeProject = NativeProject.AddOrUpdateFiles(unresolvedFile);
               }
               UpdateRessources();

               UpdateReferencedAssemblies();
           }
           catch
           {

           }
       }
       public void LoadProjectAsync()
       {
           try
           {

               AsyncParserInitializer p = new AsyncParserInitializer(LoadProject);
               p.BeginInvoke(null, null);
           
           }
           catch
           {

           }
       }
       public ProjectParser(ALProject project)
       {
           Project = project;
           Parser = new ALParser();
           NativeProject = new ALProjectContent();
           NativeProject.SetProjectFileName(project.FileName);
       }


   }
}
