using System.IO.Compression;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace CVP
{
   public class CVPWriter
    {
       public string FileName { get; set; }
       public BinaryWriter Writer {get;set;}
       public FileStream Stream {get;set;}
       public bool Created { get; set; }
       public bool Saved { get; set; }
       public ulong InstructionsCount { get; set; }
       public List<CVPInstruction> Instructions {get;set;}
       public CVPWriter(string file)
       {
           Saved = false;
           Created = false;
           FileName = file;
          Stream = File.Create(file);
           Writer = new BinaryWriter(Stream);
           InstructionsCount = 0;
           Instructions = new List<CVPInstruction>();
           Created = true;

       }
       void WriteHeader(CVPHeader header)
       {
           Writer.Write(header.Version);//1 byte
           Writer.Write(header.Language);//1 byte
           Writer.Write(header.TimeMilliseconds);//8 bytes
           Writer.Write(header.InstructionsCount);//8 bytes

       }
     
       void WriteInstruction(CVPInstruction ins)
       {
           Writer.Write((byte)ins.Instruction);//1byte
           Writer.Write(ins.Length);// 2 bytes
           Writer.Write(ins.Data); // Length bytes
           Writer.Write(ins.Line);//4 bytes
           Writer.Write(ins.Column);//4 bytes
           InstructionsCount++;
       }
       public byte[] Extract(byte[] data, int offset, int length)
       {
           if (data.Length > length && offset >= 0 && offset + length <= data.Length)
           {
               byte[] d = new byte[length];
               for (int i = offset; i < offset + length; i++)
                   d[i - offset] = data[i];

               return d;
           }
           else
               return null;
       }

       public void AddInstruction(CVPInstruction ins)
       {
           if (ins.Data.LongLength < int.MaxValue || ins.Instruction != CVPINS.PUSH)
               Instructions.Add(ins);
           else
           {
               CVPInstruction i1 = new CVPInstruction((byte)CVPINS.PUSH, Extract(ins.Data, 0, int.MaxValue), ins.Line, ins.Column);
               CVPInstruction i2 = new CVPInstruction((byte)CVPINS.APPEND, Extract(ins.Data, int.MaxValue, ins.Data.Length - int.MaxValue), ins.Line, ins.Column);
               Instructions.Add(i1);
               Instructions.Add(i2);

           }
       }
       void Save(ulong time,byte lang)
       {
           WriteHeader(new CVPHeader(lang,time, 2, InstructionsCount));
           foreach (CVPInstruction ins in Instructions)
               WriteInstruction(ins);
       }
       public void Finalize(byte lang,ulong time)
       {
           Save(time,lang);
           Writer.Close();
           Stream.Close();
           
         Compress(FileName);
           Saved = true;
       }
       void Compress(string file)
       {
    using (FileStream outstream = new FileStream(file + ".gz", FileMode.Create))
			{
				Stream s = new GZipStream(outstream, CompressionMode.Compress, false);
FileStream fs = File.OpenRead(file);
int size;
byte[] data = new byte[2048];
do
{
    size = fs.Read(data, 0, data.Length);
    s.Write(data, 0, size);
} while (size > 0);
s.Close();
fs.Close();
}
File.Delete(file);
File.Move(file + ".gz", file);
       }
    }
}
