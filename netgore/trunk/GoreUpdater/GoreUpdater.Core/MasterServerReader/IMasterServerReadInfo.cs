using System.Collections.Generic;

namespace GoreUpdater
{
    /// <summary>
    /// Interface for an object that holds the information read from the master server(s).
    /// </summary>
    public interface IMasterServerReadInfo
    {
        /// <summary>
        /// Gets the current version.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Gets the descriptors for the master servers available.
        /// </summary>
        IEnumerable<DownloadSourceDescriptor> MasterServers { get; }

        /// <summary>
        /// Gets the descriptors for the download sources available.
        /// </summary>
        IEnumerable<DownloadSourceDescriptor> DownloadSources { get; }

        /// <summary>
        /// Gets a string containing errors that occured while getting the information, or null if no errors.
        /// </summary>
        string Error { get; }
    }
}