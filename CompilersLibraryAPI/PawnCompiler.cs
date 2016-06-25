using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using PawnParser;

namespace CompilersLibraryAPI
{
    public static class PawnCompiler
    {
        public static string ErrorMessage = "";
        public static string BuildArgs(string file)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + file + "\" ");
            //if (prefs.Debug)
            //    sb.Append("-d"+prefs.DebugValue.ToString() + " ");

            //if (prefs.Optimize)
            //    sb.Append("-O" + prefs.OptimizeValue.ToString() + " ");

            //if (prefs.Output.Length > 0)
         sb.Append("-o=\"" +Path.ChangeExtension(file,".amx") + "\" ");

            //if (prefs.IncludePath.Length > 0)
            //    sb.Append("-i=\"" + prefs.IncludePath + "\" ");

            //if (prefs.Assembler)
            //    sb.Append("-a ");

            //if (prefs.Linker)
            //    sb.Append("-l ");




            return sb.ToString();

        }
        public static bool Compile(string source, ref bool compiled)
        {
            compiled
                 = false;
            try
            {
                CompilingFile = source;
                CommpilerMsg.Clear();
                ProcessStartInfo si = new ProcessStartInfo();
                si.FileName = Application.StartupPath + @"\Pawn\pawncc.exe";
                si.Arguments = BuildArgs(source);
                si.WorkingDirectory = Application.StartupPath + @"\Pawn";
                si.CreateNoWindow = true;
                si.UseShellExecute = false;
                si.RedirectStandardOutput = true;
                si.RedirectStandardError = true;
                Process p = new Process();
                p.StartInfo = si;
                p.Start();
                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                p.ErrorDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
              
                p.WaitForExit();
                //string output = p.StandardOutput.ReadToEnd();
                //string err = p.StandardError.ReadToEnd();
                int exitCode = p.ExitCode;
                p.Close();

                //string terr = (err.IndexOf("Compilation") != -1) ? err.Remove(err.IndexOf("Compilation")) : err;
                //terr = terr.Trim();
                //string[] errors = terr.Split('\n');
                //if (err.Length > 10)
                //{
                //    foreach (string error in errors)
                //    {
                //        Error err_elems = PawnParser.ErrorParser.ParseCompilerError(error);
                //        CompileMessage msg = new CompileMessage(err_elems.Line, 1, err_elems.Description, CompileMessage.MessageTypes.Info);
                //        if (pref.WarnAsError)
                //            msg.Type = CompileMessage.MessageTypes.Error;
                //        else
                //        {
                //            if (err_elems.Type == ErrorType.Error)
                //                msg.Type = CompileMessage.MessageTypes.Error;
                //            else
                //                msg.Type = CompileMessage.MessageTypes.Warning;
                //        }
                //        CommpilerMsg.Add(msg);
                //    }
                //}
                compiled = File.Exists(Path.ChangeExtension(source,".amx"));
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
        public static string CompilingFile="";
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
                    string error = e.Data;
                    Error err_elems = PawnParser.ErrorParser.ParseCompilerError(error);
                    CompileMessage msg = new CompileMessage(err_elems.Line, 1, err_elems.Description, CompileMessage.MessageTypes.Info, CompilingFile,true);

                        if (err_elems.Type == ErrorType.Error)
                            msg.Type = CompileMessage.MessageTypes.Error;
                        else
                            msg.Type = CompileMessage.MessageTypes.Warning;
                    
                    CommpilerMsg.Add(msg);
                }
            }
            catch
            {

            }
        }
    }
}
