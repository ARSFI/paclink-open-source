using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Paclink
{
    public class PrimaryThread
    {
        private Thread thrSMTP;
        private Thread thrChannel;
        private Thread thrBearing;

        public PrimaryThread()
        {
            // 
            // Initializes the program on startup...
            // 
            try
            {
                if (!Globals.IsCMSavailable())
                {
                    Globals.queChannelDisplay.Enqueue("R*** No CMS site found - continuing...");
                }
            }
            catch
            {
                Globals.queChannelDisplay.Enqueue("R*** No CMS site found - continuing...");
            }

            int intIndex = Globals.objINIFile.GetInteger("Properties", "Default Local IP Address Index", 0);
            if (intIndex < 0)
                intIndex = 0;
            if (Globals.strLocalIPAddresses.Length - 1 >= intIndex)
            {
                Globals.strLocalIPAddress = Globals.strLocalIPAddresses[intIndex];
            }

            Globals.SiteCallsign = Globals.objINIFile.GetString("Properties", "Site Callsign", "");
            Globals.SiteGridSquare = Globals.objINIFile.GetString("Properties", "Grid Square", "");
            Globals.intSMTPPortNumber = Globals.objINIFile.GetInteger("Properties", "SMTP Port Number", 25);
            Globals.intPOP3PortNumber = Globals.objINIFile.GetInteger("Properties", "POP3 Port Number", 110);
            Globals.blnLAN = Globals.objINIFile.GetBoolean("Properties", "LAN Connection", true);
            Globals.blnEnableRadar = Globals.objINIFile.GetBoolean("Properties", "Enable Radar", false);
            Globals.strServiceCodes = Globals.objINIFile.GetString("Properties", "ServiceCodes", "");
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

                Globals.objINIFile.WriteString("Properties", "ServiceCodes", Globals.strServiceCodes);
            }
            // Tell WinlinkInterop what our callsign is.
            if (Globals.objWL2KInterop is object)
                Globals.objWL2KInterop.SetCallsign(Globals.SiteCallsign);
            string strSitePassword = Globals.objINIFile.GetString("Properties", "Site Password", "");
            Globals.POP3Password = Globals.objINIFile.GetString("Properties", "EMail Password", strSitePassword);
            Globals.objINIFile.WriteString("Properties", "EMail Password", Globals.POP3Password);
            Globals.objINIFile.WriteString("Properties", "Site Password", "");
            Globals.SecureLoginPassword = Globals.objINIFile.GetString("Properties", "Secure Login Password", "");
            Channels.FillChannelCollection();
            try
            {
                DialogPolling.InitializePollingFlags();
                DialogAGWEngine.InitializeAGWProperties();
            }
            catch
            {
                Logs.Exception("[Main.Startup C] " + Information.Err().Description);
            }

            try
            {
                // Process any downloaded support files in file called PaclinkSupport.zip and unzips them to
                // either the Documentation or Data directories depending on extension.
                if (File.Exists(Globals.SiteBinDirectory + "PaclinkSupport.zip"))
                {
                    Globals.queChannelDisplay.Enqueue("G*** Processing downloaded support file PaclinkSupport.zip...");
                    var objSupport = new FileSupport();
                    objSupport.ProcessSupportFile(Globals.SiteBinDirectory + "PaclinkSupport.zip");
                    objSupport = null;
                    // Upon successful extraction the zip file is deleted
                }
            }
            catch
            {
                Logs.Exception("[Main.Startup D] " + Information.Err().Description);
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
                        Interaction.MsgBox("Paclink must have a valid initial configuration to continue...", MsgBoxStyle.Information);
                        My.MyProject.Forms.Main.Close();
                        return;
                    }
                }
            }
            catch
            {
                Logs.Exception("[Main.Startup F] " + Information.Err().Description);
            }

            try
            {
                Accounts.RefreshAccountsList();
            }
            catch
            {
                Logs.Exception("[Main.Startup G] " + Information.Err().Description);
            }

            try
            {
                My.MyProject.Forms.Main.UpdateChannelsList();
            }
            catch
            {
                Logs.Exception("[Main.Startup G] " + Information.Err().Description);
            }

            My.MyProject.Forms.Main.Text = "Paclink - " + Globals.SiteCallsign;
            if (Globals.UseRMSRelay())
            {
                Globals.queChannelDisplay.Enqueue("G*** Paclink is set to connect to RMS Relay.");
            }

            if (Globals.blnForceHFRouting)
            {
                Globals.queChannelDisplay.Enqueue("G*** Paclink is set to send messages via radio-only forwarding.");
            }

            Globals.queChannelDisplay.Enqueue("G*** Paclink " + Application.ProductVersion + " ready...");
            My.MyProject.Forms.Main.mnuMain.Enabled = true;
            if (!Information.IsNothing(thrSMTP))
            {
                thrSMTP.Abort();
                thrSMTP = null;
            }

            thrSMTP = new Thread(SMTPThread);
            thrSMTP.Name = "SMTP";
            thrSMTP.Start();
            if (!Information.IsNothing(thrChannel))
            {
                thrChannel.Abort();
                thrChannel = null;
            }

            thrChannel = new Thread(ChannelThread);
            thrChannel.Name = "Channel";
            thrChannel.Start();
        } // New

        public void Close()
        {
            if (thrBearing is object)
                thrBearing.Abort();
            if (thrSMTP is object)
                thrSMTP.Abort();
            if (thrChannel is object)
                thrChannel.Abort();
            if (Globals.objPOP3Port is object)
                Globals.objPOP3Port.Close();
            if (Globals.objSMTPPort is object)
                Globals.objSMTPPort.Close();
        } // Close

        private int intDay = 99;
        private void SMTPThread()
        {
            // Open SMTP/POP3 ports...
            try
            {
                // Clear and re establish the objSMTPPort
                if (!Information.IsNothing(Globals.objSMTPPort))
                {
                    Globals.objSMTPPort.Close();
                    Globals.objSMTPPort = null;
                }

                Globals.objSMTPPort = new SMTPPort();
                Globals.objSMTPPort.LocalPort = Globals.intSMTPPortNumber;
                Globals.objSMTPPort.Listen(true);

                // Clear and reestablish the objPOP3Port
                if (!Information.IsNothing(Globals.objPOP3Port))
                {
                    Globals.objPOP3Port.Close();
                    Globals.objPOP3Port = null;
                }

                Globals.objPOP3Port = new POP3Port();
                Globals.objPOP3Port.LocalPort = Globals.intPOP3PortNumber;
                Globals.objPOP3Port.Listen(true);
            }
            catch
            {
                Interaction.MsgBox(Information.Err().Description + Globals.CRLF + "There may be a SMTP/POP3 confilct due to another program/service listening on the POP3/SMTP Ports." + " Terminate that service or change POP3/SMTP ports in Paclink and your mail client." + " Check the Paclink Errors.log for details of the error.", MsgBoxStyle.Critical);

                Logs.Exception("[SMTPThread] SMTP/POP3 Port Setup: " + Information.Err().Description);
            }

            do
            {
                Thread.Sleep(4000);
                if (Globals.blnProgramClosing)
                    break;
                if (intDay != DateTime.UtcNow.Day)
                {
                    intDay = DateTime.UtcNow.Day;
                    MidsSeen.PurgeMidsSeenFile();
                }
                // 
                // Initiates processing of any messages received from Winlink.
                // 
                try
                {
                    var strWinlinkMessages = Directory.GetFiles(Globals.SiteRootDirectory + @"From Winlink\", "*.mime");
                    foreach (string strWinlinkMessage in strWinlinkMessages)
                    {
                        string strMime = My.MyProject.Computer.FileSystem.ReadAllText(strWinlinkMessage);
                        var objWinlinkMessage = new SMTPMessage(strMime, false);
                        if (objWinlinkMessage.IsAccepted)
                        {
                            if (objWinlinkMessage.SaveMessageToAccounts() == false)
                            {
                                Logs.Exception("[PrimaryThreads.SMTPThread] Failure to save " + objWinlinkMessage.Mime + " to user account");
                            }
                        }
                        else
                        {
                            Logs.Exception("[PrimaryThreads.SMTPThread] Failure to decode " + objWinlinkMessage.Mime + " from Winlink");
                        }

                        File.Delete(strWinlinkMessage);
                    }
                }
                catch
                {
                    Logs.Exception("[Main.PollSMTPSide A] " + Information.Err().Description);
                }
                // 
                // Updates the message pending counts.
                // 
                try
                {
                    var strFileList = Directory.GetFiles(Globals.SiteRootDirectory + @"To Winlink\", "*.mime");
                    Globals.intPendingForWinlink = strFileList.Length;
                    strFileList = Directory.GetFiles(Globals.SiteRootDirectory + @"Accounts\", "*.mime", SearchOption.AllDirectories);
                    Globals.intPendingForClients = strFileList.Length;
                    // 
                    // Displays the message pending counts.
                    // 
                    Globals.queSMTPStatus.Enqueue("To Clients: " + Globals.intPendingForClients.ToString() + "  To Winlink: " + Globals.intPendingForWinlink.ToString());
                }
                catch
                {
                    Logs.Exception("[Main.PollSMTPSide B] " + Information.Err().Description);
                }
            }
            while (true);
        } // SMTPThread

        private int intMinutes = 0;
        private void ChannelThread()
        {
            string strChannelName = "";
            do
            {
                Thread.Sleep(100);
                if (Globals.blnProgramClosing)
                    break;
                if (Globals.objSelectedClient is object)
                {
                    if (!Information.IsNothing(Globals.objSelectedClient))
                        Globals.objSelectedClient.Poll();
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
                        if (Globals.objSelectedClient == null)
                        {
                            Globals.blnStartingChannel = false;
                        }
                        else
                        {
                            if (Globals.objSelectedClient.State == ELinkStates.LinkFailed | Globals.objSelectedClient.State == ELinkStates.Disconnected)
                            {
                                if (Globals.blnAutoForwarding & Globals.blnFQSeen)
                                {
                                    Globals.intAutoforwardChannelIndex = 999;
                                    Globals.blnAutoForwarding = false;
                                    Globals.blnFQSeen = false;
                                }

                                Globals.blnStartingChannel = false;
                                Globals.blnEndBearingDisplay = true;
                                if (Globals.objSelectedClient.Close() == true)
                                {
                                    // objSelectedClient = Nothing
                                    if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
                                    {
                                        if (Globals.stcSelectedChannel.EnableAutoforward == false)
                                        {
                                            if (Globals.blnPactorDialogResume == true & Globals.blnPactorDialogClosing == false)
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
                                    else if (Globals.stcSelectedChannel.ChannelType == EChannelModes.Winmor)
                                    {
                                        if (Globals.stcSelectedChannel.EnableAutoforward == false)
                                        {
                                            if (Globals.blnWINMORDialogResume == true & Globals.blnWINMORDialogClosing == false)
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
                                if (Globals.objSelectedClient is object)
                                {
                                    Globals.objSelectedClient.Abort();
                                    Globals.objSelectedClient = null;
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
                    catch
                    {
                        Logs.Exception("PrimaryThreads.ChannelThread A] " + Information.Err().Description);
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
                    catch
                    {
                        Logs.Exception("[PrimaryThreads.ChannelThread B] " + Information.Err().Description);
                        Globals.AutomaticChannels.Clear();
                    }
                }

                // Starts a new channel poll at the end of each polling interval...
                try
                {
                    if (DialogPolling.AutoPoll)
                    {
                        if (intMinutes != DateTime.Now.Minute)
                        {
                            intMinutes = DateTime.Now.Minute;
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
                catch
                {
                    Logs.Exception("PrimaryThreads.ChannelThread C] " + Information.Err().Description);
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
                catch
                {
                    Logs.Exception("PrimaryThreads.ChannelThread D] " + Information.Err().Description);
                }

                if (!string.IsNullOrEmpty(strChannelName))
                {
                    StartChannel(strChannelName, false);
                    strChannelName = "";
                }
            }
            while (true);
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
                if (Channels.Entries.Contains(strChannelName))
                {
                    Globals.stcSelectedChannel = (TChannelProperties)Channels.Entries[strChannelName];
                }
                else
                {
                    Globals.queChannelDisplay.Enqueue("R***Channel name '" + strChannelName + "' not found");
                    return;
                }
            }
            catch
            {
                Logs.Exception("[StartChannel] Channel name: " + strChannelName + " Automatic: " + blnAutomatic.ToString());
                Logs.Exception("[StartChannel] " + Information.Err().Description);
            }

            try
            {
                Globals.queChannelDisplay.Enqueue(Globals.CLEAR);
            }
            catch
            {
                Logs.Exception("[StartChannel] Clear channel display: " + Information.Err().Description);
            }

            PurgeOldPartialInboundFiles(); // Clean out any partial inbound files > 24 hrs old
            try
            {
                var switchExpr = Globals.stcSelectedChannel.ChannelType;
                switch (switchExpr)
                {
                    case EChannelModes.PacketAGW:
                        {
                            Globals.objSelectedClient = new ClientAGW();
                            Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(60);
                            break;
                        }

                    case EChannelModes.PacketTNC:
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
                                        Globals.objSelectedClient = new ClientKantronics();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(60);
                                        break;
                                    }

                                case "PK-88":
                                case "PK-96":
                                case "PK-232":
                                case "TNC2/W8DEDhost":
                                case "PK-900":
                                    {
                                        Globals.objSelectedClient = new ClientTimewave();
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
                                        Globals.objSelectedClient = new ClientSCS();
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
                                        Globals.objSelectedClient = new ClientNativeKISS();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(60);
                                        break;
                                    }
                            }

                            break;
                        }

                    case EChannelModes.Telnet:
                        {
                            Globals.objSelectedClient = new ClientTelnet(ref Globals.stcSelectedChannel);
                            Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(20);
                            break;
                        }

                    case EChannelModes.PactorTNC:
                        {
                            var switchExpr2 = Globals.stcSelectedChannel.TNCType;
                            switch (switchExpr2)
                            {
                                case "KAM/+":
                                case "KAMXL":
                                case "KAM98":
                                    {
                                        Globals.objSelectedClient = new ClientKantronics();
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
                                        Globals.objSelectedClient = new ClientSCS();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(120);
                                        break;
                                    }

                                case "DSP-232":
                                case "PK-232": // , "PK-900"
                                    {
                                        Globals.objSelectedClient = new ClientTimewave();
                                        Globals.stcSelectedChannel.StartTimestamp = DateTime.Now.AddMinutes(120);
                                        break;
                                    }
                            }

                            break;
                        }

                    case EChannelModes.Winmor:
                        {
                            break;
                        }
                        // objSelectedClient = New ClientWINMOR
                        // stcSelectedChannel.StartTimestamp = Now.AddMinutes(120)
                }
            }
            catch
            {
                Logs.Exception("[StartChannel] Mode: " + Globals.stcSelectedChannel.ChannelType.ToString() + Globals.stcSelectedChannel.TNCType.ToString());
                Logs.Exception("[StartChannel] " + Information.Err().Description);
            }

            try
            {
                Globals.blnStartingChannel = true;
                if (Globals.objSelectedClient is object)
                {
                    var switchExpr3 = Globals.stcSelectedChannel.ChannelType;
                    switch (switchExpr3)
                    {
                        case EChannelModes.PacketAGW:
                        case EChannelModes.PacketTNC:
                            {
                                Globals.queChannelDisplay.Enqueue("G*** Starting Packet Channel: " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                                break;
                            }

                        case EChannelModes.PactorTNC:
                            {
                                Globals.queChannelDisplay.Enqueue("G*** Starting Pactor Channel: " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                                break;
                            }

                        case EChannelModes.Telnet:
                            {
                                Globals.queChannelDisplay.Enqueue("G*** Starting Telnet Channel: " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                                break;
                            }

                        case EChannelModes.Winmor:
                            {
                                Globals.queChannelDisplay.Enqueue("G*** Starting WINMOR Channel: " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                                break;
                            }
                    }

                    if (!Globals.objSelectedClient.Connect(blnAutomatic))
                    {
                        if (Globals.objSelectedClient is object)
                        {
                            Globals.objSelectedClient.Close();
                        }

                        Globals.objSelectedClient = null;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Logs.Exception("[StartChannel] Mode: " + Globals.stcSelectedChannel.ChannelType.ToString() + " TNC: " + Globals.stcSelectedChannel.TNCType.ToString() + " State: " + Globals.objSelectedClient.State.ToString());
                    Logs.Exception("[StartChannel] " + Information.Err().Description);
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
                var objDirectoryInfo = new DirectoryInfo(Globals.SiteRootDirectory + "Temp Inbound");
                var objFileInfo = objDirectoryInfo.GetFiles("*.indata");
                foreach (FileInfo objFile in objFileInfo)
                {
                    if (DateTime.Now.Subtract(objFile.LastWriteTime).TotalHours > 24)
                    {
                        try
                        {
                            File.Delete(objFile.FullName);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
                Logs.Exception("[PurgeOldInboundFiles] " + Information.Err().Description);
            }
        }  // PurgeOldFiles
    }
}