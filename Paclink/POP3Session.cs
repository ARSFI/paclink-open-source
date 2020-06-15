using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public class POP3Session
    {
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
        private const string ReplyOK = "+OK " + Constants.vbCrLf;
        private const string ReplyERR = "-ERR " + Constants.vbCrLf;
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
        private FileInfo[] aryPendingMimeList;
        private FileStream objMimeFileStream;
        private SessionState POP3State;
        private string strCommand;
        private string strCommandBuffer;
        private string strParameter;
        private string strMimeFilePath;
        private string strAccountName;
        private string strPassword;
        private int intNumberOfMimeFiles;
        private int intTotalBytes;

        public POP3Session(Socket sock)
        {
            Timestamp = DateAndTime.Now;
            connection = sock;
            dttSessionStart = DateAndTime.Now.AddMinutes(5);
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
            if (dttSessionStart < DateAndTime.Now)
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

            if ((Strings.Right(strInputStream, 2) ?? "") == Constants.vbCrLf)
            {
                // This is for a complete command or the CRLF to complete the command...
                strInputStream = strCommandBuffer + strInputStream;
                strCommand = Strings.Left(strInputStream, 4).ToUpper().Trim();
                strCommandBuffer = "";
                strParameter = Strings.Trim(Strings.Mid(strInputStream, 5, strInputStream.Length - 6));
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
                                    strMimeFilePath = Authorize(strAccountName, strPassword);
                                    if (!string.IsNullOrEmpty(strMimeFilePath))
                                    {
                                        AuthorizedUser = strAccountName;
                                        POP3State = SessionState.Transaction;
                                        Globals.queSMTPDisplay.Enqueue("GPOP3 link from " + strAccountName + " at " + Globals.TimestampEx());
                                        return ReplyOK;
                                    }
                                    else
                                    {
                                        return "501 Authentication failed" + Constants.vbCrLf;
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
            if (File.Exists(strMimeFilePath) == false)
            {
                Directory.CreateDirectory(strMimeFilePath);
            }

            var objMimeDirectoryInfo = new DirectoryInfo(strMimeFilePath);
            int intCount = 0;
            int intByteCount = 0;
            aryPendingMimeList = null;
            blnDeleteFlags = null;
            aryPendingMimeList = objMimeDirectoryInfo.GetFiles();
            if (aryPendingMimeList is object)
            {
                foreach (FileInfo objFileInfo in aryPendingMimeList)
                {
                    Array.Resize(ref blnDeleteFlags, intCount + 1);
                    blnDeleteFlags[intCount] = false;
                    intCount += 1;
                    intByteCount += Conversions.ToInteger(objFileInfo.Length);
                }
            }

            intNumberOfMimeFiles = intCount;
            intTotalBytes = intByteCount;
        } // GetPendingMimeList

        private string StatResponse()
        {
            string StatResponseRet = default;
            // Provides a string for the summary to the STAT command...

            GetPendingMimeList();
            StatResponseRet = "+OK " + intNumberOfMimeFiles.ToString() + " " + intTotalBytes.ToString() + Constants.vbCrLf;
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
                strResponse = "+OK " + intNumberOfMimeFiles.ToString() + " " + intTotalBytes.ToString() + Constants.vbCrLf;
                if (aryPendingMimeList is object)
                {
                    foreach (FileInfo objFileInfo in aryPendingMimeList)
                    {
                        intCount = intCount + 1;
                        strResponse = strResponse + intCount.ToString() + " " + objFileInfo.Length.ToString() + Constants.vbCrLf;
                    }
                }

                strResponse = strResponse + "." + Constants.vbCrLf;  // Add the terminator
            }
            else  // Just list the requested message as a one line response...
            {
                int intIndex;
                intIndex = Conversions.ToInteger(strMessageId);
                if (aryPendingMimeList is object)
                {
                    if (intIndex > 0 & intIndex <= intNumberOfMimeFiles)
                    {
                        strResponse = "+OK " + intIndex.ToString() + " " + aryPendingMimeList[intIndex - 1].Length.ToString() + Constants.vbCrLf;
                    }
                    else
                    {
                        strResponse = "-ERR no such message" + Constants.vbCrLf;
                    }
                }
                else
                {
                    strResponse = "-ERR no such message" + Constants.vbCrLf;
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
                UidlResponseRet = "+OK " + Constants.vbCrLf;
                if (intNumberOfMimeFiles != 0)
                {
                    if (aryPendingMimeList is object)
                    {
                        foreach (FileInfo objFileInfo in aryPendingMimeList)
                        {
                            intCount = intCount + 1;
                            strId = objFileInfo.FullName;
                            intLastSlashPosition = Strings.InStrRev(strId, @"\");
                            strId = Strings.Mid(strId, intLastSlashPosition + 1);
                            strId = Strings.Replace(strId, ".mime", "");
                            UidlResponseRet = UidlResponseRet + intCount.ToString() + " " + strId + Constants.vbCrLf;
                        }
                    }

                    UidlResponseRet = UidlResponseRet + "." + Constants.vbCrLf; // add terminator
                }
                else
                {
                    UidlResponseRet = "+OK" + Constants.vbCrLf + "." + Constants.vbCrLf;
                }
            }
            else // Request for specific message just send a single line response...
            {
                intIndex = Conversions.ToInteger(strMessageId);
                if (aryPendingMimeList is object)
                {
                    if (intIndex > 0 & intIndex <= intNumberOfMimeFiles)
                    {
                        strId = aryPendingMimeList[intIndex - 1].FullName;
                        intLastSlashPosition = Strings.InStrRev(strId, @"\");
                        strId = Strings.Mid(strId, intLastSlashPosition + 1);
                        strId = Strings.Replace(strId, ".mime", "");
                        UidlResponseRet = "+OK " + intIndex.ToString() + " " + strId + Constants.vbCrLf;
                    }
                    else
                    {
                        UidlResponseRet = "-ERR" + Constants.vbCrLf;
                    }
                }
                else
                {
                    UidlResponseRet = "-ERR" + Constants.vbCrLf;
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
                intIndex = Conversions.ToInteger(sParam);
                if (intIndex > 0 & intIndex <= Information.UBound(blnDeleteFlags) + 1)
                {
                    if (blnDeleteFlags[intIndex - 1] == false) // Only mark if not yet marked for deletion
                    {
                        blnDeleteFlags[intIndex - 1] = true;
                        DeleteMessageRet = "+OK message " + intIndex.ToString() + " marked for deletion" + Constants.vbCrLf;
                        return DeleteMessageRet;
                    }
                }
            }

            DeleteMessageRet = "-ERR no such message" + Constants.vbCrLf;
            return DeleteMessageRet;
        } // DeleteMessage

        private string ResetMessages()
        {
            // Reset all delete flags back to undelete state...

            int intIndex;
            var loopTo = Information.UBound(blnDeleteFlags) + 1;
            for (intIndex = 1; intIndex <= loopTo; intIndex++)
                blnDeleteFlags[intIndex - 1] = false;
            return "+OK maildrop has " + (1 + Information.UBound(blnDeleteFlags)).ToString() + " messages" + Constants.vbCrLf;
        } // ResetMessages

        public string DeleteMimeFiles()
        {
            string DeleteMimeFilesRet = default;
            // Delete all the MIME files marked for deletion in array blnDeleteFlags.
            // Only called after QUIT received in the Transaction state...

            int intIndex;
            if (intNumberOfMimeFiles == 0)
            {
                DeleteMimeFilesRet = "+OK" + Constants.vbCrLf;
                return DeleteMimeFilesRet;
            }

            if (aryPendingMimeList is object)
            {
                var loopTo = intNumberOfMimeFiles;
                for (intIndex = 1; intIndex <= loopTo; intIndex++)
                {
                    if (blnDeleteFlags[intIndex - 1] == true)
                    {
                        FileSystem.Kill(aryPendingMimeList[intIndex - 1].FullName);
                    }
                }
            }

            DeleteMimeFilesRet = "+OK" + Constants.vbCrLf;
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
                intIndex = Conversions.ToInteger(strMessageNumber);
                if (intNumberOfMimeFiles == 0)
                {
                    RetrieveMessageRet = "-ERR no messages" + Constants.vbCrLf;
                    return RetrieveMessageRet;
                }

                if (aryPendingMimeList is object)
                {
                    if (intIndex > 0 & intIndex <= 1 + Information.UBound(blnDeleteFlags))
                    {
                        if (blnDeleteFlags[intIndex - 1] == false) // Only retrieve messages NOT marked for deletion
                        {
                            srdMime = new StreamReader(aryPendingMimeList[intIndex - 1].FullName);
                            strMessage = srdMime.ReadToEnd();

                            // Decode header for display and logging.
                            var strSubject = default(string);
                            var strFrom = default(string);
                            var strHeaderLines = strMessage.Split(Conversions.ToChar(Constants.vbCr));
                            foreach (string strLine in strHeaderLines)
                            {
                                var tmpStrLine = strLine.Replace(Constants.vbLf, "").Trim();
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
                            strMessage = Strings.Replace(strMessage, Constants.vbCrLf + ".", Constants.vbCrLf + "..");
                            string strMessageId = aryPendingMimeList[intIndex - 1].FullName;
                            int nPosLastSlash = Strings.InStrRev(strMessageId, @"\");
                            strMessageId = Strings.Mid(strMessageId, nPosLastSlash + 1);
                            strMessageId = Strings.Replace(strMessageId, ".mime", "");
                            Globals.queSMTPDisplay.Enqueue("G" + strMessageId + " delivered to " + strAccountName);
                            Globals.queSMTPDisplay.Enqueue("G   " + strFrom);
                            Globals.queSMTPDisplay.Enqueue("G   " + strSubject);
                            RetrieveMessageRet = "+OK " + Constants.vbCrLf + strMessage + Constants.vbCrLf + "." + Constants.vbCrLf;
                            srdMime.Close();
                            return RetrieveMessageRet;
                        }
                    }
                }
            }

            RetrieveMessageRet = "-ERR no such message" + Constants.vbCrLf;
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
                    intIndex = Conversions.ToInteger(Param[0]);
                    int intLineCounter = 0;
                    if (Param.Length > 1)
                        intLineCounter = Conversions.ToInteger(Param[1]);
                    if (intNumberOfMimeFiles == 0)
                    {
                        TopResponseRet = "-ERR no messages" + Constants.vbCrLf;
                        return TopResponseRet;
                    }

                    if (intIndex > 0 & intIndex <= 1 + Information.UBound(blnDeleteFlags))
                    {
                        if (blnDeleteFlags[intIndex - 1] == false) // only retrieve messages NOT marked for deletion
                        {
                            strMime = new StreamReader(aryPendingMimeList[intIndex - 1].FullName);
                            strMessage = strMime.ReadToEnd();
                            // Add byte stuffing for <CrLf . >
                            strMessage = Strings.Replace(strMessage, Constants.vbCrLf + ".", Constants.vbCrLf + "..");
                            int intEndOfHeader = 4 + strMessage.IndexOf(Constants.vbCrLf + Constants.vbCrLf);
                            string sHeader = strMessage.Substring(0, intEndOfHeader);
                            strMessage = strMessage.Substring(intEndOfHeader);
                            string sBody = "";
                            int intLineEnd;
                            for (int intLine = 1, loopTo = intLineCounter; intLine <= loopTo; intLine++) // Will not execute if no second parameter or if it is 0
                            {
                                intLineEnd = 2 + strMessage.IndexOf(Constants.vbCrLf);
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

                            strMime.Close();
                            return "+OK " + Constants.vbCrLf + sHeader + sBody + Constants.vbCrLf + "." + Constants.vbCrLf;
                        }
                    }
                }

                return "-ERR no such message" + Constants.vbCrLf;
            }
            catch
            {
                Logs.Exception("[8273] " + Information.Err().Description);
                return "-ERR internal error" + Constants.vbCrLf;
            }
        } // TopResponse

        public string Authorize(string strAccount, string strPassword)
        {
            // Function to check authorization in the Accounts array list. Returns the Mime file path 
            // if authorized user otherwise returns and empty string...

            var objAccount = Accounts.GetUserAccount(strAccount);
            if ((objAccount.Name ?? "") == (strAccount ?? ""))
            {
                if ((objAccount.Password ?? "") == (strPassword ?? "") && !string.IsNullOrEmpty(strPassword))
                {
                    return objAccount.MimePathOut;
                }
            }

            return "";
        } // Authorize
    }
}