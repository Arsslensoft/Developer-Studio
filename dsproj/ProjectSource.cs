using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace alproj
{
    public enum SourceFileType : byte
    {
        Form = 0,
        Source = 1,
        Event = 2,
        AsmInfo = 3,
        Folder = 4
    }
    public struct SourceFile
    {
        public string Name;
        public string SourcePath;
        public string ProjectPath;
        public SourceFileType SourceType;
        public SourceFile(string name,string path,string projpath,SourceFileType type)
        {
            Name = name;
            ProjectPath = projpath;
            SourcePath = path;
            SourceType = type;
        }
    }
  public static class ProjectSource
    {
     static Dictionary<string, SourceFile> Ressources;
        public static Dictionary<string, SourceFile> LoadProjectSources(XmlDocument proj)
        {
            Ressources = new Dictionary<string, SourceFile>();
            try
            {

                XmlElement rel = (XmlElement)proj.DocumentElement.GetElementsByTagName("src")[0];
                foreach (XmlElement el in rel)
                {
                    if (el.Name == "include")
                    {
                        if (!Ressources.ContainsKey(el.GetAttribute("projpath")))
                        {
                            SourceFile item = new SourceFile(el.GetAttribute("name"), el.GetAttribute("path"),el.GetAttribute("projpath"), (SourceFileType)byte.Parse(el.GetAttribute("type")));
                            Ressources.Add(el.GetAttribute("projpath"), item);

                        }
                    }
                }
            }
            catch
            {

            }
            return Ressources;
        }
        public static void SaveProjectSources(Dictionary<string, SourceFile> res, StreamWriter str)
        {
            try
            {
                str.WriteLine("<src>");
                foreach (KeyValuePair<string, SourceFile> p in res)
                    str.WriteLine("<include name=\"" + p.Value.Name + "\" type=\"" + ((byte)p.Value.SourceType).ToString() + "\" path=\"" + p.Value.SourcePath + "\" projpath=\"" +p.Key+"\" />");

                str.WriteLine("</src>");
            }
            catch
            {

            }
        }
    }
}
