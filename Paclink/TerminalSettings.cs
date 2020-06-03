using System;
using System.IO.Ports;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public partial class TerminalSettings
    {
        public TerminalSettings()
        {
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
            DialogResult = DialogResult.OK;
            Terminal.strPort = cmbPort.Text;
            Terminal.intBaudRate = Conversions.ToInteger(cmbBaudRate.Text);
            Terminal.intDataBits = Conversions.ToInteger(cmbDataBits.Text);
            var switchExpr = cmbStopBits.Text;
            switch (switchExpr)
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

            var switchExpr1 = cmbParity.Text;
            switch (switchExpr1)
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

            var switchExpr2 = cmbHandshake.Text;
            switch (switchExpr2)
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
                Terminal.enmBufferType = Terminal.BufferType.Line;
            if (rdoSendWord.Checked)
                Terminal.enmBufferType = Terminal.BufferType.Word;
            if (rdoSendCharacter.Checked)
                Terminal.enmBufferType = Terminal.BufferType.Character;
            Terminal.blnChanged = true;
            Close();
        } // OK_Button_Click

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        } // Cancel_Button_Click

        private void Properties_Load(object sender, EventArgs e)
        {
            InitializeComboBoxes();
            if (!string.IsNullOrEmpty(Terminal.strPort))
            {
                cmbPort.Text = Terminal.strPort;
            }
            else
            {
                cmbPort.Text = Conversions.ToString(cmbPort.Items[0]);
            }

            cmbBaudRate.Text = Terminal.intBaudRate.ToString();
            cmbDataBits.Text = Terminal.intDataBits.ToString();
            var switchExpr = Terminal.intStopBits;
            switch (switchExpr)
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

            var switchExpr1 = Terminal.intParity;
            switch (switchExpr1)
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

            var switchExpr2 = Terminal.intHandshake;
            switch (switchExpr2)
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
                case Terminal.BufferType.Line:
                    {
                        rdoSendLine.Checked = true;
                        break;
                    }

                case Terminal.BufferType.Word:
                    {
                        rdoSendWord.Checked = true;
                        break;
                    }

                case Terminal.BufferType.Character:
                    {
                        rdoSendCharacter.Checked = true;
                        break;
                    }
            }
        } // Properties_Load

        private void InitializeComboBoxes()
        {
            var strPortNames = SerialPort.GetPortNames();
            cmbPort.Sorted = true;
            foreach (string strPort in strPortNames)
                cmbPort.Items.Add(Globals.CleanSerialPort(strPort));
            {
                var withBlock = cmbBaudRate.Items;
                withBlock.Add(110);
                withBlock.Add(300);
                withBlock.Add(1200);
                withBlock.Add(2400);
                withBlock.Add(4800);
                withBlock.Add(9600);
                withBlock.Add(19200);
                withBlock.Add(38400);
                withBlock.Add(57600);
                withBlock.Add(115200);
            }

            {
                var withBlock1 = cmbDataBits.Items;
                withBlock1.Add(5);
                withBlock1.Add(6);
                withBlock1.Add(7);
                withBlock1.Add(8);
            }

            {
                var withBlock2 = cmbStopBits.Items;
                withBlock2.Add(0);
                withBlock2.Add(1);
                withBlock2.Add(1.5);
                withBlock2.Add(2);
            }

            {
                var withBlock3 = cmbParity.Items;
                withBlock3.Add("None");
                withBlock3.Add("Odd");
                withBlock3.Add("Even");
                withBlock3.Add("Mark");
                withBlock3.Add("Space");
            }

            {
                var withBlock4 = cmbHandshake.Items;
                withBlock4.Add("None");
                withBlock4.Add("RTS/CTS");
                withBlock4.Add("XOn/XOff");
                withBlock4.Add("Both");
            }
        } // InitializeComboBoxes
    }
}