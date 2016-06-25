using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using ICSharpCode.AvalonEdit.Editing;
using System.Windows.Media;

namespace devstd.lang
{
    internal class PawnCodeCompletionProvider
    {


        public Dictionary<string, PASINTELIDATA> Keywords;
        public Dictionary<string, PASINTELIDATA> Identifier;
        public Dictionary<string, PASINTELIDATA> NameSpaces;
        public static bool IsInType(string line)
        {
            int x = 0;
            int p = line.IndexOf("new ");
            if (p > 0)
                return !int.TryParse(line.Substring(0, p), out x);
            else return false;
        }
        public static bool IsInAssignement(string line, int pos)
        {

            if (pos > 2)
            {
                int i = pos - 1;
                while (i > 0)
                {
                    if (!char.IsWhiteSpace(line[i]))
                    {
                        if (line[i] == '=')
                            return true;
                        else
                            return false;
                    }
                    i--;
                }
                return false;
            }
            else
                return false;
        }
        public PawnCodeCompletionProvider()
        {

            try
            {
                Keywords = new Dictionary<string, PASINTELIDATA>();
                Identifier = new Dictionary<string, PASINTELIDATA>();
                NameSpaces = new Dictionary<string, PASINTELIDATA>();

                Identifier.Add("bool", new PASINTELIDATA("It is very simple - it is either \"true\", or \"false\". Both \"true\" and \"false\" are predefined data structures", "bool", "root", 0));
                Identifier.Add("float", new PASINTELIDATA("it can store numbers with decimal places. These are called \"floating point\" numbers", "float", "root", 0));

                if (File.Exists(Application.StartupPath + @"\Data\PawnKeywords.dat"))
                {
                    foreach (string tp in File.ReadAllLines(Application.StartupPath + @"\Data\PawnKeywords.dat"))
                    {
                        if (!Keywords.ContainsKey(tp))
                            Keywords.Add(tp, new PASINTELIDATA("Pawn language keyword", tp, "root", 5));
                    }
                }
                //}
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        public static string GetWordBeforePosition(string line)
        {
            string dline = line;
            while (dline.IndexOf(' ') == 0)
                dline = dline.Remove(0, 1);

            for (int i = 0; i < dline.Length; i++)
            {
                if (!char.IsPunctuation(dline[i]) && !char.IsWhiteSpace(dline[i]) && !char.IsLetterOrDigit(dline[i]))
                    dline = dline.Remove(i, 1);
                else
                {
                    if (i > 0)
                    {
                        if (char.IsWhiteSpace(dline[i]) && char.IsWhiteSpace(dline[i - 1]))
                            dline = dline.Remove(i, 1);
                    }
                }
            }
            string[] w = dline.Split(' ');

            return w[0];
        }
        public ImageSource[] CompletionImageList
        {
            get;
            set;
        }




        ///// <summary>
        ///// Called when entry should be inserted. Forward to the insertion action of the completion data.
        ///// </summary>
        //public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        //{
        //    textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);
        //    if ((PasIntellisenseData)data.ImageIndex == PasIntellisenseData.NameSpace)
        //        PPUDiscover.DetectSymbolsAsynchronous(data.Text, Keywords, Identifier);

        //    return data.InsertAction(textArea, key);
        //}
        public Dictionary<string, PASINTELIDATA> getlist(int type)
        {
            if (type == 0)
            {
                return Identifier;
            }
            else if (type == 1 || type == 3 || type == 4 || type == 5)
            {
                return Keywords;
            }
            else
            {
                return NameSpaces;
            }

        }
        public PasIntellisenseData gettype(int type)
        {
            if (type == 0)
            {
                return PasIntellisenseData.Identifier;
            }
            else if (type == 1)
            {
                return PasIntellisenseData.Method;
            }
            else if (type == 3)
            {
                return PasIntellisenseData.Variable;
            }
            else if (type == 4)
            {
                return PasIntellisenseData.Constant;
            }
            else if (type == 5)
            {
                return PasIntellisenseData.sKeyword;
            }
            else
            {
                return PasIntellisenseData.NameSpace;
            }


        }
        public static bool IsInString(string line, int pos)
        {
            bool e = false;
            int s = 0;

            int p = line.IndexOf('"');
            if (p != -1)
                s = p;
            while (p != -1 && !e)
            {
                if ((s != p) && p < pos)
                    s = -1;

                if (s == -1)
                    s = p;

                e = (s < pos) && (p > pos);
                line = line.Remove(p, 1);
                p = line.IndexOf('"');
            }
            return e;
        }

        public MyCompletionData[] GenerateCompletionData(int type)
        {
            Dictionary<string, PASINTELIDATA> p = getlist(type);
            List<MyCompletionData> resultList = new List<MyCompletionData>();
            foreach (KeyValuePair<string, PASINTELIDATA> ps in p)
            {
                MyCompletionData dat = new MyCompletionData(ps.Key);
                dat.Description = ps.Value.Description;
                dat.Image = CompletionImageList[ps.Value.index];
                dat.IsNameSpace = false;
                dat.TargetText = ps.Value.target;
                //if (type == 2)
                //{
                //    dat.IsNameSpace = true;
                //    dat.Prov = this;
                //}
        

                if (!(type == 2 || type > 5))
                 {
                    if ((PawnCodeCompletionProvider.Includes.Contains(ps.Value.NS) || ps.Value.NS == "root"))
                        dat.Description += " \nSource : " + ps.Value.NS;
                      

                }
                // resultList.Add(new MyCompletionData(ps.Key, ps.Value.Description, (int)ps.Value.index, ps.Value.target));
                resultList.Add(dat);

            }
            return resultList.ToArray();


        }
        internal static List<string> Includes = new List<string>();
    }
}
