using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Security.Cryptography;
using CompilersLibraryAPI;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.TypeSystem;

namespace ALCodeDomProvider
{
   public struct ReferencedAssembly
    {
        public string Name;
        public string SourcePath;
        public bool Copy;
        public bool Project;
        public ReferencedAssembly(string name, string path,bool copy, bool projec)
        {
            Name = name;
            SourcePath = path;
            Copy = copy;
            Project = projec;
        }
        public ReferencedAssembly(string name, string path, bool copy)
        {
            Name = name;
            SourcePath = path;
            Copy = copy;
            Project = false;
        }

    }
   public class ArsslenLanguageCodeDomProvider
    {

       public List<CompileMessage> CompileMessages;
       public ProjectSettings Properties;
       public Dictionary<string, ReferencedAssembly> ReferencedAssemblies;
       public string OutputDirectory;
       public string TempDirectory;
       public string ProjectDirectory;
       public ArsslenLanguageCodeDomProvider(ProjectSettings projset, string projectdir,Dictionary<string, ReferencedAssembly> refs)
       {
           CompileMessages = new List<CompileMessage>();
           Properties = projset;
           ProjectDirectory = projectdir;
           OutputDirectory = projectdir + @"\bin";
           TempDirectory = projectdir + @"\temp";
           ReferencedAssemblies = refs;
        }

       public void ClearBin()
       {
           try
           {
               foreach (string file in Directory.GetFiles(OutputDirectory))
                   File.Delete(file);

           }
           catch
           {

           }
       }
       public void ClearTemp()
       {
           try
           {
               foreach (string file in Directory.GetFiles(TempDirectory))
                   File.Delete(file);

           }
           catch
           {

           }
       }
       public void Prepare(string[] files)
       {
           ClearBin();
           ClearTemp();
           try
           {
               foreach (string file in files)
                   File.Copy(file, TempDirectory + @"\" + Path.GetFileName(file));
           }
           catch
           {

           }
       }
       public bool PreBuild(ref string[] files,string name, LanguageConverter lang, TempFileService temp)
       {
           CompileMessages.Clear();
           bool error = false;
           foreach (string file in files)
           {
               // Parse Each File
               ALParser pa = new ALParser();
               
               SyntaxTree tree = pa.Parse(File.ReadAllText(file), file);
               if (tree.Errors.Count > 0)
               {
                   foreach (Error err in tree.Errors)
                   {
                       if (err.ErrorType == ErrorType.Error)
                       {
                           error = true;
                           CompileMessages.Add(new CompileMessage(err.Region.BeginLine, err.Region.BeginColumn, err.Message, CompileMessage.MessageTypes.Error, file, true, "ALP001", name));

                       }
                       else 
                           CompileMessages.Add(new CompileMessage(err.Region.BeginLine, err.Region.BeginColumn, err.Message, CompileMessage.MessageTypes.Warning, file, true, "ALP002", name)); 

                   }
               }
           }
           // Pre-Build if no error
           if (!error)
           {
               List<string> fs = new List<string>();
               foreach (string file in files)
               {
                 string cd =  File.ReadAllText(file);
                 fs.Add(temp.GetTempFileName(file));
                 File.WriteAllText( temp.GetTempFileName(file), lang.InterpretAlToCs(cd));
               }
               files = fs.ToArray();
           }
           return !error;
       }

       public string ParamerterArgs()
       {
           StringBuilder ARGS = new StringBuilder();
           if (Properties.Target.Length > 0)
               ARGS.Append("/target:" + Properties.Target + " ");

           if (Properties.Checked)
               ARGS.Append("/checked+" + " ");


           if (Properties.Optimize)
               ARGS.Append("/optimize+" + " ");
           else
               ARGS.Append("/optimize-" + " ");


           if (Properties.Platform.Length > 0)
               ARGS.Append("/platform:" + Properties.Platform + " ");
        
           if (Properties.Unsafe)
               ARGS.Append("/unsafe+" + " ");
           else
               ARGS.Append("/unsafe-" + " ");

           if (Properties.DocFile.Length > 0)
               ARGS.Append("/doc:" + Properties.DocFile + " ");

           try
           {
               if (Properties.IconFile.Length > 0)
               {
                   if (File.Exists(Properties.IconFile))
                   {
                       File.Copy(Properties.IconFile, Application.StartupPath + @"\DSAL_APPICO.ico");
                       File.WriteAllText(Application.StartupPath + @"\APPB.rc", "1 ICON \"DSAL_APPICO.ico\"");
                       ProcessStartInfo si = new ProcessStartInfo();
                       si.WorkingDirectory = Application.StartupPath;
                       si.CreateNoWindow = true;
                       si.UseShellExecute = false;
                       si.Arguments = " /r \"" + Application.StartupPath + @"\APPB.rc" + "\"";
                       si.FileName = Application.StartupPath + @"\RC.exe";
                       Process p = Process.Start(si);
                       p.WaitForExit();
                       if (File.Exists(Application.StartupPath + @"\APPB.res"))
                           ARGS.Append("/win32res:\"" + Application.StartupPath + @"\APPB.res" + "\" ");

                       File.Delete(Application.StartupPath + @"\DSAL_APPICO.ico");
                       File.Delete(Application.StartupPath + @"\APPB.rc");
                   }


               }

           }
           catch
           {

           }

           // if (Properties.IconFile.Length > 0)
           //   ARGS.Append("-win32icon:\"" + Properties.IconFile + "\" ");


           if (Properties.Symbols.Count > 0)
           {

               for (int i = 0; i < Properties.Symbols.Count - 1; i++)
                   ARGS.Append("/define:" + Properties.Symbols[i] + " ");

               ARGS.Append("/define:" + Properties.Symbols[Properties.Symbols.Count - 1] + " ");

           }

           if (Properties.Sign)
           {
               if(Properties.DelaySign)
                   ARGS.Append("/delaysign+ ");

               if (Properties.KeyFile.Length > 0)
                   ARGS.Append("/keyfile:\"" + Properties.KeyFile + "\" ");


           }
           if (Properties.HighEntropy)
               ARGS.Append("/highentropyva+ ");

           if (Properties.UTF8OUTPUT)
               ARGS.Append("/utf8output ");

           if (Properties.PreferedBuildLanguage.Length > 0)
               ARGS.Append("/preferreduilang:\"" + Properties.PreferedBuildLanguage + "\" ");

           if (Properties.LangVersion.Length > 0)
               ARGS.Append("/langversion:\"" + Properties.LangVersion + "\" ");

           if (Properties.NoWarn.Count > 0)
           {

               for (int i = 0; i < Properties.NoWarn.Count - 1; i++)
                   ARGS.Append("/nowarn:" + Properties.NoWarn[i] + " ");

               ARGS.Append("/nowarn:" + Properties.NoWarn[Properties.NoWarn.Count - 1] + " ");

           }

           if (Properties.WarnAsErrorList.Count > 0)
           {

               for (int i = 0; i < Properties.WarnAsErrorList.Count - 1; i++)
                   ARGS.Append("/warnaserror:" + Properties.WarnAsErrorList[i] + " ");

               ARGS.Append("/warnaserror:" + Properties.WarnAsErrorList[Properties.WarnAsErrorList.Count - 1] + " ");

           }
          // ARGS.Append("/noconfig ");

           //if (manifest.Length > 0)
           //   ARGS.Append("/win32manifest:\"" + manifest + "\" ");

        
           return ARGS.ToString();
       }
       public CompilerParameters CreateParams()
       {
           System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
           parameters.IncludeDebugInformation = Properties.Debug;
           parameters.MainClass = Properties.EntryPoint;

           if (Properties.Target == "library")
               parameters.GenerateExecutable = false;
           else
               parameters.GenerateExecutable = true;
           parameters.GenerateInMemory = false;
           parameters.OutputAssembly = OutputDirectory + @"\" + Properties.Output;
           parameters.TreatWarningsAsErrors = Properties.WarnAsError;
           parameters.WarningLevel = Properties.WarnLevel;
           parameters.CompilerOptions = ParamerterArgs();
           foreach (KeyValuePair<string, ReferencedAssembly> p in ReferencedAssemblies)
           {
              if(p.Key != "mscorlib")
                   parameters.ReferencedAssemblies.Add(p.Value.SourcePath);
              
           }
           return parameters;

       }
       public string GetCompilerVersion(string fx)
       {
           switch (fx)
           {
               case "2.0":
                   return "v2.0";
               case "3.0":
                   return "v3.0";
               case "3.5":
                   return "v3.5";
               case "4.0":
                   return "v4.0";
               case "4.5":
                   return "v4.0";

            
           }
         
		 return "v2.0";
       }
       public bool CodeDomBuildCode(string[] files, string name, string fx, TempFileService temp)
       {
           var settings = new Dictionary<string, string>();
           settings.Add("CompilerVersion", GetCompilerVersion(fx));
           CSharpCodeProvider codeProvider = new CSharpCodeProvider(settings);
           ICodeCompiler icc = codeProvider.CreateCompiler();
           CompilerParameters parameters = CreateParams();
        
           CompileMessages.Clear();
           CompilerResults results = icc.CompileAssemblyFromFileBatch(parameters, files);

           if (results.Errors.Count > 0)
           {
               foreach (CompilerError err in results.Errors)
               {
                     if (err.IsWarning)
                       CompileMessages.Add(new CompileMessage(err.Line, err.Column, err.ErrorText, CompileMessage.MessageTypes.Warning,temp.GetFileFromTemp(err.FileName), true,err.ErrorNumber, name));
                   else
                       CompileMessages.Add(new CompileMessage(err.Line, err.Column, err.ErrorText, CompileMessage.MessageTypes.Error,temp.GetFileFromTemp(err.FileName), true, err.ErrorNumber,name)); 


               }
           
           }
           return (results.NativeCompilerReturnValue == 0);
       }

    }
}
