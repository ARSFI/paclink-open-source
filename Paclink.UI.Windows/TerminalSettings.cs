using System;
using System.IO.Ports;
using System.Windows.Forms;
using Paclink.UI.Common;

namespace Paclink.UI.Windows
{
    public partial class TerminalSettings : IWindow<ITerminalSettingsBacking>
    {
        private ITerminalSettingsBacking _backingObject;
        public ITerminalSettingsBacking BackingObject => _backingObject;

        public TerminalSettings(ITerminalSettingsBacking backingObject)
        {
            _backingObject = backingObject;

            InitializeComponent();
            _TableLayoutPanel1.Name = "TableLayoutPanel1";
            _OK_Button.Name = "OK_Button";
            _Cancel_Button.Name = "Cancel_Button";
            _Label1.Name = "Label1";
            _Label2.Name = "Label2";
            _Label3.Name = "Label3";
            _Label4.Name = "Label4";
            _Label5.Name = "Label5";
            _Label6.Name = "Label6";
            _chkRTSEnable.Name = "chkRTSEnable";
            _chkDTREnable.Name = "chkDTREnable";
            _chkLocalEcho.Name = "chkLocalEcho";
            _cmbHandshake.Name = "cmbHandshake";
            _cmbPort.Name = "cmbPort";
            _cmbDataBits.Name = "cmbDataBits";
            _cmbStopBits.Name = "cmbStopBits";
            _cmbParity.Name = "cmbParity";
            _txtWriteTimeout.Name = "txtWriteTimeout";
            _cmbBaudRate.Name = "cmbBaudRate";
            _Label7.Name = "Label7";
            _rdoSendLine.Name = "rdoSendLine";
            _rdoSendWord.Name = "rdoSendWord";
            _rdoSendCharacter.Name = "rdoSendCharacter";
            _chkWordWrap.Name = "chkWordWrap";
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            BackingObject.DialogResult = DialogFormResult.OK;
            Terminal.strPort = cmbPort.Text;
            Terminal.intBaudRate = Convert.ToInt32(cmbBaudRate.Text);
            Terminal.intDataBits = Convert.ToInt32(cmbDataBits.Text);

            switch (cmbStopBits.Text)
            {
                case "0":
                    {
                        Terminal.intStopBits = 0;
                        break;
                    }

                case "1":
                    {
                        Terminal.intStopBits = 1;
                        break;
                    }

                case "2":
                    {
                        Terminal.intStopBits = 2;
                        break;
                    }

                case "1.5":
                    {
                        Terminal.intStopBits = 3;
                        break;
                    }
            }

            switch (cmbParity.Text)
            {
                case "None":
                    {
                        Terminal.intParity = 0;
                        break;
                    }

                case "Odd":
                    {
                        Terminal.intParity = 1;
                        break;
                    }

                case "Even":
                    {
                        Terminal.intParity = 2;
                        break;
                    }

                case "Mark":
                    {
                        Terminal.intParity = 3;
                        break;
                    }

                case "Space":
                    {
                        Terminal.intParity = 4;
                        break;
                    }
            }

            switch (cmbHandshake.Text)
            {
                case "None":
                    {
                        Terminal.intHandshake = 0;
                        break;
                    }

                case "XOn/XOff":
                    {
                        Terminal.intHandshake = 1;
                        break;
                    }

                case "RTS/CTS":
                    {
                        Terminal.intHandshake = 2;
                        break;
                    }

                case "Both":
                    {
                        Terminal.intHandshake = 3;
                        break;
                    }
            }

            Terminal.blnRTSEnable = chkRTSEnable.Checked;
            Terminal.blnDTREnable = chkDTREnable.Checked;
            Terminal.blnLocalEcho = chkLocalEcho.Checked;
            Terminal.blnWordWrap = chkWordWrap.Checked;
            if (rdoSendLine.Checked)
                Terminal.enmBufferType = BufferType.Line;
            if (rdoSendWord.Checked)
                Terminal.enmBufferType = BufferType.Word;
            if (rdoSendCharacter.Checked)
                Terminal.enmBufferType = BufferType.Character;
            Terminal.blnChanged = true;
            Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            BackingObject.DialogResult = DialogFormResult.Cancel;
            Close();
        }

        private void Properties_Load(object sender, EventArgs e)
        {
            InitializeComboBoxes();
            if (!string.IsNullOrEmpty(Terminal.strPort))
            {
                cmbPort.Text = Terminal.strPort;
            }
            else
            {
                if (cmbPort.Items.Count > 0)
                {
                    cmbPort.Text = cmbPort.Items[0].ToString();
                }
                else
                {
                    //no com ports found - exit
                    MessageBox.Show("No Com ports were found. Unable to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }

            cmbBaudRate.Text = Terminal.intBaudRate.ToString();
            cmbDataBits.Text = Terminal.intDataBits.ToString();

            switch (Terminal.intStopBits)
            {
                case 0:
                    {
                        cmbStopBits.Text = "0";
                        break;
                    }

                case 1:
                    {
                        cmbStopBits.Text = "1";
                        break;
                    }

                case 2:
                    {
                        cmbStopBits.Text = "2";
                        break;
                    }

                case 3:
                    {
                        cmbStopBits.Text = "1.5";
                        break;
                    }
            }

            switch (Terminal.intParity)
            {
                case 0:
                    {
                        cmbParity.Text = "None";
                        break;
                    }

                case 1:
                    {
                        cmbParity.Text = "Odd";
                        break;
                    }

                case 2:
                    {
                        cmbParity.Text = "Even";
                        break;
                    }

                case 3:
                    {
                        cmbParity.Text = "Mark";
                        break;
                    }

                case 4:
                    {
                        cmbParity.Text = "Space";
                        break;
                    }
            }

            switch (Terminal.intHandshake)
            {
                case 0:
                    {
                        cmbHandshake.Text = "None";
                        break;
                    }

                case 1:
                    {
                        cmbHandshake.Text = "XOn/XOff";
                        break;
                    }

                case 2:
                    {
                        cmbHandshake.Text = "RTS/CTS";
                        break;
                    }

                case 3:
                    {
                        cmbHandshake.Text = "Both";
                        break;
                    }
            }

            txtWriteTimeout.Text = Terminal.intWriteTimeout.ToString();
            chkRTSEnable.Checked = Terminal.blnRTSEnable;
            chkDTREnable.Checked = Terminal.blnDTREnable;
            chkLocalEcho.Checked = Terminal.blnLocalEcho;
            chkWordWrap.Checked = Terminal.blnWordWrap;
            var switchExpr3 = Terminal.enmBufferType;
            switch (switchExpr3)
            {
                case BufferType.Line:
                    {
                        rdoSendLine.Checked = true;
                        break;
                    }

                case BufferType.Word:
                    {
                        rdoSendWord.Checked = true;
                        break;
                    }

                case BufferType.Character:
                    {
                        rdoSendCharacter.Checked = true;
                        break;
                    }
            }
        }

        private void InitializeComboBoxes()
        {
            var portNames = SerialPort.GetPortNames();
            if (portNames.Length == 0) return;

            cmbPort.Sorted = true;
            foreach (string port in portNames)
            {
                cmbPort.Items.Add(BackingObject.CleanSerialPort(port));
            }

            cmbBaudRate.Items.Add(110);
            cmbBaudRate.Items.Add(300);
            cmbBaudRate.Items.Add(1200);
            cmbBaudRate.Items.Add(2400);
            cmbBaudRate.Items.Add(4800);
            cmbBaudRate.Items.Add(9600);
            cmbBaudRate.Items.Add(19200);
            cmbBaudRate.Items.Add(38400);
            cmbBaudRate.Items.Add(57600);
            cmbBaudRate.Items.Add(115200);

            cmbDataBits.Items.Add(5);
            cmbDataBits.Items.Add(6);
            cmbDataBits.Items.Add(7);
            cmbDataBits.Items.Add(8);

            cmbStopBits.Items.Add(0);
            cmbStopBits.Items.Add(1);
            cmbStopBits.Items.Add(1.5);
            cmbStopBits.Items.Add(2);

            cmbParity.Items.Add("None");
            cmbParity.Items.Add("Odd");
            cmbParity.Items.Add("Even");
            cmbParity.Items.Add("Mark");
            cmbParity.Items.Add("Space");

            cmbHandshake.Items.Add("None");
            cmbHandshake.Items.Add("RTS/CTS");
            cmbHandshake.Items.Add("XOn/XOff");
            cmbHandshake.Items.Add("Both");
        }

        public UiDialogResult ShowModal()
        {
            throw new NotImplementedException();
        }

        public void RefreshWindow()
        {
            // empty
        }

        public void CloseWindow()
        {
            // empty
        }
    }
}