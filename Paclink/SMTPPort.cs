using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic;
using NLog;

namespace Paclink
{

    // Creates a listening port for inbound SMTP connections...
    public class SMTPPort
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public string LocalHost = "127.0.0.1";
        public int LocalPort = 25;
        private Socket _daemonSocket;

        private Socket objIPDaemon
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _daemonSocket;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _daemonSocket = value;
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
                        if (objSMTPSessions[intIndex].Timestamp > DateTime.Now.AddMinutes(-30))
                        {
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
            if (objIPDaemon != null)
            {
                objIPDaemon.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                objIPDaemon.Close();
                objIPDaemon = null;
            }

            if (blnValue)
            {
                objIPDaemon = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                objIPDaemon.Bind(new IPEndPoint(IPAddress.Parse(LocalHost), LocalPort));
                objIPDaemon.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, true);
                objIPDaemon.Listen(10);
                objIPDaemon.AcceptAsync().ContinueWith(t =>
                {
                    if (!t.IsFaulted) OnConnected(t.Result);
                }).Wait(0);
            }
        } // Listen

        public void Close()
        {
            // This to insure a clean shutdown of all TCP Ports... 
            try
            {
                objIPDaemon.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                for (int intIndex = 0; intIndex <= 9; intIndex++)
                {
                    try
                    {
                        if (objSMTPSessions[intIndex] != null)
                        {
                            objSMTPSessions[intIndex].Close();
                            objSMTPSessions[intIndex] = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error("[SMTPPort.Close] " + ex.Message);
                    }
                }

                objIPDaemon.Close();
                objIPDaemon.Dispose();
                objIPDaemon = null;
            }
            catch (Exception ex)
            {
                _log.Error("[SMTPPort.Dispose]: " + ex.Message);
            }
        } // Dispose

        public int ConnectionCount()
        {
            // Returns the number of open SMTP sessions...
            var intCount = default(int);
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (objSMTPSessions[intIndex] != null)
                    intCount += 1;
            }

            return intCount;
        } // ConnectionCount

        public void OnDisconnect(SMTPSession session)
        {
            for (int intIndex = 0; intIndex <= 9; intIndex++)
            {
                if (objSMTPSessions[intIndex] == session)
                {
                    objSMTPSessions[intIndex] = null;
                }
            }
        } // Disconnect

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
                    if (objSMTPSessions[intIndex] == null)
                    {
                        objSMTPSessions[intIndex] = new SMTPSession(s);
                        objSMTPSessions[intIndex].OnDisconnect += OnDisconnect;
                        break;
                    }
                }
            }

            objIPDaemon.AcceptAsync().ContinueWith(t =>
            {
                if (!t.IsFaulted) OnConnected(t.Result);
            }).Wait(0);
        } // OnConnected
    } // SMTPPort
}