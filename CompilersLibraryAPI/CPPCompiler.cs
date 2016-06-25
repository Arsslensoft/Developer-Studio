using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace CompilersLibraryAPI
{
   public static class CPPCompiler
    {
       public static string ErrorMessage = "";
       public static string BuildArgs(string file)
       {
           StringBuilder sb = new StringBuilder();
         


          
               sb.Append("-o \"" + Path.GetDirectoryName(file) + @"\"+Path.GetFileNameWithoutExtension(file) + ".exe" + "\" ");

          
               sb.Append("-g ");
          
              sb.Append("-Wall ");
           
        
               sb.Append("-march=i386 ");
             

  
                   sb.Append("-mconsole ");
           
           sb.Append("\"" + file + "\" ");
           return sb.ToString();

       }
       public static bool Compile(string source, ref bool compiled)
       {
           compiled
                = false;
           try
           {
               CommpilerMsg.Clear();
               ProcessStartInfo si = new ProcessStartInfo();
               if(source.EndsWith(".cpp"))
                si.FileName = Application.StartupPath + @"\MinGW\bin\g++.exe";
               else
                si.FileName = Application.StartupPath + @"\MinGW\bin\gcc.exe";

               si.Arguments = BuildArgs(source);
               si.CreateNoWindow = true;
               si.RedirectStandardError = true;
               si.RedirectStandardOutput = true;
               si.UseShellExecute = false;
               Process p = new Process();
                             
               p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
               p.ErrorDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
               p.StartInfo = si;
               p.Start();
               p.BeginOutputReadLine();
               p.BeginErrorReadLine();
        
            
               p.WaitForExit(10 * 60 * 1000);
              int exitCode = p.ExitCode;

               compiled = true;
               if (exitCode == 0)
                   return true;
               else
                   return false;
   
               
              
           }
           catch(Exception ex)
           {
               ErrorMessage = ex.Message;
           }
           return false;
       }
       public static List<CompileMessage> Errors
       {
           get { return CommpilerMsg; }
       }
      static List<CompileMessage> CommpilerMsg = new List<CompileMessage>();
       static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
       {
           try
           {
               if (e.Data != null)
               {
                   COutput.OutputReceived(e.Data, EventArgs.Empty);
                   if ((e.Data.ToLower().Contains("error") || e.Data.ToLower().Contains("warning")) && e.Data.Contains(":"))
                   {
                       string[] s = e.Data.ToLower().Split(':');
                       if (s.Length > 5)
                       {

                           CompileMessage msg = new CompileMessage(int.Parse(s[2]), int.Parse(s[3]), s[5], CompileMessage.MessageTypes.Warning, s[0] + ":" + s[1].Replace("/", @"\"), true);
                           if (s[4] == "error")
                               msg.Type = CompileMessage.MessageTypes.Error;
                           if (!CommpilerMsg.Contains(msg))
                           CommpilerMsg.Add(msg);
                       }
                       else if (s.Length == 5)
                       {
                           CompileMessage msg = new CompileMessage(int.Parse(s[2]), 0, s[4], CompileMessage.MessageTypes.Warning, s[0] + ":" + s[1].Replace("/", @"\"), true);
                           if (s[3] == "error")
                               msg.Type = CompileMessage.MessageTypes.Error;
                           if (!CommpilerMsg.Contains(msg))
                           CommpilerMsg.Add(msg);
                       }
                       else
                       {
                           CompileMessage msg = new CompileMessage(0, 0, e.Data, CompileMessage.MessageTypes.Error, "CURRENT", true);

                           if (!CommpilerMsg.Contains(msg))
                           CommpilerMsg.Add(msg);
                       }
                   }
               }
           }
           catch
           {

           }
       }
    }
}
