using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this._barStatus = new System.Windows.Forms.StatusStrip();
            this._lblChannelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this._ChannelRate = new System.Windows.Forms.ToolStripStatusLabel();
            this._barChannelProgress = new System.Windows.Forms.ToolStripProgressBar();
            this._lblSMTPStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this._lblProgramVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this._lblUptime = new System.Windows.Forms.ToolStripStatusLabel();
            this._lblMessageDisplay = new System.Windows.Forms.TextBox();
            this._pnlSplitter = new System.Windows.Forms.SplitContainer();
            this._ChannelDisplay = new System.Windows.Forms.RichTextBox();
            this._SMTPDisplay = new System.Windows.Forms.RichTextBox();
            this._mnuMain = new System.Windows.Forms.MenuStrip();
            this._mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuPollingInterval = new System.Windows.Forms.ToolStripMenuItem();
            this._ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._mnuAGWEngine = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuPacketAGWChannels = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuPacketTNCChannels = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuPactorChannels = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuTelnetChannels = new System.Windows.Forms.ToolStripMenuItem();
            this._ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._mnuTacticalAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuCallsignAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuEditAccount = new System.Windows.Forms.ToolStripMenuItem();
            this._ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._mnuBackup = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuRestoreSettings = new System.Windows.Forms.ToolStripMenuItem();
            this._ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuAutoConnect = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuConnectTo = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuAbort = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuLogs = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuHelpContents = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuSimpleTerminal = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuMinutesRemaining = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuTest = new System.Windows.Forms.ToolStripMenuItem();
            this._tmrMain = new System.Windows.Forms.Timer(this.components);
            this._tmrDisplay = new System.Windows.Forms.Timer(this.components);
            this._barStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pnlSplitter)).BeginInit();
            this._pnlSplitter.Panel1.SuspendLayout();
            this._pnlSplitter.Panel2.SuspendLayout();
            this._pnlSplitter.SuspendLayout();
            this._mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // _barStatus
            // 
            this._barStatus.AutoSize = false;
            this._barStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lblChannelStatus,
            this._ChannelRate,
            this._barChannelProgress,
            this._lblSMTPStatus,
            this._lblProgramVersion,
            this._lblUptime});
            this._barStatus.Location = new System.Drawing.Point(0, 329);
            this._barStatus.Name = "_barStatus";
            this._barStatus.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this._barStatus.ShowItemToolTips = true;
            this._barStatus.Size = new System.Drawing.Size(810, 25);
            this._barStatus.TabIndex = 3;
            // 
            // _lblChannelStatus
            // 
            this._lblChannelStatus.AutoSize = false;
            this._lblChannelStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this._lblChannelStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this._lblChannelStatus.Name = "_lblChannelStatus";
            this._lblChannelStatus.Size = new System.Drawing.Size(298, 20);
            this._lblChannelStatus.Spring = true;
            // 
            // _ChannelRate
            // 
            this._ChannelRate.AutoSize = false;
            this._ChannelRate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this._ChannelRate.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this._ChannelRate.Name = "_ChannelRate";
            this._ChannelRate.Size = new System.Drawing.Size(298, 20);
            this._ChannelRate.Spring = true;
            // 
            // _barChannelProgress
            // 
            this._barChannelProgress.AutoSize = false;
            this._barChannelProgress.Name = "_barChannelProgress";
            this._barChannelProgress.Size = new System.Drawing.Size(117, 19);
            // 
            // _lblSMTPStatus
            // 
            this._lblSMTPStatus.AutoToolTip = true;
            this._lblSMTPStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this._lblSMTPStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this._lblSMTPStatus.Name = "_lblSMTPStatus";
            this._lblSMTPStatus.Size = new System.Drawing.Size(26, 20);
            this._lblSMTPStatus.Text = "---";
            // 
            // _lblProgramVersion
            // 
            this._lblProgramVersion.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this._lblProgramVersion.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this._lblProgramVersion.Name = "_lblProgramVersion";
            this._lblProgramVersion.Size = new System.Drawing.Size(26, 20);
            this._lblProgramVersion.Text = "---";
            // 
            // _lblUptime
            // 
            this._lblUptime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this._lblUptime.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this._lblUptime.Name = "_lblUptime";
            this._lblUptime.Size = new System.Drawing.Size(26, 20);
            this._lblUptime.Text = "---";
            // 
            // _lblMessageDisplay
            // 
            this._lblMessageDisplay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._lblMessageDisplay.Location = new System.Drawing.Point(0, 306);
            this._lblMessageDisplay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._lblMessageDisplay.Name = "_lblMessageDisplay";
            this._lblMessageDisplay.Size = new System.Drawing.Size(810, 23);
            this._lblMessageDisplay.TabIndex = 2;
            this._lblMessageDisplay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStateDisplay_KeyPress);
            // 
            // _pnlSplitter
            // 
            this._pnlSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSplitter.Location = new System.Drawing.Point(0, 24);
            this._pnlSplitter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._pnlSplitter.Name = "_pnlSplitter";
            // 
            // _pnlSplitter.Panel1
            // 
            this._pnlSplitter.Panel1.Controls.Add(this._ChannelDisplay);
            // 
            // _pnlSplitter.Panel2
            // 
            this._pnlSplitter.Panel2.Controls.Add(this._SMTPDisplay);
            this._pnlSplitter.Size = new System.Drawing.Size(810, 282);
            this._pnlSplitter.SplitterDistance = 402;
            this._pnlSplitter.SplitterWidth = 5;
            this._pnlSplitter.TabIndex = 4;
            this._pnlSplitter.TabStop = false;
            // 
            // _ChannelDisplay
            // 
            this._ChannelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ChannelDisplay.BackColor = System.Drawing.Color.White;
            this._ChannelDisplay.Location = new System.Drawing.Point(4, 3);
            this._ChannelDisplay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._ChannelDisplay.Name = "_ChannelDisplay";
            this._ChannelDisplay.ReadOnly = true;
            this._ChannelDisplay.Size = new System.Drawing.Size(395, 271);
            this._ChannelDisplay.TabIndex = 0;
            this._ChannelDisplay.Text = "";
            // 
            // _SMTPDisplay
            // 
            this._SMTPDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._SMTPDisplay.BackColor = System.Drawing.Color.White;
            this._SMTPDisplay.Location = new System.Drawing.Point(4, 3);
            this._SMTPDisplay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._SMTPDisplay.Name = "_SMTPDisplay";
            this._SMTPDisplay.ReadOnly = true;
            this._SMTPDisplay.Size = new System.Drawing.Size(366, 271);
            this._SMTPDisplay.TabIndex = 0;
            this._SMTPDisplay.Text = "";
            // 
            // _mnuMain
            // 
            this._mnuMain.Enabled = false;
            this._mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuFile,
            this._mnuConnect,
            this._mnuAbort,
            this._mnuLogs,
            this._mnuHelp,
            this._mnuMinutesRemaining,
            this._mnuTest});
            this._mnuMain.Location = new System.Drawing.Point(0, 0);
            this._mnuMain.Name = "_mnuMain";
            this._mnuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this._mnuMain.Size = new System.Drawing.Size(810, 24);
            this._mnuMain.TabIndex = 5;
            // 
            // _mnuFile
            // 
            this._mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuProperties,
            this._mnuPollingInterval,
            this._ToolStripSeparator3,
            this._mnuAGWEngine,
            this._mnuPacketAGWChannels,
            this._mnuPacketTNCChannels,
            this._mnuPactorChannels,
            this._mnuTelnetChannels,
            this._ToolStripSeparator4,
            this._mnuTacticalAccounts,
            this._mnuCallsignAccounts,
            this._mnuEditAccount,
            this._ToolStripSeparator1,
            this._mnuBackup,
            this._mnuRestoreSettings,
            this._ToolStripSeparator2,
            this._mnuExit});
            this._mnuFile.Name = "_mnuFile";
            this._mnuFile.Size = new System.Drawing.Size(61, 20);
            this._mnuFile.Text = "Settings";
            this._mnuFile.ToolTipText = "Setup menues";
            this._mnuFile.Click += new System.EventHandler(this.mnuFile_Click);
            // 
            // _mnuProperties
            // 
            this._mnuProperties.Name = "_mnuProperties";
            this._mnuProperties.Size = new System.Drawing.Size(200, 22);
            this._mnuProperties.Text = "Site Properties...";
            this._mnuProperties.Click += new System.EventHandler(this.mnuProperties_Click);
            // 
            // _mnuPollingInterval
            // 
            this._mnuPollingInterval.Name = "_mnuPollingInterval";
            this._mnuPollingInterval.Size = new System.Drawing.Size(200, 22);
            this._mnuPollingInterval.Text = "P&olling Interval...";
            this._mnuPollingInterval.Click += new System.EventHandler(this.mnuPollingInterval_Click);
            // 
            // _ToolStripSeparator3
            // 
            this._ToolStripSeparator3.Name = "_ToolStripSeparator3";
            this._ToolStripSeparator3.Size = new System.Drawing.Size(197, 6);
            // 
            // _mnuAGWEngine
            // 
            this._mnuAGWEngine.Name = "_mnuAGWEngine";
            this._mnuAGWEngine.Size = new System.Drawing.Size(200, 22);
            this._mnuAGWEngine.Text = "AGW &Engine...";
            this._mnuAGWEngine.Click += new System.EventHandler(this.mnuAGWEngine_Click);
            // 
            // _mnuPacketAGWChannels
            // 
            this._mnuPacketAGWChannels.Name = "_mnuPacketAGWChannels";
            this._mnuPacketAGWChannels.Size = new System.Drawing.Size(200, 22);
            this._mnuPacketAGWChannels.Text = "&Packet AGW Channels...";
            this._mnuPacketAGWChannels.Click += new System.EventHandler(this.mnuPacketAGWChannels_Click);
            // 
            // _mnuPacketTNCChannels
            // 
            this._mnuPacketTNCChannels.Name = "_mnuPacketTNCChannels";
            this._mnuPacketTNCChannels.Size = new System.Drawing.Size(200, 22);
            this._mnuPacketTNCChannels.Text = "Packet &TNC Channels...";
            this._mnuPacketTNCChannels.Click += new System.EventHandler(this.mnuPacketTNCChannels_Click);
            // 
            // _mnuPactorChannels
            // 
            this._mnuPactorChannels.Name = "_mnuPactorChannels";
            this._mnuPactorChannels.Size = new System.Drawing.Size(200, 22);
            this._mnuPactorChannels.Text = "Pactor TNC Channels...";
            this._mnuPactorChannels.Click += new System.EventHandler(this.mnuPactorChannels_Click);
            // 
            // _mnuTelnetChannels
            // 
            this._mnuTelnetChannels.Name = "_mnuTelnetChannels";
            this._mnuTelnetChannels.Size = new System.Drawing.Size(200, 22);
            this._mnuTelnetChannels.Text = "&Telnet Channels...";
            this._mnuTelnetChannels.Click += new System.EventHandler(this.mnuTelnetChannels_Click);
            // 
            // _ToolStripSeparator4
            // 
            this._ToolStripSeparator4.Name = "_ToolStripSeparator4";
            this._ToolStripSeparator4.Size = new System.Drawing.Size(197, 6);
            // 
            // _mnuTacticalAccounts
            // 
            this._mnuTacticalAccounts.Name = "_mnuTacticalAccounts";
            this._mnuTacticalAccounts.Size = new System.Drawing.Size(200, 22);
            this._mnuTacticalAccounts.Text = "Tactical Accounts...";
            this._mnuTacticalAccounts.Click += new System.EventHandler(this.mnuAccounts_Click);
            // 
            // _mnuCallsignAccounts
            // 
            this._mnuCallsignAccounts.Name = "_mnuCallsignAccounts";
            this._mnuCallsignAccounts.Size = new System.Drawing.Size(200, 22);
            this._mnuCallsignAccounts.Text = "Callsign Accounts...";
            this._mnuCallsignAccounts.Click += new System.EventHandler(this.mnuCallsignAccounts_Click);
            // 
            // _mnuEditAccount
            // 
            this._mnuEditAccount.Name = "_mnuEditAccount";
            this._mnuEditAccount.Size = new System.Drawing.Size(200, 22);
            this._mnuEditAccount.Text = "Edit Account Profile...";
            // 
            // _ToolStripSeparator1
            // 
            this._ToolStripSeparator1.Name = "_ToolStripSeparator1";
            this._ToolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            // 
            // _mnuBackup
            // 
            this._mnuBackup.Name = "_mnuBackup";
            this._mnuBackup.Size = new System.Drawing.Size(200, 22);
            this._mnuBackup.Text = "Backup Settings...";
            this._mnuBackup.Click += new System.EventHandler(this.mnuBackup_Click);
            // 
            // _mnuRestoreSettings
            // 
            this._mnuRestoreSettings.Name = "_mnuRestoreSettings";
            this._mnuRestoreSettings.Size = new System.Drawing.Size(200, 22);
            this._mnuRestoreSettings.Text = "Restore Settings...";
            this._mnuRestoreSettings.Click += new System.EventHandler(this.mnuRestoreSettings_Click);
            // 
            // _ToolStripSeparator2
            // 
            this._ToolStripSeparator2.Name = "_ToolStripSeparator2";
            this._ToolStripSeparator2.Size = new System.Drawing.Size(197, 6);
            // 
            // _mnuExit
            // 
            this._mnuExit.Name = "_mnuExit";
            this._mnuExit.Size = new System.Drawing.Size(200, 22);
            this._mnuExit.Text = "E&xit";
            this._mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // _mnuConnect
            // 
            this._mnuConnect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuAutoConnect,
            this._mnuConnectTo});
            this._mnuConnect.Name = "_mnuConnect";
            this._mnuConnect.Size = new System.Drawing.Size(64, 20);
            this._mnuConnect.Text = "Connect";
            this._mnuConnect.ToolTipText = "Manual or automatic connection";
            this._mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // _mnuAutoConnect
            // 
            this._mnuAutoConnect.Name = "_mnuAutoConnect";
            this._mnuAutoConnect.Size = new System.Drawing.Size(143, 22);
            this._mnuAutoConnect.Text = "Autoconnect";
            this._mnuAutoConnect.Click += new System.EventHandler(this.mnuAutoConnect_Click);
            // 
            // _mnuConnectTo
            // 
            this._mnuConnectTo.Name = "_mnuConnectTo";
            this._mnuConnectTo.Size = new System.Drawing.Size(143, 22);
            this._mnuConnectTo.Text = "Connect to...";
            // 
            // _mnuAbort
            // 
            this._mnuAbort.Name = "_mnuAbort";
            this._mnuAbort.Size = new System.Drawing.Size(49, 20);
            this._mnuAbort.Text = "Abort";
            this._mnuAbort.ToolTipText = "Interrupt and connection in progress";
            this._mnuAbort.Click += new System.EventHandler(this.mnuAbort_Click);
            // 
            // _mnuLogs
            // 
            this._mnuLogs.Name = "_mnuLogs";
            this._mnuLogs.Size = new System.Drawing.Size(42, 20);
            this._mnuLogs.Text = "Log ";
            this._mnuLogs.ToolTipText = "Manage and View Log";
            this._mnuLogs.Click += new System.EventHandler(this.mnuLogs_Click);
            // 
            // _mnuHelp
            // 
            this._mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuHelpContents,
            this._mnuSimpleTerminal});
            this._mnuHelp.Name = "_mnuHelp";
            this._mnuHelp.Size = new System.Drawing.Size(44, 20);
            this._mnuHelp.Text = "Help";
            // 
            // _mnuHelpContents
            // 
            this._mnuHelpContents.Name = "_mnuHelpContents";
            this._mnuHelpContents.Size = new System.Drawing.Size(167, 22);
            this._mnuHelpContents.Text = "&Contents...";
            this._mnuHelpContents.Click += new System.EventHandler(this.mnuHelpContents_Click);
            // 
            // _mnuSimpleTerminal
            // 
            this._mnuSimpleTerminal.Name = "_mnuSimpleTerminal";
            this._mnuSimpleTerminal.Size = new System.Drawing.Size(167, 22);
            this._mnuSimpleTerminal.Text = "Simple Terminal...";
            this._mnuSimpleTerminal.Click += new System.EventHandler(this.mnuSimpleTerminal_Click);
            // 
            // _mnuMinutesRemaining
            // 
            this._mnuMinutesRemaining.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._mnuMinutesRemaining.ForeColor = System.Drawing.SystemColors.ControlText;
            this._mnuMinutesRemaining.Name = "_mnuMinutesRemaining";
            this._mnuMinutesRemaining.Size = new System.Drawing.Size(121, 20);
            this._mnuMinutesRemaining.Text = "Next Poll in 0 Minutes";
            // 
            // _mnuTest
            // 
            this._mnuTest.Name = "_mnuTest";
            this._mnuTest.Size = new System.Drawing.Size(12, 20);
            // 
            // _tmrMain
            // 
            this._tmrMain.Interval = 1000;
            this._tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // _tmrDisplay
            // 
            this._tmrDisplay.Enabled = true;
            this._tmrDisplay.Interval = 1000;
            this._tmrDisplay.Tick += new System.EventHandler(this.tmrDisplay_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(810, 354);
            this.Controls.Add(this._pnlSplitter);
            this.Controls.Add(this._lblMessageDisplay);
            this.Controls.Add(this._barStatus);
            this.Controls.Add(this._mnuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Main";
            this.Text = "Paclink (... Initializing)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainClosed);
            this.Load += new System.EventHandler(this.MainLoad);
            this._barStatus.ResumeLayout(false);
            this._barStatus.PerformLayout();
            this._pnlSplitter.Panel1.ResumeLayout(false);
            this._pnlSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pnlSplitter)).EndInit();
            this._pnlSplitter.ResumeLayout(false);
            this._mnuMain.ResumeLayout(false);
            this._mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private ToolStripMenuItem _mnuTest;

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