using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace CVP
{
   public class CVPReader
    {
        public string FileName { get; set; }
        public BinaryReader Reader { get; set; }
        public Stream Stream { get; set; }
        public bool Opened { get; set; }
        public bool Closed { get; set; }
        public CVPHeader Header { get; set; }
        public ulong InstructionsRead { get; set; }
        public CVPReader(string file)
        {
            Opened = false;
            Closed = false;
            FileName =  file;
            Stream = File.OpenRead(file);
          Stream = Decompress(Stream);
            Stream.Position = 0;
            Reader = new BinaryReader(Stream);
            Header = ReadHeader();
            InstructionsRead = 0;
            Opened = true;
  
        }
        public Stream Decompress(Stream input)
        {
            MemoryStream m = new MemoryStream();
            byte[] dataBuffer = new byte[4096];
            using (GZipInputStream gzipStream = new GZipInputStream(input))
            {

                // Change this to your needs

                StreamUtils.Copy(gzipStream, m, dataBuffer);

            }
            return m;
        }
        CVPHeader ReadHeader()
        {
            byte ver = Reader.ReadByte();
            byte lang = Reader.ReadByte();
            ulong tm = Reader.ReadUInt64();
            ulong ins = Reader.ReadUInt64();

            return new CVPHeader(lang,tm, ver, ins);

          //  return new CVPHeader(0, 0, 0);
        }
        public CVPInstruction ReadCurrentInstruction()
        {
            byte ins = Reader.ReadByte();
            int len = Reader.ReadInt32();
            byte[] data = new byte[len];

            Reader.Read(data,0,len);
            int line = Reader.ReadInt32();
            int col = Reader.ReadInt32();
            InstructionsRead++;
            return new CVPInstruction(ins, data,line,col);
        }
        public bool EndOfFile()
        {
            return (Stream.Position >= Stream.Length);
        }
        public void Finalize()
        {
            Reader.Close();
            Stream.Close();
            Closed = true;
        }
    }
}
