using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    [DesignerGenerated()]
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
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogTelnetChannels));
            _btnAdd = new Button();
            _btnAdd.Click += new EventHandler(btnAdd_Click);
            _btnRemove = new Button();
            _btnRemove.Click += new EventHandler(btnRemove_Click);
            _btnUpdate = new Button();
            _btnUpdate.Click += new EventHandler(btnUpdate_Click);
            _btnClose = new Button();
            _btnClose.Click += new EventHandler(btnClose_Click);
            _Label1 = new Label();
            _Label2 = new Label();
            _cmbChannelName = new ComboBox();
            _cmbChannelName.SelectedIndexChanged += new EventHandler(cmbChannelName_SelectedIndexChanged);
            _cmbChannelName.TextChanged += new EventHandler(cmbChannelName_TextChanged);
            _chkEnabled = new CheckBox();
            _ToolTip1 = new ToolTip(components);
            _nudPriority = new NumericUpDown();
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            _Label9 = new Label();
            ((System.ComponentModel.ISupportInitialize)_nudPriority).BeginInit();
            SuspendLayout();
            // 
            // btnAdd
            // 
            _btnAdd.Location = new Point(25, 84);
            _btnAdd.Name = "_btnAdd";
            _btnAdd.Size = new Size(129, 30);
            _btnAdd.TabIndex = 10;
            _btnAdd.Text = "Add New Channel";
            _btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            _btnRemove.Location = new Point(160, 84);
            _btnRemove.Name = "_btnRemove";
            _btnRemove.Size = new Size(129, 30);
            _btnRemove.TabIndex = 11;
            _btnRemove.Text = "Remove This Channel";
            _btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            _btnUpdate.Location = new Point(295, 84);
            _btnUpdate.Name = "_btnUpdate";
            _btnUpdate.Size = new Size(129, 30);
            _btnUpdate.TabIndex = 12;
            _btnUpdate.Text = "Update The Channel";
            _btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            _btnClose.Location = new Point(430, 84);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(129, 30);
            _btnClose.TabIndex = 13;
            _btnClose.Text = "Close";
            _ToolTip1.SetToolTip(_btnClose, "Close or abandon edits.");
            _btnClose.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(174, 34);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(78, 13);
            _Label1.TabIndex = 5;
            _Label1.Text = "Channel name:";
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(225, 59);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(82, 13);
            _Label2.TabIndex = 6;
            _Label2.Text = "Channel priority:";
            // 
            // cmbChannelName
            // 
            _cmbChannelName.FormattingEnabled = true;
            _cmbChannelName.Location = new Point(258, 31);
            _cmbChannelName.MaxDropDownItems = 24;
            _cmbChannelName.Name = "_cmbChannelName";
            _cmbChannelName.Size = new Size(286, 21);
            _cmbChannelName.Sorted = true;
            _cmbChannelName.TabIndex = 0;
            _ToolTip1.SetToolTip(_cmbChannelName, "Select a channel or enter a new unique channel name");
            // 
            // chkEnabled
            // 
            _chkEnabled.AutoSize = true;
            _chkEnabled.Location = new Point(387, 61);
            _chkEnabled.Name = "_chkEnabled";
            _chkEnabled.RightToLeft = RightToLeft.Yes;
            _chkEnabled.Size = new Size(106, 17);
            _chkEnabled.TabIndex = 6;
            _chkEnabled.Text = "Channel enabled";
            _ToolTip1.SetToolTip(_chkEnabled, "Check to enable this channel");
            _chkEnabled.UseVisualStyleBackColor = true;
            // 
            // nudPriority
            // 
            _nudPriority.Location = new Point(316, 58);
            _nudPriority.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            _nudPriority.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _nudPriority.Name = "_nudPriority";
            _nudPriority.Size = new Size(36, 20);
            _nudPriority.TabIndex = 16;
            _nudPriority.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudPriority, "Select channel priority 1-5, 1=highest (default = 1)");
            _nudPriority.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnHelp
            // 
            _btnHelp.Location = new Point(565, 84);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(129, 30);
            _btnHelp.TabIndex = 17;
            _btnHelp.Text = "Help";
            _ToolTip1.SetToolTip(_btnHelp, "Close or abandon edits.");
            _btnHelp.UseVisualStyleBackColor = true;
            // 
            // Label9
            // 
            _Label9.AutoSize = true;
            _Label9.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
            _Label9.Location = new Point(119, 9);
            _Label9.Name = "_Label9";
            _Label9.Size = new Size(481, 13);
            _Label9.TabIndex = 145;
            _Label9.Text = "To create a new channel type a new channel name in the Channel Name text box...";
            // 
            // DialogTelnetChannels
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(718, 126);
            Controls.Add(_Label9);
            Controls.Add(_btnHelp);
            Controls.Add(_nudPriority);
            Controls.Add(_chkEnabled);
            Controls.Add(_cmbChannelName);
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
            Name = "DialogTelnetChannels";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Telnet Channels";
            ((System.ComponentModel.ISupportInitialize)_nudPriority).EndInit();
            Load += new EventHandler(TelnetChannels_Load);
            ResumeLayout(false);
            PerformLayout();
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