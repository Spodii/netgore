using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// Holds the information read from the master server(s) and provides a way of combining the information from multiple servers.
    /// This class is fully thread-safe.
    /// </summary>
    public class MasterServerReadInfo : IMasterServerReadInfo
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly object _errorSync = new object();
        readonly List<DownloadSourceDescriptor> _masters = new List<DownloadSourceDescriptor>();
        readonly object _mastersSync = new object();
        readonly List<DownloadSourceDescriptor> _sources = new List<DownloadSourceDescriptor>();
        readonly object _sourcesSync = new object();
        readonly object _versionFileListTextSync = new object();
        readonly object _versionSync = new object();

        bool _currVersionFileListTextLegal;
        string _error;
        int _version = 0;
        string _versionFileListText;

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

            if (log.IsDebugEnabled)
                log.DebugFormat("Adding download source `{0}` to MasterServerReadInfo `{1}`.", source, this);

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

            if (log.IsDebugEnabled)
                log.DebugFormat("Adding master server `{0}` to MasterServerReadInfo `{1}`.", master, this);

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
            if (log.IsDebugEnabled)
                log.DebugFormat("Adding version `{0}` to MasterServerReadInfo `{1}`.", version, this);

            // Use the greatest version
            lock (_versionSync)
            {
                if (version > _version)
                    _version = version;
            }
        }

        /// <summary>
        /// Adds the text read for the <see cref="VersionFileList"/>. This method will use a "best guess" approach
        /// to determine what text to use when multiple different texts are added.
        /// </summary>
        /// <param name="text">The text for the <see cref="VersionFileList"/>.</param>
        public void AddVersionFileListText(string text)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Adding VersionFileList text `{0}` to MasterServerReadInfo `{1}`.", text, this);

            lock (_versionFileListTextSync)
            {
                // If the text is exactly the same as the current _versionFileListText, then just ignore it since there is obviously
                // nothing to be done
                if (StringComparer.Ordinal.Equals(text, _versionFileListText))
                    return;

                // Try to parse the text as a VersionFileList to see if it is valid
                bool textIsValid;
                try
                {
                    VersionFileList.CreateFromString(text);
                    textIsValid = true;
                }
                catch (InvalidDataException)
                {
                    // Ignore InvalidDataException since its expected when the version is invalid
                    textIsValid = false;
                }
                catch (Exception ex)
                {
                    // For any other exception, it still must be invalid, but check it out when debugging
                    Debug.Fail(ex.ToString());
                    textIsValid = false;
                }

                // If no value set, just use the text no matter what
                if (string.IsNullOrEmpty(_versionFileListText))
                {
                    _versionFileListText = text;
                    _currVersionFileListTextLegal = textIsValid;
                    return;
                }

                // The _versionFileListText already exists. If the current text is invalid but the new text is valid, then
                // just use the new text.
                if (!_currVersionFileListTextLegal && textIsValid)
                {
                    _versionFileListText = text;
                    _currVersionFileListTextLegal = textIsValid;
                    return;
                }

                // Likewise, if the current text is valid but the new text is invalid, do not use the new text
                if (_currVersionFileListTextLegal && !textIsValid)
                    return;

                // From here, they are either both valid or both invalid. To keep things simple, just assume that the larger
                // of the two strings are "better". So change to the new text if it is longer.
                if (text.Length > _versionFileListText.Length)
                {
                    _versionFileListText = text;
                    _currVersionFileListTextLegal = textIsValid;
                    return;
                }
            }
        }

        /// <summary>
        /// Appends a new error string.
        /// </summary>
        /// <param name="error">The new error string.</param>
        public void AppendError(string error)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Adding error `{0}` to MasterServerReadInfo `{1}`.", error, this);

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
        /// Gets the current version. When using <see cref="IMasterServerReader.BeginReadVersionFileList"/>, this value is
        /// equal to the version value given when calling the method.
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

        /// <summary>
        /// Gets the text for the <see cref="VersionFileList"/>. This value is only set when using
        /// <see cref="IMasterServerReader.BeginReadVersionFileList"/>, and will stay null if using
        /// <see cref="IMasterServerReader.BeginReadVersion"/> or if the file failed to be read.
        /// </summary>
        public string VersionFileListText
        {
            get
            {
                lock (_versionFileListTextSync)
                {
                    return _versionFileListText;
                }
            }
        }

        #endregion
    }
}