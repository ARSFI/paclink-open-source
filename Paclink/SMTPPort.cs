using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using nsoftware.IPWorks;

namespace Paclink
{

    // Creates a listening port for inbound SMTP connections...
    public class SMTPPort
    {
        public string LocalHost = "127.0.0.1";
        public int LocalPort = 25;
        private Ipdaemon _objIPDaemon;

        private Ipdaemon objIPDaemon
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objIPDaemon;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _objIPDaemon = value;
            }
        }

        private SMTPSession[] objSMTPSessions = new SMTPSession[10];
        private System.Timers.Timer tmrTimeout;

        public SMTPPort()
        {
            tmrTimeout = new System.Timers.Timer();
            tmrTimeout.Interval = 60000; // Once a minute
            tmrTimeout.AutoReset = false;
            tmrTimeout.Elapsed += CheckTimeouts;
            tmrTimeout.Start();
            objIPDaemon = new Ipdaemon();
            objIPDaemon.OnConnectionRequest += OnConnectionRequest;
            objIPDaemon.OnConnected += OnConnected;
            objIPDaemon.OnReadyToSend += OnReadyToSend;
            objIPDaemon.OnDataIn += OnDataIn;
            objIPDaemon.OnDisconnected += OnDisconnected;
        } // New

        private void CheckTimeouts(object s, System.Timers.ElapsedEventArgs e)
        {
            tmrTimeout.Stop();
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                try
                {
                    if (objSMTPSessions[intIndex] is object)
                    {
                        if (objSMTPSessions[intIndex].Timestamp > DateAndTime.Now.AddMinutes(-30))
                        {
                            objIPDaemon.Disconnect(objSMTPSessions[intIndex].ConnectionId);
                            objSMTPSessions[intIndex].Close();
                            objSMTPSessions[intIndex] = null;
                        }
                    }
                }
                catch
                {
                }
            }

            tmrTimeout.Start();
        }

        public void Listen(bool blnValue)
        {
            // This method is called to active the port...
            objIPDaemon.Linger = false;
            objIPDaemon.Listening = false;
            if (blnValue)
            {
                {
                    var withBlock = objIPDaemon;
                    withBlock.Linger = true;
                    withBlock.LocalPort = LocalPort;
                    withBlock.Listening = true;
                }
            }
        } // Listen

        public void Close()
        {
            // This to insure a clean shutdown of all TCP Ports... 
            try
            {
                objIPDaemon.Linger = false;
                for (int intIndex = 0; intIndex <= 9; intIndex++)
                {
                    try
                    {
                        if (!Information.IsNothing(objSMTPSessions[intIndex]))
                        {
                            try
                            {
                                objIPDaemon.Disconnect(objSMTPSessions[intIndex].ConnectionId);
                            }
                            catch
                            {
                            }

                            objSMTPSessions[intIndex].Close();
                            objSMTPSessions[intIndex] = null;
                        }
                    }
                    catch
                    {
                        Logs.Exception("[SMTPPort.Close] " + Information.Err().Description);
                    }
                }

                objIPDaemon.Shutdown();
                objIPDaemon.Dispose();
            }
            catch
            {
                Logs.Exception("[SMTPPort.Dispose]: " + Information.Err().Description);
            }
        } // Dispose

        public int ConnectionCount()
        {
            // Returns the number of open SMTP sessions...
            var intCount = default(int);
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (!Information.IsNothing(objSMTPSessions[intIndex]))
                    intCount += 1;
            }

            return intCount;
        } // ConnectionCount

        public void DataToSend(string strText, string strConnectionId)
        {
            objIPDaemon.Send(strConnectionId, Globals.GetBytes(strText));
        } // DataToSend

        public void Disconnect(string strConnectionId)
        {
            objIPDaemon.Disconnect(strConnectionId);
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (objSMTPSessions[intIndex] is object && (objSMTPSessions[intIndex].ConnectionId ?? "") == (strConnectionId ?? ""))
                {
                    objSMTPSessions[intIndex].Close();
                    objSMTPSessions[intIndex] = null;
                }
            }
        } // Disconnect

        private void OnConnectionRequest(object s, IpdaemonConnectionRequestEventArgs e)
        {
            if (ConnectionCount() >= 10)
                e.Accept = false;
        } // OnConnectionRequest

        private void OnConnected(object s, IpdaemonConnectedEventArgs e)
        {
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (Information.IsNothing(objSMTPSessions[intIndex]))
                {
                    objSMTPSessions[intIndex] = new SMTPSession(this, e.ConnectionId);
                    break;
                }
            }
        } // OnConnected

        private void OnReadyToSend(object s, IpdaemonReadyToSendEventArgs e)
        {
            foreach (SMTPSession objSMTPSession in objSMTPSessions)
            {
                try
                {
                    if ((objSMTPSession.ConnectionId ?? "") == (e.ConnectionId ?? ""))
                    {
                        objSMTPSession.DataIn("NewConnection");
                        break;
                    }
                }
                catch
                {
                }
            }
        } // OnReadyToSend

        private void OnDataIn(object s, IpdaemonDataInEventArgs e)
        {
            foreach (SMTPSession objSMTPSession in objSMTPSessions)
            {
                try
                {
                    if ((objSMTPSession.ConnectionId ?? "") == (e.ConnectionId ?? ""))
                    {
                        objSMTPSession.DataIn(e.Text);
                        break;
                    }
                }
                catch
                {
                }
            }
        } // OnDataIn

        private void OnDisconnected(object s, IpdaemonDisconnectedEventArgs e)
        {
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (!Information.IsNothing(objSMTPSessions[intIndex]) && (objSMTPSessions[intIndex].ConnectionId ?? "") == (e.ConnectionId ?? ""))
                {
                    objSMTPSessions[intIndex].Close();
                    objSMTPSessions[intIndex] = null;
                }
            }
        } // OnDisconnected
    } // SMTPPort
}