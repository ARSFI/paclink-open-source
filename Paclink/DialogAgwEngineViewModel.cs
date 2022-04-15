using NLog;
using Paclink.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paclink
{
    public class DialogAgwEngineViewModel : IAGWEngineBacking
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private byte[] bytTCPData;
        private EventWaitHandle _testWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        public string SiteRootDirectory => Globals.SiteRootDirectory;

        public int AgwLocation => Globals.Settings.Get(Globals.strProductName, "AGW Location", 0);

        public string AgwPath => Globals.Settings.Get(Globals.strProductName, "AGW Path", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"AGWsoft\Packet Engine Pro\"));

        public string AgwHost => Globals.Settings.Get(Globals.strProductName, "AGW Host", "localhost");

        public int AgwTcpPort => Globals.Settings.Get(Globals.strProductName, "AGW Port", 8000);

        public string AgwUserId => Globals.Settings.Get(Globals.strProductName, "AGW User Id", "");

        public string AgwPassword => Globals.Settings.Get(Globals.strProductName, "AGW Password", "");

        private TcpClient _objTCPPort;

        private TcpClient objTCPPort
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

        public void TestProposedSettings(IAGWEngineWindow window, string host, int port, string userId, string password)
        {
            if (objTCPPort != null)
            {
                objTCPPort.Close();
                objTCPPort = null;
            }

            objTCPPort = new TcpClient();
            if (Globals.strLocalIPAddress != "Default")
            {
                objTCPPort.Client.Bind(new IPEndPoint(IPAddress.Parse(Globals.strLocalIPAddress), 0));
            }

            _testWaitHandle.Reset();
            objTCPPort.ConnectAsync(host.Trim(), port).ContinueWith(t =>
            {
                OnConnected(objTCPPort, window, userId.Trim(), password.Trim());

                byte[] buffer = new byte[1024];

                Task<int> task = null;
                try
                {
                    task = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                    task.ContinueWith(t =>
                    {
                        if (t.Exception == null)
                        {
                            OnDataIn(window, buffer, t.Result);
                        }
                    });
                    task.Wait(0);
                }
                catch (Exception e)
                {
                    // empty
                }
            }).Wait(0);

            if (!_testWaitHandle.WaitOne(10000))
            {
                throw new TimeoutException();
            }
            else
            {
                window.SetRemoteButtonStatus(true, IAGWEngineWindow.ButtonStatus.SUCCESS);
            }
        }

        public void SaveSettings(int location, string path, string host, int port, string userId, string password)
        {
            Globals.Settings.Save(Globals.strProductName, "AGW Location", location);
            Globals.Settings.Save(Globals.strProductName, "AGW Port", port);
            Globals.Settings.Save(Globals.strProductName, "AGW Path", path);
            Globals.Settings.Save(Globals.strProductName, "AGW Host", host);
            Globals.Settings.Save(Globals.strProductName, "AGW User Id", userId);
            Globals.Settings.Save(Globals.strProductName, "AGW Password", password);
        }

        private void OnConnected(TcpClient sender, IAGWEngineWindow window, string username, string password)
        {
            if (!string.IsNullOrEmpty(username))
            {
                LoginAGWRemote(window, username, password);  // do a secure AGWPE login 
            }

            bytTCPData = new byte[0];
            RequestAGWPortInfo(window);    // Request port info from AGWPE
        } // IPWTCPort_OnReadyToSend


        private void OnDataIn(IAGWEngineWindow window, byte[] buffer, int length)
        {
            int intDataLength;
            while (true)
            {
                try
                {
                    if (buffer == null)
                        break;
                    Globals.ConcatanateByteArrays(ref bytTCPData, buffer); // Add data to buffer array
                    if (bytTCPData.Length < 36)
                        break; // not a complete frame header
                    intDataLength = Globals.ComputeLengthL(bytTCPData, 28); // get and decode the data length field from the header
                    if (bytTCPData.Length < 36 + intDataLength)
                        break; // not A complete "G" frame...
                    if (((char)bytTCPData[4]).ToString() != "G")
                    {
                        bytTCPData = new byte[0];
                        return;
                    }

                    _testWaitHandle.Set();
                    window.SetRemoteButtonStatus(true, IAGWEngineWindow.ButtonStatus.SUCCESS);
                    objTCPPort.Close();
                    objTCPPort = null;
                }
                catch (Exception ex)
                {
                    _log.Error("[AGWEngine, tcpOnDataIn] " + ex.Message);
                    window.SetRemoteButtonStatus(true, IAGWEngineWindow.ButtonStatus.FAILED);
                }
                break;
            }

            Task<int> t = null;
            try
            {
                t = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                t.ContinueWith(k =>
                {
                    if (k.Exception == null)
                    {
                        OnDataIn(window, buffer, k.Result);
                    }
                });
                t.Wait(0);
            }
            catch (Exception e)
            {
                // empty
            }
        } // objTCPPort_OnDataIn

        private void LoginAGWRemote(IAGWEngineWindow window, string username, string password)
        {
            // Private Sub to Login to remote AGWPE with UserID and password ("P" Frame)...

            try
            {
                var bytTemp2 = new byte[546];
                var asc = new ASCIIEncoding(); // had to declare this to eliminate an ocasional error
                asc.GetBytes("P", 0, 1, bytTemp2, 4);
                Array.Copy(Globals.ComputeLengthB(255 + 255), 0, bytTemp2, 28, 4);
                asc.GetBytes(username, 0, username.Length, bytTemp2, 36);
                asc.GetBytes(password, 0, password.Length, bytTemp2, 36 + 255);
                objTCPPort.GetStream().Write(bytTemp2, 0, bytTemp2.Length); ;
            }
            catch (Exception ex)
            {
                _log.Error("[AGWEngine, LoginAGWRemote] " + ex.Message);
                _testWaitHandle.Set();
                window.SetRemoteButtonStatus(true, IAGWEngineWindow.ButtonStatus.FAILED);
            }
        } // LoginAGWRemote 

        private void RequestAGWPortInfo(IAGWEngineWindow window)
        {
            // Private Sub to Request AGW Port Information ("G" Frame)...

            var bytTemp = new byte[36];
            try
            {
                if (!objTCPPort.Connected)
                    return;
                bytTemp[4] = (byte)Globals.Asc('G');
                objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length); ;
            }
            catch (Exception ex)
            {
                _log.Error("[AGWEngine, RequestAGWPortInfo] " + ex.Message);
                _testWaitHandle.Set();
                window.SetRemoteButtonStatus(true, IAGWEngineWindow.ButtonStatus.FAILED);
            }
        } // RequestAGWPortInfo

        public void FormLoading()
        {
            // empty
        }

        public void FormLoaded()
        {
            // empty
        }

        public void FormClosing()
        {
            // empty
        }

        public void FormClosed()
        {
            // empty
        }

        public void RefreshWindow()
        {
            // empty
        }

        public void CloseWindow()
        {
            // empty
        }
    }
}
