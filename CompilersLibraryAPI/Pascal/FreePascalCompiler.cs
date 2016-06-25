using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CompilersLibraryAPI
{
    class FreePascalCompiler : ICompiler
    {
        private List<CompileMessage> _messages = new List<CompileMessage>();

        public string CompilerPath { get; set; }

        private string _cmdOptions ="";

        public event DataReceivedEventHandler OutputReceived;

        public bool compile(string filename, string res, string args)
        {
            ProcessStartInfo info;
            Process process;
            int exitCode;
            string cmdo = " " + '"' + filename + '"';
            string execFilename = null;
       
            info = new ProcessStartInfo(CompilerPath,cmdo+" "+args+_cmdOptions);
            info.CreateNoWindow = true;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.WorkingDirectory = Path.GetDirectoryName(filename);

            process = Process.Start(info);

            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
            
            process.BeginOutputReadLine();
            //output = process.StandardOutput.ReadToEnd();

            process.WaitForExit(10 * 60 * 1000);

            exitCode = process.ExitCode;
            process.Close();

            _messages.Clear();

            /*foreach (string message in output.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries))
            {
                parseOutputString(message);
            }*/

            return (exitCode == 0);
        }
        public bool compile(string filename, string res, string args,string exe)
        {
            ProcessStartInfo info;
            Process process;
            int exitCode;
            string cmdo = "-Mtp " + '"' + Path.GetFileName(filename) + '"';
            string execFilename = null;

            info = new ProcessStartInfo(exe, cmdo + args + _cmdOptions);
            info.CreateNoWindow = true;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.WorkingDirectory = Path.GetDirectoryName(filename);

            process = Process.Start(info);

            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            process.BeginOutputReadLine();
            //output = process.StandardOutput.ReadToEnd();

            process.WaitForExit(10 * 60 * 1000);

            exitCode = process.ExitCode;
            process.Close();

            _messages.Clear();

            /*foreach (string message in output.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries))
            {
                parseOutputString(message);
            }*/

            return (exitCode == 0);
            

         
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
            COutput.OutputReceived(e.Data, EventArgs.Empty);
                CompileMessage message = parseOutputString(e.Data);
                if (message != null)
                    OnMessageReceived(message);

                OnOutputReceived(e);
            }
        }

        private void OnOutputReceived(DataReceivedEventArgs e)
        {
            if (OutputReceived != null)
            {
                OutputReceived(this, e);
            }
        }

        private void OnMessageReceived(CompileMessage message)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageReceivedEventArgs(message));
            }
        }

        private CompileMessage parseOutputString(string output)
        {
            Regex reg = new Regex(@"^(?<file>[^\(]+)\((?<line>[\d]+),(?<char>[\d]+)\) (?<type>[^:]+): (?<message>.+)");

            Match m = reg.Match(output);

            CompileMessage result = null;

            if (m.Success)
            {
                string file = m.Groups["file"].Value;
                int line = Int32.Parse(m.Groups["line"].Value);
                int character = Int32.Parse(m.Groups["char"].Value);
                string typeStr = m.Groups["type"].Value;
                string message = m.Groups["message"].Value;

                CompileMessage.MessageTypes type;

                switch (typeStr)
                {
                    case "Note":
                        type = CompileMessage.MessageTypes.Note;
                        break;
                    case "Fatal":
                    case "Error":
                        type = CompileMessage.MessageTypes.Error;
                        break;
                    default:
                        type = CompileMessage.MessageTypes.Info;
                        break;
                }

                result = new CompileMessage(line, character, message, type, file,true);

                _messages.Add(result);
            }

            return result;
        }

        public List<CompileMessage> getMessages()
        {
            return _messages;
        }
        
        public CompileMessage parseRuntimeError(string error)
        {
            Regex errorRegex = new Regex(@"^Runtime error (?<code>[\d]+) at \$(?<address>[\d]+)");

            string[] messages = error.Replace("\r", "").Split(new char[] { '\n' });

            int i = 0;

            while (i < messages.GetUpperBound(0))
            {
                Match m = errorRegex.Match(messages[i]);

                if (m.Success)
                {
                    Regex location = new Regex(m.Groups["address"].Value + @".*line (?<line>[\d]+) of (?<file>.+)");
                    i++;
                    
                    while (i < messages.GetUpperBound(0))
                    {
                        Match mLoc = location.Match(messages[i]);

                        if (mLoc.Success)
                        {
                            int messageCode = int.Parse(m.Groups["code"].Value);

                            CompileMessage message = new CompileMessage(int.Parse(mLoc.Groups["line"].Value), 
                                0, "Runtime Error " + m.Groups["code"].Value + " : " + getRuntimeErrorDescription(messageCode), 
                                CompileMessage.MessageTypes.Error, true);
                            _messages.Add(message);
                            return message;
                        }

                        i++;
                    }
                }

                i++;
            }

            return null;
        }

        public event MessageReceivedEventHandler MessageReceived;

        public string getRuntimeErrorDescription(int error)
        {
            switch (error)
            {
                case 1:
                    return "Invalid function number";
                case 2:
                    return "File not found";
                case 3:
                    return "Path not found";
                case 4:
                    return "Too many open files";
                case 5:
                    return "File access denied";
                case 6:
                    return "Invalid handle";
                case 12:
                    return "Invalid file access code";
                case 15:
                    return "Invalid drive number";
                case 16:
                    return "Cannot remove current directory";
                case 17:
                    return "Cannot rename across drives";
                case 100:
                    return "Disk read error";
                case 101:
                    return "Disk write error";
                case 102:
                    return "File not assigned";
                case 103:
                    return "File not open";
                case 104:
                    return "File not open for input";
                case 105:
                    return "File not open for output";
                case 106:
                    return "Invalid numeric format";
                case 150:
                    return "Disk is write-protected";
                case 151:
                    return "Bad drive request struct length";
                case 152:
                    return "Drive not ready";
                case 154:
                    return "CRC error in data";
                case 156:
                    return "Disk seek error";
                case 157:
                    return "Unknown media type";
                case 158:
                    return "Sector not found";
                case 159:
                    return "Printer out of paper";
                case 160:
                    return "Device write fault";
                case 161:
                    return "Device read fault";
                case 162:
                    return "Hardware failure";
                case 200:
                    return "Division by zero";
                case 201:
                    return "Range check error";
                case 202:
                    return "Stack overflow error";
                case 203:
                    return "Heap overflow error";
                case 204:
                    return "Invalid pointer operation";
                case 205:
                    return "Floating point overflow";
                case 206:
                    return "Floating point underflow";
                case 207:
                    return "Invalid floating point operation";
                case 210:
                    return "Object not initialized";
                case 211:
                    return "Call to abstract method";
                case 212:
                    return "Stream registration error";
                case 213:
                    return "Collection index out of range";
                case 214:
                    return "Collection overflow error";
                case 215:
                    return "Arithmetic overflow error";
                case 216:
                    return "General Protection fault";
                case 217:
                    return "Unhandled exception occured";
                case 218:
                    return "Invalid typecast";
                case 222:
                    return "Variant dispatch error";
                case 223:
                    return "Variant array create";
                case 224:
                    return "Variant is not an array";
                case 225:
                    return "Var Array Bounds check error";
                case 227:
                    return "Assertion failed error";
                case 229:
                    return "Safecall error check";
                case 231:
                    return "Exception stack corrupted";
                case 232:
                    return "Threads not supported";
                default:
                    return "Unknown error";
            }
        }
    }
}