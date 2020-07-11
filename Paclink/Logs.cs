using System;
using System.IO;
using System.Windows.Forms;
using Ionic.Zip;
using Microsoft.VisualBasic;
using SyslogLib;

namespace Paclink
{
    public class Logs
    {
        private static object objLogLock = new object();

        public static void ChannelEvent(string strText)
        {
        }

        public static void ChannelEvents(string strText)
        {
            // Writes the indicated text to the channel event log...

            lock (objLogLock)
                File.WriteAllText(Globals.SiteRootDirectory + @"Logs\Channel Events " + Strings.Format(DateTime.UtcNow, "yyyyMMdd") + ".log", Globals.TimestampEx() + " " + strText + Globals.CRLF);
        } // WriteChannelEvent

        public static void SMTPEvent(string strText)
        {
            // Writes the indicated text to the SMTP/POP3 event log...

            lock (objLogLock)
                File.WriteAllText(Globals.SiteRootDirectory + @"Logs\SMTP Events " + Strings.Format(DateTime.UtcNow, "yyyyMMdd") + ".log", Globals.TimestampEx() + " " + strText + Globals.CRLF);
        } // WriteSMTPEvent
        // 
        // Writes the indicated text to the exception log.
        // 
        private static string strLastException = "";
        public static void Exception(string strText, bool blnReportToLog = true)
        {
            lock (objLogLock)
            {
                File.WriteAllText(Globals.SiteRootDirectory + @"Logs\Exceptions " + Strings.Format((object)DateTime.UtcNow, "yyyyMMdd") + ".log", Globals.TimestampEx() + " [" + Application.ProductVersion + "] " + strText + Globals.CRLF);
                // Write to the central logging system
                if (blnReportToLog)
                {
                    if ((strText ?? "") != (strLastException ?? ""))
                    {
                        strLastException = strText;
                        Globals.QueueSysLog("Exception:" + strText, SyslogSeverity.Warning);
                    }
                }
            }
        } // Exception

        public static void Trace(string strText)
        {
            // Writes the indicated text to the trace log...
            lock (objLogLock)
                File.WriteAllText(Globals.SiteRootDirectory + @"Logs\Trace " + Strings.Format(DateTime.UtcNow, "yyyyMMdd") + ".log", Globals.TimestampEx() + " [" + Application.ProductVersion + "] " + strText + Globals.CRLF);
        } // Exception

        public static void WriteDebug(string strText)
        {
            // Writes the indicated text to the WINMORE log...
            lock (objLogLock)
            {
                try
                {
                    File.WriteAllText(Globals.SiteRootDirectory + @"Logs\WINMORdebug " + Strings.Format(DateTime.UtcNow, "yyyyMMdd") + ".log", Globals.TimestampEx() + " [" + Application.ProductVersion + "] " + strText + Globals.CRLF);
                }
                catch
                {
                }
            }
        } // WriteDebug

        public static void WriteAutoupdate(string strText)
        {
            // Writes the indicated text to the Autoupdate log...
            lock (objLogLock)
            {
                try
                {
                    File.WriteAllText(Globals.SiteRootDirectory + @"Logs\Autoupdate " + Strings.Format(DateTime.UtcNow, "yyyyMMdd") + ".log", Globals.TimestampEx() + " [" + Application.ProductVersion + "] " + strText + Globals.CRLF);
                }
                catch
                {
                }
            }
        } // WriteDebug

        public static void PurgeOldLogFiles()
        {
            // If the archives flag is not set removes log files that have not been written to within the last 7 days...

            if (Globals.blnArchiveLogs == false)
            {
                try
                {
                    var strFiles = Directory.GetFiles(Globals.SiteRootDirectory + "Logs");
                    foreach (string strFile in strFiles)
                    {
                        if (File.GetLastWriteTime(strFile) < DateTime.Now.AddDays(-7) & strFile.EndsWith(".log"))
                        {
                            File.Delete(strFile);
                        }
                    }
                }
                catch
                {
                    Exception("[Logs.PurgeOldLogFiles] " + Information.Err().Description);
                }
            }
            else
            {
                ArchiveOldLogFiles();
            }
        } // PurgeOldLogFiles

        public static void ArchiveOldLogFiles()
        {
            // If the archives flag has been set archives log files that have not been written to within the last 7 days...

            ZipFile objZip;
            try
            {
                if (File.Exists(Globals.SiteRootDirectory + @"Logs\Log Archive.zip"))
                {
                    // 
                    // Open existing log archive 
                    // 
                    try
                    {
                        objZip = ZipFile.Read(Globals.SiteRootDirectory + @"Logs\Log Archive.zip");
                    }
                    catch (Exception ex)
                    {
                        if (File.Exists(Globals.SiteRootDirectory + @"Logs\Log Archive_old.zip"))
                        {
                            File.SetAttributes(Globals.SiteRootDirectory + @"Logs\Log Archive_old.zip", FileAttributes.Normal);
                            File.Delete(Globals.SiteRootDirectory + @"Logs\Log Archive_old.zip");
                        }

                        File.Move(Globals.SiteRootDirectory + @"Logs\Log Archive.zip", Globals.SiteRootDirectory + @"Logs\Log Archive_old.zip");
                        objZip = new ZipFile(Globals.SiteRootDirectory + @"Logs\Log Archive.zip");
                    }
                }
                else
                {
                    // 
                    // Create new log archive 
                    // 
                    objZip = new ZipFile(Globals.SiteRootDirectory + @"Logs\Log Archive.zip");
                }

                var strFiles = Directory.GetFiles(Globals.SiteRootDirectory + "Logs");
                foreach (string strFile in strFiles)
                {
                    if (File.GetLastWriteTime(strFile) < DateTime.Now.AddDays(-7) & strFile.ToLower().EndsWith(".log"))
                    {
                        if (strFile.IndexOf("Trace") == -1)
                        {
                            objZip.AddFile(strFile);
                        }
                    }
                }

                objZip.Save();
                foreach (string strFile in strFiles)
                {
                    if (File.GetLastWriteTime(strFile) < DateTime.Now.AddDays(-7) & strFile.EndsWith(".log"))
                    {
                        File.Delete(strFile);
                    }
                }
            }
            catch
            {
                Exception("[Logs.ArchiveOldLogFiles] " + Information.Err().Description);
            }
        } // PurgeOldLogFiles
    } // ErrorLog
}