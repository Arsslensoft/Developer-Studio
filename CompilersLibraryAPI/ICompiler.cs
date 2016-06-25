using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace CompilersLibraryAPI
{
   public interface IECompiler
    {
       bool Compile(string file, CompilerPrefs prefs, ref bool compiled);
       List<CompileMessage> Errors { get; }
    }
    interface ICompiler
    {
        string CompilerPath { get; set; }

        bool compile(string filename, string res,string args);
        bool compile(string filename, string res, string args, string exe);
        List<CompileMessage> getMessages();

        event DataReceivedEventHandler OutputReceived;
        event MessageReceivedEventHandler MessageReceived;

        CompileMessage parseRuntimeError(string error);

        string getRuntimeErrorDescription(int error);
    }

    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    public class MessageReceivedEventArgs : EventArgs
    {
        public CompileMessage Message { get; set; }

        public MessageReceivedEventArgs(CompileMessage message)
        {
            this.Message = message;
        }
    }
}
