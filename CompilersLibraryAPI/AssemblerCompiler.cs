using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace CompilersLibraryAPI
{
   public static class AssemblerCompiler
    {
        public static string ErrorMessage = "";
        public static string BuildArgs( string file)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + file + "\" ");
           
           sb.Append("-o \"" + Path.ChangeExtension(file, ".exe") + "\" ");

           
                sb.Append("-g ");

            sb.Append("-Wall ");
            sb.Append("-march=i386 ");

      
                    sb.Append("-mconsole ");
                
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
                si.FileName = Application.StartupPath + @"\MinGW\bin\gcc.exe";
                si.Arguments = BuildArgs(source);
                si.CreateNoWindow = true;
                si.UseShellExecute = false;
                si.RedirectStandardOutput = true;
                si.RedirectStandardError = true;
                Process p = new Process();

                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                p.ErrorDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                p.StartInfo = si;
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
        

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
                if (e.Data != null)
                {
                    COutput.OutputReceived(e.Data, EventArgs.Empty);
                    if ((e.Data.ToLower().Contains("error") || e.Data.ToLower().Contains("warning")) && e.Data.Contains(":"))
                    {
                        string[] s = e.Data.ToLower().Split(':');


                        CompileMessage msg = new CompileMessage(0, 0, s[3], CompileMessage.MessageTypes.Warning, s[0] + ":" + s[1],true);
                        if (s[2] == "error")
                            msg.Type = CompileMessage.MessageTypes.Error;

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
