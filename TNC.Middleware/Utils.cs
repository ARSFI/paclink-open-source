using System;
using System.Linq;
using System.Text;
using TNC.Middleware.Properties;

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
        /// Converts all or a portion of a byte array to a text string
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static string GetString(byte[] buffer, int first = 0, int last = -1)
        {
            if (buffer == null) throw new ArgumentException(Resources.InvalidArgumentNull, nameof(buffer));
            if (first < 0 || first > buffer.Length - 1) throw new ArgumentException(Resources.ArgumentOutOfRange, nameof(first));
            if (last > buffer.Length - 1) throw new ArgumentException(Resources.ArgumentOutOfRange, nameof(last));

            if (first == 0 && last == -1) return Encoding.ASCII.GetString(buffer);

            var count = buffer.Skip(first).Count();
            if (last >= first) count -= last - first;
            var a = buffer.Skip(first).Take(count).ToArray();
            return Encoding.ASCII.GetString(a);
        }

    }
}
