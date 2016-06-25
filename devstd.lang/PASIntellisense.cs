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
    [Flags]
    public enum PasIntellisenseData : int
    {
        Identifier = 0,
        Method = 1,
        NameSpace = 2,
        Variable = 3,
        Constant = 4,
        sKeyword = 5
    }
    public enum CompletionDataProviderKeyResult
    {
        NormalKey,
        BeforeStartKey,
        InsertionKey
    }
    public struct PASINTELIDATA
    {
        public string target;
        public string Description;
        public string NS;
        public byte index;
        public PASINTELIDATA(string desc, string t, string ns, byte ind)
        {
            index = ind;
            NS = ns;
            target = t;
            Description = desc;
        }
    }
    internal class PasCodeCompletionProvider
    {

     
        public Dictionary<string, PASINTELIDATA> Keywords;
        public Dictionary<string, PASINTELIDATA> Identifier;
        public Dictionary<string, PASINTELIDATA> NameSpaces;
        public static bool IsInType(string line)
        {
            int x = 0;
            int p = line.IndexOf(":");
            if (p > 0)
              return  !int.TryParse(line.Substring(0, p), out x);
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
        public PasCodeCompletionProvider()
        {

            try
            {
                CompletionImageList = null;
     
                Keywords = new Dictionary<string, PASINTELIDATA>();
                Identifier = new Dictionary<string, PASINTELIDATA>();
                NameSpaces = new Dictionary<string, PASINTELIDATA>();

                XmlDocument doc = new XmlDocument();
                doc.Load(Application.StartupPath + @"\Data\PIntellisense.xml");

                foreach (XmlElement el in doc.DocumentElement.ChildNodes)
                {
                    if (el.GetAttribute("type") == "id")
                    {
                        Identifier.Add(el.InnerText, new PASINTELIDATA(el.GetAttribute("desc"), el.GetAttribute("target"), "predefined", 0));
                    }
                    else if (el.GetAttribute("type") == "method")
                    {
                        string ns = "predefined";
                        if (el.HasAttribute("ns"))
                            ns = el.GetAttribute("ns");
                        Keywords.Add(el.InnerText, new PASINTELIDATA(el.GetAttribute("desc"), el.GetAttribute("target"), ns, 1));
                    }
                    else if (el.GetAttribute("type") == "field")
                    {
                        string ns = "predefined";
                        if (el.HasAttribute("ns"))
                            ns = el.GetAttribute("ns");
                        Keywords.Add(el.InnerText, new PASINTELIDATA(el.GetAttribute("desc"), el.GetAttribute("target"), ns, 3));
                    }
                    else
                    {
                        string ns = "predefined";
                        if (el.HasAttribute("ns"))
                            ns = el.GetAttribute("ns");
                        Keywords.Add(el.InnerText, new PASINTELIDATA(el.GetAttribute("desc"), el.GetAttribute("target"), ns, 5));
                    }
                }


        //        NameSpaces.Add("crt", new PASINTELIDATA("test crt", "crt", "crt", 2));
                foreach (string file in Directory.GetFiles(Application.StartupPath + @"\Pascal\units\i386-win32", "*.o", SearchOption.AllDirectories))
                    NameSpaces.Add(Path.GetFileNameWithoutExtension(file), new PASINTELIDATA("Represents the " + Path.GetFileNameWithoutExtension(file) + " namespace (i386)", Path.GetFileNameWithoutExtension(file), Path.GetFileNameWithoutExtension(file), 2));

                //}
            }
            catch (Exception ex)
            {
        
            }
            finally
            {

            }
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
                        if (type == 2)
                        {
                            dat.IsNameSpace = true;
                            dat.Prov = this;
                        }
                        resultList.Add(dat);
                       // resultList.Add(new MyCompletionData(ps.Key, ps.Value.Description, (int)ps.Value.index, ps.Value.target));
                     
                
            }
            return resultList.ToArray();

        
        }

    }
}
