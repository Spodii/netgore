using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Holds the information read from the master server(s) and provides a way of combining the information from multiple servers.
    /// This class is fully thread-safe.
    /// </summary>
    public class MasterServerReadInfo : IMasterServerReadInfo
    {
        readonly object _errorSync = new object();
        readonly List<DownloadSourceDescriptor> _masters = new List<DownloadSourceDescriptor>();
        readonly object _mastersSync = new object();
        readonly List<DownloadSourceDescriptor> _sources = new List<DownloadSourceDescriptor>();
        readonly object _sourcesSync = new object();
        readonly object _versionSync = new object();

        string _error;
        int _version = 0;

        /// <summary>
        /// Adds a download source read from a master server.
        /// </summary>
        /// <param name="source">The <see cref="DownloadSourceDescriptor"/>.</param>
        public void AddDownloadSource(DownloadSourceDescriptor source)
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
        /// Adds a master server.
        /// </summary>
        /// <param name="master">The <see cref="DownloadSourceDescriptor"/>.</param>
        public void AddMasterServer(DownloadSourceDescriptor master)
        {
            if (master == null)
            {
                Debug.Fail("master is null.");
                return;
            }

            lock (_mastersSync)
            {
                if (!_masters.Any(master.IsIdenticalTo))
                {
                    _masters.Add(master);
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
            lock (_versionSync)
            {
                if (version > _version)
                    _version = version;
            }
        }

        /// <summary>
        /// Appends a new error string.
        /// </summary>
        /// <param name="error">The new error string.</param>
        public void AppendError(string error)
        {
            // Append the error
            lock (_errorSync)
            {
                if (_error == null)
                    _error = error;
                else
                {
                    _error += Environment.NewLine + Environment.NewLine + "---------------------" + Environment.NewLine +
                              Environment.NewLine;
                    _error += error;
                }
            }
        }

        #region Implementation of IMasterServerReadInfo

        /// <summary>
        /// Gets the descriptors for the download sources available.
        /// </summary>
        public IEnumerable<DownloadSourceDescriptor> DownloadSources
        {
            get
            {
                lock (_sourcesSync)
                {
                    return _sources.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets a string containing errors that occured while getting the information, or null if no errors.
        /// </summary>
        public string Error
        {
            get
            {
                lock (_errorSync)
                {
                    return _error;
                }
            }
        }

        /// <summary>
        /// Gets the descriptors for the master servers available.
        /// </summary>
        public IEnumerable<DownloadSourceDescriptor> MasterServers
        {
            get
            {
                lock (_mastersSync)
                {
                    return _masters.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets the current version.
        /// </summary>
        public int Version
        {
            get
            {
                lock (_versionSync)
                {
                    return _version;
                }
            }
        }

        #endregion
    }
}