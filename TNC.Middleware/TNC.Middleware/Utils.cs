using System.Linq;
using System.Text;

namespace TNC.Middleware
{
    public static class Utils
    {
        /// <summary>
        /// Converts a string into an array of bytes
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }

        /// <summary>
        /// Converts a byte array to a text string
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetString(byte[] buffer)
        {
            return Encoding.ASCII.GetString(buffer);
        }

        /// <summary>
        /// Converts a partial byte array to a text string
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static string GetString(byte[] buffer, int first = 0, int last = -1)
        {
            var count = buffer.Skip(first).Count();
            if (last > -1) count -= last - first;
            var a = buffer.Skip(first).Take(count).ToArray();
            return GetString(a);
        }

    }
}
