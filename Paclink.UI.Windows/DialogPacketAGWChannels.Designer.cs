using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPacketAGWChannels));
            this._cmbAGWPort = new System.Windows.Forms.ComboBox();
            this._Label11 = new System.Windows.Forms.Label();
            this._Label10 = new System.Windows.Forms.Label();
            this._Label9 = new System.Windows.Forms.Label();
            this._Label5 = new System.Windows.Forms.Label();
            this._Label4 = new System.Windows.Forms.Label();
            this._txtRemoteCallsign = new System.Windows.Forms.TextBox();
            this._chkEnabled = new System.Windows.Forms.CheckBox();
            this._cmbChannelName = new System.Windows.Forms.ComboBox();
            this._Label3 = new System.Windows.Forms.Label();
            this._Label2 = new System.Windows.Forms.Label();
            this._Label1 = new System.Windows.Forms.Label();
            this._btnClose = new System.Windows.Forms.Button();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._Label8 = new System.Windows.Forms.Label();
            this._Label7 = new System.Windows.Forms.Label();
            this._txtScript = new System.Windows.Forms.TextBox();
            this._Label6 = new System.Windows.Forms.Label();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._nudMaxOutstanding = new System.Windows.Forms.NumericUpDown();
            this._nudActivityTimeout = new System.Windows.Forms.NumericUpDown();
            this._nudPriority = new System.Windows.Forms.NumericUpDown();
            this._nudScriptTimeout = new System.Windows.Forms.NumericUpDown();
            this._btnRetryRemote = new System.Windows.Forms.Button();
            this._nudPacketLength = new System.Windows.Forms.NumericUpDown();
            this._btnHelp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._nudMaxOutstanding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudActivityTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudScriptTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudPacketLength)).BeginInit();
            this.SuspendLayout();
            // 
            // _cmbAGWPort
            // 
            this._cmbAGWPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbAGWPort.FormattingEnabled = true;
            this._cmbAGWPort.Location = new System.Drawing.Point(148, 127);
            this._cmbAGWPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbAGWPort.MaxDropDownItems = 24;
            this._cmbAGWPort.Name = "_cmbAGWPort";
            this._cmbAGWPort.Size = new System.Drawing.Size(483, 23);
            this._cmbAGWPort.Sorted = true;
            this._cmbAGWPort.TabIndex = 2;
            // 
            // _Label11
            // 
            this._Label11.AutoSize = true;
            this._Label11.Location = new System.Drawing.Point(440, 179);
            this._Label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label11.Name = "_Label11";
            this._Label11.Size = new System.Drawing.Size(139, 15);
            this._Label11.TabIndex = 78;
            this._Label11.Text = "Max frames outstanding:";
            // 
            // _Label10
            // 
            this._Label10.AutoSize = true;
            this._Label10.Location = new System.Drawing.Point(255, 179);
            this._Label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label10.Name = "_Label10";
            this._Label10.Size = new System.Drawing.Size(82, 15);
            this._Label10.TabIndex = 77;
            this._Label10.Text = "Packet length:";
            // 
            // _Label9
            // 
            this._Label9.AutoSize = true;
            this._Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label9.Location = new System.Drawing.Point(15, 130);
            this._Label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label9.Name = "_Label9";
            this._Label9.Size = new System.Drawing.Size(108, 13);
            this._Label9.TabIndex = 76;
            this._Label9.Text = "AGW engine port:";
            // 
            // _Label5
            // 
            this._Label5.AutoSize = true;
            this._Label5.Location = new System.Drawing.Point(517, 84);
            this._Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label5.Name = "_Label5";
            this._Label5.Size = new System.Drawing.Size(50, 15);
            this._Label5.TabIndex = 75;
            this._Label5.Text = "minutes";
            // 
            // _Label4
            // 
            this._Label4.AutoSize = true;
            this._Label4.Location = new System.Drawing.Point(374, 84);
            this._Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label4.Name = "_Label4";
            this._Label4.Size = new System.Drawing.Size(95, 15);
            this._Label4.TabIndex = 74;
            this._Label4.Text = "Activity timeout:";
            // 
            // _txtRemoteCallsign
            // 
            this._txtRemoteCallsign.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtRemoteCallsign.Location = new System.Drawing.Point(524, 36);
            this._txtRemoteCallsign.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtRemoteCallsign.Name = "_txtRemoteCallsign";
            this._txtRemoteCallsign.Size = new System.Drawing.Size(108, 23);
            this._txtRemoteCallsign.TabIndex = 1;
            this._txtRemoteCallsign.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _chkEnabled
            // 
            this._chkEnabled.AutoSize = true;
            this._chkEnabled.Checked = true;
            this._chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkEnabled.Location = new System.Drawing.Point(217, 82);
            this._chkEnabled.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkEnabled.Name = "_chkEnabled";
            this._chkEnabled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._chkEnabled.Size = new System.Drawing.Size(118, 19);
            this._chkEnabled.TabIndex = 9;
            this._chkEnabled.Text = "Channel enabled:";
            this._chkEnabled.UseVisualStyleBackColor = true;
            // 
            // _cmbChannelName
            // 
            this._cmbChannelName.FormattingEnabled = true;
            this._cmbChannelName.Location = new System.Drawing.Point(124, 37);
            this._cmbChannelName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbChannelName.MaxDropDownItems = 24;
            this._cmbChannelName.Name = "_cmbChannelName";
            this._cmbChannelName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmbChannelName.Size = new System.Drawing.Size(256, 23);
            this._cmbChannelName.Sorted = true;
            this._cmbChannelName.TabIndex = 0;
            this._cmbChannelName.SelectedIndexChanged += new System.EventHandler(this.cmbChannelName_SelectedIndexChanged);
            this._cmbChannelName.TextChanged += new System.EventHandler(this.cmbChannelName_TextChanged);
            this._cmbChannelName.Leave += new System.EventHandler(this.cmbChannelName_Leave);
            // 
            // _Label3
            // 
            this._Label3.AutoSize = true;
            this._Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label3.Location = new System.Drawing.Point(399, 39);
            this._Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label3.Name = "_Label3";
            this._Label3.Size = new System.Drawing.Size(101, 13);
            this._Label3.TabIndex = 67;
            this._Label3.Text = "Remote callsign:";
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(56, 82);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(95, 15);
            this._Label2.TabIndex = 66;
            this._Label2.Text = "Channel priority:";
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label1.Location = new System.Drawing.Point(10, 39);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(91, 13);
            this._Label1.TabIndex = 65;
            this._Label1.Text = "Channel name:";
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(573, 216);
            this._btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(150, 35);
            this._btnClose.TabIndex = 16;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Location = new System.Drawing.Point(415, 216);
            this._btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size(150, 35);
            this._btnUpdate.TabIndex = 15;
            this._btnUpdate.Text = "Update The Channel";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.Location = new System.Drawing.Point(258, 216);
            this._btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(150, 35);
            this._btnRemove.TabIndex = 14;
            this._btnRemove.Text = "Remove This Channel";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point(100, 216);
            this._btnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(150, 35);
            this._btnAdd.TabIndex = 13;
            this._btnAdd.Text = "Add New Channel";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // _Label8
            // 
            this._Label8.AutoSize = true;
            this._Label8.CausesValidation = false;
            this._Label8.Location = new System.Drawing.Point(905, 179);
            this._Label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label8.Name = "_Label8";
            this._Label8.Size = new System.Drawing.Size(50, 15);
            this._Label8.TabIndex = 123;
            this._Label8.Text = "seconds";
            // 
            // _Label7
            // 
            this._Label7.AutoSize = true;
            this._Label7.CausesValidation = false;
            this._Label7.Location = new System.Drawing.Point(654, 179);
            this._Label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label7.Name = "_Label7";
            this._Label7.Size = new System.Drawing.Size(186, 15);
            this._Label7.TabIndex = 122;
            this._Label7.Text = "Connect/Script inactivity timeout:";
            this._ToolTip1.SetToolTip(this._Label7, "Sets the timeout for scrips or simple connects.");
            // 
            // _txtScript
            // 
            this._txtScript.CausesValidation = false;
            this._txtScript.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtScript.Location = new System.Drawing.Point(658, 29);
            this._txtScript.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtScript.Multiline = true;
            this._txtScript.Name = "_txtScript";
            this._txtScript.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtScript.Size = new System.Drawing.Size(302, 133);
            this._txtScript.TabIndex = 3;
            // 
            // _Label6
            // 
            this._Label6.AutoSize = true;
            this._Label6.CausesValidation = false;
            this._Label6.Location = new System.Drawing.Point(727, 10);
            this._Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label6.Name = "_Label6";
            this._Label6.Size = new System.Drawing.Size(134, 15);
            this._Label6.TabIndex = 121;
            this._Label6.Text = "Optional connect script:";
            // 
            // _nudMaxOutstanding
            // 
            this._nudMaxOutstanding.Location = new System.Drawing.Point(589, 177);
            this._nudMaxOutstanding.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudMaxOutstanding.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this._nudMaxOutstanding.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._nudMaxOutstanding.Name = "_nudMaxOutstanding";
            this._nudMaxOutstanding.Size = new System.Drawing.Size(44, 23);
            this._nudMaxOutstanding.TabIndex = 125;
            this._nudMaxOutstanding.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudMaxOutstanding, "Select Maximum # of outstanding frames (default = 2)");
            this._nudMaxOutstanding.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // _nudActivityTimeout
            // 
            this._nudActivityTimeout.Location = new System.Drawing.Point(475, 81);
            this._nudActivityTimeout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudActivityTimeout.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this._nudActivityTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._nudActivityTimeout.Name = "_nudActivityTimeout";
            this._nudActivityTimeout.Size = new System.Drawing.Size(40, 23);
            this._nudActivityTimeout.TabIndex = 126;
            this._nudActivityTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudActivityTimeout, "Select Activity Timeout (minutes) default = 4");
            this._nudActivityTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _nudPriority
            // 
            this._nudPriority.Location = new System.Drawing.Point(160, 80);
            this._nudPriority.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudPriority.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this._nudPriority.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._nudPriority.Name = "_nudPriority";
            this._nudPriority.Size = new System.Drawing.Size(38, 23);
            this._nudPriority.TabIndex = 129;
            this._nudPriority.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudPriority, "Select channel priority 1-5, 1=highest (default = 3)");
            this._nudPriority.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _nudScriptTimeout
            // 
            this._nudScriptTimeout.Location = new System.Drawing.Point(852, 177);
            this._nudScriptTimeout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudScriptTimeout.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this._nudScriptTimeout.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this._nudScriptTimeout.Name = "_nudScriptTimeout";
            this._nudScriptTimeout.Size = new System.Drawing.Size(49, 23);
            this._nudScriptTimeout.TabIndex = 128;
            this._nudScriptTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudScriptTimeout, "Sets the timeout for scrips or simple connects.");
            this._nudScriptTimeout.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // _btnRetryRemote
            // 
            this._btnRetryRemote.Enabled = false;
            this._btnRetryRemote.Location = new System.Drawing.Point(34, 173);
            this._btnRetryRemote.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnRetryRemote.Name = "_btnRetryRemote";
            this._btnRetryRemote.Size = new System.Drawing.Size(194, 27);
            this._btnRetryRemote.TabIndex = 124;
            this._btnRetryRemote.Text = "Get Remote Port Information";
            this._btnRetryRemote.UseVisualStyleBackColor = true;
            this._btnRetryRemote.Click += new System.EventHandler(this.btnRetryRemote_Click);
            // 
            // _nudPacketLength
            // 
            this._nudPacketLength.Location = new System.Drawing.Point(351, 177);
            this._nudPacketLength.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudPacketLength.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this._nudPacketLength.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this._nudPacketLength.Name = "_nudPacketLength";
            this._nudPacketLength.Size = new System.Drawing.Size(59, 23);
            this._nudPacketLength.TabIndex = 127;
            this._nudPacketLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._nudPacketLength.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // _btnHelp
            // 
            this._btnHelp.Location = new System.Drawing.Point(730, 216);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(150, 35);
            this._btnHelp.TabIndex = 130;
            this._btnHelp.Text = "Help";
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // DialogPacketAGWChannels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(981, 270);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._nudPriority);
            this.Controls.Add(this._nudScriptTimeout);
            this.Controls.Add(this._nudPacketLength);
            this.Controls.Add(this._nudActivityTimeout);
            this.Controls.Add(this._nudMaxOutstanding);
            this.Controls.Add(this._btnRetryRemote);
            this.Controls.Add(this._Label8);
            this.Controls.Add(this._Label7);
            this.Controls.Add(this._txtScript);
            this.Controls.Add(this._Label6);
            this.Controls.Add(this._cmbAGWPort);
            this.Controls.Add(this._Label11);
            this.Controls.Add(this._Label10);
            this.Controls.Add(this._Label9);
            this.Controls.Add(this._Label5);
            this.Controls.Add(this._Label4);
            this.Controls.Add(this._txtRemoteCallsign);
            this.Controls.Add(this._chkEnabled);
            this.Controls.Add(this._cmbChannelName);
            this.Controls.Add(this._Label3);
            this.Controls.Add(this._Label2);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._btnUpdate);
            this.Controls.Add(this._btnRemove);
            this.Controls.Add(this._btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogPacketAGWChannels";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packet AGW Channels";
            this.Load += new System.EventHandler(this.PacketAGWChannels_Load);
            ((System.ComponentModel.ISupportInitialize)(this._nudMaxOutstanding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudActivityTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudScriptTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudPacketLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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