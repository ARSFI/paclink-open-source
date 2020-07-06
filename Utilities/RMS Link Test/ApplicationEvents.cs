using Microsoft.VisualBasic;

namespace RMS_Link_Test.My
{

    // The following events are availble for MyApplication:
    // 
    // Startup: Raised when the application starts, before the startup form is created.
    // Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    // UnhandledException: Raised if the application encounters an unhandled exception.
    // StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    // NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    internal partial class MyApplication
    {
        public MyApplication()
        {
            this.UnhandledException += MyUnhandledException;
        }

        private void MyUnhandledException(object s, Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs e)
        {
            string strUnhandledException = Globals.TimestampEx() + " [" + Globals.strProductVersion + "] " + s.ToString() + ": " + Constants.vbCrLf + e.Exception.Message.Trim() + Constants.vbCrLf + e.Exception.StackTrace + Constants.vbCrLf + e.Exception.TargetSite.ToString() + Constants.vbCrLf;

            MyProject.Computer.FileSystem.WriteAllText(Globals.strExecutionDirectory + "RMS Link Test Unhandled Exceptions.log", strUnhandledException, true);
            Interaction.MsgBox(strUnhandledException, MsgBoxStyle.Critical);
        }
    }
}