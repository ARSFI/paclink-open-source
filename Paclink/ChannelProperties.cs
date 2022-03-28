using Paclink.UI.Common;
using System;

namespace Paclink
{
    public struct ChannelProperties
    {
        // 
        // This structure holds the properties for any declared channel. The
        // Channels class, declared below, is used to save and recall the
        // declared channels. The value held in ChannelProperties.ChannelType
        // is used to determine which of the members of ChannelProperties is
        // saved to or recovered from the registry for any given channel...
        // 
        // All channels types...
        public DateTime StartTimestamp;
        public ChannelMode ChannelType;
        public string ChannelName;
        public int Priority;
        public string RemoteCallsign;
        // Public FrequenciesScanned As Integer
        public bool Enabled;
        public bool EnableAutoforward;

        // Telnet specific properties...
        // Public LocalIPAddressIndex As Integer

        // PacketAGW specific properties...
        public string AGWPort;
        public int AGWTimeout;
        public int AGWPacketLength;
        public int AGWMaxFrames;
        public string AGWScript;
        public int AGWScriptTimeout;
        public bool AGWAllowInbound;

        // Packet and Pactor TNC specific properties...
        public string TNCType;
        public int TNCTimeout;
        public string TNCSerialPort;
        public string TNCBaudRate;
        public string TNCConfigurationFile;
        public bool TNCConfigureOnFirstUseOnly;
        public string TNCScript; // Not used in Pactor
        public int TNCScriptTimeout; // Not used in Pactor
        public int TNCFSKLevel;
        public int TNCPSKLevel;
        public int TNCPort;
        public bool TNCBusyHold;
        public int TNCOnAirBaud;

        // WINMOR Channel Specific Properties
        public string WMCaptureDevice;
        public string WMPlaybackDevice;
        public int WMXmitLevel;
        public int WMBandwidth;
        public string WMPTTControl;
        public string WMSerialCtrlSignals;
        public bool WMcwID;
        public bool WMDebugLog;

        // Radio control specific properties...
        public string RDOControl;           // Manual or Direct Serial
        public string RDOControlPort;       // Used for radio control
        public string RDOControlBaud;       // Baud rate for radio control (8N1 assumed)
        public string RDOCenterFrequency;   // Center frequency last selected in KHZ and may include " (p3)" tag
        public string RDOModel;             // The radio model 
        public string AudioToneCenter;      // The center (average) of the Mark/Space tones set from the initialization file
        public string CIVAddress;           // The CIV address (Icom radios only)
        public bool NarrowFilter;           // True to use narrow filter for P1 and P2
        public bool PactorId;               // Flag to indicate whether to ID on Pactor 1 FEC
        public bool TTLLevel;               // Flag to indicate PTC II TTL or RS-232 Levels
        public bool NMEA;                   // Flag to indicate use of NMEA commands (Icom only)
    } 
}
