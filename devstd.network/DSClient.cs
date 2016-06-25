using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace devstd.network
{
   public class DSClient
    {
        #region Splitters
        static Regex SPSplitter = new Regex(@"<SP>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex NLSplitter = new Regex(@"<NL>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex GIGDSplitter = new Regex(@"<DEVSTD_DATA>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public string Host = "http://client.developerstudio.tk";

        #endregion

        public string ErrorMSG;
        string Key;
        public ulong CreateBroadCast(string name, string desc, string user)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Host + "/PCVP.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "DEVSTD_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "username=" + user + "&name=" + name + "&desc=" + desc + "&CMD=CPCVP";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();
                   
                   string dat = GIGDSplitter.Split(responseFromServer.Replace("OK:", ""), 3)[1];
                   Key = SPSplitter.Split(dat, 2)[1];
                   return ulong.Parse(SPSplitter.Split(dat, 2)[0]);


                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(responseFromServer.Replace("DENIED:", ""), 3)[1];
                    return 0;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        public bool PushInstructions(ulong cbid, string data)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Host + "/PCVP.php");
                request.Method = "POST";
                request.Accept = "gzip, deflate";
                request.Proxy = null;
                request.Timeout = 15000;
                request.KeepAlive = true;
                request.UserAgent = "DEVSTD_CLIENT/UserAgent 1.0";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                string postData = "data=" + data + "&id=" + cbid.ToString() + "&CMD=PUSH&key="+Key;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse sresponse = request.GetResponse();
                dataStream = sresponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                if (responseFromServer.Contains("OK:"))
                {

                    reader.Close();
                    dataStream.Close();
                    sresponse.Close();

                    return true;



                }
                else if (responseFromServer.Contains("DENIED:"))
                {
                    ErrorMSG = GIGDSplitter.Split(responseFromServer.Replace("DENIED:", ""), 3)[1];
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        } 

    }
}
