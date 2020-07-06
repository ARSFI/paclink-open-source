using System;
using System.Net;

namespace WinlinkInterop
{
    // 
    // WebClient class that allows timeout to be set.
    // 
    public class WebClientExtended : WebClient
    {
        private int m_Timeout;
        private bool m_Finished = false;
        private byte[] _uploadValuesAsync;

        public int Timeout
        {
            get
            {
                return m_Timeout;
            }

            set
            {
                m_Timeout = value;
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = Timeout;
            return request;
        }

        public void SetEvent()
        {
            UploadValuesCompleted += OnUploadCompleted;
        }

        public void OnUploadCompleted(object objSender, UploadValuesCompletedEventArgs objArg)
        {
            m_Finished = true;
        }
    }
}