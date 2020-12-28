using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using Paclink.Data;

namespace Paclink
{

    // Class to process an inbound messages from the STMP side...
    internal class SMTPMessage : IMessage
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        internal bool IsAccepted;
        internal string ErrorDescription = "Unknown mime decode failure";

        // These variables are the basic properties of this class...
        internal string MessageId = "";
        internal DateTime MessageDate;
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
        private DateTime dttMessageDate = new DateTime();
        private DateTime dttExpirationDate = new DateTime();
        private List<WinlinkAddress> cllToAddresses = new List<WinlinkAddress>();  // Collection of address objects
        private List<WinlinkAddress> cllCcAddresses = new List<WinlinkAddress>();  // Collection of address objects

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
            aryAttachments.Add(objAttachment);
        }

        internal bool SaveMessageToWinlink()
        {
            // Saves the image in the mime property into the "To Winlink" table...

            EncodeMime();
            if (!string.IsNullOrEmpty(Mime))
            {
                var messageStore = new MessageStore(DatabaseFactory.Get());
                messageStore.SaveToWinlinkMessage(MessageId, UTF8Encoding.UTF8.GetBytes(Mime));
                messageStore.AddMessageIdSeen(MessageId);
                LocalDelivery();
                return true;
            }

            ErrorDescription = "Failure to encode mime format";
            return false;
        }

        internal bool SaveMessageToAccounts()
        {
            EncodeMime();
            if (!string.IsNullOrEmpty(Mime))
            {
                LocalDelivery();
                return true;
            }

            _log.Error("[SMTPMessage.SaveMessage] " + MessageId + " empty mime");
            ErrorDescription = "Failure to encode mime format";
            return false;
        }

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
        }

        private void LocalDelivery()
        {
            var messageStore = new MessageStore(DatabaseFactory.Get());

            foreach (WinlinkAddress objAddress in cllToAddresses)
            {
                if (Accounts.AccountsString.IndexOf(objAddress.RadioAddress) != -1)
                {
                    try
                    {
                        messageStore.SaveAccountMessage(MessageId, objAddress.RadioAddress, UTF8Encoding.UTF8.GetBytes(Mime));
                    }
                    catch (Exception e)
                    {
                        _log.Error("[SMTPMessage.LocalDelivery] " + e.Message);
                    }
                }
            }

            foreach (WinlinkAddress objAddress in cllCcAddresses)
            {
                if (Accounts.AccountsString.IndexOf(objAddress.RadioAddress) != -1)
                {
                    try
                    {
                        messageStore.SaveAccountMessage(MessageId, objAddress.RadioAddress, UTF8Encoding.UTF8.GetBytes(Mime));
                    }
                    catch (Exception e)
                    {
                        _log.Error("[SMTPMessage.LocalDelivery] " + e.Message);
                    }
                }
            }
        }

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

            return false;
        }

        private bool DecodeHeader()
        {
            // Decodes the mime header and fills the public properties with the 
            // result. (called by DecodeMime)...

            bool blnValidSender = false;
            cllToAddresses = new List<WinlinkAddress>();
            cllCcAddresses = new List<WinlinkAddress>();
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

                    Sender.RadioAddress = strLine.Substring(5).Trim();
                }
                else if (strLineUpper.StartsWith("SENDER:"))
                {
                    Sender.RadioAddress = strLine.Substring(7).Trim();
                }
                else if (strLineUpper.StartsWith("TO:"))
                {
                    strLine = strLine.Substring(3).Trim();
                    var strTo = strLine.Split(",;".ToCharArray());
                    foreach (string strToAddress in strTo)
                        AddAddress(strToAddress.Trim());
                }
                else if (strLineUpper.StartsWith("CC:"))
                {
                    strLine = strLine.Substring(3).Trim();
                    var strCc = strLine.Split(",;".ToCharArray());
                    foreach (string strCcAddress in strCc)
                        AddAddress(strCcAddress.Trim(), true);
                }
                else if (strLineUpper.StartsWith("SUBJECT:"))
                {
                    Subject = strLine.Substring(8).Trim();
                    if (string.IsNullOrEmpty(Subject))
                        Subject = "---";
                }
                else if (strLineUpper.StartsWith("MESSAGE-ID:"))
                {
                    MessageId = strLine.Substring(11).Trim();
                }
                else if (strLineUpper.StartsWith("DATE:"))
                {
                    MessageDate = RFC822DateToDate(strLine.Substring(5).Trim());
                }
            }
            while (true);
            if (MessageId.Length < 4 | MessageId.Length > 12)
                MessageId = Globals.GetNewRandomMid();
            return blnValidSender;
        }

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
                            WinlinkAddress objCcAddress = cllCcAddresses[intIndex];
                            if ((objAddress.RadioAddress ?? "") == (objCcAddress.RadioAddress ?? ""))
                            {
                                cllCcAddresses.RemoveAt(intIndex);
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
        } 

        private ArrayList GetAttachments => aryAttachments;

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
                Mime = objMimeEncoder.Mime;
                return true;
            }

            return false;
        } 

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
        } 

        private void EncodeHeader()
        {
            // Encodes a mime header from the public properties (called by EncodeMime())...

            var sbdHeader = new StringBuilder();
            int intIndex;
            sbdHeader.Append("Date: " + DateToRFC822Date(MessageDate) + Globals.CRLF);
            
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
                    sbdHeader.Append("To: " + cllToAddresses[0].SMTPAddress + Globals.CRLF);
                }
                else
                {
                    var loopTo = cllToAddresses.Count;
                    for (intIndex = 0; intIndex < loopTo; intIndex++)
                    {
                        if (intIndex == 0)
                        {
                            sbdHeader.Append("To: " + cllToAddresses[0].SMTPAddress + "," + Globals.CRLF);
                        }
                        else if (intIndex == cllToAddresses.Count - 1)
                        {
                            sbdHeader.Append(" " + cllToAddresses[intIndex].SMTPAddress + Globals.CRLF);
                        }
                        else
                        {
                            sbdHeader.Append(" " + cllToAddresses[intIndex].SMTPAddress + "," + Globals.CRLF);
                        }
                    }
                }
            }

            if (cllCcAddresses.Count != 0)
            {
                if (cllCcAddresses.Count == 1)
                {
                    sbdHeader.Append("Cc: " + cllCcAddresses[0].SMTPAddress + Globals.CRLF);
                }
                else
                {
                    var loopTo1 = cllCcAddresses.Count;
                    for (intIndex = 0; intIndex < loopTo1; intIndex++)
                    {
                        if (intIndex == 0)
                        {
                            sbdHeader.Append("Cc: " + cllCcAddresses[0].SMTPAddress + "," + Globals.CRLF);
                        }
                        else if (intIndex == cllCcAddresses.Count - 1)
                        {
                            sbdHeader.Append(" " + cllCcAddresses[intIndex].SMTPAddress + Globals.CRLF);
                        }
                        else
                        {
                            sbdHeader.Append(" " + cllCcAddresses[intIndex].SMTPAddress + "," + Globals.CRLF);
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
        } 

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
                DateTime dttDate = DateTime.Parse(strNewDate);
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
            catch (Exception e)
            {
                _log.Error("[2837] " + strDate + " " + e.Message);
                return DateTime.UtcNow;
            }
        } // RFC822DateToDate

        private string DateToRFC822Date(DateTime dtUTCDate)
        {
            // This function converts a Date type to a standard RFC 822 date string. The date
            // argument must be in UTC...

            string sDays = "SunMonTueWedThuFriSat";
            string sMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";
            string sDay;
            string sMonth;
            sDay = sDays.Substring(3 * (int)dtUTCDate.DayOfWeek, 3) + ", ";
            sMonth = " " + sMonths.Substring(3 * (dtUTCDate.Month - 1), 3) + " ";
            return sDay + dtUTCDate.ToString("dd") + sMonth + dtUTCDate.ToString("yyyy") + " " + dtUTCDate.ToString("HH:mm:ss") + " -0000";
        } // DateToRFC822Date

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
        } 

    } 
}