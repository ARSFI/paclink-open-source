using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using NLog;

namespace Paclink
{
    public class SMTPSession
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        
        public delegate void DisconnectHandler(SMTPSession conn);
        public event DisconnectHandler OnDisconnect;

        // Declaration of SessionState
        private enum SessionState
        {
            Connected,
            Ready,
            AuthPlain,
            AuthLoginID,
            AuthLoginPass,
            StartMail,
            GetRecipients,
            GetData,
            Failure,
            Disconnected
        }

        // Initialize SMTP receiver reply string constants...
        private const string Reply220 = "220 Winlink.org" + Globals.CRLF; // Shortened 220 without return path
        private const string Reply221 = "221 Closing Service" + Globals.CRLF; // 221 without return path
        private const string Reply250 = "250 OK" + Globals.CRLF; // Shortened OK without return path
        private const string Reply500 = "500 Syntax Error, command unrecognized" + Globals.CRLF;
        private const string Reply501 = "501 Syntax Error in parameters or arguments" + Globals.CRLF;
        private const string Reply503 = "503 Bad sequence of commands" + Globals.CRLF;
        private const string Reply504 = "504 Command parameter not implemented" + Globals.CRLF;
        private const string Reply211 = "211 System status, or system help reply" + Globals.CRLF;
        private const string Reply214 = "214 Help Message" + Globals.CRLF;
        private const string Reply450 = "450 mailbox not available" + Globals.CRLF;
        private const string Reply550 = "550 user not registered" + Globals.CRLF;
        private const string Reply451 = "451 requested action aborted" + Globals.CRLF;
        private const string Reply452 = "452 action not taken, insufficient resources" + Globals.CRLF;
        private const string Reply553 = "553 action not taken; mailbox name syntax error" + Globals.CRLF;
        private const string Reply354 = "354 Start mail input, end with <CRLF>.<CRLF>" + Globals.CRLF;
        private const string Reply554 = "554 Transaction failed or rejected" + Globals.CRLF;
        public Socket connection;
        public DateTime Timestamp;
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

        private SessionState SMTPState;
        private string strMessageId;
        private string strMessageBody;
        private string strCommand;
        private string strCommandBuffer;
        private string strRecordBuffer;
        private string strTransparentBuffer;
        private string strNextTransparentBuffer;
        // Private strMimeFilename As String
        private bool blnMessageErrorFlag;
        private string strMimeFilePath;
        private string strAccountName;
        private string strPassword;
        private StringBuilder sbdInboundMessage;
        private string strRecipients = "";

        public SMTPSession(Socket socket)
        {
            Timestamp = DateTime.Now;
            connection = socket;
            strMessageBody = "";
            strCommandBuffer = "";
            SMTPState = SessionState.Connected;
            // strMimeFilename = ""

            dttSessionStart = DateTime.Now.AddMinutes(5);
            tmrSessionTimer = new System.Timers.Timer();
            tmrSessionTimer.Elapsed += OnSessionTimer;
            tmrSessionTimer.Enabled = false;
            tmrSessionTimer.Interval = 60000;
            tmrSessionTimer.AutoReset = true;
            tmrSessionTimer.Start();

            // Begin session.
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
            {
                _log.Info("Out to " + connection.RemoteEndPoint.ToString() + ": " + strText);
                connection.Send(Globals.GetBytes(strResponse));
                if (strResponse.StartsWith("221 "))
                    Close();
            }

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
                Close();
            }
        } // OnSessionTimer

        private string Protocol(string strInputStream)
        {
            string ProtocolRet = default;
            // This function processes the incoming data stream for the SMTPSession object.
            // If the string contains a command then the command is processed to update 
            // the SMTPstate and generate a reply string. An empty string value is returned 
            // to signal that no response is required to the partial command or data string.
            // Data and command processing is a function of the current SMTPState.
            // Authorization for both PLAIN and LOGIN is implemented... 

            string strTemporary;
            int intPointer;

            // Check for GetData state to see if CMD processing required...
            if (SMTPState != SessionState.GetData)
            {
                if (strInputStream == "NewConnection")
                {
                    // The SMTPSession has just been initialized.
                    // Acknowledge with Reply 220 with echo local IP Address...
                    _log.Info("In from " + connection.RemoteEndPoint + ": " + "(New connection started)");
                    strCommandBuffer = ""; // Clear the command buffer
                    return Reply220; // Local IP address service ready
                }

                // This is for a complete command or the CRLF to complete the command...
                if ((strInputStream.Right(2) ?? "") == Globals.CRLF)
                {
                    strInputStream = strCommandBuffer + strInputStream;
                    strCommand = strInputStream.Left(4).ToUpper();
                    strCommandBuffer = "";
                }
                else
                {
                    // This accumulates a command which bridges multiple data in events...
                    strCommandBuffer = strCommandBuffer + strInputStream;
                    return "";
                }
            }

            // Process the command...
            _log.Info("In from " + connection.RemoteEndPoint + ": " + strCommand);
            var switchExpr = SMTPState;
            switch (switchExpr)
            {
                case SessionState.Connected: // The inital state instantiated state...
                    {
                        var switchExpr1 = strCommand;
                        switch (switchExpr1)
                        {
                            case "EHLO": // Reply to ELHO only extensions supported are AUTH...
                                {
                                    SMTPState = SessionState.Ready; // Update the state
                                                                    // OK accept LOGIN and PLAIN authorization - Note both formats are 
                                                                    // required below since there was some prior incompatibility in the spec...
                                                                    // Add in a Hello response to the return packet.
                                    return "250-smtp.winlink.org Hello " + strInputStream.Substring(5) + "250-AUTH LOGIN PLAIN" + Globals.CRLF + "250-AUTH=LOGIN PLAIN" + Globals.CRLF + "250 OK" + Globals.CRLF;


                                }

                            case "QUIT": // Send reply to shut down connection...
                                {
                                    SMTPState = SessionState.Disconnected;  // Update the state
                                    return Reply221;  // Closing service
                                }

                            case "VRFY":  // Currently reply OK to all recipients
                                {
                                    return "250 OK " + strInputStream + Globals.CRLF;  // If command not "EHLO", "VRFY",  or "QUIT" return error
                                }

                            default:
                                {
                                    return Reply504; // Command paramater not implemented
                                }
                        }

                        break;
                    }

                case SessionState.AuthPlain:
                    {
                        var switchExpr2 = strCommand;
                        switch (switchExpr2)
                        {
                            case "RSET": // Restart the protocol...
                                {
                                    SMTPState = SessionState.Ready;
                                    return Reply250; // OK
                                }

                            default:
                                {
                                    // This decodes the Base64 Client reply for PLAIN for UserName and Password...
                                    UserPasswordDecode(ref strAccountName, ref strPassword, strInputStream);

                                    // Return to the ready state on Authorization success or faiure...
                                    SMTPState = SessionState.Ready;

                                    // Check the user array for authorization: UserName and Password must match (case insensitive)...
                                    strMimeFilePath = Authorize(strAccountName, strPassword);

                                    // Sucessful authorization if the MIME path is returned...
                                    if (!string.IsNullOrEmpty(strMimeFilePath))
                                    {
                                        return "235 OK Authenticated" + Globals.CRLF;
                                    }
                                    else
                                    {
                                        return "501 Authentication failed" + Globals.CRLF;
                                    }

                                    break;
                                }
                        }

                        break;
                    }

                case SessionState.AuthLoginID:
                    {
                        var switchExpr3 = strCommand;
                        switch (switchExpr3)
                        {
                            case "RSET": // Restart the protocol...
                                {
                                    SMTPState = SessionState.Ready;
                                    return Reply250; // OK
                                }

                            default:
                                {
                                    // Decode the Base64 Client reply for account name...
                                    strAccountName = Base64Decode(strInputStream).ToUpper();

                                    // Change state to receive password...
                                    SMTPState = SessionState.AuthLoginPass; // Change state to receive password

                                    // Send the Base64 encoded request for password...
                                    return "334 " + Base64Encode("Password:") + Globals.CRLF;
                                }
                        }

                        break;
                    }

                case SessionState.AuthLoginPass:
                    {
                        var switchExpr4 = strCommand;
                        switch (switchExpr4)
                        {
                            case "RSET":
                                {
                                    SMTPState = SessionState.Ready;
                                    return Reply250; // OK
                                }

                            default:
                                {
                                    // Decode the Base64 Client reply for password...
                                    strPassword = Base64Decode(strInputStream).ToUpper();

                                    // Check the user array for authorization: Account name and password must match (case insensitive)...
                                    strMimeFilePath = Authorize(strAccountName, strPassword);

                                    // Return to the ready state on Authorization success or faiure...
                                    SMTPState = SessionState.Ready;

                                    // Sucessful authorization if the MIME path is returned...
                                    if (!string.IsNullOrEmpty(strMimeFilePath))
                                    {
                                        Globals.queSMTPDisplay.Enqueue("BSMTP link from " + strAccountName + " at " + Globals.TimestampEx());
                                        return "235 OK Authenticated" + Globals.CRLF;
                                    }
                                    else
                                    {
                                        return "501 Authentication failed" + Globals.CRLF;
                                    }

                                    break;
                                }
                        }

                        break;
                    }

                case SessionState.Ready: // The state after receiving "EHLO" and Authorization...
                    {
                        var switchExpr5 = strCommand;
                        switch (switchExpr5)
                        {
                            case "MAIL":
                            case "SOML":
                                {
                                    strRecipients = ""; // clear out the recipients
                                    if (strInputStream.IndexOf("<") != -1)
                                    {
                                        if (strInputStream.ToLower().IndexOf("winlink.org") == -1)
                                        {
                                            return Reply504;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(strMimeFilePath))
                                    {
                                        blnMessageErrorFlag = false; // Reset the message error flag
                                        SMTPState = SessionState.StartMail;
                                        sbdInboundMessage = null;
                                        return Reply250; // OK
                                    }
                                    else
                                    {
                                        return "530 Authentication required" + Globals.CRLF;
                                    }

                                    break;
                                }

                            case "RSET": // Restart the protocol...
                                {
                                    SMTPState = SessionState.Ready;
                                    return Reply250; // OK
                                }

                            case "QUIT":
                                {
                                    // send reply to shut down connection
                                    SMTPState = SessionState.Disconnected;
                                    ProtocolRet = Reply221; // closing service
                                    break;
                                }

                            case "AUTH": // Authorization request check for type
                                {
                                    intPointer = strInputStream.ToUpper().IndexOf("PLAIN");
                                    if (intPointer != -1 & strInputStream.Length < intPointer + 8)
                                    {
                                        // Initial AUTH did NOT contain the Base64 Encode
                                        SMTPState = SessionState.AuthPlain;
                                        return "334 " + Globals.CRLF; // Reply for a AUTH PLAIN request
                                    }
                                    else if (intPointer != -1)
                                    {
                                        // Base64 User/Password included in AUTH command (e.g Netscape)...
                                        strTemporary = strInputStream.Substring(intPointer + 5).Trim();

                                        // This decodes the Base64 Client reply for PLAIN for UserName and Password...
                                        UserPasswordDecode(ref strAccountName, ref strPassword, strTemporary);

                                        // Return to the ready state on authorization success or failure...
                                        SMTPState = SessionState.Ready;

                                        // Check the user array for authorization: UserName and Password must match (case insensitive)
                                        strMimeFilePath = Authorize(strAccountName, strPassword);
                                        if (!string.IsNullOrEmpty(strMimeFilePath)) // sucessful authorization if the MIME path is returned
                                        {
                                            return "235 OK Authenticated" + Globals.CRLF;
                                        }
                                        else
                                        {
                                            return "501 Authentication failed" + Globals.CRLF;
                                        }
                                    }
                                    else if (strInputStream.ToUpper().IndexOf("LOGIN") != -1)
                                    {
                                        intPointer = strInputStream.ToUpper().IndexOf("LOGIN");
                                        if (strInputStream.Length < intPointer + 8)
                                        {
                                            // Initial AUTH did NOT contain the Base64 Encode
                                            SMTPState = SessionState.AuthLoginID;
                                            return "334 " + Base64Encode("Username:") + Globals.CRLF;  // Base 64 encoded account name
                                        }
                                        else
                                        {
                                            // 
                                            // LOGIN included a base64 encoded username on the line.  In this case, we skip
                                            // directly to request password
                                            // 
                                            strTemporary = strInputStream.Substring(intPointer + 5).Trim();
                                            strAccountName = Base64Decode(strTemporary);
                                            SMTPState = SessionState.AuthLoginPass;
                                            return "334 " + Base64Encode("Password:") + Globals.CRLF;
                                        }
                                    }
                                    else
                                    {
                                        return "504 Unrecognized authentication type" + Globals.CRLF;
                                    }

                                    break;
                                }

                            default:
                                {
                                    return Reply500; // Command unrecognized
                                }
                        }

                        break;
                    }

                case SessionState.GetData:
                    {
                        // In the GetData state - only check is for End of data condition and handle <CRLF>.<CRLF>
                        // The following should catch any <CRLF>.<CRLF> sequence even over multiple data in events...

                        // Append data to the inbound string builder...
                        InboundMessage(strInputStream);
                        if ((strMessageBody.Right(4) + strInputStream).Right(5) == Globals.CRLF + "." + Globals.CRLF)
                        {
                            // This is the end of data...

                            var objPaclinkMessage = new SMTPMessage(sbdInboundMessage.ToString(), true);
                            if (objPaclinkMessage.IsAccepted)
                            {
                                // Added by RM Feb 25, 2008 check for compressed size
                                if (objPaclinkMessage.SaveMessageToWinlink())
                                {
                                    if (CheckForOversizeMessage(Globals.SiteRootDirectory + @"To Winlink\" + objPaclinkMessage.MessageId + ".mime"))
                                    {
                                        Globals.queSMTPDisplay.Enqueue("B" + objPaclinkMessage.MessageId + " received from " + strAccountName);
                                        Globals.queSMTPDisplay.Enqueue("B   Subject: " + objPaclinkMessage.Subject);
                                        Globals.queSMTPDisplay.Enqueue("B   Rejected! Exceeds 120KB compressed size limit.");
                                        return "554 Message exceeds WL2K's 120KB compressed size limit" + Globals.CRLF; // Transaction failed
                                    }
                                }

                                var intPtr = default(int);
                                if (IsMARSMessage(ref objPaclinkMessage))
                                {
                                    for (int i = 0; i <= 4; i++)
                                    {
                                        intPtr = objPaclinkMessage.Subject.ToUpper().IndexOf("//MARS " + "ZOPRM".Substring(i, 1) + "/");
                                        if (intPtr != -1)
                                            break;
                                    }

                                    if (intPtr != -1)
                                    {
                                        objPaclinkMessage.Subject = objPaclinkMessage.Subject.Substring(intPtr); // Drop off anything ahead of the "//MARS flag like Re: Fw: etc
                                    }
                                    else
                                    {
                                        // No MARS flag so add Routine Mars flag
                                        objPaclinkMessage.Subject = "//MARS R/ " + objPaclinkMessage.Subject;
                                    }
                                }
                                else
                                {
                                    intPtr = objPaclinkMessage.Subject.ToUpper().IndexOf("//WL2K");
                                    if (intPtr != -1)
                                    {
                                        objPaclinkMessage.Subject = objPaclinkMessage.Subject.Substring(intPtr); // Drop off anything ahead of the "//WL2K flag like Re: Fw: etc
                                    }
                                    else
                                    {
                                        // No WL2K flag so add Routine flag
                                        objPaclinkMessage.Subject = "//WL2K " + objPaclinkMessage.Subject;
                                    }
                                }

                                if (objPaclinkMessage.SaveMessageToWinlink())
                                {
                                    Globals.queSMTPDisplay.Enqueue("B" + objPaclinkMessage.MessageId + " received from " + strAccountName);
                                    Globals.queSMTPDisplay.Enqueue("B   Subject: " + objPaclinkMessage.Subject);
                                    SMTPState = SessionState.Ready; // Exit to the ready state
                                    return Reply250; // OK
                                }
                                else
                                {
                                    SMTPState = SessionState.Failure;
                                    return "554 " + "Failure to save message" + Globals.CRLF;
                                }
                            }
                            else
                            {
                                SMTPState = SessionState.Failure;
                                return "554 " + objPaclinkMessage.ErrorDescription + Globals.CRLF;
                            }
                        }
                        else
                        {
                            // Only need to buffer the last 4 Characters to be able catch end of data sequence above
                            strMessageBody = strMessageBody + strInputStream.Right(4);
                            return "";
                        }

                        break;
                    }

                case SessionState.StartMail:
                    {
                        var switchExpr6 = strCommand;
                        switch (switchExpr6)
                        {
                            case "RSET": // Restart the protocol...
                                {
                                    SMTPState = SessionState.Ready;
                                    return Reply250; // OK
                                }

                            case "QUIT":
                                {
                                    // Send reply to shut down connection
                                    SMTPState = SessionState.Disconnected;
                                    return Reply221; // Closing service
                                }

                            case "RCPT":
                                {
                                    strRecipients = strInputStream.Substring(1 + strInputStream.IndexOf(":")).Trim();
                                    SMTPState = SessionState.GetRecipients;
                                    return Reply250; // OK
                                }

                            case "VRFY":
                                {
                                    return Reply250; // OK
                                }

                            default:
                                {
                                    // Send error response...
                                    return Reply500; // Command unrecognized
                                }
                        }

                        break;
                    }

                case SessionState.GetRecipients:
                    {
                        var switchExpr7 = strCommand;
                        switch (switchExpr7)
                        {
                            case "RSET": // Restart the protocol...
                                {
                                    SMTPState = SessionState.Ready;
                                    return Reply250; // OK
                                }

                            case "RCPT":
                                {
                                    strRecipients = strRecipients + ";" + strInputStream.Substring(1 + strInputStream.IndexOf(":")).Trim();
                                    SMTPState = SessionState.GetRecipients;
                                    return Reply250; // OK
                                }

                            case "DATA":
                                {
                                    // Change to GetData State...
                                    SMTPState = SessionState.GetData;
                                    strMessageBody = "";
                                    // strMimeFilename = ""
                                    return Reply354;  // Start mail, end with <CRLF>.<CRLF>
                                }

                            case var @case when @case == "RSET":
                                {
                                    // Send reply for going to ready state...
                                    SMTPState = SessionState.Ready;
                                    return Reply250; // OK
                                }

                            case "QUIT":
                                {
                                    // Send Reply to shut down connection...
                                    SMTPState = SessionState.Disconnected;
                                    return Reply221;  // Closing service
                                }

                            case "VRFY":
                                {
                                    return Reply250; // OK
                                }

                            default:
                                {
                                    // Wrong command for GetRecipients state
                                    return Reply500; // Bad syntax, command not recognized
                                }
                        } // sCommand

                        break;
                    }

                case SessionState.Failure:
                    {
                        var switchExpr8 = strCommand;
                        switch (switchExpr8)
                        {
                            case "RSET": // Restart the protocol...
                                {
                                    SMTPState = SessionState.Ready;
                                    return Reply250; // OK
                                }

                            case "QUIT":
                                {
                                    // Send reply to shut down connection...
                                    SMTPState = SessionState.Disconnected;
                                    return Reply221; // Closing service
                                }

                            default:
                                {
                                    return "451 Local error cannot process command" + Globals.CRLF;
                                }
                        }

                        break;
                    }

                default:
                    {
                        return Reply503;
                    }
            } // SMTPState

            return ProtocolRet;
        } // Protocol

        private void InboundMessage(string strMessage)
        {
            // This routine creates and/or appends the incoming message to a MIME file for 
            // later processing. Transparancy functions Per RFC821 are validated...

            int intPositionCrLfPeriod;
            if (sbdInboundMessage == null)
            {
                sbdInboundMessage = new StringBuilder();
                strTransparentBuffer = "  ";    // Initialize the strTransparentBuffer to 2 spaces
                strRecordBuffer = "";   // Clear the RecordBuffer
            }

            // Save the last two char as the next strNextTransparentBuffer...
            strNextTransparentBuffer = (strTransparentBuffer + strMessage).Right(2);

            // Filter strTransparentBuffer for transparent "."
            do
            {
                intPositionCrLfPeriod = (strTransparentBuffer + strMessage).IndexOf(Globals.CRLF + ".");
                if (intPositionCrLfPeriod != -1) // There is a transparent "."
                {
                    strRecordBuffer = strRecordBuffer + strMessage.Left(intPositionCrLfPeriod - 1);
                    strMessage = strMessage.Right(strMessage.Length - intPositionCrLfPeriod + 1);  // skip the transparent "."
                }
                else // No more transparent "." left in strMessage...
                {
                    strRecordBuffer = strRecordBuffer + strMessage;
                }
            }
            while (intPositionCrLfPeriod > 0);  // end of loop
            sbdInboundMessage.Append(strRecordBuffer);
            strRecordBuffer = ""; // Clear the record buffer...
            strTransparentBuffer = strNextTransparentBuffer; // Update strTransparentBuffer with 2 characters
        } // MimeFileMessage

        private string NewFilename(string strTag)
        {
            string NewFilenameRet = default;
            // Generates a 12 character random filename with lead of up to the first 8 char of strTag...

            int intIndex;
            int intSeed;
            string strName;
            strName = strTag.Right(8);
            var rng = new Random();
            for (intIndex = strTag.Right(8).Length + 1; intIndex <= 12; intIndex++)
            {
                intSeed = rng.Next(0, 36);
                if (intSeed < 36)
                    intSeed = intSeed + 1;
                strName = strName + "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(intSeed, 1);
            }

            NewFilenameRet = strName;
            return NewFilenameRet;
        } // NewFilename

        private string Authorize(string strAccount, string strPassword)
        {
            // Function to check authorization in the Accounts array list. Returns the Mime file path 
            // if authorized user otherwise returns and empty string...

            var objAccount = Accounts.GetUserAccount(strAccount);
            if ((objAccount.Name ?? "") == (strAccount ?? ""))
            {
                if ((objAccount.Password ?? "") == (strPassword ?? "") && !string.IsNullOrEmpty(strPassword))
                {
                    return objAccount.MimePathIn;
                }
            }

            return "";
        } // Authorize

        private string Base64Decode(string strB64String)
        {
            string Base64DecodeRet = default;
            // Decode the Base64 response during Authorization...

            try
            {
                Base64DecodeRet = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(strB64String));
            }
            catch (Exception e)
            {
                _log.Error("[3203] " + e.Message);
                Base64DecodeRet = "";
            }

            return Base64DecodeRet;
        } // Base64Decode

        private string Base64Encode(string strText)
        {
            string Base64EncodeRet = default;
            // Encode a Base64 challange during authorization... 

            try
            {
                Base64EncodeRet = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(strText));
            }
            catch (Exception e)
            {
                _log.Error("[3201] " + e.Message);
                Base64EncodeRet = "";
            }

            return Base64EncodeRet;
        } // Base64Encode

        private void UserPasswordDecode(ref string strAccount, ref string strPassword, string strB64String)
        {
            // Does the decode and untangle of combined UserId and Password for a plain authorization...

            byte[] bytDecodeBytes = null;
            var strTempUserId = default(string);
            var strTempPassword = default(string);
            var intCount = default(int);
            var blnPassword = default(bool);
            try
            {
                bytDecodeBytes = Convert.FromBase64String(strB64String);
                intCount = bytDecodeBytes.Length;
            }
            catch (Exception e)
            {
                _log.Error("[3202] " + e.Message);
            }

            if (intCount > 1)
            {
                var loopTo = bytDecodeBytes.Length;
                for (intCount = 0; intCount <= loopTo; intCount++)
                {
                    if (bytDecodeBytes[intCount] != 0 & !blnPassword)
                    {
                        strTempUserId = strTempUserId + (char)bytDecodeBytes[intCount];
                    }

                    blnPassword = blnPassword | intCount > 0 & bytDecodeBytes[intCount] == 0;
                    if (bytDecodeBytes[intCount] != 0 & blnPassword)
                    {
                        strTempPassword = strTempPassword + (char)bytDecodeBytes[intCount];
                    }
                }
            }

            strAccount = strTempUserId.ToUpper();
            strPassword = strTempPassword;
        } // UserPasswordDecode

        private bool CheckForOversizeMessage(string strMessageFilename)
        {
            var objMessage = new Message(strMessageFilename);
            objMessage.B2Output(0);
            int intMessageCompressedSize = objMessage.CompressedSize();
            if (intMessageCompressedSize > 119900)
            {
                objMessage.DeleteFile(strMessageFilename);
                return true;
            }

            return false;
        }  // CheckForOversizeMessage

        private bool IsMARSMessage(ref SMTPMessage objMessage)
        {
            // If site callsign is MARS callsign or
            // if sender is MARS callsign or 
            // if any recipient is MARS callsign 
            // return true otherwise false
            if (Globals.IsMARSStation())
                return true;
            if (Globals.IsMARSCallsign(objMessage.Sender.RadioAddress))
                return true;
            return objMessage.IsAnyRecipientMARS();
        } // IsMARSMessage
    } // SMTPSession
}