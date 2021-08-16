using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;
using Paclink.Data;

namespace Paclink
{
    public class PrimaryThread
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private Thread thrSMTP;
        private Thread thrChannel;
        private Thread thrBearing;

        public PrimaryThread()
        {
            // 
            // Initializes the program on startup...
            // 
            int intIndex = Globals.Settings.Get("Properties", "Default Local IP Address Index", 0);
            if (intIndex < 0)
                intIndex = 0;
            if (Globals.strLocalIPAddresses.Length - 1 >= intIndex)
            {
                Globals.strLocalIPAddress = Globals.strLocalIPAddresses[intIndex];
            }

            Globals.SiteCallsign = Globals.Settings.Get("Properties", "Site Callsign", "");
            Globals.SiteGridSquare = Globals.Settings.Get("Properties", "Grid Square", "");
            Globals.intSMTPPortNumber = Globals.Settings.Get("Properties", "SMTP Port Number", 25);
            Globals.intPOP3PortNumber = Globals.Settings.Get("Properties", "POP3 Port Number", 110);
            Globals.blnLAN = Globals.Settings.Get("Properties", "LAN Connection", true);
            Globals.blnEnableRadar = Globals.Settings.Get("Properties", "Enable Radar", false);
            Globals.strServiceCodes = Globals.Settings.Get("Properties", "ServiceCodes", "");
            if (string.IsNullOrEmpty(Globals.strServiceCodes))
            {
                if (Globals.IsMARSCallsign(Globals.SiteCallsign))
                {
                    Globals.strServiceCodes = Globals.strMARSServiceCode;
                }
                else
                {
                    Globals.strServiceCodes = Globals.strHamServiceCode;
                }

                Globals.Settings.Save("Properties", "ServiceCodes", Globals.strServiceCodes);
            }
            string strSitePassword = Globals.Settings.Get("Properties", "Site Password", "");
            Globals.POP3Password = Globals.Settings.Get("Properties", "EMail Password", strSitePassword);
            Globals.Settings.Save("Properties", "EMail Password", Globals.POP3Password);
            Globals.Settings.Save("Properties", "Site Password", "");
            Globals.SecureLoginPassword = Globals.Settings.Get("Properties", "Secure Login Password", "");
            Channels.FillChannelCollection();
            try
            {
                DialogPolling.InitializePollingFlags();
                DialogAGWEngine.InitializeAGWProperties();
            }
            catch (Exception ex)
            {
                _log.Error("[Main.Startup C] " + ex.Message);
            }

            try
            {
                // Open the properties dialog box if no initial configuration has been set...
                if (DialogSiteProperties.IsValid() == false)
                {
                    var objProperties = new DialogSiteProperties();
                    objProperties.ShowDialog();
                    if (DialogSiteProperties.IsValid() == false)
                    {
                        MessageBox.Show("Paclink must have a valid initial configuration to continue...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MyApplication.Forms.Main.CloseWindow();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("[Main.Startup F] " + ex.Message);
            }

            try
            {
                Accounts.RefreshAccountsList();
            }
            catch (Exception ex)
            {
                _log.Error("[Main.Startup G] " + ex.Message);
            }

            try
            {
                MyApplication.Forms.Main.UpdateChannelList();
            }
            catch (Exception ex)
            {
                _log.Error("[Main.Startup G] " + ex.Message);
            }

            MyApplication.Forms.Main.UpdateSiteCallsign(Globals.SiteCallsign);
            if (Globals.UseRMSRelay())
            {
                Globals.queChannelDisplay.Enqueue("G*** Paclink is set to connect to RMS Relay.");
            }

            if (Globals.blnForceHFRouting)
            {
                Globals.queChannelDisplay.Enqueue("G*** Paclink is set to send messages via radio-only forwarding.");
            }

            Globals.queChannelDisplay.Enqueue("G*** Paclink " + Application.ProductVersion + " ready...");
            MyApplication.Forms.Main.EnableMainWindowInterface();
            if (thrSMTP != null)
            {
                _abortSMTPThread = true;
                thrSMTP.Join();
                thrSMTP = null;
            }

            thrSMTP = new Thread(SMTPThread);
            thrSMTP.Name = "SMTP";
            thrSMTP.Start();
            if (thrChannel != null)
            {
                _abortChannelThread = true;
                thrChannel.Join();
                thrChannel = null;
            }

            thrChannel = new Thread(ChannelThread);
            thrChannel.Name = "Channel";
            thrChannel.Start();
        }

        public void Close()
        {
            thrBearing?.Join();

            _abortSMTPThread = true;
            thrSMTP?.Join();

            _abortChannelThread = true;
            thrChannel?.Join();

            Globals.objPOP3Port?.Close();
            Globals.objSMTPPort?.Close();
        }

        private int _intDay = 99;
        private bool _abortSMTPThread;

        private void SMTPThread()
        {
            _abortSMTPThread = false;

            // Open SMTP/POP3 ports...
            try
            {
                // Clear and re establish the objSMTPPort
                if (Globals.objSMTPPort != null)
                {
                    Globals.objSMTPPort.Close();
                    Globals.objSMTPPort = null;
                }

                Globals.objSMTPPort = new SMTPPort();
                Globals.objSMTPPort.LocalPort = Globals.intSMTPPortNumber;
                Globals.objSMTPPort.Listen(true);

                // Clear and reestablish the objPOP3Port
                if (Globals.objPOP3Port != null)
                {
                    Globals.objPOP3Port.Close();
                    Globals.objPOP3Port = null;
                }

                Globals.objPOP3Port = new POP3Port();
                Globals.objPOP3Port.LocalPort = Globals.intPOP3PortNumber;
                Globals.objPOP3Port.Listen(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message + Globals.CRLF +
                    "There may be a SMTP/POP3 confilct due to another program/service listening on the POP3/SMTP Ports." +
                    " Terminate that service or change POP3/SMTP ports in Paclink and your mail client." +
                    " Check the Paclink Errors.log for details of the error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                _log.Error("[SMTPThread] SMTP/POP3 Port Setup: " + ex.Message);
            }

            var messageStoreDatabase = new MessageStore(DatabaseFactory.Get());

            do
            {
                Thread.Sleep(4000);
                if (Globals.blnProgramClosing)
                    break;
                if (_intDay != DateTime.UtcNow.Day)
                {
                    _intDay = DateTime.UtcNow.Day;
                    messageStoreDatabase.PurgeMessageIdsSeen();
                }
                // 
                // Initiates processing of any messages received from Winlink.
                // 
                try
                {
                    var messageStore = new MessageStore(DatabaseFactory.Get());
                    foreach (var message in messageStore.GetFromWinlinkMessages())
                    {
                        string strMime = UTF8Encoding.UTF8.GetString(message.Value);
                        var objWinlinkMessage = new SMTPMessage(strMime, false);
                        if (objWinlinkMessage.IsAccepted)
                        {
                            if (objWinlinkMessage.SaveMessageToAccounts() == false)
                            {
                                _log.Error("[PrimaryThreads.SMTPThread] Failure to save " + objWinlinkMessage.Mime + " to user account");
                            }
                        }
                        else
                        {
                            _log.Error("[PrimaryThreads.SMTPThread] Failure to decode " + objWinlinkMessage.Mime + " from Winlink");
                        }

                        messageStore.DeleteFromWinlinkMessage(message.Key);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("[Main.PollSMTPSide A] " + ex.Message);
                }
                // 
                // Updates the message pending counts.
                // 
                try
                {
                    var messageStore = new MessageStore(DatabaseFactory.Get());
                    var strFromMessageList = messageStore.GetToWinlinkMessages();
                    Globals.intPendingForWinlink = strFromMessageList.Count;
                    Globals.intPendingForClients = messageStore.GetNumberOfAccountEmails();
                    // 
                    // Displays the message pending counts.
                    // 
                    Globals.queSMTPStatus.Enqueue("To Clients: " + Globals.intPendingForClients.ToString() + "  To Winlink: " + Globals.intPendingForWinlink.ToString());
                }
                catch (Exception ex)
                {
                    _log.Error("[Main.PollSMTPSide B] " + ex.Message);
                }
            }
            while (!_abortSMTPThread);
        } // SMTPThread

        private int _intMinutes;
        private bool _abortChannelThread;

        private void ChannelThread()
        {
            _abortChannelThread = false;

            string strChannelName = "";
            do
            {
                Thread.Sleep(100);
                if (Globals.blnProgramClosing)
                    break;
                if (Globals.ObjSelectedModem != null)
                {
                    if (Globals.ObjSelectedModem != null)
                        Globals.ObjSelectedModem.Poll();
                    if (!string.IsNullOrEmpty(Globals.strConnectedGridSquare) & Globals.blnEnableRadar)
                    {
                        if (thrBearing == null)
                        {
                            thrBearing = new Thread(BearingThread);
                            thrBearing.Start();
                        }
                    }
                }

                // Monitors the active channel for channel end or timeout...
                if (Globals.blnChannelActive | Globals.blnStartingChannel)
                {
                    try
                    {
                        if (Globals.ObjSelectedModem == null)
                        {
                            Globals.blnStartingChannel = false;
                        }
                        else
                        {
                            if (Globals.ObjSelectedModem.State == LinkStates.LinkFailed | Globals.ObjSelectedModem.State == LinkStates.Disconnected)
                            {
                                if (Globals.blnAutoForwarding & Globals.blnFQSeen)
                                {
                                    Globals.intAutoforwardChannelIndex = 999;
                                    Globals.blnAutoForwarding = false;
                                    Globals.blnFQSeen = false;
                                }

                                Globals.blnStartingChannel = false;
                                Globals.blnEndBearingDisplay = true;
                                if (Globals.ObjSelectedModem.Close())
                                {
                                    // ObjSelectedModem = Nothing
                                    if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
                                    {
                                        if (Globals.stcSelectedChannel.EnableAutoforward == false)
                                        {
                                            if (Globals.blnPactorDialogResume & !Globals.blnPactorDialogClosing)
                                            {
                                                strChannelName = Globals.stcSelectedChannel.ChannelName;
                                            }

                                            if (Globals.blnPactorDialogClosing)
                                            {
                                                Globals.blnPactorDialogClosing = false;
                                                // blnPactorDialogResume = False
                                            }
                                        }
                                    }
                                    else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.Winmor)
                                    {
                                        if (Globals.stcSelectedChannel.EnableAutoforward == false)
                                        {
                                            if (Globals.blnWINMORDialogResume & !Globals.blnWINMORDialogClosing)
                                            {
                                                strChannelName = Globals.stcSelectedChannel.ChannelName;
                                            }

                                            if (Globals.blnWINMORDialogClosing)
                                            {
                                                Globals.blnWINMORDialogClosing = false;
                                                Globals.blnWINMORDialogResume = false;
                                            }
                                        }
                                    }
                                }

                                Globals.queStatusDisplay.Enqueue("Idle");
                                Globals.queRateDisplay.Enqueue("------");
                                Globals.queStateDisplay.Enqueue("");
                            }

                            // Force a disconnect if the channel has been held too long...
                            if (Globals.stcSelectedChannel.StartTimestamp < DateTime.Now)
                            {
                                if (Globals.ObjSelectedModem != null)
                                {
                                    Globals.ObjSelectedModem.Abort();
                                    Globals.ObjSelectedModem = null;
                                }
                                else
                                {
                                    Globals.blnStartingChannel = false;
                                }

                                Globals.blnEndBearingDisplay = true;
                            }
                        }

                        if (Globals.blnChannelActive | Globals.blnStartingChannel)
                        {
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error("PrimaryThreads.ChannelThread A] " + ex.Message);
                    }
                }
                else if (Globals.AutomaticChannels.Count > 0 & !Globals.blnStartingChannel & Globals.blnChannelActive == false)
                {
                    // Start the next channel connection in sequence if the intAutoforward 
                    // index is less than the number of available automatic channels...
                    try
                    {
                        if (Globals.intAutoforwardChannelIndex < Globals.AutomaticChannels.Count & Globals.blnAutoForwarding)
                        {
                            Globals.blnStartingChannel = true;
                            StartChannel(Globals.AutomaticChannels[Globals.intAutoforwardChannelIndex].ToString(), true);
                            Globals.intAutoforwardChannelIndex += 1;
                        }
                        else
                        {
                            Globals.blnAutoForwarding = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error("[PrimaryThreads.ChannelThread B] " + ex.Message);
                        Globals.AutomaticChannels.Clear();
                    }
                }

                // Starts a new channel poll at the end of each polling interval...
                try
                {
                    if (DialogPolling.AutoPoll)
                    {
                        if (_intMinutes != DateTime.Now.Minute)
                        {
                            _intMinutes = DateTime.Now.Minute;
                            DialogPolling.MinutesRemaining -= 1;
                        }

                        if (Globals.blnChannelActive == false & Terminal.blnTerminalIsOpen == false)
                        {
                            if (DialogPolling.MinutesRemaining <= 0)
                            {
                                if (Globals.AutomaticChannels.Count > 0)
                                {
                                    Globals.blnAutoForwarding = true;
                                    Globals.intAutoforwardChannelIndex = 0;
                                    DialogPolling.MinutesRemaining = DialogPolling.AutoPollInterval;
                                    Globals.queChannelDisplay.Enqueue("G*** Starting automatic forwarding at " + Globals.TimestampEx());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("PrimaryThreads.ChannelThread C] " + ex.Message);
                }

                // Starts a new channel poll on receipt of a new message from an SMTP client...
                try
                {
                    if (DialogPolling.PollOnReceivedMessage && Globals.intPendingForWinlink > 0 && Globals.blnAutoForwarding == false)
                    {
                        if (Globals.AutomaticChannels.Count > 0)
                        {
                            Globals.blnAutoForwarding = true;
                            Globals.intAutoforwardChannelIndex = 0;
                            DialogPolling.MinutesRemaining = DialogPolling.AutoPollInterval;
                            Globals.queChannelDisplay.Enqueue("G*** Starting automatic forwarding at " + Globals.TimestampEx());
                        }
                        else if (Globals.IsManualPactorOnly())
                        {
                            StartChannel(Globals.stcSelectedChannel.ChannelName, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("PrimaryThreads.ChannelThread D] " + ex.Message);
                }

                if (!string.IsNullOrEmpty(strChannelName))
                {
                    StartChannel(strChannelName, false);
                    strChannelName = "";
                }
            }
            while (!_abortChannelThread);
        } // ChannelThread

        private void BearingThread()
        {
            if (Globals.blnManualAbort == false)
            {
                Globals.frmBearing = new Bearing();
                Globals.blnEndBearingDisplay = false;
                Globals.frmBearing.ShowDialog();
                Globals.frmBearing = null;
                thrBearing = null;
            }
        } // BearingThread

        public void StartChannel(string strChannelName, bool blnAutomatic)
        {
            // Starts and named channel...

            Globals.strConnectedGridSquare = "";
            try
            {
                if (Channels.Entries.ContainsKey(strChannelName))
                {
                    Globals.stcSelectedChannel = Channels.Entries[strChannelName];
                }
                else
                {
                    Globals.queChannelDisplay.Enqueue("R***Channel name '" + strChannelName + "' not found");
                    return;
                }
            }
            catch (Exception ex)
            {
                _log.Error("[StartChannel] Channel name: " + strChannelName + " Automatic: " + blnAutomatic.ToString());
                _log.Error("[StartChannel] " + ex.Message);
            }

            try
            {
                Globals.queChannelDisplay.Enqueue(Globals.CLEAR);
            }
            catch (Exception ex)
            {
                _log.Error("[StartChannel] Clear channel display: " + ex.Message);
            }

            PurgeOldPartialInboundFiles(); // Clean out any partial inbound files > 24 hrs old
            try
            {
                var switchExpr = Globals.stcSelectedChannel.ChannelType;
                switch (switchExpr)
                {
                    case ChannelMode.PacketAGW:
                        {
                            Globals.ObjSelectedModem = new ModemAGW();
                            Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(60);
                            break;
                        }

                    case ChannelMode.PacketTNC:
                        {
                            var switchExpr1 = Globals.stcSelectedChannel.TNCType;
                            switch (switchExpr1)
                            {
                                case "KPC3":
                                case "KPC3+":
                                case "KPC4":
                                case "KPC9612":
                                case "KPC9612+":
                                case "KAM/+":
                                case "KAMXL":
                                case "KAM98":
                                    {
                                        Globals.ObjSelectedModem = new ModemKantronics();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(60);
                                        break;
                                    }

                                case "PK-88":
                                case "PK-96":
                                case "PK-232":
                                case "TNC2/W8DEDhost":
                                case "PK-900":
                                    {
                                        Globals.ObjSelectedModem = new ModemTimewave();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(60);
                                        break;
                                    }

                                case "PTC II":
                                case "PTC IIe":
                                case "PTC IIex":
                                case "PTC IIpro":
                                case "PTC IIusb":
                                case "PTC DR-7800":
                                    {
                                        Globals.ObjSelectedModem = new ModemSCS();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(60);
                                        break;
                                    }

                                case "TS-2000 int":
                                case "TM-D700 int":
                                case "TM-D710 int":
                                case "TM-D72":
                                case "ALINCO int":
                                case "TH-D7 int":
                                case "Generic KISS":
                                    {
                                        Globals.ObjSelectedModem = new ModemNativeKiss();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(60);
                                        break;
                                    }
                            }

                            break;
                        }

                    case ChannelMode.Telnet:
                        {
                            Globals.ObjSelectedModem = new ModemTelnet(ref Globals.stcSelectedChannel);
                            Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(20);
                            break;
                        }

                    case ChannelMode.PactorTNC:
                        {
                            var switchExpr2 = Globals.stcSelectedChannel.TNCType;
                            switch (switchExpr2)
                            {
                                case "KAM/+":
                                case "KAMXL":
                                case "KAM98":
                                    {
                                        Globals.ObjSelectedModem = new ModemKantronics();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(120);
                                        break;
                                    }

                                case "PTC II":
                                case "PTC IIe":
                                case "PTC IIex":
                                case "PTC IIpro":
                                case "PTC IIusb":
                                case "PTC DR-7800":
                                    {
                                        Globals.ObjSelectedModem = new ModemSCS();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(120);
                                        break;
                                    }

                                case "DSP-232":
                                case "PK-232": // , "PK-900"
                                    {
                                        Globals.ObjSelectedModem = new ModemTimewave();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(120);
                                        break;
                                    }
                            }

                            break;
                        }

                    case ChannelMode.Winmor:
                        {
                            break;
                        }
                        // ObjSelectedModem = New ClientWINMOR
                        // stcSelectedChannel.StartTimestamp = Now.AddMinutes(120)
                }
            }
            catch (Exception ex)
            {
                _log.Error("[StartChannel] Mode: " + Globals.stcSelectedChannel.ChannelType + Globals.stcSelectedChannel.TNCType);
                _log.Error("[StartChannel] " + ex.Message);
            }

            try
            {
                Globals.blnStartingChannel = true;
                if (Globals.ObjSelectedModem != null)
                {
                    var switchExpr3 = Globals.stcSelectedChannel.ChannelType;
                    switch (switchExpr3)
                    {
                        case ChannelMode.PacketAGW:
                        case ChannelMode.PacketTNC:
                            {
                                Globals.queChannelDisplay.Enqueue("G*** Starting Packet Channel: " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                                break;
                            }

                        case ChannelMode.PactorTNC:
                            {
                                Globals.queChannelDisplay.Enqueue("G*** Starting Pactor Channel: " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                                break;
                            }

                        case ChannelMode.Telnet:
                            {
                                Globals.queChannelDisplay.Enqueue("G*** Starting Telnet Channel: " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                                break;
                            }

                        case ChannelMode.Winmor:
                            {
                                Globals.queChannelDisplay.Enqueue("G*** Starting WINMOR Channel: " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                                break;
                            }
                    }

                    if (!Globals.ObjSelectedModem.Connect(blnAutomatic))
                    {
                        Globals.ObjSelectedModem?.Close();
                        Globals.ObjSelectedModem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    _log.Error("[StartChannel] Mode: " + Globals.stcSelectedChannel.ChannelType.ToString() + " TNC: " + Globals.stcSelectedChannel.TNCType + " State: " + Globals.ObjSelectedModem.State);
                    _log.Error("[StartChannel] " + ex.Message);
                }
                catch
                {
                }
            }
        } // StartChannel

        private void PurgeOldPartialInboundFiles()
        {
            // This checks and purges any partial inbound files over 24 hours old...

            try
            {
                var dataStore = new MessageStore(DatabaseFactory.Get());
                dataStore.PurgeOldTemporaryInboundMessages();
            }
            catch (Exception ex)
            {
                _log.Error("[PurgeOldInboundFiles] " + ex.Message);
            }
        }  // PurgeOldFiles
    }
}