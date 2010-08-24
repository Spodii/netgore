using System;
using System.Linq;

namespace GoreUpdater.Core
{
    public class HttpDownloadSource : IDownloadSource
    {
        bool _isDisposed = false;

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            HandleDispose(false);
            _isDisposed = true;
        }
        

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="HttpDownloadSource"/> is reclaimed by garbage collection.
        /// </summary>
        ~HttpDownloadSource()
        {
            HandleDispose(true);
            _isDisposed = true;
        }

        #endregion

        #region Implementation of IDownloadSource

        public event DownloadSourceFileEventHandler DownloadFinished;

        /// <summary>
        /// Gets if this <see cref="IDownloadSource"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets if this <see cref="IDownloadSource"/> can start a download.
        /// </summary>
        public bool CanDownload
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Starts downloading a file.
        /// </summary>
        /// <param name="remoteFile">The file to download.</param>
        /// <param name="localFilePath">The complete file path that will be used to store the downloaded file.</param>
        /// <returns>True if the download was successfully started; otherwise false.</returns>
        public bool Download(string remoteFile, string localFilePath)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Handles disposing of this object.
        /// </summary>
        /// <param name="disposeManaged">If false, this object was garbage collected and managed objects do not need to be disposed.
        /// If true, Dispose was called on this object and managed objects need to be disposed.</param>
        protected virtual void HandleDispose(bool disposeManaged)
        {
        }
    }

    public interface IDownloadSource : IDisposable
    {
        /// <summary>
        /// Notifies listeners when this <see cref="IDownloadSource"/> has finished downloading a file.
        /// </summary>
        event DownloadSourceFileEventHandler DownloadFinished;

        /// <summary>
        /// Gets if this <see cref="IDownloadSource"/> has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets if this <see cref="IDownloadSource"/> can start a download.
        /// </summary>
        bool CanDownload { get; }

        /// <summary>
        /// Starts downloading a file.
        /// </summary>
        /// <param name="remoteFile">The file to download.</param>
        /// <param name="localFilePath">The complete file path that will be used to store the downloaded file.</param>
        /// <returns>True if the download was successfully started; otherwise false.</returns>
        bool Download(string remoteFile, string localFilePath);
    }
}