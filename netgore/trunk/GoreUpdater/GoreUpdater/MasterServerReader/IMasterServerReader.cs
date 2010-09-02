using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Interface for an object that handles reading the master server(s).
    /// All implementations must be completely thread-safe.
    /// </summary>
    public interface IMasterServerReader
    {
        /// <summary>
        /// Gets the path to the local file containing the list of download sources. This file will be updated automatically
        /// as new download sources are discovered by this <see cref="IMasterServerReader"/>.
        /// </summary>
        string LocalDownloadSourceListPath { get; }

        /// <summary>
        /// Gets the path to the local file containing the list of master servers. This file will be updated automatically
        /// as new master servers are discovered by this <see cref="IMasterServerReader"/>.
        /// </summary>
        string LocalMasterServerListPath { get; }

        /// <summary>
        /// Begins reading the version from the master server(s).
        /// </summary>
        /// <param name="callback">The <see cref="MasterServerReaderReadCallback"/> to invoke with the results when complete.</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        void BeginReadVersion(MasterServerReaderReadCallback callback, object userState);

        /// <summary>
        /// Begins reading the version file list from the master server(s).
        /// </summary>
        /// <param name="callback">The <see cref="MasterServerReaderReadCallback"/> to invoke with the results when complete.</param>
        /// <param name="version">The version to get the <see cref="VersionFileList"/> for.</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        void BeginReadVersionFileList(MasterServerReaderReadCallback callback, int version, object userState);
    }
}