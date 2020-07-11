using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Paclink
{
    public partial class Terminal
    {
        public Terminal()
        {
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
            _mnuViewLog.Name = "mnuViewLog";
            _txtDisplay.Name = "txtDisplay";
        }

        public enum BufferType
        {
            Line,
            Word,
            Character
        }

        // Simple terminal parameters...
        public static string strPort = "";
        public static string strLog = "";
        public static int intBaudRate = 9600;
        public static int intDataBits = 8;
        public static int intStopBits = 1;
        public static int intParity = 0;
        public static int intHandshake = 2;
        public static int intWriteTimeout = 1000;
        public static bool blnTerminalIsOpen;
        public static bool blnRTSEnable = true;
        public static bool blnDTREnable = true;
        public static bool blnLocalEcho = true;
        public static bool blnWordWrap = false;
        public static bool blnChanged = false;
        public static BufferType enmBufferType;
        private TerminalSettings dlgProperties;

        private void Terminal_Load(object sender, EventArgs e)
        {
            blnTerminalIsOpen = true;
            tmrTerminal.Start();
        } // Terminal_Load

        private void Terminal_Activated(object sender, EventArgs e)
        {
            txtKeyboard.Focus();
        } // Terminal_Activated

        private void Terminal_FormClosed(object sender, FormClosedEventArgs e)
        {
            objSerialPort.Close();
            Thread.Sleep(100);
            SaveCurrentProperties();
            blnTerminalIsOpen = false;
        } // Terminal_FormClosed

        private void Terminal_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtKeyboard.Focus();
        } // Terminal_KeyPress

        private bool blnStarted = false;

        private void tmrTerminal_Tick(object sender, EventArgs e)
        {
            if (!blnStarted)
            {
                blnStarted = true;
                strLog = Globals.SiteRootDirectory + @"Logs\Simple Terminal.Log";
                LoadSavedProperties();
                if (!string.IsNullOrEmpty(strPort))
                {
                    if (!InitializeSerialPort())
                    {
                        txtDisplay.Text = "Serial port " + strPort + " failed to open..." + Globals.CRLF;
                    }
                }

                return;
            }

            if (dlgProperties == null)
            {
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

                        string strData = sbd.ToString().Replace(Globals.LF, "");
                        strData = strData.Replace(Globals.CR, Globals.CRLF);
                        txtDisplay.AppendText(strData);
                        My.MyProject.Computer.FileSystem.WriteAllText(strLog, strData, true);
                    }
                }
            }
        } // tmrTerminal_Tick

        private void txtKeyboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (objSerialPort.IsOpen)
            {
                if (e.KeyChar == '\u0003' | e.KeyChar == '\u001b')
                {
                    if (blnLocalEcho)
                        txtDisplay.AppendText(Globals.CRLF);
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
                            if (e.KeyChar.ToString() == Globals.CR)
                            {
                                if (blnLocalEcho)
                                    txtDisplay.AppendText(txtKeyboard.Text + Globals.CRLF);
                                try
                                {
                                    objSerialPort.Write(txtKeyboard.Text + Globals.CR);
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

                            if (e.KeyChar.ToString() == Globals.CR)
                            {
                                if (blnLocalEcho)
                                    txtDisplay.AppendText(txtKeyboard.Text + Globals.CRLF);
                                try
                                {
                                    objSerialPort.Write(txtKeyboard.Text + Globals.CR);
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
                                if (e.KeyChar.ToString() == Globals.CR)
                                    txtDisplay.AppendText(Globals.LF);
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
        } // txtKeyboard_KeyPress

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
        } // txtKeyboard_KeyUp

        private void txtDisplay_KeyUp(object sender, KeyEventArgs e)
        {
            txtKeyboard.Focus();
        } // txtDisplay_KeyUp

        private void txtDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            txtKeyboard.Focus();
        } // txtDisplay_MouseUp

        private bool InitializeSerialPort()
        {
            {
                var withBlock = objSerialPort;
                txtDisplay.WordWrap = blnWordWrap;
                if (withBlock.IsOpen)
                {
                    try
                    {
                        withBlock.Close(); // Try and close in case port is already open
                        Thread.Sleep(Globals.intComCloseTime);
                    }
                    catch
                    {
                        Text = "Simple Terminal";
                        Interaction.MsgBox("Unable to reinitialize " + strPort);
                        return false;
                    }
                }

                withBlock.WriteTimeout = intWriteTimeout;
                withBlock.BaudRate = intBaudRate;
                withBlock.DataBits = intDataBits;
                withBlock.StopBits = (StopBits)Convert.ToInt32(intStopBits);
                withBlock.PortName = strPort;
                withBlock.Parity = (Parity)Convert.ToInt32(intParity);
                withBlock.Handshake = (Handshake)Convert.ToInt32(intHandshake);
                withBlock.RtsEnable = blnRTSEnable;
                withBlock.DtrEnable = blnDTREnable;
                try
                {
                    withBlock.Open();
                    withBlock.DiscardInBuffer();
                    withBlock.DiscardOutBuffer();
                }
                catch
                {
                    Text = "Simple Terminal " + strPort + " Closed";
                    Interaction.MsgBox("Unable to open " + strPort + ". May be open by another program...", MsgBoxStyle.Information);
                    return false;
                }

                if (withBlock.IsOpen)
                {
                    Text = "Simple Terminal " + strPort + " Open";
                }
                else
                {
                    Text = "Simple Terminal " + strPort + " Closed";
                }

                return withBlock.IsOpen;
            }

            return false;
        } // InitializeSerialPort

        private void SaveCurrentProperties()
        {
            var sbdProperties = new StringBuilder();
            sbdProperties.Append("Port " + strPort + Globals.CRLF);
            sbdProperties.Append("BaudRate " + intBaudRate.ToString() + Globals.CRLF);
            sbdProperties.Append("DataBits " + intDataBits.ToString() + Globals.CRLF);
            sbdProperties.Append("StopBits " + intStopBits.ToString() + Globals.CRLF);
            sbdProperties.Append("Parity " + intParity.ToString() + Globals.CRLF);
            sbdProperties.Append("Handshake " + intHandshake.ToString() + Globals.CRLF);
            sbdProperties.Append("WriteTimeout " + intWriteTimeout.ToString() + Globals.CRLF);
            sbdProperties.Append("RTSEnable " + blnRTSEnable.ToString() + Globals.CRLF);
            sbdProperties.Append("DTREnable " + blnDTREnable.ToString() + Globals.CRLF);
            sbdProperties.Append("LocalEcho " + blnLocalEcho.ToString() + Globals.CRLF);
            sbdProperties.Append("WordWrap " + blnWordWrap.ToString() + Globals.CRLF);
            sbdProperties.Append("BufferType " + enmBufferType.ToString() + Globals.CRLF);
            sbdProperties.Append("Top " + Top.ToString() + Globals.CRLF);
            sbdProperties.Append("Left " + Left.ToString() + Globals.CRLF);
            sbdProperties.Append("Width " + Width.ToString() + Globals.CRLF);
            sbdProperties.Append("Height " + Height.ToString() + Globals.CRLF);
            My.MyProject.Computer.FileSystem.WriteAllText(Globals.SiteRootDirectory + @"Data\Simple Terminal.stx", sbdProperties.ToString(), false);
        } // SaveCurrentProperties

        private void LoadSavedProperties()
        {
            string strProperties = "";
            string strPath = Globals.SiteRootDirectory + @"Data\Simple Terminal.stx";
            if (File.Exists(strPath))
            {
                strProperties = My.MyProject.Computer.FileSystem.ReadAllText(strPath);
            }
            else
            {
                dlgProperties = new TerminalSettings();
                dlgProperties.ShowDialog();
                SaveCurrentProperties();
                dlgProperties = null;
            }

            var objTextString = new StringReader(strProperties);
            do
            {
                string strLine = objTextString.ReadLine();
                if (string.IsNullOrEmpty(strLine))
                    break;
                var strTokens = strLine.Split(' ');
                if (strTokens.Length > 1)
                {
                    var switchExpr = strTokens[0];
                    switch (switchExpr)
                    {
                        case "Port":
                            {
                                strPort = strTokens[1];
                                break;
                            }

                        case "BaudRate":
                            {
                                intBaudRate = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "DataBits":
                            {
                                intDataBits = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "StopBits":
                            {
                                intStopBits = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "Parity":
                            {
                                intParity = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "Handshake":
                            {
                                intHandshake = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "WriteTimeout":
                            {
                                intWriteTimeout = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "RTSEnable":
                            {
                                blnRTSEnable = Convert.ToBoolean(strTokens[1]);
                                break;
                            }

                        case "DTREnable":
                            {
                                blnDTREnable = Convert.ToBoolean(strTokens[1]);
                                break;
                            }

                        case "LocalEcho":
                            {
                                blnLocalEcho = Convert.ToBoolean(strTokens[1]);
                                break;
                            }

                        case "WordWrap":
                            {
                                blnWordWrap = Convert.ToBoolean(strTokens[1]);
                                break;
                            }

                        case "BufferType":
                            {
                                enmBufferType = (BufferType)Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "Top":
                            {
                                Top = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "Left":
                            {
                                Left = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "Width":
                            {
                                Width = Convert.ToInt32(strTokens[1]);
                                break;
                            }

                        case "Height":
                            {
                                Height = Convert.ToInt32(strTokens[1]);
                                break;
                            }
                    }
                }
            }
            while (true);
            objTextString.Dispose();
        } // LoadSavedProperties

        private void mnuProperties_Click(object sender, EventArgs e)
        {
            dlgProperties = new TerminalSettings();
            dlgProperties.StartPosition = FormStartPosition.CenterParent;
            if (dlgProperties.ShowDialog() == DialogResult.OK)
                InitializeSerialPort();
            SaveCurrentProperties();
            dlgProperties = null;
        } // mnuProperties

        private void mnuClearDisplay_Click(object sender, EventArgs e)
        {
            txtDisplay.Clear();
            txtKeyboard.Clear();
            txtKeyboard.Focus();
        } // mnuClearDisplay

        private void mnuViewLog_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(strLog);
            }
            catch
            {
                Interaction.MsgBox(Information.Err().Description, MsgBoxStyle.Information);
            }
        } // mnuViewLog_Click

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
                objSerialPort.Write(Globals.CR);
            }
            catch
            {
            }
        } // mnuClearSCSHostMode_Click

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
                objSerialPort.Write(Globals.CR);
            }
            catch
            {
            }
        } // mnuClearKantronicsHostMode_Click

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
                objSerialPort.Write(Globals.CR);
            }
            catch
            {
            }
        } // mnuClearTimewaveHostMode_Click

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
        } // mnuClearKiss_Click

        private void mnuClose_Click(object sender, EventArgs e)
        {
            Close();
        } // mnuClose_Click
    }
}