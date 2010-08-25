using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GoreUpdater
{
    public class MasterServerReader
    {
    }

    /// <summary>
    /// Interface for an object that handles reading the master server(s).
    /// </summary>
    public interface IMasterServerReader
    {

    }

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
        /// Gets the descriptors for the download sources available.
        /// </summary>
        IEnumerable<DownloadSourceDescriptor> Sources { get; }
    }

    /// <summary>
    /// Holds the information read from the master server(s) and provides a way of combining the information from multiple servers.
    /// </summary>
    public class MasterServerReadInfo : IMasterServerReadInfo
    {
        readonly List<DownloadSourceDescriptor> _sources = new List<DownloadSourceDescriptor>();
        readonly object _sourcesSync = new object();

        int _version = 0;

        /// <summary>
        /// Adds a download source read from a master server.
        /// </summary>
        /// <param name="source">The <see cref="DownloadSourceDescriptor"/>.</param>
        public void AddSource(DownloadSourceDescriptor source)
        {
            if (source == null)
            {
                Debug.Fail("source is null.");
                return;
            }

            lock (_sourcesSync)
            {
                if (!_sources.Any(source.IsIdenticalTo))
                {
                    _sources.Add(source);
                    return;
                }
            }
        }

        /// <summary>
        /// Adds the version information read from a master server.
        /// </summary>
        /// <param name="version">The verison.</param>
        public void AddVersion(int version)
        {
            // Use the greatest version
            if (version > _version)
                _version = version; 
        }

        #region Implementation of IMasterServerReadInfo

        /// <summary>
        /// Gets the current version.
        /// </summary>
        public int Version
        {
            get { return _version; }
        }

        /// <summary>
        /// Gets the descriptors for the download sources available.
        /// </summary>
        public IEnumerable<DownloadSourceDescriptor> Sources
        {
            get
            {
                lock (_sources)
                {
                    return _sources.ToArray();
                }
            }
        }

        #endregion
    }
}
