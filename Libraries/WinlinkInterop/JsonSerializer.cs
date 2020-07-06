using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WinlinkInterop
{
    public sealed class JsonSerializer<T> where T : class
    {
        /// <summary>
    /// Serializes an object to a JSON string
    /// </summary>
        public static string Serialize(T instance)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, instance);
                    return Encoding.Default.GetString(stream.ToArray());
                }
            }
            catch
            {
                return null;
            }
        }

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