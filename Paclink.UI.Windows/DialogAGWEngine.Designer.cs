using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public partial class DialogAGWEngine : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogAGWEngine));
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._rdoNotUsed = new System.Windows.Forms.RadioButton();
            this._rdoLocal = new System.Windows.Forms.RadioButton();
            this._rdoRemote = new System.Windows.Forms.RadioButton();
            this._Label1 = new System.Windows.Forms.Label();
            this._txtAGWPath = new System.Windows.Forms.TextBox();
            this._Label2 = new System.Windows.Forms.Label();
            this._Label3 = new System.Windows.Forms.Label();
            this._Label4 = new System.Windows.Forms.Label();
            this._Label5 = new System.Windows.Forms.Label();
            this._txtAGWPort = new System.Windows.Forms.TextBox();
            this._txtAGWHost = new System.Windows.Forms.TextBox();
            this._txtAGWUserId = new System.Windows.Forms.TextBox();
            this._txtAGWPassword = new System.Windows.Forms.TextBox();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._btnRemote = new System.Windows.Forms.Button();
            this._btnBrowse = new System.Windows.Forms.Button();
            this._btnHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(491, 130);
            this._btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(125, 32);
            this._btnCancel.TabIndex = 9;
            this._btnCancel.Text = "Close";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Location = new System.Drawing.Point(491, 88);
            this._btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size(125, 32);
            this._btnUpdate.TabIndex = 8;
            this._btnUpdate.Text = "Update";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // _rdoNotUsed
            // 
            this._rdoNotUsed.AutoSize = true;
            this._rdoNotUsed.Location = new System.Drawing.Point(51, 27);
            this._rdoNotUsed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoNotUsed.Name = "_rdoNotUsed";
            this._rdoNotUsed.Size = new System.Drawing.Size(74, 19);
            this._rdoNotUsed.TabIndex = 0;
            this._rdoNotUsed.TabStop = true;
            this._rdoNotUsed.Text = "Not Used";
            this._ToolTip1.SetToolTip(this._rdoNotUsed, "Select to not use AGWPE");
            this._rdoNotUsed.UseVisualStyleBackColor = true;
            this._rdoNotUsed.CheckedChanged += new System.EventHandler(this.rdoNotUsed_CheckedChanged);
            // 
            // _rdoLocal
            // 
            this._rdoLocal.AutoSize = true;
            this._rdoLocal.Location = new System.Drawing.Point(162, 27);
            this._rdoLocal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoLocal.Name = "_rdoLocal";
            this._rdoLocal.Size = new System.Drawing.Size(102, 19);
            this._rdoLocal.TabIndex = 1;
            this._rdoLocal.TabStop = true;
            this._rdoLocal.Text = "Local Machine";
            this._ToolTip1.SetToolTip(this._rdoLocal, "Select if AGWPE is on THIS computer");
            this._rdoLocal.UseVisualStyleBackColor = true;
            this._rdoLocal.CheckedChanged += new System.EventHandler(this.rdoLocal_CheckedChanged);
            // 
            // _rdoRemote
            // 
            this._rdoRemote.AutoSize = true;
            this._rdoRemote.Location = new System.Drawing.Point(292, 27);
            this._rdoRemote.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoRemote.Name = "_rdoRemote";
            this._rdoRemote.Size = new System.Drawing.Size(115, 19);
            this._rdoRemote.TabIndex = 2;
            this._rdoRemote.TabStop = true;
            this._rdoRemote.Text = "Remote Machine";
            this._ToolTip1.SetToolTip(this._rdoRemote, "Select if AGWPE on a remote computer");
            this._rdoRemote.UseVisualStyleBackColor = true;
            this._rdoRemote.CheckedChanged += new System.EventHandler(this.rdoRemote_CheckedChanged);
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(24, 57);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(172, 15);
            this._Label1.TabIndex = 11;
            this._Label1.Text = "Path to the AGW Packet Engine";
            // 
            // _txtAGWPath
            // 
            this._txtAGWPath.Location = new System.Drawing.Point(28, 75);
            this._txtAGWPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtAGWPath.Name = "_txtAGWPath";
            this._txtAGWPath.Size = new System.Drawing.Size(363, 23);
            this._txtAGWPath.TabIndex = 3;
            this._txtAGWPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtAGWPath, "Path to the AGWPE Exe file. (AGW Packet Engine.exe  or Packet Engine Pro.exe) ");
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(65, 138);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(62, 15);
            this._Label2.TabIndex = 13;
            this._Label2.Text = "AGW Port:";
            // 
            // _Label3
            // 
            this._Label3.AutoSize = true;
            this._Label3.Location = new System.Drawing.Point(65, 108);
            this._Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label3.Name = "_Label3";
            this._Label3.Size = new System.Drawing.Size(65, 15);
            this._Label3.TabIndex = 14;
            this._Label3.Text = "AGW Host:";
            // 
            // _Label4
            // 
            this._Label4.AutoSize = true;
            this._Label4.Location = new System.Drawing.Point(48, 168);
            this._Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label4.Name = "_Label4";
            this._Label4.Size = new System.Drawing.Size(76, 15);
            this._Label4.TabIndex = 15;
            this._Label4.Text = "AGW User Id:";
            // 
            // _Label5
            // 
            this._Label5.AutoSize = true;
            this._Label5.Location = new System.Drawing.Point(34, 198);
            this._Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label5.Name = "_Label5";
            this._Label5.Size = new System.Drawing.Size(90, 15);
            this._Label5.TabIndex = 16;
            this._Label5.Text = "AGW Password:";
            // 
            // _txtAGWPort
            // 
            this._txtAGWPort.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtAGWPort.Location = new System.Drawing.Point(140, 135);
            this._txtAGWPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtAGWPort.Name = "_txtAGWPort";
            this._txtAGWPort.Size = new System.Drawing.Size(182, 23);
            this._txtAGWPort.TabIndex = 5;
            this._txtAGWPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtAGWPort, "Port on which AGWPE is listening (default for normal AGWPE installation is 8000)");
            // 
            // _txtAGWHost
            // 
            this._txtAGWHost.Location = new System.Drawing.Point(140, 105);
            this._txtAGWHost.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtAGWHost.Name = "_txtAGWHost";
            this._txtAGWHost.Size = new System.Drawing.Size(182, 23);
            this._txtAGWHost.TabIndex = 4;
            this._txtAGWHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtAGWHost, "Friendly or dotted IP address for the AGWPE computer (\"localhost or 127.0.0.1 for" +
        " this compuer) ");
            // 
            // _txtAGWUserId
            // 
            this._txtAGWUserId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtAGWUserId.Location = new System.Drawing.Point(140, 165);
            this._txtAGWUserId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtAGWUserId.Name = "_txtAGWUserId";
            this._txtAGWUserId.Size = new System.Drawing.Size(182, 23);
            this._txtAGWUserId.TabIndex = 6;
            this._txtAGWUserId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtAGWUserId, "AGW User ID (needed only if secure login set for AGWPE) ");
            // 
            // _txtAGWPassword
            // 
            this._txtAGWPassword.Location = new System.Drawing.Point(140, 195);
            this._txtAGWPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtAGWPassword.Name = "_txtAGWPassword";
            this._txtAGWPassword.Size = new System.Drawing.Size(182, 23);
            this._txtAGWPassword.TabIndex = 7;
            this._txtAGWPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtAGWPassword, "AGW Password (needed only if secure login set for AGWPE) ");
            // 
            // _btnRemote
            // 
            this._btnRemote.Location = new System.Drawing.Point(491, 48);
            this._btnRemote.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnRemote.Name = "_btnRemote";
            this._btnRemote.Size = new System.Drawing.Size(125, 32);
            this._btnRemote.TabIndex = 18;
            this._btnRemote.Text = "Test Remote Login";
            this._ToolTip1.SetToolTip(this._btnRemote, "Check remote login. Yellow = In process, Green = Login OK, Red = Fail or 10 secon" +
        "d timeout");
            this._btnRemote.UseVisualStyleBackColor = true;
            this._btnRemote.Click += new System.EventHandler(this.btnRemote_Click);
            // 
            // _btnBrowse
            // 
            this._btnBrowse.Location = new System.Drawing.Point(399, 73);
            this._btnBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnBrowse.Name = "_btnBrowse";
            this._btnBrowse.Size = new System.Drawing.Size(58, 25);
            this._btnBrowse.TabIndex = 17;
            this._btnBrowse.Text = "Browse";
            this._btnBrowse.UseVisualStyleBackColor = true;
            this._btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // _btnHelp
            // 
            this._btnHelp.Location = new System.Drawing.Point(491, 170);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(125, 32);
            this._btnHelp.TabIndex = 19;
            this._btnHelp.Text = "Help";
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // DialogAGWEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(642, 249);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._btnRemote);
            this.Controls.Add(this._btnBrowse);
            this.Controls.Add(this._txtAGWPassword);
            this.Controls.Add(this._txtAGWUserId);
            this.Controls.Add(this._txtAGWHost);
            this.Controls.Add(this._txtAGWPort);
            this.Controls.Add(this._Label5);
            this.Controls.Add(this._Label4);
            this.Controls.Add(this._Label3);
            this.Controls.Add(this._Label2);
            this.Controls.Add(this._txtAGWPath);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._rdoRemote);
            this.Controls.Add(this._rdoLocal);
            this.Controls.Add(this._rdoNotUsed);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogAGWEngine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AGW Engine Properties";
            this.Load += new System.EventHandler(this.AGWEngine_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private RadioButton _rdoNotUsed;

        internal RadioButton rdoNotUsed
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoNotUsed;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoNotUsed != null)
                {
                    _rdoNotUsed.CheckedChanged -= rdoNotUsed_CheckedChanged;
                }

                _rdoNotUsed = value;
                if (_rdoNotUsed != null)
                {
                    _rdoNotUsed.CheckedChanged += rdoNotUsed_CheckedChanged;
                }
            }
        }

        private RadioButton _rdoLocal;

        internal RadioButton rdoLocal
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoLocal;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoLocal != null)
                {
                    _rdoLocal.CheckedChanged -= rdoLocal_CheckedChanged;
                }

                _rdoLocal = value;
                if (_rdoLocal != null)
                {
                    _rdoLocal.CheckedChanged += rdoLocal_CheckedChanged;
                }
            }
        }

        private RadioButton _rdoRemote;

        internal RadioButton rdoRemote
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoRemote;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoRemote != null)
                {
                    _rdoRemote.CheckedChanged -= rdoRemote_CheckedChanged;
                }

                _rdoRemote = value;
                if (_rdoRemote != null)
                {
                    _rdoRemote.CheckedChanged += rdoRemote_CheckedChanged;
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

        private TextBox _txtAGWPath;

        internal TextBox txtAGWPath
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtAGWPath;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtAGWPath != null)
                {
                }

                _txtAGWPath = value;
                if (_txtAGWPath != null)
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

        private TextBox _txtAGWPort;

        internal TextBox txtAGWPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtAGWPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtAGWPort != null)
                {
                }

                _txtAGWPort = value;
                if (_txtAGWPort != null)
                {
                }
            }
        }

        private TextBox _txtAGWHost;

        internal TextBox txtAGWHost
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtAGWHost;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtAGWHost != null)
                {
                }

                _txtAGWHost = value;
                if (_txtAGWHost != null)
                {
                }
            }
        }

        private TextBox _txtAGWUserId;

        internal TextBox txtAGWUserId
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtAGWUserId;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtAGWUserId != null)
                {
                }

                _txtAGWUserId = value;
                if (_txtAGWUserId != null)
                {
                }
            }
        }

        private TextBox _txtAGWPassword;

        internal TextBox txtAGWPassword
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtAGWPassword;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtAGWPassword != null)
                {
                }

                _txtAGWPassword = value;
                if (_txtAGWPassword != null)
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

        private Button _btnBrowse;

        internal Button btnBrowse
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnBrowse;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnBrowse != null)
                {
                    _btnBrowse.Click -= btnBrowse_Click;
                }

                _btnBrowse = value;
                if (_btnBrowse != null)
                {
                    _btnBrowse.Click += btnBrowse_Click;
                }
            }
        }

        private Button _btnRemote;

        internal Button btnRemote
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnRemote;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnRemote != null)
                {
                    _btnRemote.Click -= btnRemote_Click;
                }

                _btnRemote = value;
                if (_btnRemote != null)
                {
                    _btnRemote.Click += btnRemote_Click;
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