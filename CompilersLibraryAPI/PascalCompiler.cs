using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CompilersLibraryAPI
{
    public static class PascalCompiler
    {
        static List<CompileMessage> MSG = new List<CompileMessage>();
        public static string BuildArgs()
        {

            StringBuilder sb = new StringBuilder();

      
            //if (prefs.Platform == "x86")
            //{
                sb.Append(" -al -Twin32 -Pi386 \"-Fu" + (Application.StartupPath + "\\Pascal\\units\\i386-win32\""));

            //}
            //else
            //    sb.Append("  -Twin64 -Px86_64 \"-Fu" + (Application.StartupPath + "\\Pascal\\units\\x86_64-win64\" "));

       
            return sb.ToString();

        }
        static FreePascalCompiler comp = new FreePascalCompiler();
        public static List<CompileMessage> Errors
        {
            get
            {
                return MSG;
            }
           
        }
        static int lines = 0;
        public static bool Compile(string source)
        {
            try
            {
                MSG.Clear();
                lines = 0;
                if (File.Exists(source + ".bak"))
                    File.Replace(source, source + ".bak", Path.GetDirectoryName(source) + @"\last.bak");
                else
                    File.Copy(source, source + ".bak");

        
                comp.MessageReceived += new MessageReceivedEventHandler(comp_MessageReceived);
                
                comp.CompilerPath = Application.StartupPath + @"\Pascal\bin\i386-win32\fpc.exe";
               bool ret = comp.compile(source , "", BuildArgs());

               File.Replace(source+".bak", source, Path.GetDirectoryName(source) + @"\last0.bak");

               return ret;
            }
            catch
            {

            }
            return false;
        }
        static void comp_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                CompileMessage m = e.Message;
                m.LineNumber -= lines;
             if(!MSG.Contains(m))
                MSG.Add(m);
            }
            catch
            {

            }
        }
    }
}
