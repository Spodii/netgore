using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a class that provides statistics for <see cref="IMessageProcessor"/>s.
    /// </summary>
    public interface IMessageProcessorStatistics
    {
        /// <summary>
        /// Gets if file output is currently enabled.
        /// </summary>
        bool IsFileOutputEnabled { get; }

        /// <summary>
        /// Turns off automatic file output for the statistics.
        /// </summary>
        void DisableFileOutput();

        /// <summary>
        /// Turns on file output for the statistics.
        /// </summary>
        /// <param name="filePath">The output file path.</param>
        /// <param name="dumpRate">The frequency, in milliseconds, of writing the output. Default value is 10 seconds.</param>
        void EnableFileOutput(string filePath, int dumpRate = 10000);

        /// <summary>
        /// Gets all of the <see cref="IMessageProcessorStats"/>.
        /// </summary>
        IEnumerable<IMessageProcessorStats> GetAllStats();

        /// <summary>
        /// Gets the <see cref="IMessageProcessorStats"/> for a <see cref="IMessageProcessor"/> identified by the ID.
        /// </summary>
        /// <param name="msgID">The ID of the <see cref="IMessageProcessor"/> to get the stats for.</param>
        /// <returns>The <see cref="IMessageProcessorStats"/> for the given <paramref name="msgID"/>, or null if no
        /// stats exist for the given <paramref name="msgID"/>.</returns>
        IMessageProcessorStats GetStats(MessageProcessorID msgID);

        /// <summary>
        /// Writes the statistics to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        void Write(IValueWriter writer);

        /// <summary>
        /// Writes the stats to a file.
        /// </summary>
        /// <param name="filePath">The file path to write to.</param>
        void Write(string filePath);
    }
}