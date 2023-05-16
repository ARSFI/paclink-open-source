using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPolling));
            this._chkAutoPoll = new System.Windows.Forms.CheckBox();
            this._chkAutoSend = new System.Windows.Forms.CheckBox();
            this._txtInterval = new System.Windows.Forms.TextBox();
            this._Label1 = new System.Windows.Forms.Label();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._btnHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _chkAutoPoll
            // 
            this._chkAutoPoll.AutoSize = true;
            this._chkAutoPoll.Location = new System.Drawing.Point(30, 20);
            this._chkAutoPoll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkAutoPoll.Name = "_chkAutoPoll";
            this._chkAutoPoll.Size = new System.Drawing.Size(154, 19);
            this._chkAutoPoll.TabIndex = 0;
            this._chkAutoPoll.Text = "Automatically poll every";
            this._ToolTip1.SetToolTip(this._chkAutoPoll, "Check to poll on a schedule");
            this._chkAutoPoll.UseVisualStyleBackColor = true;
            this._chkAutoPoll.CheckedChanged += new System.EventHandler(this.chkAutoPoll_CheckedChanged);
            // 
            // _chkAutoSend
            // 
            this._chkAutoSend.AutoSize = true;
            this._chkAutoSend.Location = new System.Drawing.Point(30, 47);
            this._chkAutoSend.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkAutoSend.Name = "_chkAutoSend";
            this._chkAutoSend.Size = new System.Drawing.Size(378, 19);
            this._chkAutoSend.TabIndex = 2;
            this._chkAutoSend.Text = "Automatically send any pending messages without waiting for poll";
            this._ToolTip1.SetToolTip(this._chkAutoSend, "Check to poll immediately upon receiving any pending outbound messages.");
            this._chkAutoSend.UseVisualStyleBackColor = true;
            // 
            // _txtInterval
            // 
            this._txtInterval.Location = new System.Drawing.Point(196, 17);
            this._txtInterval.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtInterval.Name = "_txtInterval";
            this._txtInterval.Size = new System.Drawing.Size(30, 23);
            this._txtInterval.TabIndex = 1;
            this._txtInterval.Text = "60";
            this._txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtInterval, "polling interval (default = 60)");
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(233, 21);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(50, 15);
            this._Label1.TabIndex = 3;
            this._Label1.Text = "minutes";
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Location = new System.Drawing.Point(31, 84);
            this._btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size(125, 32);
            this._btnUpdate.TabIndex = 3;
            this._btnUpdate.Text = "Update";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(163, 84);
            this._btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(125, 32);
            this._btnCancel.TabIndex = 4;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _btnHelp
            // 
            this._btnHelp.Location = new System.Drawing.Point(295, 84);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(125, 32);
            this._btnHelp.TabIndex = 5;
            this._btnHelp.Text = "Help";
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // DialogPolling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(450, 140);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnUpdate);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._txtInterval);
            this.Controls.Add(this._chkAutoSend);
            this.Controls.Add(this._chkAutoPoll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogPolling";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Polling Intervals";
            this.Load += new System.EventHandler(this.Properties_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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