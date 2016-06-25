using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CVP
{
   public class CVPRecorder
    {
       long LastFrameMs=0;
       public CVPWriter Writer { get; set; }
       public Stopwatch TimeLaps { get; set; }
       public CVPSupportedLanguage Language { get; set; }
       public bool Recording { get; set; }
       public void Create(string file)
       {
           Recording = false;
           Language = CVPSupportedLanguage.AL;
           Writer = new CVPWriter(file);
           TimeLaps = new Stopwatch();
       }
       public void DetectLanguage(string file)
       {
           switch (Path.GetExtension(file))
           {
               case ".al":
                   Language = CVPSupportedLanguage.AL;
                   break;
               case ".asax":
                   Language = CVPSupportedLanguage.Asax;
                   break;
               case ".ashx":
                   Language = CVPSupportedLanguage.Ashx;
                   break;
               case "aspx":
               case ".asp":
                   Language = CVPSupportedLanguage.Aspx;
                   break;
               case ".cs":
                   Language = CVPSupportedLanguage.CSharp;
                   break;
               case ".css":
                   Language = CVPSupportedLanguage.Css;
                   break;
               case ".fs":
                   Language = CVPSupportedLanguage.FSharp;
                   break;
               case ".java":
                   Language = CVPSupportedLanguage.Java;
                   break;
               case ".js":
                   Language = CVPSupportedLanguage.JavaScript;
                   break;
               case ".php":
                   Language = CVPSupportedLanguage.Php;
                   break;
               case ".ps1":
                   Language = CVPSupportedLanguage.PowerShell;
                   break;
               case ".sql":
                   Language = CVPSupportedLanguage.Sql;
                   break;
               case ".vb":
                   Language = CVPSupportedLanguage.VisualBasic;
                   break;
               case ".xml":
                   Language = CVPSupportedLanguage.Xml;
                   break;
               case ".xaml":
                   Language = CVPSupportedLanguage.Xaml;
                   break;
               case ".pascal":
               case ".pp":
               case ".pas":
                   Language = CVPSupportedLanguage.Pascal;
                   break;
               case ".pwn":
               case ".cpp":
               case ".c":
               case ".h":
               case ".hpp":
                   Language = CVPSupportedLanguage.Cpp;
                   break;
               default:
                   Language = CVPSupportedLanguage.Cpp;
                   break;
           }
       }
       public void Record()
       {
           Recording = true;
           TimeLaps.Start();
           

       }

       public void AddWaitInstruction()
       {
           int timel = (int)(TimeLaps.ElapsedMilliseconds-LastFrameMs);
           LastFrameMs = TimeLaps.ElapsedMilliseconds;
           Writer.AddInstruction(new CVPInstruction((byte)CVPINS.WAIT,  BitConverter.GetBytes(timel), 0, 0));
        
       }
       public void AddCode(string code, int line, int colum)
       {
           AddWaitInstruction();
           Writer.AddInstruction(new CVPInstruction((byte)CVPINS.PUSH, Encoding.UTF8.GetBytes(code), line, colum));
       }
       public void AddSaySync(string text)
       {
           AddWaitInstruction();
           Writer.AddInstruction(new CVPInstruction((byte)CVPINS.SAYSYNC, Encoding.UTF8.GetBytes(text), 0, 0));

       }
       public void AddSayAsync(string text)
       {
           AddWaitInstruction();
           Writer.AddInstruction(new CVPInstruction((byte)CVPINS.SAYASYNC,Encoding.UTF8.GetBytes(text), 0, 0));

       }

       public void Stop()
       {
           TimeLaps.Stop();
           Writer.Finalize((byte)Language,(ulong)TimeLaps.ElapsedMilliseconds);
           Recording = false;
       }

    }
}
