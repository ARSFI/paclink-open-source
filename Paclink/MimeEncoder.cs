using System.Collections;
using System.Text;
using Microsoft.VisualBasic;
using nsoftware.IPWorks;

namespace Paclink
{
    public struct Attachment
    {
        // A structure to hold the file name and binary image of an attachment...
        public string FileName;
        public byte[] Image;
    } // Attachment

    public class MimeEncoder
    {
        public string MessageId;
        public ArrayList Attachments = new ArrayList();
        public string Header = "";
        public string Mime = "";
        public string Body = "";

        // Add the message body...
        public void AddMessageBody(string strMessage)
        {
            Body = FormatMessageBody(strMessage);
        } // AddMessageBody

        private string FormatMessageBody(string strMessageBody)
        {
            // Normalizes line ends and removes excess white space from the body of
            // the message...

            if (Information.IsNothing(strMessageBody))
                strMessageBody = "<no message body>";
            strMessageBody = strMessageBody.Trim().Replace(Constants.vbLf, "");
            strMessageBody = strMessageBody.Replace(Constants.vbCr, Constants.vbCrLf) + Constants.vbCrLf;
            do
            {
                if (strMessageBody.IndexOf(Constants.vbCrLf + Constants.vbCrLf + Constants.vbCrLf) == -1)
                    break;
                strMessageBody = strMessageBody.Replace(Constants.vbCrLf + Constants.vbCrLf + Constants.vbCrLf, Constants.vbCrLf + Constants.vbCrLf);
            }
            while (true);
            return strMessageBody;
        } // FormatMessageBody

        private bool MakePlainTextMessage()
        {
            // This subroutine creates a complete mime encoded message where there are
            // no attachments...
            try
            {
                var stbMessage = new StringBuilder();
                stbMessage.Append("MIME-Version: 1.0" + Constants.vbCrLf);
                stbMessage.Append("Content-Type: text/plain; charset = \"iso-8859-1\"" + Constants.vbCrLf);
                stbMessage.Append("Content-Transfer-Encoding: 7bit" + Constants.vbCrLf + Constants.vbCrLf);
                stbMessage.Append(Body);
                Mime = Header + stbMessage.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        } // MakePlainTextMessage

        public bool EncodeMime()
        {
            if (!string.IsNullOrEmpty(Header))
            {
                if (Attachments.Count == 0)
                {
                    return MakePlainTextMessage();
                }
                else
                {
                    var objMime = new Mime();
                    objMime.ResetData();
                    objMime.Parts.Add(new MIMEPart());
                    objMime.Parts[0].DecodedString = Body;
                    objMime.Parts[0].Encoding = MIMEPartEncodings.peQuotedPrintable;
                    objMime.Parts[0].ContentType = "text/plain";
                    for (int intIndex = 1, loopTo = Attachments.Count; intIndex <= loopTo; intIndex++)
                    {
                        Attachment stcAttachment = (Attachment)Attachments[intIndex - 1];
                        var objPart = new MIMEPart();
                        objPart.Filename = stcAttachment.FileName;
                        objPart.Encoding = MIMEPartEncodings.peBase64;
                        objPart.ContentType = "attachment";
                        objPart.ContentTypeAttr = "name=\"" + stcAttachment.FileName + "\"";
                        objPart.ContentDisposition = "attachment";
                        objPart.ContentDispositionAttr = "filename=\"" + stcAttachment.FileName + "\"";
                        objPart.DecodedStringB = stcAttachment.Image;
                        objMime.Parts.Add(objPart);
                    }

                    objMime.EncodeToString();
                    string strPartialEncodedMessage = objMime.Message;
                    Header = Header + objMime.MessageHeadersString;
                    Mime = Header + strPartialEncodedMessage;
                    objMime.Dispose();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}