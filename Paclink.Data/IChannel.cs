using System;
using System.Collections.Generic;
using System.Text;

namespace Paclink.Data
{
    public interface IChannel
    {
        void AddChannelRecord(bool isPacket, string callsign, int frequency, string gridSquare, int mode, string operatingHours, string serviceCode);

        bool ContainsChannelList(bool isPacket);

        void ClearChannelList(bool isPacket);

        SortedDictionary<string, string> GetChannelList(bool isPacket);

        void WriteScript(string channelName, string script);

        string GetScript(string channelName);
    }
}
