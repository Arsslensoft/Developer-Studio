using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace devstd.lang
{
   public static class PascalReflector
    {
       public static string[] Keywords = File.ReadAllLines(Application.StartupPath+@"\Data\Keywords.dat");
       public static bool Verify(string n)
       {
           foreach (string s in Keywords)
               if (s.ToUpper() == n)
                   return false;

           return true;
       }
       static public List<string> Reflect(string input)
       {
           List<string> methods = new List<string>();
           Regex _regex = new Regex(@"(?<name>\w*)\((?<params>.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
           bool sm = true;
           string ma = input;
           while (sm)
           {
               MatchCollection match = _regex.Matches(ma);
               if (match.Count > 0)
               {
                   foreach (Match m in match)
                   {
                       if (m.Success)
                       {
                           if (Verify(m.Groups["name"].Value.ToUpper()) && !string.IsNullOrEmpty(m.Groups["name"].Value) && !methods.Contains(m.Groups["name"].Value))
                           {
                               methods.Add(m.Groups["name"].Value);
                               sm = (_regex.Matches(m.Groups["params"].Value).Count > 0);

                               ma = m.Groups["params"].Value;
                           }
                       }
                       else
                           sm = false;

                   }
               }
               else
                   sm = false;

           }
           for (int i = 0; i <= methods.Count - 1; i++ )
           {
               string d = methods[i].Remove(0, 1);
               d = d.Insert(0, methods[i].ToUpper()[0].ToString());
               methods[i] = d;
           }
           return methods;

       }
       static public string GetProgramName(string input)
       {
           Regex _regex = new Regex(@"program\s+(?<name>\w+);", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

           MatchCollection match = _regex.Matches(input.ToLower());
           foreach (Match m in match)
           {
               if (m.Success)
               {
                  return m.Groups["name"].Captures[0].Value;
               }

           }
           return "Program";
       }
      
       public static void WriteMap(string code, string file, Dictionary<string,PASINTELIDATA> keywords)
       {
        
               string name = GetProgramName(code);
               List<string> Methods = Reflect(code);
               Dictionary<string, string> codes = new Dictionary<string, string>();

               for (int i = 0; i <= Methods.Count - 1; i++)
               {
                   if (keywords.ContainsKey(Methods[i]))
                   {
                       if (codes.ContainsKey(keywords[Methods[i]].NS))
                           codes[keywords[Methods[i]].NS] = codes[keywords[Methods[i]].NS] + Environment.NewLine + "<" + Methods[i] + " />";
                       else
                           codes.Add(keywords[Methods[i]].NS, "<" + Methods[i] + " />");

                   }
                   else
                   {
                       if (codes.ContainsKey("unknown"))
                           codes["unknown"] = codes["unknown"] + Environment.NewLine + "<" + Methods[i] + " />";
                       else
                           codes.Add("unknown", "<" + Methods[i] + " />");
                   }
               }

               using (StreamWriter str = new StreamWriter(file, false))
               {
                   str.WriteLine("<" + name + " type='basic'>");


                   foreach (KeyValuePair<string, string> p in codes)
                       str.WriteLine("<" + p.Key + ">" + p.Value + "</" + p.Key + ">");

                   str.WriteLine("</" + name + ">");
               }

           
       }
     
       
    }
}
