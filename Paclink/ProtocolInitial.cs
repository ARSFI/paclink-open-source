using Paclink.Data;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace Paclink
{
    public class ProtocolInitial
    {
        // Declare Function CHALLENGED_PASSWORD Lib "VBSUPPORT.DLL" (ByVal sChallengePhrase As String, _
        // ByVal sPassword As String, ByRef lPassword As UInt32) As Boolean

        public enum EProtocolStates
        {
            Undefined,
            Connected,      // State when initiating the link locally
            Accepted,       // State after exchange of SIDs
            Disconnecting,  // State while disconnecting
            Disconnected   // State while disconnected
        } // EProtocolStates

        public IModem ObjModem;
        private ChannelProperties stcChannel;
        private ProtocolB2 objB2Protocol;
        private EProtocolStates ProtocolState;  // The protocol state variable
        private ArrayList aryOutboundMessages;  // Holds pending outbound messages
        private string strSID;                  // Holds this channel's SID
        private string strBaseConnectedCall;    // The "connected to" callsign without the SSID
        private bool blnIFlag;               // Set if remote station accepts ID string
        private System.Timers.Timer tmrDisconnect;     // Used to delay disconnect
        private string strSecureReply = "";     // Used to hold optional secure reply
        private bool blnSecureLogin = false; // Flag used to indicate secure login expected
        private string strChallengePhrase = ""; // Challenge phrase as received by ;PQ: command

        public ProtocolInitial(IModem Parent, ref ChannelProperties strNewChannel)
        {
            ObjModem = Parent;
            stcChannel = strNewChannel;
            Globals.strConnectedCallsign = strNewChannel.RemoteCallsign; // Set the global callsign for use by Radar
            Globals.strConnectedGridSquare = ""; // and clear the connected Grid square
            strSID = "[Paclink-" + Application.ProductVersion + "-" + SSIDTag() + "B2FIHM$]";
            ProtocolStateChange(EProtocolStates.Connected);
        } // New

        // Receives binary data from the channel...
        private StringBuilder sbdDataIn = new StringBuilder();
        public void ChannelInput(ref byte[] bytData) // made static on 2.0.53.0
        {
            if (ProtocolState != EProtocolStates.Accepted)
            {
                foreach (byte byt in bytData)
                {
                    if ((int)byt != 0)
                        sbdDataIn.Append((char)byt);
                }

                this.ChannelInput(sbdDataIn.ToString());
                sbdDataIn.Length = 0;
            }
            else if (objB2Protocol is object)
            {
                objB2Protocol.ChannelInput(ref bytData);
            }
        } // ChannelInput (Byte())

        // Receives string data from the channel... 
        private StringBuilder sbdDataOut = new StringBuilder();
        public void ChannelInput(string strData)  // moved here on 2.0.53.0 from after the next line
        {
            if (ProtocolState != EProtocolStates.Accepted)
            {
                for (int intIndex = 0, loopTo = strData.Length - 1; intIndex <= loopTo; intIndex++)
                {
                    if (strData[intIndex] != '\n')
                    {
                        if (strData[intIndex] == '\r')
                        {
                            this.ProtocolCommands(sbdDataOut.ToString(), false);
                            sbdDataOut.Length = 0;
                        }
                        else
                        {
                            sbdDataOut.Append(strData[intIndex]);
                        }
                    }
                }
            }
            else if (objB2Protocol is object)
            {
                objB2Protocol.ChannelInput(strData);
            }
        } // ChannelInput (String)

        public void CloseProtocol()
        {
            objB2Protocol = null;
            Globals.queStateDisplay.Enqueue("");
            Globals.queProgressDisplay.Enqueue(0);
            Globals.strConnectedGridSquare = ""; // and clear the connected Grid square
        } // CloseProtocol

        private string SSIDTag()
        {
            // Creates the SSID feature for the SID...

            var strTokens = Globals.SiteCallsign.Split('-');
            int result = 0;
            if (strTokens.Length < 2)
            {
                return "N00";
            }
            else if (Int32.TryParse(strTokens[1], out result))
            {
                return "N" + result.ToString("0#");
            }
            else
            {
                return "N00";
            }
        } // SSIDTag

        public void LinkStateChange(ConnectionOrigin enmLink)
        {
            // Receives link state changes from the channel client...

            switch (enmLink)
            {
                case ConnectionOrigin.Disconnected:
                    {
                        Globals.strConnectedGridSquare = ""; // clear the connected Grid square
                        ProtocolStateChange(EProtocolStates.Disconnected);
                        break;
                    }

                case ConnectionOrigin.OutboundConnection:
                    {
                        ProtocolStateChange(EProtocolStates.Connected);
                        break;
                    }
            }
        } // LinkState

        private void ProtocolStateChange(EProtocolStates enmNewState)
        {
            // This function is called to change the state of the protocol...

            Globals.queStatusDisplay.Enqueue(enmNewState.ToString());
            if (ProtocolState != enmNewState)
            {
                ProtocolState = enmNewState;
                var switchExpr = ProtocolState;
                switch (switchExpr)
                {
                    case EProtocolStates.Disconnected:
                        {
                            if (objB2Protocol != null)
                            {
                                objB2Protocol.Close();
                                objB2Protocol = null;
                            }

                            break;
                        }

                    case EProtocolStates.Connected:
                        {
                            if (objB2Protocol != null)
                                objB2Protocol = null;
                            GetPendingOutboundMessages();
                            break;
                        }
                }
            }
        } // NewState

        private void Send(string strText, bool blnCR = true)
        {
            // Sends a text stream to the channel...

            Globals.queChannelDisplay.Enqueue("B" + strText);
            if (blnCR)
            {
                ObjModem.DataToSend(strText + Globals.CR);
            }
            else
            {
                ObjModem.DataToSend(strText);
            }
        } // Send

        private void ParseInboundSID(string strText)
        {
            // Checks the remote station's SID for critical protocol flags...

            var sTokens = strText.Split('-');
            string sFeatures = sTokens[sTokens.Length - 1];
            if (sFeatures.IndexOf("I") != -1)
                blnIFlag = true;
            else
                blnIFlag = false;
            if (sFeatures.IndexOf("B2") == -1)
            {
                Send("B2F protocol required");
                Disconnect();
            }
        } // ParseInboundSID

        private void GetPendingOutboundMessages()
        {
            // Fill the outbound message collection... 

            aryOutboundMessages = new ArrayList();
            var aryOutboundMessageIDs = new ArrayList();
            var aryTemporaryOutboundMessages = new ArrayList();

            // Find all files pending for delivery...
            var messageStore = new MessageStore(DatabaseFactory.Get());
            var messages = messageStore.GetToWinlinkMessages();
            if (messages.Count > 1)
                Globals.queChannelDisplay.Enqueue("G*** Sorting " + messages.Count + " outbound messages for precedence...");
            for (int intIndex = 0; intIndex <= 4; intIndex++) // step through each of the Precedence classes Highest to lowest order
            {
                aryTemporaryOutboundMessages.Clear();
                foreach (var kvp in messages)
                {
                    var objMessage = new Message(kvp.Key);
                    if (objMessage.Subject.ToUpper().IndexOf("//MARS " + "ZOPRM".Substring(intIndex, 1) + "/") != -1)
                    {
                        if (!string.IsNullOrEmpty(objMessage.MessageId) & !string.IsNullOrEmpty(objMessage.Mime) & objMessage.CompressedSize() > 0)
                        {
                            aryTemporaryOutboundMessages.Add(objMessage);
                        }
                        else
                        {
                            Globals.queChannelDisplay.Enqueue("R*** " + kvp.Key + " failed");
                            Globals.queChannelDisplay.Enqueue("R*** Check 'Failed Mime' table");
                            messageStore.SaveFailedMimeMessage(kvp.Key, kvp.Value);
                        }
                    }

                    if (objMessage.Subject.ToUpper().IndexOf("//WL2K " + "ZOPR ".Substring(intIndex, 1).Trim() + "/") != -1 | intIndex == 4 & objMessage.Subject.ToUpper().IndexOf("//WL2K") != -1)
                    {
                        if (!string.IsNullOrEmpty(objMessage.MessageId) & !string.IsNullOrEmpty(objMessage.Mime) & objMessage.CompressedSize() > 0)
                        {
                            aryTemporaryOutboundMessages.Add(objMessage);
                        }
                        else
                        {
                            Globals.queChannelDisplay.Enqueue("R*** " + kvp.Key + " failed");
                            Globals.queChannelDisplay.Enqueue("R*** Check 'Failed Mime' table");
                            messageStore.SaveFailedMimeMessage(kvp.Key, kvp.Value);
                        }
                    }
                }

                if (aryTemporaryOutboundMessages.Count > 0)
                {
                    aryTemporaryOutboundMessages.Sort(); // Sort messages of like precedence in size order
                    foreach (Message objMessage in aryTemporaryOutboundMessages)
                    {
                        if (aryOutboundMessageIDs.Contains(objMessage.MessageId) == false)
                        {
                            aryOutboundMessageIDs.Add(objMessage.MessageId);
                            aryOutboundMessages.Add(objMessage);
                        }
                    }
                }
            }
        } // GetPendingOutboundMessages

        private void ProtocolCommands(string strText, bool blnCrLf)
        {
            // Interprets the protocol commands as they are received...

            Globals.queChannelDisplay.Enqueue("X" + strText);
            string strCommand = strText.ToUpper();
            if (strCommand.StartsWith(";PM:"))
            {
                // Ignore preview-message commands
                return;
            }

            var switchExpr = ProtocolState;
            switch (switchExpr)
            {
                case EProtocolStates.Connected:
                    {
                        if (string.IsNullOrEmpty(Globals.strConnectedGridSquare)) // test for one
                        {
                            IsGridSquare(strCommand);
                        }

                        if (strCommand.StartsWith("[") & strText.EndsWith("]"))
                        {
                            ParseInboundSID(strText);
                        }
                        else if (strText.EndsWith(">"))
                        {
                            ProtocolStateChange(EProtocolStates.Accepted);
                            SendFWFeature();
                            Send(strSID);
                            if (blnSecureLogin)
                            {
                                uint intResult;
                                // This is compatible with WL2K secure login and generates the same ;PR: reply as AirMail 
                                intResult = WinlinkAuth.ChallengedPassword(strChallengePhrase, Globals.SecureLoginPassword);
                                strSecureReply = ";PR: " + intResult.ToString("0000000000").Substring(2, 8);
                                Send(strSecureReply);
                            }

                            if (blnIFlag)
                                Send("; " + stcChannel.RemoteCallsign + " DE " + Globals.SiteCallsign + " (" + Globals.SiteGridSquare + ") QTC " + aryOutboundMessages.Count.ToString() + "...");
                            objB2Protocol = new ProtocolB2(this, ref stcChannel, ref aryOutboundMessages);
                        }
                        else if (strCommand.StartsWith(";PQ: ")) // Check for secure login challenge phrase
                        {
                            strChallengePhrase = strCommand.Substring(4).Trim();
                            uint temp = 0;
                            if (!UInt32.TryParse(strChallengePhrase, out temp))
                            {
                                Globals.queChannelDisplay.Enqueue("R*** Non numeric challange phrase - ending connection");
                                Disconnect();
                            }
                            else
                            {
                                blnSecureLogin = true;
                            }
                        }

                        break;
                    }

                case EProtocolStates.Accepted:
                    {
                        objB2Protocol.ChannelInput(strText);
                        break;
                    }
            }
        } // ProtocolCommands

        private void Disconnect()
        {
            ProtocolState = EProtocolStates.Disconnecting;
            tmrDisconnect = new System.Timers.Timer();
            tmrDisconnect.Elapsed += OnDisconnectTimer;
            tmrDisconnect.AutoReset = false;
            tmrDisconnect.Interval = 2000;
            tmrDisconnect.Enabled = true;
        } // Disconnect

        private void OnDisconnectTimer(object source, ElapsedEventArgs e)
        {
            ObjModem.Disconnect();
            tmrDisconnect.Dispose();
            tmrDisconnect = null;
        } // OnDisconnectTimer

        private string ChallengePhrase()
        {
            // Function used to generate a random challenge phrase for secure login (not used for Client)
            var rng = new Random();
            return ";PQ: " + rng.Next(0, 100000000).ToString() + Globals.CR;
        } // ChallengePhrase

        private void SendFWFeature()
        {
            // This creates and sends the new ;FW feature
            // Typical FW reply  ";FW: W1ABC-8 WILLY-FC4 WILLY-FC5 NILLY
            string strFW = ";FW: " + Globals.SiteCallsign;
            try
            {
                var strAccountNames = Globals.Settings.Get("Properties", "Account Names", "").Split('|');
                foreach (string strAccount in strAccountNames)
                {
                    if ((strAccount ?? "") != (Globals.SiteCallsign ?? ""))
                    {
                        string strPassword = Globals.Settings.Get(strAccount, "EMail Password", "");
                        strFW += " " + MakeFWentry(strChallengePhrase, strAccount, strPassword);
                    }
                }
            }
            catch
            {
            }

            Send(strFW);
        } // SendFWFeature

        private string MakeFWentry(string strChallengePhrase, string strCallsign, string strPassword)
        {
            // 
            // Make a callsign/tactical address entry for the ;FW command.  If requested, include the password hascode.
            // 
            string strFW = strCallsign;
            if (!string.IsNullOrEmpty(strChallengePhrase) & !string.IsNullOrEmpty(strPassword))
            {
                uint intResult = WinlinkAuth.ChallengedPassword(strChallengePhrase, strPassword);
                strFW += "|" + intResult.ToString("0000000000").Substring(2, 8);
            }

            return strFW;
        }

        private bool IsGridSquare(string strCommandLine)
        {
            // Function to scan  for grid square in initial sign on of protocol...
            int intPtr1 = strCommandLine.IndexOf("(");
            int intPtr2 = strCommandLine.IndexOf(")");
            string strAlpha = "ABCDEFGHIJKLMNOPQRSTUVWX";
            string strNumeric = "0123456789";
            if (intPtr1 == -1 | intPtr2 == -1 | intPtr2 - intPtr1 < 5)
            {
                return false;
            }
            else
            {
                string strGS = strCommandLine.Substring(1 + intPtr1, intPtr2 - (intPtr1 + 1)).Trim().ToUpper();
                if (!(strGS.Length == 4 | strGS.Length == 6 | strGS.Length == 8))
                    return false;
                if (strAlpha.IndexOf(strGS.Substring(0, 1)) != -1 & strAlpha.IndexOf(strGS.Substring(1, 1)) != -1 & strNumeric.IndexOf(strGS.Substring(2, 1)) != -1 & strNumeric.IndexOf(strGS.Substring(3, 1)) != -1)


                {
                    if (strGS.Length == 4)
                    {
                        Globals.strConnectedGridSquare = strGS;
                        return true;
                    }

                    if (strGS.Length >= 6)
                    {
                        if (strAlpha.IndexOf(strGS.Substring(4, 1)) != -1 & strAlpha.IndexOf(strGS.Substring(5, 1)) != -1)
                        {
                            Globals.strConnectedGridSquare = strGS.Substring(0, 6); // truncate 8 GS to 6 for now
                            return true;
                        }
                    }
                }
            }

            return false;
        } // IsGridSquare
    }
}