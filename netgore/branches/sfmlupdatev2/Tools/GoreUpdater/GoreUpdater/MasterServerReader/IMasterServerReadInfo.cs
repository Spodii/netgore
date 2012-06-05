using System.Collections.Generic;
using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Interface for an object that holds the information read from the master server(s).
    /// All implementations must be completely thread-safe.
    /// </summary>
    public interface IMasterServerReadInfo
    {
        /// <summary>
        /// Gets the descriptors for the download sources available.
        /// </summary>
        IEnumerable<DownloadSourceDescriptor> DownloadSources { get; }

        /// <summary>
        /// Gets a string containing errors that occured while getting the information, or null if no errors.
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Gets the descriptors for the master servers available.
        /// </summary>
        IEnumerable<DownloadSourceDescriptor> MasterServers { get; }

        /// <summary>
        /// Gets the current version. When using <see cref="IMasterServerReader.BeginReadVersionFileList"/>, this value is
        /// equal to the version value given when calling the method.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Gets the text for the <see cref="VersionFileList"/>. This value is only set when using
        /// <see cref="IMasterServerReader.BeginReadVersionFileList"/>, and will stay null if using
        /// <see cref="IMasterServerReader.BeginReadVersion"/> or if the file failed to be read.
        /// </summary>
        string VersionFileListText { get; }
    }
}