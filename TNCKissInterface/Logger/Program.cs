using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security;

namespace Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Report();

        }
    }
    static public class Log
    {
        static String Q = Convert.ToChar(34).ToString();
        static CookieContainer Cookies = new CookieContainer();

        static String RootURL = "http://shepherd-of-the-hills-church.org/";
        static String LoginURL = "contactus.aspx";
        static public void Report()
        {

            String Param = "";
            String Result = "";
            Byte[] BufferOut;
            Byte[] BufferIn = new Byte[1000000];
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(RootURL + LoginURL);

                //<input type="hidden" name="MSO_PageHashCode" id="MSO_PageHashCode" value="1612069830039" />
                //<input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="" />
                //<input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" />
                //<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUBMGRkcl7dYh+40hAtf81tcF46W+CqSYM=" />


                Request.CachePolicy = HttpWebRequest.DefaultCachePolicy;

                Request.CookieContainer = Cookies;
                Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                Request.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.None;
                Request.UseDefaultCredentials = false;

                //Request.ContentLength = BufferOut.Length;
                //Request.ContentType = "application/x-www-form-urlencoded";
                Request.Method = "GET";

                //Request.ContentType = "application/x-www-form-urlencoded";

                String foo1 = GetResp(ref Request);

                Request = (HttpWebRequest)WebRequest.Create(RootURL + LoginURL);
                Request.CachePolicy = HttpWebRequest.DefaultCachePolicy;
                Request.CookieContainer = Cookies;
                Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                Request.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.None;
                Request.UseDefaultCredentials = false;
                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded";

                Param = //"MSO_PageHashCode=" + GetHidden(foo1, "MSO_PageHashCode") + 
                    //"&__EVENTTARGET=" + GetHidden(foo1, "__EVENTTARGET") + 
                    //"&__EVENTARGUMENT=" + GetHidden(foo1, "__EVENTARGUMENT") + 
                    "__VIEWSTATE=" + GetHidden(foo1, "__VIEWSTATE") +
                    "&ctl00%24IWS_WH_CPH_Content%24ContactUsControl1%24firstName=Foo" +
                    "&ctl00%24IWS_WH_CPH_Content%24ContactUsControl1%24lastName=Bar" +
                    "&ctl00%24IWS_WH_CPH_Content%24ContactUsControl1%24Address=woodsprw@comcast.net" +
                    "&ctl00%24IWS_WH_CPH_Content%24ContactUsControl1%24PhoneNumber=" +
                    "&ctl00%24IWS_WH_CPH_Content%24ContactUsControl1%24Msg=Test+via+soth+5" +
                    "&ctl00%24IWS_WH_CPH_Content%24ContactUsControl1%24Submit=Submit" +
                    "&__EVENTVALIDATION=" + GetHidden(foo1, "__EVENTVALIDATION");

                BufferOut = Encoding.ASCII.GetBytes(Param);
                Request.ContentLength = BufferOut.Length;

                Stream DataStream = Request.GetRequestStream();

                DataStream.Write(BufferOut, 0, BufferOut.Length);
                DataStream.Close();

                Result = GetResp(ref Request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);

            }


            Console.WriteLine("Result: " + Result);

        }
        static String GetResp(ref HttpWebRequest Request)
        {
            String Result = "";
            try
            {
                WebResponse Response = Request.GetResponse();

                Stream DataStream = Response.GetResponseStream();
                StreamReader DataRead = new StreamReader(DataStream);
                Result = DataRead.ReadToEnd();
                DataRead.Close();
                DataStream.Close();
                Response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:: " + ex.Message);
            }
            return Result;
        }

        public static String GetHidden(String s, String q)
        {
            if (s.Contains(q))
            {
                Int32 l = s.IndexOf(q);
                s = s.Substring(l);
                l = s.IndexOf("value=\"") + 7;
                s = s.Substring(l);
                l = s.IndexOf("\"");
                s = s.Substring(0, l).Replace("$", "%24");
                s = s.Replace("/", "%2F");
                s = s.Replace(" ", "%2B");
                s = s.Replace("+", "%2B");
                s = s.Replace("=", "%3D");
                return s;
            }
            return "";
        }


        //<input type="hidden" name="MSO_PageHashCode" id="MSO_PageHashCode" value="1612069830039" />
        //<input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="" />
        //<input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" />
        //<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUBMGRkcl7dYh+40hAtf81tcF46W+CqSYM=" />



    }

}
