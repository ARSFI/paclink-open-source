using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace Paclink
{
    public class ClientTelnet : IClient
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private TcpClient objTCPPort;
        private ProtocolInitial objProtocol;
        private TChannelProperties stcChannel;
        private ELinkStates enmState = ELinkStates.Undefined;
        private EConnection enmConnectionStatus;
        private Queue queDataBytesIn = Queue.Synchronized(new Queue());
        private bool blnNormalDisconnect;
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private int intTimeout = 0;

        public void Poll()
        {
            if (Globals.blnChannelActive == false)
                return;
            if (enmState != ELinkStates.Connected)
            {
                intTimeout += 1;
                if (intTimeout > 100) // Approx 10 seconds worth of timer ticks...
                {
                    intTimeout = 0;
                    if (Globals.UseRMSRelay())
                    {
                        Globals.queChannelDisplay.Enqueue("R*** No connection to RMS Relay at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC"));
                    }
                    else
                    {
                        Globals.queChannelDisplay.Enqueue("R*** No connection to WL2K CMS Telnet at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC"));
                    }

                    enmState = ELinkStates.LinkFailed;
                    cancelTokenSource.Cancel(); // cancels any pending reads
                    objTCPPort.Close();
                }
            }

            while (queDataBytesIn.Count > 0)
            {
                byte[] bytIn;
                try
                {
                    bytIn = (byte[])queDataBytesIn.Dequeue();
                }
                catch
                {
                    break;
                }

                if (enmState == ELinkStates.Connected)
                {
                    objProtocol.ChannelInput(ref bytIn);
                }
                else
                {
                    SignInLine(Globals.GetString(bytIn));
                }
            }

            if (enmState == ELinkStates.LinkFailed)
            {
                if (objTCPPort is object)
                {
                    if (objTCPPort.Connected)
                        Disconnect();
                }
                else
                {
                    try
                    {
                        if (objProtocol != null)
                        {
                            objProtocol.LinkStateChange(EConnection.Disconnected);
                            objProtocol = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("[ClientTelnet.Poll] " + ex.Message);
                    }
                }
            }
        } // Poll

        public void SendRadioCommand(byte[] bytCommand)
        {
            // Function to send radio command via PTC II (not used in this class)
        } // SendRadioCommand

        public void SendRadioCommand(string strCommand)
        {
            // Function to send radio command via PTC II (not used in this class)
        } // SendRadioCommand

        public void Abort()
        {
            if (enmState != ELinkStates.Connected)
            {
                Close();
                return;
            }

            try
            {
                cancelTokenSource.Cancel(); // cancels any pending reads
                objTCPPort.Close();
            }
            catch (Exception ex)
            {
                Log.Error("[TelnetClient.Abort] " + ex.Message);
            }

            enmState = ELinkStates.LinkFailed;
        } // Abort 

        public ClientTelnet(ref TChannelProperties strNewChannel)
        {
            stcChannel = strNewChannel;
            Globals.blnChannelActive = true;
            enmState = ELinkStates.Initialized;
            Globals.queRateDisplay.Enqueue("Internet");
        } // New

        public ELinkStates State
        {
            get
            {
                return enmState;
            }
        } // State

        public bool NormalDisconnect
        {
            get
            {
                return blnNormalDisconnect;
            }

            set
            {
                blnNormalDisconnect = value;
            }
        } // Normal Disconnect

        // Always call this method before the instance goes out of scope...
        private bool blnClose = false;
        public bool Close()
        {
            if (blnClose == false)
            {
                blnClose = true;
                Globals.queChannelDisplay.Enqueue("G*** Closing " + stcChannel.ChannelName + " at " + Globals.TimestampEx());
                if (objTCPPort != null)
                {
                    try
                    {
                        objTCPPort.LingerState = new LingerOption(false, 0);
                        cancelTokenSource.Cancel(); // cancels any pending reads
                        objTCPPort.Close();
                        OnDisconnected(objTCPPort);
                    }
                    catch
                    {
                    }

                    objTCPPort.Dispose();
                    objTCPPort = null;
                }

                if (objProtocol is object)
                    objProtocol.CloseProtocol();
                Globals.queStateDisplay.Enqueue("");
                Globals.blnChannelActive = false;
                Globals.objSelectedClient = null;
                return true;
            }

            return default;
        } // Close

        public bool Connect(bool blnAutomatic)
        {
            // 
            // Called to start a connection with a telnet server...
            // 
            DateTime dttLogonStart;
            string strCMSHost;
            blnNormalDisconnect = false;
            if (Globals.UseRMSRelay())
            {
                strCMSHost = "";
            }
            else
            {
                strCMSHost = "cms-z.winlink.org";
            }

            if (stcChannel.Enabled)
            {
                objTCPPort = new TcpClient();
                if (Globals.strLocalIPAddress != "Default")
                    objTCPPort.Client.Bind(new IPEndPoint(IPAddress.Parse(Globals.strLocalIPAddress), 0));
                objTCPPort.ReceiveTimeout = 30;
                objTCPPort.SendTimeout = 30;
                enmState = ELinkStates.Connecting;
                try
                {
                    // If objWL2KServers.IsCMSAvailable = False Then
                    // queChannelDisplay.Enqueue("R*** No CMS Telnet server available")
                    // objTCPPort.LingerState = new LingerOption(false, 0)
                    // Try
                    // objTCPPort.Disconnect()
                    // Catch
                    // End Try
                    // objTCPPort.Dispose()
                    // If objProtocol IsNot Nothing Then objProtocol.LinkStateChange(EConnection.Disconnected)
                    // Return False
                    // End If
                    objTCPPort.LingerState = new LingerOption(true, 5);
                    dttLogonStart = DateTime.Now;
                    Task connectTask = null;
                    if (Globals.UseRMSRelay() == false)
                    {
                        Globals.queChannelDisplay.Enqueue("G*** Requesting connection to " + strCMSHost);
                        connectTask = objTCPPort.ConnectAsync(strCMSHost, 8772);
                    }
                    else
                    {
                        Globals.queChannelDisplay.Enqueue("G*** Requesting connection to RMS Relay at " + Globals.strRMSRelayIPPath + " port " + Globals.intRMSRelayPort.ToString());
                        connectTask = objTCPPort.ConnectAsync(Globals.strRMSRelayIPPath, Globals.intRMSRelayPort);
                    }

                    connectTask.ContinueWith(t =>
                    {
                        OnConnected(objTCPPort);

                        byte[] buffer = new byte[1024];

                        Task<int> task = null;
                        try
                        {
                            task = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024, cancelTokenSource.Token);
                            task.ContinueWith(t =>
                            {
                                if (!t.IsFaulted)
                                {
                                    OnDataIn(buffer, t.Result);
                                }
                            });
                            task.Wait(0);
                        }
                        catch (Exception e)
                        {
                            OnError(task.Exception);
                        }
                    }).Wait(30000);

                    if (enmState == ELinkStates.Connected || enmState == ELinkStates.Callsign || enmState == ELinkStates.Password)

                        return true;
                }
                catch (Exception ex)
                {
                    Globals.queChannelDisplay.Enqueue("R*** " + ex.Message);
                }

                objTCPPort.LingerState = new LingerOption(false, 0);
                try
                {
                    cancelTokenSource.Cancel(); // cancels any pending reads
                    objTCPPort.Close();
                }
                catch
                {
                }
            }

            Globals.queChannelDisplay.Enqueue("R*** No Connection to any CMS Telnet server");
            enmState = ELinkStates.LinkFailed;
            objTCPPort.LingerState = new LingerOption(false, 0);
            try
            {
                cancelTokenSource.Cancel(); // cancels any pending reads
                objTCPPort.Close();
            }
            catch
            {
            }

            objTCPPort.Dispose();
            objTCPPort = null;
            if (objProtocol is object)
                objProtocol.LinkStateChange(EConnection.Disconnected);
            return false;
        } // Connect

        public void DataToSend(string strOutput)
        {
            // This Subroutine is called to send string text to the outbound channel...

            var objEncoder = new ASCIIEncoding();
            DataToSend(Globals.GetBytes(strOutput));
        } // Send

        public void DataToSend(byte[] bytOutput)
        {
            // This Subroutine is called to send binary data to the outbound channel...

            int intBytesSent = 0;
            int intTries = 0;

            while (intTries < 20)
            {
                try
                {
                    objTCPPort.GetStream().Write(bytOutput, 0, bytOutput.Length);
                    Globals.UpdateProgressBar(bytOutput.Length);
                    return;
                }
                catch (Exception e)
                {
                    if (intTries < 20 & e.Message.StartsWith("Operation would block"))
                    {
                        intBytesSent = bytOutput.Length;
                        if (intBytesSent > 0)
                        {
                            var aryTemp = new byte[bytOutput.Length - (intBytesSent + 1) + 1];
                            Array.Copy(bytOutput, intBytesSent, aryTemp, 0, aryTemp.Length);
                            bytOutput = aryTemp;
                        }

                        Thread.Sleep(1000);
                        Application.DoEvents();
                        intTries += 1;
                        continue;
                    }
                }
            }
        } // Send

        public void Disconnect()
        {
            try
            {
                cancelTokenSource.Cancel(); // cancels any pending reads
                objTCPPort.Close();
                OnDisconnected(objTCPPort);
            }
            catch (Exception ex)
            {
                Log.Error("[TelnetClient.Disconnect] " + ex.Message);
            }
        } // Disconnect

        private void SignInLine(string strData)
        {
            // Supporting code for the telnet sign in...

            var strLines = strData.Split('\r');
            foreach (string strLine in strLines)
            {
                var switchExpr = enmState;
                switch (switchExpr)
                {
                    case ELinkStates.Callsign:
                        {
                            Globals.queChannelDisplay.Enqueue("X" + strLine);
                            if (strLine.Contains("Callsign"))
                            {
                                var objEncoder = new ASCIIEncoding();
                                string strPactorCallsign = Globals.SiteCallsign;
                                if (Globals.blnForceHFRouting)
                                {
                                    if (Globals.SiteCallsign.Length == 7 & Globals.SiteCallsign.Contains("-") == false)
                                    {
                                        // Add 'T' as 8th character without a dash
                                        strPactorCallsign += "T";
                                    }
                                    else
                                    {
                                        // Add "-T"
                                        strPactorCallsign += "-T";
                                    }
                                }

                                var bytesToSend = Globals.GetBytes("." + strPactorCallsign + Globals.CR);
                                objTCPPort.GetStream().Write(bytesToSend, 0, bytesToSend.Length);
                                Globals.queChannelDisplay.Enqueue("B" + Globals.SiteCallsign);
                                enmState = ELinkStates.Password;
                            }

                            break;
                        }

                    case ELinkStates.Password:
                        {
                            Globals.queChannelDisplay.Enqueue("X" + strLine);
                            if (strLine.Contains("Password"))
                            {
                                var objEncoder = new ASCIIEncoding();
                                var bytesToSend = Globals.GetBytes("CMSTelnet" + Globals.CR);
                                objTCPPort.GetStream().Write(bytesToSend, 0, bytesToSend.Length);
                                Globals.queChannelDisplay.Enqueue("B(CMS password)");
                                enmState = ELinkStates.Connected;
                                objProtocol = new ProtocolInitial(this, ref stcChannel);
                            }

                            break;
                        }
                }
            }
        } // SignInLine

        private void OnConnected(object s)
        {
            Globals.queChannelDisplay.Enqueue("G*** Connected");
            enmState = ELinkStates.Callsign;
        } // OnConnected

        private void OnDataIn(object s, int bytesRead)
        {
            if (bytesRead > 0)
            {
                byte[] buffer = (byte[])s;
                byte[] newBuffer = new byte[bytesRead];
                Array.Copy(buffer, newBuffer, bytesRead);
                queDataBytesIn.Enqueue(newBuffer);

                Task<int> t = null;
                try
                {
                    t = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024, cancelTokenSource.Token);
                    t.ContinueWith(k =>
                    {
                        if (!k.IsFaulted)
                        {
                            OnDataIn(buffer, k.Result);
                        }
                    });
                    t.Wait(0);
                }
                catch (Exception e)
                {
                    OnError(t.Exception);
                }
            }
            else
            {
                Disconnect();
            }
        } // OnDataIn

        private void OnDisconnected(object s)
        {
            // If the Connection never gets established the following WriteToChannelDisplay causes a hang
            // so RM disabled it on Sept 30 2007

            Globals.queChannelDisplay.Enqueue("G*** Telnet Disconnected");
            Thread.Sleep(1000);
            try
            {
                if (objProtocol is object)
                {
                    objProtocol.LinkStateChange(EConnection.Disconnected);
                    objProtocol = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("[TelnetClient.OnDisconnected] " + ex.Message);
            }

            if (blnNormalDisconnect)
            {
                enmState = ELinkStates.Disconnected;
            }
            else
            {
                enmState = ELinkStates.LinkFailed;
            }
        } // OnDisconnected

        private void OnError(Exception e)
        {
            if (e is SocketException)
            {
                var socketException = (SocketException)e;
                if (socketException.SocketErrorCode == SocketError.ConnectionAborted)
                {
                    OnDisconnected(socketException);
                    return;
                }
            }

            // Called following a abnormal disconnect...
            try
            {
                objProtocol.LinkStateChange(EConnection.Disconnected);
                objProtocol = null;
            }
            catch
            {
            }

            Globals.queChannelDisplay.Enqueue("R*** Telnet Error: " + e.Message);
            try
            {
                objTCPPort.LingerState = new LingerOption(false, 0);
                cancelTokenSource.Cancel(); // cancels any pending reads
                objTCPPort.Close();
            }
            catch
            {
            }

            enmState = ELinkStates.LinkFailed;
        } // OnLinkFailed
    }
}