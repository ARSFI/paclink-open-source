using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using MimeKit;

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
            strMessageBody = strMessageBody.Trim().Replace(Globals.LF, "");
            strMessageBody = strMessageBody.Replace(Globals.CR, Globals.CRLF) + Globals.CRLF;
            do
            {
                if (strMessageBody.IndexOf(Globals.CRLF + Globals.CRLF + Globals.CRLF) == -1)
                    break;
                strMessageBody = strMessageBody.Replace(Globals.CRLF + Globals.CRLF + Globals.CRLF, Globals.CRLF + Globals.CRLF);
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
                stbMessage.Append("MIME-Version: 1.0" + Globals.CRLF);
                stbMessage.Append("Content-Type: text/plain; charset = \"iso-8859-1\"" + Globals.CRLF);
                stbMessage.Append("Content-Transfer-Encoding: 7bit" + Globals.CRLF + Globals.CRLF);
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
                    // Create message from existing data so we can copy the header and text
                    // data to the new message.
                    var multipartBoundaryRgx = Regex.Match(Body, "^--([^\r\n]+)");
                    var multipartHeader = string.Empty;
                    if (multipartBoundaryRgx.Success)
                    {
                        var multipartBoundary = multipartBoundaryRgx.Groups[1];
                        multipartHeader = string.Format("Content-Type: multipart/alternative; boundary=\"{0}\"", multipartBoundary);
                    }

                    var memStream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(Header + multipartHeader + "\r\n\r\n" + Body));
                    var mimeParser = new MimeParser(memStream);
                    var mimeMessage = mimeParser.ParseMessage();

                    // Create new message and set body/attachments.
                    var objMime = new MimeMessage();
                    objMime.Headers.Clear();
                    foreach (var header in mimeMessage.Headers)
                    {
                        objMime.Headers.Add(header);
                    }

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.TextBody = mimeMessage.TextBody;
                    bodyBuilder.HtmlBody = mimeMessage.HtmlBody;
                    foreach (Attachment attachment in Attachments)
                    {
                        bodyBuilder.Attachments.Add(attachment.FileName, attachment.Image);
                    }

                    objMime.Body = bodyBuilder.ToMessageBody();
                    
                    using (var headerStream = new MemoryStream())
                    {
                        objMime.Headers.WriteTo(headerStream);
                        Header = ASCIIEncoding.ASCII.GetString(headerStream.ToArray());
                    }

                    using (var msgStream = new MemoryStream())
                    {
                        objMime.WriteTo(msgStream);
                        Mime = ASCIIEncoding.ASCII.GetString(msgStream.ToArray());
                    }

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