using System;
using Microsoft.VisualBasic;

namespace Paclink.My
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
    internal partial class MyApplication
    {
        private void MyUnhandledException(object s, Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs e)
        {
            string strUnhandledException = Globals.TimestampEx() + " [" + Globals.strProductVersion + "] " + s.ToString() + ": " + Globals.CRLF + e.Exception.Message + Globals.CRLF + e.Exception.StackTrace + Globals.CRLF + e.Exception.TargetSite.ToString() + Globals.CRLF + Globals.CRLF;

            MyProject.Computer.FileSystem.WriteAllText(Globals.SiteRootDirectory + @"Logs\Unhandled Exceptions.log", strUnhandledException, true);
            Interaction.MsgBox(strUnhandledException, MsgBoxStyle.Critical, "Unhandled Exception Error");
            Environment.Exit(0);
            ReportUnhandledException(s, e);
        }

        private void ReportUnhandledException(object s, Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs e)
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