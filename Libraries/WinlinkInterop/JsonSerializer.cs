using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WinlinkInterop
{
    public sealed class JsonSerializer<T> where T : class
    {
        /// <summary>
        /// DeSerializes an object from a JSON string
        /// </summary>
        public static T DeSerialize(string json)
        {
            try
            {
                using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return serializer.ReadObject(stream) as T;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}