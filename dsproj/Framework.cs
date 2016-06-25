using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ALCodeDomProvider;
using Microsoft.Build.Utilities;

namespace alproj
{

    public class TargetFramework
    {
        public string version;
        public string RuntimeVersion;
        public string Name;
        public string BaseFrameworkPath;
        public TargetFramework(string ver, string runtime, string locations, string name)
        {
            version = ver;
            RuntimeVersion = runtime;
            BaseFrameworkPath = locations;
            Name = name;
        }
        public string FindRealName(string name)
        {
            if (File.Exists(BaseFrameworkPath + @"\" + name))
                return BaseFrameworkPath + @"\" + name;
            else
                return null;
        }
   
    }
   public static class Framework
    {
       public static List<TargetFramework> Targets;
       public static void LoadTargetFrameworks()
       {
           Targets = new List<TargetFramework>();
           try
           {

            
               Targets.Add(new TargetFramework("2.0", "", ToolLocationHelper.GetPathToDotNetFramework(
         TargetDotNetFrameworkVersion.Version20, DotNetFrameworkArchitecture.Bitness32), "Framework 2.0"));
            
               Targets.Add(new TargetFramework("3.0", "", ToolLocationHelper.GetPathToDotNetFramework(
         TargetDotNetFrameworkVersion.Version30, DotNetFrameworkArchitecture.Bitness32), "Framework 3.0"));

               Targets.Add(new TargetFramework("3.5", "", ToolLocationHelper.GetPathToDotNetFramework(
         TargetDotNetFrameworkVersion.Version35, DotNetFrameworkArchitecture.Bitness32), "Framework 3.5"));

               Targets.Add(new TargetFramework("4.0", "", ToolLocationHelper.GetPathToDotNetFramework(
         TargetDotNetFrameworkVersion.Version40, DotNetFrameworkArchitecture.Bitness32), "Framework 4.0"));

           }
           catch
           {

           }
       }
       public static TargetFramework GetTargetVersion(string ver)
       {
           foreach (TargetFramework t in Targets)
               if (t.version == ver)
                   return t;

           return null;
       }
       public static Dictionary<string,ReferencedAssembly> LoadAssemblies(XmlDocument prj)
       {
           Dictionary<string,ReferencedAssembly> Asm = new Dictionary<string,ReferencedAssembly>();
           try
           {
                XmlElement rel = (XmlElement)prj.DocumentElement.GetElementsByTagName("asm")[0];
                foreach (XmlElement el in rel)
                {
                    if (el.Name == "include")
                    {
                        if(!Asm.ContainsKey(el.GetAttribute("name")))
                        Asm.Add(el.GetAttribute("name"), new ReferencedAssembly(el.GetAttribute("name"),el.GetAttribute("path"),bool.Parse(el.GetAttribute("copy"))));
                    }
                }
           }
           catch
           {

           }
           return Asm;
       }
       public static void Savessembblies(Dictionary<string, ReferencedAssembly> Asm, StreamWriter str)
       {
           try
           {
               str.WriteLine("<asm>");
               foreach (KeyValuePair<string, ReferencedAssembly> p in Asm)
                   str.WriteLine("<include name=\"" + p.Key + "\" copy=\"" + p.Value.Copy.ToString() + "\" path=\"" + p.Value.SourcePath + "\" />");
          
               str.WriteLine("</asm>");
           }
           catch
           {

           }
       }
    }
}
