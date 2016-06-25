using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace devstd.utils
{

       public static class SettingsManager
       {
           public static Dictionary<string, string> Prefs;

           public static void SetBool(string op, bool dat)
           {
               if (Prefs.ContainsKey(op))
                   Prefs[op] = dat.ToString();
               else
                   Prefs.Add(op, dat.ToString());
           }
           public static void Save()
           {
               try
               {
                   File.Delete(Application.StartupPath + @"\Config.conf");
                   FileStream fs = File.Create(Application.StartupPath + @"\Config.conf");
                   fs.Close();

                   using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\Config.conf"))
                   {
                       foreach (KeyValuePair<string, string> p in Prefs)
                           str.WriteLine(p.Key + "=" + p.Value);
                   }
               }
               catch
               {

               }
           }
           public static void SetString(string op, string dat)
           {
               if (Prefs.ContainsKey(op))
                   Prefs[op] = dat;
               else
                   Prefs.Add(op, dat);
           }
           public static void SetInt(string op, int dat)
           {
               if (Prefs.ContainsKey(op))
                   Prefs[op] = dat.ToString();
               else
                   Prefs.Add(op, dat.ToString());
           }
           public static void SetLong(string op, long dat)
           {
               if (Prefs.ContainsKey(op))
                   Prefs[op] = dat.ToString();
               else
                   Prefs.Add(op, dat.ToString());
           }
           public static bool GetBool(string op)
           {
               try
               {
                   if (Prefs.ContainsKey(op))
                   {
                       return Boolean.Parse(Prefs[op]);
                   }
                   else
                   {
                       return false;
                   }
               }
               catch (Exception ex)
               {
                   Log.Error(ex, 13);
               }
               return false;
           }

           public static int GetInt(string op)
           {
               try
               {
                   if (Prefs.ContainsKey(op))
                   {
                       return Int32.Parse(Prefs[op]);
                   }
                   else
                   {
                       return 0;
                   }
               }
               catch (Exception ex)
               {
                   Log.Error(ex, 13);
               }
               return 0;
           }
           public static long GetLong(string op)
           {
               try
               {
                   if (Prefs.ContainsKey(op))
                   {
                       return Int64.Parse(Prefs[op]);
                   }
                   else
                   {
                       return 0;
                   }
               }
               catch (Exception ex)
               {
                   Log.Error(ex, 13);
               }
               return 0;
           }
           public static string GetString(string op)
           {
               try
               {
                   if (Prefs.ContainsKey(op))
                   {
                       return Prefs[op];
                   }
                   else
                   {
                       return null;
                   }
               }
               catch (Exception ex)
               {
                   Log.Error(ex, 13);
               }
               return null;
           }
           public static void Init()
           {
               try
               {
                   Regex s = new Regex("=", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                   Prefs = new Dictionary<string, string>();
                   string[] l = File.ReadAllLines(Application.StartupPath + @"\Config.conf");
                   foreach (string ln in l)
                   {
                       string[] vals = s.Split(ln, 2);
                       Prefs.Add(vals[0], vals[1]);
                   }
               }
               catch (Exception ex)
               {
                   Log.Error(ex, 14);
               }
           }
       }
    
}
