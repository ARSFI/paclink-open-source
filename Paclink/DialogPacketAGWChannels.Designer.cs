using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    [DesignerGenerated()]
    public partial class DialogPacketAGWChannels : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPacketAGWChannels));
            _cmbAGWPort = new ComboBox();
            _Label11 = new Label();
            _Label10 = new Label();
            _Label9 = new Label();
            _Label5 = new Label();
            _Label4 = new Label();
            _txtRemoteCallsign = new TextBox();
            _chkEnabled = new CheckBox();
            _cmbChannelName = new ComboBox();
            _cmbChannelName.Leave += new EventHandler(cmbChannelName_Leave);
            _cmbChannelName.SelectedIndexChanged += new EventHandler(cmbChannelName_SelectedIndexChanged);
            _cmbChannelName.TextChanged += new EventHandler(cmbChannelName_TextChanged);
            _Label3 = new Label();
            _Label2 = new Label();
            _Label1 = new Label();
            _btnClose = new Button();
            _btnClose.Click += new EventHandler(btnClose_Click);
            _btnUpdate = new Button();
            _btnUpdate.Click += new EventHandler(btnUpdate_Click);
            _btnRemove = new Button();
            _btnRemove.Click += new EventHandler(btnRemove_Click);
            _btnAdd = new Button();
            _btnAdd.Click += new EventHandler(btnAdd_Click);
            _Label8 = new Label();
            _Label7 = new Label();
            _txtScript = new TextBox();
            _Label6 = new Label();
            _ToolTip1 = new ToolTip(components);
            _nudMaxOutstanding = new NumericUpDown();
            _nudActivityTimeout = new NumericUpDown();
            _nudPriority = new NumericUpDown();
            _nudScriptTimeout = new NumericUpDown();
            _tmrTimer10sec = new Timer(components);
            _tmrTimer10sec.Tick += new EventHandler(tmrTimer10sec_Tick);
            _btnRetryRemote = new Button();
            _btnRetryRemote.Click += new EventHandler(btnRetryRemote_Click);
            _nudPacketLength = new NumericUpDown();
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            ((System.ComponentModel.ISupportInitialize)_nudMaxOutstanding).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudActivityTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudPriority).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudScriptTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudPacketLength).BeginInit();
            SuspendLayout();
            // 
            // cmbAGWPort
            // 
            _cmbAGWPort.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbAGWPort.FormattingEnabled = true;
            _cmbAGWPort.Location = new Point(127, 110);
            _cmbAGWPort.MaxDropDownItems = 24;
            _cmbAGWPort.Name = "_cmbAGWPort";
            _cmbAGWPort.Size = new Size(415, 21);
            _cmbAGWPort.Sorted = true;
            _cmbAGWPort.TabIndex = 2;
            // 
            // Label11
            // 
            _Label11.AutoSize = true;
            _Label11.Location = new Point(377, 155);
            _Label11.Name = "_Label11";
            _Label11.Size = new Size(122, 13);
            _Label11.TabIndex = 78;
            _Label11.Text = "Max frames outstanding:";
            // 
            // Label10
            // 
            _Label10.AutoSize = true;
            _Label10.Location = new Point(219, 155);
            _Label10.Name = "_Label10";
            _Label10.Size = new Size(76, 13);
            _Label10.TabIndex = 77;
            _Label10.Text = "Packet length:";
            // 
            // Label9
            // 
            _Label9.AutoSize = true;
            _Label9.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label9.Location = new Point(13, 113);
            _Label9.Name = "_Label9";
            _Label9.Size = new Size(108, 13);
            _Label9.TabIndex = 76;
            _Label9.Text = "AGW engine port:";
            // 
            // Label5
            // 
            _Label5.AutoSize = true;
            _Label5.Location = new Point(443, 73);
            _Label5.Name = "_Label5";
            _Label5.Size = new Size(43, 13);
            _Label5.TabIndex = 75;
            _Label5.Text = "minutes";
            // 
            // Label4
            // 
            _Label4.AutoSize = true;
            _Label4.Location = new Point(321, 73);
            _Label4.Name = "_Label4";
            _Label4.Size = new Size(81, 13);
            _Label4.TabIndex = 74;
            _Label4.Text = "Activity timeout:";
            // 
            // txtRemoteCallsign
            // 
            _txtRemoteCallsign.CharacterCasing = CharacterCasing.Upper;
            _txtRemoteCallsign.Location = new Point(449, 31);
            _txtRemoteCallsign.Name = "_txtRemoteCallsign";
            _txtRemoteCallsign.Size = new Size(93, 20);
            _txtRemoteCallsign.TabIndex = 1;
            _txtRemoteCallsign.TextAlign = HorizontalAlignment.Center;
            // 
            // chkEnabled
            // 
            _chkEnabled.AutoSize = true;
            _chkEnabled.Checked = true;
            _chkEnabled.CheckState = CheckState.Checked;
            _chkEnabled.Location = new Point(186, 71);
            _chkEnabled.Name = "_chkEnabled";
            _chkEnabled.RightToLeft = RightToLeft.Yes;
            _chkEnabled.Size = new Size(109, 17);
            _chkEnabled.TabIndex = 9;
            _chkEnabled.Text = "Channel enabled:";
            _chkEnabled.UseVisualStyleBackColor = true;
            // 
            // cmbChannelName
            // 
            _cmbChannelName.FormattingEnabled = true;
            _cmbChannelName.Location = new Point(106, 32);
            _cmbChannelName.MaxDropDownItems = 24;
            _cmbChannelName.Name = "_cmbChannelName";
            _cmbChannelName.RightToLeft = RightToLeft.No;
            _cmbChannelName.Size = new Size(220, 21);
            _cmbChannelName.Sorted = true;
            _cmbChannelName.TabIndex = 0;
            // 
            // Label3
            // 
            _Label3.AutoSize = true;
            _Label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label3.Location = new Point(342, 34);
            _Label3.Name = "_Label3";
            _Label3.Size = new Size(101, 13);
            _Label3.TabIndex = 67;
            _Label3.Text = "Remote callsign:";
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(48, 71);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(82, 13);
            _Label2.TabIndex = 66;
            _Label2.Text = "Channel priority:";
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label1.Location = new Point(9, 34);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(91, 13);
            _Label1.TabIndex = 65;
            _Label1.Text = "Channel name:";
            // 
            // btnClose
            // 
            _btnClose.Location = new Point(491, 187);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(129, 30);
            _btnClose.TabIndex = 16;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            _btnUpdate.Location = new Point(356, 187);
            _btnUpdate.Name = "_btnUpdate";
            _btnUpdate.Size = new Size(129, 30);
            _btnUpdate.TabIndex = 15;
            _btnUpdate.Text = "Update The Channel";
            _btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            _btnRemove.Location = new Point(221, 187);
            _btnRemove.Name = "_btnRemove";
            _btnRemove.Size = new Size(129, 30);
            _btnRemove.TabIndex = 14;
            _btnRemove.Text = "Remove This Channel";
            _btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            _btnAdd.Location = new Point(86, 187);
            _btnAdd.Name = "_btnAdd";
            _btnAdd.Size = new Size(129, 30);
            _btnAdd.TabIndex = 13;
            _btnAdd.Text = "Add New Channel";
            _btnAdd.UseVisualStyleBackColor = true;
            // 
            // Label8
            // 
            _Label8.AutoSize = true;
            _Label8.CausesValidation = false;
            _Label8.Location = new Point(776, 155);
            _Label8.Name = "_Label8";
            _Label8.Size = new Size(47, 13);
            _Label8.TabIndex = 123;
            _Label8.Text = "seconds";
            // 
            // Label7
            // 
            _Label7.AutoSize = true;
            _Label7.CausesValidation = false;
            _Label7.Location = new Point(561, 155);
            _Label7.Name = "_Label7";
            _Label7.Size = new Size(163, 13);
            _Label7.TabIndex = 122;
            _Label7.Text = "Connect/Script inactivity timeout:";
            _ToolTip1.SetToolTip(_Label7, "Sets the timeout for scrips or simple connects.");
            // 
            // txtScript
            // 
            _txtScript.CausesValidation = false;
            _txtScript.CharacterCasing = CharacterCasing.Upper;
            _txtScript.Location = new Point(564, 25);
            _txtScript.Multiline = true;
            _txtScript.Name = "_txtScript";
            _txtScript.RightToLeft = RightToLeft.No;
            _txtScript.ScrollBars = ScrollBars.Both;
            _txtScript.Size = new Size(259, 116);
            _txtScript.TabIndex = 3;
            // 
            // Label6
            // 
            _Label6.AutoSize = true;
            _Label6.CausesValidation = false;
            _Label6.Location = new Point(623, 9);
            _Label6.Name = "_Label6";
            _Label6.Size = new Size(119, 13);
            _Label6.TabIndex = 121;
            _Label6.Text = "Optional connect script:";
            // 
            // nudMaxOutstanding
            // 
            _nudMaxOutstanding.Location = new Point(505, 153);
            _nudMaxOutstanding.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            _nudMaxOutstanding.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _nudMaxOutstanding.Name = "_nudMaxOutstanding";
            _nudMaxOutstanding.Size = new Size(38, 20);
            _nudMaxOutstanding.TabIndex = 125;
            _nudMaxOutstanding.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudMaxOutstanding, "Select Maximum # of outstanding frames (default = 2)");
            _nudMaxOutstanding.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // nudActivityTimeout
            // 
            _nudActivityTimeout.Location = new Point(407, 70);
            _nudActivityTimeout.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            _nudActivityTimeout.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _nudActivityTimeout.Name = "_nudActivityTimeout";
            _nudActivityTimeout.Size = new Size(34, 20);
            _nudActivityTimeout.TabIndex = 126;
            _nudActivityTimeout.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudActivityTimeout, "Select Activity Timeout (minutes) default = 4");
            _nudActivityTimeout.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // nudPriority
            // 
            _nudPriority.Location = new Point(137, 69);
            _nudPriority.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            _nudPriority.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _nudPriority.Name = "_nudPriority";
            _nudPriority.Size = new Size(33, 20);
            _nudPriority.TabIndex = 129;
            _nudPriority.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudPriority, "Select channel priority 1-5, 1=highest (default = 3)");
            _nudPriority.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // nudScriptTimeout
            // 
            _nudScriptTimeout.Location = new Point(730, 153);
            _nudScriptTimeout.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            _nudScriptTimeout.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            _nudScriptTimeout.Name = "_nudScriptTimeout";
            _nudScriptTimeout.Size = new Size(42, 20);
            _nudScriptTimeout.TabIndex = 128;
            _nudScriptTimeout.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudScriptTimeout, "Sets the timeout for scrips or simple connects.");
            _nudScriptTimeout.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // tmrTimer10sec
            // 
            _tmrTimer10sec.Interval = 10000;
            // 
            // btnRetryRemote
            // 
            _btnRetryRemote.Enabled = false;
            _btnRetryRemote.Location = new Point(29, 150);
            _btnRetryRemote.Name = "_btnRetryRemote";
            _btnRetryRemote.Size = new Size(166, 23);
            _btnRetryRemote.TabIndex = 124;
            _btnRetryRemote.Text = "Get Remote Port Information";
            _btnRetryRemote.UseVisualStyleBackColor = true;
            // 
            // nudPacketLength
            // 
            _nudPacketLength.Location = new Point(301, 153);
            _nudPacketLength.Maximum = new decimal(new int[] { 256, 0, 0, 0 });
            _nudPacketLength.Minimum = new decimal(new int[] { 32, 0, 0, 0 });
            _nudPacketLength.Name = "_nudPacketLength";
            _nudPacketLength.Size = new Size(51, 20);
            _nudPacketLength.TabIndex = 127;
            _nudPacketLength.TextAlign = HorizontalAlignment.Center;
            _nudPacketLength.Value = new decimal(new int[] { 128, 0, 0, 0 });
            // 
            // btnHelp
            // 
            _btnHelp.Location = new Point(626, 187);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(129, 30);
            _btnHelp.TabIndex = 130;
            _btnHelp.Text = "Help";
            _btnHelp.UseVisualStyleBackColor = true;
            // 
            // DialogPacketAGWChannels
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(841, 234);
            Controls.Add(_btnHelp);
            Controls.Add(_nudPriority);
            Controls.Add(_nudScriptTimeout);
            Controls.Add(_nudPacketLength);
            Controls.Add(_nudActivityTimeout);
            Controls.Add(_nudMaxOutstanding);
            Controls.Add(_btnRetryRemote);
            Controls.Add(_Label8);
            Controls.Add(_Label7);
            Controls.Add(_txtScript);
            Controls.Add(_Label6);
            Controls.Add(_cmbAGWPort);
            Controls.Add(_Label11);
            Controls.Add(_Label10);
            Controls.Add(_Label9);
            Controls.Add(_Label5);
            Controls.Add(_Label4);
            Controls.Add(_txtRemoteCallsign);
            Controls.Add(_chkEnabled);
            Controls.Add(_cmbChannelName);
            Controls.Add(_Label3);
            Controls.Add(_Label2);
            Controls.Add(_Label1);
            Controls.Add(_btnClose);
            Controls.Add(_btnUpdate);
            Controls.Add(_btnRemove);
            Controls.Add(_btnAdd);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogPacketAGWChannels";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Packet AGW Channels";
            ((System.ComponentModel.ISupportInitialize)_nudMaxOutstanding).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudActivityTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudPriority).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudScriptTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudPacketLength).EndInit();
            Load += new EventHandler(PacketAGWChannels_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private ComboBox _cmbAGWPort;

        internal ComboBox cmbAGWPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbAGWPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbAGWPort != null)
                {
                }

                _cmbAGWPort = value;
                if (_cmbAGWPort != null)
                {
                }
            }
        }

        private Label _Label11;

        internal Label Label11
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label11;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label11 != null)
                {
                }

                _Label11 = value;
                if (_Label11 != null)
                {
                }
            }
        }

        private Label _Label10;

        internal Label Label10
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label10;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label10 != null)
                {
                }

                _Label10 = value;
                if (_Label10 != null)
                {
                }
            }
        }

        private Label _Label9;

        internal Label Label9
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label9;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label9 != null)
                {
                }

                _Label9 = value;
                if (_Label9 != null)
                {
                }
            }
        }

        private Label _Label5;

        internal Label Label5
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label5;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label5 != null)
                {
                }

                _Label5 = value;
                if (_Label5 != null)
                {
                }
            }
        }

        private Label _Label4;

        internal Label Label4
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label4;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label4 != null)
                {
                }

                _Label4 = value;
                if (_Label4 != null)
                {
                }
            }
        }

        private TextBox _txtRemoteCallsign;

        internal TextBox txtRemoteCallsign
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtRemoteCallsign;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtRemoteCallsign != null)
                {
                }

                _txtRemoteCallsign = value;
                if (_txtRemoteCallsign != null)
                {
                }
            }
        }

        private CheckBox _chkEnabled;

        internal CheckBox chkEnabled
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkEnabled;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkEnabled != null)
                {
                }

                _chkEnabled = value;
                if (_chkEnabled != null)
                {
                }
            }
        }

        private ComboBox _cmbChannelName;

        internal ComboBox cmbChannelName
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbChannelName;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbChannelName != null)
                {
                    _cmbChannelName.Leave -= cmbChannelName_Leave;
                    _cmbChannelName.SelectedIndexChanged -= cmbChannelName_SelectedIndexChanged;
                    _cmbChannelName.TextChanged -= cmbChannelName_TextChanged;
                }

                _cmbChannelName = value;
                if (_cmbChannelName != null)
                {
                    _cmbChannelName.Leave += cmbChannelName_Leave;
                    _cmbChannelName.SelectedIndexChanged += cmbChannelName_SelectedIndexChanged;
                    _cmbChannelName.TextChanged += cmbChannelName_TextChanged;
                }
            }
        }

        private Label _Label3;

        internal Label Label3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label3 != null)
                {
                }

                _Label3 = value;
                if (_Label3 != null)
                {
                }
            }
        }

        private Label _Label2;

        internal Label Label2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label2 != null)
                {
                }

                _Label2 = value;
                if (_Label2 != null)
                {
                }
            }
        }

        private Label _Label1;

        internal Label Label1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label1 != null)
                {
                }

                _Label1 = value;
                if (_Label1 != null)
                {
                }
            }
        }

        private Button _btnClose;

        internal Button btnClose
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnClose;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnClose != null)
                {
                    _btnClose.Click -= btnClose_Click;
                }

                _btnClose = value;
                if (_btnClose != null)
                {
                    _btnClose.Click += btnClose_Click;
                }
            }
        }

        private Button _btnUpdate;

        internal Button btnUpdate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdate != null)
                {
                    _btnUpdate.Click -= btnUpdate_Click;
                }

                _btnUpdate = value;
                if (_btnUpdate != null)
                {
                    _btnUpdate.Click += btnUpdate_Click;
                }
            }
        }

        private Button _btnRemove;

        internal Button btnRemove
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnRemove;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnRemove != null)
                {
                    _btnRemove.Click -= btnRemove_Click;
                }

                _btnRemove = value;
                if (_btnRemove != null)
                {
                    _btnRemove.Click += btnRemove_Click;
                }
            }
        }

        private Button _btnAdd;

        internal Button btnAdd
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAdd;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAdd != null)
                {
                    _btnAdd.Click -= btnAdd_Click;
                }

                _btnAdd = value;
                if (_btnAdd != null)
                {
                    _btnAdd.Click += btnAdd_Click;
                }
            }
        }

        private Label _Label8;

        internal Label Label8
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label8;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label8 != null)
                {
                }

                _Label8 = value;
                if (_Label8 != null)
                {
                }
            }
        }

        private Label _Label7;

        internal Label Label7
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label7;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label7 != null)
                {
                }

                _Label7 = value;
                if (_Label7 != null)
                {
                }
            }
        }

        private TextBox _txtScript;

        internal TextBox txtScript
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtScript;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtScript != null)
                {
                }

                _txtScript = value;
                if (_txtScript != null)
                {
                }
            }
        }

        private Label _Label6;

        internal Label Label6
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label6;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label6 != null)
                {
                }

                _Label6 = value;
                if (_Label6 != null)
                {
                }
            }
        }

        private ToolTip _ToolTip1;

        internal ToolTip ToolTip1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolTip1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolTip1 != null)
                {
                }

                _ToolTip1 = value;
                if (_ToolTip1 != null)
                {
                }
            }
        }

        private Timer _tmrTimer10sec;

        internal Timer tmrTimer10sec
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrTimer10sec;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrTimer10sec != null)
                {
                    _tmrTimer10sec.Tick -= tmrTimer10sec_Tick;
                }

                _tmrTimer10sec = value;
                if (_tmrTimer10sec != null)
                {
                    _tmrTimer10sec.Tick += tmrTimer10sec_Tick;
                }
            }
        }

        private Button _btnRetryRemote;

        internal Button btnRetryRemote
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnRetryRemote;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnRetryRemote != null)
                {
                    _btnRetryRemote.Click -= btnRetryRemote_Click;
                }

                _btnRetryRemote = value;
                if (_btnRetryRemote != null)
                {
                    _btnRetryRemote.Click += btnRetryRemote_Click;
                }
            }
        }

        private NumericUpDown _nudMaxOutstanding;

        internal NumericUpDown nudMaxOutstanding
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudMaxOutstanding;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudMaxOutstanding != null)
                {
                }

                _nudMaxOutstanding = value;
                if (_nudMaxOutstanding != null)
                {
                }
            }
        }

        private NumericUpDown _nudActivityTimeout;

        internal NumericUpDown nudActivityTimeout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudActivityTimeout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudActivityTimeout != null)
                {
                }

                _nudActivityTimeout = value;
                if (_nudActivityTimeout != null)
                {
                }
            }
        }

        private NumericUpDown _nudPacketLength;

        internal NumericUpDown nudPacketLength
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudPacketLength;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudPacketLength != null)
                {
                }

                _nudPacketLength = value;
                if (_nudPacketLength != null)
                {
                }
            }
        }

        private NumericUpDown _nudScriptTimeout;

        internal NumericUpDown nudScriptTimeout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudScriptTimeout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudScriptTimeout != null)
                {
                }

                _nudScriptTimeout = value;
                if (_nudScriptTimeout != null)
                {
                }
            }
        }

        private NumericUpDown _nudPriority;

        internal NumericUpDown nudPriority
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudPriority;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudPriority != null)
                {
                }

                _nudPriority = value;
                if (_nudPriority != null)
                {
                }
            }
        }

        private Button _btnHelp;

        internal Button btnHelp
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnHelp;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnHelp != null)
                {
                    _btnHelp.Click -= btnHelp_Click;
                }

                _btnHelp = value;
                if (_btnHelp != null)
                {
                    _btnHelp.Click += btnHelp_Click;
                }
            }
        }
    }
}