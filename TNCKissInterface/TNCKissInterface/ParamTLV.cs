using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TNCKissInterface
{
    public class ParamTLV
    {
        public Byte tag;
        public Byte length;
        public Byte[] value;

        public ParamTLV(Int32 PI)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = 0;
            value = null;

        }

        public ParamTLV(Int32 PI, Byte[] PV)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = Convert.ToByte(PV.Length);
            value = PV;
        }

        public ParamTLV(Int32 PI, Byte PV)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = (Byte) 1;
            value = new Byte[] { PV };
        }

        public ParamTLV(Int32 PI, Int16 PV)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = (Byte)2;
            value = new Byte[] { Convert.ToByte((PV >> 8) & 0xff), Convert.ToByte(PV & 0xff) };
        }

        public ParamTLV(Int32 PI, UInt16 PV)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = (Byte)2;
            value = new Byte[] { Convert.ToByte((PV >> 8) & 0xff), Convert.ToByte(PV & 0xff) };
        }

        public ParamTLV(Int32 PI, Int32 PV)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = (Byte)4;
            value = new Byte[] { Convert.ToByte((PV >> 24) & 0xff), Convert.ToByte((PV >> 16) & 0xff),
                                 Convert.ToByte((PV >> 8) & 0xff), Convert.ToByte(PV & 0xff) };
        }

        public ParamTLV(Int32 PI, UInt32 PV)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = (Byte)4;
            value = new Byte[] { Convert.ToByte((PV >> 24) & 0xff), Convert.ToByte((PV >> 16) & 0xff),
                                 Convert.ToByte((PV >> 8) & 0xff), Convert.ToByte(PV & 0xff) };
        }

        public ParamTLV(Int32 PI, Int64 PV)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = (Byte)8;
            value = new Byte[] { Convert.ToByte((PV >> 56) & 0xff), Convert.ToByte((PV >> 48) & 0xff),
                                 Convert.ToByte((PV >> 40) & 0xff), Convert.ToByte((PV >> 32) & 0xff),
                                 Convert.ToByte((PV >> 24) & 0xff), Convert.ToByte((PV >> 16) & 0xff),
                                 Convert.ToByte((PV >> 8) & 0xff), Convert.ToByte(PV & 0xff) };
        }

        public ParamTLV(Int32 PI, UInt64 PV)
        {
            tag = Convert.ToByte(PI & 0xff);
            length = (Byte)8;
            value = new Byte[] { Convert.ToByte((PV >> 56) & 0xff), Convert.ToByte((PV >> 48) & 0xff),
                                 Convert.ToByte((PV >> 40) & 0xff), Convert.ToByte((PV >> 32) & 0xff),
                                 Convert.ToByte((PV >> 24) & 0xff), Convert.ToByte((PV >> 16) & 0xff),
                                 Convert.ToByte((PV >> 8) & 0xff), Convert.ToByte(PV & 0xff) };
        }
    }
}
