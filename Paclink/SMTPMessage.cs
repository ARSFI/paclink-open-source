using System;
using System.Collections;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{

    // Class to process an inbound messages from the STMP side...
    internal class SMTPMessage : IMessage
    {
        private const string strBlockedExtensions = "exe pif scr ";
        internal bool IsAccepted = false;
        internal string ErrorDescription = "Unknown mime decode failure";

        // These variables are the basic properties of this class...
        internal string MessageId = "";
        internal DateTime MessageDate;
        internal DateTime ExpirationDate;
        internal WinlinkAddress Sender = new WinlinkAddress("");    // The Sender's address
        internal WinlinkAddress ReplyTo = new WinlinkAddress("");
        internal string Subject = "";          // The message subject
        internal string Header = "";           // The header used in mime format
                                               // Friend blnDecodeMimeThreadResult As Boolean
        internal bool FromClient;

        // Note: the Attachment structure is defined in Global.vb...
        private ArrayList aryAttachments = new ArrayList(); // An array of Attachment structures
        private string strMime = "";            // The entire message in mime format
        private string strBody = "";            // The body of the message
        private DateTime dttMessageDate = Conversions.ToDate("00:00");
        private DateTime dttExpirationDate = Conversions.ToDate("00:00");
        private Collection cllToAddresses = new Collection();  // Collection of address objects
        private Collection cllCcAddresses = new Collection();  // Collection of address objects

        internal SMTPMessage(string strNewMime, bool blnFromClient)
        {
            FromClient = blnFromClient;
            Mime = strNewMime;
            if (blnFromClient)
                MessageId = Globals.GetNewRandomMid();
            IsAccepted = DecodeMime();
        } // New

        public void AddAttachment(Attachment objAttachment)
        {
            string strExtension = GetFileExtension(objAttachment.FileName);
            if ("EXE SCR VBS COM".IndexOf(strExtension) != -1) // basic safety filtering of risky types
            {
                Attachment objReplacement;
                objReplacement.FileName = Globals.GetNewRandomMid() + ".txt";
                var objEncoder = new ASCIIEncoding();
                objReplacement.Image = Globals.GetBytes("The attachment '" + objAttachment.FileName + "' has been " + "removed from this message. The file type ." + strExtension + " is blocked.");
                aryAttachments.Add(objReplacement);
            }
            else
            {
                aryAttachments.Add(objAttachment);
            }
        } // AddAttachment

        internal bool AnyBcc(string strRecipients)
        {
            var Recipients = strRecipients.Split(';');
            foreach (string recipient in Recipients)
            {
                // Strip the <> brackets (required for some email clients like Thunderbird) 
                string tmpRecipient = recipient.Replace("<", "");
                tmpRecipient = tmpRecipient.Replace(">", "").Trim();
                if (Header.IndexOf(tmpRecipient) == -1)
                    return true; // Not found in header implies Bcc
            }

            return false;
        } // AnyBcc

        internal bool SaveMessageToWinlink()
        {
            // Saves the image in the mime property into the "To Winlink" subdirectory...

            EncodeMime();
            if (!string.IsNullOrEmpty(Mime))
            {
                string strMessageFilePath = Globals.SiteRootDirectory + @"To Winlink\" + MessageId + ".mime";
                File.WriteAllText(strMessageFilePath, Mime);
                MidsSeen.AddMessageId(MessageId);
                LocalDelivery();
                return true;
            }
            else
            {
                ErrorDescription = "Failure to encode mime format";
                return false;
            }
        } // SaveMessage

        internal bool SaveMessageToAccounts()
        {
            EncodeMime();
            if (!string.IsNullOrEmpty(Mime))
            {
                LocalDelivery();
                return true;
            }
            else
            {
                Logs.Exception("[SMTPMessage.SaveMessage] " + MessageId + " empty mime");
                ErrorDescription = "Failure to encode mime format";
                return false;
            }
        } // SaveMessageToAccount

        public string Mime
        {
            get
            {
                return strMime;
            }

            set
            {
                strMime = value;
            }
        } // Mime

        public string Body
        {
            get
            {
                return strBody;
            }

            set
            {
                strBody = value;
            }
        } // Body

        private void LocalDelivery()
        {
            string strTemp = "";
            foreach (WinlinkAddress objAddress in cllToAddresses)
            {
                strTemp = objAddress.RadioAddress;
                if (Accounts.AccountsString.IndexOf(objAddress.RadioAddress) != -1)
                {
                    string strMessageFilePath = Globals.SiteRootDirectory + @"Accounts\" + objAddress.RadioAddress + @"_Account\" + MessageId + ".mime";
                    try
                    {
                        File.WriteAllText(strMessageFilePath, Mime);
                    }
                    catch
                    {
                        Logs.Exception("[SMTPMessage.LocalDelivery] " + Information.Err().Description);
                    }
                }
            }

            foreach (WinlinkAddress objAddress in cllCcAddresses)
            {
                strTemp = objAddress.RadioAddress;
                if (Accounts.AccountsString.IndexOf(objAddress.RadioAddress) != -1)
                {
                    string strMessageFilePath = Globals.SiteRootDirectory + @"Accounts\" + objAddress.RadioAddress + @"_Account\" + MessageId + ".mime";
                    try
                    {
                        File.WriteAllText(strMessageFilePath, Mime);
                    }
                    catch
                    {
                        Logs.Exception("[SMTPMessage.LocalDelivery] " + Information.Err().Description);
                    }
                }
            }
        } // LocalDelivery

        private bool DecodeMime()
        {
            // Decodes the mime image saved in Mime above...
            if (ExtractHeader() == false)
            {
                ErrorDescription = "Unable to extract the message header";
                return false;
            }

            var objMimeDecoder = new MimeDecoder();
            if (objMimeDecoder.DecodeMime(Mime))
            {
                DecodeHeader();
                Body = objMimeDecoder.Body;
                aryAttachments.Clear();
                foreach (Attachment stcAttachment in objMimeDecoder.AttachmentCollection)
                    aryAttachments.Add(stcAttachment);
                return true;
            }
            else
            {
                return false;
            }
        } // DecodeMime

        private bool DecodeHeader()
        {
            // Decodes the mime header and fills the public properties with the 
            // result. (called by DecodeMime)...

            bool blnValidSender = false;
            cllToAddresses = new Collection();
            cllCcAddresses = new Collection();
            string strPreDecode = Header.Replace("," + Globals.CRLF, ";");
            var objTextStream = new StringReader(strPreDecode + Globals.CRLF + Globals.CRLF);
            do
            {
                string strLine = objTextStream.ReadLine();
                if (string.IsNullOrEmpty(strLine))
                    break;
                string strLineUpper = strLine.ToUpper();
                if (strLineUpper.StartsWith("FROM:"))
                {
                    if (FromClient)
                    {
                        if (strLineUpper.IndexOf("@WINLINK.ORG") != -1)
                            blnValidSender = true;
                    }
                    else
                    {
                        blnValidSender = true;
                    }

                    Sender.RadioAddress = Strings.Trim(strLine.Substring(5));
                }
                else if (strLineUpper.StartsWith("SENDER:"))
                {
                    Sender.RadioAddress = Strings.Trim(strLine.Substring(7));
                }
                else if (strLineUpper.StartsWith("TO:"))
                {
                    strLine = Strings.Trim(strLine.Substring(3));
                    var strTo = strLine.Split(",;".ToCharArray());
                    foreach (string strToAddress in strTo)
                        AddAddress(strToAddress.Trim());
                }
                else if (strLineUpper.StartsWith("CC:"))
                {
                    strLine = Strings.Trim(strLine.Substring(3));
                    var strCc = strLine.Split(",;".ToCharArray());
                    foreach (string strCcAddress in strCc)
                        AddAddress(strCcAddress.Trim(), true);
                }
                else if (strLineUpper.StartsWith("SUBJECT:"))
                {
                    Subject = Strings.Trim(strLine.Substring(8));
                    if (string.IsNullOrEmpty(Subject))
                        Subject = "---";
                }
                else if (strLineUpper.StartsWith("MESSAGE-ID:"))
                {
                    MessageId = Strings.Trim(strLine.Substring(11));
                }
                else if (strLineUpper.StartsWith("DATE:"))
                {
                    MessageDate = RFC822DateToDate(Strings.Trim(strLine.Substring(5)));
                }
            }
            while (true);
            if (MessageId.Length < 4 | MessageId.Length > 12)
                MessageId = Globals.GetNewRandomMid();
            return blnValidSender;
        } // DecodeHeader

        private string GetFileExtension(string strFileName)
        {
            var strTokens = strFileName.Split('.');
            return strTokens[strTokens.Length - 1].ToUpper();
        } // GetFileExtension

        private void AddAddress(string strAddress, bool blnCc = false)
        {
            // Add a destination address to the message making sure there is
            // no duplicate entry...

            if (!string.IsNullOrEmpty(strAddress))
            {
                bool blnToExists = false;
                bool blnCcExists = false;
                var objAddress = new WinlinkAddress(strAddress);
                if (!string.IsNullOrEmpty(objAddress.RadioAddress))
                {

                    // See if the address is already in the "To" collection...
                    foreach (WinlinkAddress objToAddress in cllToAddresses)
                    {
                        if ((objAddress.RadioAddress ?? "") == (objToAddress.RadioAddress ?? ""))
                        {
                            blnToExists = true;
                            break;
                        }
                    }

                    // See if the address is already in the "Cc" collection...
                    foreach (WinlinkAddress objCcAddress in cllCcAddresses)
                    {
                        if ((objAddress.RadioAddress ?? "") == (objCcAddress.RadioAddress ?? ""))
                        {
                            blnCcExists = true;
                            break;
                        }
                    }

                    // If a duplicate address is in the "Cc" collection and the new address is 
                    // being posted to the "To" collection remove it from the "Cc" collection...
                    if (blnCcExists & !blnCc)
                    {
                        for (int intIndex = 1, loopTo = cllCcAddresses.Count; intIndex <= loopTo; intIndex++)
                        {
                            WinlinkAddress objCcAddress = (WinlinkAddress)cllCcAddresses[intIndex];
                            if ((objAddress.RadioAddress ?? "") == (objCcAddress.RadioAddress ?? ""))
                            {
                                cllCcAddresses.Remove(intIndex);
                                blnCcExists = false;
                                break;
                            }
                        }
                    }

                    // Add the address to the "To" or "Cc" collection if no duplicate address
                    // would result...
                    if (!blnCc)
                    {
                        if (!blnToExists)
                            cllToAddresses.Add(objAddress);
                    }
                    else if (!blnToExists & !blnCcExists)
                        cllCcAddresses.Add(objAddress);
                }
            }
        } // AddAddress

        private ArrayList GetAttachments
        {
            get
            {
                return aryAttachments;
            }
        } // GetAttachments

        private bool EncodeMime()
        {
            // Encodes the mime image from the public properties...

            EncodeHeader();
            var objMimeEncoder = new MimeEncoder();
            objMimeEncoder.MessageId = MessageId;
            objMimeEncoder.AddMessageBody(Body);
            objMimeEncoder.Header = Header;
            objMimeEncoder.Attachments = GetAttachments;
            if (objMimeEncoder.EncodeMime())
            {
                Mime = string.Copy(objMimeEncoder.Mime);
                return true;
            }
            else
            {
                return false;
            }
        } // EncodeMime

        private bool ExtractHeader()
        {
            if (Mime.Length < 20)
                return false;
            var objStringReader = new StringReader(Mime + Globals.CRLF + Globals.CRLF);
            var sbdHeader = new StringBuilder();
            string strLine;
            string strNextParameter = "";
            do
            {
                strLine = objStringReader.ReadLine();
                if (string.IsNullOrEmpty(strLine))
                {
                    sbdHeader.Append(strNextParameter + Globals.CRLF);
                    Header = sbdHeader.ToString();
                    return true;
                }

                if (string.IsNullOrEmpty(strNextParameter))
                {
                    strNextParameter += strLine;
                    continue;
                }

                if (strLine[0] < '!')
                {
                    strNextParameter += strLine.Trim();
                }
                else
                {
                    sbdHeader.Append(strNextParameter + Globals.CRLF);
                    strNextParameter = strLine;
                }
            }
            while (true);
        } // ExtractHeader

        private void EncodeHeader()
        {
            // Encodes a mime header from the public properties (called by EncodeMime())...

            var sbdHeader = new StringBuilder();
            int intIndex;
            if (Information.IsDate(MessageDate))
            {
                sbdHeader.Append("Date: " + DateToRFC822Date(MessageDate) + Globals.CRLF);
            }
            else
            {
                sbdHeader.Append("Date: " + DateToRFC822Date(DateTime.UtcNow) + Globals.CRLF);
            }

            if (!string.IsNullOrEmpty(Sender.SMTPAddress))
            {
                sbdHeader.Append("From: " + Sender.SMTPAddress + Globals.CRLF);
                if (string.IsNullOrEmpty(ReplyTo.RadioAddress))
                {
                    sbdHeader.Append("Reply-To: " + Sender.SMTPAddress + Globals.CRLF);
                }
                else
                {
                    sbdHeader.Append("Reply-To: " + ReplyTo.SMTPAddress + Globals.CRLF);
                }
            }

            if (!string.IsNullOrEmpty(Subject))
                sbdHeader.Append("Subject: " + Subject + Globals.CRLF);
            if (cllToAddresses.Count != 0)
            {
                if (cllToAddresses.Count == 1)
                {
                    sbdHeader.Append("To: " + ((WinlinkAddress)cllToAddresses[1]).SMTPAddress + Globals.CRLF);
                }
                else
                {
                    var loopTo = cllToAddresses.Count;
                    for (intIndex = 1; intIndex <= loopTo; intIndex++)
                    {
                        if (intIndex == 1)
                        {
                            sbdHeader.Append("To: " + ((WinlinkAddress)cllToAddresses[1]).SMTPAddress + "," + Globals.CRLF);
                        }
                        else if (intIndex == cllToAddresses.Count)
                        {
                            sbdHeader.Append(" " + ((WinlinkAddress)cllToAddresses[intIndex]).SMTPAddress + Globals.CRLF);
                        }
                        else
                        {
                            sbdHeader.Append(" " + ((WinlinkAddress)cllToAddresses[intIndex]).SMTPAddress + "," + Globals.CRLF);
                        }
                    }
                }
            }

            if (cllCcAddresses.Count != 0)
            {
                if (cllCcAddresses.Count == 1)
                {
                    sbdHeader.Append("Cc: " + ((WinlinkAddress)cllCcAddresses[1]).SMTPAddress + Globals.CRLF);
                }
                else
                {
                    var loopTo1 = cllCcAddresses.Count;
                    for (intIndex = 1; intIndex <= loopTo1; intIndex++)
                    {
                        if (intIndex == 1)
                        {
                            sbdHeader.Append("Cc: " + ((WinlinkAddress)cllCcAddresses[1]).SMTPAddress + "," + Globals.CRLF);
                        }
                        else if (intIndex == cllCcAddresses.Count)
                        {
                            sbdHeader.Append(" " + ((WinlinkAddress)cllCcAddresses[intIndex]).SMTPAddress + Globals.CRLF);
                        }
                        else
                        {
                            sbdHeader.Append(" " + ((WinlinkAddress)cllCcAddresses[intIndex]).SMTPAddress + "," + Globals.CRLF);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(MessageId))
                sbdHeader.Append("Message-ID: " + MessageId + Globals.CRLF);
            if (dttExpirationDate > dttMessageDate)
            {
                sbdHeader.Append("X-Cancel: " + Globals.FormatDate(dttExpirationDate) + Globals.CRLF);
            }

            sbdHeader.Append("MIME-Version: 1.0" + Globals.CRLF);
            Header = sbdHeader.ToString();
        } // EncodeHeader

        private DateTime RFC822DateToDate(string strDate)
        {
            // This function converts a standard RFC 822 date/time string to a UTC Date type
            // with full correction for UTC to local offset. If the argument does not convert
            // to a proper date then the current UTC date/time is returned...

            if (strDate.IndexOf(",") < 0)
                strDate = "---, " + strDate;
            string strMonths = "   JANFEBMARAPRMAYJUNJULAUGSEPOCTNOVDEC";
            var strDateParts = strDate.Split(' ');
            int intMonth;
            int intOffsetHours = 0;
            int intOffsetMinutes = 0;
            bool blnWest = true;
            try
            {
                intMonth = strMonths.IndexOf(strDateParts[2].ToUpper()) / 3;
                if (intMonth < 1)
                    return DateTime.UtcNow;
                string strNewDate = strDateParts[3] + "/" + intMonth.ToString() + "/" + strDateParts[1] + " " + strDateParts[4];
                DateTime dttDate = Conversions.ToDate(strNewDate);
                var switchExpr = strDateParts[5];
                switch (switchExpr)
                {
                    case "UT":
                    case "GMT":
                    case "Z":
                        {
                            break;
                        }
                    // Do nothing...
                    case "A":
                        {
                            intOffsetHours = 1;
                            break;
                        }

                    case "M":
                        {
                            intOffsetHours = 12;
                            break;
                        }

                    case "N":
                        {
                            blnWest = false;
                            intOffsetHours = 1;
                            break;
                        }

                    case "Y":
                        {
                            blnWest = false;
                            intOffsetHours = 12;
                            break;
                        }

                    case "EDT":
                        {
                            intOffsetHours = 4;
                            break;
                        }

                    case "EST":
                    case "CDT":
                        {
                            intOffsetHours = 5;
                            break;
                        }

                    case "CST":
                    case "MDT":
                        {
                            intOffsetHours = 6;
                            break;
                        }

                    case "MST":
                    case "PDT":
                        {
                            intOffsetHours = 7;
                            break;
                        }

                    case "PST":
                        {
                            intOffsetHours = 8;
                            break;
                        }

                    default:
                        {
                            if (strDateParts[5].Substring(0, 1) != "-")
                                blnWest = false;
                            intOffsetHours = Convert.ToInt32(strDateParts[5].Substring(1, 2));
                            intOffsetMinutes = Convert.ToInt32(strDateParts[5].Substring(3, 2));
                            break;
                        }
                }

                if (blnWest)
                {
                    dttDate = dttDate.AddHours(intOffsetHours);
                    dttDate = dttDate.AddMinutes(intOffsetMinutes);
                }
                else
                {
                    dttDate = dttDate.AddHours(-intOffsetHours);
                    dttDate = dttDate.AddMinutes(-intOffsetMinutes);
                }

                return dttDate;
            }
            catch
            {
                Logs.Exception("[2837] " + strDate + " " + Information.Err().Description);
                return DateTime.UtcNow;
            }
        } // RFC822DateToDate

        private string DateToRFC822Date(DateTime dtUTCDate)
        {
            // This function converts a Date type to a standard RFC 822 date string. The date
            // arguement must be in UTC...

            string sDays = "SunMonTueWedThuFriSat";
            string sMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";
            string sDay;
            string sMonth;
            sDay = sDays.Substring(3 * (int)dtUTCDate.DayOfWeek, 3) + ", ";
            sMonth = " " + sMonths.Substring(3 * (dtUTCDate.Month - 1), 3) + " ";
            return sDay + Strings.Format(dtUTCDate, "dd") + sMonth + Strings.Format(dtUTCDate, "yyyy") + " " + Strings.Format(dtUTCDate, "HH:mm:ss") + " -0000";
        } // DateToRFC822Date

        private string FormatMessageBody(string strSender, string strBody)
        {
            // Remove excessive blank lines from inbound message body...

            // Remove excess blank lines...
            bool blnBlankLine = true;
            var objBodyStream = new StringReader(strBody);
            var sbdBody = new StringBuilder();
            do
            {
                string strLine = objBodyStream.ReadLine();
                if (strLine == null)
                    break;
                if (!string.IsNullOrEmpty(strLine.Trim()))
                    blnBlankLine = false;
                if (!blnBlankLine)
                    sbdBody.Append(strLine + Globals.CRLF);
                if (string.IsNullOrEmpty(strLine.Trim()))
                {
                    blnBlankLine = true;
                }
            }
            while (true);
            strBody = sbdBody.ToString().Trim();
            if (string.IsNullOrEmpty(strBody))
                strBody = "<no message body>" + Globals.CRLF;
            return strBody;
        } // FormatMessageBody

        public bool IsAnyRecipientMARS()
        {
            // Function to scan TO and CC recipients for valid MARS callsign... 
            string strTemp;
            foreach (WinlinkAddress objAddress in cllToAddresses)
            {
                strTemp = objAddress.RadioAddress;
                if (Globals.IsMARSCallsign(strTemp))
                    return true;
            }

            foreach (WinlinkAddress objAddress in cllCcAddresses)
            {
                strTemp = objAddress.RadioAddress;
                if (Globals.IsMARSCallsign(strTemp))
                    return true;
            }

            return false;
        } // IsAnyRecipientMARS
    } // PaclinkMessage
}