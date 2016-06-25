using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVP
{
   public enum CVPINS : byte
    {
       UNKNOWN = 0,
       PUSH = 1,
       WAIT = 2,
       SAYSYNC = 3,
       SAYASYNC = 4,
       APPEND = 5
    }
}
