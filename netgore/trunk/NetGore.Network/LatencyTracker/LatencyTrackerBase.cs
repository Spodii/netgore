using System.Diagnostics;
using System.Linq;
using NetGore;

namespace NetGore.Network
{
    /// <summary>
    /// Helper functions for the LatencyTrackerServer and LatencyTrackerClient.
    /// </summary>
    static class LatencyTrackerHelper
    {
        /// <summary>
        /// Size of the ping signature in bytes.
        /// </summary>
        public const int SignatureSize = 4;

        /// <summary>
        /// Reads the ping signature from a buffer.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="offset">Offset in the buffer to start reading the signature from.</param>
        /// <returns>The ping signature from a buffer.</returns>
        public static int ReadSignature(byte[] buffer, int offset)
        {
            Debug.Assert(buffer.Length >= SignatureSize);

            int signature = buffer[offset] << 24;
            signature |= buffer[offset + 1] << 16;
            signature |= buffer[offset + 2] << 8;
            signature |= buffer[offset + 3];

            return signature;
        }

        /// <summary>
        /// Writes a ping signature to a buffer.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="signature">The ping signature to write.</param>
        /// <param name="offset">Offset in the buffer to start writing the signature to.</param>
        public static void WriteSignature(byte[] buffer, int signature, int offset)
        {
            Debug.Assert(buffer.Length >= SignatureSize);

            buffer[offset] = (byte)((signature >> 24) & 255);
            buffer[offset + 1] = (byte)((signature >> 16) & 255);
            buffer[offset + 2] = (byte)((signature >> 8) & 255);
            buffer[offset + 3] = (byte)(signature & 255);
        }
    }
}