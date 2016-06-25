using ALCodeDomProvider;
using alproj;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace devstd
{
    public class GlobalTools
    {

        // TODO:PARSER RESULTS
        public static DSolution Solution;
        public static void UpdateForParse()
        {
    
            foreach (DSProjectItem dsp in Solution.Projects)
            {
                dsp.Project.LoadUpdateProjectNative();
            
            }
        }
    }
}
