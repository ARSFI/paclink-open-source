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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalSettings));
            _TableLayoutPanel1 = new TableLayoutPanel();
            _OK_Button = new Button();
            _OK_Button.Click += new EventHandler(OK_Button_Click);
            _Cancel_Button = new Button();
            _Cancel_Button.Click += new EventHandler(Cancel_Button_Click);
            _Label1 = new Label();
            _Label2 = new Label();
            _Label3 = new Label();
            _Label4 = new Label();
            _Label5 = new Label();
            _Label6 = new Label();
            _chkRTSEnable = new CheckBox();
            _chkDTREnable = new CheckBox();
            _chkLocalEcho = new CheckBox();
            _cmbHandshake = new ComboBox();
            _cmbPort = new ComboBox();
            _cmbDataBits = new ComboBox();
            _cmbStopBits = new ComboBox();
            _cmbParity = new ComboBox();
            _txtWriteTimeout = new TextBox();
            _cmbBaudRate = new ComboBox();
            _Label7 = new Label();
            _rdoSendLine = new RadioButton();
            _rdoSendWord = new RadioButton();
            _rdoSendCharacter = new RadioButton();
            _chkWordWrap = new CheckBox();
            _TableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // TableLayoutPanel1
            // 
            _TableLayoutPanel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _TableLayoutPanel1.ColumnCount = 2;
            _TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0F));
            _TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0F));
            _TableLayoutPanel1.Controls.Add(_OK_Button, 0, 0);
            _TableLayoutPanel1.Controls.Add(_Cancel_Button, 1, 0);
            _TableLayoutPanel1.Location = new Point(224, 214);
            _TableLayoutPanel1.Name = "_TableLayoutPanel1";
            _TableLayoutPanel1.RowCount = 1;
            _TableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50.0F));
            _TableLayoutPanel1.Size = new Size(146, 29);
            _TableLayoutPanel1.TabIndex = 0;
            // 
            // OK_Button
            // 
            _OK_Button.Anchor = AnchorStyles.None;
            _OK_Button.Location = new Point(3, 3);
            _OK_Button.Name = "_OK_Button";
            _OK_Button.Size = new Size(67, 23);
            _OK_Button.TabIndex = 0;
            _OK_Button.Text = "OK";
            // 
            // Cancel_Button
            // 
            _Cancel_Button.Anchor = AnchorStyles.None;
            _Cancel_Button.DialogResult = DialogResult.Cancel;
            _Cancel_Button.Location = new Point(76, 3);
            _Cancel_Button.Name = "_Cancel_Button";
            _Cancel_Button.Size = new Size(67, 23);
            _Cancel_Button.TabIndex = 1;
            _Cancel_Button.Text = "Cancel";
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(69, 17);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(29, 13);
            _Label1.TabIndex = 1;
            _Label1.Text = "Port:";
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(46, 85);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(53, 13);
            _Label2.TabIndex = 2;
            _Label2.Text = "Data Bits:";
            // 
            // Label3
            // 
            _Label3.AutoSize = true;
            _Label3.Location = new Point(46, 119);
            _Label3.Name = "_Label3";
            _Label3.Size = new Size(52, 13);
            _Label3.TabIndex = 3;
            _Label3.Text = "Stop Bits:";
            // 
            // Label4
            // 
            _Label4.AutoSize = true;
            _Label4.Location = new Point(62, 153);
            _Label4.Name = "_Label4";
            _Label4.Size = new Size(36, 13);
            _Label4.TabIndex = 4;
            _Label4.Text = "Parity:";
            // 
            // Label5
            // 
            _Label5.AutoSize = true;
            _Label5.Location = new Point(33, 187);
            _Label5.Name = "_Label5";
            _Label5.Size = new Size(65, 13);
            _Label5.TabIndex = 5;
            _Label5.Text = "Handshake:";
            // 
            // Label6
            // 
            _Label6.AutoSize = true;
            _Label6.Location = new Point(23, 220);
            _Label6.Name = "_Label6";
            _Label6.Size = new Size(76, 13);
            _Label6.TabIndex = 6;
            _Label6.Text = "Write Timeout:";
            // 
            // chkRTSEnable
            // 
            _chkRTSEnable.AutoSize = true;
            _chkRTSEnable.Checked = true;
            _chkRTSEnable.CheckState = CheckState.Checked;
            _chkRTSEnable.Location = new Point(246, 24);
            _chkRTSEnable.Name = "_chkRTSEnable";
            _chkRTSEnable.Size = new Size(84, 17);
            _chkRTSEnable.TabIndex = 7;
            _chkRTSEnable.Text = "RTS Enable";
            _chkRTSEnable.UseVisualStyleBackColor = true;
            // 
            // chkDTREnable
            // 
            _chkDTREnable.AutoSize = true;
            _chkDTREnable.Checked = true;
            _chkDTREnable.CheckState = CheckState.Checked;
            _chkDTREnable.Location = new Point(246, 47);
            _chkDTREnable.Name = "_chkDTREnable";
            _chkDTREnable.Size = new Size(85, 17);
            _chkDTREnable.TabIndex = 8;
            _chkDTREnable.Text = "DTR Enable";
            _chkDTREnable.UseVisualStyleBackColor = true;
            // 
            // chkLocalEcho
            // 
            _chkLocalEcho.AutoSize = true;
            _chkLocalEcho.Checked = true;
            _chkLocalEcho.CheckState = CheckState.Checked;
            _chkLocalEcho.Location = new Point(246, 70);
            _chkLocalEcho.Name = "_chkLocalEcho";
            _chkLocalEcho.Size = new Size(80, 17);
            _chkLocalEcho.TabIndex = 9;
            _chkLocalEcho.Text = "Local Echo";
            _chkLocalEcho.UseVisualStyleBackColor = true;
            // 
            // cmbHandshake
            // 
            _cmbHandshake.FormattingEnabled = true;
            _cmbHandshake.Location = new Point(105, 184);
            _cmbHandshake.Name = "_cmbHandshake";
            _cmbHandshake.Size = new Size(94, 21);
            _cmbHandshake.TabIndex = 10;
            // 
            // cmbPort
            // 
            _cmbPort.FormattingEnabled = true;
            _cmbPort.Location = new Point(105, 14);
            _cmbPort.Name = "_cmbPort";
            _cmbPort.Size = new Size(94, 21);
            _cmbPort.TabIndex = 11;
            // 
            // cmbDataBits
            // 
            _cmbDataBits.FormattingEnabled = true;
            _cmbDataBits.Location = new Point(105, 82);
            _cmbDataBits.Name = "_cmbDataBits";
            _cmbDataBits.Size = new Size(94, 21);
            _cmbDataBits.TabIndex = 12;
            // 
            // cmbStopBits
            // 
            _cmbStopBits.FormattingEnabled = true;
            _cmbStopBits.Location = new Point(105, 116);
            _cmbStopBits.Name = "_cmbStopBits";
            _cmbStopBits.Size = new Size(94, 21);
            _cmbStopBits.TabIndex = 13;
            // 
            // cmbParity
            // 
            _cmbParity.FormattingEnabled = true;
            _cmbParity.Location = new Point(105, 150);
            _cmbParity.Name = "_cmbParity";
            _cmbParity.Size = new Size(94, 21);
            _cmbParity.TabIndex = 14;
            // 
            // txtWriteTimeout
            // 
            _txtWriteTimeout.Location = new Point(105, 218);
            _txtWriteTimeout.Name = "_txtWriteTimeout";
            _txtWriteTimeout.Size = new Size(63, 20);
            _txtWriteTimeout.TabIndex = 15;
            // 
            // cmbBaudRate
            // 
            _cmbBaudRate.FormattingEnabled = true;
            _cmbBaudRate.Location = new Point(105, 48);
            _cmbBaudRate.Name = "_cmbBaudRate";
            _cmbBaudRate.Size = new Size(94, 21);
            _cmbBaudRate.TabIndex = 17;
            // 
            // Label7
            // 
            _Label7.AutoSize = true;
            _Label7.Location = new Point(38, 48);
            _Label7.Name = "_Label7";
            _Label7.Size = new Size(61, 13);
            _Label7.TabIndex = 16;
            _Label7.Text = "Baud Rate:";
            // 
            // rdoSendLine
            // 
            _rdoSendLine.AutoSize = true;
            _rdoSendLine.Checked = true;
            _rdoSendLine.Location = new Point(246, 135);
            _rdoSendLine.Name = "_rdoSendLine";
            _rdoSendLine.Size = new Size(73, 17);
            _rdoSendLine.TabIndex = 18;
            _rdoSendLine.TabStop = true;
            _rdoSendLine.Text = "Send Line";
            _rdoSendLine.UseVisualStyleBackColor = true;
            // 
            // rdoSendWord
            // 
            _rdoSendWord.AutoSize = true;
            _rdoSendWord.Location = new Point(246, 158);
            _rdoSendWord.Name = "_rdoSendWord";
            _rdoSendWord.Size = new Size(79, 17);
            _rdoSendWord.TabIndex = 19;
            _rdoSendWord.TabStop = true;
            _rdoSendWord.Text = "Send Word";
            _rdoSendWord.UseVisualStyleBackColor = true;
            // 
            // rdoSendCharacter
            // 
            _rdoSendCharacter.AutoSize = true;
            _rdoSendCharacter.Location = new Point(246, 181);
            _rdoSendCharacter.Name = "_rdoSendCharacter";
            _rdoSendCharacter.Size = new Size(99, 17);
            _rdoSendCharacter.TabIndex = 20;
            _rdoSendCharacter.TabStop = true;
            _rdoSendCharacter.Text = "Send Character";
            _rdoSendCharacter.UseVisualStyleBackColor = true;
            // 
            // chkWordWrap
            // 
            _chkWordWrap.AutoSize = true;
            _chkWordWrap.Location = new Point(246, 93);
            _chkWordWrap.Name = "_chkWordWrap";
            _chkWordWrap.Size = new Size(81, 17);
            _chkWordWrap.TabIndex = 21;
            _chkWordWrap.Text = "Word Wrap";
            _chkWordWrap.UseVisualStyleBackColor = true;
            // 
            // TerminalSettings
            // 
            AcceptButton = _OK_Button;
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 255);
            Controls.Add(_chkWordWrap);
            Controls.Add(_rdoSendCharacter);
            Controls.Add(_rdoSendWord);
            Controls.Add(_rdoSendLine);
            Controls.Add(_cmbBaudRate);
            Controls.Add(_Label7);
            Controls.Add(_txtWriteTimeout);
            Controls.Add(_cmbParity);
            Controls.Add(_cmbStopBits);
            Controls.Add(_cmbDataBits);
            Controls.Add(_cmbPort);
            Controls.Add(_cmbHandshake);
            Controls.Add(_chkLocalEcho);
            Controls.Add(_chkDTREnable);
            Controls.Add(_chkRTSEnable);
            Controls.Add(_Label6);
            Controls.Add(_Label5);
            Controls.Add(_Label4);
            Controls.Add(_Label3);
            Controls.Add(_Label2);
            Controls.Add(_Label1);
            Controls.Add(_TableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TerminalSettings";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Simple Terminal  Settings";
            _TableLayoutPanel1.ResumeLayout(false);
            Load += new EventHandler(Properties_Load);
            ResumeLayout(false);
            PerformLayout();
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