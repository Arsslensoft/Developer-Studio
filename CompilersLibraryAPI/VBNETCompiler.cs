using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace CompilersLibraryAPI
{
    public static class VBNETCompiler
    {
        public static string ErrorMessage = "";
        public static string BuildArgs( string file)
        {

            StringBuilder sb = new StringBuilder();
      
          
    
                sb.Append("-out:\"" + Path.ChangeExtension(file, ".exe") + "\" ");

     
                sb.Append("/debug+ ");

        
            sb.Append("/platform:x86 ");

       
                    sb.Append("/target:exe ");
                  
            sb.Append("\"/r:System.Windows.Forms.dll,System.Drawing.dll,System.Data.dll,System.Xml.dll,System.dll,mscorlib.dll\" ");

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
                si.FileName = @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\vbc.exe";
                si.Arguments = BuildArgs(source);
                si.CreateNoWindow = true;
                si.UseShellExecute = false;
                si.RedirectStandardOutput = true;
                Process p = new Process();
                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
         
                p.StartInfo = si;
                p.Start();
                p.BeginOutputReadLine();


                p.WaitForExit();
                int exitCode = p.ExitCode;
                p.Close();
                compiled = true;
                if (exitCode == 0)
                    return true;
                else
                    return false;



            }
            catch (Exception ex)
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
                int pos = 0;
                int ln = 0;
                if (e.Data != null)
                {
                    COutput.OutputReceived(e.Data, EventArgs.Empty);
                    if ((e.Data.ToLower().Contains("error") || e.Data.ToLower().Contains("warning")) && e.Data.Contains(":"))
                    {
                        int start = 0;
                        string[] s = e.Data.ToLower().Split(':');
                        if (s[0].Length == 1)
                            start = 1;

                        string loc = s[start].Split('(')[1].Replace(")", "");
                        if (loc.Contains(","))
                        {
                            pos = int.Parse(loc.Split(',')[1]);
                            ln = int.Parse(loc.Split(',')[0]);
                        }
                        else
                            ln = int.Parse(loc);
                        CompileMessage msg = new CompileMessage(ln, pos, s[start + 2], CompileMessage.MessageTypes.Warning, s[start].Split('(')[0], true);
                        if (s[start + 1].Contains("error"))
                            msg.Type = CompileMessage.MessageTypes.Error;

                        if (start == 1)
                            msg.FileName = s[0] + ":" + msg.FileName;
                        if (!CommpilerMsg.Contains(msg))
                            CommpilerMsg.Add(msg);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
