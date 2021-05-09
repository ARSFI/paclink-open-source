using System;
using System.Text;

namespace TNCKissInterface
{
    //
    // This class handles station callsign and relay information used by connections
    //
    // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
    //
    public class Station
    {
        public enum CommandResponse { DoResponse = 0, DoCommand = 1};

        public StationAddress destinationStation = new StationAddress();
        public StationAddress sourceStation = new StationAddress();
        public StationAddress relayStation1 = new StationAddress();
        public StationAddress relayStation2 = new StationAddress();

        // Outbound packet match string
        public String GetConnectionPath()
        {
            return sourceStation.stationIDString + destinationStation.stationIDString + relayStation1.stationIDString + relayStation2.stationIDString;
        }

        // Inbound packet match string
        public String GetConnectionPathRev()
        {
            return destinationStation.stationIDString + sourceStation.stationIDString + relayStation2.stationIDString + relayStation1.stationIDString;
        }

        //
        // Build an octet buffer holding the callsign destination, source, and relay stations
        //
        public Byte[] GetStationAddresBuffer(CommandResponse cmdRsp)
        {
            Int32 i = 14;
            Int32 ptr = 0;

            if (relayStation1.stationIDString.Length > 0)
            {
                i += 7;
                if (relayStation2.stationIDString.Length > 0)
                {
                    i += 7;
                }
            }

            Byte[] oBuf = new Byte[i];

            destinationStation.stationBytes.CopyTo(oBuf, ptr);
            ptr += 7;
            oBuf[ptr - 1] &= 0x7e;      // remove current command/response/ext bits


            if (cmdRsp.Equals(CommandResponse.DoCommand))
            {
                oBuf[ptr - 1] |= 0x80;      // Set Command bit
            }

            sourceStation.stationBytes.CopyTo(oBuf, ptr);
            ptr += 7;
            if (cmdRsp.Equals(CommandResponse.DoResponse))
            {
                oBuf[ptr - 1] |= 0x80;      // Set Response bit
            }

            if (relayStation1.stationIDString.Length > 0)
            {
                //
                // Load any relay addresses into the header
                //
                relayStation1.stationBytes.CopyTo(oBuf, ptr);
                ptr += 7;

                if (relayStation2.stationIDString.Length > 0)
                {
                    relayStation2.stationBytes.CopyTo(oBuf, ptr);
                    ptr += 7;
                }
            }

            oBuf[ptr - 1] |= 0x01;      // Set Ext bit
            return oBuf;
        }

        public Station Rev()
        {
            Station tmpSL = new Station();

            tmpSL.sourceStation = destinationStation;
            tmpSL.destinationStation = sourceStation;
            if (relayStation2.stationIDString.Length > 0)
            {
                tmpSL.relayStation1 = relayStation2;
                tmpSL.relayStation2 = relayStation1;
            }
            else
            {
                tmpSL.relayStation1 = relayStation1;
            }

            return tmpSL;
        }
        public Int32 numRelays
        {
            get
            {
                Int32 tmp = 0;
                if (relayStation1.stationIDString.Length > 0)
                {
                    tmp++;
                    if (relayStation2.stationIDString.Length > 0)
                    {
                        tmp++;
                    }
                }
                return tmp;
            }
        }
    }

    public class StationAddressString
    {
        //
        // General station address string class
        //
        String destination;
        String source;
        String relayAddr1;
        String relayAddr2;

        public StationAddressString(String d, String s, String r1, String r2)
        {
            destination = d;
            source = s;
            relayAddr1 = r1;
            relayAddr2 = r2;
        }
    }


    public class StationAddress
    {
        //
        // General station address class
        //
        public string stationIDString = "";
        public Int32 stationSSID = 0;
        public Byte[] stationBytes = new Byte[7];
        public Int32 chBit = 0;
        public Int32 extBit = 1;

        public StationAddress()
        {
        }

        public StationAddress(String stationID)
        {
            SetCallSign(stationID);
        }

        public void Clear()
        {
            stationIDString = "";
            stationSSID = 0;
            chBit = 0;
            extBit = 0;
            stationBytes.Initialize();
        }

        public Int32 Decode(Byte[] buf, Int32 ptr)
        {
            Int32 i;
            stationIDString = "";
            stationSSID = 0;

            for (i = 0; i < 6; i++)
            {
                stationBytes[i] = buf[ptr++];
                stationIDString += Convert.ToChar(stationBytes[i] >> 1).ToString();
            }
            stationIDString = stationIDString.Trim() + "-";
            stationBytes[6] = buf[ptr++];
            stationSSID = (stationBytes[6] >> 1) & 0x0f;
            stationIDString += stationSSID.ToString();

            chBit = (stationBytes[6] >> 7) & 0x01;
            extBit = stationBytes[6] & 0x01;

            return ptr;
        }

        public void SetCallSign(String stationID)
        {
            StationIDInfo tmpID = Validate(stationID);

            stationIDString = tmpID.staString;
            stationSSID = tmpID.staSSID;

            tmpID.staBuf.CopyTo(stationBytes, 0);
            stationBytes[6] = (Byte)((stationSSID << 1) | 0x60);
        }

        StationIDInfo Validate(String stationID)
        {
            //
            // Validate the format and characters in the incoming callsign.  Throws an exception if any illegal 
            // characters are found or the format is invalid
            //
            Char[] s1 = { '-' };
            String validChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
            Int32 i;
            Exception ex = new Exception("Bad callsign supplied:" + stationID);
            StationIDInfo retID = new StationIDInfo();

            stationID = stationID.ToUpper().Trim();

            foreach (Char c in stationID)
            {
                if (!validChar.Contains(c.ToString()))
                {
                    throw ex;
                }
            }

            String[] a = stationID.Split(s1);

            if ((a.Length > 2) || (a[0].Length > 6))
            {
                throw ex;
            }

            if (a.Length == 2)
            {
                retID.staSSID = Convert.ToInt32(a[1]);
                if (retID.staSSID > 15)
                {
                    throw ex;
                }
            }

            retID.staString = a[0] + "-" + retID.staSSID.ToString();

            stationID = a[0].PadRight(6, ' ');
            Byte[] tmpB = Encoding.ASCII.GetBytes(stationID);
            for (i = 0; i < 6; i++)
            {
                retID.staBuf[i] = (Byte)(tmpB[i] << 1);
            }
            return retID;
        }

        class StationIDInfo
        {
            public String staString = "";
            public Int32 staSSID = 0;
            public Int32 CHBit = 1;
            public Int32 ExtBit = 0;
            public Byte[] staBuf = new Byte[6];
        }
    }
}
