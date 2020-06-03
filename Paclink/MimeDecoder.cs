using System.Collections;
using Microsoft.VisualBasic;
using nsoftware.IPWorks;

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
            var objMime = new Mime();
            aryAttachments.Clear();
            try
            {
                objMime.Message = strMime;
                objMime.DecodeFromString();
                strMessageHeader = objMime.MessageHeadersString;
                strMessageBody = objMime.Parts[0].DecodedString;
                if (objMime.Parts.Count > 1)
                {
                    foreach (MIMEPart objPart in objMime.Parts)
                    {
                        if (!string.IsNullOrEmpty(objPart.Name))
                        {
                            Attachment stcAttachment;
                            stcAttachment.FileName = objPart.Filename.Replace(Constants.vbCr, "").Replace(Constants.vbLf, "");
                            stcAttachment.Image = objPart.DecodedStringB;
                            aryAttachments.Add(stcAttachment);
                        }
                    }
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