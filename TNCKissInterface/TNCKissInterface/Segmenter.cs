using System;

namespace TNCKissInterface
{
    //
    // This class handles breaking incoming packets into the appropriate number of segments.  
    // UI frames can be up to 128 segments.  IFrame sends can be up to 2^30 byte in length.
    //
    // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
    //
    public class Segmenter
    {

        const Int32 MaxUISegments = 128;
        const Int32 MaxSendBytes = 0x40000000;
        public const Int32 StartOfSegment = 0x80;

        Byte[] buf;
        Int32 fSize;
        Int32 _segmentTotal = 0;
        Frame.FrameTypes fType;
        public Int32 segmentTotal
        {
            get { return (_segmentTotal); }
        }

        Int32 _segmentPtr = 0;
        public Int32 segmentsRemaining
        {
            get { return (_segmentTotal - _segmentPtr); }
        }

        public Segmenter(Int32 frameSize, Byte[] buffer, Frame.FrameTypes frameType, Frame.ProtocolVersion version)
        {
            fType = frameType;
            buf = buffer;
            fSize = frameSize;

            if (frameType.Equals(Frame.FrameTypes.IType))
            {
                //
                // Call the IFRame segmenter.  This segmenter does not use fragmented packets
                //
                SegmentIFrames(frameSize, buffer);
            }
            else
            {
                //
                // Call the UIFRame segmenter.  This segmenter does use fragmented packets if we're running V22 protocol
                //
                SegmentUIFrames(frameSize, buffer, version);
            }

        }

        void SegmentIFrames(Int32 frameSize, Byte[] buffer)
        {
            //
            // Sending IFrames. Since we guaranteed packet ordering with the AX25 protocol, there is no need
            // to use fragmented frames.  Also, we can send up to 2^30 bytes of data at a shot.  We'll set our
            // max segment value equal to 2^30 / frame size
            //  

            if (buffer.Length > MaxSendBytes)
            {
                throw new Exception("Data buffer too large. " + MaxSendBytes + " byte maximum");
            }

            _segmentTotal = (buffer.Length + (frameSize - 1)) / frameSize;
        }

        void SegmentUIFrames(Int32 frameSize, Byte[] buffer, Frame.ProtocolVersion version)
        {
            Int32 tmpMaxSegments = MaxUISegments;
            fSize = frameSize;

            if (version.Equals(Frame.ProtocolVersion.V20))
            {
                tmpMaxSegments = 1;     // No segmenter if using the old protocol
            }

            if (buf.Length <= fSize)
            {
                _segmentTotal = 1;
            }
            else
            {
                fSize--;     // Reduce frame size by one byte if we will be using segmented frames
                _segmentTotal = (buffer.Length + (fSize - 1)) / fSize;
            }

            if (_segmentTotal > tmpMaxSegments)
            {
                throw new Exception("Data buffer too large. " + fSize * tmpMaxSegments + " byte maximum");
            }
        }

        public Byte[] GetNextSegment()
        {
            Byte[] tmpB;
            Int32 bufLen = fSize;

            //
            // Return a specific segment from a larger packet.  Adds the appropriate PID & Segmentation information
            // as appropriate
            //
            if (_segmentPtr == _segmentTotal)
            {
                //
                // At the end, so return null
                //
                return null;
            }

            if (fType.Equals(Frame.FrameTypes.IType))
            {
                //
                // Iframes don't use fragmented frames
                //
                if (_segmentPtr == (_segmentTotal - 1))
                {
                    //
                    // Calculate buffer size of the final buffer
                    //
                    bufLen = (buf.Length % fSize);
                    if (bufLen == 0)
                    {
                        bufLen = fSize;
                    }
                }

                tmpB = new Byte[bufLen + 1];
                tmpB[0] = (Byte)(Frame.PidInfo.PidTypes.NoL3Proto);
                for (Int32 i = 0; i < bufLen; i++)
                {
                    tmpB[i + 1] = buf[(_segmentPtr * fSize) + i];
                }
                _segmentPtr++;
                return tmpB;
            }

            //
            // UIFrames use fragmented frames
            //
            if (_segmentTotal == 1)
            {
                //
                // Buffer fits in a single packet
                //
                tmpB = new Byte[buf.Length + 1];
                tmpB[0] = (Byte)(Frame.PidInfo.PidTypes.NoL3Proto);
                buf.CopyTo(tmpB, 1);
                _segmentPtr++;
                return tmpB;
            }

            if (_segmentPtr == (_segmentTotal - 1))
            {
                //
                // Calculate buffer size of the final buffer
                //
                bufLen = (buf.Length % fSize);
                if (bufLen == 0)
                {
                    bufLen = fSize;
                }
            }

            tmpB = new Byte[bufLen + 2];
            tmpB[0] = (Byte)Frame.PidInfo.PidTypes.SegFrag;
            tmpB[1] = (Byte)((_segmentTotal - _segmentPtr - 1) & 0x7f);

            if (_segmentPtr == 0)
            {
                tmpB[1] |= StartOfSegment;        // Set the start of segment bit
            }

            for (Int32 i = 0; i < bufLen; i++)
            {
                //
                // Populate the return buffer
                //
                tmpB[i + 2] = buf[(_segmentPtr * fSize) + i];
            }
            _segmentPtr++;

            return tmpB;
        }
    }
}
