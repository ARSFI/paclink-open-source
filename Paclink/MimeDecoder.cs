using System.Collections;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using MimeKit;

namespace Paclink
{
    public class MimeDecoder
    {
        private ArrayList aryAttachments = new ArrayList(); // a collection of aryAttachments, one element for each attachment
        private string strMessageBody;  // The Decoded message body as a string
        private string strMessageHeader; // The mime Header (shows Date, To, Cc, Reply To etc)

        public string Header
        {
            // Returns the message header...
            get
            {
                return strMessageHeader;
            }
        }

        public string Body
        {
            // Returns the message body...
            get
            {
                return strMessageBody;
            }
        }

        public int AttachmentCount
        {
            // Returns a count of the number of attachments to the message...
            get
            {
                return aryAttachments.Count;
            }
        }

        public ArrayList AttachmentCollection
        {
            // Returns an array list of all of the attachments..
            get
            {
                return aryAttachments;
            }
        }
        // Returns an attachment's base file name; 1-based...
        public string get_AttachmentFileName(int nIndex)
        {
            string AttachmentFileNameRet = default;
            try
            {
                Attachment objAttachment = (Attachment)aryAttachments[nIndex - 1];
                AttachmentFileNameRet = objAttachment.FileName;
            }
            catch
            {
                Interaction.MsgBox("[5003] " + Information.Err().ToString());
                AttachmentFileNameRet = "";
            }

            return AttachmentFileNameRet;
        }
        // Returns an attachment's image as a byte array; 1-based...
        public byte[] get_AttachmentImage(int nIndex)
        {
            byte[] AttachmentImageRet = default;
            try
            {
                Attachment objAttachment = (Attachment)aryAttachments[nIndex - 1];
                AttachmentImageRet = objAttachment.Image;
            }
            catch
            {
                Interaction.MsgBox("[5004] " + Information.Err().ToString());
                return null;
            }

            return AttachmentImageRet;
        }

        public bool DecodeMime(string strMime)
        {
            try
            {
                var memStream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(strMime));
                var mimeParser = new MimeParser(memStream);
                aryAttachments.Clear();

                foreach (var mimeMessage in mimeParser)
                {
                    using (var headerStream = new MemoryStream())
                    {
                        mimeMessage.Headers.WriteTo(headerStream);
                        strMessageHeader = ASCIIEncoding.ASCII.GetString(headerStream.ToArray());
                    }
                    strMessageBody = mimeMessage.TextBody;

                    foreach (var attachment in mimeMessage.Attachments)
                    {
                        if (!(attachment is MessagePart))
                        {
                            var part = (MimePart)attachment;
                            var fileName = part.FileName;

                            using (var stream = new MemoryStream())
                            {
                                part.Content.DecodeTo(stream);
                                Attachment stcAttachment = new Attachment()
                                {
                                    FileName = part.FileName.Replace(Globals.CR, "").Replace(Globals.LF, ""),
                                    Image = stream.ToArray()
                                };
                                aryAttachments.Add(stcAttachment);
                            }
                        }
                    }

                    // Only one message should exist.
                    break;
                }

                return true;
            } 
            catch
            {
                return false;
            }
        }
    }
}