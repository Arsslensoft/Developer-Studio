using System;
using System.Collections.Generic;

using System.Text;

namespace CompilersLibraryAPI
{
    public class CompileMessage
    {
        public int LineNumber { get; set; }

        public int CharNumber { get; set; }

        public string Message { get; set; }
        public string FileName { get; set; }
        public string Code { get; set; }
        public string Project { get; set; }
        public enum MessageTypes : byte
        {
            Info = 0,
            Note = 1,
            Warning = 2,
            Error = 3
        };

        public MessageTypes Type { get; set; }
        public bool Compile;
        public CompileMessage(int lineNumber, int charNumber, string message, MessageTypes type, string file)
        {
            Compile = false;
            LineNumber = lineNumber;
            CharNumber = charNumber;
            Message = message;
            Type = type;
            FileName = file;
            Code = "COMP";
            Project = "";
        }
        public CompileMessage(int lineNumber, int charNumber, string message, MessageTypes type)
        {
            Compile = false;
            LineNumber = lineNumber;
            CharNumber = charNumber;
            Message = message;
            Type = type;
            FileName = "CURRENT";
            Code = "COMP";
            Project = "";
        }
        public CompileMessage(int lineNumber, int charNumber, string message, MessageTypes type, string file, bool com)
        {

            Compile = com;
            LineNumber = lineNumber;
            CharNumber = charNumber;
            Message = message;
            Type = type;
            FileName = file;
            Code = "COMP";
            Project = "";
        }
        public CompileMessage(int lineNumber, int charNumber, string message, MessageTypes type, bool com)
        {
            Compile = com;
            LineNumber = lineNumber;
            CharNumber = charNumber;
            Message = message;
            Type = type;
            Code = "COMP";
            Project = "";
        }
        public CompileMessage(int lineNumber, int charNumber, string message, MessageTypes type, string file, bool com, string code,string project)
        {

            Compile = com;
            LineNumber = lineNumber;
            CharNumber = charNumber;
            Message = message;
            Type = type;
            FileName = file;
            Code = code;
            Project = project;
        }


    }
}
