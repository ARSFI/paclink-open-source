using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Paclink.UI.Common;
using NLog;

namespace Paclink.UI.Windows
{
    public partial class Terminal : IWindow<ITerminalBacking>
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private ITerminalBacking _backingObject;
        public ITerminalBacking BackingObject => _backingObject;

        public Terminal(ITerminalBacking backingObject)
        {
            _backingObject = backingObject;

            InitializeComponent();
            _txtKeyboard.Name = "txtKeyboard";
            _mnuMain.Name = "mnuMain";
            _mnuClose.Name = "mnuClose";
            _mnuProperties.Name = "mnuProperties";
            _mnuClearDisplay.Name = "mnuClearDisplay";
            _mnuClearHost.Name = "mnuClearHost";
            _mnuClearSCSHostMode.Name = "mnuClearSCSHostMode";
            _mnuClearKantronicsHostMode.Name = "mnuClearKantronicsHostMode";
            _mnuClearTimewaveHostMode.Name = "mnuClearTimewaveHostMode";
            _mnuClearKiss.Name = "mnuClearKiss";
            _txtDisplay.Name = "txtDisplay";
        }

        //!!! replace with instance of TerminalProperties
        // Simple terminal parameters...
        public static string strPort = "";
        public static int intBaudRate = 9600;
        public static int intDataBits = 8;
        public static int intStopBits = 1;
        public static int intParity = 0;
        public static int intHandshake = 2;
        public static int intWriteTimeout = 1000;
        public static bool blnRTSEnable = true;
        public static bool blnDTREnable = true;
        public static bool blnLocalEcho = true;
        public static bool blnWordWrap = false;
        public static BufferType enmBufferType;

        public static bool blnChanged = false;

        private void Terminal_Load(object sender, EventArgs e)
        {
            tmrTerminal.Start();
        }

        private void Terminal_Activated(object sender, EventArgs e)
        {
            txtKeyboard.Focus();
            BackingObject.TerminalIsActive = true;
        }

        private void Terminal_FormClosed(object sender, FormClosedEventArgs e)
        {
            objSerialPort.Close();
            Thread.Sleep(100);
            SaveCurrentProperties();
            BackingObject.TerminalIsActive = false;
        }

        private void Terminal_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtKeyboard.Focus();
        }

        private bool blnStarted = false;

        private void tmrTerminal_Tick(object sender, EventArgs e)
        {
            if (!blnStarted)
            {
                blnStarted = true;
                LoadSavedProperties();
                if (!string.IsNullOrEmpty(strPort))
                {
                    if (!InitializeSerialPort())
                    {
                        txtDisplay.Text = "Serial port " + strPort + " failed to open...\r\n";
                    }
                }

                return;
            }

            if (objSerialPort.IsOpen)
            {
                int intCount = objSerialPort.BytesToRead;
                if (intCount > 0)
                {
                    var sbd = new StringBuilder();
                    var bytIn = new byte[intCount];
                    objSerialPort.Read(bytIn, 0, intCount);
                    foreach (byte byt in bytIn)
                    {
                        if (byt != 0)
                            sbd.Append((char)byt);
                    }

                    string strData = sbd.ToString().Replace("\n", "");
                    strData = strData.Replace("\r", "\r\n");
                    txtDisplay.AppendText(strData);
                    _log.Info(strData);
                }
            }
        }

        private void txtKeyboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (objSerialPort.IsOpen)
            {
                if (e.KeyChar == '\u0003' | e.KeyChar == '\u001b')
                {
                    if (blnLocalEcho)
                        txtDisplay.AppendText("\r\n");
                    try
                    {
                        objSerialPort.Write(e.KeyChar.ToString());
                    }
                    catch
                    {
                    }

                    txtKeyboard.Clear();
                    e.KeyChar = '\0';
                    return;
                }

                var switchExpr = enmBufferType;
                switch (switchExpr)
                {
                    case BufferType.Line:
                        {
                            if (e.KeyChar.ToString() == "\r")
                            {
                                if (blnLocalEcho)
                                    txtDisplay.AppendText(txtKeyboard.Text + "\r\n");
                                try
                                {
                                    objSerialPort.Write(txtKeyboard.Text + "\r");
                                }
                                catch
                                {
                                }

                                txtKeyboard.Clear();
                            }

                            break;
                        }

                    case BufferType.Word:
                        {
                            if (e.KeyChar == '\b')
                            {
                                return;
                            }

                            if (e.KeyChar.ToString() == " ")
                            {
                                if (blnLocalEcho)
                                    txtDisplay.AppendText(txtKeyboard.Text + " ");
                                try
                                {
                                    objSerialPort.Write(txtKeyboard.Text + " ");
                                }
                                catch
                                {
                                }

                                e.KeyChar = '\0';
                                txtKeyboard.Clear();
                            }

                            if (e.KeyChar.ToString() == "\r")
                            {
                                if (blnLocalEcho)
                                    txtDisplay.AppendText(txtKeyboard.Text + "\r\n");
                                try
                                {
                                    objSerialPort.Write(txtKeyboard.Text + "\r");
                                }
                                catch
                                {
                                }

                                e.KeyChar = '\0';
                                txtKeyboard.Clear();
                            }

                            break;
                        }

                    case BufferType.Character:
                        {
                            if (blnLocalEcho)
                            {
                                txtDisplay.AppendText(e.KeyChar.ToString());
                                if (e.KeyChar.ToString() == "\r")
                                    txtDisplay.AppendText("\n");
                            }

                            try
                            {
                                objSerialPort.Write(e.KeyChar.ToString());
                            }
                            catch
                            {
                            }

                            e.KeyChar = '\0';
                            txtKeyboard.Clear();
                            break;
                        }
                }
            }
            else
            {
                txtKeyboard.Clear();
                e.KeyChar = '\0';
            }
        }

        private void txtKeyboard_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 106)
            {
                try
                {
                    objSerialPort.Write("*");
                }
                catch
                {
                }

                txtKeyboard.Clear();
            }
        }

        private void txtDisplay_KeyUp(object sender, KeyEventArgs e)
        {
            txtKeyboard.Focus();
        }

        private void txtDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            txtKeyboard.Focus();
        }

        private bool InitializeSerialPort()
        {
            txtDisplay.WordWrap = blnWordWrap;
            if (objSerialPort.IsOpen)
            {
                try
                {
                    // Try and close in case port is already open
                    objSerialPort.Close();
                    Thread.Sleep(BackingObject.GetComCloseTime);
                }
                catch
                {
                    Text = "Simple Terminal";
                    MessageBox.Show("Unable to reinitialize " + strPort);
                    return false;
                }
            }

            objSerialPort.WriteTimeout = intWriteTimeout;
            objSerialPort.BaudRate = intBaudRate;
            objSerialPort.DataBits = intDataBits;
            objSerialPort.StopBits = (StopBits)Convert.ToInt32(intStopBits);
            objSerialPort.PortName = strPort;
            objSerialPort.Parity = (Parity)Convert.ToInt32(intParity);
            objSerialPort.Handshake = (Handshake)Convert.ToInt32(intHandshake);
            objSerialPort.RtsEnable = blnRTSEnable;
            objSerialPort.DtrEnable = blnDTREnable;
            try
            {
                objSerialPort.Open();
                objSerialPort.DiscardInBuffer();
                objSerialPort.DiscardOutBuffer();
            }
            catch
            {
                Text = "Simple Terminal " + strPort + " Closed";
                MessageBox.Show("Unable to open " + strPort + ". May be open by another program...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (objSerialPort.IsOpen)
            {
                Text = "Simple Terminal " + strPort + " Open";
            }
            else
            {
                Text = "Simple Terminal " + strPort + " Closed";
            }

            return objSerialPort.IsOpen;
        }

        private TerminalProperties GetCurrentProperties()
        {
            var terminalProperties = new TerminalProperties
            {
                Port = strPort,
                BaudRate = intBaudRate,
                DataBits = intDataBits,
                StopBits = intStopBits,
                Parity = intParity,
                Handshake = intHandshake,
                WriteTimeout = intWriteTimeout,
                RTSEnable = blnRTSEnable,
                DTREnable = blnDTREnable,
                LocalEcho = blnLocalEcho,
                WordWrap = blnWordWrap,
                BufferType = enmBufferType
            };
            return terminalProperties;
        }

        private void SaveCurrentProperties()
        {
            var terminalProperties = GetCurrentProperties();
            BackingObject.SaveTerminalProperties(terminalProperties);
        }

        private void LoadSavedProperties()
        {
            TerminalProperties terminalProperties = BackingObject.LoadTerminalProperties();
            strPort = terminalProperties.Port;
            intBaudRate = terminalProperties.BaudRate;
            intDataBits = terminalProperties.DataBits;
            intStopBits = terminalProperties.StopBits;
            intParity = terminalProperties.Parity;
            intHandshake = terminalProperties.Handshake;
            intWriteTimeout = terminalProperties.WriteTimeout;
            blnRTSEnable = terminalProperties.RTSEnable;
            blnDTREnable = terminalProperties.DTREnable;
            blnLocalEcho = terminalProperties.LocalEcho;
            blnWordWrap = terminalProperties.WordWrap;
            enmBufferType = terminalProperties.BufferType;
        }

        private void mnuProperties_Click(object sender, EventArgs e)
        {
            var terminalProperties = GetCurrentProperties();
            if (BackingObject.EditTerminalProperties(terminalProperties))
            {
                InitializeSerialPort();
            }
        }

        private void mnuClearDisplay_Click(object sender, EventArgs e)
        {
            txtDisplay.Clear();
            txtKeyboard.Clear();
            txtKeyboard.Focus();
        }

        private void mnuClearSCSHostMode_Click(object sender, EventArgs e)
        {
            var bytClearPTCHost = new byte[] { 0xAA, 0xAA, 0x1F, 0xC1, 0x5, 0x4A, 0x48, 0x4F, 0x53, 0x54, 0x30, 0x54, 0xFA };
            try
            {
                objSerialPort.Write(bytClearPTCHost, 0, 13);
            }
            catch
            {
            }

            Thread.Sleep(1000);
            try
            {
                objSerialPort.Write("\r");
            }
            catch
            {
            }
        }

        private void mnuClearKantronicsHostMode_Click(object sender, EventArgs e)
        {
            var bytClearKantronicsHost = new byte[] { 0xC0, 0x71, 0xC0 };
            try
            {
                objSerialPort.Write(bytClearKantronicsHost, 0, 3);
            }
            catch
            {
            }

            Thread.Sleep(1000);
            try
            {
                objSerialPort.Write("\r");
            }
            catch
            {
            }
        }

        private void mnuClearTimewaveHostMode_Click(object sender, EventArgs e)
        {
            var bytClearDEDHost = new byte[] { 0x1, 0x4F, 0x48, 0x4F, 0x4E, 0x17 };
            try
            {
                objSerialPort.Write(bytClearDEDHost, 0, 6);
            }
            catch
            {
            }

            Thread.Sleep(1000);
            try
            {
                objSerialPort.Write("\r");
            }
            catch
            {
            }
        }

        private void mnuClearKiss_Click(object sender, EventArgs e)
        {
            var bytClearKiss = new byte[] { 0xC0, 0xFF, 0xC0 };
            try
            {
                objSerialPort.Write(bytClearKiss, 0, 3);
            }
            catch
            {
            }
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            Close();
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