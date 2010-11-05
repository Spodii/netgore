using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// Implementation of the <see cref="IDownloadSource"/> for downloading over HTTP.
    /// </summary>
    public class HttpDownloadSource : IDownloadSource
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string _rootPath;
        readonly Stack<WebClient> _webClients = new Stack<WebClient>();
        readonly object _webClientsSync = new object();

        bool _isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDownloadSource"/> class.
        /// </summary>
        /// <param name="rootPath">The root path to the HTTP server.</param>
        public HttpDownloadSource(string rootPath)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Creating {0} (rootPath: {1})", GetType().Name, rootPath);

            _rootPath = PathHelper.ForceEndWithChar(rootPath, '/', Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Create the web clients that we will be using for the downloading
            var numWebClients = GetNumWebClients();

            lock (_webClientsSync)
            {
                for (var i = 0; i < numWebClients; i++)
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Creating worker thread #{0} for {1}", i, this);

                    var wc = CreateWebClient();
                    _webClients.Push(wc);
                }
            }
        }

        /// <summary>
        /// Gets the root path to the HTTP server.
        /// </summary>
        public string RootPath
        {
            get { return _rootPath; }
        }

        /// <summary>
        /// Creates a <see cref="WebClient"/> that will be used to perform the downloading.
        /// </summary>
        /// <returns>A <see cref="WebClient"/> that will be used to perform the downloading.</returns>
        WebClient CreateWebClient()
        {
            if (log.IsDebugEnabled)
                log.Debug("Creating WebClient instance");

            var wc = new WebClient();
            wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            return wc;
        }

        /// <summary>
        /// Gets the number of <see cref="WebClient"/>s to use per <see cref="HttpDownloadSource"/>.
        /// </summary>
        /// <returns>The number of <see cref="WebClient"/>s to use per <see cref="HttpDownloadSource"/>.</returns>
        static int GetNumWebClients()
        {
            return 4;
        }

        /// <summary>
        /// Handles disposing of this object.
        /// </summary>
        /// <param name="disposeManaged">If false, this object was garbage collected and managed objects do not need to be disposed.
        /// If true, Dispose was called on this object and managed objects need to be disposed.</param>
        protected virtual void HandleDispose(bool disposeManaged)
        {
            lock (_webClientsSync)
            {
                foreach (var wc in _webClients)
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Disposing WebClient instance: {0}", wc);

                    wc.CancelAsync();
                    wc.Dispose();
                }
            }
        }

        /// <summary>
        /// Handles the DownloadFileCompleted event of the <see cref="WebClient"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/> instance containing the event data.</param>
        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Debug.Assert(sender is WebClient);

            var downloadInfo = (AsyncDownloadInfo)e.UserState;

            if (log.IsDebugEnabled)
                log.DebugFormat("DownloadFileComplete received from `{0}`.", sender);

            // Raise the appropriate event
            if (e.Cancelled || e.Error != null)
            {
                if (log.IsInfoEnabled)
                {
                    log.InfoFormat("Download of file `{0}` to `{1}` using {2} failed (cancelled? {3}). Error: {4}",
                                   downloadInfo.RemoteFile, downloadInfo.LocalFilePath, sender, e.Cancelled,
                                   e.Error != null ? e.Error.ToString() : "[NULL]");
                }

                try
                {
                    if (DownloadFailed != null)
                        DownloadFailed(this, downloadInfo.RemoteFile, downloadInfo.LocalFilePath);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
            else
            {
                if (log.IsInfoEnabled)
                {
                    log.InfoFormat("Download of file `{0}` to `{1}` using {2} successful.", downloadInfo.RemoteFile,
                                   downloadInfo.LocalFilePath, sender);
                }

                try
                {
                    if (DownloadFinished != null)
                        DownloadFinished(this, downloadInfo.RemoteFile, downloadInfo.LocalFilePath);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }

            // Add the WebClient back to the pool
            lock (_webClientsSync)
            {
                _webClients.Push((WebClient)sender);
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="HttpDownloadSource"/> is reclaimed by garbage collection.
        /// </summary>
        ~HttpDownloadSource()
        {
            HandleDispose(true);
            _isDisposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            HandleDispose(false);
            _isDisposed = true;
        }

        #endregion

        #region Implementation of IDownloadSource

        /// <summary>
        /// Notifies listeners when this <see cref="IDownloadSource"/> has failed to download a file, such as because
        /// the file did not exist on the source.
        /// </summary>
        public event DownloadSourceFileFailedEventHandler DownloadFailed;

        /// <summary>
        /// Notifies listeners when this <see cref="IDownloadSource"/> has finished downloading a file.
        /// </summary>
        public event DownloadSourceFileEventHandler DownloadFinished;

        /// <summary>
        /// Gets if this <see cref="IDownloadSource"/> can start a download.
        /// </summary>
        public bool CanDownload
        {
            get
            {
                lock (_webClientsSync)
                {
                    if (_webClients.Count > 0)
                        return true;
                    else
                        return false;
                }
            }
        }

        /// <summary>
        /// Gets if this <see cref="IDownloadSource"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Starts downloading a file.
        /// </summary>
        /// <param name="remoteFile">The file to download.</param>
        /// <param name="localFilePath">The complete file path that will be used to store the downloaded file.</param>
        /// <param name="version">The file version to download.</param>
        /// <returns>True if the download was successfully started; otherwise false.</returns>
        public bool Download(string remoteFile, string localFilePath, int? version)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Attempting Download on `{0}`. RemoteFile: {1}. LocalFilePath: {2}. Version: {3}", this,
                                remoteFile, localFilePath, version.HasValue ? version.Value.ToString() : "[NULL]");
            }

            var uriPath = RootPath;

            // Add the version directory if needed
            if (version.HasValue)
                uriPath = PathHelper.CombineDifferentPaths(uriPath, PathHelper.GetVersionString(version.Value));

            // Add the file to get to the path
            uriPath = PathHelper.CombineDifferentPaths(uriPath, remoteFile);

            var uri = new Uri(uriPath);

            // Ensure the directory exists
            var dir = Path.GetDirectoryName(localFilePath);
            if (dir != null)
            {
                if (!Directory.Exists(dir))
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Creating directory: {0}", dir);
                    Directory.CreateDirectory(dir);
                }
            }

            // Get the WebClient to use
            lock (_webClientsSync)
            {
                // Check for a free WebClient
                if (_webClients.Count == 0)
                {
                    if (log.IsInfoEnabled)
                    {
                        log.InfoFormat(
                            "Could not start download on `{0}` since no WebClients are available. RemoteFile: {1}. LocalFilePath: {2}. Version: {3}",
                            this, remoteFile, localFilePath, version.HasValue ? version.Value.ToString() : "[NULL]");
                    }
                    return false;
                }

                // Grab the free WebClient and start the download
                var wc = _webClients.Pop();
                wc.DownloadFileAsync(uri, localFilePath, new AsyncDownloadInfo(remoteFile, localFilePath));

                if (log.IsInfoEnabled)
                {
                    log.InfoFormat(
                        "Starting download on `{0}` using WebClient {4}. RemoteFile: {1}. LocalFilePath: {2}. Version: {3}", this,
                        remoteFile, localFilePath, version.HasValue ? version.Value.ToString() : "[NULL]", wc);
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if this <see cref="IDownloadSource"/> contains the same values as the given <see cref="DownloadSourceDescriptor"/>.
        /// </summary>
        /// <param name="descriptor">The <see cref="DownloadSourceDescriptor"/> to compare to.</param>
        /// <returns>True if they have equal values; otherwise false.</returns>
        public bool IsIdenticalTo(DownloadSourceDescriptor descriptor)
        {
            if (descriptor.Type != DownloadSourceType.Http)
                return false;

            if (descriptor.RootPath != RootPath)
                return false;

            return true;
        }

        #endregion

        /// <summary>
        /// Contains the information for an asynchronous download.
        /// </summary>
        class AsyncDownloadInfo
        {
            readonly string _localFilePath;
            readonly string _remoteFile;

            /// <summary>
            /// Initializes a new instance of the <see cref="AsyncDownloadInfo"/> class.
            /// </summary>
            /// <param name="remoteFile">The remote file.</param>
            /// <param name="localFilePath">The local file path.</param>
            public AsyncDownloadInfo(string remoteFile, string localFilePath)
            {
                _remoteFile = remoteFile;
                _localFilePath = localFilePath;
            }

            /// <summary>
            /// Gets the local file path for the asynchronous download.
            /// </summary>
            public string LocalFilePath
            {
                get { return _localFilePath; }
            }

            /// <summary>
            /// Gets the remote file path for the asynchronous download.
            /// </summary>
            public string RemoteFile
            {
                get { return _remoteFile; }
            }
        }
    }
}