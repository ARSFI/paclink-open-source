using System;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Threading;
using NLog;

namespace Paclink
{
    public class RadioKenwood : IRadio
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public RadioKenwood()
        {

            // Objects 
            objSerial = new SerialPort();
        }

        // Strings
        private string strRadioReply = "";
        private string strTrace = ""; // Used for debugging 

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
                _log.Error("[RadioKenwood.SetDtrControl] : " + ex.ToString());
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
                _log.Error("[RadioDenwood.SetRtsControl] : " + ex.ToString());
                return false;
            }
        }

        public bool SetPTT(bool Send)
        {
            // Only currently used for Icom radios
            return false;
        }

        public bool InitializeSerialPort(ref TChannelProperties stcChannel)
        {
            // Opens the serial port used to control the radio. Returns true if port opens...

            if (stcChannel.RDOControl == "Via PTCII")
            {
                blnViaPTC = true;
                if (stcChannel.TNCType == "PTC II")
                {
                    Globals.ObjScsModem.SendRadioCommand("#TRX TY K " + stcChannel.RDOControlBaud + " A");
                }
                else if (stcChannel.TTLLevel)
                {
                    Globals.ObjScsModem.SendRadioCommand("#TRX TY K " + stcChannel.RDOControlBaud + " A TTL");
                }
                else
                {
                    Globals.ObjScsModem.SendRadioCommand("#TRX TY K " + stcChannel.RDOControlBaud + " A V24");
                }

                Thread.Sleep(200);
                return true;
            }

            try
            {
                if (objSerial is object)
                {
                    if (objSerial.IsOpen)
                    {
                        objSerial.DiscardInBuffer();
                        objSerial.DiscardOutBuffer();
                        objSerial.Close(); // Close the serial port if already open
                        Thread.Sleep(Globals.intComCloseTime);
                    }

                    objSerial.WriteTimeout = 1000;
                    objSerial.ReceivedBytesThreshold = 1;         // Minimum of 1 bytes for interrupt 
                    objSerial.BaudRate = Convert.ToInt32(stcChannel.RDOControlBaud);
                    objSerial.DataBits = 8;
                    objSerial.Parity = Parity.None;
                    objSerial.StopBits = StopBits.Two; // two stop bits for TS-440, 450, 690
                    objSerial.PortName = stcChannel.RDOControlPort;
                    objSerial.Handshake = Handshake.None;
                    objSerial.RtsEnable = true;
                    objSerial.DtrEnable = true;
                    objSerial.NewLine = ";";
                    objSerial.Open();
                    objSerial.DiscardInBuffer();
                    objSerial.DiscardOutBuffer();
                    return objSerial.IsOpen;
                }
            }
            catch (Exception ex)
            {
                _log.Error("[RadioKenwood.InitializeSerialPort] " + ex.Message);
                return false;
            }

            return default;
        }   // InitializeSerialPort *

        public bool SetParameters(ref TChannelProperties stcChannel)
        {
            // Function to set parameters for frequency and filters, possibly other parameters later...

            if (stcChannel.ChannelType == EChannelModes.PacketTNC)
            {
                try
                {
                    if (!SetFMModeFrequency("A", Globals.ExtractFreq(ref stcChannel.RDOCenterFrequency), stcChannel.RDOModel))
                        return false;
                    strTrace = "";
                }
                catch (Exception ex)
                {
                    _log.Error("[KenwoodRadio.SetParameters] Packet Channel:  " + ex.Message);
                    return false;
                }
            }
            else
            {
                try
                {
                    SetFrequency("A", Globals.ExtractFreq(ref stcChannel.RDOCenterFrequency), stcChannel.AudioToneCenter, stcChannel.RDOModel);
                    if (!SetFilter(stcChannel.NarrowFilter, Globals.ExtractFreq(ref stcChannel.RDOCenterFrequency), stcChannel.RDOModel, stcChannel.AudioToneCenter))
                        return false;
                    strTrace = "";
                }
                catch (Exception ex)
                {
                    _log.Error("[KenwoodRadio.SetParameters] " + ex.Message);
                    return false;
                }
            }

            return true;
        } // SetParameters *

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

                    objSerial = null;
                }
            }
            catch (Exception ex)
            {
                _log.Error("[RadioKenwood.Close] " + ex.Message);
            }
        } // Close *

        private bool SetFrequency(string strVFO, string strKilohertz, string strAudioCenterFrequency, string strRadioType)
        {
            // Sets the radio's frequency...

            // Verified with TS-480 and TS-2000
            string strCommand;
            strKilohertz = Globals.StripMode(strKilohertz); // Strip off any mode designator
            int intHertz = Globals.KHzToHz(strKilohertz) - Convert.ToInt32(strAudioCenterFrequency);
            SendCommand("PR0;"); // Turn off any speech processor (not used on all models)
            try
            {
                string strHertz = intHertz.ToString("00000000000");
                switch (strRadioType)
                {
                    case "Kenwood TS-450":
                    case "Kenwood TS-690":
                        {
                            try
                            {
                                if (!SendCommand("FR0;"))
                                    return false; // Set VFO A receive
                                if (!SendCommand("FT0;"))
                                    return false; // Set VFO A transmit
                                strCommand = "F" + strVFO + strHertz + ";";
                                SendCommand(strCommand);
                                return SendCommand("MD2;");  // Set USB
                            }
                            catch (Exception ex)
                            {
                                _log.Error("[KenwoodRadio.SetFrequency TS-450/690] " + ex.Message);
                                return false;
                            }

                            break;
                        }

                    default:
                        {
                            try
                            {
                                if (!SendCommand("FR0;"))
                                    return false; // Set VFO A
                                strCommand = "F" + strVFO + strHertz + ";";
                                SendCommand(strCommand);
                                return SendCommand("MD2;");
                            }
                            catch (Exception ex)
                            {
                                _log.Error("[KenwoodRadio.SetFrequency Kenwood(generic)] " + ex.Message);
                                return false;
                            }

                            break;
                        }
                }
            }
            catch
            {
                return false;
            }

            return false;
        } // SetFrequency

        private bool SetFilter(bool blnUseNarrow, string strRDOCenterFreq, string strRadioType, string strAudioCenterFrequency)
        {
            // Sets the radio's filter...

            DateTime dttTimeout;
            switch (strRadioType)
            {
                case "Kenwood TS-2000": // Verified on TS-2000 with Audio center = 1500
                    {
                        try
                        {
                            if (Globals.UseWideFilter(strRDOCenterFreq) | !blnUseNarrow)
                            {
                                // Set the wide filter
                                int intHighCO = 1200 + Convert.ToInt32(strAudioCenterFrequency);
                                int intLowCO = Convert.ToInt32(strAudioCenterFrequency) - 1200;
                                int intIndex = intHighCO / 200 - 7;
                                SendCommand("SH" + intIndex.ToString("00") + ";"); // 1200 Hz above Audio Center
                                intIndex = Math.Min(10, intLowCO / 100) + 1;
                                return SendCommand("SL" + intIndex.ToString("00") + ";"); // 1200 Hz below Audio Center
                            }
                            // set the narrow filter
                            else
                            {
                                int intHighCO = 300 + Convert.ToInt32(strAudioCenterFrequency);
                                int intLowCO = Convert.ToInt32(strAudioCenterFrequency) - 300;
                                int intIndex = intHighCO / 200 - 7;
                                SendCommand("SH" + intIndex.ToString("00") + ";"); // 300 Hz above Audio Center
                                intIndex = Math.Min(10, intLowCO / 100) + 1;
                                return SendCommand("SL" + intIndex.ToString("00") + ";");
                            } // 300 Hz below Audio Center
                        }
                        catch (Exception ex)
                        {
                            _log.Error("[KenwoodRadio.SetFilter TS-2000] " + ex.Message);
                        }

                        break;
                    }

                case "Kenwood TS-450":
                case "Kenwood TS-690":
                    {
                        try
                        {
                            dttTimeout = DateTime.Now;
                            strRadioReply = "";
                            // While strRadioReply.IndexOf("FL") = -1
                            // If dttTimeout.AddSeconds(3) < Now Then
                            // Log.Error("[RadioKenwood.SetFilter] No reply to FL; command from " & strRadioType)
                            // Return False
                            // End If
                            // SendCommand("FL;")
                            // Thread.Sleep(200)
                            // End While
                            SendCommand("FL;");
                            Thread.Sleep(200);
                            if (Globals.UseWideFilter(strRDOCenterFreq) | !blnUseNarrow)
                            {
                                return SendCommand("FL007007;"); // 2.4 Khz for 8Mhz and 455 Khz
                            }
                            else
                            {
                                return SendCommand("FL007009;");
                            } // 2.4 Khz for 8Mhs and 500 Hz for 455 Khz
                        }
                        catch (Exception ex)
                        {
                            _log.Error("[KenwoodRadio.SetFilter TS-450/690] " + ex.Message);
                        }

                        break;
                    }

                case "Kenwood TS-480":
                case "Kenwood (other)":
                    {
                        // Used to set the filter...may have to look at radio type (Kenwood model)
                        // Verified OK with TS-480
                        try
                        {
                            dttTimeout = DateTime.Now;
                            strRadioReply = "";
                            // While strRadioReply.IndexOf("FW") = -1
                            // If dttTimeout.AddSeconds(3) < Now Then
                            // Log.Error("[RadioKenwood.SetFilter] No reply to FW; command from " & strRadioType)
                            // Return False
                            // End If
                            // SendCommand("FW;")
                            // Thread.Sleep(200)
                            // End While

                            SendCommand("FW;");
                            Thread.Sleep(200);
                            if (Globals.UseWideFilter(strRDOCenterFreq) | !blnUseNarrow)
                            {
                                return SendCommand("FW0000;"); // set  normal filter
                            }
                            else
                            {
                                return SendCommand("FW0001;");
                            } // set narrow filter
                        }
                        catch (Exception ex)
                        {
                            _log.Error("[KenwoodRadio.SetFilter TS-480/Other] " + ex.Message);
                        }

                        break;
                    }
            }

            return default;
        } // SetFilter

        private bool SendCommand(string strCommand)
        {
            // Sends a command to the radio...

            // Could later add readback to confirm... 
            try
            {
                if (blnViaPTC)
                {
                    Globals.ObjScsModem.SendRadioCommand("#TRX T " + strCommand); // Use the Transfer capability of the TRX commands
                    strTrace = strTrace + "#TRX T " + strCommand;
                }
                else
                {
                    objSerial.Write(strCommand);
                    strTrace = strTrace + strCommand + Globals.CR;
                }

                Thread.Sleep(100);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("[RadioKenwood.SendCommand] " + ex.Message);
                return false;
            }
        } // SendCommand

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Receives data from the radio's serial port...

            int intBytesToRead = objSerial.BytesToRead;
            strRadioReply = objSerial.ReadExisting();
        } // OnDataReceived

        private bool SetFMModeFrequency(string strVFO, string strKilohertz, string strRadioType)
        {
            // Sets the radio's frequency and mode to FM for packet...


            string strCmd = "";
            int intHertz = Globals.KHzToHz(strKilohertz);
            try
            {
                string strFreqHz = intHertz.ToString("00000000000");
                switch (strRadioType)
                {
                    case "Kenwood TS-2000":
                        {
                            try
                            {
                                if (!SendCommand("FR0;"))
                                    return false; // Set VFO A receive
                                if (!SendCommand("FT0;"))
                                    return false; // Set VFO A transmit
                                if (!SendCommand("MD4;"))
                                    return false; // Set FM
                                if (!SendCommand("SH11;"))
                                    return false; // Set high filter to 5000
                                if (!SendCommand("SL00;"))
                                    return false; // Set low filter to 10Hz
                                strCmd = "F" + strVFO + strFreqHz + ";";
                                return SendCommand(strCmd);
                            }
                            catch (Exception ex)
                            {
                                _log.Error("[KenwoodRadio.SetFMModeFrequency TS-2000] :" + ex.Message);
                                return false;
                            }

                            break;
                        }

                    case "Kenwood TM-D710":
                        {
                            // 1/25/09 This appears to work OK on the D710. No setting of power but sets frequency
                            // and move PTT and CTRL over to correct A or B band. R.M. 
                            // check to see if freq is 136 - 199 MHz (A Band) or 
                            // or 400 - 534 (B band) 
                            if (intHertz >= 136000000 & intHertz <= 199000000) // A Band
                            {
                                SendCommand("BC 0,0" + Globals.CR);
                                // Set A Vfo, 0 offset, 5 KHz steps  (default)
                                strCmd = "FO 0,0" + intHertz.ToString() + ",0,0,0,0,0,0,08,08,000,00600000,0" + Globals.CR;
                            }
                            else if (intHertz >= 400000000 & intHertz <= 534000000) // B Band
                            {
                                SendCommand("BC 1,1" + Globals.CR);
                                // Set B Vfo, 0 offset, 25 KHz steps (default)
                                strCmd = "FO 1,0" + intHertz.ToString() + ",7,0,0,0,0,0,08,08,000,05000000,0" + Globals.CR;
                            }

                            return SendCommand(strCmd);
                        }

                    case "Kenwood TM-D700":
                    case "Kenwood TH-D7":
                        {
                            // this cannot be used currently if the  internal TNC is used on the D700 or D7
                            // 1/25/09 This appears to work OK on the D710. No setting of power but sets frequency
                            // and move PTT and CTRL over to correct A or B band. R.M. 
                            // check to see if freq is 136 - 199 MHz (A Band) or 
                            // or 400 - 534 (B band) 
                            if (intHertz >= 136000000 & intHertz <= 199000000) // A Band
                            {
                                SendCommand("BC 0,0" + Globals.CR);
                                // Set A Vfo, 0 offset, 5 KHz steps  (default)
                                strCmd = "FO 0,0" + intHertz.ToString() + ",0,0,0,0,0,0,08,08,000,00600000,0" + Globals.CR;
                            }
                            else if (intHertz >= 400000000 & intHertz <= 534000000) // B Band
                            {
                                SendCommand("BC 1,1" + Globals.CR);
                                // Set B Vfo, 0 offset, 25 KHz steps (default)
                                strCmd = "FO 1,0" + intHertz.ToString() + ",7,0,0,0,0,0,08,08,000,05000000,0" + Globals.CR;
                            }

                            return SendCommand(strCmd);
                        }
                }
            }
            catch
            {
                return false;
            }

            return default;
        } // SetFMModeFrequency
    }
}