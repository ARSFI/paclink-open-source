using System;
using System.IO;
using System.Windows.Forms;
using NLog;
using Paclink.UI.Common;

namespace Paclink
{
    // 
    // The following events are availble for MyApplication:
    // 
    // Startup: Raised when the application starts, before the startup form is created.
    // Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    // UnhandledException: Raised if the application encounters an unhandled exception.
    // StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    // NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    // 
    static class MyApplication
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UserInterfaceFactory.GetUiSystem().DisplayMainForm(new Paclink.Main());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string version = typeof(Main).Assembly.GetName().Version.ToString();
            string time = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");

            string strUnhandledException = 
                time + " [" + version + "] " + 
                sender.ToString() + ": " + "\r\n" + e.Exception.Message.Trim() + 
                "\r\n" + e.Exception.StackTrace + "\r\n" + 
                e.Exception.TargetSite.ToString() + "\r\n";

            _log.Fatal(strUnhandledException);
            MessageBox.Show(strUnhandledException, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Environment.Exit(0);
        }

    }
}