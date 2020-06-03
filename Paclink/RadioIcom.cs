using System;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public class RadioIcom : IRadio
    {
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
                Logs.Exception("[RadioIcom.SetDtrControl] : " + ex.ToString());
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
                Logs.Exception("[RadioIcom.SetRtsControl] : " + ex.ToString());
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
                Logs.Exception("[RadioIcom.SetPTT] : " + ex.ToString());
                return false;
            }
        }

        public bool InitializeSerialPort(ref TChannelProperties Channel)
        {
            // Opens the serial port used to control the radio. Returns true if port opens...

            strCIVAddress = Channel.CIVAddress;
            bytCIVAddress = Conversions.ToByte("&H" + strCIVAddress);
            if (Channel.RDOControl == "Via PTCII")
            {
                blnViaPTC = true;
                Globals.objSCSClient.SendRadioCommand("#TRX TY I " + Channel.RDOControlBaud + " $" + Channel.CIVAddress);
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
                objSerial.BaudRate = Conversions.ToInteger(Channel.RDOControlBaud);
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
            catch
            {
                Logs.Exception("[RadioIcom.InitializeSerialPort] " + Information.Err().Description);
                return false;
            }
        }   // InitializeSerialPort 

        public bool SetParameters(ref TChannelProperties Channel)
        {
            bool SetParametersRet = default;
            // Function to set parameters for frequency and filters, possibly other parameters later...
            strRadioModel = Channel.RDOModel;
            SetParametersRet = true;
            if (Channel.ChannelType == EChannelModes.PacketTNC)
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
            catch
            {
                Logs.Exception("[RadioIcom.Close] " + Information.Err().Description);
            }
        } // Close 

        private bool SetHFFrequency(string strRDOFreq, string strAudioCenterFrequency, string strCIVAddress, string strRadioType, bool blnUseNarrow)
        {
            // Sets the radio's frequency...
            string strKilohertz;
            strKilohertz = Globals.StripMode(strRDOFreq); // Strip off any mode designator
            int intHertz = Globals.KHzToHz(strKilohertz) - Conversions.ToInteger(strAudioCenterFrequency);
            switch (strRadioType)
            {
                case "Icom IC-M700pro":
                case "Icom IC-M710":
                case "Icom IC-M710RT":
                case "Icom IC-M802":
                case "Icom (other NMEA)":
                    {
                        SendCommand(ComputeNMEACommand("REMOTE,ON"));
                        SendCommand(ComputeNMEACommand("RXF," + Strings.Format(intHertz / (double)1000000, "#0.000000")));
                        SendCommand(ComputeNMEACommand("TXF," + Strings.Format(intHertz / (double)1000000, "#0.000000")));
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
            catch
            {
                Logs.Exception("[RadioIcom.SetFrequency] " + Information.Err().Description);
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
                        strHertz = Strings.Format(Conversions.ToInteger(intHertz), "000000000");
                        intPa1 = 16 * Conversions.ToInteger(strHertz.Substring(7, 1)) + Conversions.ToInteger(strHertz.Substring(8, 1));
                        intPa2 = 16 * Conversions.ToInteger(strHertz.Substring(5, 1)) + Conversions.ToInteger(strHertz.Substring(6, 1));
                        intPa3 = 16 * Conversions.ToInteger(strHertz.Substring(3, 1)) + Conversions.ToInteger(strHertz.Substring(4, 1));
                        intPa4 = 16 * Conversions.ToInteger(strHertz.Substring(1, 1)) + Conversions.ToInteger(strHertz.Substring(2, 1));
                        intPa5 = Conversions.ToInteger(strHertz.Substring(0, 1));
                        bytCommand = new byte[11];
                        bytCommand[0] = 0xFE;
                        bytCommand[1] = 0xFE;
                        bytCommand[2] = bytCIVAddress;
                        bytCommand[3] = 0xF1;
                        bytCommand[4] = 0x5; // Set frequency command
                        bytCommand[5] = Conversions.ToByte(intPa1);
                        bytCommand[6] = Conversions.ToByte(intPa2);
                        bytCommand[7] = Conversions.ToByte(intPa3);
                        bytCommand[8] = Conversions.ToByte(intPa4);
                        bytCommand[9] = Conversions.ToByte(intPa5);
                        bytCommand[10] = 0xFD;
                        SendCommand(bytCommand);
                        break;
                    }

                default:
                    {
                        strHertz = Strings.Format(Conversions.ToInteger(intHertz), "00000000");
                        intPa1 = 16 * Conversions.ToInteger(strHertz.Substring(6, 1)) + Conversions.ToInteger(strHertz.Substring(7, 1));
                        intPa2 = 16 * Conversions.ToInteger(strHertz.Substring(4, 1)) + Conversions.ToInteger(strHertz.Substring(5, 1));
                        intPa3 = 16 * Conversions.ToInteger(strHertz.Substring(2, 1)) + Conversions.ToInteger(strHertz.Substring(3, 1));
                        intPa4 = 16 * Conversions.ToInteger(strHertz.Substring(0, 1)) + Conversions.ToInteger(strHertz.Substring(1, 1));
                        bytCommand = new byte[10];
                        bytCommand[0] = 0xFE;
                        bytCommand[1] = 0xFE;
                        bytCommand[2] = bytCIVAddress;
                        bytCommand[3] = 0xF1;
                        bytCommand[4] = 0x5; // Set frequency command
                        bytCommand[5] = Conversions.ToByte(intPa1);
                        bytCommand[6] = Conversions.ToByte(intPa2);
                        bytCommand[7] = Conversions.ToByte(intPa3);
                        bytCommand[8] = Conversions.ToByte(intPa4);
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
                intCheckSum = intCheckSum ^ Strings.Asc(strBuffer.Substring(intIndex, 1));
            strCheckSum = "*" + Strings.Right("0" + Conversion.Hex(intCheckSum), 2);
            ComputeNMEACommandRet = strBuffer + strCheckSum + Constants.vbCrLf;
            return ComputeNMEACommandRet;
        } // ComputeNMEACommand

        private bool SendCommand(byte[] bytCommand)
        {
            // Sends a command to the radio...

            if (blnViaPTC)
            {
                string strBuffer = "";
                for (int intIndex = 0, loopTo = bytCommand.Length - 1; intIndex <= loopTo; intIndex++)
                    strBuffer = strBuffer + Strings.Right("00" + Conversion.Hex(bytCommand[intIndex]), 2);
                Globals.objSCSClient.SendRadioCommand("#TRX T " + strBuffer); // Use the Transfer capability of the TRX commands
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
                        strBuffer = strBuffer + Strings.Right("00" + Conversion.Hex(Strings.Asc(strCommand.Substring(intIndex, 1))), 2);
                    Globals.objSCSClient.SendRadioCommand("#TRX T " + strBuffer); // Use the Transfer capability of the TRX commands
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
            catch
            {
                Logs.Exception("[RadioIcom.SendCommand] " + Information.Err().Description);
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