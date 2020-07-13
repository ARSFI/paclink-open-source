using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public class RadioYaesu : IRadio
    {
        public RadioYaesu()
        {

            // Objects 
            objSerial = new SerialPort();
        }

        // Strings
        private string strRadioReply;

        // Booleans
        private bool blnViaPTC; // Holds a flag to indicate via the PTC else direct serial
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
                    _objSerial.DataReceived -= OnDataReceived;
                }

                _objSerial = value;
                if (_objSerial != null)
                {
                    _objSerial.DataReceived += OnDataReceived;
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
                Logs.Exception("[RadioYaesu.SetDtrControl] : " + ex.ToString());
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
                Logs.Exception("[RadioYaesu.SetRtsControl] : " + ex.ToString());
                return false;
            }
        }

        public bool SetPTT(bool Send)
        {
            // Only currently used for Icom radios
            return false;
        }

        public bool InitializeSerialPort(ref TChannelProperties Channel)
        {
            // Opens the serial port used to control the radio. Returns true if port opens...

            if (Channel.RDOControl == "Via PTCII")
            {
                blnViaPTC = true;
                if (Channel.TNCType == "PTC II")
                {
                    Globals.objSCSClient.SendRadioCommand("#TRX TY Y " + Channel.RDOControlBaud + " A");
                }
                else if (Channel.TTLLevel)
                {
                    Globals.objSCSClient.SendRadioCommand("#TRX TY Y " + Channel.RDOControlBaud + " A TTL");
                }
                else
                {
                    Globals.objSCSClient.SendRadioCommand("#TRX TY Y " + Channel.RDOControlBaud + " A V24");
                }

                Thread.Sleep(100);
                return true;
            }

            try
            {
                if (objSerial.IsOpen)
                {
                    objSerial.DiscardInBuffer();
                    objSerial.DiscardOutBuffer();
                    objSerial.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                }

                objSerial.WriteTimeout = 1000;
                objSerial.ReceivedBytesThreshold = 1;
                objSerial.BaudRate = Convert.ToInt32(Channel.RDOControlBaud);
                objSerial.DataBits = 8;
                objSerial.Parity = Parity.None;
                objSerial.StopBits = StopBits.Two;
                objSerial.PortName = Channel.RDOControlPort;
                objSerial.Handshake = Handshake.None;
                objSerial.RtsEnable = true;
                objSerial.DtrEnable = true;
                objSerial.NewLine = ";";
                objSerial.Open();
                objSerial.DiscardInBuffer();
                objSerial.DiscardOutBuffer();
                OpenRadio(Channel.RDOModel);
                return objSerial.IsOpen;
            }
            catch
            {
                Logs.Exception("[RadioYaesu.InitializeSerialPort] " + Information.Err().Description);
                return false;
            }
        } // InitializeSerialPort 

        private void OpenRadio(string strRadioType)
        {
            var bytCommand = new byte[5];
            switch (strRadioType)
            {
                case "Yaesu FT-1000":
                    {
                        bytCommand[0] = 0;
                        bytCommand[1] = 0;
                        bytCommand[2] = 0;
                        bytCommand[3] = 0x30;
                        bytCommand[4] = 0x75;
                        SendCommand(bytCommand); // Set the mode
                        break;
                    }

                case "Yaesu FT-450":
                case "Yaesu FT-950":
                case "Yaesu FT-2000":
                    {
                        SendCommand("IS0+0000;"); // Set i.f. shift to center
                        break;
                    }
            }
        }

        public bool SetParameters(ref TChannelProperties Channel)
        {
            // Function to set parameters for frequency and filters, possibly other parameters later...
            try
            {
                if (Channel.ChannelType == EChannelModes.PacketTNC)
                {
                    SetVHFFrequency(Globals.ExtractFreq(ref Channel.RDOCenterFrequency), Channel.RDOModel);
                }
                else
                {
                    SetHFFrequency(Globals.ExtractFreq(ref Channel.RDOCenterFrequency), Channel.AudioToneCenter, Channel.RDOModel);
                    SetFilter(Channel.NarrowFilter, Globals.ExtractFreq(ref Channel.RDOCenterFrequency), Channel.RDOModel, Channel.AudioToneCenter);
                }

                return true;
            }
            catch
            {
                Logs.Exception("[RadioYaesu.SetParameters] " + Information.Err().Description);
                return false;
            }
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
                Logs.Exception("[RadioYaesu.Close] " + Information.Err().Description);
            }
        } // Close 

        private bool SetHFFrequency(string strKilohertz, string strAudioCenterFrequency, string strRadioType)
        {
            // Set the radio's mode and frequency...

            var bytCommand = new byte[5];
            try
            {
                strKilohertz = Globals.StripMode(strKilohertz); // Strip off any mode designator
                int intHertz = Globals.KHzToHz(strKilohertz) - Convert.ToInt32(strAudioCenterFrequency);
                string strHertz = intHertz.ToString("000000000");
                switch (strRadioType)
                {
                    case "Yaesu FT-857":
                    case "Yaesu FT-857D":
                    case "Yaesu FT-897":
                        {
                            // Set digital mode...
                            if (strRadioType == "Yaesu FT-857D")
                            {
                                bytCommand[0] = 0xA;
                            }
                            else
                            {
                                bytCommand[0] = 1;
                            }

                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 7;
                            SendCommand(bytCommand);

                            // Set split off...
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 0x82;
                            SendCommand(bytCommand);

                            // Set RIT off...
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 0x85;
                            SendCommand(bytCommand);
                            bytCommand[0] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(0, 1)) * 16 + Convert.ToInt32(strHertz.Substring(1, 1)));
                            bytCommand[1] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(2, 1)) * 16 + Convert.ToInt32(strHertz.Substring(3, 1)));
                            bytCommand[2] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(4, 1)) * 16 + Convert.ToInt32(strHertz.Substring(5, 1)));
                            bytCommand[3] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(6, 1)) * 16 + Convert.ToInt32(strHertz.Substring(7, 1)));
                            bytCommand[4] = 1;
                            return SendCommand(bytCommand);
                        }

                    case "Yaesu FT-847":
                        {
                            // Set mode...USB main VFO
                            bytCommand[0] = 1;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 7;
                            SendCommand(bytCommand);
                            // Set Satellite mode off...
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 0x8E;
                            SendCommand(bytCommand);
                            // Set Simplex f...
                            bytCommand[0] = 0x89;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 0x9;
                            SendCommand(bytCommand);
                            // Set frequency
                            bytCommand[0] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(0, 1)) * 16 + Convert.ToInt32(strHertz.Substring(1, 1)));
                            bytCommand[1] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(2, 1)) * 16 + Convert.ToInt32(strHertz.Substring(3, 1)));
                            bytCommand[2] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(4, 1)) * 16 + Convert.ToInt32(strHertz.Substring(5, 1)));
                            bytCommand[3] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(6, 1)) * 16 + Convert.ToInt32(strHertz.Substring(7, 1)));
                            bytCommand[4] = 1;
                            return SendCommand(bytCommand);
                        }

                    case "Yaesu FT-450":
                    case "Yaesu FT-950":
                    case "Yaesu FT-2000":
                        {
                            try
                            {
                                string strFrequency = intHertz.ToString("00000000");
                                SendCommand("MD02;");                   // Set USB
                                SendCommand("FA" + strFrequency + ";"); // Set frequency
                            }
                            catch
                            {
                                Logs.Exception("[RadioYaesu.SetFrequency FT-2000] " + Information.Err().Description);
                                return false;
                            } // Earlier Yaesu radios...

                            break;
                        }

                    default:
                        {
                            // Select VFO-A
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 5;
                            SendCommand(bytCommand);

                            // Set USB...
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 1;
                            bytCommand[4] = 0xC;
                            SendCommand(bytCommand);

                            // Set frequency...
                            bytCommand[0] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(6, 1)) * 16 + Convert.ToInt32(strHertz.Substring(7, 1)));
                            bytCommand[1] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(4, 1)) * 16 + Convert.ToInt32(strHertz.Substring(5, 1)));
                            bytCommand[2] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(2, 1)) * 16 + Convert.ToInt32(strHertz.Substring(3, 1)));
                            bytCommand[3] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(0, 1)) * 16 + Convert.ToInt32(strHertz.Substring(1, 1)));
                            bytCommand[4] = 0xA;
                            SendCommand(bytCommand);
                            break;
                        }
                }
            }
            catch
            {
                return false;
            }

            return default;
        } // SetFrequency

        private bool SetVHFFrequency(string strKilohertz, string strRadioType)
        {
            // Set the radio's mode to FM and frequency...

            var bytCommand = new byte[5];
            try
            {
                int intHertz = Globals.KHzToHz(strKilohertz);
                string strHertz = intHertz.ToString("000000000");
                switch (strRadioType)
                {
                    case "Yaesu FT-857":
                    case "Yaesu FT-857D":
                        {
                            // Set FM
                            bytCommand[0] = 0x8;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 7;
                            SendCommand(bytCommand); // Set the mode

                            // Turn off split for 857...
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 0x82;
                            SendCommand(bytCommand);

                            // Set the frequency...
                            bytCommand[0] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(0, 1)) * 16 + Convert.ToInt32(strHertz.Substring(1, 1)));
                            bytCommand[1] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(2, 1)) * 16 + Convert.ToInt32(strHertz.Substring(3, 1)));
                            bytCommand[2] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(4, 1)) * 16 + Convert.ToInt32(strHertz.Substring(5, 1)));
                            bytCommand[3] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(6, 1)) * 16 + Convert.ToInt32(strHertz.Substring(7, 1)));
                            bytCommand[4] = 1;
                            return SendCommand(bytCommand);
                        }

                    case "Yaesu FT-847":
                        {
                            // Set mode...FM main VFO
                            bytCommand[0] = 0x88;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 7;
                            SendCommand(bytCommand);
                            // Set Satellite mode off...
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 0x8E;
                            SendCommand(bytCommand);
                            // Set Simplex ...
                            bytCommand[0] = 0x89;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 0x9;
                            SendCommand(bytCommand);
                            // Set frequency
                            bytCommand[0] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(0, 1)) * 16 + Convert.ToInt32(strHertz.Substring(1, 1)));
                            bytCommand[1] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(2, 1)) * 16 + Convert.ToInt32(strHertz.Substring(3, 1)));
                            bytCommand[2] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(4, 1)) * 16 + Convert.ToInt32(strHertz.Substring(5, 1)));
                            bytCommand[3] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(6, 1)) * 16 + Convert.ToInt32(strHertz.Substring(7, 1)));
                            bytCommand[4] = 1;
                            return SendCommand(bytCommand);
                        }

                    default:
                        {
                            bytCommand[0] = 0x8;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 7;
                            SendCommand(bytCommand);
                            bytCommand[0] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(0, 1)) * 16 + Convert.ToInt32(strHertz.Substring(1, 1)));
                            bytCommand[1] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(2, 1)) * 16 + Convert.ToInt32(strHertz.Substring(3, 1)));
                            bytCommand[2] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(4, 1)) * 16 + Convert.ToInt32(strHertz.Substring(5, 1)));
                            bytCommand[3] = Convert.ToByte(Convert.ToInt32(strHertz.Substring(6, 1)) * 16 + Convert.ToInt32(strHertz.Substring(7, 1)));
                            bytCommand[4] = 1;
                            return SendCommand(bytCommand);
                        }
                }
            }
            catch
            {
                return false;
            }
        } // SetFrequency

        private bool SetFilter(bool blnUseNarrow, string strRDOCenterFreq, string strRadioType, string strAudioCenterFrequency)
        {
            // Sets the radio's filter...

            var bytCommand = new byte[5];
            switch (strRadioType)
            {
                case "Yaesu FT-1000":
                    {
                        if (Globals.UseWideFilter(strRDOCenterFreq) | !blnUseNarrow)
                        {
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0x54;
                            bytCommand[4] = 0x8C;
                            SendCommand(bytCommand);
                        }
                        else
                        {
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0x52;
                            bytCommand[4] = 0x8C;
                            SendCommand(bytCommand);
                        }

                        break;
                    }

                case "Yaesu FT-920":
                    {
                        if (Globals.UseWideFilter(strRDOCenterFreq) | !blnUseNarrow)
                        {
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 0;
                            bytCommand[4] = 0x8C;
                            SendCommand(bytCommand);
                        }
                        else
                        {
                            bytCommand[0] = 0;
                            bytCommand[1] = 0;
                            bytCommand[2] = 0;
                            bytCommand[3] = 2;
                            bytCommand[4] = 0x8C;
                            SendCommand(bytCommand);
                        }

                        break;
                    }

                case "Yaesu FT-857":
                case "Yaesu FT-857D":
                case "Yaesu FT-897":
                    {
                        break;
                    }
                // No filter select for 857 per documentation

                case "Yaesu FT-450":
                case "Yaesu FT-950":
                case "Yaesu FT-2000":
                    {
                        if (Globals.UseWideFilter(strRDOCenterFreq) | !blnUseNarrow)
                        {
                            if (!SendCommand("NA00;"))
                                return false; // Set wide filter
                        }
                        else if (!SendCommand("NA01;"))
                            return false; // Set narrow filter
                        break;
                    }
            }

            return default;
        } // SetFilter

        private bool SendCommand(byte[] bytCommand)
        {
            // Sends a command to the radio...
            Debug.WriteLine(Globals.GetString(bytCommand));
            if (blnViaPTC)
            {
                string strBuffer = "";
                for (int intIndex = 0, loopTo = bytCommand.Length - 1; intIndex <= loopTo; intIndex++)
                    strBuffer = strBuffer + "00" + Conversion.Hex(bytCommand[intIndex]).Right(2);
                Globals.objSCSClient.SendRadioCommand("#TRX T " + strBuffer); // Use the Transfer capability of the TRX commands
            }
            else
            {
                objSerial.Write(bytCommand, 0, bytCommand.Length);
            }

            Thread.Sleep(100); // Delay for radio commands 
            return true;
        } // SendCommand (Byte())

        private bool SendCommand(string strCommand)
        {
            Debug.WriteLine(strCommand);
            // Sends a command to the radio...
            try
            {
                // Could later add readback to confirm 
                if (blnViaPTC)
                {
                    // Must convert string back to ASCII (from Han's code)...
                    string strTemp = "";
                    for (int intIndex = 0, loopTo = strCommand.Length - 1; intIndex <= loopTo; intIndex++)
                        strTemp = strTemp + ("00" + Conversion.Hex(Strings.Asc(strCommand.Substring(intIndex, 1)))).Right(2);
                    Globals.objSCSClient.SendRadioCommand("#TRX T " + strTemp); // Use the Transfer capability of the TRX commands
                }
                else
                {
                    // Use byte write to handle character values > 128
                    var objEncoder = new ASCIIEncoding();
                    objSerial.Write(Globals.GetBytes(strCommand), 0, strCommand.Length);
                }

                Thread.Sleep(100); // Delay for radio commands 
                return true;
            }
            catch
            {
                Logs.Exception("[RadioYaesu.SendCommand] " + Information.Err().Description);
                Logs.Exception("[RadioYaesu.SendCommand] " + strCommand);
                return false;
            }
        } // SendCommand (String)

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Receives data from the radio's serial port...
            int intBytesToRead = objSerial.BytesToRead;
            strRadioReply = objSerial.ReadExisting();
        } // OnDataReceived
    }
}