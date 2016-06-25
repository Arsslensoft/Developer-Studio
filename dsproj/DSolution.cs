using ALCodeDomProvider;
using CompilersLibraryAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace alproj
{
    public class DSProjectItem
    {
        public ALProject Project;
        public byte BuildOrder = 0;
        public bool Build = true;
        public bool Startup = false;
    }
   public class DSolution
    {
       public string FileName;
       public string SolutionDirectory;
       public string CurrentVersion;
       public string Name;
       public DSolution(string file, string ver)
       {
           FileName = file;
           Name = Path.GetFileNameWithoutExtension(FileName);
           SolutionDirectory = Path.GetDirectoryName(file);
           CurrentVersion = ver;
           Projects = new List<DSProjectItem>();
           Errors = new List<CompileMessage>();
       }
       string ReplaceArgs(string file)
       {
           return file.Replace("{$SolutionDir$}", SolutionDirectory).Replace("{$equal$}", "=");
       }
       string ReverseReplaceArgs(string file)
       {
           return file.Replace(SolutionDirectory,"{$SolutionDir$}").Replace("=","{$equal$}");
       }
       public List<DSProjectItem> Projects;
       public BackgroundWorker Background { get; set; }
       public bool Open(BackgroundWorker bgw)
       {
           try
           {
               Background = bgw;
               Projects.Clear();
               if (!File.Exists(FileName))
                   return false;
               if (Path.GetExtension(FileName) != ".dsol")
                   return false;
               string[] lin = File.ReadAllLines(FileName);
               if (lin[0] == "Arsslensoft Developer Studio Solution File" && lin[1] == CurrentVersion)
               {
                   int i = 2;
               for(i = 3; i < lin.Length -1; i++)
               {
                 
                       string projectpath = ReplaceArgs(lin[i].Split('=')[0]);
                       byte order = byte.Parse(lin[i].Split('=')[1]);
                       DSProjectItem proj = new DSProjectItem();
                       proj.Build = bool.Parse(lin[i].Split('=')[2]);
                       proj.Startup = bool.Parse(lin[i].Split('=')[3]);
                       proj.BuildOrder = order;
                       Background.ReportProgress(0, "Loading "+projectpath);
                       if (File.Exists(projectpath))
                           proj.Project = new ALProject(projectpath);
                       else proj.Project = null;

                       proj.Project.ParentSolution = this;
                       Projects.Add(proj);
                      
                   }
               Background.ReportProgress(0, "Loading User Data");
          
               Background.ReportProgress(0, "Done");
                   return true;
               }
               else
                   return false;
           }
           catch
           {
               return false;
           }
       }

       public bool SortByBuildOrder()
       {
           try
           {
               bool sorted = true;
               do
               {
                   sorted = true;
                   for (int i = 0; i < Projects.Count-1; i++)
                   {
                       if (Projects[i].BuildOrder > Projects[i + 1].BuildOrder)
                       {
                           DSProjectItem aux = Projects[i];
                           Projects[i] = Projects[i + 1];
                           Projects[i + 1] = aux;
                           sorted = false;
                       }
                   }
               } while (sorted == false);
               return true;
           }
           catch
           {
               return false;
           }
       }
       public int Contains(string projectname)
       {
           int i = 0;
           bool e = false;
           while (i < Projects.Count && e == false)
           {
             
               e = (projectname == Projects[i].Project.Name);
               i++;
           }
           if (e == false)
               return -1;
           return (i-1);
       }
       public bool AddEditProject(ALProject project, byte buildorder,bool build)
       {
           try
           {
               if (project != null)
               {
                   int w = Contains(project.Name);
                   if (w == -1)
                   {
                       DSProjectItem proj = new DSProjectItem();
                       proj.Project = project;
                       proj.BuildOrder = buildorder;
                       proj.Build = build;
                       Projects.Add(proj);
                   }
                   else
                   {
                       Projects[w].Project = project;
                       Projects[w].BuildOrder = buildorder;
                       Projects[w].Build = build;
                   }
                   return true;
               }
               return false;

           }
           catch
           {
               return false;
           }
       }
       public bool CanAddReference(string project, string used)
       {
           try
           {
               int p = Contains(project);
               int u = Contains(used);
               int x = 0;
               if (p != -1 && u != -1)
               {
                   // check if the project already referenced
                   if (Projects[p].Project.Asm.ContainsKey(used))
                       return false;

                   // check the one way relation used=>project
                   if (Projects[u].Project.Asm.ContainsKey(project))
                       return false;

                   // check the triangle relation A uses B / C uses A  / B USES C

                   foreach (KeyValuePair<string,ReferencedAssembly> pa in Projects[u].Project.Asm)
                   {

                       if (pa.Value.Project == true)
                       {
                           // Get The C Project index
                           x = Contains(pa.Key);
                           if (x != -1)
                           {
                               if (Projects[x].Project.Asm.ContainsKey(project))
                                   return false;
                           }
                       }
                     
                   }


                   return true;
               }
               return false;
           }
           catch
           {
               return false;
           }
       }

       void Move(ref DSProjectItem[] r, int pos, int newpos)
       {
           int pas = 1;
           if (pos > newpos)
           {
               pas = pos;
               pos = newpos;
               newpos = pos;
           }
           DSProjectItem aux = r[newpos];
           int j = newpos;
           while (j > pos && j >= 0)
           {
               r[j] = r[j - 1];
               j--;

           }
           r[pos] = aux;

       }
       int GetPos(DSProjectItem[] x, string a)
       {
           for (int i = 0; i < x.Length; i++)
           {
               if (x[i].Project.Name == a)
                   return i;
           }
           return -1;
       }
       public ALProject GetProjectByName(string name)
       {
           foreach (DSProjectItem it in Projects)
               if (it.Project.Name == name)
                   return it.Project;
           return null;
       }
       public bool BuildOrderWork()
       {
           try
           {

               int newpos = 0;
               DSProjectItem[] y = Projects.ToArray();
               for (int i = 0; i < Projects.Count; i++)
               {
                   newpos = GetPos(y, Projects[i].Project.Name);
                   DSProjectItem pr = Projects[i];
                   // check refs
                   if (pr.Project != null)
                   {
                       // get referenced by project
                  
                       // try sort
                       foreach (KeyValuePair<string,ReferencedAssembly> refe in pr.Project.Asm)
                       {
                           // Find Pos
                           for (int j = 0; j < Projects.Count; j++)
                           {
                               if (y[j].Project.Name == refe.Key)
                               {
                                   
                                   // move the current project to the used position
                                   Move(ref y, newpos, j);
                                   // set current new pos
                                   newpos = j;
                                   break;
                               }
                           }

                       }
                   }

               }
               Projects.Clear();
               Projects.AddRange(y);
               // Assign Build Order
               for (int i = 0; i < Projects.Count; i++)
                   Projects[i].BuildOrder = (byte)(i+1);
          
               return true;

           }
           catch
           {
               return false;
           }
       }
       public bool Save(BackgroundWorker bgw)
       {
           try
           {
               Background = bgw;
               if (File.Exists(FileName))
                   File.Delete(FileName);
               Background.ReportProgress(0, "Saving projects...");

               foreach (DSProjectItem proj in Projects)
                   proj.Project.Save();
               Background.ReportProgress(0, "Saving solution...");
               using (StreamWriter str = new StreamWriter(FileName, false))
               {
                   str.WriteLine("Arsslensoft Developer Studio Solution File");
                   str.WriteLine(CurrentVersion);
                   str.WriteLine("BeginProjects");
                   for (int i = 0; i < Projects.Count; i++)
                       str.WriteLine(ReverseReplaceArgs(Projects[i].Project.FileName) + "=" + Projects[i].BuildOrder.ToString() + "=" + Projects[i].Build.ToString() + "=" + Projects[i].Startup.ToString());
                   str.WriteLine("EndProjects");
               }
               return true;
           }
           catch
           {
               return false;
           }
       }

       public void UpdateProject(ALProject current)
       {
           foreach (DSProjectItem it in Projects)
           {
               if (it.Project.FileName == current.FileName)
                   it.Project = current;
           }

       }
       public static DSolution CreateSolutionForProject(ALProject proj, string ver)
       {
           DSolution sol = null;
           string file = Path.ChangeExtension(proj.FileName, ".dsol");
           if (File.Exists(file))
               return new DSolution(file, ver);
           else
           {
               using (StreamWriter str = new StreamWriter(file, false))
               {
                   str.WriteLine("Arsslensoft Developer Studio Solution File");
                   str.WriteLine(ver);
                   str.WriteLine("BeginProjects");
                   str.WriteLine(proj.FileName.Replace(Path.GetDirectoryName(file), "{$SolutionDir$}").Replace("=", "{$equal$}") + "=1=True=True");
                   str.WriteLine("EndProjects");
               }
     
               return new DSolution(file, ver);
           }

       }

       public ALProject GetStartupProject()
       {
           foreach (DSProjectItem it in Projects)
           {
               if (it.Startup)
                   return it.Project;
           }
           return Projects[0].Project;
       }
       public void SetStartupProject(string name)
       {
           for (int i = 0; i < Projects.Count; i++)
           {
               if (Projects[i].Project.Name == name)
                   Projects[i].Startup = true;
               else
                  Projects[i].Startup = false;
           }
       }
       public List<DSProjectItem> GetProjectsToBuild()
       {
           List<DSProjectItem> proj = new List<DSProjectItem>();
           BuildOrderWork();
           int i = 0;
           foreach (DSProjectItem prij in Projects)
           {
               if (prij.Project.NeedBuild || prij.Startup )
                   break;

               i++;
           }
           if (i < Projects.Count)
           {
               // Copy From Position
               for (int j = 0; j < Projects.Count; j++)
                   proj.Add(Projects[j]);
           }
          
           return proj;

            
       }
       public void Build(BackgroundWorker bgw)
       {
           Background = bgw;
           List<DSProjectItem> proj = GetProjectsToBuild();
           Errors.Clear();
           if (proj.Count > 0)
           {
               // build
               foreach (DSProjectItem pro in proj)
                   pro.Project.Build();
            
           }

       }
       public void ReBuild(BackgroundWorker bgw)
       {
           Background = bgw;
           Errors.Clear();
          
               foreach (DSProjectItem pro in  Projects)
                   pro.Project.Build();

   

       }

       public void Clean()
       {
    
           foreach (DSProjectItem pro in Projects)
               pro.Project.Clean();



       }

      public void GetErrors()
       {
           foreach (DSProjectItem proj in Projects)
               Errors.AddRange(proj.Project.CodeDomProvider.CompileMessages.ToArray());
       }

       public List<CompileMessage> Errors
       {
           get;
           set;
       }
    }
}
