﻿using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public class SMTPSession
    {

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
        private const string Reply220 = "220 Winlink.org" + Constants.vbCrLf; // Shortened 220 without return path
        private const string Reply221 = "221 Closing Service" + Constants.vbCrLf; // 221 without return path
        private const string Reply250 = "250 OK" + Constants.vbCrLf; // Shortened OK without return path
        private const string Reply500 = "500 Syntax Error, command unrecognized" + Constants.vbCrLf;
        private const string Reply501 = "501 Syntax Error in parameters or arguments" + Constants.vbCrLf;
        private const string Reply503 = "503 Bad sequence of commands" + Constants.vbCrLf;
        private const string Reply504 = "504 Command parameter not implemented" + Constants.vbCrLf;
        private const string Reply211 = "211 System status, or system help reply" + Constants.vbCrLf;
        private const string Reply214 = "214 Help Message" + Constants.vbCrLf;
        private const string Reply450 = "450 mailbox not available" + Constants.vbCrLf;
        private const string Reply550 = "550 user not registered" + Constants.vbCrLf;
        private const string Reply451 = "451 requested action aborted" + Constants.vbCrLf;
        private const string Reply452 = "452 action not taken, insufficient resources" + Constants.vbCrLf;
        private const string Reply553 = "553 action not taken; mailbox name syntax error" + Constants.vbCrLf;
        private const string Reply354 = "354 Start mail input, end with <CRLF>.<CRLF>" + Constants.vbCrLf;
        private const string Reply554 = "554 Transaction failed or rejected" + Constants.vbCrLf;
        public string ConnectionId;
        public DateTime Timestamp;
        private SMTPPort objSMTPPort;
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

        public SMTPSession(SMTPPort parent, string strNewConnectionId)
        {
            Timestamp = DateAndTime.Now;
            objSMTPPort = parent;
            ConnectionId = strNewConnectionId;
            strMessageBody = "";
            strCommandBuffer = "";
            SMTPState = SessionState.Connected;
            // strMimeFilename = ""

            dttSessionStart = DateAndTime.Now.AddMinutes(5);
            tmrSessionTimer = new System.Timers.Timer();
            tmrSessionTimer.Elapsed += OnSessionTimer;
            tmrSessionTimer.Enabled = false;
            tmrSessionTimer.Interval = 60000;
            tmrSessionTimer.AutoReset = true;
            tmrSessionTimer.Start();
        } // New

        public void Close()
        {
            tmrSessionTimer.Stop();
            tmrSessionTimer.Dispose();
            tmrSessionTimer = null;
        } // Close

        public void DataIn(string strText)
        {
            string strResponse = Protocol(strText);
            if (!string.IsNullOrEmpty(strResponse))
            {
                Logs.SMTPEvent("Out to " + ConnectionId + ": " + strText);
                objSMTPPort.DataToSend(strResponse, ConnectionId);
                if (strResponse.StartsWith("221 "))
                    objSMTPPort.Disconnect(ConnectionId);
            }
        } // DataIn

        private void OnSessionTimer(object s, ElapsedEventArgs e)
        {
            if (dttSessionStart < DateAndTime.Now)
            {
                tmrSessionTimer.Stop();
                tmrSessionTimer.Dispose();
                objSMTPPort.Disconnect(ConnectionId);
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
                    Logs.SMTPEvent("In from " + ConnectionId + ": " + "(New connection started)");
                    strCommandBuffer = ""; // Clear the command buffer
                    return Reply220; // Local IP address service ready
                }

                // This is for a complete command or the CRLF to complete the command...
                if ((Strings.Right(strInputStream, 2) ?? "") == Constants.vbCrLf)
                {
                    strInputStream = strCommandBuffer + strInputStream;
                    strCommand = Strings.UCase(Strings.Left(strInputStream, 4));
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
            Logs.SMTPEvent("In from " + ConnectionId + ": " + strCommand);
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
                                    return "250-smtp.winlink.org Hello " + strInputStream.Substring(5) + "250-AUTH LOGIN PLAIN" + Constants.vbCrLf + "250-AUTH=LOGIN PLAIN" + Constants.vbCrLf + "250 OK" + Constants.vbCrLf;


                                }

                            case "QUIT": // Send reply to shut down connection...
                                {
                                    SMTPState = SessionState.Disconnected;  // Update the state
                                    return Reply221;  // Closing service
                                }

                            case "VRFY":  // Currently reply OK to all recipients
                                {
                                    return "250 OK " + strInputStream + Constants.vbCrLf;  // If command not "EHLO", "VRFY",  or "QUIT" return error
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
                                        return "235 OK Authenticated" + Constants.vbCrLf;
                                    }
                                    else
                                    {
                                        return "501 Authentication failed" + Constants.vbCrLf;
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
                                    return "334 " + Base64Encode("Password:") + Constants.vbCrLf;
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
                                        return "235 OK Authenticated" + Constants.vbCrLf;
                                    }
                                    else
                                    {
                                        return "501 Authentication failed" + Constants.vbCrLf;
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
                                        return "530 Authentication required" + Constants.vbCrLf;
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
                                    intPointer = Strings.InStr(strInputStream.ToUpper(), "PLAIN");
                                    if (intPointer != 0 & strInputStream.Length < intPointer + 7)
                                    {
                                        // Initial AUTH did NOT contain the Base64 Encode
                                        SMTPState = SessionState.AuthPlain;
                                        return "334 " + Constants.vbCrLf; // Reply for a AUTH PLAIN request
                                    }
                                    else if (intPointer != 0)
                                    {
                                        // Base64 User/Password included in AUTH command (e.g Netscape)...
                                        strTemporary = Strings.Trim(Strings.Mid(strInputStream, intPointer + 5));

                                        // This decodes the Base64 Client reply for PLAIN for UserName and Password...
                                        UserPasswordDecode(ref strAccountName, ref strPassword, strTemporary);

                                        // Return to the ready state on authorization success or failure...
                                        SMTPState = SessionState.Ready;

                                        // Check the user array for authorization: UserName and Password must match (case insensitive)
                                        strMimeFilePath = Authorize(strAccountName, strPassword);
                                        if (!string.IsNullOrEmpty(strMimeFilePath)) // sucessful authorization if the MIME path is returned
                                        {
                                            return "235 OK Authenticated" + Constants.vbCrLf;
                                        }
                                        else
                                        {
                                            return "501 Authentication failed" + Constants.vbCrLf;
                                        }
                                    }
                                    else if (Strings.InStr(strInputStream.ToUpper(), "LOGIN") != 0)
                                    {
                                        intPointer = Strings.InStr(strInputStream.ToUpper(), "LOGIN");
                                        if (strInputStream.Length < intPointer + 7)
                                        {
                                            // Initial AUTH did NOT contain the Base64 Encode
                                            SMTPState = SessionState.AuthLoginID;
                                            return "334 " + Base64Encode("Username:") + Constants.vbCrLf;  // Base 64 encoded account name
                                        }
                                        else
                                        {
                                            // 
                                            // LOGIN included a base64 encoded username on the line.  In this case, we skip
                                            // directly to request password
                                            // 
                                            strTemporary = Strings.Trim(Strings.Mid(strInputStream, intPointer + 5));
                                            strAccountName = Base64Decode(strTemporary);
                                            SMTPState = SessionState.AuthLoginPass;
                                            return "334 " + Base64Encode("Password:") + Constants.vbCrLf;
                                        }
                                    }
                                    else
                                    {
                                        return "504 Unrecognized authentication type" + Constants.vbCrLf;
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
                        if ((Strings.Right(Strings.Right(strMessageBody, 4) + strInputStream, 5) ?? "") == Constants.vbCrLf + "." + Constants.vbCrLf)
                        {
                            // This is the end of data...

                            var objPaclinkMessage = new SMTPMessage(sbdInboundMessage.ToString(), true);
                            if (objPaclinkMessage.AnyBcc(strRecipients))
                            {
                                Globals.queSMTPDisplay.Enqueue("B" + objPaclinkMessage.MessageId + " from " + strAccountName + " rejected due to Bcc");
                                Globals.queSMTPDisplay.Enqueue("BSubject: " + objPaclinkMessage.Subject);
                                SMTPState = SessionState.Failure;
                                return "554 " + "Winlink does not accept Bcc recipients" + Constants.vbCrLf;
                            }

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
                                        return "554 Message exceeds WL2K's 120KB compressed size limit" + Constants.vbCrLf; // Transaction failed
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
                                    return "554 " + "Failure to save message" + Constants.vbCrLf;
                                }
                            }
                            else
                            {
                                SMTPState = SessionState.Failure;
                                return "554 " + objPaclinkMessage.ErrorDescription + Constants.vbCrLf;
                            }
                        }
                        else
                        {
                            // Only need to buffer the last 4 Characters to be able catch end of data sequence above
                            strMessageBody = Strings.Right(strMessageBody + strInputStream, 4);
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
                                    return "451 Local error cannot process command" + Constants.vbCrLf;
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
            if (Information.IsNothing(sbdInboundMessage))
            {
                sbdInboundMessage = new StringBuilder();
                strTransparentBuffer = "  ";    // Initialize the strTransparentBuffer to 2 spaces
                strRecordBuffer = "";   // Clear the RecordBuffer
            }

            // Save the last two char as the next strNextTransparentBuffer...
            strNextTransparentBuffer = Strings.Right(strTransparentBuffer + strMessage, 2);

            // Filter strTransparentBuffer for transparent "."
            do
            {
                intPositionCrLfPeriod = Strings.InStr(strTransparentBuffer + strMessage, Constants.vbCrLf + ".");
                if (intPositionCrLfPeriod != 0) // There is a transparent "."
                {
                    strRecordBuffer = strRecordBuffer + Strings.Left(strMessage, intPositionCrLfPeriod - 1);
                    strMessage = Strings.Right(strMessage, Strings.Len(strMessage) - intPositionCrLfPeriod);  // skip the transparent "."
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
            strName = Strings.Left(strTag, 8);
            VBMath.Randomize();
            for (intIndex = Strings.Len(Strings.Left(strTag, 8)) + 1; intIndex <= 12; intIndex++)
            {
                intSeed = Conversions.ToInteger(VBMath.Rnd() * 36);
                if (intSeed < 36)
                    intSeed = intSeed + 1;
                strName = strName + Strings.Mid("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", intSeed, 1);
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
            catch
            {
                Logs.Exception("[3203] " + Information.Err().Description);
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
            catch
            {
                Logs.Exception("[3201] " + Information.Err().Description);
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
                intCount = Information.UBound(bytDecodeBytes);
            }
            catch
            {
                Logs.Exception("[3202] " + Information.Err().Description);
            }

            if (intCount > 1)
            {
                var loopTo = Information.UBound(bytDecodeBytes);
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