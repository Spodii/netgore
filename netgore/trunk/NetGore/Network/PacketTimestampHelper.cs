using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Helper methods for working with placing a time stamp on unreliable packets to ensure ordering.
    /// </summary>
    public static class PacketTimeStampHelper
    {
        /// <summary>
        /// The maximum size of the timestamp. Once this value is hit, it will roll over back to zero.
        /// </summary>
        public const int MaxTimeStampSize = ushort.MaxValue;

        /// <summary>
        /// The resolution of the time stamps in milliseconds.
        /// </summary>
        public const int TimeStampResolution = 1 << 3;

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <param name="currentTime">The time to get the time stamp for.</param>
        /// <returns>The time stamp for the given time.</returns>
        public static ushort GetTimeStamp(TickCount currentTime)
        {
            unchecked
            {
                return (ushort)((currentTime >> 3) % (MaxTimeStampSize + 1));
            }
        }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <returns>The time stamp for the current time.</returns>
        public static ushort GetTimeStamp()
        {
            return GetTimeStamp(TickCount.Now);
        }

        /// <summary>
        /// Checks if a time stamp is newer than another time stamp.
        /// </summary>
        /// <param name="newestTimeStamp">The time stamp to check if newer than the <paramref name="lastTimeStamp"/>.
        /// This is generally the time stamp of the packet that was just received.</param>
        /// <param name="lastTimeStamp">The time stamp to check if older than the <paramref name="newestTimeStamp"/>.
        /// This is generally the time stamp from the last received packet.</param>
        /// <returns>True if the <paramref name="newestTimeStamp"/> is newer than the <paramref name="lastTimeStamp"/>, or
        /// if the <paramref name="newestTimeStamp"/> is equal to the <paramref name="lastTimeStamp"/>;
        /// otherwise false.</returns>
        public static bool IsTimeStampNewer(ushort newestTimeStamp, ushort lastTimeStamp)
        {
            if (newestTimeStamp == lastTimeStamp)
                return true;

            int diff;
            if (newestTimeStamp > lastTimeStamp)
                diff = newestTimeStamp - lastTimeStamp;
            else
                diff = newestTimeStamp + (MaxTimeStampSize - lastTimeStamp);

            return diff < (MaxTimeStampSize / 2);
        }
    }
}