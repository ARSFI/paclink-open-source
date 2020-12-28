using System;
using System.IO;
using Microsoft.VisualBasic;
using NLog;
using Paclink.Data;

namespace Paclink
{
    public class PartialSession
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public bool blnFirstRcvdBlk;
        public bool blnRemoveHdr8;
        private string strMID;
        private byte[] bytData;

        public void StartNewPartial(string MID)
        {
            strMID = MID;
            bytData = new byte[0];
            try
            {
                var messageStore = new MessageStore(DatabaseFactory.Get());
                messageStore.DeleteTemporaryInboundMessage(MID);
            }
            catch (Exception ex)
            {
                _log.Error("[PartialSession.StartNewPartial] " + ex.Message);
            }
        } // StartNewFile

        public void AccumulateByte(byte SingleByte)
        {
            Array.Resize(ref bytData, bytData.Length + 1);
            bytData[bytData.Length - 1] = SingleByte;
        } // AccumulateByte

        // Subroutine to write the partial to a file...should only be called on block boundaries.
        public void WritePartialToFile()
        {
            try
            {
                if (bytData.Length < 255)
                    return;

                var messageStore = new MessageStore(DatabaseFactory.Get());

                // 
                // Write or append the byte data to the output file
                //
                var bytOrig = messageStore.GetTemporaryInboundMessage(strMID);
                int intLen = bytOrig.Length;
                if (intLen > 0)
                {
                    Array.Resize(ref bytOrig, intLen + bytData.Length + 1);
                    bytData.CopyTo(bytOrig, intLen);
                    messageStore.SaveTemporaryInboundMessage(strMID, bytOrig);
                }
                else
                {
                    messageStore.SaveTemporaryInboundMessage(strMID, bytData);
                }

                bytData = new byte[0];
            }
            catch (Exception ex)
            {
                _log.Error("[PartialSessions, WritePartialToFile] Error: " + ex.Message);
            }
        } // WritePartialToFile

        public void PurgeCurrentPartial()
        {
            // Called to purge the partial if the message was received completly or protocol failure
            bytData = new byte[0];
            try
            {
                var messageStore = new MessageStore(DatabaseFactory.Get());
                messageStore.DeleteTemporaryInboundMessage(strMID);
            }
            catch
            {
            }
        } // PurgeCurrentPartial

        // Function to retrieve the partial file to be re processed by MessageInbound
        public byte[] RetrievePartial(string MID)
        {
            var messageStore = new MessageStore(DatabaseFactory.Get());
            byte[] bytFiledata = messageStore.GetTemporaryInboundMessage(MID);
            if (bytFiledata.Length == 0)
            {
                blnFirstRcvdBlk = false;
                blnRemoveHdr8 = false;
            }
            return bytFiledata;
        } // RetrievePartial

        // Function to strip the headers from the first blocks of a transmission of a  partial and return checksum of the stripped header
        public int StripHeaderFromPartial(ref byte[] bytPartial)
        {
            // This function strips the unneaded headers. For the second header of a partial where 
            // there are a total of 8 bytes it also computes and returns what the sumcheck would be to be able
            // to be able to preset the sumcheck to that starting value.

            if (!(blnFirstRcvdBlk | blnRemoveHdr8))
                return 0; // don't modify once headers are removed
            int intHeaderLen = bytPartial[1] + 2; // this accounts <SOH> or <STX> and the count of either header
            try
            {
                var bytTemp = new byte[bytPartial.Length - (1 + intHeaderLen) + 1];
                int intSumcheck = 0;
                if (bytTemp.Length > 0)
                {
                    Array.Copy(bytPartial, intHeaderLen, bytTemp, 0, bytTemp.Length);
                }
                // compute sumcheck of bytes to be removed
                if (!blnFirstRcvdBlk)
                {
                    for (int i = 2; i <= 7; i++)
                        intSumcheck += bytPartial[i];
                }

                bytPartial = bytTemp;
                if (blnFirstRcvdBlk)
                {
                    blnFirstRcvdBlk = false;
                    if (bytPartial.Length < 8)
                        return 0;
                    // compute sumcheck of bytes to be removed
                    for (int i = 2; i <= 7; i++)
                        intSumcheck += bytPartial[i];
                    bytTemp = new byte[bytPartial.Length - 9 + 1];
                    if (bytTemp.Length > 0)
                    {
                        Array.Copy(bytPartial, 8, bytTemp, 0, bytTemp.Length);
                    }

                    blnRemoveHdr8 = false;
                    bytPartial = bytTemp;
                    return intSumcheck;
                }
                else
                {
                    blnRemoveHdr8 = false;
                    return intSumcheck;
                }
            }
            catch (Exception ex)
            {
                _log.Error("[StripHeaderFromPartial] " + ex.Message);
                return 0;
            }
        } // StripHeaderFromPartial
    }
}