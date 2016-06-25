using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ALCodeDomProvider
{
   public class TempFileService
    {
        public string TempDirectory;
        public Dictionary<string, string> TempFilesIndexer = new Dictionary<string, string>();
        public string GetFileFromTemp(string temp)
        {
            if (TempFilesIndexer.ContainsKey(temp.ToLower()))
                return TempFilesIndexer[temp.ToLower()];
            else
                return null;
        }
        public string GetTempFileName(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            if (TempFilesIndexer.ContainsKey((TempDirectory + @"\" + sb.ToString() + ".al").ToLower()))
                TempFilesIndexer[(TempDirectory + @"\" + sb.ToString() + ".al").ToLower()] = input;
            else
                TempFilesIndexer.Add((TempDirectory + @"\" + sb.ToString() + ".al").ToLower(), input);

            return TempDirectory + @"\" + sb.ToString() + ".al";
        }
        public string GetTempFileNameWithoutIndex(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString() + ".al";
        }
    }
}
