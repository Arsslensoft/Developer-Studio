using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace devstd.utils
{
    public delegate void ExceptionArrivedHandler(Exception ex, int code);
    public static class Log
    {
        public static event ExceptionArrivedHandler ExceptionArrived;
        public static void ShowInfo(string message, string title)
        {
            MessageBoxEx.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        public static void ShowAlert(string message, string title)
        {
            MessageBoxEx.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        public static void Error(Exception ex,int code)
        {
            try
            {
                using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\Error.txt"))
                {
                    str.WriteLine("-----------------------------------------");
                    str.WriteLine(ex.Message);
                    str.WriteLine(ex.Source);
                    str.WriteLine(ex.StackTrace);
                    str.WriteLine("  ");
                }
                if (ExceptionArrived != null)
                    ExceptionArrived(ex, code);
            }
            catch
            {

            }

        }

        public static void Error(Exception ex)
        {
            try
            {
                using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\ErrorLogs.txt"))
                {
                    str.WriteLine("-----------------------------------------");
                    str.WriteLine(ex.Message);
                    str.WriteLine(ex.Source);
                    str.WriteLine(ex.StackTrace);
                    str.WriteLine("  ");
                }
            }
            catch
            {

            }
        }

    }
}
