using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace WinlinkInterop
{

    // 
    // Class to update the SFI from the Internet.
    // 

    public class SFIhelper
    {

        // Objects/Classes
        private HttpClient _objHTTP;

        private HttpClient objHTTP
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objHTTP;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _objHTTP = value;
            }
        }

        private bool blnProcessingSFI = false;
        private bool blnGetSFIDownloadComplete = false;
        private string strGetSFIError = "";
        private bool blnGetSFISuccess;
        private bool blnFTPDownloadError = false;
        private string strSFIDownload = "";   // Test returned by download site
        private string strSFI = "";           // Latest SFI value.
        private string strWorkDirectory = "";
        private string strTempFile = "FTPSFIData.txt";
        private string strLocalPath = "";
        private string strConnectionStatus = "";
        private string strErrorMessage = "";
        private Thread thrGetSFI = null;

        ~SFIhelper()
        {
            // 
            // Object destructor.
            // 
            Close();
            return;
        }

        public void Close()
        {
            // 
            // Close connections and stop threads.
            // 
            try
            {
                try
                {
                    if (!Information.IsNothing(thrGetSFI) && thrGetSFI.IsAlive)
                        thrGetSFI.Abort();
                }
                catch
                {
                }

                if (!Information.IsNothing(objHTTP))
                {
                    objHTTP.Dispose();
                    objHTTP = null;
                }
            }
            catch
            {
            }

            strGetSFIError = "Aborted";
            return;
        }

        public string GetErrorMessage()
        {
            // 
            // Return the stored error message.
            // 
            return strErrorMessage;
        }

        public string GetSFI(string strTempFolder, double dblMaxWaitSeconds = 240.0)
        {
            // 
            // Update the solar flux index.
            // strTempFolder is a work folder where the downloaded file can be stored.
            // dblMaxWaitSeconds is the maximum number of seconds to wait for a download.
            // If the SFI value is obtained, it is returned.  Othewise, and empty string is returned.
            // If an error occurs, GetError can be called to get the error message.
            // 
            string strResult = "";
            SetError("");
            strWorkDirectory = strTempFolder;
            strLocalPath = strWorkDirectory + strTempFile;
            try
            {
                // 
                // Start a thread to download the SFI value.
                // 
                if (!blnProcessingSFI)
                {
                    if (!Information.IsNothing(thrGetSFI))
                    {
                        if (thrGetSFI.IsAlive)
                            thrGetSFI.Abort();
                        thrGetSFI = null;
                    }

                    thrGetSFI = new Thread(() => DownloadSFI());
                    thrGetSFI.Priority = ThreadPriority.BelowNormal;
                    blnGetSFIDownloadComplete = false;
                    blnProcessingSFI = true;
                    thrGetSFI.Start();
                }
                // 
                // Wait for the thread to finish.
                // 
                strResult = "";
                var dtmStart = DateTime.Now;
                while (blnProcessingSFI & (DateTime.Now - dtmStart).TotalSeconds < dblMaxWaitSeconds)
                    Thread.Sleep(500);
                // 
                // See if we successfully obtained a value.
                // 
                if (blnGetSFIDownloadComplete & string.IsNullOrEmpty(strGetSFIError) & !blnProcessingSFI)
                {
                    strResult = strSFI;
                }
                else if (blnProcessingSFI)
                {
                    SetError("Timeout while waiting to download SFI information");
                }
                else if (!string.IsNullOrEmpty(strGetSFIError))
                {
                    SetError("Error downloading SFI information: " + strGetSFIError);
                }
                else
                {
                    SetError("Error downloading SFI information");
                }
            }
            catch (Exception ex)
            {
                SetError("Exception in GetSFI: " + ex.Message);
                strResult = "";
            }

            return strResult;
        }

        private void DownloadSFI(double dblMaxWaitSeconds = 30)
        {
            // 
            // Subroutine running as a thread to get the solar flux index from the web.
            // blnProcessingSFI is set False when this thread finishes and strSFI gets the SFI value.
            // URL as of July 5, 2020   https://services.swpc.noaa.gov/text/sgas.txt
            // 
            string strError = "";
            blnGetSFISuccess = false;
            blnProcessingSFI = true;
            // 
            // FTP access mechanism (after Dec 8, 2014 due to dropping of txt version on http web site.)
            // Try a couple of times
            // 
            for (int intTry = 1; intTry <= 5; intTry++)
            {
                if (DownloadFileHTTP(dblMaxWaitSeconds)) break;

                // Error downloading SFI value.  Wait 10 seconds and try again.
                Thread.Sleep(10000);
            }

            strGetSFIError = strError;
            blnProcessingSFI = false;
            thrGetSFI = null;
            return;
        }

        public bool DownloadFileHTTP(double dblMaxWaitSeconds)
        {
            // 
            // Attempt to download the SFI information from an http site.  Return True on success.
            // 
            SetError("");
            blnGetSFISuccess = false;
            try
            {
                if (Information.IsNothing(objHTTP))
                {
                    objHTTP = new HttpClient();
                }

                var timeoutInSec = Convert.ToInt32(dblMaxWaitSeconds);
                objHTTP.Timeout = new TimeSpan(0, 0, timeoutInSec / 60, timeoutInSec % 60, 0);
                blnGetSFIDownloadComplete = false;
                strSFIDownload = "";
                var dttStartDownload = DateAndTime.Now;
                var httpCancelTokenSource = new CancellationTokenSource();
                var httpCancelToken = httpCancelTokenSource.Token;
                var httpDownloadTask = objHTTP.GetAsync("https://services.swpc.noaa.gov/text/sgas.txt", httpCancelToken);
                httpDownloadTask.ContinueWith(t =>
                {
                    try
                    {
                        if (httpCancelToken.IsCancellationRequested)
                        {
                            return;
                        }

                        var readTask = t.Result.Content.ReadAsStringAsync();
                        readTask.ContinueWith(v =>
                        {
                            strSFIDownload = v.Result;
                            objHTTP_OnEndTransfer(t.Result);
                        }).Wait();
                    } 
                    catch (Exception e)
                    {
                        // 
                        // An error occurred while downloading from the HTTP site.
                        // 
                        blnProcessingSFI = false;
                        blnGetSFIDownloadComplete = true;
                        blnGetSFISuccess = false;
                        SetError("Error occurred downloading from https://services.swpc.noaa.gov: " + e.Message);
                    }
                }).Wait(0);

                while (DateAndTime.Now.Subtract(dttStartDownload).TotalSeconds < dblMaxWaitSeconds & !blnGetSFIDownloadComplete)
                {
                    Thread.Sleep(100);
                }

                if (!blnGetSFIDownloadComplete)
                {
                    // Timeout on HTTP download.
                    SetError("Timeout occurred downloading from https://services.swpc.noaa.gov");
                    httpCancelTokenSource.Cancel();
                    try
                    {
                        if (objHTTP is object)
                        {
                            objHTTP.Dispose();
                            objHTTP = null;
                        }
                    }
                    catch
                    {
                        // Ignore Dispose error.
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                // Some sort of download failure.
                SetError("Exception downloading from http://services.swpc.noaa.gov: " + ex.Message);
                try
                {
                    if (objHTTP is object)
                    {
                        objHTTP.Dispose();
                        objHTTP = null;
                    }
                }
                catch
                {
                    // Ignore Dispose error.
                }

                objHTTP = null;
                return false;
            }
            // 
            // We got the SFI value.
            // 
            try
            {
                if (objHTTP is object)
                {
                    objHTTP.Dispose();
                    objHTTP = null;
                }
            }
            catch
            {
                // Ignore Dispose error.
            }

            objHTTP = null;
            // 
            // Finished
            // 
            return true;
        }

        private void objHTTP_OnEndTransfer(HttpResponseMessage msg)
        {
            // 
            // The HTTP transfer completed.
            // 
            try
            {
                if (!string.IsNullOrEmpty(strSFIDownload))
                {
                    int intSFIIndex = strSFIDownload.IndexOf("10 cm ");
                    if (intSFIIndex != -1)
                    {
                        try
                        {
                            string strNewSFI = strSFIDownload.Substring(intSFIIndex + 5, 5).Trim();
                            if (Information.IsNumeric(strNewSFI))
                            {
                                // We got an SFI update.
                                strSFI = strNewSFI;
                                blnGetSFISuccess = true;
                            }
                            else
                            {
                                SetError("Non-numeric SFI value downloaded from http://services.swpc.noaa.gov");
                            }
                        }
                        catch (Exception ex)
                        {
                            SetError("Exception parsing SFI value from http://services.swpc.noaa.gov download: " + ex.Message);
                        }
                    }
                }

                blnGetSFIDownloadComplete = true;
            }
            catch
            {
            }

            return;
        }

        private double ConvertStringToDouble(string s)
        {
            // 
            // Parse a string value and return it as a double.  Deal with various delimiters.
            // 
            double dblRet = 0.0;
            string strDecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            s = s.Replace(".", strDecimalSeparator);
            try
            {
                dblRet = Conversions.ToDouble(s);
            }
            catch (Exception ex)
            {
                // 
                // Log an exception
                // 
                SetError("Exception in ConvertStringToDouble: " + ex.Message);
                return 0.0;
            }

            return dblRet;
        }

        private void SetError(string strError)
        {
            // 
            // Record an error.
            // 
            strErrorMessage = strError;
            return;
        }
    }
}