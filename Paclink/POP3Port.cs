using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using nsoftware.IPWorks;

namespace Paclink
{
    public class POP3Port
    {
        public string LocalHost = "127.0.0.1";
        public int LocalPort = 110;
        private Ipdaemon _objTCPPort;

        private Ipdaemon objTCPPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objTCPPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _objTCPPort = value;
            }
        }

        private POP3Session[] objPOP3Sessions = new POP3Session[10];
        private System.Timers.Timer tmrTimeout;

        public POP3Port()
        {
            tmrTimeout = new System.Timers.Timer();
            tmrTimeout.Interval = 60000; // Once a minute
            tmrTimeout.AutoReset = false;
            tmrTimeout.Elapsed += CheckTimeouts;
            tmrTimeout.Start();
            objTCPPort = new Ipdaemon();
            objTCPPort.OnConnectionRequest += OnConnectionRequest;
            objTCPPort.OnConnected += OnConnected;
            objTCPPort.OnReadyToSend += OnReadyToSend;
            objTCPPort.OnDataIn += OnDataIn;
            objTCPPort.OnDisconnected += OnDisconnected;
        } // New

        private void CheckTimeouts(object s, System.Timers.ElapsedEventArgs e)
        {
            tmrTimeout.Stop();
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                try
                {
                    if (objPOP3Sessions[intIndex] is object)
                    {
                        if (objPOP3Sessions[intIndex].Timestamp > DateAndTime.Now.AddMinutes(-30))
                        {
                            objTCPPort.Disconnect(objPOP3Sessions[intIndex].ConnectionId);
                            objPOP3Sessions[intIndex].Close();
                            objPOP3Sessions[intIndex] = null;
                        }
                    }
                }
                catch
                {
                }
            }

            tmrTimeout.Start();
        }

        public void Close()
        {
            // This to insure a clean shutdown of all TCP Ports...
            try
            {
                objTCPPort.Linger = false;
                for (int intIndex = 0; intIndex <= 9; intIndex++)
                {
                    try
                    {
                        if (objPOP3Sessions[intIndex] is object)
                        {
                            objTCPPort.Disconnect(objPOP3Sessions[intIndex].ConnectionId);
                            objPOP3Sessions[intIndex].Close();
                            objPOP3Sessions[intIndex] = null;
                        }
                    }
                    catch
                    {
                    }
                }

                objTCPPort.Shutdown();
                objTCPPort.Dispose();
            }
            catch
            {
                Logs.Exception("[POP3Port.Dispose]: " + Information.Err().Description);
            }
        } // Dispose

        public void Listen(bool blnValue)
        {
            // This method is called to active the port...
            objTCPPort.Linger = false;
            objTCPPort.Listening = false;
            if (blnValue)
            {
                {
                    var withBlock = objTCPPort;
                    withBlock.Linger = true;
                    withBlock.LocalPort = LocalPort;
                    withBlock.Listening = true;
                }
            }
        } // Listen

        public int ConnectionCount()
        {
            // Returns the number of open SMTP sessions...
            var intCount = default(int);
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (!Information.IsNothing(objPOP3Sessions[intIndex]))
                    intCount += 1;
            }

            return intCount;
        } // ConnectionCount

        public void DataToSend(string strText, string strConnectionId)
        {
            objTCPPort.Send(strConnectionId, Globals.GetBytes(strText));
        } // DataToSend

        public void Disconnect(string strConnectionId)
        {
            objTCPPort.Disconnect(strConnectionId);
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (!Information.IsNothing(objPOP3Sessions[intIndex]) && (objPOP3Sessions[intIndex].ConnectionId ?? "") == (strConnectionId ?? ""))
                {
                    objPOP3Sessions[intIndex].Close();
                    objPOP3Sessions[intIndex] = null;
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
                if (Information.IsNothing(objPOP3Sessions[intIndex]))
                {
                    objPOP3Sessions[intIndex] = new POP3Session(this, e.ConnectionId);
                    break;
                }
            }
        } // OnConnected

        private void OnReadyToSend(object s, IpdaemonReadyToSendEventArgs e)
        {
            try
            {
                foreach (POP3Session objPOP3Session in objPOP3Sessions)
                {
                    try
                    {
                        if ((objPOP3Session.ConnectionId ?? "") == (e.ConnectionId ?? ""))
                        {
                            objPOP3Session.DataIn("NewConnection");
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
                Logs.Exception("[POP3Port, OnReadyToSend]: " + Information.Err().Description);
            }
        } // OnReadyToSend

        private void OnDataIn(object s, IpdaemonDataInEventArgs e)
        {
            foreach (POP3Session objPOP3Session in objPOP3Sessions)
            {
                try
                {
                    if ((objPOP3Session.ConnectionId ?? "") == (e.ConnectionId ?? ""))
                    {
                        objPOP3Session.DataIn(e.Text);
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
                if (!Information.IsNothing(objPOP3Sessions[intIndex]) && (objPOP3Sessions[intIndex].ConnectionId ?? "") == (e.ConnectionId ?? ""))
                {
                    objPOP3Sessions[intIndex].Close();
                    objPOP3Sessions[intIndex] = null;
                }
            }
        } // OnDisconnected
    }
}