using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace CompilersLibraryAPI
{

 
   public class CompilerPrefs
    {
       public string Output;
       public string Target;
       public string IconFile;
       public string EntryPoint;
       public string Platform;
       public string DocFile;
       public bool Debug;
       public bool Unsafe;
       public bool Optimize;
       public bool Checked;
       public bool ClsCheck;
       public bool WarnAsError;
       public List<string> Symbols;

       
    }
}
