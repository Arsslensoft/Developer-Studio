using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace PawnParser
{
    public enum ErrorType
    {
        Error,
        Warning
    };
    /// <summary>
    /// Errors Parsing. Parses errors for suggestions.
    /// </summary>
    public class ErrorParser
    {
        /// <summary>
        /// Parses the usual PAWN error into yummy bits.
        /// </summary>
        /// <param name="err">The ususal PAWN error</param>
        public static Error ParseCompilerError(string err)
        {
            Error pce = new Error();
            string[] p_elems = err.Split(':');
            p_elems[1] = p_elems[0] + ":" + p_elems[1];
            pce.Line = Int32.Parse(p_elems[1].Split('(').Last<String>().Split(')').First<String>().Split().First<string>());
            pce.ID = Int32.Parse(p_elems[2].Trim().Split(' ').Last<String>());
            for (int i = 3; i < p_elems.Length; i++) pce.Description += p_elems[i] + ":";
            pce.Description = pce.Description.TrimEnd(':');
            pce.Type = p_elems[2].Trim().Split(' ')[0] == "warning" ? ErrorType.Warning : ErrorType.Error;
            string filename = p_elems[1].Remove(p_elems[1].Length - p_elems[1].Split('(').Last<String>().Length - 1); // No purpose to serve?
            return pce;
        }

        /// <summary>
        /// Gives help for errors.
        /// ---
        /// TODO: Create BASIC error parsing [PROGRESS]
        /// TODO: Add online error parsing. [PLANNING]
        /// </summary>
        /// <param name="errID">The error ID.</param>
        /// <param name="err">The error itself!</param>
        /// <returns></returns>
        public static string GetErrorHelp(Error err)
        {
            string errVar, errRes;
            switch (err.ID)
            {
                case 100:
                    errVar = err.Description.Split(':').Last<string>();
                    errVar = errVar.Trim();
                    errRes = "Add " + errVar + " into the 'include' folder.\r\n";
                    break;
                    /*
                case 235:
                    errStr = new string
                     // */

                default:
                    return null;
            }

            return errRes;
        }
    }

    /// <summary>
    /// Simple Errors Store
    /// </summary>
    public class Error
    {
        public int ID;
        public string Description;
        public int Line;
        public ErrorType Type;

        public Error() { }
        public Error(int errid, string desc, int line, ErrorType errtype)
        {
            ID = errid;
            Description = desc;
            Line = line;
            Type = errtype;
        }
    }
}
