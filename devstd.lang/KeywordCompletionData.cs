using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;

namespace devstd.lang
{
    class INTELanguage
    {
        public Dictionary<string, PASINTELIDATA> Keywords = new Dictionary<string, PASINTELIDATA>();
    }
    class AnyCodeCompletionProvider
    {

       
        public Dictionary<string, INTELanguage> INTE;
        INTELanguage current;
        public AnyCodeCompletionProvider()
        {
            try
            {
              


                INTE = new Dictionary<string, INTELanguage>();
                XmlDocument doc = new XmlDocument();
                doc.Load(Application.StartupPath + @"\Data\AnyIntellisense.xml");

                foreach (XmlElement el in doc.DocumentElement.ChildNodes)
                {
                    string[] keywords = el.GetElementsByTagName("keywords")[0].InnerText.Split(';');
                    string[] types = el.GetElementsByTagName("types")[0].InnerText.Split(';');

                    INTELanguage ln = new INTELanguage();
                    foreach (string key in keywords)
                    {
                        if (!ln.Keywords.ContainsKey(key))
                            ln.Keywords.Add(key, new PASINTELIDATA(el.GetAttribute("lname") + " keyword " + key, key, "pre-defined", 5));
                    }

                    foreach (string ty in types)
                    {
                        if (!ln.Keywords.ContainsKey(ty))
                            ln.Keywords.Add(ty, new PASINTELIDATA(el.GetAttribute("lname") + " type " + ty, ty, "pre-defined", 0));
                    }

                    if (el.GetElementsByTagName("modules").Count == 1)
                    {
                        foreach (XmlElement mel in el.GetElementsByTagName("modules")[0])
                        {
                            if (!ln.Keywords.ContainsKey(mel.InnerText))
                                ln.Keywords.Add(mel.InnerText, new PASINTELIDATA(mel.GetAttribute("desc"), mel.GetAttribute("target"), mel.GetAttribute("ns"), 1));
                        }
                    }
                    INTE.Add(el.GetAttribute("lname"), ln);
 
                }


            }
            catch (Exception ex)
            {
          
            }
            finally
            {

            }
        }


        public ImageSource[] ImageList
        {
            get;
            set;
        }


 
        public Dictionary<string, PASINTELIDATA> getlist(int type)
        {

            return current.Keywords;

        }
        void GetCurrent(string file)
        {
            string ext = Path.GetExtension(file);
            switch (ext)
            {
                case ".cs":
                    current = INTE["C#"];
                    break;


                case ".c":
                    current = INTE["C/C++"];
                    break;
                case ".cpp":
                    current = INTE["C/C++"];
                    break;
                case ".h":
                    current = INTE["C/C++"];
                    break;
                case ".hpp":
                    current = INTE["C/C++"];
                    break;
                case ".pwn":
                    current = INTE["C/C++"];
                    break;
                case ".cc":
                    current = INTE["C/C++"];
                    break;


                case ".vb":
                    current = INTE["VB.NET"];
                    break;
                case ".html":
                    current = INTE["HTML"];
                    break;
                case ".htm":
                    current = INTE["HTML"];
                    break;
                case ".js":
                    current = INTE["JS"];
                    break;
                case ".sql":
                    current = INTE["SQL"];
                    break;
                default:
                    current = INTE["C#"];
                    break;

            }
        }
        public MyCompletionData[] GenerateCompletionData(string fileName)
        {
            GetCurrent(fileName);
            Dictionary<string, PASINTELIDATA> p = getlist(1);
            List<MyCompletionData> resultList = new List<MyCompletionData>();
            foreach (KeyValuePair<string, PASINTELIDATA> ps in p)
            {
                MyCompletionData dat = new MyCompletionData(ps.Key);
                dat.Description = ps.Value.Description;
                dat.Image = ImageList[ps.Value.index];
                dat.TargetText = ps.Value.target;
                resultList.Add(dat);
                // resultList.Add(new MyCompletionData(ps.Key, ps.Value.Description, (int)ps.Value.index, ps.Value.target));


            }
            return resultList.ToArray();

        }

    }
}
