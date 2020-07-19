using System;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;

namespace Paclink
{

    // This class manages the persistent message IDs seen table. All methods are
    // 'Shared'. This class does not require instantiation...
    public class MidsSeen
    {
        private static object objMidsSeenLock = new object();

        public static void AddMessageId(string strMessageId)
        {
            // Adds a message ID to the message IDs seen table...

            lock (objMidsSeenLock)
            {
                try
                {
                    string strMessageIdString;
                    string strMessagesSeenPath = Globals.SiteRootDirectory + @"Data\Mids Seen.dat";
                    if (File.Exists(strMessagesSeenPath))
                    {
                        strMessageIdString = File.ReadAllText(strMessagesSeenPath);
                    }
                    else
                    {
                        strMessageIdString = "";
                    }

                    if (strMessageIdString.IndexOf(strMessageId) == -1)
                    {
                        string strMessageIdRecord = Globals.TimestampEx() + " " + strMessageId + Globals.CRLF;
                        File.WriteAllText(strMessagesSeenPath, strMessageIdRecord);
                    }
                }
                catch (Exception ex)
                {
                    Logs.Exception("[MidsSeen.AddMessageId] " + ex.Message);
                }
            }
        } // AddMessageId

        public static bool IsMessageIdSeen(string strMessageId)
        {
            // Returns true if a message ID has already been processed...

            int intResult = -1;
            lock (objMidsSeenLock)
            {
                try
                {
                    string strMessagesSeenPath = Globals.SiteRootDirectory + @"Data\Mids Seen.dat";
                    if (File.Exists(strMessagesSeenPath))
                    {
                        string strMessageIdString = File.ReadAllText(strMessagesSeenPath);
                        intResult = strMessageIdString.IndexOf(strMessageId);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Logs.Exception("[MidsSeen.IsMessageIdSeen] " + ex.Message);
                }
            }

            if (intResult == -1)
                return false;
            else
                return true;
        } // IsMessageIdSeen

        public static void PurgeMidsSeenFile()
        {
            // Purges the message IDs seen table if any entries over 30 days old...

            var sbdRecords = new StringBuilder();
            string strTimestamp = Globals.FormatDate(DateTime.UtcNow.AddDays(-30));
            string strMessageIdString;
            string strMessagesSeenPath = Globals.SiteRootDirectory + @"Data\Mids Seen.dat";
            if (File.Exists(strMessagesSeenPath))
            {
                lock (objMidsSeenLock)
                {
                    try
                    {
                        // Get the Mids Seen file content...
                        strMessageIdString = File.ReadAllText(strMessagesSeenPath);

                        // Parse into records and test each record for a timestamp...
                        strMessageIdString = strMessageIdString.Replace(Globals.CRLF, Globals.CR);
                        var strRecords = strMessageIdString.Split('\r');
                        foreach (string strRecord in strRecords)
                        {
                            var strTokens = strRecord.Split(' ');

                            // Save only records that are less than 30 days old...
                            if (strTokens[0].CompareTo(strTimestamp) > 0)
                            {
                                sbdRecords.Append(strRecord + Globals.CRLF);
                            }
                        }

                        // Rewrite the Mids Seen file...
                        File.WriteAllText(strMessagesSeenPath, sbdRecords.ToString());
                    }
                    catch (Exception ex)
                    {
                        Logs.Exception("[MidsSeen.PurgeMidsSeenFile] " + ex.Message);
                    }
                }
            }
        } // PurgeMidsSeenFile
    } // MidsSeen
}