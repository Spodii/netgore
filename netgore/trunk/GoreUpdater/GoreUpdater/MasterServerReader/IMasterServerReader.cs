using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Interface for an object that handles reading the master server(s).
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
        /// Begins reading the master server information.
        /// </summary>
        /// <param name="callback">The <see cref="MasterServerReaderReadCallback"/> to invoke with the results when complete.</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        void BeginRead(MasterServerReaderReadCallback callback, object userState);
    }
}