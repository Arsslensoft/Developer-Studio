using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVP
{
   public struct CVPHeader
    {
       public byte Version;
       public ulong TimeMilliseconds;
       public ulong InstructionsCount;
       public byte Language;
       public CVPHeader(byte lang,ulong time, byte name, ulong inscount)
       {
           Language = lang;
           TimeMilliseconds = time;
           Version = name;
           InstructionsCount = inscount;
       }
    }
}
