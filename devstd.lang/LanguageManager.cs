using CompilersLibraryAPI;
using CompilersLibraryAPI.GCC;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace devstd.lang
{
   public class LanguageManager
    {
       public string SourceFile;
       public List<CompileMessage> Errors = new List<CompileMessage>();
       public bool Compiled = true;
       public bool Compile()
       {
           try
           {
               Errors.Clear();
               CompilerPrefs pr = new CompilerPrefs();
               bool cp = false;
               switch (Path.GetExtension(SourceFile))
               {
                   case ".pascal":
                       Compiled = PascalCompiler.Compile(Editor.Document.FileName);
                       Errors = PascalCompiler.Errors;
                       return Compiled;
                   case ".pp":
                       Compiled = PascalCompiler.Compile(Editor.Document.FileName);
                       Errors = PascalCompiler.Errors;
                       return Compiled;
                   case ".pas":
                       Compiled = PascalCompiler.Compile(Editor.Document.FileName);
                       Errors = PascalCompiler.Errors;
                       return Compiled;

                   case ".cs":
                       Compiled = CSharpCompiler.Compile(Editor.Document.FileName, ref cp);
                       Errors = CSharpCompiler.Errors;
                       return Compiled;
                   case ".vb":
                       Compiled = VBNETCompiler.Compile(Editor.Document.FileName, ref cp);
                       Errors = VBNETCompiler.Errors;
                       return Compiled;
                   case ".cpp":
                       Compiled = CPPCompiler.Compile(Editor.Document.FileName, ref cp);
                       Errors = CPPCompiler.Errors;
                       return Compiled;
                   case ".c":
                       Compiled = CPPCompiler.Compile(Editor.Document.FileName, ref cp);
                       Errors = CPPCompiler.Errors;
                       return Compiled;
                   case ".asm":
                       Compiled =  AssemblerCompiler.Compile(Editor.Document.FileName, ref cp);
                       Errors = AssemblerCompiler.Errors;
                       return Compiled;
                   case ".s":
                       Compiled = AssemblerCompiler.Compile(Editor.Document.FileName, ref cp);
                       Errors = AssemblerCompiler.Errors;
                       return Compiled;
                   case ".pwn":
                       Clean();
                       Compiled = PawnCompiler.Compile(Editor.Document.FileName, ref cp);
                       Errors = PawnCompiler.Errors;
                       return Compiled;
               }
           }
           catch
           {

           }
           return false;
       }
       public void Clean()
       {
           try
           {
               if (File.Exists(Path.ChangeExtension(SourceFile, ".exe")))
                   File.Delete(Path.ChangeExtension(SourceFile, ".exe"));
               else if (File.Exists(Path.ChangeExtension(SourceFile, ".amx")))
                   File.Delete(Path.ChangeExtension(SourceFile, ".amx"));
           }
           catch
           {

           }
       }
       public void Run()
       {
           try
           {
               if (File.Exists(Path.ChangeExtension(SourceFile, ".exe")))
                  Process.Start(Path.ChangeExtension(SourceFile, ".exe"));

           }
           catch
           {

           }
       }
       #region pascal
       string DetectVar(string varname, string content)
       {
           Regex m = new Regex(varname + @":(?<type>\w+);", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
           Match ma = m.Match(content);

           return ma.Groups["type"].Value;

       }
       string[] DetectModule(string input)
       {
           List<string> s = new List<string>();
           Regex _regex = new Regex(@"Procedure+\s(?<name>\w+)|Function+\s(?<name>\w+)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

           MatchCollection match = _regex.Matches(input);
           foreach (Match m in match)
           {
               if (m.Success)
                   s.AddRange(m.Groups["name"].Value.Split(','));

           }
           for (int i = 0; i <= s.Count - 1; i++)
           {
               string d = s[i].Remove(0, 1);
               d = d.Insert(0, s[i].ToUpper()[0].ToString());
               s[i] = d.Replace(" ", "");
           }
           return s.ToArray();
       }
       public List<CompileMessage> Error = new List<CompileMessage>();
       public delegate void PascalColorizer(string type);
       public event PascalColorizer OnColorRequested;
       public void PascalParsing(string content)
       {
           string code = content;
           int pos = code.ToLower().IndexOf("uses ");
           if (pos != -1)
           {
               int i = pos + 5;
               while (code[i] != ';')
               {
                   i++;
               }
               code = code.Remove(pos, i - pos + 1);
           }

           File.WriteAllText(SourceFile + "1", code);
           Scanner s = new Scanner(SourceFile+"1");
           Parser p = new Parser(s);
           p.Parse();
           Error.Clear();
          foreach(CompileMessage m in p.errors.Messages)
              Error.Add(new CompileMessage(m.LineNumber,m.CharNumber,m.Message, m.Type, SourceFile,false,"Parser",""));

    

          File.Delete(SourceFile + "1");
          List<string> mod = new List<string>();
          mod.AddRange(DetectModule(content));
          List<string> ns = new List<string>();
          ns.AddRange(PPUDiscover.DetectNS(code));

          List<string> remove = new List<string>();
          // update methods, constants, global variables for namespaces in keyword
          foreach (KeyValuePair<string, PASINTELIDATA> id in pman.completion.Keywords)
          {
              if (id.Value.NS != "root" && id.Value.NS != "unknown" && id.Value.NS != "predefined")
              {
                  if (!ns.Contains(id.Value.NS))
                      remove.Add(id.Key);
              }
              else if (id.Value.NS == "root" && (!p.detect.Vars.Contains(id.Key) && !p.detect.Consts.Contains(id.Key) && !p.detect.Fields.Contains(id.Key) && !mod.Contains(id.Key)))
                  remove.Add(id.Key);

          }
          foreach (string v in remove)
              pman.completion.Keywords.Remove(v);

          remove.Clear();

          // update types for namespaces in keyword
          foreach (KeyValuePair<string, PASINTELIDATA> id in pman.completion.Identifier)
          {
              if (id.Value.NS != "root" && id.Value.NS != "unknown" && id.Value.NS != "predefined")
              {
                  if (!ns.Contains(id.Value.NS))
                      remove.Add(id.Key);
              }
              else if (id.Value.NS == "root" && !p.detect.Types.Contains(id.Key))
                  remove.Add(id.Key);
          }
          foreach (string v in remove)
              pman.completion.Identifier.Remove(v);



          // update root
          remove.Clear();



          if (p.detect.Vars.Count > 0)
          {
              foreach (string v in p.detect.Vars)
                  if (!pman.completion.Keywords.ContainsKey(v))
                      pman.completion.Keywords.Add(v, new PASINTELIDATA("This is a user-variable '" + v + "' declared as " + DetectVar(v,content), v, "root", 3));

          }
          if (p.detect.Types.Count > 0)
          {
              foreach (string v in p.detect.Types)
                  if (!pman.completion.Identifier.ContainsKey(v))
                  {
                      pman.completion.Identifier.Add(v, new PASINTELIDATA("This is a user-type '" + v + "'  ", v, "root", 0));
                      // highlight the type
                      if (!pman.completion.Keywords.ContainsKey(v))
                          OnColorRequested(v);
                  }
          }
          if (p.detect.Consts.Count > 0)
          {
              foreach (string v in p.detect.Consts)
                  if (!pman.completion.Keywords.ContainsKey(v))
                      pman.completion.Keywords.Add(v, new PASINTELIDATA("This is a user-constant '" + v + "' ", v, "root", 4));

          }
          if (p.detect.Fields.Count > 0)
          {
              foreach (string v in p.detect.Fields)
                  if (!pman.completion.Keywords.ContainsKey(v))
                      pman.completion.Keywords.Add(v, new PASINTELIDATA("This is a user-field '" + v + "' ", v, "root", 3));

          }

          if (mod.Count > 0)
          {
              foreach (string v in mod)
                  if (!pman.completion.Keywords.ContainsKey(v))
                      pman.completion.Keywords.Add(v, new PASINTELIDATA("This is a user-module '" + v + "' ", v, "root", 1));

          }
       }
       #endregion

       #region Pawn
       delegate void LoadIncludeAsync(string inc);
       List<string> ExtractInclude(TextEditor edit)
       {
           List<string> ls = new List<string>();
           try
           {
               int i = 0;

               bool incl = true;
               while ((i < edit.Document.LineCount) && (incl))
               {
                   string line = edit.Document.GetText(edit.Document.GetLineByNumber(i));
                   if (line.Contains("#include"))
                       ls.Add(line.Substring(line.IndexOf('<') + 1, line.IndexOf('>') - line.IndexOf('<') - 1));

                   i++;
                   incl = !(line.Contains("public") || line.Contains("#define") || line.Contains("new ") || line.Contains("forward"));
               }
           }
           catch
           {

           }
           return ls;
       }
       PawnParser.Parser pwnparser = new PawnParser.Parser();
       public void PawnParse(TextEditor editor)
       {
           PawnCodeCompletionProvider.Includes = ExtractInclude(editor);
           pwnparser.CodeParser(editor.Document.FileName, editor.Text, false, "default");
           PawnParser.c_FileFnc fnc = pwnparser.curFile;
         
           // Get Functions
           foreach (KeyValuePair<string, PawnParser.c_function> func in fnc.functions)
           {
               if (!pwman.completion.Keywords.ContainsKey(func.Key))
               {
                   string sp = "";
                   foreach (string s in func.Value.fParams)
                       sp += ", " + s;
                   if (sp.Length > 0)
                       sp = sp.Remove(0, 1);
                   pwman.completion.Keywords.Add(func.Key, new PASINTELIDATA(func.Value.fName + "(" + sp + ") \nRetourne " + func.Value.fReturn, func.Value.fName + "(" + sp + ")", Path.GetFileNameWithoutExtension(func.Value.fFileName), 1));
               }
               else
               {
                   string sp = "";
                   foreach (string s in func.Value.fParams)
                       sp += ", " + s;
                   if (sp.Length > 0)
                       sp = sp.Remove(0, 1);
                   pwman.completion.Keywords[func.Key] = new PASINTELIDATA(func.Value.fName + "(" + sp + ") \nRetourne " + func.Value.fReturn, func.Value.fName + "(" + sp + ")", Path.GetFileNameWithoutExtension(func.Value.fFileName), 1);
               }
           }
           // Get Vars
           foreach (KeyValuePair<string, PawnParser.c_function> func in fnc.variables)
           {
               if (!pwman.completion.Keywords.ContainsKey(func.Key))
                   pwman.completion.Keywords.Add(func.Key, new PASINTELIDATA(func.Value.fName + "Variable \n Type " + func.Value.fReturn, func.Key, Path.GetFileNameWithoutExtension(func.Value.fFileName), 3));
               else
                   pwman.completion.Keywords[func.Key] = new PASINTELIDATA(func.Value.fName + "Variable \n Type " + func.Value.fReturn, func.Key, Path.GetFileNameWithoutExtension(func.Value.fFileName), 3);

           }
           // Get CONST
           foreach (KeyValuePair<string, PawnParser.c_function> func in fnc.constants)
           {
               if (!pwman.completion.Keywords.ContainsKey(func.Key))
                   pwman.completion.Keywords.Add(func.Key, new PASINTELIDATA(func.Value.fName + "Constante\n Type " + func.Value.fReturn, func.Key, Path.GetFileNameWithoutExtension(func.Value.fFileName), 4));
               else
                   pwman.completion.Keywords[func.Key] = new PASINTELIDATA(func.Value.fName + "Constante\n Type " + func.Value.fReturn, func.Key, Path.GetFileNameWithoutExtension(func.Value.fFileName), 4);
           }
                  


       }

       void LoadInclude(string includepath)
       {
           try
           {
               if (Directory.Exists(includepath))
               {
                   PawnParser.Parser p = new PawnParser.Parser();
                   DirectoryInfo di = new DirectoryInfo(includepath);
                   p.incarry = new PawnParser.c_FileFnc[di.GetFiles("*.inc").Length];
                   foreach (FileInfo fi in di.GetFiles("*.inc"))
                   {
                       try
                       {
                           StreamReader sr = new StreamReader(fi.FullName);
                           p.CodeParser(fi.Name, sr.ReadToEnd(), true, "default");
                           if (!pwman.completion.NameSpaces.ContainsKey(Path.GetFileNameWithoutExtension(fi.Name)))
                               pwman.completion.NameSpaces.Add(Path.GetFileNameWithoutExtension(fi.Name), new PASINTELIDATA(Path.GetFileNameWithoutExtension(fi.Name) + " include", Path.GetFileNameWithoutExtension(fi.Name), Path.GetFileNameWithoutExtension(fi.Name), 2));
                       }
                       catch
                       {

                       }
                   }

                   foreach (PawnParser.c_FileFnc fnc in p.incarry)
                   {
                       // Get Functions
                       foreach (KeyValuePair<string, PawnParser.c_function> func in fnc.functions)
                       {
                           if (!pwman.completion.Keywords.ContainsKey(func.Key))
                           {
                               string sp = "";
                               foreach (string s in func.Value.fParams)
                                   sp += ", " + s;
                               if (sp.Length > 0)
                                   sp = sp.Remove(0, 1);
                               pwman.completion.Keywords.Add(func.Key, new PASINTELIDATA(func.Value.fName + "(" + sp + ") \nRetourne " + func.Value.fReturn, func.Value.fName + "(" + sp + ")", Path.GetFileNameWithoutExtension(func.Value.fFileName), 1));
                           }
                       }
                       // Get Vars
                       foreach (KeyValuePair<string, PawnParser.c_function> func in fnc.variables)
                       {
                           if (!pwman.completion.Keywords.ContainsKey(func.Key))
                               pwman.completion.Keywords.Add(func.Key, new PASINTELIDATA(func.Value.fName + "Variable \nType " + func.Value.fReturn, func.Key, Path.GetFileNameWithoutExtension(func.Value.fFileName), 3));

                       }
                       // Get CONST
                       foreach (KeyValuePair<string, PawnParser.c_function> func in fnc.constants)
                       {
                           if (!pwman.completion.Keywords.ContainsKey(func.Key))
                               pwman.completion.Keywords.Add(func.Key, new PASINTELIDATA(func.Value.fName + "Constante \nType " + func.Value.fReturn, func.Key, Path.GetFileNameWithoutExtension(func.Value.fFileName), 4));
                       }
                   }

               }
            

           }
           catch
           {

           }
       }
       #endregion

       public bool IsPascalCompletion()
       {
           return SourceFile.EndsWith(".pas") || SourceFile.EndsWith(".pascal") || SourceFile.EndsWith(".pp");

       }
       public bool IsPawnCompletion()
       {
           return SourceFile.EndsWith(".pwn");

       }
       List<string> AnyExts;
       public bool IsAnyCompletion()
       {
           return AnyExts.Contains(new FileInfo(SourceFile).Extension); ;

       }
       PascalCompletionManager pman;
       PawnCompletionManager pwman;
       public TextEditor Editor;
       AnyCompletionManager aman;
       void LoadExts()
       {
           AnyExts.AddRange(new string[] { ".c", ".h", ".cc", ".cpp", ".hpp" });
           AnyExts.Add(".vb");
           AnyExts.Add(".cs");
           AnyExts.Add(".sql");
           AnyExts.Add(".js");
           AnyExts.Add(".html");
           AnyExts.Add(".htm");
       }
       public LanguageManager(string file,TextEditor edit, ImageList imgl)
       {
           SourceFile = file;
           AnyExts = new List<string>();
           LoadExts();
           // load any ext
           if (IsPascalCompletion())
           {
               pman = new PascalCompletionManager();
               // build imgsrc
               ImageSource[] src = new ImageSource[imgl.Images.Count];
               int i = 0;
               foreach (System.Drawing.Image img in imgl.Images)
               {

                   src[i] = ConvertDrawingImageToWPFImage(img).Source;
                   i++;
               }


               pman.completion.CompletionImageList = src;
           }
           else if (IsAnyCompletion())
           {
               aman = new AnyCompletionManager();
               // build imgsrc
               ImageSource[] src = new ImageSource[imgl.Images.Count];
               int i = 0;
               foreach (System.Drawing.Image img in imgl.Images)
               {
                 
                   src[i] = ConvertDrawingImageToWPFImage(img).Source;
                   i++;
               }

               aman.completion.ImageList = src;
           }
           else if (IsPawnCompletion())
           {
               pwman = new PawnCompletionManager();
               // build imgsrc
               ImageSource[] src = new ImageSource[imgl.Images.Count];
               int i = 0;
               foreach (System.Drawing.Image img in imgl.Images)
               {

                   src[i] = ConvertDrawingImageToWPFImage(img).Source;
                   i++;
               }

               pwman.completion.CompletionImageList = src;
               LoadIncludeAsync del = new LoadIncludeAsync(LoadInclude);
               del.BeginInvoke(System.Windows.Forms.Application.StartupPath + @"\Pawn\include", null, null);
           }

           Editor = edit;
           edit.TextArea.TextEntered += TextArea_TextEntered;
           edit.TextArea.TextEntering += TextArea_TextEntering;

       }
       private System.Windows.Controls.Image ConvertDrawingImageToWPFImage(System.Drawing.Image gdiImg)
       {


           System.Windows.Controls.Image img = new System.Windows.Controls.Image();

           //convert System.Drawing.Image to WPF image
           System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(gdiImg);
           IntPtr hBitmap = bmp.GetHbitmap();
           System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

           img.Source = WpfBitmap;
           img.Width = 16;
           img.Height = 16;
           img.Stretch = System.Windows.Media.Stretch.Fill;
           return img;
       }
       void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
       {
         if(pman != null)
             pman.TextEntered(sender, e, Editor);
         else if(aman != null)
             aman.TextEntered(sender, e, Editor);
         else if(pwman != null)
             pwman.TextEntered(sender, e, Editor);
       }
       void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
       {
           if (pman != null)
               pman.TextEntering(sender, e);
           else if (aman != null)
               aman.TextEntering(sender, e);
           else if (pwman != null)
               pwman.TextEntering(sender, e);
       }
    }
}
