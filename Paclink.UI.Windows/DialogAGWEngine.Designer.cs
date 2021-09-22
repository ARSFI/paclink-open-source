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
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogAGWEngine));
            _btnCancel = new Button();
            _btnCancel.Click += new EventHandler(btnCancel_Click);
            _btnUpdate = new Button();
            _btnUpdate.Click += new EventHandler(btnUpdate_Click);
            _rdoNotUsed = new RadioButton();
            _rdoNotUsed.CheckedChanged += new EventHandler(rdoNotUsed_CheckedChanged);
            _rdoLocal = new RadioButton();
            _rdoLocal.CheckedChanged += new EventHandler(rdoLocal_CheckedChanged);
            _rdoRemote = new RadioButton();
            _rdoRemote.CheckedChanged += new EventHandler(rdoRemote_CheckedChanged);
            _Label1 = new Label();
            _txtAGWPath = new TextBox();
            _Label2 = new Label();
            _Label3 = new Label();
            _Label4 = new Label();
            _Label5 = new Label();
            _txtAGWPort = new TextBox();
            _txtAGWHost = new TextBox();
            _txtAGWUserId = new TextBox();
            _txtAGWPassword = new TextBox();
            _ToolTip1 = new ToolTip(components);
            _btnRemote = new Button();
            _btnRemote.Click += new EventHandler(btnRemote_Click);
            _btnBrowse = new Button();
            _btnBrowse.Click += new EventHandler(btnBrowse_Click);
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            SuspendLayout();
            // 
            // btnCancel
            // 
            _btnCancel.Location = new Point(421, 113);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(107, 28);
            _btnCancel.TabIndex = 9;
            _btnCancel.Text = "Close";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            _btnUpdate.Location = new Point(421, 76);
            _btnUpdate.Name = "_btnUpdate";
            _btnUpdate.Size = new Size(107, 28);
            _btnUpdate.TabIndex = 8;
            _btnUpdate.Text = "Update";
            _btnUpdate.UseVisualStyleBackColor = true;
            // 
            // rdoNotUsed
            // 
            _rdoNotUsed.AutoSize = true;
            _rdoNotUsed.Location = new Point(44, 23);
            _rdoNotUsed.Name = "_rdoNotUsed";
            _rdoNotUsed.Size = new Size(70, 17);
            _rdoNotUsed.TabIndex = 0;
            _rdoNotUsed.TabStop = true;
            _rdoNotUsed.Text = "Not Used";
            _ToolTip1.SetToolTip(_rdoNotUsed, "Select to not use AGWPE");
            _rdoNotUsed.UseVisualStyleBackColor = true;
            // 
            // rdoLocal
            // 
            _rdoLocal.AutoSize = true;
            _rdoLocal.Location = new Point(139, 23);
            _rdoLocal.Name = "_rdoLocal";
            _rdoLocal.Size = new Size(95, 17);
            _rdoLocal.TabIndex = 1;
            _rdoLocal.TabStop = true;
            _rdoLocal.Text = "Local Machine";
            _ToolTip1.SetToolTip(_rdoLocal, "Select if AGWPE is on THIS computer");
            _rdoLocal.UseVisualStyleBackColor = true;
            // 
            // rdoRemote
            // 
            _rdoRemote.AutoSize = true;
            _rdoRemote.Location = new Point(250, 23);
            _rdoRemote.Name = "_rdoRemote";
            _rdoRemote.Size = new Size(106, 17);
            _rdoRemote.TabIndex = 2;
            _rdoRemote.TabStop = true;
            _rdoRemote.Text = "Remote Machine";
            _ToolTip1.SetToolTip(_rdoRemote, "Select if AGWPE on a remote computer");
            _rdoRemote.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(21, 49);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(161, 13);
            _Label1.TabIndex = 11;
            _Label1.Text = "Path to the AGW Packet Engine";
            // 
            // txtAGWPath
            // 
            _txtAGWPath.Location = new Point(24, 65);
            _txtAGWPath.Name = "_txtAGWPath";
            _txtAGWPath.Size = new Size(312, 20);
            _txtAGWPath.TabIndex = 3;
            _txtAGWPath.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtAGWPath, "Path to the AGWPE Exe file. (AGW Packet Engine.exe  or Packet Engine Pro.exe) ");
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(56, 120);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(58, 13);
            _Label2.TabIndex = 13;
            _Label2.Text = "AGW Port:";
            // 
            // Label3
            // 
            _Label3.AutoSize = true;
            _Label3.Location = new Point(56, 94);
            _Label3.Name = "_Label3";
            _Label3.Size = new Size(61, 13);
            _Label3.TabIndex = 14;
            _Label3.Text = "AGW Host:";
            // 
            // Label4
            // 
            _Label4.AutoSize = true;
            _Label4.Location = new Point(41, 146);
            _Label4.Name = "_Label4";
            _Label4.Size = new Size(73, 13);
            _Label4.TabIndex = 15;
            _Label4.Text = "AGW User Id:";
            // 
            // Label5
            // 
            _Label5.AutoSize = true;
            _Label5.Location = new Point(29, 172);
            _Label5.Name = "_Label5";
            _Label5.Size = new Size(85, 13);
            _Label5.TabIndex = 16;
            _Label5.Text = "AGW Password:";
            // 
            // txtAGWPort
            // 
            _txtAGWPort.CharacterCasing = CharacterCasing.Upper;
            _txtAGWPort.Location = new Point(120, 117);
            _txtAGWPort.Name = "_txtAGWPort";
            _txtAGWPort.Size = new Size(157, 20);
            _txtAGWPort.TabIndex = 5;
            _txtAGWPort.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtAGWPort, "Port on which AGWPE is listening (default for normal AGWPE installation is 8000)");
            // 
            // txtAGWHost
            // 
            _txtAGWHost.Location = new Point(120, 91);
            _txtAGWHost.Name = "_txtAGWHost";
            _txtAGWHost.Size = new Size(157, 20);
            _txtAGWHost.TabIndex = 4;
            _txtAGWHost.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtAGWHost, "Friendly or dotted IP address for the AGWPE computer (\"localhost or 127.0.0.1 for" + " this compuer) ");
            // 
            // txtAGWUserId
            // 
            _txtAGWUserId.CharacterCasing = CharacterCasing.Upper;
            _txtAGWUserId.Location = new Point(120, 143);
            _txtAGWUserId.Name = "_txtAGWUserId";
            _txtAGWUserId.Size = new Size(157, 20);
            _txtAGWUserId.TabIndex = 6;
            _txtAGWUserId.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtAGWUserId, "AGW User ID (needed only if secure login set for AGWPE) ");
            // 
            // txtAGWPassword
            // 
            _txtAGWPassword.Location = new Point(120, 169);
            _txtAGWPassword.Name = "_txtAGWPassword";
            _txtAGWPassword.Size = new Size(157, 20);
            _txtAGWPassword.TabIndex = 7;
            _txtAGWPassword.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtAGWPassword, "AGW Password (needed only if secure login set for AGWPE) ");
            // 
            // btnRemote
            // 
            _btnRemote.Location = new Point(421, 42);
            _btnRemote.Name = "_btnRemote";
            _btnRemote.Size = new Size(107, 28);
            _btnRemote.TabIndex = 18;
            _btnRemote.Text = "Test Remote Login";
            _ToolTip1.SetToolTip(_btnRemote, "Check remote login. Yellow = In process, Green = Login OK, Red = Fail or 10 secon" + "d timeout");
            _btnRemote.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            _btnBrowse.Location = new Point(342, 63);
            _btnBrowse.Name = "_btnBrowse";
            _btnBrowse.Size = new Size(50, 22);
            _btnBrowse.TabIndex = 17;
            _btnBrowse.Text = "Browse";
            _btnBrowse.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            _btnHelp.Location = new Point(421, 147);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(107, 28);
            _btnHelp.TabIndex = 19;
            _btnHelp.Text = "Help";
            _btnHelp.UseVisualStyleBackColor = true;
            // 
            // DialogAGWEngine
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 216);
            Controls.Add(_btnHelp);
            Controls.Add(_btnRemote);
            Controls.Add(_btnBrowse);
            Controls.Add(_txtAGWPassword);
            Controls.Add(_txtAGWUserId);
            Controls.Add(_txtAGWHost);
            Controls.Add(_txtAGWPort);
            Controls.Add(_Label5);
            Controls.Add(_Label4);
            Controls.Add(_Label3);
            Controls.Add(_Label2);
            Controls.Add(_txtAGWPath);
            Controls.Add(_Label1);
            Controls.Add(_rdoRemote);
            Controls.Add(_rdoLocal);
            Controls.Add(_rdoNotUsed);
            Controls.Add(_btnCancel);
            Controls.Add(_btnUpdate);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogAGWEngine";
            StartPosition = FormStartPosition.CenterParent;
            Text = "AGW Engine Properties";
            Load += new EventHandler(AGWEngine_Load);
            ResumeLayout(false);
            PerformLayout();
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