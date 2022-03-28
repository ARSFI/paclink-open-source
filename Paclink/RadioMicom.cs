using System;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using NLog;

namespace Paclink
{
    public class RadioMicom : IRadio
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        // Strings
        private string strIcomAddress;
        private string strRadioResponse;

        // Booleans
        private bool blnViaPTC;

        // **********************************************************************************************************
        // Open serial port and establish communications with the radio. Do initial radio setup.
        // 
        // Return true on success of false on failure
        // **********************************************************************************************************
        public bool InitializeSerialPort(ref ChannelProperties stcTNC)
        {
            int retc;
            try
            {
                if (MICOMOpenPort(ref stcTNC.RDOControlPort, Convert.ToInt32(stcTNC.RDOControlBaud)) != 0) // Com port open worked
                {
                    // *****************************************************************
                    // * Perform initial radio setup.
                    // 1 set the radio into frequency mode vs channel mode
                    // 2 check to make sure it is in USB mode and report an error if not
                    // 3 turn off clarifier to prevent hard to find problems 
                    // *****************************************************************

                    // Put the radio into frequency mode...
                    byte argMode = 2;
                    retc = MICOMSetMode(ref argMode);
                    if (retc == 0)           // had a problem with the command
                    {
                        if (Timer2Popped == true)
                        {
                            _log.Error("[RadioMicom.InitializeSerialPort] " + "Radio response timeout. Check radio power and connection");
                        }
                        else
                        {
                            _log.Error("[RadioMicom.InitializeSerialPort] " + "MICOM Set MOde Command failed");
                        }

                        return false;
                    }
                    else
                    {
                        retc = MICOMRptSSBState();
                        if (retc != 2)
                        {
                            MessageBox.Show("Radio must be in USB mode...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _log.Error("[RadioMicom.InitializeSerialPort] " + "MICOM not in USB mode");
                            return false;
                        }
                        else
                        {
                            byte argClarifierState = 0;
                            retc = MICOMSetClarifier(ref argClarifierState);
                            if (retc == 0)
                            {
                                _log.Error("[RadioMicom.InitializeSerialPort] " + "Set Clarifier off failed");
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    _log.Error("[RadioMicom.InitializeSerialPort] " + "Serial port open failed: " + stcTNC.RDOControlPort);
                    return false;
                }
            }
            catch (Exception e)
            {
                _log.Error("[RadioMicom.InitializeSerialPort] " + e.Message);
                return false;
            }
        }   // InitializeSerialPort 

        public bool SetDtrControl(bool Dtr)
        {
            try
            {
                objSerial.DtrEnable = Dtr;
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("[RadioMicom.SetDtrControl] : " + ex.ToString());
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
                _log.Error("[RadioMicom.SetRtsControl] : " + ex.ToString());
                return false;
            }
        }

        public bool SetPTT(bool Send)
        {
            // Only currently used for Icom radios
            return false;
        }

        public bool SetParameters(ref ChannelProperties stcTNC)
        {
            bool SetParametersRet = default;
            // Function to set parameters for frequency and filters, possibly other parameters later...
            SetParametersRet = true;
            try
            {
                string strKilohertz = Globals.StripMode(stcTNC.RDOCenterFrequency); // Strip off any mode designator
                int intHertz = Globals.KHzToHz(strKilohertz) - Convert.ToInt32(stcTNC.AudioToneCenter);
                if (!SetFrequency(intHertz))
                {
                    SetParametersRet = false;
                }
            }
            catch
            {
                SetParametersRet = false;
            }

            return SetParametersRet;
        } // SetParameters 

        public void Close()
        {
            // Closes and disposes of the radio's serial port...
            try
            {
                MICOMPortClose();
            }
            catch (Exception e)
            {
                _log.Error("[RadioMicom.Close] " + e.Message);
            }
        } // Close 

        private bool SetFrequency(int intHertz)
        {
            int retc;
            try
            {
                retc = MICOMSetRXFreq(intHertz);
                if (retc != 0)                   // If it worked
                {
                    retc = MICOMSetTXFreq(intHertz);
                    if (retc != 0)                // If it worked
                    {
                        return true;
                    }
                }

                _log.Error("[RadioMicom.SetFrequency]" + " Failed");
                return false;
            }
            catch (Exception e)
            {
                _log.Error("[RadioMicom.SetFrequency] " + e.Message);
                return false;
            }

            return true;
        } // SetFrequency

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
                }

                _objSerial = value;
                if (_objSerial != null)
                {
                }
            }
        }

        private System.Timers.Timer _MiTimer;

        private System.Timers.Timer MiTimer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _MiTimer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_MiTimer != null)
                {

                    // ********************************************************************************
                    // Timer event handler
                    // *******************************************************************************
                    _MiTimer.Elapsed -= MiTimer_Elapsed;
                }

                _MiTimer = value;
                if (_MiTimer != null)
                {
                    _MiTimer.Elapsed += MiTimer_Elapsed;
                }
            }
        }

        private static short SizeofACK = 6;
        private const short SizeofSSBRpt = 7;
        private const short SizeofFrpt = 10;
        private static byte FrmTo = 0x18;
        private bool Timer2Popped;

        // ***********************************************************************
        // Set receive frequency in HZ. Input is an unsigned 32-bit binary number
        // that represents the desired frequency in HZ. Range: 100 kHz to 30 mHz.
        // Returns the set frequency or 0 on failure.
        // ***********************************************************************
        private int MICOMSetRXFreq(int RXFreq)
        {
            int MICOMSetRXFreqRet = default;
            byte DataLen = 6; // 0x06
            byte OpCode = 0x5; // 5 '0x05
            const byte RptReq = 0; // 0x00
                                   // Length of an ACk message

            byte rc;
            var freqval = new byte[4];
            var Mydata = new byte[5];
            var MyResult = new byte[SizeofACK + 1];
            if (RXFreq > 100000 & RXFreq < 30000001)
            {
                rc = LongToLine(ref RXFreq, ref freqval); // convert input freq to 4-byte big endian
                Mydata[0] = RptReq;
                Mydata[1] = freqval[0];
                Mydata[2] = freqval[1];
                Mydata[3] = freqval[2];
                Mydata[4] = freqval[3];
                rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
                if (rc != 0)
                {
                    rc = GetMICOMData(ref RadioMicom.SizeofACK, ref MyResult); // Get the response from MICOM
                    if (rc != 0)
                    {
                        MICOMSetRXFreqRet = RXFreq;
                    }
                    else
                    {
                        MICOMSetRXFreqRet = 0;
                    }
                }
                else
                {
                    MICOMSetRXFreqRet = 0;
                }
            }
            else
            {
                MICOMSetRXFreqRet = 0;
            }

            return MICOMSetRXFreqRet;
        }

        // ***********************************************************************
        // Set traansmit frequency in HZ. Input is an unsigned 32-bit binary number
        // that represents the desired frequency in HZ. Range: 1.6 mHz to 30 mHz.
        // Returns the set frequency or 0 on failure.
        // ***********************************************************************
        private int MICOMSetTXFreq(int TXFreq)
        {
            int MICOMSetTXFreqRet = default;
            byte DataLen = 6; // 0x06
            byte OpCode = 7; // 0x07
            const byte RptReq = 0; // 0x00
            byte rc;
            var freqval = new byte[4];
            var Mydata = new byte[5];
            var Result = new byte[SizeofACK + 1];
            if (TXFreq > 1600000 & TXFreq < 30000001)
            {
                rc = LongToLine(ref TXFreq, ref freqval); // convert input freq to 4-byte big endian
                Mydata[0] = RptReq;
                Mydata[1] = freqval[0];
                Mydata[2] = freqval[1];
                Mydata[3] = freqval[2];
                Mydata[4] = freqval[3];
                rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
                if (rc != 0)
                {
                    rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result); // Get the response from MICOM
                    if (rc != 0)
                    {
                        MICOMSetTXFreqRet = TXFreq;
                    }
                    else
                    {
                        MICOMSetTXFreqRet = 0;
                    }
                }
                else
                {
                    MICOMSetTXFreqRet = 0;
                }
            }
            else
            {
                MICOMSetTXFreqRet = 0;
            }

            return MICOMSetTXFreqRet;
        }

        // ***************************************************************************
        // * Set MICOM Side Band
        // * ARG = 1 = USB = 0 = LSB
        // * returns 1 = USB 2 = LSB 0 = error
        // ***************************************************************************
        private byte MICOMSetSSBState(ref byte SSBState)
        {
            byte MICOMSetSSBStateRet = default;
            byte DataLen = 3; // 0x03
            byte OpCode = 3; // 0x03
            var Mydata = new byte[3];
            var Result = new byte[SizeofACK + SizeofSSBRpt + 1];
            byte rc;
            Mydata[0] = 0; // Don't Get SSB Report
            Mydata[1] = SSBState; // 1 = USB 0 = LSB
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result);
                // If Result(9) = OpCode Then
                // MICOMSetSSBState = Result(9) + 1 ' return 1 for LSB or 2 for USB
                // Else
                // MICOMSetSSBState = 0            ' fail
                // End If
                MICOMSetSSBStateRet = 1; // pass
            }
            else
            {
                MICOMSetSSBStateRet = 0;
            } // fail

            return MICOMSetSSBStateRet;
        }

        // **************************************************************************
        // * Report active side band
        // * returns 2 = USB, 1 = LSB, 0 = Error
        // **************************************************************************
        // ***************************************************************************
        // * Set MICOM Side Band
        // * ARG = 1 = USB = 0 = LSB
        // * returns 2 = USB 1 = LSB 0 = error
        // ***************************************************************************
        private byte MICOMRptSSBState()
        {
            byte MICOMRptSSBStateRet = default;
            byte DataLen = 1; // 0x01
            byte OpCode = 4; // 0x04
            var Mydata = new byte[3];
            var Result = new byte[SizeofACK + SizeofSSBRpt + 1];
            byte rc;
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                short argNRBytes = Convert.ToInt16(SizeofACK + SizeofSSBRpt);
                rc = GetMICOMData(ref argNRBytes, ref Result);
                if (Result[9] == OpCode - 1)
                {
                    MICOMRptSSBStateRet = Convert.ToByte(Result[10] + Convert.ToByte(1)); // return 1 for LSB or 2 for USB
                }
                else
                {
                    MICOMRptSSBStateRet = 0;
                } // fail
            }
            else
            {
                MICOMRptSSBStateRet = 0;
            } // fail

            return MICOMRptSSBStateRet;
        }

        // **************************************************************************
        // * Set MICOM Squelch state
        // * ARG = 1 = on = 0 = off
        // **************************************************************************
        private byte MICOMSetSquelch(ref byte SQState)
        {
            byte MICOMSetSquelchRet = default;
            byte DataLen = 3; // 0x03
            byte OpCode = 9; // 0x09
            var Mydata = new byte[3];
            var Result = new byte[SizeofACK + 1];
            byte rc;
            Mydata[0] = 0; // No Report required
            Mydata[1] = SQState; // 1 = on 0 = off
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result);
                MICOMSetSquelchRet = rc; // Pass or fail
            }
            else
            {
                MICOMSetSquelchRet = 0;
            } // fail

            return MICOMSetSquelchRet;
        }

        // **************************************************************************
        // * Set MICOM Noise blanker state
        // * ARG = 1 = on = 0 = off
        // **************************************************************************
        private byte MICOMSetNB(ref byte NBState)
        {
            byte MICOMSetNBRet = default;
            byte DataLen = 6; // 0x06
            byte OpCode = 0x15; // 21 '0x15
            var Mydata = new byte[6];
            var Result = new byte[SizeofACK + 1];
            byte rc;
            Mydata[0] = 0; // No Report required
            Mydata[1] = 0; // Notch filter off
            Mydata[2] = 0; // Clipper off
            Mydata[3] = NBState; // 1 = on 0 = off
            Mydata[4] = 0xFF; // 255 ' No Change
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result);
                MICOMSetNBRet = rc; // Pass or fail
            }
            else
            {
                MICOMSetNBRet = 0;
            } // fail

            return MICOMSetNBRet;
        }

        // **************************************************************************
        // * Set MICOM Attenuator state
        // * ARG = 1 = on = 0 = off
        // **************************************************************************
        private byte MICOMSetAten(ref byte AtenState)
        {
            byte MICOMSetAtenRet = default;
            const byte DataLen = 6; // 0x06
            const byte OpCode = 0x15; // 21 '0x15
            var Mydata = new byte[6];
            var Result = new byte[SizeofACK + 1];
            byte rc;
            Mydata[0] = 0; // No Report required
            Mydata[1] = 0; // Notch filter off
            Mydata[2] = 0; // Clipper off
            Mydata[3] = 0xFF;
            Mydata[4] = AtenState;
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result);
                MICOMSetAtenRet = rc; // Pass or fail
            }
            else
            {
                MICOMSetAtenRet = 0;
            } // fail

            return MICOMSetAtenRet;
        }

        // **************************************************************************
        // * Set MICOM AGC State
        // * ARG = 0 = slow = 1`= fast
        // **************************************************************************
        private byte MICOMSetAGC(ref byte AGCState)
        {
            byte MICOMSetAGCRet = default;
            byte DataLen = 4; // 0x04
            byte OpCode = 0xD; // 13 '0x0D
            var Mydata = new byte[5];
            var Result = new byte[SizeofACK + 1];
            byte rc;
            Mydata[0] = 0; // No Report required
            Mydata[1] = 0; // SSB 2700
            Mydata[2] = AGCState; // 0 = Slow 1 = fast
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result);
                MICOMSetAGCRet = rc; // Pass or fail
            }
            else
            {
                MICOMSetAGCRet = 0;
            }

            return MICOMSetAGCRet;
        }

        // *************************************************************************
        // * Set MICOM Radio Op
        // * This command is used to set "frquency Mode ARG = 2"
        // *************************************************************************
        private byte MICOMSetMode(ref byte Mode)
        {
            byte MICOMSetModeRet = default;
            const byte DataLen = 3; // 0x03
            const byte OpCode = 0x17; // 23 '0x17
            var Mydata = new byte[3];
            var Result = new byte[SizeofACK + 1];
            byte rc;
            Mydata[0] = 0; // No Report required
            Mydata[1] = Mode; // Mode: 0=ALE Channel Mode 1=ALE Net scan 2=Freq Mode 3=channel mode
                              // 4 = channel scan mode

            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result);
                MICOMSetModeRet = rc; // Pass or fail
            }
            else
            {
                MICOMSetModeRet = 0;
            } // fail

            return MICOMSetModeRet;
        }

        // *************************************************************************
        // * Set MICOM Clarifier state
        // * arg = 0: clarifier off else clairifier on
        // * This command included so the program can ensure the clarifier is off.
        // *************************************************************************
        private byte MICOMSetClarifier(ref byte ClarifierState)
        {
            byte MICOMSetClarifierRet = default;
            const byte DataLen = 3; // 0x03
            const byte OpCode = 0xF; // 15 '0x0F
            var Mydata = new byte[3];
            var Result = new byte[SizeofACK + 1];
            byte rc;
            Mydata[0] = 0; // No Report required
            Mydata[1] = 0; // Clarifier value
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result);
                MICOMSetClarifierRet = rc; // Pass or fail
            }
            else
            {
                MICOMSetClarifierRet = 0;
            } // fail

            return MICOMSetClarifierRet;
        }

        // *************************************************************************
        // * Set MICOM transmitter power
        // * ARG is a value 0f 25, 62, 100, or 125  It is a 16-bit binary value.
        // *************************************************************************
        private byte MICOMSetTXPwr(ref short PWR)
        {
            byte MICOMSetTXPwrRet = default;
            const byte DataLen = 4; // 0x04
            const byte OpCode = 0x11; // 17 '0x11
            var Mydata = new byte[3];
            var Result = new byte[SizeofACK + 1];
            byte rc;
            Mydata[0] = 0; // No Report required
            Mydata[1] = Convert.ToByte(PWR >> 8);  // Power out 25,62,100 or 125
            Mydata[2] = Convert.ToByte(PWR & 0xFF);
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                rc = GetMICOMData(ref RadioMicom.SizeofACK, ref Result);
                MICOMSetTXPwrRet = rc; // Pass or fail
            }
            else
            {
                MICOMSetTXPwrRet = 0;
            } // fail

            return MICOMSetTXPwrRet;
        }

        // *************************************************************************
        // * Report MICOM receiver Freq
        // *************************************************************************
        private int MICOMRptRXFreq()
        {
            int MICOMRptRXFreqRet = default;
            const byte DataLen = 1; // 0x01
            const byte OpCode = 6; // 0x06
            byte rc;
            var freqval = new byte[4];
            var Mydata = new byte[2];
            var WrkData = new byte[4];
            var Result = new byte[SizeofACK + SizeofFrpt + 1];
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                short argNRBytes = Convert.ToInt16(SizeofACK + SizeofFrpt);
                rc = GetMICOMData(ref argNRBytes, ref Result);
                if (rc != 0)
                {
                    if (Result[9] == OpCode - 1) // check for proper opcode in response
                    {
                        WrkData[0] = Result[10]; // extract the reported frequency
                        WrkData[1] = Result[11]; // in line format
                        WrkData[2] = Result[12];
                        WrkData[3] = Result[13];
                        MICOMRptRXFreqRet = LineToLong(ref WrkData); // convert from line format to 32-bit binary value
                    }
                    else
                    {
                        MICOMRptRXFreqRet = 0;
                    }
                }
                else
                {
                    MICOMRptRXFreqRet = 0;
                }
            }
            else
            {
                MICOMRptRXFreqRet = 0;
            }

            return MICOMRptRXFreqRet;
        }

        // *************************************************************************
        // * Report MICOM transmitter Freq.
        // *************************************************************************
        private int MICOMRptTXFreq()
        {
            int MICOMRptTXFreqRet = default;
            const byte DataLen = 1; // 0x01
            const byte OpCode = 8; // 0x08
            byte rc;
            var freqval = new byte[4];
            var Mydata = new byte[2];
            var WrkData = new byte[4];
            var Result = new byte[SizeofACK + SizeofFrpt + 1];
            rc = Cmd2MICOM(DataLen, RadioMicom.FrmTo, OpCode, Mydata); // send command to MICOM
            if (rc != 0)
            {
                short argNRBytes = Convert.ToInt16(SizeofACK + SizeofFrpt);
                rc = GetMICOMData(ref argNRBytes, ref Result);
                if (rc != 0)
                {
                    if (Result[9] == OpCode - 1) // check for proper opcode in response
                    {
                        WrkData[0] = Result[10]; // extract the reported frequency
                        WrkData[1] = Result[11]; // in line format
                        WrkData[2] = Result[12];
                        WrkData[3] = Result[13];
                        MICOMRptTXFreqRet = LineToLong(ref WrkData); // convert from line format to 32-bit binary value
                    }
                    else
                    {
                        MICOMRptTXFreqRet = 0;
                    }
                }
                else
                {
                    MICOMRptTXFreqRet = 0;
                }
            }
            else
            {
                MICOMRptTXFreqRet = 0;
            }

            return MICOMRptTXFreqRet;
        }

        // *************************************************************************
        // * Send ACK
        // *************************************************************************
        private byte MICOMSendACK()
        {
            byte MICOMSendACKRet = default;
            const byte DataLen = 1; // 0x01
            const byte OpCode = 0xF3; // 243 '0xF3
            const byte FMTO = 0x10; // 16 '0x10
            byte rc;
            // Dim freqval(3) As Byte
            var Mydata = new byte[2];
            // Dim WrkData(3) As Byte
            // Dim Result(SizeofACK + SizeofFrpt) As Byte

            rc = Cmd2MICOM(DataLen, FMTO, OpCode, Mydata); // send command to Terminal
            if (rc != 0)
            {
                MICOMSendACKRet = 1;
            }
            else
            {
                MICOMSendACKRet = 0;
            }

            return MICOMSendACKRet;
        }

        // ************************************************************************
        // * open Comm Port
        // * PortName  is the comm port number to open
        // *  Port settings will be 8-bit data, Odd Parity, One stop bit and "rate" baud. 9600 is normal.
        // ************************************************************************
        private byte MICOMOpenPort(ref string PortName, int rate)
        {
            byte MICOMOpenPortRet = default;
            if (objSerial == null)
            {
                objSerial = new SerialPort();
            }

            MiTimer = new System.Timers.Timer();
            if (objSerial.IsOpen)                  // If port already open, close it and discard data
            {
                objSerial.DiscardOutBuffer();
                objSerial.DiscardInBuffer();
                objSerial.Close();
                Thread.Sleep(Globals.intComCloseTime);
            }

            objSerial.WriteTimeout = 750;
            objSerial.ReceivedBytesThreshold = 1;         // Minimum of 1 bytes for interrupt 
            objSerial.BaudRate = rate;
            objSerial.DataBits = 8;
            objSerial.Parity = Parity.Odd;
            objSerial.StopBits = StopBits.One;
            objSerial.PortName = PortName;
            objSerial.Handshake = Handshake.None;
            objSerial.RtsEnable = false;
            objSerial.DtrEnable = false;
            objSerial.DiscardNull = false;
            objSerial.Open();
            objSerial.DiscardOutBuffer();
            objSerial.DiscardInBuffer();
            MICOMOpenPortRet = 1;
            return MICOMOpenPortRet;
        }

        // ************************************************************************
        // * Close comm Port , dispose of it and dispose of the timer
        // ************************************************************************
        private byte MICOMPortClose()
        {
            byte MICOMPortCloseRet = default;
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
                MiTimer.Dispose();
            }

            MICOMPortCloseRet = 1;
            return MICOMPortCloseRet;
        }

        // ************************************************************************
        // ************************************************************************
        // *                                                                      *
        // * END OF PUBLIC FUNCTIONS.                                             *
        // *                                                                      *
        // ************************************************************************
        // ************************************************************************

        // *************************************************************************
        // * Convert 32-bit number to an array of 4 bytes to take care of the little
        // * endian to big endian problem with Intel processors. This little function
        // * will work on both little and big endian formats.
        // *************************************************************************
        private byte LongToLine(ref int NR, ref byte[] Result)
        {
            byte LongToLineRet = default;
            int NR1 = NR;
            Result[3] = Convert.ToByte(NR1 & 0xFF);
            NR1 = NR1 >> 8;
            Result[2] = Convert.ToByte(NR1 & 0xFF);
            NR1 = NR1 >> 8;
            Result[1] = Convert.ToByte(NR1 & 0xFF);
            NR1 = NR1 >> 8;
            Result[0] = Convert.ToByte(NR1 & 0xFF);
            LongToLineRet = 0;
            return LongToLineRet;
        }

        // *************************************************************************
        // * Convert four byte array to a 32-bit binary number.
        // * This little function will work on both little and big endian formats.
        // *************************************************************************
        private int LineToLong(ref byte[] Line)
        {
            int LineToLongRet = default;
            int WrkVal;
            LineToLongRet = 0;
            WrkVal = 0;
            WrkVal = Line[3];
            WrkVal = WrkVal + (Line[2] << 8);
            WrkVal = WrkVal + (Line[1] << 16);
            WrkVal = WrkVal + (Line[0] << 24);
            LineToLongRet = WrkVal;
            return LineToLongRet;
        }

        // ****************************************************************************
        // * Send a command to the MICOM radio
        // ****************************************************************************
        private byte Cmd2MICOM(byte DataLen, byte FMTO, byte OpCode, byte[] data)
        {
            byte Cmd2MICOMRet = default;
            short I;
            const byte SOM = 0x24; // 36 '0x24
            const byte EOM = 0x3;
            var OutMsg = new byte[81];
            // Dim OutStr As String

            // OutStr = ""
            OutMsg[0] = SOM;
            OutMsg[1] = DataLen;
            OutMsg[2] = FMTO;
            OutMsg[3] = OpCode;
            if (DataLen > 1)
            {
                var loopTo = DataLen - Convert.ToByte(2);
                for (I = 0; I <= loopTo; I++)
                    OutMsg[4 + I] = data[I];
            }

            OutMsg[3 + DataLen] = ChkSum(ref OutMsg, ref DataLen);
            OutMsg[4 + DataLen] = EOM;
            if (objSerial.IsOpen == true)
            {
                objSerial.DiscardInBuffer();                         // Throw away any input data
                objSerial.Write(OutMsg, 0, DataLen + 5);           // send the data
                Cmd2MICOMRet = DataLen;
            }
            else
            {
                Cmd2MICOMRet = 0;
            }

            return Cmd2MICOMRet;
        }

        // *******************************************************************************
        // * Get data from MICOM
        // * This function will not return until the correct number of bytes are received
        // ******************************************************************************
        private byte GetMICOMData(ref short NRBytes, ref byte[] Result)
        {
            byte GetMICOMDataRet = default;
            short I;
            if (objSerial.IsOpen == true)
            {
                Timer2Popped = false; // The timer interval is is milliseconds.
                MiTimer.Stop();
                MiTimer.Interval = NRBytes * 40; // This assumes a character time of 1 ms.
                MiTimer.Start();                 // Start the timer, runs in background.
                do
                    Application.DoEvents(); // Give up control to Op sys temporarily
                while (!(objSerial.BytesToRead >= NRBytes | Timer2Popped == true));
                if (Timer2Popped == true)
                {
                    GetMICOMDataRet = 0; // failed!
                }
                else
                {
                    // For I = 0 To NRBytes - 1 ' Move the data to calling routine
                    // Result(I) = objSerial.ReadByte() 'get data from port a byte at a time
                    // Next I
                    objSerial.Read(Result, 0, NRBytes);
                    if ((Result[2] & 15) != 0) // need an ACK?
                    {
                        I = MICOMSendACK();
                    }

                    GetMICOMDataRet = Convert.ToByte(NRBytes);
                }
            }
            else
            {
                GetMICOMDataRet = 0;
            } // failed

            return GetMICOMDataRet;
        }

        // *******************************************************************************
        // * generate a checksum for a message
        // *******************************************************************************
        private byte ChkSum(ref byte[] msg, ref byte DataLen)
        {
            byte ChkSumRet = default;
            short CkSumLen;
            short I;
            short Sum;
            CkSumLen = Convert.ToInt16(DataLen + Convert.ToInt16(2));
            Sum = 0; // initialize checksum
            var loopTo = (int)CkSumLen;
            for (I = 0; I <= loopTo; I++)
            {
                Sum = Convert.ToInt16(Sum + msg[I]);
                Sum = Convert.ToInt16(Sum & 0xFF); // 255
            }

            ChkSumRet = Convert.ToByte(Sum & 0xFF); // 255
            return ChkSumRet;
        }

        private void MiTimer_Elapsed(object Sender, ElapsedEventArgs e)
        {
            MiTimer.Stop();
            Timer2Popped = true;
        }
    }
}