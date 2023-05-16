using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using NLog;
using Paclink.UI.Common;

namespace Paclink
{
    public class Main : IMainFormBacking
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private IMainWindow _window;

        public IMainWindow MainWindow
        {
            set
            {
                _window = value;
            }
        }

        public bool UseRMSRelay
        {
            get
            {
                return Globals.UseRMSRelay();
            }
        }
        public bool PrimaryThreadExists
        {
            get
            {
                return Globals.objPrimaryThread != null;
            }
        }

        public string ProductVersion
        {
            get
            {
                return Globals.strProductVersion;
            }
        }

        public bool UpdateComplete
        {
            get
            {
                return Globals.blnUpdateComplete;
            }
        }

        public bool IsAutoPolling
        {
            get
            {
                return Globals.PollingData.AutoPoll;
            }
        }

        public int AutoPollingInterval
        {
            get
            {
                return Globals.PollingData.AutoPollInterval;
            }
        }

        public int AutoPollingMinutesRemaining
        {
            get
            {
                return Globals.PollingData.MinutesRemaining;
            }
        }

        public bool ChannelActive
        {
            get
            {
                return Globals.blnChannelActive;
            }
        }

        public bool HasSimpleTerminal
        {
            get
            {
                return Globals.TerminalIsActive;
            }
        }

        public bool HasSMTPStatus
        {
            get
            {
                return Globals.queSMTPStatus.Count > 0;
            }
        }

        public bool HasSMTPDisplay
        {
            get
            {
                return Globals.queSMTPDisplay.Count > 0;
            }
        }

        public bool HasStateDisplay
        {
            get
            {
                return Globals.queStateDisplay.Count > 0;
            }
        }

        public bool HasStatusDisplay
        {
            get
            {
                return Globals.queStatusDisplay.Count > 0;
            }
        }

        public bool HasProgressDisplay
        {
            get
            {
                return Globals.queProgressDisplay.Count > 0;
            }
        }

        public bool HasRateDisplay
        {
            get
            {
                return Globals.queRateDisplay.Count > 0;
            }
        }

        public bool HasChannelDisplay
        {
            get
            {
                return Globals.queChannelDisplay.Count > 0;
            }
        }

        public string Uptime
        {
            get
            {
                return Globals.GetUptime();
            }
        }

        public bool IsAGWUsed
        {
            get
            {
                return new DialogAgwEngineViewModel().AgwLocation != 0;
            }
        }

        public string SiteRootDirectory
        {
            get
            {
                return Globals.SiteRootDirectory;
            }
        }

        public void FormLoading()
        {
            Globals.strProductName = typeof(Main).Assembly.GetName().Name; // TBD: might need to use different property to retrieve this
            Globals.strProductVersion = typeof(Main).Assembly.GetName().Version.ToString();
            Globals.blnRunningInTestMode = false;

            var startupPath = Path.GetDirectoryName(typeof(Main).Assembly.Location);
            if (startupPath.IndexOf("Source") == -1)
            {
                Globals.SiteBinDirectory = startupPath + Path.PathSeparator.ToString();
                int intDirPtr = Globals.SiteBinDirectory.ToLower().IndexOf("bin" + Path.PathSeparator.ToString());
                if (intDirPtr != -1)
                {
                    Globals.SiteRootDirectory = Globals.SiteBinDirectory.Substring(0, intDirPtr);
                }
                else
                {
                    Globals.SiteRootDirectory = Globals.SiteBinDirectory + Path.PathSeparator.ToString();
                }
            }
            else
            {
                Globals.blnRunningInTestMode = true;
                string strProgramFiles = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                Globals.SiteRootDirectory = Path.Combine(strProgramFiles, "Paclink" + Path.PathSeparator.ToString());
                Globals.SiteBinDirectory = Globals.SiteRootDirectory + "Bin" + Path.PathSeparator.ToString();
            }

            Globals.SiteDataDirectory = Globals.SiteRootDirectory + "Data" + Path.PathSeparator.ToString();

            Globals.Settings.Save("Main", "Program Version", Globals.strProductVersion);
            Globals.blnUseRMSRelay = Globals.Settings.Get("Properties", "Use RMS Relay", false);
            Globals.strRMSRelayIPPath = Globals.Settings.Get("Properties", "Local IP Path", "");
            Globals.intRMSRelayPort = Globals.Settings.Get("Properties", "RMS Relay Port", 8772);
            Globals.blnUseExternalDNS = Globals.Settings.Get("Properties", "Use External DNS", false);
            Globals.blnForceHFRouting = Globals.Settings.Get("Properties", "Force radio-only", false);
            Globals.blnEnablAutoforward = Globals.Settings.Get("Properties", "Enable Autoforward", false);
            Globals.blnPactorDialogResume = Globals.Settings.Get("Properties", "Pactor Dialog Resume", true);

            Globals.InitializeLocalIPAddresses();
        }

        public void FormLoaded()
        {
            // 
            // Perform program initialization.
            // 
            Globals.objMain = this;

            try
            {
                _window.EnableWaitDisplay();

                /*
                // Create any required subdirectories...
                if (Directory.Exists(Globals.SiteRootDirectory + "Accounts") == false)
                {
                    Directory.CreateDirectory(Globals.SiteRootDirectory + "Accounts");
                }

                if (Directory.Exists(Globals.SiteRootDirectory + "Channels") == false)
                {
                    Directory.CreateDirectory(Globals.SiteRootDirectory + "Channels");
                }

                if (Directory.Exists(Globals.SiteRootDirectory + "Data") == false)
                {
                    Directory.CreateDirectory(Globals.SiteRootDirectory + "Data");
                }

                if (Directory.Exists(Globals.SiteRootDirectory + "Log") == false)
                {
                    Directory.CreateDirectory(Globals.SiteRootDirectory + "Log");
                }

                if (Directory.Exists(Globals.SiteRootDirectory + "Documentation") == false)
                {
                    Directory.CreateDirectory(Globals.SiteRootDirectory + "Documentation");
                }
                */
            } finally
            {
                _window.DisableWaitDisplay();
            }

            Globals.objPrimaryThread = new PrimaryThread();

            // 
            // Report what version of Paclink is running.
            // 
            Globals.PostVersionRecord();
        }

        public void AbortSelectedModem()
        {
            if (Globals.ObjSelectedModem != null)
            {
                Globals.blnManualAbort = true;
                Globals.ObjSelectedModem.Abort();
                Globals.ObjSelectedModem = null;
            }
        }

        public void FormClosing()
        {
            AbortSelectedModem();

            Globals.blnProgramClosing = true;
            Globals.PostVersionRecord(true);
            Thread.Sleep(1000);
        }

        public void FormClosed()
        {
            Globals.objPrimaryThread?.Close();
            Globals.objPrimaryThread = null;
        }

        public void StartChannelAutoconnect()
        {
            if (Globals.blnChannelActive == false)
            {
                Globals.PollingData.ResetMinutesRemaining();
                Globals.queChannelDisplay.Enqueue("G*** Starting channel autoconnect...");
                if (Globals.AutomaticChannels.Count == 0)
                {
                    Globals.queChannelDisplay.Enqueue("R*** No channel configured for autoforwarding");
                    return;
                }

                Globals.intAutoforwardChannelIndex = 0;
                Globals.blnAutoForwarding = true;
            }
        }

        public void StartChannelOnMainThread(string selectedChannel)
        {
            Globals.queStateDisplay.Enqueue("");
            Globals.objPrimaryThread.StartChannel(selectedChannel, false);
        }

        public void PostVersionRecord()
        {
            if (DateTime.UtcNow.Subtract(Globals.dttPostVersionRecord).TotalHours >= 24)
            {
                // Post a version record
                Globals.dttPostVersionRecord = DateTime.UtcNow;
                Globals.PostVersionRecord();
            }
        }

        public string GetSMTPStatus()
        {
            return Globals.queSMTPStatus.Dequeue().ToString();
        }

        public string GetSMTPDisplay()
        {
            return Globals.queSMTPDisplay.Dequeue().ToString();
        }

        public string GetStateDisplay()
        {
            return Globals.queStateDisplay.Dequeue().ToString();
        }

        public string GetStatusDisplay()
        {
            return Globals.queStatusDisplay.Dequeue().ToString();
        }

        public string GetProgressDisplay()
        {
            return Globals.queProgressDisplay.Dequeue().ToString();
        }

        public string GetRateDisplay()
        {
            return Globals.queRateDisplay.Dequeue().ToString();
        }

        public string GetChannelDisplay()
        {
            return Globals.queChannelDisplay.Dequeue().ToString();
        }

        public void UpdateChannelList(Action<string> channelAddAction)
        {
            // Fills the list of enabled automatic (B2) channels and fills
            // the "Connect to..." menu items of all enabled channels...
            Globals.AutomaticChannels.Clear();
            
            try
            {
                for (int intPriority = 1; intPriority <= 5; intPriority++)
                {
                    foreach (ChannelProperties stcChannel in Channels.Entries.Values)
                    {
                        if (stcChannel.Enabled == true & stcChannel.ChannelType != ChannelMode.Winmor)
                        {
                            if (stcChannel.Priority == intPriority)
                            {
                                channelAddAction(stcChannel.ChannelName);
                                if (stcChannel.EnableAutoforward)
                                {
                                    if (stcChannel.ChannelName != null && !string.IsNullOrEmpty(stcChannel.ChannelName))
                                    {
                                        Globals.AutomaticChannels.Add(stcChannel.ChannelName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Main:BackingObject.UpdateChannelList] " + e.Message);
            }
        }

        public void ClosePOP3AndSMTPPorts()
        {
            try
            {
                if (Globals.objSMTPPort != null)
                    Globals.objSMTPPort.Listen(false);
            }
            catch (Exception ex)
            {
                _log.Error("[Main:BackingObject.ClosePOP3AndSMTPPorts A] " + ex.Message);
            }

            try
            {
                if (Globals.objPOP3Port != null)
                    Globals.objPOP3Port.Listen(false);
            }
            catch (Exception ex)
            {
                _log.Error("[Main:BackingObject.ClosePOP3AndSMTPPorts B] " + ex.Message);
            }
        }

        public void ShowSiteProperties()
        {
            DialogSitePropertiesViewModel vm = new DialogSitePropertiesViewModel();

            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.SiteProperties, vm);
            if (vm.IsCallsignAndGridSquareValid() == false)
            {
                UserInterfaceFactory.GetUiSystem().DisplayModalError(
                    "Paclink must have a valid configuration to continue...", "Error");
                Environment.Exit(-1);
            }
        }

        public void OpenPOP3AndSMTPPorts()
        {
            try
            {
                // Open SMTP/POP3 ports...
                if (Globals.objSMTPPort != null)
                {
                    Globals.objSMTPPort.LocalPort = Globals.intSMTPPortNumber;
                    Globals.objSMTPPort.Listen(true);
                }

                if (Globals.objPOP3Port != null)
                {
                    Globals.objPOP3Port.LocalPort = Globals.intPOP3PortNumber;
                    Globals.objPOP3Port.Listen(true);
                }
            }
            catch (Exception ex)
            {
                UserInterfaceFactory.GetUiSystem().DisplayModalError("Error restarting POP3 and SMTP Ports: " + ex.Message, "Error");
            }
        }

        /* TBD: need to move each of these forms to Paclink.UI.Windows and create interfaces
         * (in Paclink.UI.Common) as well as the respective backing objects.
         */
        public void ShowTacticalAccounts()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.TacticalAccounts, new DialogTacticalAccountsViewModel());
        }

        public void ShowCallsignAccounts()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.CallsignAccounts, new DialogCallsignAccountsViewModel());
        }

        public void ShowAGWEngine()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.AgwEngine, new DialogAgwEngineViewModel());
        }

        public void ShowPacketAGWChannels()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(
                AvailableForms.AgwChannels, 
                new DialogPacketAGWChannelsViewModel(new DialogAgwEngineViewModel()));
        }

        public void ShowPacketTNCChannels()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(
                AvailableForms.TncChannels,
                new DialogPacketTNCChannelViewModel());
        }

        public void ShowTelnetChannels()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.TelnetChannels, new DialogTelnetChannelsViewModel());
        }

        public void ShowPollingInterval()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.Polling, Globals.PollingData);
        }

        public void ShowPactorChannels()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.PactorChannels, new DialogPactorTNCChannelViewModel());
        }

        public void ShowSimpleTerminal()
        {
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.Terminal, new TerminalViewModel());
        }

        public void UpdateSiteCallsign(string callsign)
        {
            _window.UpdateSiteCallsign(callsign);
        }

        public void RefreshWindow()
        {
            _window.RefreshWindow();
        }

        public void CloseWindow()
        {
            _window.CloseWindow();
        }

        public void UpdateChannelList()
        {
            _window.UpdateChannelList();
        }

        public void EnableMainWindowInterface()
        {
            _window.EnableMainWindowInterface();
        }
    }
}
