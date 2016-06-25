using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using ALCodeDomProvider;
using System.Windows.Forms;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.AL;

namespace alproj
{

    public class ALProject
    {
        public IProjectContent NativeProject
        {
            get
            {
              return  ParserService.NativeProject;
            }
        }
        public ProjectParser ParserService { get; set; }
        public bool NeedBuild { get; set; }
        public static void GetReference(ref Dictionary<string, ReferencedAssembly> asm, string name, TargetFramework target)
        {
            if (name != "Al")
            {
                string aepath = target.FindRealName(name + ".dll");
                if (aepath != null)
                    asm.Add(name, new ReferencedAssembly(name, aepath, false));
            }
            else
                asm.Add(name, new ReferencedAssembly(name, Application.StartupPath + @"\Al.dll", true));
        }
        public static ALProject CreateConsole(string path, string name, TargetFramework target)
        {
            ALProject proj;
            try
            {
              
                Dictionary<string, SourceFile> src = new Dictionary<string, SourceFile>();
                Dictionary<string, ReferencedAssembly> asm = new Dictionary<string,ReferencedAssembly>();
                Dictionary<string, RessourceItem> res = new Dictionary<string,RessourceItem>();
                ProjectSettings props;
                Directory.CreateDirectory(path + @"\bin");
                Directory.CreateDirectory(path + @"\temp");
                Directory.CreateDirectory(path + @"\src");
                Directory.CreateDirectory(path + @"\res");
                Directory.CreateDirectory(path + @"\asminfo");
                Directory.CreateDirectory(path + @"\data");
                // Write Sources
                File.WriteAllText(path + @"\src\main.al", Templates.ConsoleCode.Replace("{$namespace.$}",name));
                File.WriteAllText(path + @"\asminfo\info.al", Templates.AssemblyInfoCode.Replace("{$namespace.$}", name));
              //  File.WriteAllText(path + @"\asminfo\app.manifest", Templates.ManifestCode);
              //  File.WriteAllText(path + @"\asminfo\app.config", Templates.ConfigCode);
                // Manage Project Sources
                src.Add("main.al", new SourceFile("main.al", path + @"\src\main.al", @"src\main.al", SourceFileType.Source));
                src.Add("info.al", new SourceFile("info.al", path + @"\asminfo\info.al", @"asminfo\info.al", SourceFileType.AsmInfo));
           
                if (target.version == "2.0")
                {
                    GetReference(ref asm, "System", target);
                    GetReference(ref asm, "System.Data", target);
                    GetReference(ref asm, "System.Drawing", target);
                    GetReference(ref asm, "mscorlib", target);
                    GetReference(ref asm, "Al", target);
                }
                else if (target.version == "3.0")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "Al", target);
                }
                else if (target.version == "3.5")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Core",target);
                    GetReference(ref asm, "Al", target);
                }
                else if (target.version == "4.0")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Core", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "Microsoft.CSharp", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "Al", target);
                }
                else if (target.version == "4.5")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Core", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "Microsoft.CSharp", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "Al", target);
                }
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(alproj.Properties.Resources.ProjectSetsXml.Replace("{$bin.$}", name + ".exe").Replace("{$at.$}", "exe"));
                props = new ProjectSettings(doc.DocumentElement);
                using (StreamWriter str = new StreamWriter(path+@"\"+name+".alproj", false))
                {
                    str.WriteLine("<project name=\"" + name + "\" target=\"" + target.version + "\" lang=\"AL\" version=\"1.0\">");
                    ProjectSource.SaveProjectSources(src, str);
                    RessourceManager.SaveProjectRessources(res, str);
                    Framework.Savessembblies(asm, str);
                    props.Save(str);


                   // str.WriteLine(" <config path=\"" + path + @"\asminfo\app.config" + "\"/>");
                 //   str.WriteLine("<manifest path=\""+path + @"\asminfo\app.manifest"+"\" />");
                    str.WriteLine("</project>");

                }

                return new ALProject(path + @"\" + name + ".alproj");
            }
            catch
            {

            }
            return null;
        }
        public static ALProject CreateForm(string path, string name, TargetFramework target)
        {
            ALProject proj;
            try
            {

                Dictionary<string, SourceFile> src = new Dictionary<string, SourceFile>();
                Dictionary<string, ReferencedAssembly> asm = new Dictionary<string, ReferencedAssembly>();
                Dictionary<string, RessourceItem> res = new Dictionary<string, RessourceItem>();
                ProjectSettings props;
                Directory.CreateDirectory(path + @"\bin");
                Directory.CreateDirectory(path + @"\temp");
                Directory.CreateDirectory(path + @"\src");
                Directory.CreateDirectory(path + @"\res");
                Directory.CreateDirectory(path + @"\asminfo");
                Directory.CreateDirectory(path + @"\forms");
                Directory.CreateDirectory(path + @"\formevent");
                Directory.CreateDirectory(path + @"\data");

                // Write Sources
                File.WriteAllText(path + @"\src\main.al", Templates.EPCode.Replace("{$namespace.$}", name));
                File.WriteAllText(path + @"\formevent\MainForm.Events.al", Templates.EventCode.Replace("{$namespace.$}", name).Replace("{$form.$}", "MainForm"));
                File.WriteAllText(path + @"\forms\MainForm.Designer.al", Templates.DesignerCode.Replace("{$namespace.$}", name).Replace("{$class.$}", "MainForm"));
                File.WriteAllText(path + @"\src\MainForm.al", Templates.FormCode.Replace("{$namespace.$}", name).Replace("{$class.$}", "MainForm"));
                File.WriteAllText(path + @"\asminfo\info.al", Templates.AssemblyInfoCode.Replace("{$namespace.$}", name));
             //   File.WriteAllText(path + @"\asminfo\app.manifest", Templates.ManifestCode);
            //    File.WriteAllText(path + @"\asminfo\app.config", Templates.ConfigCode);

                // Manage Project Sources
                src.Add("main.al", new SourceFile("main.al", path + @"\src\main.al", @"src\main.al", SourceFileType.Source));
                src.Add("info.al", new SourceFile("info.al", path + @"\asminfo\info.al", @"asminfo\info.al", SourceFileType.AsmInfo));
                src.Add("MainForm.Events.al", new SourceFile("MainForm.Events.al", path + @"\formevent\MainForm.Events.al", @"formevent\MainForm.Events.al", SourceFileType.Event));
                src.Add("MainForm.Designer.al", new SourceFile("MainForm.Designer.al", path + @"\forms\MainForm.Designer.al", @"forms\MainForm.Designer.al", SourceFileType.Form));
                src.Add("MainForm.al", new SourceFile("MainForm.al", path + @"\src\MainForm.al", @"src\MainForm.al", SourceFileType.Source));



                if (target.version == "2.0")
                {
                    GetReference(ref asm, "System", target);
                    GetReference(ref asm, "System.Data", target);
                    GetReference(ref asm, "System.Windows.Forms", target);
                    GetReference(ref asm, "System.Drawing", target);
                    GetReference(ref asm, "mscorlib", target);

                }
                else if (target.version == "3.0")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Windows.Forms", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("2.0"));

                }
                else if (target.version == "3.5")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Core", target);
                    GetReference(ref asm, "System.Windows.Forms", Framework.GetTargetVersion("2.0"));
                }
                else if (target.version == "4.0")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Core", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Windows.Forms", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "Microsoft.CSharp", Framework.GetTargetVersion("4.0"));
                }
                else if (target.version == "4.5")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Core", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Windows.Forms", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "Microsoft.CSharp", Framework.GetTargetVersion("4.5"));
                }
                GetReference(ref asm, "Al", target);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(alproj.Properties.Resources.ProjectSetsXml.Replace("{$bin.$}",name +".exe").Replace("{$at.$}", "winexe"));
                props = new ProjectSettings(doc.DocumentElement);
                using (StreamWriter str = new StreamWriter(path + @"\" + name + ".alproj", false))
                {
                    str.WriteLine("<project name=\"" + name + "\" target=\"" + target.version + "\" lang=\"AL\" version=\"1.0\">");
                    ProjectSource.SaveProjectSources(src, str);
                    RessourceManager.SaveProjectRessources(res, str);
                    Framework.Savessembblies(asm, str);
                    props.Save(str);
                 //   str.WriteLine(" <config path=\"" + path + @"\asminfo\app.config" + "\"/>");
                 //   str.WriteLine("<manifest path=\"" + path + @"\asminfo\app.manifest" + "\" />");
                    str.WriteLine("</project>");
                }

                return new ALProject(path + @"\" + name + ".alproj");
            }
            catch
            {

            }
            return null;
        }
        public static ALProject CreateLibrary(string path, string name, TargetFramework target)
        {
            ALProject proj;
            try
            {

                Dictionary<string, SourceFile> src = new Dictionary<string, SourceFile>();
                Dictionary<string, ReferencedAssembly> asm = new Dictionary<string, ReferencedAssembly>();
                Dictionary<string, RessourceItem> res = new Dictionary<string, RessourceItem>();
                ProjectSettings props;
                Directory.CreateDirectory(path + @"\bin");
                Directory.CreateDirectory(path + @"\temp");
                Directory.CreateDirectory(path + @"\src");
                Directory.CreateDirectory(path + @"\res");
                Directory.CreateDirectory(path + @"\asminfo");
                Directory.CreateDirectory(path + @"\data");

                // Write Sources
                File.WriteAllText(path + @"\src\class1.al", Templates.ClassCode.Replace("{$namespace.$}", name).Replace("{$class.$}","class1"));
                File.WriteAllText(path + @"\asminfo\info.al", Templates.AssemblyInfoCode.Replace("{$namespace.$}", name));
         
             //   File.WriteAllText(path + @"\asminfo\app.config", Templates.ConfigCode);

                // Manage Project Sources
                src.Add("class1.al", new SourceFile("class1.al", path + @"\src\class1.al", @"src\class1.al", SourceFileType.Source));
                src.Add("info.al", new SourceFile("info.al", path + @"\asminfo\info.al", @"asminfo\info.al", SourceFileType.AsmInfo));
                if (target.version == "2.0")
                {
                    GetReference(ref asm, "System", target);
                    GetReference(ref asm, "System.Data", target);
                    GetReference(ref asm, "System.Drawing", target);
                    GetReference(ref asm, "mscorlib", target);

                }
                else if (target.version == "3.0")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("2.0"));

                }
                else if (target.version == "3.5")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("2.0"));
                    GetReference(ref asm, "System.Core", target);
                }
                else if (target.version == "4.0")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Core", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "Microsoft.CSharp", Framework.GetTargetVersion("4.0"));
                }
                else if (target.version == "4.5")
                {
                    GetReference(ref asm, "System", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Data", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Drawing", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "mscorlib", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "System.Data.Xml", Framework.GetTargetVersion("4.0"));
                    GetReference(ref asm, "System.Core", Framework.GetTargetVersion("4.5"));
                    GetReference(ref asm, "Microsoft.CSharp", Framework.GetTargetVersion("4.5"));
                }
                GetReference(ref asm, "Al", target);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(alproj.Properties.Resources.ProjectSetsXml.Replace("{$bin.$}", name + ".dll").Replace("{$at.$}", "library"));
                props = new ProjectSettings(doc.DocumentElement);
                using (StreamWriter str = new StreamWriter(path + @"\" + name + ".alproj", false))
                {
                    str.WriteLine("<project name=\"" + name + "\" target=\"" + target.version + "\" lang=\"AL\" version=\"1.0\">");
                    ProjectSource.SaveProjectSources(src, str);
                    RessourceManager.SaveProjectRessources(res, str);
                    Framework.Savessembblies(asm, str);
                    props.Save(str);
                //    str.WriteLine(" <config path=\"" + path + @"\asminfo\app.config" + "\"/>");
                 //   str.WriteLine("<manifest path=\"" + path + @"\asminfo\app.manifest" + "\" />");
                    str.WriteLine("</project>");
                }

                return new ALProject(path + @"\" + name + ".alproj");
            }
            catch
            {

            }
            return null;
        }

        public void AddClass(string name)
        {
            try
            {
                string n = name;
                if(!name.EndsWith(".al"))
                    n += ".al";

                if (!SourceFiles.ContainsKey(n))
                {
                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\src\" + n, Templates.ClassCode.Replace("{$namespace.$}", this.Name).Replace("{$class.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(n, new SourceFile(n, Path.GetDirectoryName(this.FileName) + @"\src\" + n, @"src\" + n, SourceFileType.Source));
                }
            }
            catch
            {

            }
        }
        public void AddForm(string name)
        {
            try
            {
                string n = name;
                if (!name.EndsWith(".al"))
                    n += ".al";

                if (!SourceFiles.ContainsKey(n))
                {
                    string designer = Path.GetFileNameWithoutExtension(n) + ".Designer.al";
                    string events = Path.GetFileNameWithoutExtension(n) + ".Events.al";
                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\src\" + n, Templates.FormCode.Replace("{$namespace.$}", this.Name).Replace("{$class.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(n, new SourceFile(n, Path.GetDirectoryName(this.FileName) + @"\src\" + n, @"src\" + n, SourceFileType.Source));

                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\forms\" + designer, Templates.DesignerCode.Replace("{$namespace.$}", this.Name).Replace("{$class.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(designer, new SourceFile(designer, Path.GetDirectoryName(this.FileName) + @"\forms\" + designer, @"forms\" + designer, SourceFileType.Form));

                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\formevent\" + events, Templates.EventCode.Replace("{$namespace.$}", this.Name).Replace("{$form.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(events, new SourceFile(events, Path.GetDirectoryName(this.FileName) + @"\formevent\" + events, @"formevent\" + events, SourceFileType.Event));

                }
                CheckFormRefs();
                Save();
            }
            catch
            {

            }
        }
        public void AddControl(string name)
        {
            try
            {
                string n = name;
                if (!name.EndsWith(".al"))
                    n += ".al";

                if (!SourceFiles.ContainsKey(n))
                {
                    string designer = Path.GetFileNameWithoutExtension(n) + ".Designer.al";
                    string events = Path.GetFileNameWithoutExtension(n) + ".Events.al";
                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\src\" + n, Templates.ControlCode.Replace("{$namespace.$}", this.Name).Replace("{$class.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(n, new SourceFile(n, Path.GetDirectoryName(this.FileName) + @"\src\" + n, @"src\" + n, SourceFileType.Source));

                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\forms\" + designer, Templates.DesignerCode.Replace("{$namespace.$}", this.Name).Replace("{$class.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(designer, new SourceFile(designer, Path.GetDirectoryName(this.FileName) + @"\forms\" + designer, @"forms\" + designer, SourceFileType.Form));

                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\formevent\" + events, Templates.EventCode.Replace("{$namespace.$}", this.Name).Replace("{$form.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(events, new SourceFile(events, Path.GetDirectoryName(this.FileName) + @"\formevent\" + events, @"formevent\" + events, SourceFileType.Event));

                }
                CheckFormRefs();
                Save();
            }
            catch
            {

            }
        }
        public void AddUserControl(string name)
        {
            try
            {
                string n = name;
                if (!name.EndsWith(".al"))
                    n += ".al";

                if (!SourceFiles.ContainsKey(n))
                {
                    string designer = Path.GetFileNameWithoutExtension(n) + ".Designer.al";
                    string events = Path.GetFileNameWithoutExtension(n) + ".Events.al";
                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\src\" + n, Templates.UserControlCode.Replace("{$namespace.$}", this.Name).Replace("{$class.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(n, new SourceFile(n, Path.GetDirectoryName(this.FileName) + @"\src\" + n, @"src\" + n, SourceFileType.Source));

                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\forms\" + designer, Templates.DesignerCode.Replace("{$namespace.$}", this.Name).Replace("{$class.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(designer, new SourceFile(designer, Path.GetDirectoryName(this.FileName) + @"\forms\" + designer, @"forms\" + designer, SourceFileType.Form));

                    File.WriteAllText(Path.GetDirectoryName(this.FileName) + @"\formevent\" + events, Templates.EventCode.Replace("{$namespace.$}", this.Name).Replace("{$form.$}", Path.GetFileNameWithoutExtension(n)));
                    SourceFiles.Add(events, new SourceFile(events, Path.GetDirectoryName(this.FileName) + @"\formevent\" + events, @"formevent\" + events, SourceFileType.Event));

                }
                CheckFormRefs();
                Save();
            }
            catch
            {

            }
        }
        void CheckFormRefs()
        {
            try
            {
                if(!Asm.ContainsKey("System.Windows.Forms"))
                   Asm.Add("System.Windows.Forms", new ReferencedAssembly("System.Windows.Forms", @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\System.Windows.Forms.dll", false));

            }
            catch
            {

            }
        }
        public void RemoveSource(string name)
        {
            try
            {
                if (SourceFiles.ContainsKey(name))
                {
                    SourceFile src = SourceFiles[name];
                    if (src.SourceType == SourceFileType.Form || src.SourceType == SourceFileType.Event)
                    {
                        string basepath = Path.GetDirectoryName(this.FileName);

                        string classname = basepath + @"\src\" + Path.GetFileNameWithoutExtension(src.ProjectPath).Split('.')[0] + ".al";
                        string designer = basepath + @"\forms\" + Path.GetFileNameWithoutExtension(src.ProjectPath).Split('.')[0] + ".Designer.al";
                        string events = basepath + @"\formevent\" + Path.GetFileNameWithoutExtension(src.ProjectPath).Split('.')[0] + ".Events.al";
                        // remove
                        if (SourceFiles.ContainsKey(Path.GetFileName(classname)))
                            SourceFiles.Remove(Path.GetFileName(classname));
                        if (SourceFiles.ContainsKey(Path.GetFileName(designer)))
                            SourceFiles.Remove(Path.GetFileName(designer));
                        if (SourceFiles.ContainsKey(Path.GetFileName(events)))
                            SourceFiles.Remove(Path.GetFileName(events));
                    }
                    else if (src.SourceType == SourceFileType.Source)
                    {
                        string basepath = Path.GetDirectoryName(this.FileName);
                        string classname = name;
                        string designer = basepath + @"\forms\" + Path.GetFileNameWithoutExtension(src.ProjectPath) + ".Designer.al";
                        string events = basepath + @"\formevent\" + Path.GetFileNameWithoutExtension(src.ProjectPath) + ".Events.al";
                      
                        if (SourceFiles.ContainsKey(Path.GetFileName(classname)))
                            SourceFiles.Remove(Path.GetFileName(classname));
                        if (SourceFiles.ContainsKey(Path.GetFileName(designer)))
                            SourceFiles.Remove(Path.GetFileName(designer));
                        if (SourceFiles.ContainsKey(Path.GetFileName(events)))
                            SourceFiles.Remove(Path.GetFileName(events));
                    }
                    else
                        SourceFiles.Remove(name);
                   
                }
                Save();
            }
            catch
            {

            }
        }
        public void RemoveAsm(string name)
        {
            try
            {
                if (Asm.ContainsKey(name))
                    Asm.Remove(name);

                Save();
            }
            catch
            {

            }
        }
        public void RemoveRes(string name)
        {
            try
            {
                if (Ressources.ContainsKey(name))
                    Ressources.Remove(name);

                Save();
            }
            catch
            {

            }
        }
        public TempFileService TempService { get; set; }
        public Dictionary<string, SourceFile> SourceFiles;
        public Dictionary<string, RessourceItem> Ressources;
        public Dictionary<string, ReferencedAssembly> Asm;
        public ProjectSettings Properties;
        public string FileName;

        public string ConfigFile;
        public string ManifestFile;

        public string Name;
        public string version;
        public string Lang;
        public TargetFramework Target;
        public bool Loaded;
        public LanguageConverter alang;
        public static void Initialize()
        {
            try
            {
                Framework.LoadTargetFrameworks();
            }
            catch
            {

            }
        }
        public ALCodeDomProvider.ArsslenLanguageCodeDomProvider CodeDomProvider;
        public DSolution ParentSolution { get; set; }
        public ALProject(string file)
        {
            Loaded = false;
            try
            {
                if (File.Exists(file) && file.EndsWith(".alproj"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    Name = doc.DocumentElement.GetAttribute("name");
                    version = doc.DocumentElement.GetAttribute("version");
                    Lang = doc.DocumentElement.GetAttribute("lang");

                    foreach (TargetFramework fm in Framework.Targets)
                        if (fm.version == doc.DocumentElement.GetAttribute("target"))
                            Target = fm;


                    FileName = file;
                    SourceFiles = ProjectSource.LoadProjectSources(doc);
                    Ressources = RessourceManager.LoadProjectRessources(doc);
                    Asm = Framework.LoadAssemblies(doc);
                  //  ConfigFile = ((XmlElement)doc.DocumentElement.GetElementsByTagName("config")[0]).GetAttribute("path");
                  
                    Properties = new ProjectSettings((XmlElement)doc.DocumentElement.GetElementsByTagName("sets")[0]);
                    CodeDomProvider = new ALCodeDomProvider.ArsslenLanguageCodeDomProvider(Properties, Path.GetDirectoryName(file),Asm);
                    TempService = new TempFileService();
                    TempService.TempDirectory = CodeDomProvider.TempDirectory;
               
                    ParserService = new ProjectParser(this);
                    ParserService.LoadProjectAsync();
                    Loaded = true;
                    NeedBuild = true;
                    alang = new LanguageConverter();
                }
            }
            catch
            {

            }
        }
        string ConvertImg(Image s)
        {
            MemoryStream stm = new MemoryStream();

            s.Save(stm, System.Drawing.Imaging.ImageFormat.Png);
            return Convert.ToBase64String(stm.GetBuffer());
        }

        string ConvertIco(Icon s)
        {
            MemoryStream stm = new MemoryStream();
            s.Save(stm);
            return Convert.ToBase64String(stm.GetBuffer());
        }

        public bool ImportRessource(string file, RessourceType tp)
        {
            if (tp == RessourceType.Icon)
            {
              string res = Convert.ToBase64String(File.ReadAllBytes(file));
              RessourceItem itm = new RessourceItem(Path.GetFileNameWithoutExtension(file), file, RessourceType.Icon, res,"Internal");
              if (!Ressources.ContainsKey(itm.Name))
                  Ressources.Add(itm.Name, itm);
              else
                  return false;
            }
            else if (tp == RessourceType.Image)
            {
                string res = Convert.ToBase64String(File.ReadAllBytes(file));
                RessourceItem itm = new RessourceItem(Path.GetFileNameWithoutExtension(file), file, RessourceType.Image, res, "Internal");
                if (!Ressources.ContainsKey(itm.Name))
                    Ressources.Add(itm.Name, itm);
                else
                    return false;
            }
            else if (tp == RessourceType.Data)
            {
                string res = Convert.ToBase64String(File.ReadAllBytes(file));
                RessourceItem itm = new RessourceItem(Path.GetFileNameWithoutExtension(file), file, RessourceType.Data, res, "Internal");
                if (!Ressources.ContainsKey(itm.Name))
                    Ressources.Add(itm.Name, itm);
                else
                    return false;
            }
            return true;
        }
       
        public void Build()
        {
            try
            {
                if(ParentSolution.Background != null)
                    if(ParentSolution.Background.IsBusy)
                         ParentSolution.Background.ReportProgress(0,Name+": "+ "Exporting ressources...");
                RessourceManager.RessourcesToCode(Ressources, Name, CodeDomProvider.ProjectDirectory + @"\res\" +Name+".res.al");
              //  File.WriteAllText(CodeDomProvider.TempDirectory + @"\" + Name + ".res.al", alang.InterpretAlToCs(File.ReadAllText(CodeDomProvider.TempDirectory + @"\" + Name + ".res.al")));
                List<string> files = new List<string>();
                CodeDomProvider.ReferencedAssemblies = Asm;
             if (ParentSolution.Background != null)
                 if (ParentSolution.Background.IsBusy)
                      ParentSolution.Background.ReportProgress(0, Name + ": " + "Saving files...");
                foreach (KeyValuePair<string, SourceFile> s in SourceFiles)
                {
                    if (s.Value.SourceType == SourceFileType.Event || s.Value.SourceType == SourceFileType.Form || s.Value.SourceType == SourceFileType.Source || s.Value.SourceType == SourceFileType.AsmInfo)
                    {
                        //if (File.Exists(CodeDomProvider.TempDirectory + @"\" + Path.GetFileName(s.Value.SourcePath)))
                        //    File.Delete(CodeDomProvider.TempDirectory + @"\" + Path.GetFileName(s.Value.SourcePath));
                    //    File.WriteAllText(GetTempFileName( s.Value.SourcePath), alang.InterpretAlToCs(File.ReadAllText(s.Value.SourcePath)));

                        files.Add(s.Value.SourcePath);
                    }
                    
                }
                

              //  File.WriteAllText(CodeDomProvider.TempDirectory + @"\CRT.al", alang.InterpretAlToCs(File.ReadAllText(Application.StartupPath + @"\Data\CRT.al")));
           //     files.Add(CodeDomProvider.TempDirectory + @"\CRT.al");
                files.Add(CodeDomProvider.ProjectDirectory + @"\res\" + Name + ".res.al");
           
                if (ParentSolution.Background != null)
                 if (ParentSolution.Background.IsBusy)
                   ParentSolution.Background.ReportProgress(0, Name + ": " + "Pre-Building files...");

                string[] fs = files.ToArray();
                bool prebuild = CodeDomProvider.PreBuild(ref fs, Name, alang, TempService);

                if (prebuild)
                {
                    if (ParentSolution.Background != null)
                        if (ParentSolution.Background.IsBusy)
                         ParentSolution.Background.ReportProgress(0, Name + ": " + "Prebuild succeeded! Building files...");

                    CodeDomProvider.CodeDomBuildCode(fs, this.Name, this.Target.version,TempService);
                }
                if (ParentSolution.Background != null)
                    if (ParentSolution.Background.IsBusy)
                     ParentSolution.Background.ReportProgress(0, Name + ": " + "Copying referenced assemblies...");

                foreach (ReferencedAssembly r in Asm.Values)
                {
                    if (r.Copy)
                    {
                        if (File.Exists(r.SourcePath) && !File.Exists(CodeDomProvider.OutputDirectory + @"\" + Path.GetFileName(r.SourcePath)))
                            File.Copy(r.SourcePath, CodeDomProvider.OutputDirectory + @"\" + Path.GetFileName(r.SourcePath));
                        else if (File.Exists(r.SourcePath))
                        {
                            File.Delete(CodeDomProvider.OutputDirectory + @"\" + Path.GetFileName(r.SourcePath));
                            File.Copy(r.SourcePath, CodeDomProvider.OutputDirectory + @"\" + Path.GetFileName(r.SourcePath));
                        }
                    }
                    
                }
                if (File.Exists(Application.StartupPath + @"\APPB.res"))
                    File.Delete(Application.StartupPath + @"\APPB.res");

                NeedBuild = false;

            }
            catch
            {

            }
        }
        public void Save()
        {
            try
            {
                using (StreamWriter str = new StreamWriter(FileName, false))
                {
                    str.WriteLine("<project name=\""+Name+"\" target=\"" + Target.version + "\" lang=\"AL\" version=\"1.0\">");
                    ProjectSource.SaveProjectSources(SourceFiles, str);
                    RessourceManager.SaveProjectRessources(Ressources, str);
                    Framework.Savessembblies(Asm, str);
                    Properties.Save(str);
                    str.WriteLine("</project>");
                }
            }
            catch
            {

            }
        }
        public void Clean()
        {
            try
            {
                string exe = this.CodeDomProvider.OutputDirectory + @"\" + this.Properties.Output;
                if (File.Exists(exe) && exe.EndsWith(".exe"))
                    File.Delete(exe);
            }
            catch
            {

            }
        }
        public void SaveAs(string filename)
        {
            try
            {
                using (StreamWriter str = new StreamWriter(filename, false))
                {
                    str.WriteLine("<project name=\"" + Name + "\" target=\"" + Target.version + "\" lang=\"AL\" version=\"1.0\">");
                    ProjectSource.SaveProjectSources(SourceFiles, str);
                    RessourceManager.SaveProjectRessources(Ressources, str);
                    Framework.Savessembblies(Asm, str);
                    Properties.Save(str);
                    str.WriteLine("</project>");
                }
            }
            catch
            {

            }
        }
    
    }
}
