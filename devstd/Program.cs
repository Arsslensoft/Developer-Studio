using DevComponents.DotNetBar;
using devstd.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace devstd
{
    public static class Programs
    {
        public static event EventHandler CloseSplashScreen;
        public static void CallClosespl()
        {
            if (CloseSplashScreen != null)
                CloseSplashScreen(null, EventArgs.Empty);
        }
    }
    static public class SingleInstance
    {
        public static readonly int WM_SHOWFIRSTINSTANCE =
            WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
        static Mutex mutex;
        static public bool Start()
        {
            bool onlyInstance = false;
            string mutexName = String.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);

            mutex = new Mutex(true, mutexName, out onlyInstance);
            return onlyInstance;
        }
        static public void ShowFirstInstance()
        {
            WinApi.PostMessage(
                (IntPtr)WinApi.HWND_BROADCAST,
                WM_SHOWFIRSTINSTANCE,
                IntPtr.Zero,
                IntPtr.Zero);
        }
        static public void Stop()
        {
            mutex.ReleaseMutex();
        }
    }



    static public class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, SW_SHOWNORMAL);
            SetForegroundWindow(window);
        }
    }


    static public class ProgramInfo
    {
        static public string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
            }
        }
        static public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }
    }
    public delegate void Asynchronous();
    static class Program
    {
        internal static bool Initialized = false;
        static void NewS()
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }
        static void M()
        {
            Asynchronous thr = new Asynchronous(NewS);
            thr.BeginInvoke(null, null);
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!SingleInstance.Start())
            {
                File.WriteAllLines(Application.StartupPath + @"\ARGS.t", Environment.GetCommandLineArgs());
                SingleInstance.ShowFirstInstance();
                return;
            }
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Log.ExceptionArrived += Log_ExceptionArrived;
            try
            {
                try
                {


                    if (!File.Exists(Application.StartupPath + @"\FIRSTRUN.f"))
                    {
                        SoundPlayer sp = new SoundPlayer(Application.StartupPath + @"\install.wav");
                        sp.Load();
                        sp.Play();

                        SettingsForm sfrm = new SettingsForm();
                        sfrm.ShowDialog();
                        FileStream fs = File.Create(Application.StartupPath + @"\FIRSTRUN.f");
                        fs.Close();

                    }
                    M();
                    //if (File.Exists(Application.StartupPath + @"\DSWatcher.exe"))
                    //    Process.Start(Application.StartupPath + @"\DSWatcher.exe", Process.GetCurrentProcess().Id.ToString());

                    File.WriteAllText(Application.StartupPath + @"\Session_Watch.dat", "");
                    Application.Run(new MainForm());
                    File.Delete(Application.StartupPath + @"\Session_Watch.dat");



                }
                catch (Exception ex)
                {
                    Log.Error(ex, 2020);
                }
                finally
                {

                }
            }
            catch (Exception e)
            {
                Log.Error(e, 2020);
            }

            SingleInstance.Stop();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
         

        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Error(e.Exception, 2020);
        }
        static void Log_ExceptionArrived(Exception ex, int code)
        {

            try
            {
                Report frm = new Report();
                frm.err = ex;
                frm.Label1.Text = code.ToString();
                frm.Label2.Text = ex.Message;
                frm.ShowDialog();
            }
            catch
            {

                MessageBoxEx.Show("The Application will close now");
            }
            finally
            {

            }

        }
    }
}
