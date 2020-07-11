using System;
using System.IO;
using System.Windows.Forms;

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
            Application.Run(Forms.Main);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string strUnhandledException = Globals.TimestampEx() + " [" + Globals.strProductVersion + "] " + sender.ToString() + ": " + Globals.CRLF + e.Exception.Message.Trim() + Globals.CRLF + e.Exception.StackTrace + Globals.CRLF + e.Exception.TargetSite.ToString() + Globals.CRLF;

            File.WriteAllText(Globals.SiteRootDirectory + @"Logs\Unhandled Exceptions.log", strUnhandledException);
            MessageBox.Show(strUnhandledException, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Environment.Exit(0);
            ReportUnhandledException(sender, e);
        }

        private static void ReportUnhandledException(object s, System.Threading.ThreadExceptionEventArgs e)
        {
            // 
            // Write a message to the central log for an unhandled exception.
            // 
            try
            {
                // 
                // Remove CrLf and noise words from the stack trace.
                // 
                string strTrace = e.Exception.StackTrace.Replace(Globals.CRLF, "$").Replace("   at ", " ");
                // 
                // Remove arguments in parentheses.
                // 
                for (int intCount = 1; intCount <= 100; intCount++)
                {
                    int intStart = strTrace.IndexOf('(');
                    if (intStart < 0)
                        break;
                    int intEnd = strTrace.IndexOf(')', intStart);
                    if (intEnd < 0)
                        break;
                    strTrace = strTrace.Substring(0, intStart) + strTrace.Substring(intEnd + 1);
                }

                strTrace = strTrace.Replace("  ", " ").Trim();
                // 
                // Find start of routines in Paclink.
                // 
                int intRelay = strTrace.IndexOf("Paclink.");
                if (intRelay > 0)
                {
                    strTrace = strTrace.Substring(intRelay);
                }

                strTrace = strTrace.Replace("Paclink.", "Pac.").Replace("System.Windows.Forms.", "SWF.");
                // 
                // Write the information to the logging system.
                // 
                if (strTrace.Length > 200)
                    strTrace = strTrace.Substring(0, 200);
                Globals.WriteSysLog("Unhandled_Exception: " + strTrace, SyslogLib.SyslogSeverity.Error);
                Globals.CloseSysLog(5);
            }
            catch
            {
            }
            // 
            // Finished
            // 
            return;
        }
    }
}