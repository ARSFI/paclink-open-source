using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public partial class DialogTelnetChannels : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogTelnetChannels));
            this._btnAdd = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._btnClose = new System.Windows.Forms.Button();
            this._Label1 = new System.Windows.Forms.Label();
            this._Label2 = new System.Windows.Forms.Label();
            this._cmbChannelName = new System.Windows.Forms.ComboBox();
            this._chkEnabled = new System.Windows.Forms.CheckBox();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._nudPriority = new System.Windows.Forms.NumericUpDown();
            this._btnHelp = new System.Windows.Forms.Button();
            this._Label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._nudPriority)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point(29, 97);
            this._btnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(150, 35);
            this._btnAdd.TabIndex = 10;
            this._btnAdd.Text = "Add New Channel";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.Location = new System.Drawing.Point(187, 97);
            this._btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(150, 35);
            this._btnRemove.TabIndex = 11;
            this._btnRemove.Text = "Remove This Channel";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Location = new System.Drawing.Point(344, 97);
            this._btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size(150, 35);
            this._btnUpdate.TabIndex = 12;
            this._btnUpdate.Text = "Update The Channel";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(502, 97);
            this._btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(150, 35);
            this._btnClose.TabIndex = 13;
            this._btnClose.Text = "Close";
            this._ToolTip1.SetToolTip(this._btnClose, "Close or abandon edits.");
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(203, 39);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(87, 15);
            this._Label1.TabIndex = 5;
            this._Label1.Text = "Channel name:";
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(262, 68);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(95, 15);
            this._Label2.TabIndex = 6;
            this._Label2.Text = "Channel priority:";
            // 
            // _cmbChannelName
            // 
            this._cmbChannelName.FormattingEnabled = true;
            this._cmbChannelName.Location = new System.Drawing.Point(301, 36);
            this._cmbChannelName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbChannelName.MaxDropDownItems = 24;
            this._cmbChannelName.Name = "_cmbChannelName";
            this._cmbChannelName.Size = new System.Drawing.Size(333, 23);
            this._cmbChannelName.Sorted = true;
            this._cmbChannelName.TabIndex = 0;
            this._ToolTip1.SetToolTip(this._cmbChannelName, "Select a channel or enter a new unique channel name");
            this._cmbChannelName.SelectedIndexChanged += new System.EventHandler(this.cmbChannelName_SelectedIndexChanged);
            this._cmbChannelName.TextChanged += new System.EventHandler(this.cmbChannelName_TextChanged);
            // 
            // _chkEnabled
            // 
            this._chkEnabled.AutoSize = true;
            this._chkEnabled.Location = new System.Drawing.Point(451, 70);
            this._chkEnabled.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkEnabled.Name = "_chkEnabled";
            this._chkEnabled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._chkEnabled.Size = new System.Drawing.Size(115, 19);
            this._chkEnabled.TabIndex = 6;
            this._chkEnabled.Text = "Channel enabled";
            this._ToolTip1.SetToolTip(this._chkEnabled, "Check to enable this channel");
            this._chkEnabled.UseVisualStyleBackColor = true;
            // 
            // _nudPriority
            // 
            this._nudPriority.Location = new System.Drawing.Point(369, 67);
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
            this._nudPriority.Size = new System.Drawing.Size(42, 23);
            this._nudPriority.TabIndex = 16;
            this._nudPriority.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudPriority, "Select channel priority 1-5, 1=highest (default = 1)");
            this._nudPriority.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _btnHelp
            // 
            this._btnHelp.Location = new System.Drawing.Point(659, 97);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(150, 35);
            this._btnHelp.TabIndex = 17;
            this._btnHelp.Text = "Help";
            this._ToolTip1.SetToolTip(this._btnHelp, "Close or abandon edits.");
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // _Label9
            // 
            this._Label9.AutoSize = true;
            this._Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label9.Location = new System.Drawing.Point(139, 10);
            this._Label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label9.Name = "_Label9";
            this._Label9.Size = new System.Drawing.Size(481, 13);
            this._Label9.TabIndex = 145;
            this._Label9.Text = "To create a new channel type a new channel name in the Channel Name text box...";
            // 
            // DialogTelnetChannels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(838, 145);
            this.Controls.Add(this._Label9);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._nudPriority);
            this.Controls.Add(this._chkEnabled);
            this.Controls.Add(this._cmbChannelName);
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
            this.Name = "DialogTelnetChannels";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Telnet Channels";
            this.Load += new System.EventHandler(this.TelnetChannels_Load);
            ((System.ComponentModel.ISupportInitialize)(this._nudPriority)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
                    _cmbChannelName.SelectedIndexChanged -= cmbChannelName_SelectedIndexChanged;
                    _cmbChannelName.TextChanged -= cmbChannelName_TextChanged;
                }

                _cmbChannelName = value;
                if (_cmbChannelName != null)
                {
                    _cmbChannelName.SelectedIndexChanged += cmbChannelName_SelectedIndexChanged;
                    _cmbChannelName.TextChanged += cmbChannelName_TextChanged;
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
    }
}