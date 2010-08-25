using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GoreUpdater.Core
{
    public class HttpDownloadSource : IDownloadSource
    {
        bool _isDisposed = false;

        readonly string _rootPath;
        readonly Stack<WebClient> _webClients = new Stack<WebClient>();
        readonly object _webClientsSync = new object();

        /// <summary>
        /// Gets the root path to the HTTP server.
        /// </summary>
        public string RootPath { get { return _rootPath; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDownloadSource"/> class.
        /// </summary>
        /// <param name="rootPath">The root path to the HTTP server.</param>
        public HttpDownloadSource(string rootPath)
        {
            _rootPath = rootPath;

            var numWebClients = GetNumWebClients();

            lock (_webClientsSync)
            {
                for (int i = 0; i < numWebClients; i++)
                {
                    var wc = new WebClient();
                    wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                    _webClients.Push(wc);
                }
            }
        }

        /// <summary>
        /// Handles the DownloadFileCompleted event of the <see cref="WebClient"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/> instance containing the event data.</param>
        void wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var downloadInfo = (AsyncDownloadInfo)e.UserState;

            if (e.Cancelled || e.Error != null)
            {
                if (DownloadFailed != null)
                    DownloadFailed(this, downloadInfo.RemoteFile);
            }
            else
            {
                if (DownloadFinished != null)
                    DownloadFinished(this, downloadInfo.RemoteFile, downloadInfo.LocalFilePath);
            }
        }

        /// <summary>
        /// Gets the number of <see cref="WebClient"/>s to use per <see cref="HttpDownloadSource"/>.
        /// </summary>
        /// <returns>The number of <see cref="WebClient"/>s to use per <see cref="HttpDownloadSource"/>.</returns>
        static int GetNumWebClients()
        {
            return 4;
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
        /// Notifies listeners when this <see cref="IDownloadSource"/> has finished downloading a file.
        /// </summary>
        public event DownloadSourceFileEventHandler DownloadFinished;

        /// <summary>
        /// Notifies listeners when this <see cref="IDownloadSource"/> has failed to download a file, such as because
        /// the file did not exist on the source.
        /// </summary>
        public event DownloadSourceFileFailedEventHandler DownloadFailed;

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
        /// <returns>True if the download was successfully started; otherwise false.</returns>
        public bool Download(string remoteFile, string localFilePath)
        {
            var uri = new Uri(RootPath + remoteFile);

            lock (_webClientsSync)
            {
                if (_webClients.Count == 0)
                    return false;

                var wc = _webClients.Pop();
                wc.DownloadFileAsync(uri, localFilePath, new AsyncDownloadInfo(remoteFile, localFilePath));
            }

            return true;
        }

        #endregion

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
                    wc.CancelAsync();
                    wc.Dispose();
                }
            }
        }

        class AsyncDownloadInfo
        {
            readonly string _remoteFile;
            readonly string _localFilePath;

            public AsyncDownloadInfo(string remoteFile, string localFilePath)
            {
                _remoteFile = remoteFile;
                _localFilePath = localFilePath;
            }

            public string RemoteFile { get { return _remoteFile; } }

            public string LocalFilePath { get { return _localFilePath; } }
        }
    }
}