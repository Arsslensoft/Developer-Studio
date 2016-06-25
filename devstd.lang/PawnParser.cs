using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace PawnParser
{
    /// <summary>
    /// PAWN Code Parser - designed by Skaty
    /// ---
    /// NOTE: This is different from AMX parsing.
    /// ---
    /// TODO: Cleanup and documentation. [PROGRESS]
    /// TODO: Seperate into DLL. [DONE]
    /// TODO: Add overloading support. [PLANNING]
    /// TODO: Add threading and optimization [PROGRESS]
    /// </summary>
    public class Parser
    {
        public c_FileFnc[] incarry;
        public c_FileFnc curFile = new c_FileFnc(null);
      public int counter = 0, unmatchedBrace = 0;
        static bool dlmKomen = false; // Comment switch
        bool ContinueDef;

        public Parser() { }

        /// <summary>
        /// Parses an include file for anything useful!
        /// </summary>
        /// <param name="fn">The filename of the include</param>
        /// <param name="cd">The code</param>
        public void CodeParser(string fn, string cd, bool include, string fd)
        {
            try
            {
                c_FileFnc curRef = new c_FileFnc(fn);
                string[] lines = cd.Split('\n');
                string DetectionType = fd;
                int linenum = -1;
            GoBack:
                foreach (string line in lines)
                {
                    try
                    {
                        linenum++;
                        string cl;
                        cl = line.Trim();
                        if (DetectionType == "default" || include == false) // If someone wants PAWNO way, we don't care about comments or braces
                        {
                            cl = buangKomen(cl);
                            cl = BraceCheck(cl);
                        }
                        //if (cl == "#pragma rational Float") break; // float.inc causes TOO many problems
                        if (cl == null || cl == "") continue;
                        c_function tmp;
                        switch (CheckLineForTypes(cl, DetectionType, include))
                        {
                            case CodeType.None: continue;
                            case CodeType.Function:
                                if (cl.IndexOf("forward") != -1 & include == false) continue; // forwards are ignored when checking file.
                                tmp = Digest(cl, fn, linenum, CodeType.Function);
                                if (!curRef.functions.Keys.Contains<string>(tmp.fName)) curRef.functions.Add(tmp.fName, tmp);
                                break;

                            case CodeType.Definition:
                                if (DetectionType == "pawno" & include == true)
                                {
                                    cl = buangKomen(cl);
                                    if (cl == null || cl == "") continue;
                                }
                                tmp = Digest(cl, fn, linenum, CodeType.Definition);
                                if (!curRef.constants.Keys.Contains<string>(tmp.fName)) curRef.constants.Add(tmp.fName, tmp);
                                break;

                            case CodeType.Variable:
                                if (DetectionType == "pawno" & include == true)
                                {
                                    cl = buangKomen(cl);
                                    if (cl == null || cl == "") continue;
                                }
                                string[] frnt = { "new " };
                                cl = cl.Split(frnt, StringSplitOptions.RemoveEmptyEntries)[0];
                                if (cl.IndexOf(',') != -1)
                                {
                                    string first = "void";
                                    bool fone = false;
                                    foreach (string var in cl.Split(','))
                                    {
                                        string v = var.Trim();

                                        tmp = Digest(v, fn, linenum, CodeType.Variable);
                                        if (!fone)
                                        {
                                            first = tmp.fReturn;
                                            fone = true;
                                        }
                                        tmp.fReturn = first;
                                        if (!curRef.variables.Keys.Contains<string>(tmp.fName)) curRef.variables.Add(tmp.fName, tmp);
                                    }
                                }
                                else
                                {
                                    tmp = Digest(cl, fn, linenum, CodeType.Variable);
                                    if (!curRef.variables.Keys.Contains<string>(tmp.fName)) curRef.variables.Add(tmp.fName, tmp);
                                }
                                break;

                        }
                    }
                    catch
                    {

                    }
                }
                if (fd == "backward" && DetectionType == "backward")
                {
                    DetectionType = "pawno";
                    goto GoBack;
                }
                if (include == true) incarry[counter] = curRef;
                else curFile = curRef;
                counter++;
            }
            catch
            {

            }
        }

        /// <summary>
        /// Searchs for a function in include files.
        /// </summary>
        /// <param name="func">The function you want to retrieve</param>
        /// <returns>A c_function, if the function exsists or null if it does not!</returns>
        public c_function SearchForFunc(string func)
        {
            foreach (c_FileFnc inc in incarry)
            {
                if (inc.functions.Keys.Contains<string>(func)) return inc.functions[func];
            }
            return null;
        }

        /// <summary>
        /// Counts the number of times a function appears in a certain code.
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="func">The function</param>
        /// <returns>An integer with the number of times.</returns>
        public static int GetFunctionsInCode(string code, string func)
        {
            int times = 0;
            dlmKomen = false;
            string[] codelines = code.Split('\n');
            foreach (string codeline in codelines)
            {
                string rehabline = buangKomen(codeline.Trim());
                if (rehabline == null) continue;
                if (rehabline.IndexOf(func) != -1) times++;
            }
            return times;
        }

        /// <summary>
        /// The type of code.
        /// </summary>
        public enum CodeType
        {
            None,
            Function,
            Definition,
            Variable
        }

        /// <summary>
        /// Digests a code type into yummy bits.
        /// </summary>
        /// <param name="line">Line containing the code</param>
        /// <param name="numLine">Name of the file!</param>
        /// <param name="numLine">Line number, obviously</param>
        /// <param name="type">The CodeType</param>
        /// <returns>Yummy mashed potatoes</returns>
        public c_function Digest(string line, string FileName, int numLine, CodeType type)
        {
            c_function stor = new c_function();
            stor.fLn = numLine;
            stor.fFileName = FileName;
            switch (type)
            {
                case CodeType.Function:
                    string[] front = { "native ", "stock ", "public ", "forward ", "static " };
                    foreach (string part in front) { line = line.Replace(part, ""); } // lol in front :P
                    line = line.TrimEnd(')', ';');
                    string fNR = line.Split('(')[0];
                    if (fNR.IndexOf(':') != -1)
                    {
                        string[] tmp = fNR.Split(':');
                        stor.fReturn = tmp[0];
                        stor.fName = tmp[1];
                    }
                    else stor.fName = line.Split('(')[0];
                    stor.fParams = line.Split('(')[1].Split(',');
                    break;

                case CodeType.Definition:
                    //string[] sa = { "#define" };
                    /*
                     * Example: #define LOL \
                     *          Hello
                     */
                    if (line.EndsWith("\\")) ContinueDef = true;
                    else if (ContinueDef == true) ContinueDef = false;
                    stor.fName = line.Split()[1];
                    stor.fParams = new string[1];
                    if (line.Split().Length < 3) stor.fParams[0] = "true";
                    else stor.fParams[0] = line.Split()[2];
                    stor.fReturn = "define";
                    break;

                case CodeType.Variable:
                    //line = line.Split("new ", StringSplitOptions.None)[0];

                    line = line.TrimEnd(';');
                    if (line.IndexOf(':') != -1)
                    {
                        stor.fReturn = line.Split(':')[0];
                        stor.fName = line.Split(':')[1];
                    }
                    else
                        stor.fName = line.Split()[0];

                    if (stor.fName.IndexOf('[') != -1)
                        stor.fName = stor.fName.Split('[')[0];

                    stor.fParams = new string[1];
                    if (line.IndexOf('=') != -1)
                    {
                        stor.fParams[0] = line.Split('=')[1].Trim('\'', '\"');
                    }
                    else stor.fParams[0] = null;
                    break;
            }
            return stor;
        }

        /// <summary>
        /// Check if the line has something we can use :)
        /// </summary>
        /// <param name="line">The line, obviously!</param>
        /// <returns>The type in a CodeType, lol</returns>
        public CodeType CheckLineForTypes(string line, string fd, bool include)
        {
            if (ContinueDef == true || line.StartsWith("#define") == true) return CodeType.Definition;
            if (line.StartsWith("new") == true) return CodeType.Variable;
            if (fd == "pawno" & include == true) return line.IndexOf("native") != -1 ? CodeType.Function : CodeType.None;
            if (line.IndexOf("(") != -1 && line.IndexOf(")") != -1 && line.IndexOf("return") == -1) return CodeType.Function;
            return CodeType.None;
        }

        /// <summary>
        /// This will remove any comments from the code :)
        /// </summary>
        /// <param name="line">Line with suspected comment mastermind.</param>
        /// <returns>A code from rehab.</returns>
        public static string buangKomen(string line)
        {
            string temp;
            if (dlmKomen == true)
            {
                if (line.IndexOf("*/") != -1)
                {
                    string[] sem = { "*/" };
                    dlmKomen = false;
                    if (line.Split(sem, StringSplitOptions.RemoveEmptyEntries).Length < 2 || line.Split(sem, StringSplitOptions.RemoveEmptyEntries)[1] == null) return "";
                    else temp = line.Split(sem, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                else return null;
            }
            else
            {
                if (line.IndexOf("/*") != -1)
                {
                    temp = line.Remove(line.IndexOf("/*"));
                    if (line.IndexOf("*/") != -1) dlmKomen = false;
                    else dlmKomen = true;
                }
                else temp = line;
            }
            temp = (temp.IndexOf("//") != -1) ? temp.Remove(temp.IndexOf("//")) : temp;
            return temp.TrimEnd();
        }

        /// <summary>
        /// COMING SOON! Calculate the depth and save to array
        /// </summary>
        /// <param name="line">The current line</param>
        public void CurrentCodeDepth(string line)
        {


        }

        /// <summary>
        /// Checks the code to see if it is first level
        /// ---
        /// TODO: Add seperate function for removing certain parts
        /// TODO: Remove parts :)
        /// ---
        /// </summary>
        /// <param name="line">The current line</param>
        /// <returns>First level code, if available</returns>
        public string BraceCheck(string line)
        {
            string rtnV = "";

            if (line == null || line == "") return null;
            if (line.Split('{').Length > 1)
            {
                if (unmatchedBrace == 0) rtnV = line.Split('{').First<String>().Trim();
                unmatchedBrace += (line.Split('{').Length - 1);
            }

            if (line.Split('}').Length > 1)
            {
                unmatchedBrace -= (line.Split('}').Length - 1);
                if (unmatchedBrace == 0 & rtnV == "") rtnV = line.Split('}').Last<String>();
            }

            if (unmatchedBrace == 0 & rtnV == "") rtnV = line;

            return rtnV.Trim();
        }
    }

    /// <summary>
    /// Stores a function, split into parts (for easy access)
    /// </summary>
    public class c_function
    {
        // FDV means Function/Define/Variable
        /// <summary>
        /// The name of the FDV
        /// </summary>
        public string fName;
        /// <summary>
        /// Name of the file the FDV exists on
        /// </summary>
        public string fFileName;
        /// <summary>
        /// The parameters/values the FDV has
        /// </summary>
        public string[] fParams;
        /// <summary>
        /// The return type the FDV is
        /// </summary>
        public string fReturn = "void";
        /// <summary>
        /// The line the FDV resides on (+1 for visual)
        /// </summary>
        public int fLn = 0;
    }

    /// <summary>
    /// Stores functions of a particular file.
    /// </summary>
    public class c_FileFnc
    {
        public string FileName;
        public Dictionary<string, c_function> functions = new Dictionary<string, c_function>();
        public Dictionary<string, c_function> constants = new Dictionary<string, c_function>();
        public Dictionary<string, c_function> variables = new Dictionary<string, c_function>();

        public c_FileFnc(string fn)
        {
            FileName = fn;
        }
    }
}
