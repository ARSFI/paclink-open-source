using Paclink.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink
{
    public class DialogPactorConnectViewModel : IPactorConnectBacking
    {
        private ChannelProperties stcChannel;
        private IModem _objModem;

        public bool TNCBusyHold
        {
            get
            {
                return stcChannel.TNCBusyHold;
            }
        }

        public string SiteRootDirectory
        {
            get
            {
                return Globals.SiteRootDirectory;
            }
        }

        public bool PactorDialogResume
        {
            get
            {
                return Globals.blnPactorDialogResume;
            }
            set
            {
                Globals.blnPactorDialogResume = value;
                Globals.Settings.Save("Properties", "Pactor Dialog Resume", value);
            }
        }

        public bool PactorDialogResuming
        {
            get
            {
                return Globals.blnPactorDialogResuming;
            }
            set
            {
                Globals.blnPactorDialogResuming = value;
            }
        }

        public string TNCType
        {
            get
            {
                return stcChannel.TNCType;
            }
        }

        public int AudioToneCenter
        {
            get
            {
                return Convert.ToInt32(stcChannel.AudioToneCenter);
            }
        }

        public int WindowTop
        {
            get
            {
                return Globals.Settings.Get("Pactor Control", "Top", 100);
            }
            set
            {
                Globals.Settings.Save("Pactor Control", "Top", value);
            }
        }

        public int WindowLeft
        {
            get
            {
                return Globals.Settings.Get("Pactor Control", "Left", 100);
            }
            set
            {
                Globals.Settings.Save("Pactor Control", "Left", value);
            }
        }

        public string ChannelName
        {
            get
            {
                return stcChannel.ChannelName;
            }
        }

        public string ServiceCodes
        {
            get
            {
                return Globals.strServiceCodes;
            }
        }

        public string RemoteCallsign
        {
            get 
            {
                return stcChannel.RemoteCallsign;
            }
        }

        public string CenterFrequency
        {
            get
            {
                return stcChannel.RDOCenterFrequency;
            }
        }

        public IEnumerable<string> ChannelNames
        {
            get
            {
                var aryResults = Channels.ParseChannelList(false);

                foreach (string strStationCallsign in aryResults)
                {
                    yield return strStationCallsign.Substring(0, strStationCallsign.IndexOf(":"));
                }

                foreach (string station in aryResults)
                {
                    int intIndex = station.IndexOf(":");
                    string strFreqList = station.Substring(intIndex + 1);
                    if (Globals.AnyUseableFrequency(strFreqList, ""))
                    {
                        yield return station.Substring(0, intIndex);
                    }
                }
            }
        }

        public bool CanRadioControl
        {
            get
            {
                return Globals.objRadioControl != null;
            }
        }

        public bool IsValidFrequency
        {
            get
            {
                int argintFreqHz = 0;
                return Globals.IsValidFrequency(Globals.StripMode(stcChannel.RDOCenterFrequency), intFreqHz: ref argintFreqHz);
            }
        }

        public DialogPactorConnectViewModel(IModem objSender, ref ChannelProperties Channel)
        {
            _objModem = objSender;
            stcChannel = Channel;
        }

        public IEnumerable<string> GetFrequencies(string callsign, string tncName)
        {
            var aryResults = Channels.ParseChannelList(false);

            for (int i = 0, loopTo = aryResults.Length - 1; i <= loopTo; i++)
            {
                if (aryResults[i].StartsWith(callsign))
                {
                    var aryFrequencies = aryResults[i].Substring(aryResults[i].IndexOf(":") + 1).Split(',');
                    if (aryFrequencies.Length > 1)
                    {
                        for (int j = 0, loopTo1 = aryFrequencies.Length - 1; j <= loopTo1; j++)
                        {
                            var strFreqEntry = aryFrequencies[j].ToString();
                            if (Globals.CanUseFrequency(strFreqEntry, tncName))
                            {
                                yield return Globals.FormatFrequency(strFreqEntry);
                            }
                        }
                    }

                    break;
                }
            }

            stcChannel.RemoteCallsign = callsign;
        }

        public bool IsValidCallsign(string callsign)
        {
            return Globals.IsValidRadioCallsign(callsign);
        }

        public string GetCenterFreq(string channelInfo)
        {
            return Globals.ExtractFreq(channelInfo);
        }

        public bool IsValidFreq(string centerFreq, out int intFreqHz)
        {
            int resultIntFreq = 0;
            var result = Globals.IsValidFrequency(centerFreq, ref resultIntFreq);
            intFreqHz = resultIntFreq;
            return result;
        }

        public void PollModem()
        {
            _objModem.Poll();
        }

        public void UpdateChannel(string remoteCallsign, string centerFrequency)
        {
            stcChannel.RemoteCallsign = remoteCallsign.Trim().ToUpper();
            stcChannel.RDOCenterFrequency = centerFrequency.Trim();
            Globals.stcEditedSelectedChannel = stcChannel;
        }

        public void SetRadioControlInfo(string centerFreq)
        {
            stcChannel.RDOCenterFrequency = centerFreq;
            if (Globals.objRadioControl != null)
            {
                Globals.objRadioControl.SetParameters(ref stcChannel);
            }
        }

        public bool RefreshRadioControlInfo()
        {
            return Globals.objRadioControl.SetParameters(ref stcChannel);
        }

        public void FormClosing()
        {
            Globals.blnPactorDialogClosing = true;
            Globals.blnPactorDialogResuming = false;
            Globals.blnChannelActive = false;
            Globals.ObjSelectedModem.Close();
            Globals.ObjSelectedModem = null;
            Globals.stcEditedSelectedChannel = default;
        }

        public void FormClosed()
        {
            // empty
        }

        public void FormLoading()
        {
            // empty
        }

        public void FormLoaded()
        {
            // empty
        }

        public void RefreshWindow()
        {
            // empty
        }

        public void CloseWindow()
        {
            // empty
        }
    }
}
