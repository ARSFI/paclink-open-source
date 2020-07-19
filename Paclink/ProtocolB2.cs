using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Paclink
{
    public class ProtocolB2
    {
        public enum EB2States
        {
            WaitingForNewCommand,
            ReceivingB2Proposal,
            ReceivingB2Messages,
            SendingB2Proposal,
            ReceivingB2Acceptance,
            SendingB2Messages,
            WaitingForAcknowledgement,
            Disconnecting
        } // EB2States

        private TChannelProperties stcChannel;
        private ProtocolInitial objInitialProtocol;
        private Message objMessageInbound;
        private PartialSession objPartialSession = new PartialSession();
        private EB2States enmB2State;              // The protocol state variable
        private string strBaseConnectedCall;       // The "connected to" callsign without the SSID
        private string strChannelDirectory;        // Path to the subdirectory for outbound messages
        private int intCheckSum;               // Holds partial checksums on proposals
        private int intInboundMessageCount;    // Holds message count on transfers
        private string strLastCommandReceived;     // Holds the previously seen command
        private int intInboundProposalIndex;   // Holds the index to the currently received proposed message
        private int intProposed;               // Holds the number of messages actually proposed to send
        private System.Timers.Timer tmrDisconnect;               // Used to delay disconnect
        private ArrayList aryOutboundMessages;     // Holds waiting outbound messages
        private List<Globals.Proposal> cllInboundMessageIDs = new List<Globals.Proposal>();
        private Queue queInboundData = Queue.Synchronized(new Queue()); // FIFO queue used to buffer inbound data
                                                                        // Private tmrKeepAlive As Timer                ' Keeps the Remote TCP channel alice during long downloads

        // Session statistics...
        private int intCompressedMessageBytesSent;
        private int intCompressedMessageBytesReceived;
        private int intUncompressedMessageBytesSent;
        private int intUncompressedMessageBytesReceived;
        private int[] intProposedCompressedSize = new int[5]; // An array to hold the compressed size of message for each proposal.

        public ProtocolB2(ProtocolInitial Parent, ref TChannelProperties stcNewChannel, ref ArrayList aryMessages)
        {

            // 
            // Initialize the keepalive timer used during long file downloads to keep the remote TCP link from timing out
            // 
            // tmrKeepAlive = New Timer
            // AddHandler tmrKeepAlive.Elapsed, AddressOf OnKeepAliveTimer
            // tmrKeepAlive.AutoReset = False
            // tmrKeepAlive.Interval = 120000   ' 2 minutes

            // Instantiates a B2 channel protocol handler...

            stcChannel = stcNewChannel;
            objInitialProtocol = Parent;
            StateChange(EB2States.WaitingForNewCommand);
            Globals.blnFQSeen = false;
            if (!string.IsNullOrEmpty(stcChannel.RemoteCallsign))
            {
                var strTokens = stcChannel.RemoteCallsign.Split('-');
                strBaseConnectedCall = strTokens[0];
            }

            aryOutboundMessages = aryMessages;
            B2OutboundProposal();
        } // New

        public void Close()
        {
            Globals.queChannelDisplay.Enqueue("G*** Session Statistics: " + intUncompressedMessageBytesSent.ToString() + "/" + intCompressedMessageBytesSent.ToString() + " message bytes sent;  " + intUncompressedMessageBytesReceived.ToString() + "/" + intCompressedMessageBytesReceived + " message bytes received");


            Globals.ResetProgressBar();
            if (tmrDisconnect is object)
            {
                tmrDisconnect.Stop();
                tmrDisconnect.Dispose();
                tmrDisconnect = null;
            }
        } // Close

        // Receives string data from the channel...
        private StringBuilder sbdDataStr = new StringBuilder();
        public void ChannelInput(string strData)
        {
            for (int intIndex = 0, loopTo = strData.Length - 1; intIndex <= loopTo; intIndex++)
            {
                if (strData[intIndex] != '\n')
                {
                    if (strData[intIndex] == '\r')
                    {
                        if (sbdDataStr.Length != 0)
                            this.ProtocolCommands(sbdDataStr.ToString(), false);
                        sbdDataStr.Length = 0;
                    }
                    else
                    {
                        sbdDataStr.Append(strData[intIndex]);
                    }
                }
            }
        } // ChannelInput - String
          
        // Receives binary data from the channel...
        private StringBuilder sbdDataIn = new StringBuilder();
        public void ChannelInput(ref byte[] bytBytes)
        {
            if (enmB2State == EB2States.ReceivingB2Messages)
            {
                B2MessageInbound(bytBytes);
            }
            else
            {
                foreach (byte byt in bytBytes)
                {
                    if ((int)byt != 10)
                    {
                        if ((int)byt == 13)
                        {
                            if (sbdDataIn.Length != 0)
                            {
                                this.ProtocolCommands(sbdDataIn.ToString(), false);
                                sbdDataIn.Length = 0;
                            }
                        }
                        else if ((int)byt != 0)
                            sbdDataIn.Append((char)byt);
                    }
                }
            }
        } // ChannelInput - Bytes

        private void StateChange(EB2States enmNewState)
        {

            // If enmB2State <> enmNewState Then
            // '
            // ' State change
            // '
            // If enmNewState = EB2States.ReceivingB2Messages Then
            // '
            // ' We're receiving a B2 message, so enable the 2 minute keep-alive timer
            // '
            // tmrKeepAlive.Start()
            // Else
            // '
            // ' The timer is disabled for all other modes
            // '
            // tmrKeepAlive.Stop()
            // End If
            // End If

            enmB2State = enmNewState;
            Globals.enmEB2States = enmB2State; // Mirror of instance state in Global
            switch (enmNewState)
            {
                case EB2States.Disconnecting:
                    {
                        Globals.queStatusDisplay.Enqueue("Disconnecting");
                        break;
                    }

                case EB2States.ReceivingB2Acceptance:
                    {
                        Globals.queStatusDisplay.Enqueue("Receiving Acceptance");
                        break;
                    }

                case EB2States.ReceivingB2Messages:
                    {
                        Globals.queStatusDisplay.Enqueue("Receiving Messages");
                        break;
                    }

                case EB2States.ReceivingB2Proposal:
                    {
                        Globals.queStatusDisplay.Enqueue("Receiving Proposal");
                        break;
                    }

                case EB2States.SendingB2Proposal:
                    {
                        Globals.queStatusDisplay.Enqueue("Sending Proposal");
                        break;
                    }

                case EB2States.SendingB2Messages:
                    {
                        Globals.queStatusDisplay.Enqueue("Sending Messages");
                        break;
                    }

                case EB2States.WaitingForAcknowledgement:
                    {
                        Globals.queStatusDisplay.Enqueue("Sending Messages");
                        break;
                    }

                case EB2States.WaitingForNewCommand:
                    {
                        Globals.queStatusDisplay.Enqueue("Waiting for Command");
                        break;
                    }
            }
        } // StateChange

        // Private Sub OnKeepAliveTimer(ByVal source As Object, ByVal e As ElapsedEventArgs)
        // '
        // ' Keepalive timer handler
        // '
        // tmrKeepAlive.Stop()
        // If enmB2State = EB2States.ReceivingB2Messages Then
        // '
        // ' The timer fired while we're receiving a message, so send a comment character back up the channel
        // '
        // objInitialProtocol.objClient.DataToSend(";" & vbCr)

        // '
        // ' Restart tjhe timer
        // '
        // tmrKeepAlive.Start()
        // End If
        // End Sub ' OnKeepAliveTimer

        private void Send(string strText, bool blnCR = true)
        {
            // Sends a text stream to the channel...
            Globals.queChannelDisplay.Enqueue("B" + strText);
            if (blnCR)
            {
                objInitialProtocol.objClient.DataToSend(strText + Globals.CR);
            }
            else
            {
                objInitialProtocol.objClient.DataToSend(strText);
            }
        } // Send

        private void ProtocolCommands(string strText, bool blnCRLF)
        {
            // 
            // Interprets the protocol commands as they are received.
            // 
            Globals.queChannelDisplay.Enqueue("X" + strText);
            if (blnCRLF)
                Globals.queChannelDisplay.Enqueue("X" + Globals.CRLF);
            string strCommand = strText.ToUpper();
            if (strCommand.StartsWith(";PM:"))
            {
                // Ignore preview-message commands
                return;
            }

            strLastCommandReceived = strCommand; // Save to be able to process FF FQ sequence
            var switchExpr = enmB2State;
            switch (switchExpr)
            {
                case EB2States.WaitingForNewCommand:
                    {
                        if (strCommand.StartsWith(";"))
                        {
                        }
                        // Do nothing - this is a comment string
                        else if (strCommand.StartsWith("FC"))
                        {
                            // If blnNeedAcknowledgement Then B2ConfirmSentMessages()
                            StateChange(EB2States.ReceivingB2Proposal);
                            B2InboundProposal(strText, true);
                        }
                        else if (strCommand.StartsWith("FF"))
                        {
                            // If blnNeedAcknowledgement Then B2ConfirmSentMessages()
                            B2OutboundProposal();
                        }
                        else if (strCommand.StartsWith("S"))
                        {
                            Send("[441] - Command:\"" + strCommand + "\" not recognized - disconnecting");
                            Disconnect();
                        }
                        else if (strCommand.StartsWith("B"))
                        {
                            Disconnect();
                        }
                        else if (strCommand.StartsWith("FQ"))
                        {
                            Globals.blnFQSeen = true;
                            objInitialProtocol.objClient.NormalDisconnect = true;
                            Disconnect(500);
                        }
                        else if (strCommand.EndsWith(">"))
                        {
                        }
                        // Do nothing...
                        else if (strCommand.StartsWith("*** Reconnected to"))
                        {
                            objInitialProtocol.objClient.NormalDisconnect = true;
                            Disconnect();
                        }
                        else
                        {
                            Send("[442] - Command: \"" + strCommand + "\"  not recognized - disconnecting");
                            Logs.Exception("B2Protocol [442] Command:\"" + strCommand + "\"  not recognized");
                            Disconnect();
                        }

                        break;
                    }

                case EB2States.ReceivingB2Acceptance:
                    {
                        B2Acceptance(strText);
                        break;
                    }

                case EB2States.SendingB2Messages:
                    {
                        break;
                    }
                // Do not accept data while sending messages
                case EB2States.WaitingForAcknowledgement:
                    {
                        if (strCommand.StartsWith(";"))
                        {
                        }
                        // Do nothing - this is a comment string
                        else if (strCommand.StartsWith("FC"))
                        {
                            B2ConfirmSentMessages();
                            StateChange(EB2States.ReceivingB2Proposal);
                            B2InboundProposal(strText, true);
                        }
                        else if (strCommand.StartsWith("FF"))
                        {
                            B2ConfirmSentMessages();
                            B2OutboundProposal();
                        }
                        else if (strCommand.StartsWith("FQ"))
                        {
                            B2ConfirmSentMessages();
                            objInitialProtocol.objClient.NormalDisconnect = true;
                            Disconnect(500);
                            Globals.blnFQSeen = true;
                        }
                        else if (strCommand.EndsWith(">"))
                        {
                            B2ConfirmSentMessages();
                            B2OutboundProposal();
                        }
                        else if (strCommand.StartsWith("S"))
                        {
                            Send("[441] - Command:\"" + strCommand + "\" not recognized - disconnecting");
                            Disconnect();
                        }
                        else if (strCommand.StartsWith("B"))
                        {
                            Disconnect();
                        }
                        else if (strCommand.StartsWith("*** Reconnected to"))
                        {
                            objInitialProtocol.objClient.NormalDisconnect = true;
                            Disconnect();
                        }
                        else
                        {
                            Send("[442] - Command: \"" + strCommand + "\"  not recognized - disconnecting");
                            Logs.Exception("B2Protocol [442] Command:\"" + strCommand + "\"  not recognized");
                            Disconnect();
                        }

                        break;
                    }

                case EB2States.ReceivingB2Proposal:
                    {
                        B2InboundProposal(strText);
                        break;
                    }

                case EB2States.ReceivingB2Messages:
                    {
                        Logs.Exception("B2Protocol [449] State ReceivingB2Messages, Command: " + strCommand);
                        Send("[449] Protocol error - disconnecting");
                        Disconnect();
                        break;
                    }

                case EB2States.Disconnecting:
                    {
                        break;
                    }
                    // Do nothing...
            }
        } // ProtocolCommands

        private void B2OutboundProposal()
        {
            // Sends a B2 Style proposal...

            int intProposalCount = aryOutboundMessages.Count;
            int intCumulativeSize = 0;
            bool blnSkip = false;
            intCheckSum = 0;
            intProposed = 0;

            // The total cumulative message transmission size is limited to 10,000 bytes for multiple messages
            // proposals. If a message exceeds 10,000 bytes then it must be sent with a single proposal...
            if (intProposalCount != 0)
            {
                if (intProposalCount > 5)
                    intProposalCount = 5;
                for (int intIndex = 0, loopTo = intProposalCount - 1; intIndex <= loopTo; intIndex++)
                {
                    Message objOutboundMessage = (Message)aryOutboundMessages[intIndex];
                    if (intIndex == 0)
                    {
                        intCumulativeSize = objOutboundMessage.CompressedSize();
                    }
                    else if (intCumulativeSize + objOutboundMessage.CompressedSize() > 10000)
                    {
                        blnSkip = true;
                    }
                    else
                    {
                        intCumulativeSize += objOutboundMessage.CompressedSize();
                    }

                    if (!blnSkip)
                    {
                        intProposed = intIndex + 1;
                        string strProposal = objOutboundMessage.B2Proposal();
                        if (!string.IsNullOrEmpty(strProposal))
                        {
                            var strTokens = strProposal.Split(' ');
                            intProposedCompressedSize[intIndex] = Convert.ToInt32(strTokens[4]);
                            foreach (char c in strProposal)
                                intCheckSum += Globals.Asc(c);
                            intCheckSum += Globals.Asc(Globals.CR[0]);
                            Send(strProposal);
                        }
                        else
                        {
                            objOutboundMessage.DeleteFile(Globals.SiteRootDirectory + @"To Winlink\" + objOutboundMessage.MessageId + ".mime");
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                intCheckSum = -intCheckSum;
                string sChecksum = "0" + (intCheckSum & 0xFF).ToString("X").Right(2);
                StateChange(EB2States.ReceivingB2Acceptance);
                Send("F> " + sChecksum, true);
            }
            else if (strLastCommandReceived is object && strLastCommandReceived.StartsWith("FF"))
            {
                StateChange(EB2States.WaitingForNewCommand); // End of session send FQ and queue disconnect
                Send("FQ");
                Globals.blnFQSeen = true;
                objInitialProtocol.objClient.NormalDisconnect = true;
                Disconnect(5000);
            }
            else
            {
                StateChange(EB2States.WaitingForNewCommand); // No messages to send - request a proposal from remote BBS...
                Send("FF", true);
                // setting Normal Disconnect = True will allow a normal disconnect even if the remote station disconnects before sending the "FQ"
                objInitialProtocol.objClient.NormalDisconnect = true;
                Globals.ResetProgressBar();
            }
        } // B2OutboundProposal

        private List<int> ParseAcceptance(string strText)
        {
            // Decodes the acceptance string from Winlink...

            var cllResponse = new List<int>();
            var sbdText = new StringBuilder();
            foreach (char c in strText)
            {
                switch (c)
                {
                    case 'Y':
                    case 'H':
                    case '+':
                        {
                            if (sbdText.Length > 0)
                            {
                                cllResponse.Add(Convert.ToInt32(sbdText.ToString()));
                                sbdText.Length = 0;
                            }

                            cllResponse.Add(0);
                            break;
                        }

                    case 'N':
                    case 'R':
                    case '-':
                        {
                            if (sbdText.Length > 0)
                            {
                                cllResponse.Add(Convert.ToInt32(sbdText.ToString()));
                                sbdText.Length = 0;
                            }

                            cllResponse.Add(-1);
                            break;
                        }

                    case 'L':
                    case '=':
                        {
                            if (sbdText.Length > 0)
                            {
                                cllResponse.Add(Convert.ToInt32(sbdText.ToString()));
                                sbdText.Length = 0;
                            }

                            cllResponse.Add(-2);
                            break;
                        }

                    case '!':
                        {
                            if (sbdText.Length > 0)
                            {
                                cllResponse.Add(Convert.ToInt32(sbdText.ToString()));
                            }

                            sbdText.Length = 0;
                            break;
                        }

                    case object _ when '0' <= c && c <= '9':
                        {
                            sbdText.Append(c);
                            break;
                        }
                }
            }

            if (sbdText.Length > 0)
                cllResponse.Add(Convert.ToInt32(sbdText.ToString()));
            return cllResponse;
        } // ParseAcceptance

        private void B2Acceptance(string strText)
        {
            // 
            // Processes the proposal acceptance response.
            // 
            int intCount = intProposed;
            if (intCount > 5)
                intCount = 5;
            var cllResponse = ParseAcceptance(strText);
            if (cllResponse.Count == intCount)
            {
                // Loop through all of the pending messages and send the compressed
                // binary image...
                StateChange(EB2States.SendingB2Messages);
                for (int intIndex = 0, loopTo = intCount - 1; intIndex <= loopTo; intIndex++)
                {
                    int intOffset = Convert.ToInt32(cllResponse[intIndex]);
                    Message objMessage = (Message)aryOutboundMessages[intIndex];
                    if (intOffset >= 0)
                    {
                        Globals.ResetProgressBar(intProposedCompressedSize[intIndex] - intOffset);
                        var bytBuffer = objMessage.B2Output(intOffset);
                        if (bytBuffer.Length < 6)
                        {
                            Globals.queChannelDisplay.Enqueue("R*** [428] Protocol error - Message Id: " + objMessage.MessageId + " Offset: " + intOffset.ToString() + " Failure to encode B2 format...");
                            Disconnect();
                            return;
                        }

                        Globals.queChannelDisplay.Enqueue("G*** Sending " + objMessage.MessageId + "...");
                        intCompressedMessageBytesSent += bytBuffer.Length;
                        intUncompressedMessageBytesSent += objMessage.Size();
                        objInitialProtocol.objClient.DataToSend(bytBuffer);
                        StateChange(EB2States.WaitingForAcknowledgement);
                    }
                    else if (intOffset == -1)
                    {
                        Globals.queChannelDisplay.Enqueue("R*** [498] Protocol error - Message Id: " + objMessage.MessageId + " Offset: " + intOffset.ToString() + " Failure to encode B2 format...");
                        objMessage.DeleteFile(Globals.SiteRootDirectory + @"To Winlink\" + objMessage.MessageId + ".mime");
                    }
                }

                if (!(enmB2State == EB2States.WaitingForAcknowledgement))
                    StateChange(EB2States.WaitingForNewCommand);
            }
            else
            {
                Logs.Exception("[B2Acceptance] Response Count/intCount: " + cllResponse.Count.ToString() + "/" + intCount.ToString());
                Globals.queChannelDisplay.Enqueue("R*** [418] Protocol error - response to proposals does not match. Expect " + intCount.ToString() + "  Got " + cllResponse.Count.ToString());
                Disconnect();
            }
        } // B2Acceptance

        private List<string> cllInboundProposals = new List<string>();
        private void B2InboundProposal(string strText, bool blnNew = false)
        {
            // Receives and responds to an inbound B2 proposal...
            byte[] bytReceived;
            Globals.Proposal objProp; // Holds inbound proposals

            if (blnNew)
            {
                intCheckSum = 0;
                cllInboundProposals = new List<string>();
                Globals.ResetProgressBar();
            }

            if (strText.StartsWith("FC"))
            {
                cllInboundProposals.Add(strText);
                foreach (char chrText in strText)
                    intCheckSum += Globals.Asc(chrText);
                intCheckSum += Globals.Asc(Globals.CR[0]);
            }
            else if (strText.StartsWith("F>"))
            {
                // Compare received and calculated check sum...
                var strTokens = strText.Split(' ');
                intCheckSum = -intCheckSum & 0xFF;
                var intVal = int.Parse(strTokens[1], System.Globalization.NumberStyles.HexNumber);
                if (intVal != intCheckSum)
                {
                    Logs.Exception("[B2InboundProposal] Inbound checksum does not match (Value/Checksum): " + intVal.ToString() + "/" + intCheckSum.ToString());
                    Send("[419] Protocol error - inbound checksum does not match");
                    Disconnect();
                    return;
                }

                intInboundMessageCount = 0;
                cllInboundMessageIDs.Clear();
                var sbdText = new StringBuilder();
                sbdText.Append("FS ");
                foreach (string strLine in cllInboundProposals)
                {
                    var strToks = strLine.Split(' ');
                    // FC EM mid uncompsize compsize 0
                    objProp = new Globals.Proposal();
                    objProp.msgID = strToks[2];
                    objProp.uncompressedSize = Convert.ToInt32(strToks[3]);
                    objProp.compressedSize = Convert.ToInt32(strToks[4]);
                    if (MidsSeen.IsMessageIdSeen(objProp.msgID))
                    {
                        sbdText.Append("N");
                    }
                    else
                    {
                        int intPartialCount = IsPartialMessage(objProp.msgID, strToks[4]);
                        intPartialCount = 0; // TODO:  Temporary force until partial is working
                        if (intPartialCount == 0)
                        {
                            sbdText.Append("Y");
                        }
                        else
                        {
                            sbdText.Append("!" + intPartialCount.ToString());
                        } // TODO: actual reply if partial available

                        cllInboundMessageIDs.Add(objProp);
                        intInboundMessageCount += 1;
                        int result = 0;
                        if (Int32.TryParse(strToks[4], out result))
                        {
                            Globals.ResetProgressBar(result - intPartialCount);
                        }
                    }
                }

                Send(sbdText.ToString());
                if (intInboundMessageCount > 0)
                {
                    objMessageInbound = new Message();
                    intInboundProposalIndex = 0;
                    objProp = (Globals.Proposal)cllInboundMessageIDs[intInboundProposalIndex];
                    objMessageInbound.MessageId = objProp.msgID;
                    objMessageInbound.intUncompressedSize = objProp.uncompressedSize;
                    string strMID = objProp.msgID;
                    bytReceived = objPartialSession.RetrievePartial(strMID);
                    if (true) // bytReceived.Length = 0 Then ' TODO: Temporary bypass until partial is working
                    {
                        objPartialSession.StartNewPartial(strMID);
                        StateChange(EB2States.ReceivingB2Messages);
                    }
                    else
                    {
                        objPartialSession.StartNewPartial(strMID);
                        StateChange(EB2States.ReceivingB2Messages);
                        B2MessageInbound(bytReceived); // This "reprocesses" the previously received data
                        objPartialSession.blnFirstRcvdBlk = true; // Set this so the first header info is stripped from the first received Block
                        objPartialSession.blnRemoveHdr8 = true;
                    } // Set this so the extra 8 byte header is also stripped (may not be in first block!)
                }
                else
                {
                    B2OutboundProposal();
                    return;
                }
            }
            else
            {
                Logs.Exception("[B2InboundProposal] Protocol Error: " + strText);
                Globals.queChannelDisplay.Enqueue("R*** [413] Protocol error - Disconnecting");
                Disconnect();
            }
        } // B2InboundProposal

        // Processes and inbound compressed binary message stream...
        private bool blnInProcess = false;
        private byte[] bytInProcess = null;
        private void B2MessageInbound(byte[] bytInbound) // Flag to indicate still processing, used to handle re entrancy
        {
            Globals.Proposal objProp;

            // Code modified from RMSLLite Packet Server:
            // Processes and inbound compressed binary message stream...
            if (blnInProcess) // this handles re entrancy
            {
                queInboundData.Enqueue(bytInbound);
                return;
            }
            else
            {
                bytInProcess = bytInbound;
                queInboundData.Clear();
            }

            blnInProcess = true;
            var intResult = default(int);
            int intPosition;
        Reprocess:
            ;

            // Strip the header on the first block of a new parial recovery 
            int intHdr2Sumcheck = objPartialSession.StripHeaderFromPartial(ref bytInProcess);
            if (intHdr2Sumcheck != 0)
            {
                objMessageInbound.intB2Checksum = intHdr2Sumcheck;
            }

            intPosition = 0;
            Globals.UpdateProgressBar(bytInbound.Length);
            foreach (byte byt in bytInProcess)
            {
                intPosition += 1;
                intResult = objMessageInbound.BinaryInput(byt);
                objPartialSession.AccumulateByte(byt);
                if (intResult == 2)
                {
                    // All OK - Block boundary ...update partial upon block boundary (every 250 bytes)
                    objPartialSession.WritePartialToFile();
                }
                else if (intResult == 1)
                {
                    objPartialSession.PurgeCurrentPartial(); // no longer needed as message now complete
                    if (objMessageInbound.Decompress(true) == false)
                    {
                        Logs.Exception("B2Protocol [423] Unable to decompress Inbound Message ");
                        // objInitialProtocol.objClient.Break() ' Break for Pactor, has no affect on other channels TODO: verify placement
                        Send("[423] Protocol Error - Unable to decompress received binary compressed message...");
                        Disconnect();
                        blnInProcess = false;
                        queInboundData.Clear();
                        return;
                    }

                    if (objMessageInbound.DecodeB2Message() == false)
                    {
                        Logs.Exception("B2Protocol [424] Unable to decode received binary compressed message...");
                        // objInitialProtocol.objClient.Break() ' Break for Pactor, has no affect on other channels TODO: verify placement
                        Send("[424] Protocol Error - Unable to decode received binary compressed message...");
                        Disconnect();
                        blnInProcess = false;
                        queInboundData.Clear();
                        return;
                    }

                    intInboundProposalIndex += 1;
                    intInboundMessageCount -= 1;

                    // If the message has been successfully received and saved then
                    // record the message ID as having been seen and processed...
                    if (objMessageInbound.SaveToFile(Globals.SiteRootDirectory + @"From Winlink\" + objMessageInbound.MessageId + ".mime"))
                    {
                        MidsSeen.AddMessageId(objMessageInbound.MessageId);
                    }

                    // Save statistics...
                    intCompressedMessageBytesReceived += objMessageInbound.Progress;
                    intUncompressedMessageBytesReceived += objMessageInbound.Size();

                    // Show message complete...
                    Globals.queChannelDisplay.Enqueue("G*** " + objMessageInbound.MessageId + " - " + objMessageInbound.Size().ToString() + "/" + objMessageInbound.Progress.ToString() + " bytes received");
                    if (intInboundMessageCount == 0)
                    {
                        objMessageInbound = null;
                        StateChange(EB2States.SendingB2Proposal);
                        B2OutboundProposal();
                        blnInProcess = false; // Added Rev  2.0.22.0 to fix second inbound session
                        queInboundData.Clear();
                        return;
                    }
                    else
                    {
                        objMessageInbound = new Message();
                        break;
                    }
                }
                else if (intResult < 0)
                {
                    objPartialSession.PurgeCurrentPartial(); // If error delete current partial to avoid potential loop.
                    Logs.Exception("[B2MessageInbound] Protocol Error Problem receiving B2 message...nResult: " + intResult.ToString());
                    // objInitialProtocol.objClient.Break() ' Break for Pactor, has no affect on other channels TODO: verify placement
                    Send("[425/" + intResult.ToString() + "] Protocol Error - Problem receiving B2 message...");
                    Disconnect();
                    blnInProcess = false;
                    queInboundData.Clear();
                    return;
                }
            }

            if (intResult == 0 | intResult == 2)
            {
                if (queInboundData.Count == 0)
                {
                    blnInProcess = false;
                    return;
                }
                else
                {
                    try
                    {
                        bytInProcess = (byte[])queInboundData.Dequeue();
                    }
                    catch
                    {
                        blnInProcess = false;
                        return;
                    }

                    while (queInboundData.Count > 0)
                    {
                        try
                        {
                            Globals.AppendBuffer(ref bytInProcess, (byte[])queInboundData.Dequeue());
                        }
                        catch
                        {
                            return;
                        }
                    }

                    goto Reprocess;
                }
            }
            // There may be all or part of the next message in the bytInProcess() array...
            else if (intPosition < bytInProcess.Length)
            {
                var binMore = new byte[(bytInProcess.Length - intPosition)];
                for (int intCount = 0, loopTo = binMore.Length - 1; intCount <= loopTo; intCount++)
                {
                    binMore[intCount] = bytInProcess[intPosition];
                    intPosition += 1;
                }

                objProp = (Globals.Proposal)cllInboundMessageIDs[intInboundProposalIndex];
                string strMID = objProp.msgID;
                // Dim strTemp() As String = CStr(cllAcceptedInboundMids(intInboundProposalIndex)).Split(Chr(32))
                var bytReceived = objPartialSession.RetrievePartial(strMID);
                if (true) // If bytReceived.Length = 0 Then ' TODO: This temporarily bypasses partial file recovery
                {
                    objPartialSession.StartNewPartial(strMID);
                    bytInProcess = binMore;
                    while (queInboundData.Count > 0)
                    {
                        try
                        {
                            Globals.AppendBuffer(ref bytInProcess, (byte[])queInboundData.Dequeue());
                        }
                        catch
                        {
                            break;
                        }
                    }

                    goto Reprocess;
                }
                else // Here is where partial recovery is and there is still a latent bug here TODO:
                {
                    bytInProcess = bytReceived;
                    objPartialSession.StartNewPartial(strMID);

                    // Set this so the first header info is stripped from the first received Block...
                    objPartialSession.blnFirstRcvdBlk = true;
                    // ' Set this so the extra 8 byte header is also stripped (may not be in first block!)
                    objPartialSession.blnRemoveHdr8 = true;
                    objPartialSession.StripHeaderFromPartial(ref binMore); // TODO: not sure what to do with sumcheck?
                    objPartialSession.blnFirstRcvdBlk = false;
                    objPartialSession.blnRemoveHdr8 = false;
                    Globals.AppendBuffer(ref bytInProcess, binMore);
                    while (queInboundData.Count > 0)
                    {
                        try
                        {
                            Globals.AppendBuffer(ref bytInProcess, (byte[])queInboundData.Dequeue());
                        }
                        catch
                        {
                            break;
                        }
                    }

                    objPartialSession.StartNewPartial(strMID);
                    goto Reprocess;
                }
            }

            blnInProcess = false;
        } // B2MessageInbound

        private void B2ConfirmSentMessages()
        {
            // Deletes outbound messages when they are confirmed...

            int intCount = intProposed;
            if (intCount > 5)
                intCount = 5;
            for (int intIndex = 1, loopTo = intCount; intIndex <= loopTo; intIndex++)
            {
                Message objMessage = (Message)aryOutboundMessages[0];
                if (objMessage.MessageId.Length != 0)
                {
                    objMessage.DeleteFile(Globals.SiteRootDirectory + @"To Winlink\" + objMessage.MessageId + ".mime");
                }

                aryOutboundMessages.RemoveAt(0);
            }

            StateChange(EB2States.WaitingForNewCommand);
        } // B2ConfirmSentMessages

        private void Disconnect(int intMsDelay = 3000)
        {
            StateChange(EB2States.Disconnecting);
            tmrDisconnect = new System.Timers.Timer();
            tmrDisconnect.Elapsed += OnDisconnectTimer;
            tmrDisconnect.AutoReset = false;
            tmrDisconnect.Interval = intMsDelay;
            tmrDisconnect.Enabled = true;
        } // Disconnect

        private void OnDisconnectTimer(object source, ElapsedEventArgs e)
        {
            objInitialProtocol.objClient.Disconnect();
        } // OnDisconnectTimer

        private int IsPartialMessage(string MID, string Length)
        {
            // Function to check and return if any partially received MIDs are saved
            // Returns 0 if none or if current file size >= Length or an error,
            // otherwise the length of data already received.
            var intBytesReceived = default(int);
            byte[] bytFiledata;
            try
            {
                if (File.Exists(Globals.SiteRootDirectory + @"Temp Inbound\" + MID + ".indata"))
                {
                    // Return the bytes accumulated in the file...
                    bytFiledata = File.ReadAllBytes(Globals.SiteRootDirectory + @"Temp Inbound\" + MID + ".indata");

                    // Assume any partial will be a multiple of 250 real bytes as the last block of < 250 bytes
                    // would indicate a completed file...
                    if (bytFiledata.Length > 255)
                    {
                        intBytesReceived = bytFiledata.Length - (bytFiledata[1] + 2 + 2 * (bytFiledata.Length / 250));
                        if (intBytesReceived < 250 | intBytesReceived % 250 != 0)
                        {
                            // Problem with partial... 
                            Logs.Exception("IsPartialMessage: Computed Bytes received is not modulo 250, Partial MID: " + MID + " Purged!");
                            File.Delete(Globals.SiteRootDirectory + @"Temp Inbound\" + MID + ".indata");
                            intBytesReceived = 0;
                        }
                        else if (intBytesReceived >= Convert.ToInt32(Length))
                        {
                            Logs.Exception("IsPartialMessage: Partial Length exceeded proposed length, Partial MID: " + MID + " Purged!");
                            File.Delete(Globals.SiteRootDirectory + @"Temp Inbound\" + MID + ".indata");
                            intBytesReceived = 0;
                        }
                    }
                    else
                    {
                        Logs.Exception("IsPartialMessage: Partial Files size < 255, Partial MID: " + MID + " Purged!");
                        File.Delete(Globals.SiteRootDirectory + @"Temp Inbound\" + MID + ".indata");
                    }
                }

                return intBytesReceived;
            }
            catch (Exception e)
            {
                File.Delete(Globals.SiteRootDirectory + @"Temp Inbound\" + MID + ".indata");
                Logs.Exception("[IsPartialMessage] " + e.Message);
                return 0;
            }
        } // IsPartialMessage
    } // B2Protocol
}