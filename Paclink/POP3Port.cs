using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using NLog;

namespace Paclink
{
    public class POP3Port
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public string LocalHost = "127.0.0.1";
        public int LocalPort = 110;
        private Socket _objTCPPort;

        private Socket objTCPPort
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
                        if (objPOP3Sessions[intIndex].Timestamp > DateTime.Now.AddMinutes(-30))
                        {
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
                objTCPPort.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                for (int intIndex = 0; intIndex <= 9; intIndex++)
                {
                    try
                    {
                        if (objPOP3Sessions[intIndex] is object)
                        {
                            objPOP3Sessions[intIndex].Close();
                            objPOP3Sessions[intIndex] = null;
                        }
                    }
                    catch
                    {
                    }
                }

                objTCPPort.Close();
                objTCPPort.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error("[POP3Port.Dispose]: " + ex.Message);
            }
        } // Dispose

        public void Listen(bool blnValue)
        {
            // This method is called to active the port...
            if (objTCPPort != null)
            {
                objTCPPort.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                objTCPPort.Close();
                objTCPPort = null;
            }

            if (blnValue)
            {
                objTCPPort = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                objTCPPort.Bind(new IPEndPoint(IPAddress.Parse(LocalHost), LocalPort));
                objTCPPort.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, true);
                objTCPPort.Listen(10);
                objTCPPort.AcceptAsync().ContinueWith(t =>
                {
                    if (t.Exception == null) OnConnected(t.Result);
                }).Wait(0);
            }
        } // Listen

        public int ConnectionCount()
        {
            // Returns the number of open SMTP sessions...
            var intCount = default(int);
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (objPOP3Sessions[intIndex] != null)
                    intCount += 1;
            }

            return intCount;
        } // ConnectionCount

        private void OnConnected(Socket s)
        {
            if (ConnectionCount() >= 10)
            {
                s.Close();
            }
            else
            {
                for (int intIndex = 0; intIndex <= 9; intIndex++)
                {
                    if (objPOP3Sessions[intIndex] == null)
                    {
                        objPOP3Sessions[intIndex] = new POP3Session(s);
                        objPOP3Sessions[intIndex].OnDisconnect += OnDisconnected;
                        break;
                    }
                }
            }

            objTCPPort.AcceptAsync().ContinueWith(t =>
            {
                if (t.Exception == null) OnConnected(t.Result);
            }).Wait(0);
        } // OnConnected

        private void OnDisconnected(POP3Session s)
        {
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (objPOP3Sessions[intIndex] == s)
                {
                    objPOP3Sessions[intIndex] = null;
                }
            }
        } // OnDisconnected
    }
}