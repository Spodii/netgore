using System;
using System.Linq;

namespace GoreUpdater
{
    public interface IDownloadSource : IDisposable
    {
        /// <summary>
        /// Notifies listeners when this <see cref="IDownloadSource"/> has failed to download a file, such as because
        /// the file did not exist on the source.
        /// </summary>
        event DownloadSourceFileFailedEventHandler DownloadFailed;

        /// <summary>
        /// Notifies listeners when this <see cref="IDownloadSource"/> has finished downloading a file.
        /// </summary>
        event DownloadSourceFileEventHandler DownloadFinished;

        /// <summary>
        /// Gets if this <see cref="IDownloadSource"/> can start a download.
        /// </summary>
        bool CanDownload { get; }

        /// <summary>
        /// Gets if this <see cref="IDownloadSource"/> has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Starts downloading a file.
        /// </summary>
        /// <param name="remoteFile">The file to download.</param>
        /// <param name="localFilePath">The complete file path that will be used to store the downloaded file.</param>
        /// <returns>True if the download was successfully started; otherwise false.</returns>
        bool Download(string remoteFile, string localFilePath);
    }
}