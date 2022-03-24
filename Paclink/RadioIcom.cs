using System;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using NLog;
using Paclink.UI.Common;

namespace Paclink
{
    public class RadioIcom : IRadio
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public RadioIcom()
        {

            // Objects
            objSerial = new SerialPort();
        }

        // String
        private string strCIVAddress;
        private string strRadioModel;

        // Bytes
        private byte bytCIVAddress;

        // Booleans
        private bool blnViaPTC;
        private bool blnIcomNMEA;
        private SerialPort _objSerial;

        private SerialPort objSerial
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objSerial;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_objSerial != null)
                {
                    _objSerial.DataReceived -= objSerial_DataReceived;
                }

                _objSerial = value;
                if (_objSerial != null)
                {
                    _objSerial.DataReceived += objSerial_DataReceived;
                }
            }
        }

        public bool SetDtrControl(bool Dtr)
        {
            try
            {
                objSerial.DtrEnable = Dtr;
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("[RadioIcom.SetDtrControl] : " + ex.ToString());
                return false;
            }
        }

        public bool SetRtsControl(bool Rts)
        {
            try
            {
                objSerial.RtsEnable = Rts;
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("[RadioIcom.SetRtsControl] : " + ex.ToString());
                return false;
            }
        }

        public bool SetPTT(bool Send)
        {
            // Only currently used for Icom radios
            try
            {
                var bytCommand = new byte[8];
                bytCommand[0] = 0xFE;
                bytCommand[1] = 0xFE;
                bytCommand[2] = bytCIVAddress;
                bytCommand[3] = 0xF1;
                bytCommand[4] = 0x1C;        // Set Transceiver PTT
                bytCommand[5] = 0;          // 
                if (Send)
                {
                    bytCommand[6] = 1;       // Set to Transmit PTT on
                }
                else
                {
                    bytCommand[6] = 0;
                }      // Set to transmit PTT Off

                bytCommand[7] = 0xFD;
                objSerial.Write(bytCommand, 0, bytCommand.Length);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("[RadioIcom.SetPTT] : " + ex.ToString());
                return false;
            }
        }

        public bool InitializeSerialPort(ref TChannelProperties Channel)
        {
            // Opens the serial port used to control the radio. Returns true if port opens...

            strCIVAddress = Channel.CIVAddress;
            bytCIVAddress = byte.Parse(strCIVAddress, System.Globalization.NumberStyles.HexNumber);
            if (Channel.RDOControl == "Via PTCII")
            {
                blnViaPTC = true;
                Globals.ObjScsModem.SendRadioCommand("#TRX TY I " + Channel.RDOControlBaud + " $" + Channel.CIVAddress);
                Thread.Sleep(200);  // Changed to 200 by W4PHS
                return true;
            }

            try
            {
                if (objSerial.IsOpen)
                {
                    objSerial.DiscardInBuffer();
                    objSerial.DiscardOutBuffer();
                    objSerial.Close(); // Close the serial port if it is already open
                    Thread.Sleep(Globals.intComCloseTime);
                }

                objSerial.WriteTimeout = 1000;
                objSerial.ReceivedBytesThreshold = 1;
                objSerial.BaudRate = Convert.ToInt32(Channel.RDOControlBaud);
                objSerial.DataBits = 8;
                objSerial.Parity = Parity.None;
                objSerial.StopBits = StopBits.One;
                objSerial.PortName = Channel.RDOControlPort;
                objSerial.Handshake = Handshake.None;
                objSerial.RtsEnable = true;
                objSerial.DtrEnable = true;
                objSerial.Open();
                objSerial.DiscardInBuffer();
                objSerial.DiscardOutBuffer();
                return objSerial.IsOpen;
            }
            catch (Exception ex)
            {
                _log.Error("[RadioIcom.InitializeSerialPort] " + ex.Message);
                return false;
            }
        }   // InitializeSerialPort 

        public bool SetParameters(ref TChannelProperties Channel)
        {
            bool SetParametersRet = default;
            // Function to set parameters for frequency and filters, possibly other parameters later...
            strRadioModel = Channel.RDOModel;
            SetParametersRet = true;
            if (Channel.ChannelType == ChannelMode.PacketTNC)
            {
                SetParametersRet = SetVHFFrequency(Globals.ExtractFreq(ref Channel.RDOCenterFrequency), Channel.CIVAddress, Channel.RDOModel);
            }
            else
            {
                SetHFFrequency(Globals.ExtractFreq(ref Channel.RDOCenterFrequency), Channel.AudioToneCenter, Channel.CIVAddress, Channel.RDOModel, Channel.NarrowFilter);
            }

            return SetParametersRet;
        } // SetParameters 

        public void Close()
        {
            // Closes and disposes of the radio's serial port...
            try
            {
                if (objSerial is object)
                {
                    if (objSerial.IsOpen)
                    {
                        objSerial.DiscardOutBuffer();
                        objSerial.DiscardInBuffer();
                        objSerial.Close();
                        Thread.Sleep(Globals.intComCloseTime);
                    }
                    // objSerial.Dispose()
                    objSerial = null;
                }
            }
            catch (Exception ex)
            {
                _log.Error("[RadioIcom.Close] " + ex.Message);
            }
        } // Close 

        private bool SetHFFrequency(string strRDOFreq, string strAudioCenterFrequency, string strCIVAddress, string strRadioType, bool blnUseNarrow)
        {
            // Sets the radio's frequency...
            string strKilohertz;
            strKilohertz = Globals.StripMode(strRDOFreq); // Strip off any mode designator
            int intHertz = Globals.KHzToHz(strKilohertz) - Convert.ToInt32(strAudioCenterFrequency);
            switch (strRadioType)
            {
                case "Icom IC-M700pro":
                case "Icom IC-M710":
                case "Icom IC-M710RT":
                case "Icom IC-M802":
                case "Icom (other NMEA)":
                    {
                        SendCommand(ComputeNMEACommand("REMOTE,ON"));
                        SendCommand(ComputeNMEACommand("RXF," + (intHertz / (double)1000000).ToString("#0.000000")));
                        SendCommand(ComputeNMEACommand("TXF," + (intHertz / (double)1000000).ToString("#0.000000")));
                        break;
                    }

                case "Icom IC-7000":
                case "Icom IC-7200":
                case "Icom IC-706":
                case "Icom IC-746":
                case "Icom IC-746pro":
                case "Icom IC-756pro":
                    {
                        SetCIVFrequency(intHertz);  // Set radio frequency
                        var bytCommand = new byte[8];
                        bytCommand[0] = 0xFE;
                        bytCommand[1] = 0xFE;
                        bytCommand[2] = bytCIVAddress;
                        bytCommand[3] = 0xF1;
                        bytCommand[4] = 0x6;        // Set mode
                        bytCommand[5] = 1;          // Set USB
                        if (Globals.UseWideFilter(strRDOFreq) | !blnUseNarrow)
                        {
                            bytCommand[6] = 1;       // Set wide/normal filter
                        }
                        else
                        {
                            bytCommand[6] = 2;
                        }       // Set narrow filter

                        bytCommand[7] = 0xFD;
                        SendCommand(bytCommand);
                        break;
                    }

                default:
                    {
                        SetCIVFrequency(intHertz);  // Set radio frequency
                        var bytCommand = new byte[7];
                        bytCommand[0] = 0xFE;
                        bytCommand[1] = 0xFE;
                        bytCommand[2] = bytCIVAddress;
                        bytCommand[3] = 0xF1;
                        bytCommand[4] = 0x6;        // Set mode
                        bytCommand[5] = 1;          // Set USB
                        bytCommand[6] = 0xFD;
                        SendCommand(bytCommand);
                        break;
                    }
            }

            return default;
        } // SetFrequency

        private bool SetVHFFrequency(string strKilohertz, string strCIVAddress, string strRadioType)
        {
            // Sets the radio's frequency and mode to FM...

            strKilohertz = Globals.StripMode(strKilohertz); // Strip off any mode designator
            int intHertz = Globals.KHzToHz(strKilohertz);
            try
            {
                Thread.Sleep(100);
                // Set to FM mode...
                byte[] bytCommand;
                string strModeCmd = "";
                switch (strRadioType)
                {
                    case "Icom IC-7000":
                    case "Icom IC-746":
                        {
                            bytCommand = new byte[7];
                            bytCommand[0] = 0xFE;
                            bytCommand[1] = 0xFE;
                            bytCommand[2] = bytCIVAddress;
                            bytCommand[3] = 0xF1;
                            bytCommand[4] = 6;        // Set mode
                            bytCommand[5] = 5;        // Set FM
                            bytCommand[6] = 0xFD;
                            SendCommand(bytCommand);
                            break;
                        }

                    case "Icom IC-706":
                        {
                            bytCommand = new byte[8];
                            bytCommand[0] = 0xFE;
                            bytCommand[1] = 0xFE;
                            bytCommand[2] = bytCIVAddress;
                            bytCommand[3] = 0xF1;
                            bytCommand[4] = 6;        // Set mode
                            bytCommand[5] = 5;        // Set FM
                            bytCommand[6] = 1;
                            bytCommand[7] = 0xFD;
                            SendCommand(bytCommand);
                            break;
                        }
                }

                SetCIVFrequency(intHertz);  // Set radio frequency
            }
            catch (Exception ex)
            {
                _log.Error("[RadioIcom.SetFrequency] " + ex.Message);
                return false;
            }

            Thread.Sleep(100);
            return true;
        } // SetFMFrequency

        private bool SetCIVFrequency(double intHertz)
        {
            var bytCommand = new byte[11];
            string strHertz;
            int intPa1;
            int intPa2;
            int intPa3;
            int intPa4;
            int intPa5;
            var switchExpr = strRadioModel;
            switch (switchExpr)
            {
                case "Icom IC-7000":
                case "Icom IC-7200":
                case "Icom IC-706":
                case "Icom IC-746":
                case "Icom IC-746pro":
                case "Icom IC-756pro":
                    {
                        strHertz = Convert.ToInt32(intHertz).ToString("000000000");
                        intPa1 = 16 * Convert.ToInt32(strHertz.Substring(7, 1)) + Convert.ToInt32(strHertz.Substring(8, 1));
                        intPa2 = 16 * Convert.ToInt32(strHertz.Substring(5, 1)) + Convert.ToInt32(strHertz.Substring(6, 1));
                        intPa3 = 16 * Convert.ToInt32(strHertz.Substring(3, 1)) + Convert.ToInt32(strHertz.Substring(4, 1));
                        intPa4 = 16 * Convert.ToInt32(strHertz.Substring(1, 1)) + Convert.ToInt32(strHertz.Substring(2, 1));
                        intPa5 = Convert.ToInt32(strHertz.Substring(0, 1));
                        bytCommand = new byte[11];
                        bytCommand[0] = 0xFE;
                        bytCommand[1] = 0xFE;
                        bytCommand[2] = bytCIVAddress;
                        bytCommand[3] = 0xF1;
                        bytCommand[4] = 0x5; // Set frequency command
                        bytCommand[5] = Convert.ToByte(intPa1);
                        bytCommand[6] = Convert.ToByte(intPa2);
                        bytCommand[7] = Convert.ToByte(intPa3);
                        bytCommand[8] = Convert.ToByte(intPa4);
                        bytCommand[9] = Convert.ToByte(intPa5);
                        bytCommand[10] = 0xFD;
                        SendCommand(bytCommand);
                        break;
                    }

                default:
                    {
                        strHertz = Convert.ToInt32(intHertz).ToString("00000000");
                        intPa1 = 16 * Convert.ToInt32(strHertz.Substring(6, 1)) + Convert.ToInt32(strHertz.Substring(7, 1));
                        intPa2 = 16 * Convert.ToInt32(strHertz.Substring(4, 1)) + Convert.ToInt32(strHertz.Substring(5, 1));
                        intPa3 = 16 * Convert.ToInt32(strHertz.Substring(2, 1)) + Convert.ToInt32(strHertz.Substring(3, 1));
                        intPa4 = 16 * Convert.ToInt32(strHertz.Substring(0, 1)) + Convert.ToInt32(strHertz.Substring(1, 1));
                        bytCommand = new byte[10];
                        bytCommand[0] = 0xFE;
                        bytCommand[1] = 0xFE;
                        bytCommand[2] = bytCIVAddress;
                        bytCommand[3] = 0xF1;
                        bytCommand[4] = 0x5; // Set frequency command
                        bytCommand[5] = Convert.ToByte(intPa1);
                        bytCommand[6] = Convert.ToByte(intPa2);
                        bytCommand[7] = Convert.ToByte(intPa3);
                        bytCommand[8] = Convert.ToByte(intPa4);
                        bytCommand[9] = 0xFD;
                        SendCommand(bytCommand);
                        break;
                    }
            }

            return default;
        } // SetCIVFrequency

        private string ComputeNMEACommand(string strCommand)
        {
            string ComputeNMEACommandRet = default;
            string strBuffer;
            string strCheckSum;
            int intCheckSum = 0;
            int intIndex;
            strBuffer = "$PICOA,90," + strCIVAddress + "," + strCommand;
            var loopTo = strBuffer.Length - 1;
            for (intIndex = 1; intIndex <= loopTo; intIndex++)
                intCheckSum = intCheckSum ^ Globals.Asc(strBuffer.Substring(intIndex, 1)[0]);
            strCheckSum = "*" + "0" + intCheckSum.ToString("X").Right(2);
            ComputeNMEACommandRet = strBuffer + strCheckSum + Globals.CRLF;
            return ComputeNMEACommandRet;
        } // ComputeNMEACommand

        private bool SendCommand(byte[] bytCommand)
        {
            // Sends a command to the radio...

            if (blnViaPTC)
            {
                string strBuffer = "";
                for (int intIndex = 0, loopTo = bytCommand.Length - 1; intIndex <= loopTo; intIndex++)
                    strBuffer = strBuffer + "00" + (bytCommand[intIndex]).ToString("X").Right(2);
                Globals.ObjScsModem.SendRadioCommand("#TRX T " + strBuffer); // Use the Transfer capability of the TRX commands
                Thread.Sleep(100);   // W4PHS
            }
            else
            {
                objSerial.Write(bytCommand, 0, bytCommand.Length);
            }

            Thread.Sleep(100); // Delay for radio commands 
            return true;
        } // SendCommand(Byte())

        private bool SendCommand(string strCommand)
        {
            // Sends a command to the radio...

            try
            {
                if (blnViaPTC)
                {
                    string strBuffer = "";
                    for (int intIndex = 0, loopTo = strCommand.Length - 1; intIndex <= loopTo; intIndex++)
                        strBuffer = strBuffer + ("00" + Globals.Asc(strCommand.Substring(intIndex, 1)[0]).ToString("X")).Right(2);
                    Globals.ObjScsModem.SendRadioCommand("#TRX T " + strBuffer); // Use the Transfer capability of the TRX commands
                    Thread.Sleep(100);   // W4PHS
                }
                else
                {
                    // Use byte write to handle character values > 128
                    var objEncoder = new ASCIIEncoding();
                    objSerial.Write(Globals.GetBytes(strCommand), 0, strCommand.Length);
                }

                Thread.Sleep(100);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("[RadioIcom.SendCommand] " + ex.Message);
                return false;
            }
        } // SendCommand(String)

        private void objSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int intCount = objSerial.BytesToRead;
            var bytData = new byte[intCount];
            objSerial.Read(bytData, 0, intCount);
        }
    }
}