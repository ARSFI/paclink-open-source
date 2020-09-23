using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;

namespace Paclink
{
    public partial class Main
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public Main()
        {
            InitializeComponent();
            _barStatus.Name = "barStatus";
            _lblChannelStatus.Name = "lblChannelStatus";
            _ChannelRate.Name = "ChannelRate";
            _barChannelProgress.Name = "barChannelProgress";
            _lblSMTPStatus.Name = "lblSMTPStatus";
            _lblProgramVersion.Name = "lblProgramVersion";
            _lblUptime.Name = "lblUptime";
            _lblMessageDisplay.Name = "lblMessageDisplay";
            _pnlSplitter.Name = "pnlSplitter";
            _ChannelDisplay.Name = "ChannelDisplay";
            _SMTPDisplay.Name = "SMTPDisplay";
            _mnuMain.Name = "mnuMain";
            _mnuFile.Name = "mnuFile";
            _mnuProperties.Name = "mnuProperties";
            _mnuPollingInterval.Name = "mnuPollingInterval";
            _ToolStripSeparator3.Name = "ToolStripSeparator3";
            _mnuAGWEngine.Name = "mnuAGWEngine";
            _mnuPacketAGWChannels.Name = "mnuPacketAGWChannels";
            _mnuPacketTNCChannels.Name = "mnuPacketTNCChannels";
            _mnuPactorChannels.Name = "mnuPactorChannels";
            _mnuTelnetChannels.Name = "mnuTelnetChannels";
            _ToolStripSeparator4.Name = "ToolStripSeparator4";
            _mnuTacticalAccounts.Name = "mnuTacticalAccounts";
            _mnuCallsignAccounts.Name = "mnuCallsignAccounts";
            _mnuEditAccount.Name = "mnuEditAccount";
            _ToolStripSeparator1.Name = "ToolStripSeparator1";
            _mnuBackup.Name = "mnuBackup";
            _mnuRestoreSettings.Name = "mnuRestoreSettings";
            _ToolStripSeparator2.Name = "ToolStripSeparator2";
            _mnuExit.Name = "mnuExit";
            _mnuConnect.Name = "mnuConnect";
            _mnuAutoConnect.Name = "mnuAutoConnect";
            _mnuConnectTo.Name = "mnuConnectTo";
            _mnuAbort.Name = "mnuAbort";
            _mnuLogs.Name = "mnuLogs";
            _mnuHelp.Name = "mnuHelp";
            _mnuHelpContents.Name = "mnuHelpContents";
            _mnuSimpleTerminal.Name = "mnuSimpleTerminal";
            _mnuMinutesRemaining.Name = "mnuMinutesRemaining";
            _mnuTest.Name = "mnuTest";

            _log.Info("Starting Paclink");
        }

        private string strChannelSelection;
        private bool blnStartAutoconnect;
        private System.Timers.Timer tmrStartChannel;

        private void ChannelDisplayWrite(string strText)
        {
            if ((strText ?? "") == Globals.CLEAR)
            {
                ChannelDisplay.Clear();
                return;
            }
            else if (string.IsNullOrEmpty(strText))
            {
                // Do nothing
                return;
            }

            var objColor = ColorSelection(strText);
            strText = strText.Substring(1);
            try
            {
                ChannelDisplay.SelectionColor = objColor;
                ChannelDisplay.SelectionStart = ChannelDisplay.Text.Length;
                ChannelDisplay.AppendText(strText + Globals.CRLF);
                ChannelDisplay.SelectionLength = strText.Length;
                ChannelDisplay.Select();
                ChannelDisplay.SelectionLength = 0;
                ChannelDisplay.Refresh();
            }
            catch
            {
                // Do nothing...
            }

            switch (objColor)
            {
                case var @case when @case == Color.Red:
                case var case1 when case1 == Color.Green:
                    {
                        _log.Debug(strText);
                        break;
                    }

                case var case2 when case2 == Color.Purple:
                    {
                        _log.Debug(strText);
                        break;
                    }

                case var case3 when case3 == Color.Black:
                    {
                        _log.Debug("> " + strText);
                        break;
                    }

                case var case4 when case4 == Color.Blue:
                    {
                        _log.Debug("< " + strText);
                        break;
                    }
            }
        }

        private void ChannelSelection(object s, EventArgs e)
        {
            // Responds to a manual channel selection event...
            blnStartAutoconnect = false;
            strChannelSelection = ((ToolStripMenuItem)s).Text;
            tmrStartChannel = new System.Timers.Timer(100);
            tmrStartChannel.Elapsed += StartChannelEvent;
            tmrStartChannel.AutoReset = false;
            tmrStartChannel.Start();
            mnuConnect.Enabled = false;
        } // ChannelSelection

        private Color ColorSelection(string strColor)
        {
            var switchExpr = strColor[0];
            switch (switchExpr)
            {
                case 'R':
                    {
                        return Color.Red;
                    }

                case 'B':
                    {
                        return Color.Blue;
                    }

                case 'G':
                    {
                        return Color.Green;
                    }

                case 'P':
                    {
                        return Color.Purple;
                    }

                default:
                    {
                        return Color.Black;
                    }
            }
        } // ColorSelection

        private void Initialize()
        {
            // 
            // Perform program initialization.
            // 
            Globals.objMain = this;
            Cursor = Cursors.WaitCursor;
            ChannelDisplay.Cursor = Cursors.WaitCursor;
            SMTPDisplay.Cursor = Cursors.WaitCursor;
            ChannelDisplay.ForeColor = Color.Green;
            if (Globals.UseRMSRelay())
            {
                ChannelDisplay.Text = "*** Initializing. Paclink is set to connect to RMS Relay.  RMS Relay must be running." + Globals.CRLF;
            }
            else
            {
                ChannelDisplay.Text = "*** Initializing..." + Globals.CRLF;
            }

            Refresh();

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

            if (Directory.Exists(Globals.SiteRootDirectory + "From Winlink") == false)
            {
                Directory.CreateDirectory(Globals.SiteRootDirectory + "From Winlink");
            }

            if (Directory.Exists(Globals.SiteRootDirectory + "To Winlink") == false)
            {
                Directory.CreateDirectory(Globals.SiteRootDirectory + "To Winlink");
            }

            if (Directory.Exists(Globals.SiteRootDirectory + "Temp Inbound") == false)
            {
                Directory.CreateDirectory(Globals.SiteRootDirectory + "Temp Inbound"); // Used to save partial inbound messages
            }

            Globals.objPrimaryThread = new PrimaryThread();
            Cursor = Cursors.Default;
            ChannelDisplay.Cursor = Cursors.Default;
            SMTPDisplay.Cursor = Cursors.Default;
            // 
            // Report what version of Paclink is running.
            // 
            Globals.PostVersionRecord();
        }

        private void MainClosed(object sender, FormClosedEventArgs e)
        {
            Globals.objPrimaryThread?.Close();
            Globals.objPrimaryThread = null;
            Globals.objWL2KInterop?.Close();
            Globals.objWL2KInterop = null;
        }

        private void MainClosing(object s, FormClosingEventArgs e)
        {
            if (Globals.objSelectedClient != null)
            {
                Globals.blnManualAbort = true;
                Globals.objSelectedClient.Abort();
                Globals.objSelectedClient = null;
            }

            Globals.blnProgramClosing = true;
            Globals.PostVersionRecord(true);
            Thread.Sleep(1000);
        }

        private void MainLoad(object s, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Globals.strProductName = Application.ProductName;
            Globals.strProductVersion = Application.ProductVersion;
            Globals.blnRunningInTestMode = false;
            if (Application.StartupPath.IndexOf("Source") == -1)
            {
                Globals.SiteBinDirectory = Application.StartupPath + @"\";
                int intDirPtr = Globals.SiteBinDirectory.ToLower().IndexOf(@"bin\");
                if (intDirPtr != -1)
                {
                    Globals.SiteRootDirectory = Globals.SiteBinDirectory.Substring(0, intDirPtr);
                }
            }
            else
            {
                Globals.blnRunningInTestMode = true;
                string strProgramFiles = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                Globals.SiteRootDirectory = Path.Combine(strProgramFiles, @"Paclink\");
                Globals.SiteBinDirectory = Globals.SiteRootDirectory + @"Bin\";
            }

            Globals.SiteDataDirectory = Globals.SiteRootDirectory + @"Data\";
            if (Globals.SiteBinDirectory.ToLower().IndexOf("bin") == -1)
            {
                MessageBox.Show(
                    "Illegal Paclink directory structure...Paclink.exe " + @"Must be in the \Bin subdirectory as installed!",
                    "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }

            lblProgramVersion.Text = "Version: " + Globals.strProductVersion;
            Globals.Settings.Save("Main", "Program Version", Application.ProductVersion);
            Globals.blnUseRMSRelay = Globals.Settings.Get("Properties", "Use RMS Relay", false);
            Globals.strRMSRelayIPPath = Globals.Settings.Get("Properties", "Local IP Path", "");
            Globals.intRMSRelayPort = Globals.Settings.Get("Properties", "RMS Relay Port", 8772);
            Globals.blnUseExternalDNS = Globals.Settings.Get("Properties", "Use External DNS", false);
            Globals.blnForceHFRouting = Globals.Settings.Get("Properties", "Force radio-only", false);
            Globals.blnEnablAutoforward = Globals.Settings.Get("Properties", "Enable Autoforward", false);
            Globals.blnPactorDialogResume = Globals.Settings.Get("Properties", "Pactor Dialog Resume", true);
            Globals.objWL2KInterop.SetWebServiceKey("CC5E139204DA41A3B544A5F2CEB21051");

            lblMessageDisplay.Enabled = true;
            lblMessageDisplay.BackColor = ChannelDisplay.BackColor;
            barChannelProgress.Enabled = false;
            mnuMinutesRemaining.Visible = false;
            Show();

            // Save the references to the display components on this form
            Globals.DisplayForm = this;
            Globals.MainMenu = mnuMain;
            Globals.MainMenuConnect = mnuConnect;
            Globals.MainMenuAbort = mnuAbort;
            Globals.InitializeLocalIPAddresses();
            tmrMain.Start();
        } // MainLoad

        private void SMTPDisplayWrite(string strText)
        {
            if ((strText ?? "") == Globals.CLEAR)
            {
                SMTPDisplay.Clear();
                return;
            }
            else if (string.IsNullOrEmpty(strText))
            {
                // Do nothing
                return;
            }

            var objColor = ColorSelection(strText);
            strText = strText.Substring(1);
            try
            {
                SMTPDisplay.SelectionColor = objColor;
                SMTPDisplay.SelectionStart = SMTPDisplay.Text.Length;
                SMTPDisplay.AppendText(strText + Globals.CRLF);
                SMTPDisplay.SelectionLength = strText.Length;
                SMTPDisplay.Select();
                SMTPDisplay.SelectionLength = 0;
                SMTPDisplay.Refresh();
            }
            catch
            {
                // Do nothing
            }
        } // SMTPDisplayWrite

        private void StartChannelEvent(object s, System.Timers.ElapsedEventArgs e)
        {
            tmrStartChannel.Stop();
            tmrStartChannel.Dispose();
            tmrStartChannel = null;
            if (blnStartAutoconnect)
            {
                blnStartAutoconnect = false;
                if (Globals.blnChannelActive == false)
                {
                    DialogPolling.MinutesRemaining = DialogPolling.AutoPollInterval;
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
            else if (Globals.objPrimaryThread != null)
            {
                Globals.queStateDisplay.Enqueue("");
                Globals.objPrimaryThread.StartChannel(strChannelSelection, false);
            }
        }

        // 
        // The main timer tick has occurred.
        //
        static int intHour = 99;
        private bool blnMainStarted = false;
        private DateTime dttCMSCheck = DateTime.MinValue;
        private void tmrMain_Tick(object sender, EventArgs e)
        {
            if (dttCMSCheck == DateTime.MinValue)
            {
                dttCMSCheck = DateTime.Now;
            }

            tmrMain.Enabled = false;
            if (Globals.blnUpdateComplete & !Text.StartsWith("*"))
            {
                Text = "*" + Text;
            }

            if (blnMainStarted == false)
            {
                blnMainStarted = true;
                Initialize();
                tmrMain.Interval = 10000; // 10 seconds
                tmrMain.Enabled = true;
                return;
            }

            if (intHour != DateTime.Now.Hour)
            {
                // We just entered a new hour.
                intHour = DateTime.Now.Hour;
                //TODO: Establish backup process
                //Globals.objINIFile.CheckBackupIni();
                // See if it's time to post a version record
                if (DateTime.UtcNow.Subtract(Globals.dttPostVersionRecord).TotalHours >= 24)
                {
                    // Post a version record
                    Globals.dttPostVersionRecord = DateTime.UtcNow;
                    Globals.PostVersionRecord();
                }
            }

            if (DialogPolling.AutoPoll)
            {
                mnuMinutesRemaining.Visible = true;
                int intInterval = DialogPolling.MinutesRemaining;
                if (intInterval > DialogPolling.AutoPollInterval)
                    intInterval = DialogPolling.AutoPollInterval;
                if (intInterval < 0)
                    intInterval = 0;
                if (intInterval == 1)
                {
                    mnuMinutesRemaining.Text = "Next Poll in less than 1 Minute";
                }
                else
                {
                    mnuMinutesRemaining.Text = "Next Poll in " + intInterval.ToString() + " Minutes";
                }
            }
            else
            {
                mnuMinutesRemaining.Visible = false;
            }

            tmrMain.Enabled = true;
        } // tmrMain_Tick

        private void tmrDisplay_Tick(object sender, EventArgs e)
        {
            string strText;
            try
            {
                if (Globals.blnChannelActive == false & Globals.objTerminal == null)
                {
                    mnuConnect.Enabled = true;
                }
                else
                {
                    mnuConnect.Enabled = false;
                }

                while (Globals.queSMTPStatus.Count > 0)
                    lblSMTPStatus.Text = Globals.queSMTPStatus.Dequeue().ToString();
                while (Globals.queStateDisplay.Count > 0)
                    lblMessageDisplay.Text = Globals.queStateDisplay.Dequeue().ToString();
                while (Globals.queStatusDisplay.Count > 0)
                    lblChannelStatus.Text = Globals.queStatusDisplay.Dequeue().ToString();
                while (Globals.queProgressDisplay.Count > 0)
                    barChannelProgress.Value = Convert.ToInt32(Globals.queProgressDisplay.Dequeue());
                while (Globals.queRateDisplay.Count > 0)
                    ChannelRate.Text = Globals.queRateDisplay.Dequeue().ToString();
                while (Globals.queSMTPDisplay.Count > 0)
                {
                    strText = Globals.queSMTPDisplay.Dequeue().ToString();
                    SMTPDisplayWrite(strText);
                }

                while (Globals.queChannelDisplay.Count > 0)
                {
                    strText = Globals.queChannelDisplay.Dequeue().ToString();
                    ChannelDisplayWrite(strText);
                }

                lblUptime.Text = Globals.GetUptime();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
        } // tmrDisplay

        private void txtStateDisplay_KeyPress(object sender, KeyPressEventArgs e)
        {
            lblMessageDisplay.Text = "";
            e.Handled = true;
        } // txtStateDisplay_KeyPress

        public void UpdateChannelsList()
        {
            // Fills the list of enabled automatic (B2) channels and fills
            // the "Connect to..." menu items of all enabled channels...
            Globals.AutomaticChannels.Clear();
            mnuConnectTo.DropDownItems.Clear();
            try
            {
                for (int intPriority = 1; intPriority <= 5; intPriority++)
                {
                    foreach (TChannelProperties stcChannel in Channels.Entries.Values)
                    {
                        if (stcChannel.Enabled == true & stcChannel.ChannelType != EChannelModes.Winmor)
                        {
                            if (stcChannel.Priority == intPriority)
                            {
                                var mnuItem = new ToolStripMenuItem(stcChannel.ChannelName);
                                mnuItem.Click += ChannelSelection;
                                mnuConnectTo.DropDownItems.Add(mnuItem);
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
                _log.Error("Main.UpdateChannelsList A] " + e.Message);
            }

            try
            {
                if (mnuConnectTo.DropDownItems.Count == 0)
                {
                    var mnuItem = new ToolStripMenuItem("No active channel configured...");
                    mnuItem.Click += ChannelSelection;
                    mnuConnectTo.DropDownItems.Add(mnuItem);
                }
            }
            catch (Exception e)
            {
                _log.Error("Main.UpdateChannelsList B] " + e.Message);
            }
        } // UpdateChannelsList

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void mnuProperties_Click(object sender, EventArgs e)
        {
            // Temporarily close SMTP/POP3 ports...
            try
            {
                if (Globals.objSMTPPort != null)
                    Globals.objSMTPPort.Listen(false);
            }
            catch (Exception ex)
            {
                _log.Error("[Main.mnuProperties_Click A] " + ex.Message);
            }

            try
            {
                if (Globals.objPOP3Port != null)
                    Globals.objPOP3Port.Listen(false);
            }
            catch (Exception ex)
            {
                _log.Error("[Main.mnuProperties_Click B] " + ex.Message);
            }

            var dlgProperties = new DialogSiteProperties();
            dlgProperties.ShowDialog();
            if (DialogSiteProperties.IsValid() == false)
            {
                MessageBox.Show("Paclink must have a valid configuration to continue...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }

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
                MessageBox.Show("Error restarting POP3 and SMTP Ports: " + ex.Message);
            }
        } // mnuProperties_Click

        private void mnuAccounts_Click(object sender, EventArgs e)
        {
            var dlgTacticalAccounts = new DialogTacticalAccounts();
            dlgTacticalAccounts.ShowDialog();
        } // mnuAccounts_Click

        private void mnuCallsignAccounts_Click(object sender, EventArgs e)
        {
            var dlgCallsignAccount = new DialogCallsignAccounts();
            dlgCallsignAccount.ShowDialog();
        } // mnuCallsignAccounts_Click

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        } // mnuExit_Click

        private void mnuAGWEngine_Click(object sender, EventArgs e)
        {
            var dlgAGWEngine = new DialogAGWEngine();
            dlgAGWEngine.ShowDialog();
        } // mnuAGWEngine_Click

        private void mnuPacketAGWChannels_Click(object sender, EventArgs e)
        {
            var dlgPacketAGWChannels = new DialogPacketAGWChannels();
            dlgPacketAGWChannels.ShowDialog();
            UpdateChannelsList();
        } // mnuPacketChannels_Click

        private void mnuPacketTNCChannels_Click(object sender, EventArgs e)
        {
            var dlgPacketTNCChannels = new DialogPacketTNCChannels();
            dlgPacketTNCChannels.ShowDialog();
            UpdateChannelsList();
        } // mnuPacketTNCChannels_Click

        private void mnuTelnetChannels_Click(object sender, EventArgs e)
        {
            var dlgTelnetChannels = new DialogTelnetChannels();
            dlgTelnetChannels.ShowDialog();
            UpdateChannelsList();
        } // mnuTelnetChannels_Click

        private void mnuPollingInterval_Click(object sender, EventArgs e)
        {
            var dlgPolling = new DialogPolling();
            dlgPolling.ShowDialog();
        } // mnuPollingInterval_Click

        private void mnuConnect_Click(object sender, EventArgs e)
        {
            // Updates the available channels lists on manual connect...
            UpdateChannelsList();
        } // mnuConnect_Click

        private void mnuAutoConnect_Click(object sender, EventArgs e)
        {
            blnStartAutoconnect = true;
            tmrStartChannel = new System.Timers.Timer(100);
            tmrStartChannel.Elapsed += StartChannelEvent;
            tmrStartChannel.AutoReset = false;
            tmrStartChannel.Start();
        } // mnuAutoConnect_Click

        private void mnuAbort_Click(object sender, EventArgs e)
        {
            mnuAbort.Enabled = false;
            if (Globals.objSelectedClient != null)
            {
                Globals.blnManualAbort = true;
                Globals.objSelectedClient.Abort();
                Globals.objSelectedClient = null;
            }

            ChannelDisplayWrite("G*** Restarting Paclink after Abort clicked...");
            //TODO: Application.Restart(); -- this may not be necessary
            Restart();
        } // mnuAbort_Click

        //work-around for .net core not (currently) supporting Application.Restart()
        //from https://github.com/dotnet/winforms/issues/2769
        private void Restart()
        {
            string[] arguments = Environment.GetCommandLineArgs();
            Debug.Assert(arguments != null && arguments.Length > 0);
            StringBuilder sb = new StringBuilder((arguments.Length - 1) * 16);
            for (int argumentIndex = 1; argumentIndex < arguments.Length - 1; argumentIndex++)
            {
                sb.Append('"');
                sb.Append(arguments[argumentIndex]);
                sb.Append("\" ");
            }
            if (arguments.Length > 1)
            {
                sb.Append('"');
                sb.Append(arguments[^1]);
                sb.Append('"');
            }
            ProcessStartInfo currentStartInfo = new ProcessStartInfo
            {
                FileName = Path.ChangeExtension(Application.ExecutablePath, "exe")
            };
            if (sb.Length > 0)
            {
                currentStartInfo.Arguments = sb.ToString();
            }
            Application.Exit();
            Process.Start(currentStartInfo);
        }

        private void mnuLogs_Click(object sender, EventArgs e)
        {
            try
            {
                var dlgViewLog = new OpenFileDialog();
                dlgViewLog.Multiselect = true;
                dlgViewLog.Title = "Select a Log File to View...";
                dlgViewLog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Paclink\Logs");
                dlgViewLog.Filter = "Log File(.log)|*.log";
                dlgViewLog.RestoreDirectory = true;
                if (dlgViewLog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Process.Start(dlgViewLog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("[Main.mnuView_Click] " + ex.Message);
            }
        } // mnuLogs_Click

        private void mnuFile_Click(object sender, EventArgs e)
        {
            // Only enable AGWChannel Menu if AGW is used (AGWLocation <> 0)...
            mnuPacketAGWChannels.Enabled = DialogAGWEngine.AGWLocation != 0;
        }

        private void mnuBackup_Click(object sender, EventArgs e)
        {
            try
            {
                var frmSaveFile = new SaveFileDialog();
                frmSaveFile.AddExtension = true;
                frmSaveFile.InitialDirectory = Globals.SiteRootDirectory + @"Data\";
                frmSaveFile.Filter = "INI files (*.ini)|*.ini";
                frmSaveFile.DefaultExt = "ini";
                frmSaveFile.ShowDialog();
                if (!string.IsNullOrEmpty(frmSaveFile.FileName))
                {
                    //TODO: backup database somewhere else
                    //Globals.objINIFile.Backup(frmSaveFile.FileName, true);
                }
            }
            catch (Exception ex)
            {
                _log.Error("[Main.mnuBackup_Click] " + ex.Message);
            }
        } // mnuBackup_Click

        private void mnuRestoreSettings_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO: Restore database
                //var frmOpenFile = new OpenFileDialog();
                //frmOpenFile.AddExtension = true;
                //frmOpenFile.InitialDirectory = Globals.SiteRootDirectory + @"Data\";
                //frmOpenFile.Filter = "INI files (*.ini) (*.ini.bak)|*.ini;*.ini.bak";
                //frmOpenFile.FilterIndex = 1;
                //frmOpenFile.ShowDialog();
                //if (!string.IsNullOrEmpty(frmOpenFile.FileName))
                //{
                //    if (Globals.objINIFile.Restore(frmOpenFile.FileName))
                //        Close();
                //}
            }
            catch (Exception ex)
            {
                _log.Error("[Main.mnuRestoreSettings_Click] " + ex.Message);
            }
        } // mnuRestoreSettings_Click

        private void mnuPactorChannels_Click(object sender, EventArgs e)
        {
            var dlgPactorTNCChannels = new DialogPactorTNCChannels();
            dlgPactorTNCChannels.ShowDialog();
            UpdateChannelsList();
        } // mnuPactorChannels_Click

        private void mnuHelpContents_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm");
            }
            catch (Exception ex)
            {
                _log.Error("[Main.mnuHelpContents_Click] " + ex.Message);
            }
        } // mnuHelpContents_Click

        private void mnuSimpleTerminal_Click(object sender, EventArgs e)
        {
            mnuConnect.Enabled = false;
            Globals.objTerminal = new Terminal();
            Globals.objTerminal.ShowDialog();
            Globals.objTerminal = null;
        } // mnuSimpleTerminal_Click

        private void mnuNoCMS_Click(object sender, EventArgs e)
        {
            return;
        } // mnuNoCMS_Click

        private void mnuTest_Click(object sender, EventArgs e)
        {
            GC.Collect();
        } // mnuTest

    } // Main
}