using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ALCodeDomProvider
{

 
   public class ProjectSettings
    {
       public string Output {get;set;}
       public string Target { get; set; }
       public string IconFile { get; set; }
       public string EntryPoint { get; set; }
       public string Platform { get; set; }
       public string DocFile { get; set; }
       public bool Debug { get; set; }
       public bool Unsafe { get; set; }
       public bool Optimize { get; set; }
       public bool Checked { get; set; }
       public bool ClsCheck { get; set; }
       public bool WarnAsError { get; set; }
       public List<string> Symbols;

       public int WarnLevel { get; set; }
       public string KeyFile { get; set; }
       public bool DelaySign { get; set; }
       public bool Sign {get;set;}
       public bool HighEntropy { get; set; }
       public bool UTF8OUTPUT { get; set; }
       public string LangVersion   {get;set;}
       public string PreferedBuildLanguage { get; set; }
       public List<string> NoWarn;
       public List<string> WarnAsErrorList;

       public ProjectSettings(XmlElement setel)
       {
           try
           {
               Symbols = new List<string>();
               WarnAsErrorList = new List<string>();
               NoWarn = new List<string>();

               DocFile = setel.GetElementsByTagName("doc")[0].InnerText;
               Output = setel.GetElementsByTagName("output")[0].InnerText;
               Target = setel.GetElementsByTagName("target")[0].InnerText;
               IconFile = setel.GetElementsByTagName("icon")[0].InnerText;
               EntryPoint = setel.GetElementsByTagName("main")[0].InnerText;
               Platform = setel.GetElementsByTagName("platform")[0].InnerText;
              

               Debug = bool.Parse(setel.GetElementsByTagName("debug")[0].InnerText);
               Optimize = bool.Parse(setel.GetElementsByTagName("optimize")[0].InnerText);
               Checked = bool.Parse(setel.GetElementsByTagName("checked")[0].InnerText);
               ClsCheck = bool.Parse(setel.GetElementsByTagName("clscheck")[0].InnerText);
               WarnAsError = bool.Parse(setel.GetElementsByTagName("warnaserror")[0].InnerText);
               Unsafe = bool.Parse(setel.GetElementsByTagName("unsafe")[0].InnerText);


               Sign = bool.Parse(setel.GetElementsByTagName("sign")[0].InnerText);
               DelaySign = bool.Parse(setel.GetElementsByTagName("delaysign")[0].InnerText);
               UTF8OUTPUT = bool.Parse(setel.GetElementsByTagName("utf8output")[0].InnerText);
               HighEntropy = bool.Parse(setel.GetElementsByTagName("highentropy")[0].InnerText);
               WarnLevel = int.Parse(setel.GetElementsByTagName("warnlevel")[0].InnerText);
               KeyFile = setel.GetElementsByTagName("keyfile")[0].InnerText;
               LangVersion = setel.GetElementsByTagName("langversion")[0].InnerText;
               PreferedBuildLanguage = setel.GetElementsByTagName("prefbuildlang")[0].InnerText;

               foreach (XmlElement el in setel.GetElementsByTagName("nowarn")[0].ChildNodes)
                   NoWarn.Add(el.InnerText);

               foreach (XmlElement el in setel.GetElementsByTagName("warnlist")[0].ChildNodes)
                WarnAsErrorList.Add(el.InnerText);

               if (setel.GetElementsByTagName("symbols")[0].InnerText.Length > 0)
               {
                  
                   if (setel.GetElementsByTagName("symbols")[0].InnerText.Contains(","))
                       Symbols.AddRange(setel.GetElementsByTagName("symbols")[0].InnerText.Split(','));
                   else
                       Symbols.Add(setel.GetElementsByTagName("symbols")[0].InnerText);
               }
           }
           catch
           {

           }
       }
       public void Save(StreamWriter str)
       {
           try
           {
               str.WriteLine("<sets>");
               str.WriteLine("<output>"+Output+"</output>");
               str.WriteLine("<target>" + Target + "</target>");
               str.WriteLine("<icon>" + IconFile + "</icon>");
               str.WriteLine("<main>" + EntryPoint + "</main>");
               str.WriteLine("<platform>" + Platform + "</platform>");
               str.WriteLine("<doc>" + DocFile + "</doc>");

               str.WriteLine("<debug>" + Debug.ToString() + "</debug>");
               str.WriteLine("<unsafe>" + Unsafe.ToString() + "</unsafe>");
               str.WriteLine("<optimize>" + Optimize.ToString() + "</optimize>");
               str.WriteLine("<clscheck>" + ClsCheck.ToString() + "</clscheck>");
               str.WriteLine("<checked>" + Checked.ToString() + "</checked>");
               str.WriteLine("<warnaserror>" + WarnAsError.ToString() + "</warnaserror>");



               str.WriteLine("<sign>" + Sign.ToString() + "</sign>");
               str.WriteLine("<delaysign>" + DelaySign.ToString() + "</delaysign>");
               str.WriteLine("<highentropy>" + HighEntropy.ToString() + "</highentropy>");
               str.WriteLine("<utf8output>" + UTF8OUTPUT.ToString() + "</utf8output>");
               str.WriteLine("<warnlevel>" + WarnLevel.ToString() + "</warnlevel>");

               str.WriteLine("<keyfile>" + KeyFile + "</keyfile>");
               str.WriteLine("<langversion>" + LangVersion + "</langversion>");
               str.WriteLine("<prefbuildlang>" + PreferedBuildLanguage + "</prefbuildlang>");

           string symb = "";
               foreach(string sym in Symbols)
                   symb += sym+",";

               if(symb.Length > 0)
                   symb = symb.Remove(symb.Length-1,1);

               str.WriteLine("<symbols>"+symb+"</symbols>");

               str.WriteLine("<nowarn>");
               foreach (string s in NoWarn)
                   str.WriteLine("<warning>" + s + "</warning>");
               str.WriteLine("</nowarn>");

               str.WriteLine("<warnlist>");
               foreach (string sx in WarnAsErrorList)
                   str.WriteLine("<warning>" + sx + "</warning>");
               str.WriteLine("</warnlist>");

               str.WriteLine("</sets>");
           }
           catch
           {

           }
       }
       
    }
}
