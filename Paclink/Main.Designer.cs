using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    [DesignerGenerated()]
    public partial class Main : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            _barStatus = new StatusStrip();
            _lblChannelStatus = new ToolStripStatusLabel();
            _ChannelRate = new ToolStripStatusLabel();
            _barChannelProgress = new ToolStripProgressBar();
            _lblSMTPStatus = new ToolStripStatusLabel();
            _lblProgramVersion = new ToolStripStatusLabel();
            _lblUptime = new ToolStripStatusLabel();
            _lblMessageDisplay = new TextBox();
            _lblMessageDisplay.KeyPress += new KeyPressEventHandler(txtStateDisplay_KeyPress);
            _pnlSplitter = new SplitContainer();
            _ChannelDisplay = new RichTextBox();
            _SMTPDisplay = new RichTextBox();
            _mnuMain = new MenuStrip();
            _mnuFile = new ToolStripMenuItem();
            _mnuFile.Click += new EventHandler(mnuFile_Click);
            _mnuProperties = new ToolStripMenuItem();
            _mnuProperties.Click += new EventHandler(mnuProperties_Click);
            _mnuPollingInterval = new ToolStripMenuItem();
            _mnuPollingInterval.Click += new EventHandler(mnuPollingInterval_Click);
            _ToolStripSeparator3 = new ToolStripSeparator();
            _mnuAGWEngine = new ToolStripMenuItem();
            _mnuAGWEngine.Click += new EventHandler(mnuAGWEngine_Click);
            _mnuPacketAGWChannels = new ToolStripMenuItem();
            _mnuPacketAGWChannels.Click += new EventHandler(mnuPacketAGWChannels_Click);
            _mnuPacketTNCChannels = new ToolStripMenuItem();
            _mnuPacketTNCChannels.Click += new EventHandler(mnuPacketTNCChannels_Click);
            _mnuPactorChannels = new ToolStripMenuItem();
            _mnuPactorChannels.Click += new EventHandler(mnuPactorChannels_Click);
            _mnuTelnetChannels = new ToolStripMenuItem();
            _mnuTelnetChannels.Click += new EventHandler(mnuTelnetChannels_Click);
            _ToolStripSeparator4 = new ToolStripSeparator();
            _mnuTacticalAccounts = new ToolStripMenuItem();
            _mnuTacticalAccounts.Click += new EventHandler(mnuAccounts_Click);
            _mnuCallsignAccounts = new ToolStripMenuItem();
            _mnuCallsignAccounts.Click += new EventHandler(mnuCallsignAccounts_Click);
            _mnuEditAccount = new ToolStripMenuItem();
            _ToolStripSeparator1 = new ToolStripSeparator();
            _mnuBackup = new ToolStripMenuItem();
            _mnuBackup.Click += new EventHandler(mnuBackup_Click);
            _mnuRestoreSettings = new ToolStripMenuItem();
            _mnuRestoreSettings.Click += new EventHandler(mnuRestoreSettings_Click);
            _ToolStripSeparator2 = new ToolStripSeparator();
            _mnuExit = new ToolStripMenuItem();
            _mnuExit.Click += new EventHandler(mnuExit_Click);
            _mnuConnect = new ToolStripMenuItem();
            _mnuConnect.Click += new EventHandler(mnuConnect_Click);
            _mnuAutoConnect = new ToolStripMenuItem();
            _mnuAutoConnect.Click += new EventHandler(mnuAutoConnect_Click);
            _mnuConnectTo = new ToolStripMenuItem();
            _mnuAbort = new ToolStripMenuItem();
            _mnuAbort.Click += new EventHandler(mnuAbort_Click);
            _mnuLogs = new ToolStripMenuItem();
            _mnuLogs.Click += new EventHandler(mnuLogs_Click);
            _mnuHelp = new ToolStripMenuItem();
            _mnuHelpContents = new ToolStripMenuItem();
            _mnuHelpContents.Click += new EventHandler(mnuHelpContents_Click);
            _mnuDocumentation = new ToolStripMenuItem();
            _mnuDocumentation.Click += new EventHandler(mnuDocumentation_Click);
            _ToolStripSeparator6 = new ToolStripSeparator();
            _mnuSimpleTerminal = new ToolStripMenuItem();
            _mnuSimpleTerminal.Click += new EventHandler(mnuSimpleTerminal_Click);
            _ToolStripSeparator5 = new ToolStripSeparator();
            _mnuAbout = new ToolStripMenuItem();
            _mnuAbout.Click += new EventHandler(mnuAbout_Click);
            _mnuMinutesRemaining = new ToolStripMenuItem();
            _mnuNoCMS = new ToolStripMenuItem();
            _mnuNoCMS.Click += new EventHandler(mnuNoCMS_Click);
            _mnuTest = new ToolStripMenuItem();
            _mnuTest.Click += new EventHandler(mnuTest_Click);
            _tmrMain = new Timer(components);
            _tmrMain.Tick += new EventHandler(tmrMain_Tick);
            _tmrDisplay = new Timer(components);
            _tmrDisplay.Tick += new EventHandler(tmrDisplay_Tick);
            _barStatus.SuspendLayout();
            _pnlSplitter.Panel1.SuspendLayout();
            _pnlSplitter.Panel2.SuspendLayout();
            _pnlSplitter.SuspendLayout();
            _mnuMain.SuspendLayout();
            SuspendLayout();
            // 
            // barStatus
            // 
            _barStatus.AutoSize = false;
            _barStatus.Items.AddRange(new ToolStripItem[] { _lblChannelStatus, _ChannelRate, _barChannelProgress, _lblSMTPStatus, _lblProgramVersion, _lblUptime });
            _barStatus.Location = new Point(0, 285);
            _barStatus.Name = "_barStatus";
            _barStatus.ShowItemToolTips = true;
            _barStatus.Size = new Size(694, 22);
            _barStatus.TabIndex = 3;
            // 
            // lblChannelStatus
            // 
            _lblChannelStatus.AutoSize = false;
            _lblChannelStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;

            _lblChannelStatus.BorderStyle = Border3DStyle.Sunken;
            _lblChannelStatus.Name = "_lblChannelStatus";
            _lblChannelStatus.Size = new Size(249, 17);
            _lblChannelStatus.Spring = true;
            // 
            // ChannelRate
            // 
            _ChannelRate.AutoSize = false;
            _ChannelRate.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;

            _ChannelRate.BorderStyle = Border3DStyle.Sunken;
            _ChannelRate.Name = "_ChannelRate";
            _ChannelRate.Size = new Size(249, 17);
            _ChannelRate.Spring = true;
            // 
            // barChannelProgress
            // 
            _barChannelProgress.AutoSize = false;
            _barChannelProgress.Name = "_barChannelProgress";
            _barChannelProgress.Size = new Size(100, 16);
            // 
            // lblSMTPStatus
            // 
            _lblSMTPStatus.AutoToolTip = true;
            _lblSMTPStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;

            _lblSMTPStatus.BorderStyle = Border3DStyle.Sunken;
            _lblSMTPStatus.Name = "_lblSMTPStatus";
            _lblSMTPStatus.Size = new Size(26, 17);
            _lblSMTPStatus.Text = "---";
            // 
            // lblProgramVersion
            // 
            _lblProgramVersion.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;

            _lblProgramVersion.BorderStyle = Border3DStyle.Sunken;
            _lblProgramVersion.Name = "_lblProgramVersion";
            _lblProgramVersion.Size = new Size(26, 17);
            _lblProgramVersion.Text = "---";
            // 
            // lblUptime
            // 
            _lblUptime.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;

            _lblUptime.BorderStyle = Border3DStyle.Sunken;
            _lblUptime.Name = "_lblUptime";
            _lblUptime.Size = new Size(26, 17);
            _lblUptime.Text = "---";
            // 
            // lblMessageDisplay
            // 
            _lblMessageDisplay.Dock = DockStyle.Bottom;
            _lblMessageDisplay.Location = new Point(0, 265);
            _lblMessageDisplay.Name = "_lblMessageDisplay";
            _lblMessageDisplay.Size = new Size(694, 20);
            _lblMessageDisplay.TabIndex = 2;
            // 
            // pnlSplitter
            // 
            _pnlSplitter.Dock = DockStyle.Fill;
            _pnlSplitter.Location = new Point(0, 24);
            _pnlSplitter.Name = "_pnlSplitter";
            // 
            // pnlSplitter.Panel1
            // 
            _pnlSplitter.Panel1.Controls.Add(_ChannelDisplay);
            // 
            // pnlSplitter.Panel2
            // 
            _pnlSplitter.Panel2.Controls.Add(_SMTPDisplay);
            _pnlSplitter.Size = new Size(694, 241);
            _pnlSplitter.SplitterDistance = 345;
            _pnlSplitter.TabIndex = 4;
            _pnlSplitter.TabStop = false;
            // 
            // ChannelDisplay
            // 
            _ChannelDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            _ChannelDisplay.BackColor = Color.White;
            _ChannelDisplay.Location = new Point(3, 3);
            _ChannelDisplay.Name = "_ChannelDisplay";
            _ChannelDisplay.ReadOnly = true;
            _ChannelDisplay.Size = new Size(339, 232);
            _ChannelDisplay.TabIndex = 0;
            _ChannelDisplay.Text = "";
            // 
            // SMTPDisplay
            // 
            _SMTPDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            _SMTPDisplay.BackColor = Color.White;
            _SMTPDisplay.Location = new Point(3, 3);
            _SMTPDisplay.Name = "_SMTPDisplay";
            _SMTPDisplay.ReadOnly = true;
            _SMTPDisplay.Size = new Size(339, 232);
            _SMTPDisplay.TabIndex = 0;
            _SMTPDisplay.Text = "";
            // 
            // mnuMain
            // 
            _mnuMain.Enabled = false;
            _mnuMain.Items.AddRange(new ToolStripItem[] { _mnuFile, _mnuConnect, _mnuAbort, _mnuLogs, _mnuHelp, _mnuMinutesRemaining, _mnuNoCMS, _mnuTest });
            _mnuMain.Location = new Point(0, 0);
            _mnuMain.Name = "_mnuMain";
            _mnuMain.Size = new Size(694, 24);
            _mnuMain.TabIndex = 5;
            // 
            // mnuFile
            // 
            _mnuFile.DropDownItems.AddRange(new ToolStripItem[] { _mnuProperties, _mnuPollingInterval, _ToolStripSeparator3, _mnuAGWEngine, _mnuPacketAGWChannels, _mnuPacketTNCChannels, _mnuPactorChannels, _mnuTelnetChannels, _ToolStripSeparator4, _mnuTacticalAccounts, _mnuCallsignAccounts, _mnuEditAccount, _ToolStripSeparator1, _mnuBackup, _mnuRestoreSettings, _ToolStripSeparator2, _mnuExit });
            _mnuFile.Name = "_mnuFile";
            _mnuFile.Size = new Size(61, 20);
            _mnuFile.Text = "Settings";
            _mnuFile.ToolTipText = "Setup menues";
            // 
            // mnuProperties
            // 
            _mnuProperties.Name = "_mnuProperties";
            _mnuProperties.Size = new Size(200, 22);
            _mnuProperties.Text = "Site Properties...";
            // 
            // mnuPollingInterval
            // 
            _mnuPollingInterval.Name = "_mnuPollingInterval";
            _mnuPollingInterval.Size = new Size(200, 22);
            _mnuPollingInterval.Text = "P&olling Interval...";
            // 
            // ToolStripSeparator3
            // 
            _ToolStripSeparator3.Name = "_ToolStripSeparator3";
            _ToolStripSeparator3.Size = new Size(197, 6);
            // 
            // mnuAGWEngine
            // 
            _mnuAGWEngine.Name = "_mnuAGWEngine";
            _mnuAGWEngine.Size = new Size(200, 22);
            _mnuAGWEngine.Text = "AGW &Engine...";
            // 
            // mnuPacketAGWChannels
            // 
            _mnuPacketAGWChannels.Name = "_mnuPacketAGWChannels";
            _mnuPacketAGWChannels.Size = new Size(200, 22);
            _mnuPacketAGWChannels.Text = "&Packet AGW Channels...";
            // 
            // mnuPacketTNCChannels
            // 
            _mnuPacketTNCChannels.Name = "_mnuPacketTNCChannels";
            _mnuPacketTNCChannels.Size = new Size(200, 22);
            _mnuPacketTNCChannels.Text = "Packet &TNC Channels...";
            // 
            // mnuPactorChannels
            // 
            _mnuPactorChannels.Name = "_mnuPactorChannels";
            _mnuPactorChannels.Size = new Size(200, 22);
            _mnuPactorChannels.Text = "Pactor TNC Channels...";
            // 
            // mnuTelnetChannels
            // 
            _mnuTelnetChannels.Name = "_mnuTelnetChannels";
            _mnuTelnetChannels.Size = new Size(200, 22);
            _mnuTelnetChannels.Text = "&Telnet Channels...";
            // 
            // ToolStripSeparator4
            // 
            _ToolStripSeparator4.Name = "_ToolStripSeparator4";
            _ToolStripSeparator4.Size = new Size(197, 6);
            // 
            // mnuTacticalAccounts
            // 
            _mnuTacticalAccounts.Name = "_mnuTacticalAccounts";
            _mnuTacticalAccounts.Size = new Size(200, 22);
            _mnuTacticalAccounts.Text = "Tactical Accounts...";
            // 
            // mnuCallsignAccounts
            // 
            _mnuCallsignAccounts.Name = "_mnuCallsignAccounts";
            _mnuCallsignAccounts.Size = new Size(200, 22);
            _mnuCallsignAccounts.Text = "Callsign Accounts...";
            // 
            // mnuEditAccount
            // 
            _mnuEditAccount.Name = "_mnuEditAccount";
            _mnuEditAccount.Size = new Size(200, 22);
            _mnuEditAccount.Text = "Edit Account Profile...";
            // 
            // ToolStripSeparator1
            // 
            _ToolStripSeparator1.Name = "_ToolStripSeparator1";
            _ToolStripSeparator1.Size = new Size(197, 6);
            // 
            // mnuBackup
            // 
            _mnuBackup.Name = "_mnuBackup";
            _mnuBackup.Size = new Size(200, 22);
            _mnuBackup.Text = "Backup INI file...";
            // 
            // mnuRestoreSettings
            // 
            _mnuRestoreSettings.Name = "_mnuRestoreSettings";
            _mnuRestoreSettings.Size = new Size(200, 22);
            _mnuRestoreSettings.Text = "Restore INI file...";
            // 
            // ToolStripSeparator2
            // 
            _ToolStripSeparator2.Name = "_ToolStripSeparator2";
            _ToolStripSeparator2.Size = new Size(197, 6);
            // 
            // mnuExit
            // 
            _mnuExit.Name = "_mnuExit";
            _mnuExit.Size = new Size(200, 22);
            _mnuExit.Text = "E&xit";
            // 
            // mnuConnect
            // 
            _mnuConnect.DropDownItems.AddRange(new ToolStripItem[] { _mnuAutoConnect, _mnuConnectTo });
            _mnuConnect.Name = "_mnuConnect";
            _mnuConnect.Size = new Size(64, 20);
            _mnuConnect.Text = "Connect";
            _mnuConnect.ToolTipText = "Manual or automatic connection";
            // 
            // mnuAutoConnect
            // 
            _mnuAutoConnect.Name = "_mnuAutoConnect";
            _mnuAutoConnect.Size = new Size(143, 22);
            _mnuAutoConnect.Text = "Autoconnect";
            // 
            // mnuConnectTo
            // 
            _mnuConnectTo.Name = "_mnuConnectTo";
            _mnuConnectTo.Size = new Size(143, 22);
            _mnuConnectTo.Text = "Connect to...";
            // 
            // mnuAbort
            // 
            _mnuAbort.Name = "_mnuAbort";
            _mnuAbort.Size = new Size(49, 20);
            _mnuAbort.Text = "Abort";
            _mnuAbort.ToolTipText = "Interrupt and connection in progress";
            // 
            // mnuLogs
            // 
            _mnuLogs.Name = "_mnuLogs";
            _mnuLogs.Size = new Size(47, 20);
            _mnuLogs.Text = "Logs ";
            _mnuLogs.ToolTipText = "Manage and View Logs";
            // 
            // mnuHelp
            // 
            _mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { _mnuHelpContents, _mnuDocumentation, _ToolStripSeparator6, _mnuSimpleTerminal, _ToolStripSeparator5, _mnuAbout });
            _mnuHelp.Name = "_mnuHelp";
            _mnuHelp.Size = new Size(44, 20);
            _mnuHelp.Text = "Help";
            // 
            // mnuHelpContents
            // 
            _mnuHelpContents.Name = "_mnuHelpContents";
            _mnuHelpContents.Size = new Size(168, 22);
            _mnuHelpContents.Text = "&Contents...";
            // 
            // mnuDocumentation
            // 
            _mnuDocumentation.Name = "_mnuDocumentation";
            _mnuDocumentation.Size = new Size(168, 22);
            _mnuDocumentation.Text = "Documentation...";
            _mnuDocumentation.ToolTipText = "Find and read documentation ";
            // 
            // ToolStripSeparator6
            // 
            _ToolStripSeparator6.Name = "_ToolStripSeparator6";
            _ToolStripSeparator6.Size = new Size(165, 6);
            // 
            // mnuSimpleTerminal
            // 
            _mnuSimpleTerminal.Name = "_mnuSimpleTerminal";
            _mnuSimpleTerminal.Size = new Size(168, 22);
            _mnuSimpleTerminal.Text = "Simple Terminal...";
            // 
            // ToolStripSeparator5
            // 
            _ToolStripSeparator5.Name = "_ToolStripSeparator5";
            _ToolStripSeparator5.Size = new Size(165, 6);
            // 
            // mnuAbout
            // 
            _mnuAbout.Name = "_mnuAbout";
            _mnuAbout.Size = new Size(168, 22);
            _mnuAbout.Text = "&About...";
            // 
            // mnuMinutesRemaining
            // 
            _mnuMinutesRemaining.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
            _mnuMinutesRemaining.ForeColor = SystemColors.ControlText;
            _mnuMinutesRemaining.Name = "_mnuMinutesRemaining";
            _mnuMinutesRemaining.Size = new Size(121, 20);
            _mnuMinutesRemaining.Text = "Next Poll in 0 Minutes";
            // 
            // mnuNoCMS
            // 
            _mnuNoCMS.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            _mnuNoCMS.ForeColor = Color.Red;
            _mnuNoCMS.Name = "_mnuNoCMS";
            _mnuNoCMS.Size = new Size(126, 20);
            _mnuNoCMS.Text = "No CMS Connection";
            _mnuNoCMS.Visible = false;
            // 
            // mnuTest
            // 
            _mnuTest.Name = "_mnuTest";
            _mnuTest.Size = new Size(40, 20);
            _mnuTest.Text = "Test";
            _mnuTest.Visible = false;
            // 
            // tmrMain
            // 
            _tmrMain.Interval = 1000;
            // 
            // tmrDisplay
            // 
            _tmrDisplay.Enabled = true;
            _tmrDisplay.Interval = 1000;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(694, 307);
            Controls.Add(_pnlSplitter);
            Controls.Add(_lblMessageDisplay);
            Controls.Add(_barStatus);
            Controls.Add(_mnuMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Main";
            StartPosition = FormStartPosition.Manual;
            Text = "Paclink (... Initializing)";
            _barStatus.ResumeLayout(false);
            _barStatus.PerformLayout();
            _pnlSplitter.Panel1.ResumeLayout(false);
            _pnlSplitter.Panel2.ResumeLayout(false);
            _pnlSplitter.ResumeLayout(false);
            _mnuMain.ResumeLayout(false);
            _mnuMain.PerformLayout();
            FormClosed += new FormClosedEventHandler(MainClosed);
            FormClosing += new FormClosingEventHandler(MainClosing);
            Load += new EventHandler(MainLoad);
            Resize += new EventHandler(MainResize);
            ResumeLayout(false);
            PerformLayout();
        }

        private StatusStrip _barStatus;

        internal StatusStrip barStatus
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _barStatus;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_barStatus != null)
                {
                }

                _barStatus = value;
                if (_barStatus != null)
                {
                }
            }
        }

        private TextBox _lblMessageDisplay;

        internal TextBox lblMessageDisplay
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblMessageDisplay;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblMessageDisplay != null)
                {
                    _lblMessageDisplay.KeyPress -= txtStateDisplay_KeyPress;
                }

                _lblMessageDisplay = value;
                if (_lblMessageDisplay != null)
                {
                    _lblMessageDisplay.KeyPress += txtStateDisplay_KeyPress;
                }
            }
        }

        private ToolStripStatusLabel _lblChannelStatus;

        internal ToolStripStatusLabel lblChannelStatus
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblChannelStatus;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblChannelStatus != null)
                {
                }

                _lblChannelStatus = value;
                if (_lblChannelStatus != null)
                {
                }
            }
        }

        private ToolStripStatusLabel _ChannelRate;

        internal ToolStripStatusLabel ChannelRate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ChannelRate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ChannelRate != null)
                {
                }

                _ChannelRate = value;
                if (_ChannelRate != null)
                {
                }
            }
        }

        private ToolStripStatusLabel _lblSMTPStatus;

        internal ToolStripStatusLabel lblSMTPStatus
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblSMTPStatus;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblSMTPStatus != null)
                {
                }

                _lblSMTPStatus = value;
                if (_lblSMTPStatus != null)
                {
                }
            }
        }

        private ToolStripProgressBar _barChannelProgress;

        internal ToolStripProgressBar barChannelProgress
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _barChannelProgress;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_barChannelProgress != null)
                {
                }

                _barChannelProgress = value;
                if (_barChannelProgress != null)
                {
                }
            }
        }

        private SplitContainer _pnlSplitter;

        internal SplitContainer pnlSplitter
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _pnlSplitter;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_pnlSplitter != null)
                {
                }

                _pnlSplitter = value;
                if (_pnlSplitter != null)
                {
                }
            }
        }

        private RichTextBox _ChannelDisplay;

        internal RichTextBox ChannelDisplay
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ChannelDisplay;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ChannelDisplay != null)
                {
                }

                _ChannelDisplay = value;
                if (_ChannelDisplay != null)
                {
                }
            }
        }

        private RichTextBox _SMTPDisplay;

        internal RichTextBox SMTPDisplay
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _SMTPDisplay;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_SMTPDisplay != null)
                {
                }

                _SMTPDisplay = value;
                if (_SMTPDisplay != null)
                {
                }
            }
        }

        private MenuStrip _mnuMain;

        internal MenuStrip mnuMain
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuMain;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuMain != null)
                {
                }

                _mnuMain = value;
                if (_mnuMain != null)
                {
                }
            }
        }

        private ToolStripMenuItem _mnuFile;

        internal ToolStripMenuItem mnuFile
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuFile;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuFile != null)
                {
                    _mnuFile.Click -= mnuFile_Click;
                }

                _mnuFile = value;
                if (_mnuFile != null)
                {
                    _mnuFile.Click += mnuFile_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuProperties;

        internal ToolStripMenuItem mnuProperties
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuProperties;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuProperties != null)
                {
                    _mnuProperties.Click -= mnuProperties_Click;
                }

                _mnuProperties = value;
                if (_mnuProperties != null)
                {
                    _mnuProperties.Click += mnuProperties_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuAGWEngine;

        internal ToolStripMenuItem mnuAGWEngine
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuAGWEngine;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuAGWEngine != null)
                {
                    _mnuAGWEngine.Click -= mnuAGWEngine_Click;
                }

                _mnuAGWEngine = value;
                if (_mnuAGWEngine != null)
                {
                    _mnuAGWEngine.Click += mnuAGWEngine_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuPacketAGWChannels;

        internal ToolStripMenuItem mnuPacketAGWChannels
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuPacketAGWChannels;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuPacketAGWChannels != null)
                {
                    _mnuPacketAGWChannels.Click -= mnuPacketAGWChannels_Click;
                }

                _mnuPacketAGWChannels = value;
                if (_mnuPacketAGWChannels != null)
                {
                    _mnuPacketAGWChannels.Click += mnuPacketAGWChannels_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuPacketTNCChannels;

        internal ToolStripMenuItem mnuPacketTNCChannels
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuPacketTNCChannels;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuPacketTNCChannels != null)
                {
                    _mnuPacketTNCChannels.Click -= mnuPacketTNCChannels_Click;
                }

                _mnuPacketTNCChannels = value;
                if (_mnuPacketTNCChannels != null)
                {
                    _mnuPacketTNCChannels.Click += mnuPacketTNCChannels_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuPactorChannels;

        internal ToolStripMenuItem mnuPactorChannels
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuPactorChannels;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuPactorChannels != null)
                {
                    _mnuPactorChannels.Click -= mnuPactorChannels_Click;
                }

                _mnuPactorChannels = value;
                if (_mnuPactorChannels != null)
                {
                    _mnuPactorChannels.Click += mnuPactorChannels_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuTelnetChannels;

        internal ToolStripMenuItem mnuTelnetChannels
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuTelnetChannels;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuTelnetChannels != null)
                {
                    _mnuTelnetChannels.Click -= mnuTelnetChannels_Click;
                }

                _mnuTelnetChannels = value;
                if (_mnuTelnetChannels != null)
                {
                    _mnuTelnetChannels.Click += mnuTelnetChannels_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuPollingInterval;

        internal ToolStripMenuItem mnuPollingInterval
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuPollingInterval;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuPollingInterval != null)
                {
                    _mnuPollingInterval.Click -= mnuPollingInterval_Click;
                }

                _mnuPollingInterval = value;
                if (_mnuPollingInterval != null)
                {
                    _mnuPollingInterval.Click += mnuPollingInterval_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuTacticalAccounts;

        internal ToolStripMenuItem mnuTacticalAccounts
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuTacticalAccounts;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuTacticalAccounts != null)
                {
                    _mnuTacticalAccounts.Click -= mnuAccounts_Click;
                }

                _mnuTacticalAccounts = value;
                if (_mnuTacticalAccounts != null)
                {
                    _mnuTacticalAccounts.Click += mnuAccounts_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuCallsignAccounts;

        internal ToolStripMenuItem mnuCallsignAccounts
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuCallsignAccounts;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuCallsignAccounts != null)
                {
                    _mnuCallsignAccounts.Click -= mnuCallsignAccounts_Click;
                }

                _mnuCallsignAccounts = value;
                if (_mnuCallsignAccounts != null)
                {
                    _mnuCallsignAccounts.Click += mnuCallsignAccounts_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuEditAccount;

        internal ToolStripMenuItem mnuEditAccount
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuEditAccount;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuEditAccount != null)
                {
                }

                _mnuEditAccount = value;
                if (_mnuEditAccount != null)
                {
                }
            }
        }

        private ToolStripSeparator _ToolStripSeparator1;

        internal ToolStripSeparator ToolStripSeparator1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolStripSeparator1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolStripSeparator1 != null)
                {
                }

                _ToolStripSeparator1 = value;
                if (_ToolStripSeparator1 != null)
                {
                }
            }
        }

        private ToolStripMenuItem _mnuBackup;

        internal ToolStripMenuItem mnuBackup
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuBackup;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuBackup != null)
                {
                    _mnuBackup.Click -= mnuBackup_Click;
                }

                _mnuBackup = value;
                if (_mnuBackup != null)
                {
                    _mnuBackup.Click += mnuBackup_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuRestoreSettings;

        internal ToolStripMenuItem mnuRestoreSettings
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuRestoreSettings;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuRestoreSettings != null)
                {
                    _mnuRestoreSettings.Click -= mnuRestoreSettings_Click;
                }

                _mnuRestoreSettings = value;
                if (_mnuRestoreSettings != null)
                {
                    _mnuRestoreSettings.Click += mnuRestoreSettings_Click;
                }
            }
        }

        private ToolStripSeparator _ToolStripSeparator2;

        internal ToolStripSeparator ToolStripSeparator2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolStripSeparator2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolStripSeparator2 != null)
                {
                }

                _ToolStripSeparator2 = value;
                if (_ToolStripSeparator2 != null)
                {
                }
            }
        }

        private ToolStripMenuItem _mnuExit;

        internal ToolStripMenuItem mnuExit
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuExit;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuExit != null)
                {
                    _mnuExit.Click -= mnuExit_Click;
                }

                _mnuExit = value;
                if (_mnuExit != null)
                {
                    _mnuExit.Click += mnuExit_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuConnect;

        internal ToolStripMenuItem mnuConnect
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuConnect;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuConnect != null)
                {
                    _mnuConnect.Click -= mnuConnect_Click;
                }

                _mnuConnect = value;
                if (_mnuConnect != null)
                {
                    _mnuConnect.Click += mnuConnect_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuAutoConnect;

        internal ToolStripMenuItem mnuAutoConnect
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuAutoConnect;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuAutoConnect != null)
                {
                    _mnuAutoConnect.Click -= mnuAutoConnect_Click;
                }

                _mnuAutoConnect = value;
                if (_mnuAutoConnect != null)
                {
                    _mnuAutoConnect.Click += mnuAutoConnect_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuConnectTo;

        internal ToolStripMenuItem mnuConnectTo
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuConnectTo;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuConnectTo != null)
                {
                }

                _mnuConnectTo = value;
                if (_mnuConnectTo != null)
                {
                }
            }
        }

        private ToolStripMenuItem _mnuAbort;

        internal ToolStripMenuItem mnuAbort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuAbort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuAbort != null)
                {
                    _mnuAbort.Click -= mnuAbort_Click;
                }

                _mnuAbort = value;
                if (_mnuAbort != null)
                {
                    _mnuAbort.Click += mnuAbort_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuLogs;

        internal ToolStripMenuItem mnuLogs
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuLogs;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuLogs != null)
                {
                    _mnuLogs.Click -= mnuLogs_Click;
                }

                _mnuLogs = value;
                if (_mnuLogs != null)
                {
                    _mnuLogs.Click += mnuLogs_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuHelp;

        internal ToolStripMenuItem mnuHelp
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuHelp;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuHelp != null)
                {
                }

                _mnuHelp = value;
                if (_mnuHelp != null)
                {
                }
            }
        }

        private ToolStripMenuItem _mnuHelpContents;

        internal ToolStripMenuItem mnuHelpContents
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuHelpContents;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuHelpContents != null)
                {
                    _mnuHelpContents.Click -= mnuHelpContents_Click;
                }

                _mnuHelpContents = value;
                if (_mnuHelpContents != null)
                {
                    _mnuHelpContents.Click += mnuHelpContents_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuDocumentation;

        internal ToolStripMenuItem mnuDocumentation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuDocumentation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuDocumentation != null)
                {
                    _mnuDocumentation.Click -= mnuDocumentation_Click;
                }

                _mnuDocumentation = value;
                if (_mnuDocumentation != null)
                {
                    _mnuDocumentation.Click += mnuDocumentation_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuSimpleTerminal;

        internal ToolStripMenuItem mnuSimpleTerminal
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuSimpleTerminal;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuSimpleTerminal != null)
                {
                    _mnuSimpleTerminal.Click -= mnuSimpleTerminal_Click;
                }

                _mnuSimpleTerminal = value;
                if (_mnuSimpleTerminal != null)
                {
                    _mnuSimpleTerminal.Click += mnuSimpleTerminal_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuAbout;

        internal ToolStripMenuItem mnuAbout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuAbout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuAbout != null)
                {
                    _mnuAbout.Click -= mnuAbout_Click;
                }

                _mnuAbout = value;
                if (_mnuAbout != null)
                {
                    _mnuAbout.Click += mnuAbout_Click;
                }
            }
        }

        private ToolStripSeparator _ToolStripSeparator3;

        internal ToolStripSeparator ToolStripSeparator3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolStripSeparator3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolStripSeparator3 != null)
                {
                }

                _ToolStripSeparator3 = value;
                if (_ToolStripSeparator3 != null)
                {
                }
            }
        }

        private ToolStripSeparator _ToolStripSeparator4;

        internal ToolStripSeparator ToolStripSeparator4
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolStripSeparator4;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolStripSeparator4 != null)
                {
                }

                _ToolStripSeparator4 = value;
                if (_ToolStripSeparator4 != null)
                {
                }
            }
        }

        private Timer _tmrMain;

        internal Timer tmrMain
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrMain;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrMain != null)
                {
                    _tmrMain.Tick -= tmrMain_Tick;
                }

                _tmrMain = value;
                if (_tmrMain != null)
                {
                    _tmrMain.Tick += tmrMain_Tick;
                }
            }
        }

        private ToolStripMenuItem _mnuNoCMS;

        internal ToolStripMenuItem mnuNoCMS
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuNoCMS;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuNoCMS != null)
                {
                    _mnuNoCMS.Click -= mnuNoCMS_Click;
                }

                _mnuNoCMS = value;
                if (_mnuNoCMS != null)
                {
                    _mnuNoCMS.Click += mnuNoCMS_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuMinutesRemaining;

        internal ToolStripMenuItem mnuMinutesRemaining
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuMinutesRemaining;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuMinutesRemaining != null)
                {
                }

                _mnuMinutesRemaining = value;
                if (_mnuMinutesRemaining != null)
                {
                }
            }
        }

        private ToolStripSeparator _ToolStripSeparator6;

        internal ToolStripSeparator ToolStripSeparator6
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolStripSeparator6;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolStripSeparator6 != null)
                {
                }

                _ToolStripSeparator6 = value;
                if (_ToolStripSeparator6 != null)
                {
                }
            }
        }

        private ToolStripSeparator _ToolStripSeparator5;

        internal ToolStripSeparator ToolStripSeparator5
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolStripSeparator5;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolStripSeparator5 != null)
                {
                }

                _ToolStripSeparator5 = value;
                if (_ToolStripSeparator5 != null)
                {
                }
            }
        }

        private ToolStripMenuItem _mnuTest;

        internal ToolStripMenuItem mnuTest
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuTest;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuTest != null)
                {
                    _mnuTest.Click -= mnuTest_Click;
                }

                _mnuTest = value;
                if (_mnuTest != null)
                {
                    _mnuTest.Click += mnuTest_Click;
                }
            }
        }

        private Timer _tmrDisplay;

        internal Timer tmrDisplay
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrDisplay;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrDisplay != null)
                {
                    _tmrDisplay.Tick -= tmrDisplay_Tick;
                }

                _tmrDisplay = value;
                if (_tmrDisplay != null)
                {
                    _tmrDisplay.Tick += tmrDisplay_Tick;
                }
            }
        }

        private ToolStripStatusLabel _lblProgramVersion;

        internal ToolStripStatusLabel lblProgramVersion
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblProgramVersion;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblProgramVersion != null)
                {
                }

                _lblProgramVersion = value;
                if (_lblProgramVersion != null)
                {
                }
            }
        }

        private ToolStripStatusLabel _lblUptime;

        internal ToolStripStatusLabel lblUptime
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblUptime;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblUptime != null)
                {
                }

                _lblUptime = value;
                if (_lblUptime != null)
                {
                }
            }
        }
    }
}