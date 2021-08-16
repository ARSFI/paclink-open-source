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

        public static class Forms
        {
            public static Main Main;
        }

        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Forms.Main = new Main();
            UserInterfaceFactory.GetUiSystem().DisplayMainForm(Forms.Main);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string strUnhandledException = Globals.TimestampEx() + " [" + Globals.strProductVersion + "] " + sender.ToString() + ": " + Globals.CRLF + e.Exception.Message.Trim() + Globals.CRLF + e.Exception.StackTrace + Globals.CRLF + e.Exception.TargetSite.ToString() + Globals.CRLF;
            _log.Fatal(strUnhandledException);
            MessageBox.Show(strUnhandledException, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Environment.Exit(0);
        }

    }
}