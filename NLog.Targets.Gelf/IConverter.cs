using Newtonsoft.Json.Linq;
using NLog.Layouts;

namespace NLog.Targets.Gelf
{
    public interface IConverter
    {
        JObject GetGelfJson(LogEventInfo logEventInfo, string facility, Layout layout);
    }
}