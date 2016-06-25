using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace JSS
{
    internal static class VirusTotal
    {
      internal static string APIKey;
      internal static string scan = "https://www.virustotal.com/vtapi/v2/file/scan";
      internal static string results = "https://www.virustotal.com/vtapi/v2/file/report";
         internal static void Initialize(string apiKey)
        {
            ServicePointManager.Expect100Continue = false;
            APIKey = apiKey;
            regd = new Regex(@":", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            c = new WebClient();
            rstr = new StringBuilder();

        }       
        internal static string Scan(string file)
        {
            var v = new NameValueCollection();
            v.Add("apikey", APIKey);
            var c = new WebClient() { QueryString = v };
            c.Headers.Add("Content-type", "binary/octet-stream");
           
            byte[] b = c.UploadFile(scan, "POST", file);
            var r = ParseJSON(Encoding.Default.GetString(b));
            if (r.ContainsKey("scan_id"))
            {
                return r["scan_id"];
            }
            throw new Exception(r["result"]);
        }
        static  Regex regd;
        static  WebClient c;
        static StringBuilder rstr;
        internal static string GetResults(string id)
        {
            c = new WebClient();
           
            var data = string.Format("resource={0}&key={1}", id, APIKey);
         string s = c.UploadString(results, "POST", data);              
              var r = ParseJSON(s);
              foreach (string str in r.Values)
              {
                if (Regex.Match(str, @"[A-Z]", RegexOptions.IgnoreCase).Success)
                      rstr.Append(str + "|");
                 
                
              }
              return rstr.ToString();
        }
   

        internal static Dictionary<string, string> ParseJSON(string json)
        {
         
            var d = new Dictionary<string, string>();
            json = json.Replace("\"", null).Replace("[", null).Replace("]", null);
            var r = json.Substring(1, json.Length - 2).Split(',');
            foreach (string s in r)
            {
                if (s.Contains(":"))
                {
                    string[] t = regd.Split(s, 2);
                    if(!d.ContainsKey(t[0]))
                    d.Add(t[0], t[1]);
                }
            }
            return d;
        }
    }
    public static class VT
    {
        static Regex MatchRegex;

        public static void Init()
        {
            string beforetd = "<td class=\"ltr text-red\">";
       
            MatchRegex = new Regex(beforetd + @"\s*(.+?)\s*</td>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
        }
        public static bool Check(string hash, out string Virusname)
        {
            WebClient w = new WebClient();
           
            w.Headers["Accept-Encoding"] = "gzip";
            string url = "https://www.virustotal.com/file/" + hash.Split('-')[0].Replace("\r\n", "").Replace(" ", "") + "/analysis/";
            string Html = w.DownloadString(url);
            if (Html.Contains("<tbody>"))
            {

                Match m = MatchRegex.Match(Html);
                if (m.Success)
                {
                    Virusname = m.Groups[1].Value;
                    return true;
                }
            }


            Virusname = null;
            return false;
        }

    }

}
