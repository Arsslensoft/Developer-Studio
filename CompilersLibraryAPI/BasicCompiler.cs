using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace CompilersLibraryAPI
{
   public static class BasicCompiler
    {
        public static string ErrorMessage = "";
        public static string BuildArgs(string file,CompilerPrefs prefs)
        {
            StringBuilder sb = new StringBuilder();
            if (prefs.Debug)
                sb.Append("-g ");

            if (prefs.Output.Length > 0)
                sb.Append("-x \""+prefs.Output+"\" ");
            else
                sb.Append("-x \"" + Path.ChangeExtension(file, ".exe") + "\" ");

            if (prefs.Target == "library")
                    sb.Append("-dylib ");

            sb.Append("\"" + file + "\" ");

            return sb.ToString();

        }
        public static bool Compile(string source, CompilerPrefs pref, ref bool compiled)
        {
            compiled
                 = false;
            try
            {
                CommpilerMsg.Clear();
                ProcessStartInfo si = new ProcessStartInfo();
                si.FileName = Application.StartupPath + @"\Basic\fbc.exe";
                si.Arguments = BuildArgs(source,pref);
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
                        string[] s = e.Data.ToLower().Split('(');
                        string file = s[0];
                        string loc = s[1].Split(')')[0];
                        string err = s[1].Split(')')[1].Split(':')[0];
                        string amsg = s[1].Split(')')[1].Split(':')[1];
                        if (loc.Contains(","))
                        {
                            pos = int.Parse(loc.Split(',')[1]);
                            ln = int.Parse(loc.Split(',')[0]);
                        }
                        else
                            ln = int.Parse(loc);

                        CompileMessage msg = new CompileMessage(ln, pos, err +" : " +amsg, CompileMessage.MessageTypes.Warning, file, true);
                        if (err.Contains("error"))
                            msg.Type = CompileMessage.MessageTypes.Error;
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
