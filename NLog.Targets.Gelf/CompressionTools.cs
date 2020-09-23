using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace NLog.Targets.Gelf
{
    public static class CompressionTools
    {
        /// <summary>
        /// Compresses the given message using GZip algorithm
        /// </summary>
        /// <param name="message">Message to be compressed</param>
        /// <returns>Compressed message in bytes</returns>
        public static byte[] CompressMessage(String message)
        {
            var compressedMessageStream = new MemoryStream();
            using (var gzipStream = new GZipStream(compressedMessageStream, CompressionMode.Compress))
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                gzipStream.Write(messageBytes, 0, messageBytes.Length);
            }
            return compressedMessageStream.ToArray();
        }
    }
}
