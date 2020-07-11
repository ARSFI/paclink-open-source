using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{

    // Class to hold and process a Winlink message on the channel side...
    internal class Message : IMessage, IComparable
    {
        private enum EB2DecodeState
        {
            Inactive,
            GetStartOfHeader,
            GetHeaderCount,
            GetSubject,
            GetOffset,
            GetStartOfBlock,
            GetBlockCount,
            GetDataBlock,
            GetChecksum
        }

        private const byte NUL = 0;
        private const byte SOH = 1;
        private const byte STX = 2;
        private const byte EOT = 4;
        private const string strBlockedExtensions = "exe pif scr ";
        internal bool IsAccepted = false;
        internal string ErrorDescription = "Unknown mime decode failure";

        // These variables are the basic properties of this class...
        internal string MessageId = "";
        internal DateTime MessageDate;
        internal DateTime ExpirationDate;
        internal WinlinkAddress Sender = new WinlinkAddress("");    // The Sender's address
        internal string Source;
        internal WinlinkAddress ReplyTo = new WinlinkAddress("");
        internal string Subject = "";             // The message subject
        internal string Header = "";              // The header used in mime format

        // Note: the Attachment structure is defined in Global.vb...
        private ArrayList aryAttachments = new ArrayList();   // An array of Attachment structures
        private Collection cllToAddresses = new Collection();  // Collection of address objects
        private Collection cllCcAddresses = new Collection();  // Collection of address objects
        private string strBody = "";            // The body of the message
        private string strMime = "";            // The entire message in mime format

        // Variables used by B2 messages...
        private EB2DecodeState enmState;        // State variable used in decoding B2 images
        private byte[] bytUncompressed;         // Uncompressed copy of message
        private byte[] bytCompressed;           // Compresses copy of message
        private byte[] bytFormatted;            // Copy of message formatted for binary transmission
        private int intProgress;            // Byte counter for binary reception
        public int intB2Checksum;           // Running checksum on received compressed message
        public int intUncompressedSize;   // Size of uncompressed message

        internal Message()
        {
            MessageDate = DateTime.UtcNow;
            ExpirationDate = DateTime.UtcNow.AddDays(21);
            enmState = EB2DecodeState.GetStartOfHeader;
            bytUncompressed = new byte[1];
            bytCompressed = new byte[1];
        } // New - From Winlink

        internal Message(string strFilePath)
        {
            Mime = My.MyProject.Computer.FileSystem.ReadAllText(strFilePath);
            bytUncompressed = new byte[1];
            bytCompressed = new byte[1];
            DecodeMime();
            PackB2Message();
            enmState = EB2DecodeState.Inactive;
        } // New - To Winlink

        public int CompareTo(object objMessage)
        {
            int intSize = ((Message)objMessage).CompressedSize();
            if (intSize == bytCompressed.Length)
                return 0;
            if (intSize > bytCompressed.Length)
                return -1;
            return 1;
        } // CompareTo

        internal bool SaveToFile(string strFilePath)
        {
            if (!EncodeMime())
                return false;
            if (!string.IsNullOrEmpty(Mime))
            {
                My.MyProject.Computer.FileSystem.WriteAllText(strFilePath, Mime, false);
                return true;
            }
            else
            {
                return false;
            }
        } // SaveToFile

        internal void DeleteFile(string strFilePath)
        {
            if (File.Exists(strFilePath))
                File.Delete(strFilePath);
        }

        internal int Size()
        {
            // Returns the actual message size: Header, body, and attachments before
            // encoding as a mime file or compressing as B2 image...

            int intSize = Header.Length + Body.Length;
            foreach (Attachment objAttachment in aryAttachments)
                intSize += objAttachment.Image.Length;
            return intSize;
        } // Size

        internal int CompressedSize()
        {
            if (bytCompressed == null)
                return 0;
            return bytCompressed.Length;
        } // CompressedSize

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
        } // Body

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

        internal bool SaveMessage()
        {
            // Saves the image in the mime property into the "From Winlink" subdirectory...

            EncodeMime();
            if (!string.IsNullOrEmpty(Mime))
            {
                string strMessageFilePath = Globals.SiteRootDirectory + @"From Winlink\" + MessageId + ".mime";
                My.MyProject.Computer.FileSystem.WriteAllText(strMessageFilePath, Mime, false);
                return true;
            }
            else
            {
                Logs.Exception("[Message.SaveMessage] " + MessageId + " empty mime");
                ErrorDescription = "Failure to encode mime format";
                return false;
            }
        } // SaveMessage

        private bool DecodeMime()
        {
            // Decodes the mime image saved in Mime above...
            var objMimeDecoder = new MimeDecoder();
            if (objMimeDecoder.DecodeMime(Mime))
            {
                Header = objMimeDecoder.Header;
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

            cllToAddresses = new Collection();
            cllCcAddresses = new Collection();
            var strHeaderLines = Regex.Split(Header.Replace("," + Globals.CRLF + " ", ";"), Globals.CRLF);
            foreach (string strLine in strHeaderLines)
            {
                var tmpStrLine = strLine;
                if (tmpStrLine.StartsWith("From:"))
                {
                    Sender.RadioAddress = Strings.Trim(tmpStrLine.Substring(5));
                }
                else if (tmpStrLine.StartsWith("Sender:"))
                {
                    Sender.RadioAddress = Strings.Trim(tmpStrLine.Substring(7));
                }
                else if (tmpStrLine.StartsWith("To:"))
                {
                    tmpStrLine = Strings.Trim(tmpStrLine.Substring(3));
                    var strTo = tmpStrLine.Split(",;".ToCharArray());
                    foreach (string strToAddress in strTo)
                        AddAddress(strToAddress);
                }
                else if (tmpStrLine.StartsWith("Cc:"))
                {
                    tmpStrLine = Strings.Trim(tmpStrLine.Substring(3));
                    var strCc = tmpStrLine.Split(",;".ToCharArray());
                    foreach (string strCcAddress in strCc)
                        AddAddress(strCcAddress, true);
                }
                else if (tmpStrLine.StartsWith("Subject:"))
                {
                    Subject = Strings.Trim(tmpStrLine.Substring(8));
                    if (string.IsNullOrEmpty(Subject))
                        Subject = "---";
                }
                else if (tmpStrLine.StartsWith("Message-ID:"))
                {
                    MessageId = Strings.Trim(tmpStrLine.Substring(11));
                }
                else if (tmpStrLine.StartsWith("Date:"))
                {
                    MessageDate = Globals.RFC822DateToDate(Strings.Trim(tmpStrLine.Substring(5)));
                }
            }

            if (MessageId.Length < 4 | MessageId.Length > 12)
                MessageId = Globals.GetNewRandomMid();
            return true;
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

        internal int Progress
        {
            get
            {
                return intProgress;
            }
        } // Progress

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
            var objStringReader = new StringReader(Mime);
            var sbdHeader = new StringBuilder();
            string strLine;
            do
            {
                strLine = objStringReader.ReadLine();
                if (strLine == null)
                    return false;
                if (string.IsNullOrEmpty(strLine))
                {
                    Header = sbdHeader.ToString();
                    return true;
                }

                sbdHeader.Append(strLine + Globals.CRLF);
            }
            while (true);
            return false;
        } // ExtractHeader

        private void EncodeHeader()
        {
            // Encodes a mime header from the public properties (called by EncodeMime())...

            var sbdHeader = new StringBuilder();
            int intIndex;
            if (Information.IsDate(MessageDate))
            {
                sbdHeader.Append("Date: " + Globals.DateToRFC822Date(MessageDate) + Globals.CRLF);
            }
            else
            {
                sbdHeader.Append("Date: " + Globals.DateToRFC822Date(DateTime.UtcNow) + Globals.CRLF);
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
            if (ExpirationDate > MessageDate)
            {
                sbdHeader.Append("X-Cancel: " + Globals.FormatDate(ExpirationDate) + Globals.CRLF);
            }

            sbdHeader.Append("MIME-Version: 1.0" + Globals.CRLF);
            Header = sbdHeader.ToString();
        } // EncodeHeader

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

        internal bool Decompress(bool blnCRC)
        {
            var objCompression = new Compression();
            try
            {
                if (Compression.Decode(bytCompressed, ref bytUncompressed, blnCRC, intUncompressedSize) > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        } // Decompress

        internal bool Compress(bool blnCRC)
        {
            var objCompression = new Compression();
            try
            {
                if (Compression.Encode(bytUncompressed, ref bytCompressed, blnCRC) > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        } // Compress

        // This function receives a binary byte stream (byte at a time) from a B2 channel
        // and decompresses and decodes the B2 message. Return values:
        // 2 = End of individual block
        // 1 = End of message - successfully decoded and ready to use or save
        // 0 = In progress - no errors so far
        // -1 = Received data stream not a correct format
        // -2 = Check sum failure - failed to assemble correct binary image
        private StringBuilder sbdSubject = new StringBuilder();
        private string strOffset = null;
        private int intHeaderSize = 0;
        private int intBlockSize = 0;
        private int intArraySize = 100000;
        internal int BinaryInput(byte bytSingle)    // String version of the message subject
                                                    // String version of Offset to include in the header
                                                    // The current header size
                                                    // The current block size
                                                    // The initial size of the binary array buffer
        {
            // BinaryCount += 1
            // Disassembles the B2 binary byte stream, byte by byte...
            var switchExpr = enmState;
            switch (switchExpr)
            {
                case EB2DecodeState.GetStartOfHeader:
                    {
                        if (bytSingle == SOH)
                        {
                            sbdSubject.Length = 0;
                            strOffset = "";
                            intB2Checksum = 0;
                            intProgress = 0;
                            bytCompressed = new byte[intArraySize + 1];
                            enmState = EB2DecodeState.GetHeaderCount;
                        }

                        break;
                    }

                case EB2DecodeState.GetHeaderCount:
                    {
                        intHeaderSize = bytSingle;
                        enmState = EB2DecodeState.GetSubject;
                        break;
                    }

                case EB2DecodeState.GetSubject:
                    {
                        if (bytSingle != NUL)
                        {
                            sbdSubject.Append((char)bytSingle);
                        }
                        else
                        {
                            Subject = sbdSubject.ToString();
                            if (string.IsNullOrEmpty(Subject))
                                Subject = "---";
                            enmState = EB2DecodeState.GetOffset;
                        }

                        break;
                    }

                case EB2DecodeState.GetOffset:
                    {
                        if (bytSingle != NUL)
                        {
                            strOffset += Conversions.ToString((char)bytSingle);
                        }
                        else
                        {
                            enmState = EB2DecodeState.GetStartOfBlock;
                            return 2;
                        }

                        break;
                    }

                case EB2DecodeState.GetStartOfBlock:
                    {
                        if (bytSingle == STX)
                        {
                            enmState = EB2DecodeState.GetBlockCount;
                        }
                        else if (bytSingle == EOT)
                        {
                            enmState = EB2DecodeState.GetChecksum;
                        }
                        else
                        {
                            return -1;
                        }

                        break;
                    }

                case EB2DecodeState.GetBlockCount:
                    {
                        intBlockSize = bytSingle;
                        if (intBlockSize == 0)
                            intBlockSize = 256;
                        enmState = EB2DecodeState.GetDataBlock;
                        break;
                    }

                case EB2DecodeState.GetDataBlock:
                    {
                        bytCompressed[intProgress] = bytSingle;
                        intB2Checksum += bytSingle;
                        intProgress += 1;
                        if (intProgress > Information.UBound(bytCompressed))
                        {
                            intArraySize += 10000;
                            Array.Resize(ref bytCompressed, intArraySize + 1);
                        }

                        intBlockSize -= 1;
                        if (intBlockSize == 0)
                        {
                            enmState = EB2DecodeState.GetStartOfBlock;
                            return 2;
                        }
                        else
                        {
                            return 0;
                        }

                        break;
                    }

                case EB2DecodeState.GetChecksum:
                    {
                        Array.Resize(ref bytCompressed, intProgress);
                        intB2Checksum = (intB2Checksum & 0xFF) * -1 & 0xFF;
                        if (bytSingle == intB2Checksum)
                        {
                            return 1;
                        }
                        else
                        {
                            return -2;
                        }

                        break;
                    }
            }

            return default;
        } // BinaryInput

        internal string B2Proposal()
        {
            // Returns a formatted proposal string for an B2 binary transmission
            // Example "FC EM 12345_W1ABC 456 234 0"
            // Returns an empty string if unable to prepare the message for B2 transmission...
            if (MessageId.Length != 0)
            {
                if (PackB2Message() == true)
                {
                    return "FC EM " + MessageId + " " + bytUncompressed.Length.ToString() + " " + bytCompressed.Length.ToString() + " 0";
                }
            }

            return "";
        } // B2Proposal

        internal byte[] B2Output(int intOffset)
        {
            // Returns a byte array containing the message in B2-protocol transmission format...
            if (FormatForBinaryTransmission(intOffset) == true)
            {
                return bytFormatted;
            }
            else
            {
                var bytBuffer = new byte[0];
                return bytBuffer;
            }
        } // B2Output

        public bool DecodeB2Message()
        {
            // This function parses the binary array that holds the uncompressed B2 format
            // received message and separates out any file attachments. Returns True _
            // if successful, False if not...

            if (Information.UBound(bytCompressed) < 10)
                return false;
            try
            {
                var intHeader = default(int);
                var intBody = default(int);
                var sbdInput = new StringBuilder();
                var cllAttachmentNames = new Collection();
                var cllAttachmentSizes = new Collection();

                // Decode the B2 header...
                foreach (var bytSingle in bytUncompressed)
                {
                    intHeader += 1;
                    if (bytSingle != 10)
                    {
                        if (bytSingle == 13)
                        {
                            string strHeaderLine = sbdInput.ToString().ToUpper();
                            if (string.IsNullOrEmpty(strHeaderLine))
                            {
                                break;
                            }
                            else if (strHeaderLine.IndexOf("DATE:") == 0)
                            {
                                string strDateTime = strHeaderLine.Substring(5);
                                strDateTime = Strings.Replace(strDateTime, ".", "/");
                                strDateTime = Strings.Replace(strDateTime, "-", "/");
                                strDateTime = Globals.ReformatDate(strDateTime);
                                try
                                {
                                    MessageDate = Conversions.ToDate(strDateTime);
                                }
                                catch
                                {
                                    MessageDate = DateTime.UtcNow;
                                }
                            }
                            else if (strHeaderLine.IndexOf("MID:") == 0)
                            {
                                MessageId = Strings.Trim(strHeaderLine.Substring(4));
                            }
                            else if (strHeaderLine.IndexOf("FROM:") == 0)
                            {
                                Sender.RadioAddress = Strings.Trim(sbdInput.ToString().Substring(5));
                            }
                            else if (strHeaderLine.IndexOf("TO:") == 0)
                            {
                                var objWinlinkAddress = new WinlinkAddress(sbdInput.ToString().Substring(3).Trim());
                                cllToAddresses.Add(objWinlinkAddress);
                            }
                            else if (strHeaderLine.IndexOf("CC:") == 0)
                            {
                                cllCcAddresses.Add(new WinlinkAddress(Strings.Trim(sbdInput.ToString().Substring(3))));
                            }
                            else if (strHeaderLine.IndexOf("BODY:") == 0)
                            {
                                intBody = Convert.ToInt32(strHeaderLine.Substring(5));
                            }
                            else if (strHeaderLine.IndexOf("MBO:") == 0)
                            {
                                Source = Strings.Trim(strHeaderLine.Substring(4));
                            }
                            else if (strHeaderLine.IndexOf("FILE:") == 0)
                            {
                                var strTokens = strHeaderLine.Split(' ');
                                cllAttachmentNames.Add(Strings.Trim(sbdInput.ToString().Substring(7 + strTokens[1].Length)));
                                cllAttachmentSizes.Add(Convert.ToInt32(strTokens[1]));
                            }

                            sbdInput.Length = 0;
                        }
                        else
                        {
                            sbdInput.Append((char)bytSingle);
                        }
                    }
                }

                // Parse out the message body from the uncompressed binary array...
                sbdInput.Length = 0;
                int intIndex;
                var loopTo = intHeader + intBody;
                for (intIndex = intHeader + 1; intIndex <= loopTo; intIndex++)
                    sbdInput.Append((char)bytUncompressed[intIndex]);
                Body = sbdInput.ToString();

                // Return if there are no attachments...
                if (cllAttachmentNames.Count == 0)
                    return true;

                // Parse out the attachments from the uncompressed binary array...
                for (int intCount = 1, loopTo1 = cllAttachmentNames.Count; intCount <= loopTo1; intCount++)
                {
                    var stcAttachment = new Attachment();
                    stcAttachment.FileName = Conversions.ToString(cllAttachmentNames[intCount]);
                    stcAttachment.Image = new byte[(Convert.ToInt32(cllAttachmentSizes[intCount]))];
                    intIndex += 2;
                    for (int intPosition = 0, loopTo2 = Convert.ToInt32(cllAttachmentSizes[intCount]) - 1; intPosition <= loopTo2; intPosition++)
                    {
                        stcAttachment.Image[intPosition] = bytUncompressed[intIndex];
                        intIndex += 1;
                    }

                    aryAttachments.Add(stcAttachment);
                }
            }
            catch
            {
                Logs.Exception("[Message.DecodeB2Message] " + Information.Err().Description);
                return false;
            }

            return true;
        } // DecodeB2Message

        private bool PackB2Message()
        {
            // Prepares a message for B2 transmission. Creates a B2 header,
            // adds the body and attachments and compresses the result and
            // stores in in bytCompressed...
            try
            {
                var sbdMessage = new StringBuilder();
                sbdMessage.Append("MID: " + MessageId + Globals.CRLF);
                sbdMessage.Append("Date: " + Globals.FormatDate(MessageDate) + Globals.CRLF);
                sbdMessage.Append("Type: Private" + Globals.CRLF);
                sbdMessage.Append("From: " + Sender.RadioAddress + Globals.CRLF);

                // Add the destinations to the header...
                foreach (WinlinkAddress objAddress in cllToAddresses)
                    sbdMessage.Append("To: " + objAddress.RadioAddress + Globals.CRLF);
                foreach (WinlinkAddress objAddress in cllCcAddresses)
                    sbdMessage.Append("Cc: " + objAddress.RadioAddress + Globals.CRLF);

                // Add the subject and originating MBO to the header...
                sbdMessage.Append("Subject: " + Subject + Globals.CRLF);
                sbdMessage.Append("Mbo: " + Globals.SiteCallsign + Globals.CRLF);

                // Add the message body size to the header...
                sbdMessage.Append("Body: " + Body.Length.ToString() + Globals.CRLF);

                // Add the attachment sizes and names to the header...
                foreach (Attachment stcAttachment in aryAttachments)
                    sbdMessage.Append("File: " + stcAttachment.Image.Length.ToString() + " " + stcAttachment.FileName + Globals.CRLF);

                // Add the message body...
                sbdMessage.Append(Globals.CRLF + Body);

                // Convert header and body to a binary image...
                bytUncompressed = new byte[1];
                var objEncoder = new ASCIIEncoding();
                bytUncompressed = Globals.GetBytes(sbdMessage.ToString());

                // Add the attachments to the binary image...
                if (aryAttachments.Count > 0)
                {
                    int nNext = bytUncompressed.Length;

                    // Calculate the additional the number of additional bytes required...
                    int nFinalBinSize = bytUncompressed.Length + 2;
                    foreach (Attachment stcAttachment in aryAttachments)
                        nFinalBinSize += 2 + stcAttachment.Image.Length;

                    // Expand the array...
                    Array.Resize(ref bytUncompressed, nFinalBinSize + 1);

                    // Add the attachment image(s) to the byte array...
                    foreach (Attachment stcAttachment in aryAttachments)
                    {
                        // Add a CRLF separator...
                        bytUncompressed[nNext] = 13;
                        nNext += 1;
                        bytUncompressed[nNext] = 10;
                        nNext += 1;

                        // Add the attachment binary image...
                        foreach (byte b in stcAttachment.Image)
                        {
                            bytUncompressed[nNext] = b;
                            nNext += 1;
                        }
                    }

                    // End the array with CRLF...
                    bytUncompressed[nNext] = 13;
                    nNext += 1;
                    bytUncompressed[nNext] = 10;
                }

                // Compress the final binary image...
                if (Compress(true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        } // PackB2Message

        private bool FormatForBinaryTransmission(int intOffset)
        {
            // This function creates the final packetized image for binary transmission
            // including any offset required for a partially transmitted message...
            var intPosition = default(int); // Next position to write into the formatted binary array
            int intProgress; // Next position to read from the compressed binary array
            var intCheckSum = default(int); // Running accumulated checksum on the compressed binary array

            // Calculate the number of data packets in the formatted binary array...
            int intPackets = (bytCompressed.Length - intOffset) / 250 + 1;
            int intRemainder = (bytCompressed.Length - intOffset) % 250;

            // Calculate the upper bound of the formatted binary array...
            string strOffset = intOffset.ToString();
            int intFormattedUpperBound;

            // Set the starting point for the compressed message...
            intProgress = intOffset;

            // Create the blocks to send...
            if (intRemainder != 0)
            {
                intFormattedUpperBound = 6 + Subject.Length + intPackets * 2 + strOffset.Length + Information.UBound(bytCompressed) - intOffset;
            }
            else
            {
                intFormattedUpperBound = 4 + Subject.Length + intPackets * 2 + strOffset.Length + Information.UBound(bytCompressed) - intOffset;
            }

            if (intOffset > 0)
                intFormattedUpperBound += 8;
            bytFormatted = new byte[intFormattedUpperBound + 1];
            bytFormatted[intPosition] = SOH;
            intPosition += 1;
            bytFormatted[intPosition] = Conversions.ToByte(Subject.Length + strOffset.Length + 2);
            intPosition += 1;
            foreach (char c in Subject)
            {
                bytFormatted[intPosition] = Conversions.ToByte(Strings.Asc(c));
                intPosition += 1;
            }

            bytFormatted[intPosition] = NUL;
            intPosition += 1;
            foreach (char c in strOffset)
            {
                bytFormatted[intPosition] = Conversions.ToByte(Strings.Asc(c));
                intPosition += 1;
            }

            bytFormatted[intPosition] = NUL;
            intPosition += 1;

            // If this is a partial transmission then create a leading STX block
            // with the first six bytes of the compressed file...
            if (intOffset > 0)
            {
                bytFormatted[intPosition] = STX;
                intPosition += 1;
                bytFormatted[intPosition] = 6;
                intPosition += 1;
                for (int intIndex = 0; intIndex <= 5; intIndex++)
                {
                    bytFormatted[intPosition] = bytCompressed[intIndex];
                    intPosition += 1;
                    intCheckSum += bytCompressed[intIndex];
                }
            }

            // Format the compressed file into STX blocks beginning with the offset if any...
            try
            {
                do
                {
                    int intSTXBlockByteCount;
                    if (!(intPackets == 1 & intRemainder == 0))
                    {
                        bytFormatted[intPosition] = STX;
                        intPosition += 1;
                        if (intPackets > 1)
                        {
                            // The byte count is 250 if there are more than 250 bytes remaining...
                            bytFormatted[intPosition] = 250;
                            intSTXBlockByteCount = 250 - 1;
                        }
                        else
                        {
                            // The byte count is the remaining bytes if 250 or less are remaining
                            bytFormatted[intPosition] = Conversions.ToByte(intRemainder);
                            intSTXBlockByteCount = intRemainder - 1;
                        }

                        intPackets -= 1;
                        intPosition += 1;

                        // Transfer data bytes to formatted byte array for transmission...
                        for (int intIndex = 0, loopTo = intSTXBlockByteCount; intIndex <= loopTo; intIndex++)
                        {
                            bytFormatted[intPosition] = bytCompressed[intProgress];
                            intCheckSum += bytCompressed[intProgress];
                            intPosition += 1;
                            intProgress += 1;
                        }
                    }
                    else
                    {
                        intPackets = 0;
                    }

                    // If no remaining data is available then add an EOT and the check sum
                    // to the formatted byte array...
                    if (intPackets == 0)
                    {
                        bytFormatted[intPosition] = EOT;
                        intPosition += 1;
                        bytFormatted[intPosition] = Conversions.ToByte((intCheckSum & 0xFF) * -1 & 0xFF);
                        if (intPosition == Information.UBound(bytFormatted))
                        {
                            return true;
                        }
                        else
                        {
                            Logs.Exception("[RMSLiteMessage.FormatForBinaryTransmission] IntPackets = " + intPackets.ToString());
                            return false;
                        }
                    }
                }
                while (true);
            }
            catch
            {
                Logs.Exception("[RMSLiteMessage.FormatForBinaryTransmission] " + Information.Err().Description);
                return false;
            }
        } // FormatForBinaryTransmission
    } // PaclinkMessage
}