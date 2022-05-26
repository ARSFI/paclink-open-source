using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;
using Paclink.UI.Common;

namespace Paclink.UI.Windows
{
    public partial class Main : IMainWindow
    {
        // Use this constant to clear an RTF text display...
        private const string CLEAR = "\u0001";
        private static readonly string CRLF = "\r\n";
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public IMainFormBacking BackingObject { get; private set; }

        public Main(IMainFormBacking backingObject)
        {
            BackingObject = backingObject;
            BackingObject.MainWindow = this;

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
            if ((strText ?? "") == CLEAR)
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
                ChannelDisplay.AppendText(strText + CRLF);
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

        // Show "waiting" UI (e.g. cursors on Windows).
        public void EnableWaitDisplay()
        {
            Cursor = Cursors.WaitCursor;
            ChannelDisplay.Cursor = Cursors.WaitCursor;
            SMTPDisplay.Cursor = Cursors.WaitCursor;
        }

        public void DisableWaitDisplay()
        {
            Cursor = Cursors.Default;
            ChannelDisplay.Cursor = Cursors.Default;
            SMTPDisplay.Cursor = Cursors.Default;
        }

        private void Initialize()
        {
            ChannelDisplay.ForeColor = Color.Green;
            if (BackingObject.UseRMSRelay)
            {
                ChannelDisplay.Text = "*** Initializing. Paclink is set to connect to RMS Relay.  RMS Relay must be running." + CRLF;
            }
            else
            {
                ChannelDisplay.Text = "*** Initializing..." + CRLF;
            }

            Refresh();

            // Perform common platform independent initialization tasks.
            BackingObject.FormLoaded();
        }

        private void MainClosed(object sender, FormClosedEventArgs e)
        {
            BackingObject.FormClosed();
        }

        private void MainClosing(object s, FormClosingEventArgs e)
        {
            BackingObject.FormClosing();
        }

        private void MainLoad(object s, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            BackingObject.FormLoading();

            lblProgramVersion.Text = "Version: " + BackingObject.ProductVersion;
            lblMessageDisplay.Enabled = true;
            lblMessageDisplay.BackColor = ChannelDisplay.BackColor;
            barChannelProgress.Enabled = false;
            mnuMinutesRemaining.Visible = false;
            Show();
            tmrMain.Start();
        } // MainLoad

        private void SMTPDisplayWrite(string strText)
        {
            if ((strText ?? "") == CLEAR)
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
                SMTPDisplay.AppendText(strText + CRLF);
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
                BackingObject.StartChannelAutoconnect();
            }
            else if (BackingObject.PrimaryThreadExists)
            {
                BackingObject.StartChannelOnMainThread(strChannelSelection);
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
            if (BackingObject.UpdateComplete & !Text.StartsWith("*"))
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
                BackingObject.PostVersionRecord();
            }

            if (BackingObject.IsAutoPolling)
            {
                mnuMinutesRemaining.Visible = true;
                int intInterval = BackingObject.AutoPollingMinutesRemaining;
                if (intInterval > BackingObject.AutoPollingInterval)
                    intInterval = BackingObject.AutoPollingInterval;
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
                if (BackingObject.ChannelActive == false && !BackingObject.HasSimpleTerminal)
                {
                    mnuConnect.Enabled = true;
                }
                else
                {
                    mnuConnect.Enabled = false;
                }

                while (BackingObject.HasSMTPStatus)
                    lblSMTPStatus.Text = BackingObject.GetSMTPStatus();
                while (BackingObject.HasStateDisplay)
                    lblMessageDisplay.Text = BackingObject.GetStateDisplay();
                while (BackingObject.HasStatusDisplay)
                    lblChannelStatus.Text = BackingObject.GetStatusDisplay();
                while (BackingObject.HasProgressDisplay)
                    barChannelProgress.Value = Convert.ToInt32(BackingObject.GetProgressDisplay());
                while (BackingObject.HasRateDisplay)
                    ChannelRate.Text = BackingObject.GetRateDisplay();
                while (BackingObject.HasSMTPDisplay)
                {
                    strText = BackingObject.GetSMTPDisplay();
                    SMTPDisplayWrite(strText);
                }

                while (BackingObject.HasChannelDisplay)
                {
                    strText = BackingObject.GetChannelDisplay();
                    ChannelDisplayWrite(strText);
                }

                lblUptime.Text = BackingObject.Uptime;
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

        public void UpdateChannelList()
        {
            mnuConnectTo.DropDownItems.Clear();
            BackingObject.UpdateChannelList((name) =>
            {
                var mnuItem = new ToolStripMenuItem(name);
                mnuItem.Click += ChannelSelection;
                mnuConnectTo.DropDownItems.Add(mnuItem);
            });

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
                _log.Error("Main.UpdateChannelsList A] " + e.Message);
            }
        } // UpdateChannelsList

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void mnuProperties_Click(object sender, EventArgs e)
        {
            // Temporarily close SMTP/POP3 ports...
            BackingObject.ClosePOP3AndSMTPPorts();

            BackingObject.ShowSiteProperties();

            // Reopen SMTP/POP3 ports.
            BackingObject.OpenPOP3AndSMTPPorts();
        } // mnuProperties_Click

        private void mnuAccounts_Click(object sender, EventArgs e)
        {
            BackingObject.ShowTacticalAccounts();
        } // mnuAccounts_Click

        private void mnuCallsignAccounts_Click(object sender, EventArgs e)
        {
            BackingObject.ShowCallsignAccounts();
        } // mnuCallsignAccounts_Click

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        } // mnuExit_Click

        private void mnuAGWEngine_Click(object sender, EventArgs e)
        {
            BackingObject.ShowAGWEngine();
        } // mnuAGWEngine_Click

        private void mnuPacketAGWChannels_Click(object sender, EventArgs e)
        {
            BackingObject.ShowPacketAGWChannels();
            UpdateChannelList();
        } // mnuPacketChannels_Click

        private void mnuPacketTNCChannels_Click(object sender, EventArgs e)
        {
            BackingObject.ShowPacketTNCChannels();
            UpdateChannelList();
        } // mnuPacketTNCChannels_Click

        private void mnuTelnetChannels_Click(object sender, EventArgs e)
        {
            BackingObject.ShowTelnetChannels();
            UpdateChannelList();
        } // mnuTelnetChannels_Click

        private void mnuPollingInterval_Click(object sender, EventArgs e)
        {
            BackingObject.ShowPollingInterval();
        } // mnuPollingInterval_Click

        private void mnuConnect_Click(object sender, EventArgs e)
        {
            // Updates the available channels lists on manual connect...
            UpdateChannelList();
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
            BackingObject.AbortSelectedModem();

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
                        Process.Start(new ProcessStartInfo(dlgViewLog.FileName) { UseShellExecute = true });
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
            mnuPacketAGWChannels.Enabled = BackingObject.IsAGWUsed;
        }

        private void mnuBackup_Click(object sender, EventArgs e)
        {
            try
            {
                var frmSaveFile = new SaveFileDialog();
                frmSaveFile.AddExtension = true;
                frmSaveFile.InitialDirectory = BackingObject.SiteRootDirectory + @"Data\";
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
            BackingObject.ShowPactorChannels();
            UpdateChannelList();
        } // mnuPactorChannels_Click

        private void mnuHelpContents_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm");
            }
            catch (Exception ex)
            {
                _log.Error("[Main.mnuHelpContents_Click] " + ex.Message);
            }
        } // mnuHelpContents_Click

        private void mnuSimpleTerminal_Click(object sender, EventArgs e)
        {
            mnuConnect.Enabled = false;
            BackingObject.ShowSimpleTerminal();
        } // mnuSimpleTerminal_Click

        private void mnuNoCMS_Click(object sender, EventArgs e)
        {
            return;
        } // mnuNoCMS_Click

        private void mnuTest_Click(object sender, EventArgs e)
        {
            GC.Collect();
        } // mnuTest

        public void UpdateSiteCallsign(string callsign)
        {
            Text = "Paclink - " + callsign;
        }

        public void RefreshWindow()
        {
            Refresh();
        }

        public void CloseWindow()
        {
            Close();
        }

        public void EnableMainWindowInterface()
        {
            mnuMain.Enabled = true;
        }

    } // Main
}