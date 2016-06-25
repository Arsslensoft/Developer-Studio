using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace devstd
{

   public static class ELog
    {
       public static void LogEx(Exception ex)
       {
           try
           {
               using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\ELog.txt", true))
               {
                   str.WriteLine(ex.Message);
                   str.WriteLine(ex.StackTrace);
                   str.WriteLine(ex.Source);
                   str.WriteLine("-----------------------------------------");
               }
           }
           catch
           {

           }
       }
    }
}
