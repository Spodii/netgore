using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a class that contains the statistics for a <see cref="IMessageProcessor"/>.
    /// </summary>
    public interface IMessageProcessorStats
    {
        /// <summary>
        /// Gets the number of calls that have been made to this processor.
        /// </summary>
        uint Calls { get; }

        /// <summary>
        /// Gets the length of the longest message read by this processor.
        /// </summary>
        ushort Max { get; }

        /// <summary>
        /// Gets length of the shortest message read by this processor.
        /// </summary>
        ushort Min { get; }

        /// <summary>
        /// Gets the ID of the <see cref="IMessageProcessor"/> that these stats are for.
        /// </summary>
        MessageProcessorID ProcessorID { get; }

        /// <summary>
        /// Gets the total bits read by this processor.
        /// </summary>
        uint TotalBits { get; }

        /// <summary>
        /// Writes the <see cref="IMessageProcessorStats"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        void Write(IValueWriter writer);
    }
}