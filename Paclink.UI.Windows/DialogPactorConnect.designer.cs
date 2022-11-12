using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public partial class DialogPactorConnect : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            if (disposing && components is object)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPactorConnect));
            _cmbCallSigns = new ComboBox();
            _cmbCallSigns.SelectedIndexChanged += new EventHandler(cmbCallSigns_SelectedIndexChanged);
            _cmbFrequencies = new ComboBox();
            _cmbFrequencies.SelectedIndexChanged += new EventHandler(cmbFrequencies_SelectedIndexChanged);
            _cmbFrequencies.TextChanged += new EventHandler(cmbFrequencies_TextChanged);
            _Label1 = new Label();
            _Label2 = new Label();
            _lblUSB = new Label();
            _ToolTip1 = new ToolTip(components);
            _lblBusy = new Label();
            _Label4 = new Label();
            _btnConnect = new Button();
            _btnConnect.Click += new EventHandler(btnConnect_Click);
            _btnCancel = new Button();
            _btnCancel.Click += new EventHandler(btnCancel_Click);
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            _tmrPollClient = new Timer(components);
            _tmrPollClient.Tick += new EventHandler(tmrPollClient_Tick);
            _lblPMBOType = new Label();
            _chkResumeDialog = new CheckBox();
            _chkResumeDialog.CheckedChanged += new EventHandler(chkResumeDialog_CheckedChanged);
            SuspendLayout();
            // 
            // cmbCallSigns
            // 
            _cmbCallSigns.FormattingEnabled = true;
            _cmbCallSigns.Location = new Point(68, 88);
            _cmbCallSigns.Name = "_cmbCallSigns";
            _cmbCallSigns.Size = new Size(100, 21);
            _cmbCallSigns.TabIndex = 2;
            _ToolTip1.SetToolTip(_cmbCallSigns, "You can type in a specific call or select one from the PMBO/RMS Type of freq list" + "");
            // 
            // cmbFrequencies
            // 
            _cmbFrequencies.FormattingEnabled = true;
            _cmbFrequencies.Location = new Point(199, 88);
            _cmbFrequencies.Name = "_cmbFrequencies";
            _cmbFrequencies.Size = new Size(148, 21);
            _cmbFrequencies.TabIndex = 3;
            _ToolTip1.SetToolTip(_cmbFrequencies, "This is a list of the center frequencies for the selected remote callsign. ...Or " + "you can enter one directly (KHz).");
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(68, 72);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(83, 13);
            _Label1.TabIndex = 6;
            _Label1.Text = "Remote Callsign";
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(199, 72);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(107, 13);
            _Label2.TabIndex = 7;
            _Label2.Text = "RF Center Freq (kHz)";
            // 
            // lblUSB
            // 
            _lblUSB.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _lblUSB.Location = new Point(28, 122);
            _lblUSB.Name = "_lblUSB";
            _lblUSB.Size = new Size(359, 16);
            _lblUSB.TabIndex = 9;
            _lblUSB.Text = "USB Dial: --------";
            _lblUSB.TextAlign = ContentAlignment.MiddleCenter;
            _ToolTip1.SetToolTip(_lblUSB, "Calculated Dial freq for USB operation.");
            // 
            // lblBusy
            // 
            _lblBusy.BackColor = Color.Yellow;
            _lblBusy.BorderStyle = BorderStyle.FixedSingle;
            _lblBusy.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _lblBusy.Location = new Point(45, 9);
            _lblBusy.Name = "_lblBusy";
            _lblBusy.Size = new Size(324, 24);
            _lblBusy.TabIndex = 15;
            _lblBusy.Text = "Channel  Status";
            _lblBusy.TextAlign = ContentAlignment.MiddleCenter;
            _ToolTip1.SetToolTip(_lblBusy, "A guide to detect activity in the channel. (Green = channel clear, Yellow = waiti" + "ng for status, Red = channel busy)...PTC II models only!");
            // 
            // Label4
            // 
            _Label4.AutoSize = true;
            _Label4.Font = new Font("Microsoft Sans Serif", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label4.Location = new Point(31, 148);
            _Label4.Name = "_Label4";
            _Label4.Size = new Size(352, 20);
            _Label4.TabIndex = 16;
            _Label4.Text = "Listen for clear channel before connecting!";
            // 
            // btnConnect
            // 
            _btnConnect.Location = new Point(33, 214);
            _btnConnect.Name = "_btnConnect";
            _btnConnect.Size = new Size(100, 29);
            _btnConnect.TabIndex = 17;
            _btnConnect.Text = "C&onnect";
            _btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            _btnCancel.Location = new Point(157, 214);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(100, 29);
            _btnCancel.TabIndex = 18;
            _btnCancel.Text = "&Close";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            _btnHelp.Location = new Point(281, 214);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(100, 29);
            _btnHelp.TabIndex = 19;
            _btnHelp.Text = "&Help";
            _btnHelp.UseVisualStyleBackColor = true;
            // 
            // tmrPollClient
            // 
            _tmrPollClient.Enabled = true;
            _tmrPollClient.Interval = 300;
            // 
            // lblPMBOType
            // 
            _lblPMBOType.BorderStyle = BorderStyle.Fixed3D;
            _lblPMBOType.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _lblPMBOType.Location = new Point(92, 43);
            _lblPMBOType.Name = "_lblPMBOType";
            _lblPMBOType.Size = new Size(231, 22);
            _lblPMBOType.TabIndex = 20;
            _lblPMBOType.Text = "Public HF RMS Pactor";
            _lblPMBOType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // chkResumeDialog
            // 
            _chkResumeDialog.AutoSize = true;
            _chkResumeDialog.CheckAlign = ContentAlignment.MiddleRight;
            _chkResumeDialog.Location = new Point(106, 182);
            _chkResumeDialog.Name = "_chkResumeDialog";
            _chkResumeDialog.Size = new Size(203, 17);
            _chkResumeDialog.TabIndex = 21;
            _chkResumeDialog.Text = "Return to this dialog after connection:";
            _chkResumeDialog.UseVisualStyleBackColor = true;
            // 
            // DialogPactorConnect
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(414, 269);
            ControlBox = false;
            Controls.Add(_chkResumeDialog);
            Controls.Add(_lblPMBOType);
            Controls.Add(_btnHelp);
            Controls.Add(_btnCancel);
            Controls.Add(_btnConnect);
            Controls.Add(_Label4);
            Controls.Add(_lblBusy);
            Controls.Add(_lblUSB);
            Controls.Add(_Label2);
            Controls.Add(_Label1);
            Controls.Add(_cmbFrequencies);
            Controls.Add(_cmbCallSigns);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogPactorConnect";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pactor Connect";
            FormClosing += new FormClosingEventHandler(DialogPactorConnect_FormClosing);
            Load += new EventHandler(PactorConnect_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private ComboBox _cmbCallSigns;

        internal ComboBox cmbCallSigns
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbCallSigns;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbCallSigns != null)
                {
                    _cmbCallSigns.SelectedIndexChanged -= cmbCallSigns_SelectedIndexChanged;
                }

                _cmbCallSigns = value;
                if (_cmbCallSigns != null)
                {
                    _cmbCallSigns.SelectedIndexChanged += cmbCallSigns_SelectedIndexChanged;
                }
            }
        }

        private ComboBox _cmbFrequencies;

        internal ComboBox cmbFrequencies
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbFrequencies;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbFrequencies != null)
                {
                    _cmbFrequencies.SelectedIndexChanged -= cmbFrequencies_SelectedIndexChanged;
                    _cmbFrequencies.TextChanged -= cmbFrequencies_TextChanged;
                }

                _cmbFrequencies = value;
                if (_cmbFrequencies != null)
                {
                    _cmbFrequencies.SelectedIndexChanged += cmbFrequencies_SelectedIndexChanged;
                    _cmbFrequencies.TextChanged += cmbFrequencies_TextChanged;
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

        private Label _lblUSB;

        internal Label lblUSB
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblUSB;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblUSB != null)
                {
                }

                _lblUSB = value;
                if (_lblUSB != null)
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

        private Label _lblBusy;

        internal Label lblBusy
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblBusy;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblBusy != null)
                {
                }

                _lblBusy = value;
                if (_lblBusy != null)
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

        private Button _btnConnect;

        internal Button btnConnect
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnConnect;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnConnect != null)
                {
                    _btnConnect.Click -= btnConnect_Click;
                }

                _btnConnect = value;
                if (_btnConnect != null)
                {
                    _btnConnect.Click += btnConnect_Click;
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

        private Timer _tmrPollClient;

        internal Timer tmrPollClient
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrPollClient;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrPollClient != null)
                {
                    _tmrPollClient.Tick -= tmrPollClient_Tick;
                }

                _tmrPollClient = value;
                if (_tmrPollClient != null)
                {
                    _tmrPollClient.Tick += tmrPollClient_Tick;
                }
            }
        }

        private Label _lblPMBOType;

        internal Label lblPMBOType
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblPMBOType;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblPMBOType != null)
                {
                }

                _lblPMBOType = value;
                if (_lblPMBOType != null)
                {
                }
            }
        }

        private CheckBox _chkResumeDialog;

        internal CheckBox chkResumeDialog
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkResumeDialog;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkResumeDialog != null)
                {
                    _chkResumeDialog.CheckedChanged -= chkResumeDialog_CheckedChanged;
                }

                _chkResumeDialog = value;
                if (_chkResumeDialog != null)
                {
                    _chkResumeDialog.CheckedChanged += chkResumeDialog_CheckedChanged;
                }
            }
        }
    }
}