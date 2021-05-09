using System;
using System.Collections.Generic;
using System.Text;

namespace TNCKissInterface
{
    //
    // Define all the frame structures used in the AX.25 protocol
    // I Frame (IFrame)
    // S Frames (RRFrame, RNRFrame, REJFrame, SREJFrame)
    // U Frames (SABMFrame, SABMEFrame, DISCFrame, DMFrame, UAFrame, FRMRFrame (not used), UIFrame, XIDFrame, TESTFrame)
    //
    // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
    //
    public class Frame
    {

        static String CRLF = Convert.ToString((Char)13) + Convert.ToString((Char)10);

        public enum FrameClasses { IClass = I, UClass = U, SClass = S };
        public enum PacketType { Command = 0, Response };
        public enum FrameTypes
        {
            IType = I,
            RRType = RR, RNRType = RNR, REJType = REJ, SREJType = SREJ,
            SABMEType = SABME, SABMType = SABM, DISCType = DISC,
            DMType = DM, UAType = UA, FRMRType = FRMR,
            UIType = UI, XIDType = XID, TESTType = TEST
        };

        public enum ProtocolVersion { V20 = 0, V22 };
        public enum SequenceNumberMode { Mod8 = 8, Mod128 = 128 };

        const Byte I = 0x00;     // Information frames
        const Byte S = 0x01;     // Supervisory frames
        const Byte U = 0x03;     // Unnumbered frames

        const Byte RR = 0x01;	// Receiver ready
        const Byte RNR = 0x05;	// Receiver not ready
        const Byte REJ = 0x09;	// Reject
        const Byte SREJ = 0x0d; // Selective reject

        //
        // Control field Command types
        //
        const Byte SABM = 0x2f;  // Set Asynchronous Balanced Mode (Module 8)
        const Byte SABME = 0x6f; // Set Asynchronous Balanced Mode Extended (Modulo 128)
        const Byte DISC = 0x43;	 // Disconnect

        //
        // Control Field Response types
        //
        const Byte DM = 0x0f;	// Disconnected mode
        const Byte UA = 0x63;	// Unnumbered acknowledge
        const Byte FRMR = 0x87;	// Frame reject (Received from V1 targets

        //
        // Control Field Either types
        //
        const Byte PF = 0x10;	 // Poll/final bit
        const Byte UI = 0x03;    // Unnumbered Information
        const Byte XID = 0xaf;   // Exchange information and negotiate features
        const Byte TEST = 0xe3;  // Test 

        //
        // General purpose frame encoder 
        //
        public static TransmitFrame TransmitFrameBuild(Station stl, Station.CommandResponse cmdResp, Int32 pfBit, FrameTypes frameType)
        {
            return TransmitFrameBuild(stl, cmdResp, pfBit, frameType, null);
        }

        public static TransmitFrame TransmitFrameBuild(Station stl, Station.CommandResponse cmdResp, Int32 pfBit, FrameTypes frameType, Byte[] buf)
        {
            //
            // Build AX25 frames  
            //
            Int32 doBufLen = 0;

            if (buf != null)
            {
                doBufLen = buf.Length;
            }

            TransmitFrame transmitFrame = new TransmitFrame();
            Byte[] stlB = stl.GetStationAddresBuffer(cmdResp);

            transmitFrame.ibuf = new Byte[stlB.Length + 1 + doBufLen];
            transmitFrame.frameType = frameType;
            transmitFrame.pfBit = pfBit;
            transmitFrame.seqNumMode = SequenceNumberMode.Mod8;
            transmitFrame.cmdOctetPtr = stlB.Length;

            if (cmdResp.Equals(Station.CommandResponse.DoCommand) && pfBit == 1)     // (JNW Jan15)
                transmitFrame.AckModeID = 0xff;     // Use ackmode if enabled  (JNW Jan15)                                         


            stlB.CopyTo(transmitFrame.ibuf, 0);     // Load station address fields
            transmitFrame.ibuf[stlB.Length] = (Byte)(((Int32)frameType) | (pfBit << 4));

            if (doBufLen > 0)
            {
                buf.CopyTo(transmitFrame.ibuf, stlB.Length + 1);
            }

            return transmitFrame;
        }

        public static void AddSequence(ref TransmitFrame transmitFrame, Int32 nr, Int32 ns)
        {
            Int32 tmpI = transmitFrame.ibuf[transmitFrame.cmdOctetPtr] & 0x10;
            transmitFrame.ibuf[transmitFrame.cmdOctetPtr] = (Byte)((nr << 5) | (ns << 1) | tmpI);
        }

        public static void AddSequence(ref TransmitFrame transmitFrame, Int32 nr)
        {
            Int32 tmpI = transmitFrame.ibuf[transmitFrame.cmdOctetPtr] & 0x1f;
            transmitFrame.ibuf[transmitFrame.cmdOctetPtr] = (Byte)((nr << 5) | tmpI);
        }

        //  Routines to set and clear P bit in frame               (JNW Jan15 updated Jan17)

        public static void AddPBit(ref TransmitFrame transmitFrame)
        {
            transmitFrame.ibuf[transmitFrame.cmdOctetPtr] |= PF;
        }

        public static void ClearPBit(ref TransmitFrame transmitFrame)
        {
            transmitFrame.ibuf[transmitFrame.cmdOctetPtr] &= 0xff - PF;
        }

        public class Packet
        {
            public Frame.TransmitFrame transmitFrame;
            public Frame.ReceivedFrame receivedFrame;

            public Packet()
            {
            }

            public Packet(Frame.TransmitFrame transmitType)
            {
                transmitFrame = transmitType;
            }

            public Packet(Frame.ReceivedFrame receivedType)
            {
                receivedFrame = receivedType;
            }
        }

        public class TransmitFrame
        {
            public FrameTypes frameType;
            public SequenceNumberMode seqNumMode;
            public Int32 cmdOctetPtr;
//            public Station.CommandResponse cmdResp; // iF Acmode enables and P and Cmd will send AckMode frame    // (JNW Jan15)
            public Int16 AckModeID;                 // AckMode Session ID                // (JNW Jan15)
            public Int32 pfBit;
            public Byte[] ibuf;
        }

        public class ReceivedFrame
        {
            public FrameClasses frameClass;
            public FrameTypes frameType;
            public Boolean decodeOK = true;
            public SequenceNumberMode seqNumMode;
            public Byte[] inBuf;
            public Byte[] rawBuf;
            public PacketType cmdResp;
            public Int32 cmdOctetPtr;
            public Int32 numS;
            public Int32 numR;
            public Int32 pfBit;
            public Byte[] iBuf;
            public Station staLink = new Station();
            public ProtocolVersion version;
            public PidInfo pidInfo;
            public XIDInfo xidFrame;

            //
            // Constructor
            //
            public ReceivedFrame(Byte[] frame, out String s)
            {
                inBuf = frame;
                rawBuf = new Byte[frame.Length];
                frame.CopyTo(rawBuf, 0);
                decodeOK = Decode(out s);
            }

            //
            // Decode the received frame
            //
            Boolean Decode(out String packetFrame)
            {
                Int32 ptr = 0;
                Boolean retVal = false;
                StringBuilder s = new StringBuilder("|", 4096);

                try
                {
                    //
                    //  Catch any exceptions during decode
                    //
                    ptr = staLink.destinationStation.Decode(inBuf, ptr);
                    ptr = staLink.sourceStation.Decode(inBuf, ptr);

                    s.Append("|" + staLink.sourceStation.stationIDString + "|ch=" + staLink.sourceStation.chBit.ToString());
                    s.Append("|" + staLink.destinationStation.stationIDString + "|ch=" + staLink.destinationStation.chBit.ToString());

                    if (staLink.sourceStation.extBit == 0)
                    {
                        ptr = staLink.relayStation1.Decode(inBuf, ptr);
                        s.Append("|" + staLink.relayStation1.stationIDString + "|ch=" + staLink.relayStation1.chBit.ToString());
                        if (staLink.relayStation1.extBit == 0)
                        {
                            ptr = staLink.relayStation2.Decode(inBuf, ptr);
                            s.Append("|" + staLink.relayStation2.stationIDString + "|ch=" + staLink.relayStation2.chBit.ToString());
                            if (staLink.relayStation2.extBit == 0)
                            {
                                StationAddress tmpR = new StationAddress();
                                s.Append("|r??:(");

                                do
                                {
                                    ptr = tmpR.Decode(inBuf, ptr);
                                    s.Append("\\" + tmpR.stationIDString + ":ch=" + tmpR.chBit.ToString());
                                } while (tmpR.extBit == 0);
                                s.Append(")");
                                //throw new Exception("Too many relay addresses");
                            }
                            else
                            {
                                s.Append("|");
                            }
                        }
                        else
                        {
                            s.Append("||");
                        }
                    }
                    else
                    {
                        s.Append("|||");
                    }
                    cmdOctetPtr = ptr;

                    if ((staLink.destinationStation.chBit == staLink.sourceStation.chBit))
                    {
                        //
                        // Old protocol version
                        //
                        version = ProtocolVersion.V20;
                    }
                    else
                    {
                        //
                        // New protocol version
                        //
                        version = ProtocolVersion.V22;
                    }

                    if (staLink.destinationStation.chBit == 0)
                    {
                        //
                        // Response type
                        //
                        cmdResp = PacketType.Response;
                    }
                    else
                    {
                        //
                        // Command type
                        //
                        cmdResp = PacketType.Command;
                    }

                    //
                    // Get frame type.  I and S frame must be decoded later after we know whether we
                    // are using mod8 or mod128 sequence numbers.
                    //
                    Byte tmp = inBuf[ptr];
                    String t = "";

                    pfBit = (tmp >> 4) & 0x01;      //ToDo  change needed to support SABME

                    seqNumMode = SequenceNumberMode.Mod8;

                    //Frame.InformationFrame.Decode(this, rawBuf, ptr, out t); 

                    if ((tmp & 0x01) == I)
                    {
                        //
                        // I frame
                        //
                        frameClass = FrameClasses.IClass;
                        frameType = FrameTypes.IType;
                        ptr = InformationFrame.Decode(this, inBuf, ptr, out t);
                    }

                    if ((tmp & 0x03) == S)
                    {
                        //
                        // S frame
                        //
                        frameClass = FrameClasses.SClass;
                        frameType = (FrameTypes)(tmp & 0x0f);
                        ptr = SupervisoryFrame.Decode(this, inBuf, ptr, out t);
                    }

                    if ((tmp & 0x03) == U)
                    {
                        //
                        // U frame.  We can decode uFrames right away
                        //
                        frameClass = FrameClasses.UClass;
                        frameType = (FrameTypes)(tmp & 0xef);
                        ptr = UnnumberedFrame.Decode(this, inBuf, ptr, out t);
                    }
                    s.Append("|" + version.ToString() + "|" + frameType.ToString() + "|" + cmdResp.ToString() + "|pf=" + pfBit.ToString());

                    if (t.Length > 0)
                    {
                        s.Append(t);
                    }
                    s.Append(Support.DumpRawFrame(rawBuf));

                    retVal = true;

                }
                catch (Exception ex)
                {
                    s.Append(ex.Message + CRLF);
                    Support.DbgPrint("Exception during frame decode:" + ex.Message + CRLF);
                    retVal = false;
                }
                finally
                {
                    s.Append("]");
                    packetFrame = s.ToString();
                }

                return retVal;
            }
        }

        class InformationFrame
        {
            public static Int32 Decode(ReceivedFrame frame, Byte[] buf, Int32 ptr, out String s)
            {
                String r;
                //if (seqNumMode.Equals(SequenceNumberMode.Mod8))
                //{
                frame.numS = (buf[ptr] >> 1) & 0x07;
                frame.numR = (buf[ptr++] >> 5) & 0x07;

                //    NumR = (buf[ptr++] >> 5) & 0x07;
                //}
                //else
                //{
                //    NumS = (buf[ptr++] >> 1) & 0x7f;
                //    PFBit = buf[ptr] & 0x01;
                //    NumR = (buf[ptr++] >> 1) & 0x7f;
                //}
                s = "|ns=" + frame.numS.ToString() + "|nr=" + frame.numR.ToString();

                frame.pidInfo = new PidInfo();
                ptr = frame.pidInfo.Decode(buf, ptr, out r);

                if (frame.pidInfo.cmdDecode.Equals(PidInfo.CommandDecode.Standard))
                {
                    //
                    // On standard PID fields, pre-load the segmentation chuck data where the PID used to be in the buffer.
                    // Extended info will already have the correct segmentation chunk info there.
                    //
                    buf[ptr] = Segmenter.StartOfSegment;
                }

                frame.iBuf = Support.PackByte(buf, ptr, (buf.Length - ptr));

                s = s + r + "|len:" + frame.iBuf.Length.ToString() + "|txt:(" + Support.GetString(frame.iBuf) + ")";

                return ptr;
            }
        }

        class SupervisoryFrame
        {
            public static Int32 Decode(ReceivedFrame frame, Byte[] buf, Int32 ptr, out String s)
            {
                frame.numR = (buf[ptr++] >> 5) & 0x07;
                s = "||nr=" + frame.numR.ToString();
                return ptr;
            }
        }

        class UnnumberedFrame
        {
            public static Int32 Decode(ReceivedFrame frame, Byte[] buf, Int32 ptr, out String s)
            {
                String r = "";
                s = "";
                ptr++;

                if (frame.frameType.Equals(FrameTypes.UIType))
                {
                    frame.pidInfo = new PidInfo();
                    ptr = frame.pidInfo.Decode(buf, ptr, out r);

                    if (frame.pidInfo.cmdDecode.Equals(PidInfo.CommandDecode.Standard))
                    {
                        //
                        // On standard PID fields, pre-load the segmentation chuck data where the PID used to be in the buffer.
                        // Extended info will already have the correct segmentation chunk info there.
                        //
                        buf[ptr] = Segmenter.StartOfSegment;
                    }

                    frame.iBuf = Support.PackByte(buf, ptr, (buf.Length - ptr));

                    s = r + "|||len:" + frame.iBuf.Length.ToString() + "|txt:(" + Support.GetString(frame.iBuf) + ")";

                }
                if (frame.frameType == FrameTypes.TESTType)
                {
                    frame.iBuf = Support.PackByte(buf, ptr, (buf.Length - ptr));
                }
                if (frame.frameType == FrameTypes.XIDType)
                {
                    XIDInfo xidInfo = new XIDInfo();
                    ptr = xidInfo.Decode(buf, ptr, out r);
                    s = r;
                    frame.xidFrame = xidInfo;
                }

                return ptr;
            }
        }
        public class XIDInfo
        {
            public enum FormatIdentifier { GeneralPurposeXIDInfo = 0x82 };
            public enum GroupIdentifier { ParameterNegotiation = 0x80 };
            public enum ClassOfProcedures { BalancedABMHalfDup = 0x0021, BalancedABMFullDup = 0x0041 };
            public enum ParameterIndicator
            {
                ClassOfProcedures = 0x02, HDLCOptionalParams = 0x03,
                RXIFieldLen = 0x06, RXWindowSize = 0x08,
                AckTimer = 0x09, NumRetries = 0x0a
            }

            public enum HDLCOptionalFunctions
            {
                REJCmdResp = 0x000002,
                SREJCmdResp = 0x000004,
                ExtendedAddress = 0x000080,
                Mod8 = 0x000400,
                Mod128 = 0x000800,
                TestCmdResp = 0x008000,
                FCS16 = 0x002000,
                SyncTX = 0x020000
            };
            public FormatIdentifier formatID;
            public GroupIdentifier groupID;
            public Int32 PLVLen;
            public ClassOfProcedures classOfProc;
            public HDLCOptionalFunctions hdlcOptFunc;
            public List<PLV> PLVList = new List<PLV>();

            public static TransmitFrame Encode(Station stl, Connection.ConnectionParameterBuf cBuf, Station.CommandResponse cmdResp, Int32 pfBit)
            {
                //
                // Build the XID frame to send
                //

                TransmitFrame commandFrame = new TransmitFrame();
                Byte[] stlB = stl.GetStationAddresBuffer(cmdResp);
                Int32 ptr = stlB.Length;
                Int32 t;
                Int32 c;
                Int32 paramPtr = 0;
                Byte[] tmpP = new Byte[30];

                commandFrame.ibuf = new Byte[tmpP.Length + stlB.Length]; // overallocate buffer space to handle worst case.  We'll trim later
                commandFrame.frameType = FrameTypes.XIDType;
                commandFrame.seqNumMode = SequenceNumberMode.Mod8;
                commandFrame.cmdOctetPtr = stlB.Length;

                stlB.CopyTo(commandFrame.ibuf, 0);     // Load station address fields

                commandFrame.ibuf[ptr++] = (Byte)(((Int32)FrameTypes.XIDType) | (pfBit << 4));
                commandFrame.ibuf[ptr++] = (Byte)(FormatIdentifier.GeneralPurposeXIDInfo);
                commandFrame.ibuf[ptr++] = (Byte)(GroupIdentifier.ParameterNegotiation);
                //
                // Load parameters to negotiate
                //

                t = (Int32)ClassOfProcedures.BalancedABMHalfDup;
                tmpP[paramPtr++] = (Byte)(ParameterIndicator.ClassOfProcedures);
                tmpP[paramPtr++] = (Byte)2;
                tmpP[paramPtr++] = (Byte)((t >> 8) & 0xff);
                tmpP[paramPtr++] = (Byte)(t & 0x7f);

                tmpP[paramPtr++] = (Byte)(ParameterIndicator.HDLCOptionalParams);
                tmpP[paramPtr++] = (Byte)3;
                tmpP[paramPtr++] = (Byte)(cBuf.optionalFuncByte0);
                tmpP[paramPtr++] = (Byte)(cBuf.optionalFuncByte1);
                tmpP[paramPtr++] = (Byte)(cBuf.optionalFuncByte2);

                //
                // The IFrame size specified here is in bits.  Check to see how many bytes we'll need to specify
                //
                tmpP[paramPtr++] = (Byte)(ParameterIndicator.RXIFieldLen);
                c = cBuf.maxIFrame * 8;
                if (c >= 65536)
                {
                    //
                    // Need 4 bytes to specify the window size
                    //
                    tmpP[paramPtr++] = (Byte)4;
                    tmpP[paramPtr++] = (Byte)((c >> 24) & 0xff);
                    tmpP[paramPtr++] = (Byte)((c >> 16) & 0xff);
                }
                else
                {
                    //
                    // Need only 2 bytes
                    //
                    tmpP[paramPtr++] = (Byte)2;
                }
                tmpP[paramPtr++] = (Byte)((c >> 8) & 0xff);
                tmpP[paramPtr++] = (Byte)(c & 0xff);

                tmpP[paramPtr++] = (Byte)(ParameterIndicator.RXWindowSize);
                tmpP[paramPtr++] = (Byte)1;
                tmpP[paramPtr++] = (Byte)(cBuf.maxWindowSize & 0xff);

                //
                // The Ack timer is specified in mSec.  Check to see how many bytes we'll need 
                //
                tmpP[paramPtr++] = (Byte)(ParameterIndicator.AckTimer);
                if (cBuf.ackTimer >= 65536)
                {
                    //
                    // Need 4 bytes to specify ack timer
                    //
                    tmpP[paramPtr++] = (Byte)4;
                    tmpP[paramPtr++] = (Byte)((cBuf.ackTimer >> 24) & 0xff);
                    tmpP[paramPtr++] = (Byte)((cBuf.ackTimer >> 16) & 0xff);
                }
                else
                {
                    //
                    // Need only 2 bytes
                    //
                    tmpP[paramPtr++] = (Byte)2;
                }
                tmpP[paramPtr++] = (Byte)((cBuf.ackTimer >> 8) & 0xff);
                tmpP[paramPtr++] = (Byte)(cBuf.ackTimer & 0xff);

                tmpP[paramPtr++] = (Byte)(ParameterIndicator.NumRetries);
                tmpP[paramPtr++] = (Byte)1;
                tmpP[paramPtr++] = (Byte)(cBuf.maxRetry & 0xff);

                tmpP = Support.PackByte(tmpP, 0, paramPtr);     // Pack it down

                //
                // Add parameter length field to buffer
                //

                commandFrame.ibuf[ptr++] = (Byte)((tmpP.Length >> 8) & 0xff);
                commandFrame.ibuf[ptr++] = (Byte)(tmpP.Length & 0xff);

                //
                // Add the parameters to the buffer
                //
                tmpP.CopyTo(commandFrame.ibuf, ptr);
                ptr += tmpP.Length;

                commandFrame.ibuf = Support.PackByte(commandFrame.ibuf, 0, ptr);  //Pack it down

                return commandFrame;
            }

            public PLV GetPLV(ParameterIndicator pInd)
            {
                foreach (PLV p in PLVList)
                {
                    if (pInd.Equals(p.PI))
                    {
                        return p;
                    }
                }
                return null;
            }

            public Int32 Decode(Byte[] buf, Int32 ptr, out String s)
            {
                String r = "";
                formatID = (FormatIdentifier)buf[ptr++];
                groupID = (GroupIdentifier)buf[ptr++];
                PLVLen = (buf[ptr++] << 8) | buf[ptr++];

                s = "|" + formatID.ToString() +
                   "|" + groupID.ToString() +
                   "|len:" + PLVLen.ToString();

                while (PLVLen > 0)
                {
                    PLV tmpPLV = new PLV();
                    ptr = tmpPLV.Decode(buf, ptr, out r);
                    s += r;
                    PLVList.Add(tmpPLV);
                    PLVLen -= (tmpPLV.PL + 2);
                }

                return ptr;
            }

            public class PLV
            {
                public ParameterIndicator PI;
                public Int32 PL;
                public Int32 PV;
                public Byte[] PVB;

                public Int32 Decode(Byte[] buf, Int32 ptr, out String s)
                {
                    Int32 result = 0;
                    PI = (ParameterIndicator)buf[ptr++];
                    PL = buf[ptr++];

                    s = "|" + PI.ToString() + "(len=" + PL.ToString();

                    switch (PL)
                    {
                        case 4:
                            result |= (buf[ptr++] << 24);
                            result |= (buf[ptr++] << 16);
                            result |= (buf[ptr++] << 8);
                            PV = result | buf[ptr++];
                            s += ":" + PV.ToString("x8");
                            break;
                        case 2:
                            result |= (buf[ptr++] << 8);
                            PV = result | buf[ptr++];
                            s += ":" + PV.ToString("x4");
                            break;
                        case 1:
                            PV = result | buf[ptr++];
                            s += ":" + PV.ToString("x2");
                            break;
                        default:
                            PVB = new Byte[PL];
                            s += ":";

                            for (Int32 i = 0; i < PL; i++)
                            {
                                PVB[i] = buf[ptr++];
                                s += PVB[i].ToString("x2");
                                if (i < PL - 1)
                                {
                                    s += " ";
                                }
                            }

                            break;
                    }
                    s += ")";
                    return ptr;
                }
            }
        }

        public class PidInfo
        {
            public enum CommandDecode { Standard = 0, SegmentFragment, Unknown };
            public enum PidTypes
            {
                AX25L3Implemented = 0x00,
                ISO8208CCITT_X25PLP = 0x01,
                CompTCPIPPkt = 0x06,
                UncompTCPIPPkt = 0x07,
                SegFrag = 0x08,
                TEXNET_DGProto = 0xc3,
                LinkQualProto = 0xc4,
                Appletalk = 0xca,
                AppletalkARP = 0xcb,
                ARPA_IP = 0xcc,
                ARPA_AddrRes = 0xcd,
                FLEXNET = 0xce,
                NETROM = 0xcf,
                NoL3Proto = 0xf0,
                EscChar_0xff = 0xff
            };

            public PidTypes pidType;
            public CommandDecode cmdDecode;
            public Byte extendOctet = 0x00;

            public Int32 Decode(Byte[] buf, Int32 ptr, out String r)
            {
                cmdDecode = PidInfo.CommandDecode.Standard;
                pidType = (PidTypes)buf[ptr];

                if (pidType.Equals(PidTypes.SegFrag))
                {
                    cmdDecode = PidInfo.CommandDecode.SegmentFragment;
                    ptr++;
                }

                r = "|" + pidType.ToString() + "|" + cmdDecode.ToString();

                if (cmdDecode.Equals(PidInfo.CommandDecode.SegmentFragment))
                {
                    r += "|ext=" + extendOctet.ToString();
                }

                return ptr;
            }
        }
    }
}
