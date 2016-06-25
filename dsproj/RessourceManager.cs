using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Drawing;

namespace alproj
{
    public enum RessourceType : byte
    {
        Image  = 0,
        String = 1,
        Integer = 2,
        Boolean = 3,
        Icon = 4,
        Real = 5,
        Data = 6

    }
    public struct RessourceItem
    {
        public string Name;
        public RessourceType Type;
        public string DevPath;
        public string Value;
        public string Modifier;
        //public RessourceItem(string name,string path,RessourceType type, string value modifier)
        //{
        //    Name = name;
        //    Type = type;
        //    DevPath = path;
        //    Value = "";
        //    Modifier = modifier;
        //}
        public RessourceItem(string name, string path, RessourceType type, string val, string modifier)
        {
            Name = name;
            Type = type;
            DevPath = path;
            Value = val;
            Modifier = modifier;
        }
    }
   public class RessourceManager
    {
       public const string IMGCode = "    static Image GetImg(string base64) \r\n   {  \r\n  byte[] data =       System.Convert.FromBase64String(base64);   \r\n   MemoryStream stm = new MemoryStream(data, true); \r\n  backwith Image.FromStream(stm); \r\n }";
       public const string DataCode = "    static byte[] GetData(string base64) \r\n   {    \r\n     backwith       System.Convert.FromBase64String(base64);     \r\n }";
       public const string ICOCode = "    static Icon GetIco(string base64) \r\n   {   \r\n     byte[] data =       System.Convert.FromBase64String(base64);      \r\n    MemoryStream stm = new MemoryStream(data, true);     \r\n    backwith new Icon(stm);    \r\n }";
       static string ConvertImg(Image s)
       {
           MemoryStream stm = new MemoryStream();

           s.Save(stm, System.Drawing.Imaging.ImageFormat.Png);
           return Convert.ToBase64String(stm.GetBuffer());
       }
       static string ConvertIco(Icon s)
       {
           MemoryStream stm = new MemoryStream();
           s.Save(stm);
           return Convert.ToBase64String(stm.GetBuffer());
       }
       public const string BasicCode = "($modifier$) static ($type$) ($name$) \r\n { \r\n get \r\n { \r\n backwith ($value$); \r\n } \r\n }";
       public static Dictionary<string, RessourceItem> Ressources;
       public static Dictionary<string,RessourceItem> LoadProjectRessources(XmlDocument proj)
       {
                      Ressources = new Dictionary<string, RessourceItem>();
           try
           {
    
               XmlElement rel = (XmlElement)proj.DocumentElement.GetElementsByTagName("res")[0];
               foreach (XmlElement el in rel)
               {
                   if (el.Name == "include")
                   {
                       if (!Ressources.ContainsKey(el.GetAttribute("name")))
                       {
                           RessourceItem item = new RessourceItem(el.GetAttribute("name"), el.GetAttribute("path"), (RessourceType)byte.Parse(el.GetAttribute("type")), el.GetAttribute("value"), el.GetAttribute("modifier"));
                           Ressources.Add(el.GetAttribute("name"),item);

                       }
                   }
               }
           }
           catch
           {

           }
           return Ressources;
       }
       public static void SaveProjectRessources(Dictionary<string, RessourceItem> res, StreamWriter str)
       {
           try
           {
               str.WriteLine("<res>");
               foreach (KeyValuePair<string, RessourceItem> p in res)
                   str.WriteLine("<include name=\"" + p.Key + "\" type=\"" + ((byte)p.Value.Type).ToString() + "\" path=\"" + p.Value.DevPath + "\" value=\"" + p.Value.Value + "\" modifier=\"" + p.Value.Modifier + "\" />");

               str.WriteLine("</res>");
           }
           catch
           {

           }
       }
       static string FormatCode(string modifier, string type, string name, string svalue)
       {
           return BasicCode.Replace("($modifier$)", modifier.ToLower()).Replace("($name$)", name).Replace("($type$)", type).Replace("($value$)",svalue);
       }
       public static void RessourcesToCode(Dictionary<string, RessourceItem> res, string program, string file)
       {
           using (StreamWriter str = new StreamWriter(file, false))
           {
               
               str.WriteLine("include System.Drawing;" );
               str.WriteLine("include System.IO;");

               str.WriteLine("program " + program +".Internal");
                str.WriteLine("{");
                str.WriteLine("public class Ressources");
                str.WriteLine("{");
                str.WriteLine(IMGCode);
                str.WriteLine(DataCode);
                str.WriteLine(ICOCode);
               foreach (KeyValuePair<string, RessourceItem> p in res)
               {
                   switch (p.Value.Type)
                   {
                       case RessourceType.Image:
                           str.WriteLine(FormatCode(p.Value.Modifier, "Image", p.Value.Name, "GetImg(\"" + p.Value.Value + "\")"));
                           break;
                       case RessourceType.String:
                           str.WriteLine(FormatCode(p.Value.Modifier, "string", p.Value.Name, "\"" + p.Value.Value + "\""));
                           break;
                       case RessourceType.Integer:
                           str.WriteLine(FormatCode(p.Value.Modifier, "integer", p.Value.Name, p.Value.Value));
                           break;
                       case RessourceType.Boolean:
                           str.WriteLine(FormatCode(p.Value.Modifier, "boolean", p.Value.Name, p.Value.Value));
                           break;
                       case RessourceType.Icon:
                           str.WriteLine(FormatCode(p.Value.Modifier, "Icon", p.Value.Name, "GetIco(\"" + p.Value.Value + "\")"));
                           break;
                      
                       case RessourceType.Real:
                           str.WriteLine(FormatCode(p.Value.Modifier, "real", p.Value.Name, p.Value.Value.Replace(",", ".")));
                           break;
                      
                       case RessourceType.Data:
                           str.WriteLine(FormatCode(p.Value.Modifier, "byte[]", p.Value.Name, "GetData(\"" + p.Value.Value + "\")"));
                           break;
                     
                   }
               }
               str.WriteLine("}");
               str.WriteLine("}");
           }
       }
    }
}
