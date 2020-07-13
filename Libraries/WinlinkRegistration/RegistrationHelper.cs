using System;
using System.Net;
using System.Text;

namespace WinlinkRegistration
{
    public class RegistrationHelper
    {

        /// <summary>
        /// Test registration key to see if it was generated using this callsign
        /// </summary>

        public static bool IsValidRegistrationKey(string callsign, string strKey)
        {
            //Check registration record (at the CMS) to insure that the key has not been recinded (voided)
            //Will also return false if no record can be found
            try
            {
                System.Collections.Specialized.NameValueCollection reqParm = new System.Collections.Specialized.NameValueCollection();
                reqParm = new System.Collections.Specialized.NameValueCollection();
                reqParm.Add("Callsign", callsign);
                reqParm.Add("RegistrationKey", strKey);
                reqParm.Add("Requester", "RegHelper");
                reqParm.Add("Key", "F55CB4E25BAD46B28F0A978EC99A98AF");
                WebClient client = new WebClient();
                byte[] responsebytes = client.UploadValues("https://api.winlink.org/arsfi/registration/valid?format=json", "POST", reqParm);
                client = null;
                if (responsebytes == null) return false;
                string strResponse = Encoding.UTF8.GetString(responsebytes);
                if (strResponse == null) return false;
                if (strResponse.ToLower().Contains("\"valid\":true"))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                //Return true (valid) if any error (cms down, no internet connection, for example)
                return true;
            }
        }
    }
}
