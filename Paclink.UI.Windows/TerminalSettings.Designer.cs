using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public partial class TerminalSettings : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalSettings));
            this._TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._OK_Button = new System.Windows.Forms.Button();
            this._Cancel_Button = new System.Windows.Forms.Button();
            this._Label1 = new System.Windows.Forms.Label();
            this._Label2 = new System.Windows.Forms.Label();
            this._Label3 = new System.Windows.Forms.Label();
            this._Label4 = new System.Windows.Forms.Label();
            this._Label5 = new System.Windows.Forms.Label();
            this._Label6 = new System.Windows.Forms.Label();
            this._chkRTSEnable = new System.Windows.Forms.CheckBox();
            this._chkDTREnable = new System.Windows.Forms.CheckBox();
            this._chkLocalEcho = new System.Windows.Forms.CheckBox();
            this._cmbHandshake = new System.Windows.Forms.ComboBox();
            this._cmbPort = new System.Windows.Forms.ComboBox();
            this._cmbDataBits = new System.Windows.Forms.ComboBox();
            this._cmbStopBits = new System.Windows.Forms.ComboBox();
            this._cmbParity = new System.Windows.Forms.ComboBox();
            this._txtWriteTimeout = new System.Windows.Forms.TextBox();
            this._cmbBaudRate = new System.Windows.Forms.ComboBox();
            this._Label7 = new System.Windows.Forms.Label();
            this._rdoSendLine = new System.Windows.Forms.RadioButton();
            this._rdoSendWord = new System.Windows.Forms.RadioButton();
            this._rdoSendCharacter = new System.Windows.Forms.RadioButton();
            this._chkWordWrap = new System.Windows.Forms.CheckBox();
            this._TableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _TableLayoutPanel1
            // 
            this._TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._TableLayoutPanel1.ColumnCount = 2;
            this._TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._TableLayoutPanel1.Controls.Add(this._OK_Button, 0, 0);
            this._TableLayoutPanel1.Controls.Add(this._Cancel_Button, 1, 0);
            this._TableLayoutPanel1.Location = new System.Drawing.Point(261, 247);
            this._TableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._TableLayoutPanel1.Name = "_TableLayoutPanel1";
            this._TableLayoutPanel1.RowCount = 1;
            this._TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._TableLayoutPanel1.Size = new System.Drawing.Size(170, 33);
            this._TableLayoutPanel1.TabIndex = 0;
            // 
            // _OK_Button
            // 
            this._OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._OK_Button.Location = new System.Drawing.Point(4, 3);
            this._OK_Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._OK_Button.Name = "_OK_Button";
            this._OK_Button.Size = new System.Drawing.Size(77, 27);
            this._OK_Button.TabIndex = 0;
            this._OK_Button.Text = "OK";
            this._OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // _Cancel_Button
            // 
            this._Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._Cancel_Button.Location = new System.Drawing.Point(89, 3);
            this._Cancel_Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._Cancel_Button.Name = "_Cancel_Button";
            this._Cancel_Button.Size = new System.Drawing.Size(77, 27);
            this._Cancel_Button.TabIndex = 1;
            this._Cancel_Button.Text = "Cancel";
            this._Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(80, 20);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(32, 15);
            this._Label1.TabIndex = 1;
            this._Label1.Text = "Port:";
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(54, 98);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(56, 15);
            this._Label2.TabIndex = 2;
            this._Label2.Text = "Data Bits:";
            // 
            // _Label3
            // 
            this._Label3.AutoSize = true;
            this._Label3.Location = new System.Drawing.Point(54, 137);
            this._Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label3.Name = "_Label3";
            this._Label3.Size = new System.Drawing.Size(56, 15);
            this._Label3.TabIndex = 3;
            this._Label3.Text = "Stop Bits:";
            // 
            // _Label4
            // 
            this._Label4.AutoSize = true;
            this._Label4.Location = new System.Drawing.Point(72, 177);
            this._Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label4.Name = "_Label4";
            this._Label4.Size = new System.Drawing.Size(40, 15);
            this._Label4.TabIndex = 4;
            this._Label4.Text = "Parity:";
            // 
            // _Label5
            // 
            this._Label5.AutoSize = true;
            this._Label5.Location = new System.Drawing.Point(38, 216);
            this._Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label5.Name = "_Label5";
            this._Label5.Size = new System.Drawing.Size(69, 15);
            this._Label5.TabIndex = 5;
            this._Label5.Text = "Handshake:";
            // 
            // _Label6
            // 
            this._Label6.AutoSize = true;
            this._Label6.Location = new System.Drawing.Point(27, 254);
            this._Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label6.Name = "_Label6";
            this._Label6.Size = new System.Drawing.Size(85, 15);
            this._Label6.TabIndex = 6;
            this._Label6.Text = "Write Timeout:";
            // 
            // _chkRTSEnable
            // 
            this._chkRTSEnable.AutoSize = true;
            this._chkRTSEnable.Checked = true;
            this._chkRTSEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkRTSEnable.Location = new System.Drawing.Point(287, 28);
            this._chkRTSEnable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkRTSEnable.Name = "_chkRTSEnable";
            this._chkRTSEnable.Size = new System.Drawing.Size(82, 19);
            this._chkRTSEnable.TabIndex = 7;
            this._chkRTSEnable.Text = "RTS Enable";
            this._chkRTSEnable.UseVisualStyleBackColor = true;
            // 
            // _chkDTREnable
            // 
            this._chkDTREnable.AutoSize = true;
            this._chkDTREnable.Checked = true;
            this._chkDTREnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkDTREnable.Location = new System.Drawing.Point(287, 54);
            this._chkDTREnable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkDTREnable.Name = "_chkDTREnable";
            this._chkDTREnable.Size = new System.Drawing.Size(84, 19);
            this._chkDTREnable.TabIndex = 8;
            this._chkDTREnable.Text = "DTR Enable";
            this._chkDTREnable.UseVisualStyleBackColor = true;
            // 
            // _chkLocalEcho
            // 
            this._chkLocalEcho.AutoSize = true;
            this._chkLocalEcho.Checked = true;
            this._chkLocalEcho.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkLocalEcho.Location = new System.Drawing.Point(287, 81);
            this._chkLocalEcho.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkLocalEcho.Name = "_chkLocalEcho";
            this._chkLocalEcho.Size = new System.Drawing.Size(83, 19);
            this._chkLocalEcho.TabIndex = 9;
            this._chkLocalEcho.Text = "Local Echo";
            this._chkLocalEcho.UseVisualStyleBackColor = true;
            // 
            // _cmbHandshake
            // 
            this._cmbHandshake.FormattingEnabled = true;
            this._cmbHandshake.Location = new System.Drawing.Point(122, 212);
            this._cmbHandshake.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbHandshake.Name = "_cmbHandshake";
            this._cmbHandshake.Size = new System.Drawing.Size(109, 23);
            this._cmbHandshake.TabIndex = 10;
            // 
            // _cmbPort
            // 
            this._cmbPort.FormattingEnabled = true;
            this._cmbPort.Location = new System.Drawing.Point(122, 16);
            this._cmbPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbPort.Name = "_cmbPort";
            this._cmbPort.Size = new System.Drawing.Size(109, 23);
            this._cmbPort.TabIndex = 11;
            // 
            // _cmbDataBits
            // 
            this._cmbDataBits.FormattingEnabled = true;
            this._cmbDataBits.Location = new System.Drawing.Point(122, 95);
            this._cmbDataBits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbDataBits.Name = "_cmbDataBits";
            this._cmbDataBits.Size = new System.Drawing.Size(109, 23);
            this._cmbDataBits.TabIndex = 12;
            // 
            // _cmbStopBits
            // 
            this._cmbStopBits.FormattingEnabled = true;
            this._cmbStopBits.Location = new System.Drawing.Point(122, 134);
            this._cmbStopBits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbStopBits.Name = "_cmbStopBits";
            this._cmbStopBits.Size = new System.Drawing.Size(109, 23);
            this._cmbStopBits.TabIndex = 13;
            // 
            // _cmbParity
            // 
            this._cmbParity.FormattingEnabled = true;
            this._cmbParity.Location = new System.Drawing.Point(122, 173);
            this._cmbParity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbParity.Name = "_cmbParity";
            this._cmbParity.Size = new System.Drawing.Size(109, 23);
            this._cmbParity.TabIndex = 14;
            // 
            // _txtWriteTimeout
            // 
            this._txtWriteTimeout.Location = new System.Drawing.Point(122, 252);
            this._txtWriteTimeout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtWriteTimeout.Name = "_txtWriteTimeout";
            this._txtWriteTimeout.Size = new System.Drawing.Size(73, 23);
            this._txtWriteTimeout.TabIndex = 15;
            // 
            // _cmbBaudRate
            // 
            this._cmbBaudRate.FormattingEnabled = true;
            this._cmbBaudRate.Location = new System.Drawing.Point(122, 55);
            this._cmbBaudRate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbBaudRate.Name = "_cmbBaudRate";
            this._cmbBaudRate.Size = new System.Drawing.Size(109, 23);
            this._cmbBaudRate.TabIndex = 17;
            // 
            // _Label7
            // 
            this._Label7.AutoSize = true;
            this._Label7.Location = new System.Drawing.Point(44, 55);
            this._Label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label7.Name = "_Label7";
            this._Label7.Size = new System.Drawing.Size(63, 15);
            this._Label7.TabIndex = 16;
            this._Label7.Text = "Baud Rate:";
            // 
            // _rdoSendLine
            // 
            this._rdoSendLine.AutoSize = true;
            this._rdoSendLine.Checked = true;
            this._rdoSendLine.Location = new System.Drawing.Point(287, 156);
            this._rdoSendLine.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoSendLine.Name = "_rdoSendLine";
            this._rdoSendLine.Size = new System.Drawing.Size(76, 19);
            this._rdoSendLine.TabIndex = 18;
            this._rdoSendLine.TabStop = true;
            this._rdoSendLine.Text = "Send Line";
            this._rdoSendLine.UseVisualStyleBackColor = true;
            // 
            // _rdoSendWord
            // 
            this._rdoSendWord.AutoSize = true;
            this._rdoSendWord.Location = new System.Drawing.Point(287, 182);
            this._rdoSendWord.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoSendWord.Name = "_rdoSendWord";
            this._rdoSendWord.Size = new System.Drawing.Size(83, 19);
            this._rdoSendWord.TabIndex = 19;
            this._rdoSendWord.TabStop = true;
            this._rdoSendWord.Text = "Send Word";
            this._rdoSendWord.UseVisualStyleBackColor = true;
            // 
            // _rdoSendCharacter
            // 
            this._rdoSendCharacter.AutoSize = true;
            this._rdoSendCharacter.Location = new System.Drawing.Point(287, 209);
            this._rdoSendCharacter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoSendCharacter.Name = "_rdoSendCharacter";
            this._rdoSendCharacter.Size = new System.Drawing.Size(105, 19);
            this._rdoSendCharacter.TabIndex = 20;
            this._rdoSendCharacter.TabStop = true;
            this._rdoSendCharacter.Text = "Send Character";
            this._rdoSendCharacter.UseVisualStyleBackColor = true;
            // 
            // _chkWordWrap
            // 
            this._chkWordWrap.AutoSize = true;
            this._chkWordWrap.Location = new System.Drawing.Point(287, 107);
            this._chkWordWrap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkWordWrap.Name = "_chkWordWrap";
            this._chkWordWrap.Size = new System.Drawing.Size(86, 19);
            this._chkWordWrap.TabIndex = 21;
            this._chkWordWrap.Text = "Word Wrap";
            this._chkWordWrap.UseVisualStyleBackColor = true;
            // 
            // TerminalSettings
            // 
            this.AcceptButton = this._OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(446, 294);
            this.Controls.Add(this._chkWordWrap);
            this.Controls.Add(this._rdoSendCharacter);
            this.Controls.Add(this._rdoSendWord);
            this.Controls.Add(this._rdoSendLine);
            this.Controls.Add(this._cmbBaudRate);
            this.Controls.Add(this._Label7);
            this.Controls.Add(this._txtWriteTimeout);
            this.Controls.Add(this._cmbParity);
            this.Controls.Add(this._cmbStopBits);
            this.Controls.Add(this._cmbDataBits);
            this.Controls.Add(this._cmbPort);
            this.Controls.Add(this._cmbHandshake);
            this.Controls.Add(this._chkLocalEcho);
            this.Controls.Add(this._chkDTREnable);
            this.Controls.Add(this._chkRTSEnable);
            this.Controls.Add(this._Label6);
            this.Controls.Add(this._Label5);
            this.Controls.Add(this._Label4);
            this.Controls.Add(this._Label3);
            this.Controls.Add(this._Label2);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._TableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TerminalSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Simple Terminal  Settings";
            this.Load += new System.EventHandler(this.Properties_Load);
            this._TableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private TableLayoutPanel _TableLayoutPanel1;

        internal TableLayoutPanel TableLayoutPanel1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TableLayoutPanel1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_TableLayoutPanel1 != null)
                {
                }

                _TableLayoutPanel1 = value;
                if (_TableLayoutPanel1 != null)
                {
                }
            }
        }

        private Button _OK_Button;

        internal Button OK_Button
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _OK_Button;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_OK_Button != null)
                {
                    _OK_Button.Click -= OK_Button_Click;
                }

                _OK_Button = value;
                if (_OK_Button != null)
                {
                    _OK_Button.Click += OK_Button_Click;
                }
            }
        }

        private Button _Cancel_Button;

        internal Button Cancel_Button
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Cancel_Button;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Cancel_Button != null)
                {
                    _Cancel_Button.Click -= Cancel_Button_Click;
                }

                _Cancel_Button = value;
                if (_Cancel_Button != null)
                {
                    _Cancel_Button.Click += Cancel_Button_Click;
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

        private CheckBox _chkRTSEnable;

        internal CheckBox chkRTSEnable
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkRTSEnable;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkRTSEnable != null)
                {
                }

                _chkRTSEnable = value;
                if (_chkRTSEnable != null)
                {
                }
            }
        }

        private CheckBox _chkDTREnable;

        internal CheckBox chkDTREnable
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkDTREnable;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkDTREnable != null)
                {
                }

                _chkDTREnable = value;
                if (_chkDTREnable != null)
                {
                }
            }
        }

        private CheckBox _chkLocalEcho;

        internal CheckBox chkLocalEcho
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkLocalEcho;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkLocalEcho != null)
                {
                }

                _chkLocalEcho = value;
                if (_chkLocalEcho != null)
                {
                }
            }
        }

        private ComboBox _cmbHandshake;

        internal ComboBox cmbHandshake
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbHandshake;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbHandshake != null)
                {
                }

                _cmbHandshake = value;
                if (_cmbHandshake != null)
                {
                }
            }
        }

        private ComboBox _cmbPort;

        internal ComboBox cmbPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbPort != null)
                {
                }

                _cmbPort = value;
                if (_cmbPort != null)
                {
                }
            }
        }

        private ComboBox _cmbDataBits;

        internal ComboBox cmbDataBits
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbDataBits;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbDataBits != null)
                {
                }

                _cmbDataBits = value;
                if (_cmbDataBits != null)
                {
                }
            }
        }

        private ComboBox _cmbStopBits;

        internal ComboBox cmbStopBits
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbStopBits;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbStopBits != null)
                {
                }

                _cmbStopBits = value;
                if (_cmbStopBits != null)
                {
                }
            }
        }

        private ComboBox _cmbParity;

        internal ComboBox cmbParity
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbParity;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbParity != null)
                {
                }

                _cmbParity = value;
                if (_cmbParity != null)
                {
                }
            }
        }

        private TextBox _txtWriteTimeout;

        internal TextBox txtWriteTimeout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtWriteTimeout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtWriteTimeout != null)
                {
                }

                _txtWriteTimeout = value;
                if (_txtWriteTimeout != null)
                {
                }
            }
        }

        private ComboBox _cmbBaudRate;

        internal ComboBox cmbBaudRate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbBaudRate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbBaudRate != null)
                {
                }

                _cmbBaudRate = value;
                if (_cmbBaudRate != null)
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

        private RadioButton _rdoSendLine;

        internal RadioButton rdoSendLine
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoSendLine;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoSendLine != null)
                {
                }

                _rdoSendLine = value;
                if (_rdoSendLine != null)
                {
                }
            }
        }

        private RadioButton _rdoSendWord;

        internal RadioButton rdoSendWord
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoSendWord;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoSendWord != null)
                {
                }

                _rdoSendWord = value;
                if (_rdoSendWord != null)
                {
                }
            }
        }

        private RadioButton _rdoSendCharacter;

        internal RadioButton rdoSendCharacter
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoSendCharacter;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoSendCharacter != null)
                {
                }

                _rdoSendCharacter = value;
                if (_rdoSendCharacter != null)
                {
                }
            }
        }

        private CheckBox _chkWordWrap;

        internal CheckBox chkWordWrap
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkWordWrap;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkWordWrap != null)
                {
                }

                _chkWordWrap = value;
                if (_chkWordWrap != null)
                {
                }
            }
        }
    }
}