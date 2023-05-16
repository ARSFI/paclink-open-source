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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPactorConnect));
            this._cmbCallSigns = new System.Windows.Forms.ComboBox();
            this._cmbFrequencies = new System.Windows.Forms.ComboBox();
            this._Label1 = new System.Windows.Forms.Label();
            this._Label2 = new System.Windows.Forms.Label();
            this._lblUSB = new System.Windows.Forms.Label();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._lblBusy = new System.Windows.Forms.Label();
            this._Label4 = new System.Windows.Forms.Label();
            this._btnConnect = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnHelp = new System.Windows.Forms.Button();
            this._tmrPollClient = new System.Windows.Forms.Timer(this.components);
            this._lblPMBOType = new System.Windows.Forms.Label();
            this._chkResumeDialog = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _cmbCallSigns
            // 
            this._cmbCallSigns.FormattingEnabled = true;
            this._cmbCallSigns.Location = new System.Drawing.Point(79, 102);
            this._cmbCallSigns.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbCallSigns.Name = "_cmbCallSigns";
            this._cmbCallSigns.Size = new System.Drawing.Size(116, 23);
            this._cmbCallSigns.TabIndex = 2;
            this._ToolTip1.SetToolTip(this._cmbCallSigns, "You can type in a specific call or select one from the PMBO/RMS Type of freq list" +
        "");
            this._cmbCallSigns.SelectedIndexChanged += new System.EventHandler(this.cmbCallSigns_SelectedIndexChanged);
            // 
            // _cmbFrequencies
            // 
            this._cmbFrequencies.FormattingEnabled = true;
            this._cmbFrequencies.Location = new System.Drawing.Point(232, 102);
            this._cmbFrequencies.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbFrequencies.Name = "_cmbFrequencies";
            this._cmbFrequencies.Size = new System.Drawing.Size(172, 23);
            this._cmbFrequencies.TabIndex = 3;
            this._ToolTip1.SetToolTip(this._cmbFrequencies, "This is a list of the center frequencies for the selected remote callsign. ...Or " +
        "you can enter one directly (KHz).");
            this._cmbFrequencies.SelectedIndexChanged += new System.EventHandler(this.cmbFrequencies_SelectedIndexChanged);
            this._cmbFrequencies.TextChanged += new System.EventHandler(this.cmbFrequencies_TextChanged);
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(79, 83);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(93, 15);
            this._Label1.TabIndex = 6;
            this._Label1.Text = "Remote Callsign";
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(232, 83);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(115, 15);
            this._Label2.TabIndex = 7;
            this._Label2.Text = "RF Center Freq (kHz)";
            // 
            // _lblUSB
            // 
            this._lblUSB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._lblUSB.Location = new System.Drawing.Point(33, 141);
            this._lblUSB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblUSB.Name = "_lblUSB";
            this._lblUSB.Size = new System.Drawing.Size(419, 18);
            this._lblUSB.TabIndex = 9;
            this._lblUSB.Text = "USB Dial: --------";
            this._lblUSB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._ToolTip1.SetToolTip(this._lblUSB, "Calculated Dial freq for USB operation.");
            // 
            // _lblBusy
            // 
            this._lblBusy.BackColor = System.Drawing.Color.Yellow;
            this._lblBusy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblBusy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._lblBusy.Location = new System.Drawing.Point(52, 10);
            this._lblBusy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblBusy.Name = "_lblBusy";
            this._lblBusy.Size = new System.Drawing.Size(378, 27);
            this._lblBusy.TabIndex = 15;
            this._lblBusy.Text = "Channel  Status";
            this._lblBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._ToolTip1.SetToolTip(this._lblBusy, "A guide to detect activity in the channel. (Green = channel clear, Yellow = waiti" +
        "ng for status, Red = channel busy)...PTC II models only!");
            // 
            // _Label4
            // 
            this._Label4.AutoSize = true;
            this._Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label4.Location = new System.Drawing.Point(36, 171);
            this._Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label4.Name = "_Label4";
            this._Label4.Size = new System.Drawing.Size(352, 20);
            this._Label4.TabIndex = 16;
            this._Label4.Text = "Listen for clear channel before connecting!";
            // 
            // _btnConnect
            // 
            this._btnConnect.Location = new System.Drawing.Point(38, 247);
            this._btnConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.Size = new System.Drawing.Size(117, 33);
            this._btnConnect.TabIndex = 17;
            this._btnConnect.Text = "C&onnect";
            this._btnConnect.UseVisualStyleBackColor = true;
            this._btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(183, 247);
            this._btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(117, 33);
            this._btnCancel.TabIndex = 18;
            this._btnCancel.Text = "&Close";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _btnHelp
            // 
            this._btnHelp.Location = new System.Drawing.Point(328, 247);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(117, 33);
            this._btnHelp.TabIndex = 19;
            this._btnHelp.Text = "&Help";
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // _tmrPollClient
            // 
            this._tmrPollClient.Enabled = true;
            this._tmrPollClient.Interval = 300;
            this._tmrPollClient.Tick += new System.EventHandler(this.tmrPollClient_Tick);
            // 
            // _lblPMBOType
            // 
            this._lblPMBOType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lblPMBOType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._lblPMBOType.Location = new System.Drawing.Point(107, 50);
            this._lblPMBOType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblPMBOType.Name = "_lblPMBOType";
            this._lblPMBOType.Size = new System.Drawing.Size(270, 25);
            this._lblPMBOType.TabIndex = 20;
            this._lblPMBOType.Text = "Public HF RMS Pactor";
            this._lblPMBOType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _chkResumeDialog
            // 
            this._chkResumeDialog.AutoSize = true;
            this._chkResumeDialog.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkResumeDialog.Location = new System.Drawing.Point(124, 210);
            this._chkResumeDialog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkResumeDialog.Name = "_chkResumeDialog";
            this._chkResumeDialog.Size = new System.Drawing.Size(226, 19);
            this._chkResumeDialog.TabIndex = 21;
            this._chkResumeDialog.Text = "Return to this dialog after connection:";
            this._chkResumeDialog.UseVisualStyleBackColor = true;
            this._chkResumeDialog.CheckedChanged += new System.EventHandler(this.chkResumeDialog_CheckedChanged);
            // 
            // DialogPactorConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(483, 310);
            this.ControlBox = false;
            this.Controls.Add(this._chkResumeDialog);
            this.Controls.Add(this._lblPMBOType);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnConnect);
            this.Controls.Add(this._Label4);
            this.Controls.Add(this._lblBusy);
            this.Controls.Add(this._lblUSB);
            this.Controls.Add(this._Label2);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._cmbFrequencies);
            this.Controls.Add(this._cmbCallSigns);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogPactorConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pactor Connect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogPactorConnect_FormClosing);
            this.Load += new System.EventHandler(this.PactorConnect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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