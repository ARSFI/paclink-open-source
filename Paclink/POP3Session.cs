using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using NLog;
using Paclink.Data;

namespace Paclink
{
    public class POP3Session
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public delegate void DisconnectHandler(POP3Session conn);
        public event DisconnectHandler OnDisconnect;

        // Declaration of SessionState of the POP3 Server...
        private enum SessionState
        {
            Authorization,
            Transaction,
            Update
        }

        // Initialize the basic POP3Server Reply String Constants
        private const string ReplyOK = "+OK " + Globals.CRLF;
        private const string ReplyERR = "-ERR " + Globals.CRLF;
        public Socket connection;
        public string AuthorizedUser;
        public DateTime Timestamp;
        private POP3Port objPOP3Port;
        private DateTime dttSessionStart;
        private System.Timers.Timer _tmrSessionTimer;

        private System.Timers.Timer tmrSessionTimer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrSessionTimer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _tmrSessionTimer = value;
            }
        }

        private bool[] blnDeleteFlags;
        private SessionState POP3State;
        private string strCommand;
        private string strCommandBuffer;
        private string strParameter;
        private bool authorized;
        private string strAccountName;
        private string strPassword;
        private int intNumberOfMimeFiles;
        private int intTotalBytes;

        public POP3Session(Socket sock)
        {
            Timestamp = DateTime.Now;
            connection = sock;
            dttSessionStart = DateTime.Now.AddMinutes(5);
            tmrSessionTimer = new System.Timers.Timer();
            tmrSessionTimer.Elapsed += OnSessionTimer;
            tmrSessionTimer.Enabled = false;
            tmrSessionTimer.Interval = 60000;
            tmrSessionTimer.AutoReset = true;
            tmrSessionTimer.Start();

            // Begin session
            DataIn("NewConnection");
        } // New

        public void Close()
        {
            tmrSessionTimer.Stop();
            tmrSessionTimer.Dispose();
            tmrSessionTimer = null;

            connection.Close();
            OnDisconnect?.Invoke(this);
        } // Close

        public void DataIn(string strText)
        {
            string strResponse = Protocol(strText);
            if (!string.IsNullOrEmpty(strResponse))
                connection.Send(UTF8Encoding.UTF8.GetBytes(strResponse));

            if (connection.Connected)
            {
                byte[] receiveBuffer = new byte[1024];
                connection.ReceiveAsync(new ArraySegment<byte>(receiveBuffer, 0, 1024), SocketFlags.None).ContinueWith(t =>
                {
                    if (t.Result > 0)
                    {
                        byte[] dataIn = new byte[t.Result];
                        Array.Copy(receiveBuffer, dataIn, t.Result);
                        DataIn(UTF8Encoding.UTF8.GetString(dataIn));
                    }
                    else
                    {
                        Close();
                    }
                }).Wait(0);
            }
        } // DataIn

        private void OnSessionTimer(object s, ElapsedEventArgs e)
        {
            if (dttSessionStart < DateTime.Now)
            {
                try
                {
                    Close();
                }
                catch
                {
                }
            }
        } // OnSessionTimer

        private string Protocol(string strInputStream)
        {
            // This function processes the incoming data stream for the POP3Session object.
            // If the string contains a command then the command is processed to update 
            // the POP3State and generate a reply string. An empty string value is 
            // returned to signal that no response is required to the partial command 
            // or data string. Data and command processing is a function of current 
            // POP3State...

            if (strInputStream == "NewConnection")
            {
                // The POP3Session has just been initialized - acknowledge with an OK Reply... 
                strCommandBuffer = ""; // Clear the command buffer
                POP3State = SessionState.Authorization;
                return ReplyOK;
            }

            if ((strInputStream.Right(2) ?? "") == Globals.CRLF)
            {
                // This is for a complete command or the CRLF to complete the command...
                strInputStream = strCommandBuffer + strInputStream;
                strCommand = strInputStream.Left(4).ToUpper().Trim();
                strCommandBuffer = "";
                strParameter = strInputStream.Substring(4).Trim();
            }
            else
            {
                // This accumulates a command which bridges multiple data in events...
                strCommandBuffer = strCommandBuffer + strInputStream;
                return "";
            }  // Empty reply string to signal no reply needed

            // Process the command
            var switchExpr = POP3State;
            switch (switchExpr)
            {
                case SessionState.Authorization: // The inital state instantiated state...
                    {
                        var switchExpr1 = strCommand;
                        switch (switchExpr1)
                        {
                            case "USER": // Accept the user name to later verify... 
                                {
                                    strAccountName = strParameter.ToUpper();
                                    return ReplyOK;
                                }

                            case "PASS": // Accept the password to verify account...
                                {
                                    strPassword = strParameter.ToUpper();
                                    authorized = Authorize(strAccountName, strPassword);
                                    if (authorized)
                                    {
                                        AuthorizedUser = strAccountName;
                                        POP3State = SessionState.Transaction;
                                        Globals.queSMTPDisplay.Enqueue("GPOP3 link from " + strAccountName + " at " + Globals.TimestampEx());
                                        return ReplyOK;
                                    }
                                    else
                                    {
                                        return "501 Authentication failed" + Globals.CRLF;
                                    }

                                    break;
                                }

                            case "QUIT":
                                {
                                    return ReplyOK;
                                }

                            default:
                                {
                                    return ReplyERR;
                                }
                        }

                        break;
                    }

                case SessionState.Transaction: // The state after a successful authorization...
                    {
                        var switchExpr2 = strCommand;
                        switch (switchExpr2)
                        {
                            case "STAT":
                                {
                                    return StatResponse();
                                }

                            case "LIST":
                                {
                                    return ListResponse(strParameter);
                                }

                            case "RETR":
                                {
                                    return RetrieveMessage(strParameter);
                                }

                            case "DELE":
                                {
                                    return DeleteMessage(strParameter);
                                }

                            case "NOOP":
                                {
                                    return ReplyOK;
                                }

                            case "RSET":
                                {
                                    return ResetMessages();
                                }

                            case "UIDL":
                                {
                                    return UidlResponse(strParameter);
                                }

                            case "QUIT":
                                {
                                    POP3State = SessionState.Update;
                                    return DeleteMimeFiles();
                                }

                            case "TOP":
                                {
                                    return TopResponse(strParameter);
                                }

                            default:
                                {
                                    return ReplyERR;
                                }
                        }

                        break;
                    }

                case SessionState.Update:
                    {
                        return "";
                    }

                default:
                    {
                        return ReplyERR;
                    }
            }
        } // Protocol

        private void GetPendingMimeList()
        {
            var messageStore = new MessageStore(DatabaseFactory.Get());
            int intCount = 0;
            int intByteCount = 0;
            blnDeleteFlags = null;
            var pendingEmails = messageStore.GetAccountEmails(strAccountName);
            foreach (var email in pendingEmails)
            {
                Array.Resize(ref blnDeleteFlags, intCount + 1);
                blnDeleteFlags[intCount] = false;
                intCount += 1;
                intByteCount += Convert.ToInt32(email.Value.Length);
            }

            intNumberOfMimeFiles = intCount;
            intTotalBytes = intByteCount;
        } // GetPendingMimeList

        private string StatResponse()
        {
            string StatResponseRet = default;
            // Provides a string for the summary to the STAT command...

            GetPendingMimeList();
            StatResponseRet = "+OK " + intNumberOfMimeFiles.ToString() + " " + intTotalBytes.ToString() + Globals.CRLF;
            return StatResponseRet;
        } // StatResponse

        private string ListResponse(string strMessageId)
        {
            // Returns the size of each file in the MIME array (aryMime)...
            GetPendingMimeList();
            string strResponse;
            int intCount = 0;
            if (string.IsNullOrEmpty(strMessageId))  // List everything as a multi line response...
            {
                strResponse = "+OK " + intNumberOfMimeFiles.ToString() + " " + intTotalBytes.ToString() + Globals.CRLF;
                if (intNumberOfMimeFiles > 0)
                {
                    var messageStore = new MessageStore(DatabaseFactory.Get());
                    var pendingEmails = messageStore.GetAccountEmails(strAccountName);
                    foreach (var email in pendingEmails)
                    {
                        intCount = intCount + 1;
                        strResponse = strResponse + intCount.ToString() + " " + email.Value.Length.ToString() + Globals.CRLF;
                    }
                }

                strResponse = strResponse + "." + Globals.CRLF;  // Add the terminator
            }
            else  // Just list the requested message as a one line response...
            {
                int intIndex;
                intIndex = Convert.ToInt32(strMessageId);
                if (intNumberOfMimeFiles > 0)
                {
                    var messageStore = new MessageStore(DatabaseFactory.Get());                
                    if (intIndex > 0 & intIndex <= intNumberOfMimeFiles)
                    {
                        var email = messageStore.GetAccountEmails(strAccountName)[intIndex - 1].Value;
                        strResponse = "+OK " + intIndex.ToString() + " " + email.Length.ToString() + Globals.CRLF;
                    }
                    else
                    {
                        strResponse = "-ERR no such message" + Globals.CRLF;
                    }
                }
                else
                {
                    strResponse = "-ERR no such message" + Globals.CRLF;
                }
            }

            return strResponse;
        } // ListResponse

        private string UidlResponse(string strMessageId)
        {
            string UidlResponseRet = default;
            // Returns a unique code for each file in the MIME array (aryMime).
            // This uses the unique aryMime name (without extension). If 
            // strMessageId = "" returns all messages otherwise the message 
            // requested parameter...

            GetPendingMimeList();
            int intCount = 0;
            int intLastSlashPosition;
            int intIndex;
            string strId;
            if (string.IsNullOrEmpty(strMessageId)) // Request for all messages and send a multi line response...
            {
                UidlResponseRet = "+OK " + Globals.CRLF;
                if (intNumberOfMimeFiles != 0)
                {
                    var messageStore = new MessageStore(DatabaseFactory.Get());
                    var emails = messageStore.GetAccountEmails(strAccountName);
                    foreach (var email in emails)
                    {
                        intCount = intCount + 1;
                        UidlResponseRet = UidlResponseRet + intCount.ToString() + " " + email.Key + Globals.CRLF;
                    }

                    UidlResponseRet = UidlResponseRet + "." + Globals.CRLF; // add terminator
                }
                else
                {
                    UidlResponseRet = "+OK" + Globals.CRLF + "." + Globals.CRLF;
                }
            }
            else // Request for specific message just send a single line response...
            {
                intIndex = Convert.ToInt32(strMessageId);
               
                if (intIndex > 0 & intIndex <= intNumberOfMimeFiles)
                {
                    var messageStore = new MessageStore(DatabaseFactory.Get());
                    var email = messageStore.GetAccountEmails(strAccountName)[intIndex];
                    UidlResponseRet = "+OK " + intIndex.ToString() + " " + email.Key + Globals.CRLF;
                }
                else
                {
                    UidlResponseRet = "-ERR" + Globals.CRLF;
                }
            }

            return UidlResponseRet;
        } // UidlResponse

        private string DeleteMessage(string sParam)
        {
            string DeleteMessageRet = default;
            // Function to mark a specific message for deletion. Does not 
            // actually delete the message until the Update state...

            int intIndex;
            if (!string.IsNullOrEmpty(sParam))
            {
                intIndex = Convert.ToInt32(sParam);
                if (intIndex > 0 & intIndex <= blnDeleteFlags.Length)
                {
                    if (blnDeleteFlags[intIndex - 1] == false) // Only mark if not yet marked for deletion
                    {
                        blnDeleteFlags[intIndex - 1] = true;
                        DeleteMessageRet = "+OK message " + intIndex.ToString() + " marked for deletion" + Globals.CRLF;
                        return DeleteMessageRet;
                    }
                }
            }

            DeleteMessageRet = "-ERR no such message" + Globals.CRLF;
            return DeleteMessageRet;
        } // DeleteMessage

        private string ResetMessages()
        {
            // Reset all delete flags back to undelete state...

            int intIndex;
            var loopTo = blnDeleteFlags.Length;
            for (intIndex = 1; intIndex <= loopTo; intIndex++)
                blnDeleteFlags[intIndex - 1] = false;
            return "+OK maildrop has " + (blnDeleteFlags.Length).ToString() + " messages" + Globals.CRLF;
        } // ResetMessages

        public string DeleteMimeFiles()
        {
            string DeleteMimeFilesRet = default;
            // Delete all the MIME files marked for deletion in array blnDeleteFlags.
            // Only called after QUIT received in the Transaction state...

            int intIndex;
            if (intNumberOfMimeFiles == 0)
            {
                DeleteMimeFilesRet = "+OK" + Globals.CRLF;
                return DeleteMimeFilesRet;
            }

            if (intNumberOfMimeFiles > 0)
            {
                var messageStore = new MessageStore(DatabaseFactory.Get());
                var emails = messageStore.GetAccountEmails(strAccountName);
                var loopTo = intNumberOfMimeFiles;
                for (intIndex = 1; intIndex <= loopTo; intIndex++)
                {
                    if (blnDeleteFlags[intIndex - 1] == true)
                    {
                        messageStore.DeleteAccountEmail(strAccountName, emails[intIndex - 1].Key);
                    }
                }
            }

            DeleteMimeFilesRet = "+OK" + Globals.CRLF;
            return DeleteMimeFilesRet;
        } // DeleteMimeFiles

        public string RetrieveMessage(string strMessageNumber)
        {
            string RetrieveMessageRet = default;
            // Retrieve a specific message identified in strMessageId...

            string strMessage;
            StreamReader srdMime;
            int intIndex;
            if (!string.IsNullOrEmpty(strMessageNumber))
            {
                intIndex = Convert.ToInt32(strMessageNumber);
                if (intNumberOfMimeFiles == 0)
                {
                    RetrieveMessageRet = "-ERR no messages" + Globals.CRLF;
                    return RetrieveMessageRet;
                }

                if (intNumberOfMimeFiles > 0)
                {
                    var messageStore = new MessageStore(DatabaseFactory.Get());
                    var emails = messageStore.GetAccountEmails(strAccountName);
                    if (intIndex > 0 & intIndex <= blnDeleteFlags.Length)
                    {
                        if (blnDeleteFlags[intIndex - 1] == false) // Only retrieve messages NOT marked for deletion
                        {
                            strMessage = UTF8Encoding.UTF8.GetString(emails[intIndex - 1].Value);

                            // Decode header for display and logging.
                            var strSubject = default(string);
                            var strFrom = default(string);
                            var strHeaderLines = strMessage.Split(Convert.ToChar(Globals.CR));
                            foreach (string strLine in strHeaderLines)
                            {
                                var tmpStrLine = strLine.Replace(Globals.LF, "").Trim();
                                if (tmpStrLine.StartsWith("From:"))
                                {
                                    strFrom = tmpStrLine;
                                }
                                else if (tmpStrLine.StartsWith("Subject:"))
                                {
                                    strSubject = tmpStrLine;
                                    break;
                                }
                            }

                            // Add byte stuffing for <CrLf . >
                            strMessage = strMessage.Replace(Globals.CRLF + ".", Globals.CRLF + "..");
                            string strMessageId = emails[intIndex - 1].Key;
                            Globals.queSMTPDisplay.Enqueue("G" + strMessageId + " delivered to " + strAccountName);
                            Globals.queSMTPDisplay.Enqueue("G   " + strFrom);
                            Globals.queSMTPDisplay.Enqueue("G   " + strSubject);
                            RetrieveMessageRet = "+OK " + Globals.CRLF + strMessage + Globals.CRLF + "." + Globals.CRLF;
                            return RetrieveMessageRet;
                        }
                    }
                }
            }

            RetrieveMessageRet = "-ERR no such message" + Globals.CRLF;
            return RetrieveMessageRet;
        } // RetrieveMessage

        public string TopResponse(string strMessageId)
        {
            string TopResponseRet = default;
            // Returns the Header and top n lines of a specific message identified in strMessageId...

            string strMessage;
            StreamReader strMime;
            int intIndex;
            try
            {
                if (!string.IsNullOrEmpty(strMessageId))
                {
                    var Param = strMessageId.Split(' ');
                    intIndex = Convert.ToInt32(Param[0]);
                    int intLineCounter = 0;
                    if (Param.Length > 1)
                        intLineCounter = Convert.ToInt32(Param[1]);
                    if (intNumberOfMimeFiles == 0)
                    {
                        TopResponseRet = "-ERR no messages" + Globals.CRLF;
                        return TopResponseRet;
                    }

                    if (intIndex > 0 & intIndex <= blnDeleteFlags.Length)
                    {
                        if (blnDeleteFlags[intIndex - 1] == false) // only retrieve messages NOT marked for deletion
                        {
                            var messageStore = new MessageStore(DatabaseFactory.Get());
                            var emails = messageStore.GetAccountEmails(strAccountName);

                            strMessage = UTF8Encoding.UTF8.GetString(emails[intIndex - 1].Value);
                            // Add byte stuffing for <CrLf . >
                            strMessage = strMessage.Replace(Globals.CRLF + ".", Globals.CRLF + "..");
                            int intEndOfHeader = 4 + strMessage.IndexOf(Globals.CRLF + Globals.CRLF);
                            string sHeader = strMessage.Substring(0, intEndOfHeader);
                            strMessage = strMessage.Substring(intEndOfHeader);
                            string sBody = "";
                            int intLineEnd;
                            for (int intLine = 1, loopTo = intLineCounter; intLine <= loopTo; intLine++) // Will not execute if no second parameter or if it is 0
                            {
                                intLineEnd = 2 + strMessage.IndexOf(Globals.CRLF);
                                if (intLineEnd != 1)
                                {
                                    sBody = sBody + strMessage.Substring(0, intLineEnd);
                                    strMessage = strMessage.Substring(intLineEnd);
                                }
                                else
                                {
                                    sBody = sBody + strMessage;
                                    break;
                                }
                            }

                            return "+OK " + Globals.CRLF + sHeader + sBody + Globals.CRLF + "." + Globals.CRLF;
                        }
                    }
                }

                return "-ERR no such message" + Globals.CRLF;
            }
            catch (Exception e)
            {
                Log.Error("[8273] " + e.Message);
                return "-ERR internal error" + Globals.CRLF;
            }
        } // TopResponse

        public bool Authorize(string strAccount, string strPassword)
        {
            // Function to check authorization in the Accounts array list. Returns true 
            // if authorized user otherwise returns false.

            var objAccount = Accounts.GetUserAccount(strAccount);
            if ((objAccount.Name ?? "") == (strAccount ?? ""))
            {
                if ((objAccount.Password ?? "") == (strPassword ?? "") && !string.IsNullOrEmpty(strPassword))
                {
                    return true;
                }
            }

            return false;
        } // Authorize
    }
}