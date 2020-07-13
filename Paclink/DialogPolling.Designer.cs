using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogPolling : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPolling));
            _chkAutoPoll = new CheckBox();
            _chkAutoPoll.CheckedChanged += new EventHandler(chkAutoPoll_CheckedChanged);
            _chkAutoSend = new CheckBox();
            _txtInterval = new TextBox();
            _Label1 = new Label();
            _btnUpdate = new Button();
            _btnUpdate.Click += new EventHandler(btnUpdate_Click);
            _btnCancel = new Button();
            _btnCancel.Click += new EventHandler(btnCancel_Click);
            _ToolTip1 = new ToolTip(components);
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            SuspendLayout();
            // 
            // chkAutoPoll
            // 
            _chkAutoPoll.AutoSize = true;
            _chkAutoPoll.Location = new Point(26, 17);
            _chkAutoPoll.Name = "_chkAutoPoll";
            _chkAutoPoll.Size = new Size(136, 17);
            _chkAutoPoll.TabIndex = 0;
            _chkAutoPoll.Text = "Automatically poll every";
            _ToolTip1.SetToolTip(_chkAutoPoll, "Check to poll on a schedule");
            _chkAutoPoll.UseVisualStyleBackColor = true;
            // 
            // chkAutoSend
            // 
            _chkAutoSend.AutoSize = true;
            _chkAutoSend.Location = new Point(26, 41);
            _chkAutoSend.Name = "_chkAutoSend";
            _chkAutoSend.Size = new Size(332, 17);
            _chkAutoSend.TabIndex = 2;
            _chkAutoSend.Text = "Automatically send any pending messages without waiting for poll";
            _ToolTip1.SetToolTip(_chkAutoSend, "Check to poll immediately upon receiving any pending outbound messages.");
            _chkAutoSend.UseVisualStyleBackColor = true;
            // 
            // txtInterval
            // 
            _txtInterval.Location = new Point(168, 15);
            _txtInterval.Name = "_txtInterval";
            _txtInterval.Size = new Size(26, 20);
            _txtInterval.TabIndex = 1;
            _txtInterval.Text = "60";
            _txtInterval.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtInterval, "polling interval (default = 60)");
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(200, 18);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(43, 13);
            _Label1.TabIndex = 3;
            _Label1.Text = "minutes";
            // 
            // btnUpdate
            // 
            _btnUpdate.Location = new Point(27, 73);
            _btnUpdate.Name = "_btnUpdate";
            _btnUpdate.Size = new Size(107, 28);
            _btnUpdate.TabIndex = 3;
            _btnUpdate.Text = "Update";
            _btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            _btnCancel.Location = new Point(140, 73);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(107, 28);
            _btnCancel.TabIndex = 4;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            _btnHelp.Location = new Point(253, 73);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(107, 28);
            _btnHelp.TabIndex = 5;
            _btnHelp.Text = "Help";
            _btnHelp.UseVisualStyleBackColor = true;
            // 
            // DialogPolling
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(386, 121);
            Controls.Add(_btnHelp);
            Controls.Add(_btnCancel);
            Controls.Add(_btnUpdate);
            Controls.Add(_Label1);
            Controls.Add(_txtInterval);
            Controls.Add(_chkAutoSend);
            Controls.Add(_chkAutoPoll);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogPolling";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Polling Intervals";
            Load += new EventHandler(Properties_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private CheckBox _chkAutoPoll;

        internal CheckBox chkAutoPoll
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkAutoPoll;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkAutoPoll != null)
                {
                    _chkAutoPoll.CheckedChanged -= chkAutoPoll_CheckedChanged;
                }

                _chkAutoPoll = value;
                if (_chkAutoPoll != null)
                {
                    _chkAutoPoll.CheckedChanged += chkAutoPoll_CheckedChanged;
                }
            }
        }

        private CheckBox _chkAutoSend;

        internal CheckBox chkAutoSend
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkAutoSend;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkAutoSend != null)
                {
                }

                _chkAutoSend = value;
                if (_chkAutoSend != null)
                {
                }
            }
        }

        private TextBox _txtInterval;

        internal TextBox txtInterval
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtInterval;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtInterval != null)
                {
                }

                _txtInterval = value;
                if (_txtInterval != null)
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

        private Button _btnCancel;

        internal Button btnCancel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCancel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCancel != null)
                {
                    _btnCancel.Click -= btnCancel_Click;
                }

                _btnCancel = value;
                if (_btnCancel != null)
                {
                    _btnCancel.Click += btnCancel_Click;
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