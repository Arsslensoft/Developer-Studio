using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace devstd.lang
{
    public delegate void DetectSymbolsAsync(string ns, Dictionary<string, PASINTELIDATA> Keyword, Dictionary<string, PASINTELIDATA> Identifier);
  public static class PPUDiscover
    {
      static public void DetectType(string input, Dictionary<string, PASINTELIDATA> keyword, Dictionary<string, PASINTELIDATA> identifier, string ns)
      {
          try
          {
              Regex _regex = new Regex(@"(?<symboltype>\w+) symbol (?<name>\w+)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

              MatchCollection match = _regex.Matches(input);
              foreach (Match m in match)
              {
                  if (m.Success)
                  {
                      if (m.Groups["symboltype"].Value == "Procedure" || m.Groups["symboltype"].Value == "Function")
                      {
                          if (!keyword.ContainsKey(m.Groups["name"].Value.ToUpper()))
                              keyword.Add(m.Groups["name"].Value, new PASINTELIDATA(m.Groups["name"].Value + " is a " + m.Groups["symboltype"].Value + " found in the unit " + ns, m.Groups["name"].Value + "()", ns, 1));
                      }
                      else if (m.Groups["symboltype"].Value == "Type")
                      {
                          if (!identifier.ContainsKey(m.Groups["name"].Value.ToUpper()))
                              identifier.Add(m.Groups["name"].Value, new PASINTELIDATA(m.Groups["name"].Value + " is a " + m.Groups["symboltype"].Value + " found in the unit " + ns, m.Groups["name"].Value, ns, 0));
                      }
                      else if (m.Groups["symboltype"].Value == "Constant")
                      {
                          if (!keyword.ContainsKey(m.Groups["name"].Value.ToUpper()))
                              keyword.Add(m.Groups["name"].Value, new PASINTELIDATA(m.Groups["name"].Value + " is a " + m.Groups["symboltype"].Value + " found in the unit " + ns, m.Groups["name"].Value, ns, 4));
                      }
                      else if (m.Groups["symboltype"].Value == "Global Variable")
                      {
                          if (!keyword.ContainsKey(m.Groups["name"].Value.ToUpper()))
                              keyword.Add(m.Groups["name"].Value, new PASINTELIDATA(m.Groups["name"].Value + " is a " + m.Groups["symboltype"].Value + " found in the unit " + ns, m.Groups["name"].Value, ns, 3));
                      }

                  }

              }
          }
          catch
          {

          }
          finally
          {

          }
      }
      static bool ParsingSymbol = false;
      public static void DetectSymbols(string ns, Dictionary<string, PASINTELIDATA> Keyword, Dictionary<string, PASINTELIDATA> Identifier)
      {
          string ppu = Application.StartupPath + @"\Pascal\units\i386-win32\" + ns + ".ppu";
          if (!File.Exists(ppu) && File.Exists(Application.StartupPath + @"\Pascal\units\i386-win32\temp\" + ns + ".ppu"))
              ppu = Application.StartupPath + @"\Pascal\units\i386-win32\temp\" + ns + ".ppu";

          if (File.Exists(ppu))
          {
              ProcessStartInfo inf = new ProcessStartInfo(Application.StartupPath + @"\Pascal\bin\i386-win32\ppudump.exe", "-va " + '"' + ppu + '"');
              inf.WorkingDirectory = Application.StartupPath + @"\Pascal\bin\i386-win32";
              inf.UseShellExecute = false;
              inf.CreateNoWindow = true;
              inf.RedirectStandardOutput = true;


              Process p = Process.Start(inf);
              p.BeginOutputReadLine();
              p.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
                           {

                               if (e.Data != null)
                               {
                                   if (ParsingSymbol)
                                   {
                                       if (e.Data.StartsWith("Procedure ") || e.Data.StartsWith("Function ") || e.Data.StartsWith("Type ") || e.Data.StartsWith("Constant") || e.Data.StartsWith("Global Variable"))
                                       {
                                           DetectType(e.Data, Keyword, Identifier, ns);
                                           ParsingSymbol = false;
                                       }
                                   }

                                   if (e.Data.StartsWith("** Symbol Id "))
                                       ParsingSymbol = true;
                               }
                           };

              while (!p.HasExited)
                  Thread.Sleep(1000);
          }
      }

      static public string[] DetectNS(string input)
      {
          List<string> s = new List<string>();
          Regex _regex = new Regex(@"Uses+\s(?<ns>.*);", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

          MatchCollection match = _regex.Matches(input.ToLower());
          foreach (Match m in match)
          {
              if (m.Success)
                  s.AddRange(m.Groups["ns"].Value.Replace(" ","").Split(','));

          }
          return s.ToArray();
      }
      public static void DetectSymbolsAsynchronous(string ns, Dictionary<string, PASINTELIDATA> Keyword, Dictionary<string, PASINTELIDATA> Identifier)
      {
          try
          {
              DetectSymbolsAsync async = new DetectSymbolsAsync(DetectSymbols);
              async.BeginInvoke(ns, Keyword, Identifier, null, null);
          }
          catch
          {

          }
      }
    }
}
