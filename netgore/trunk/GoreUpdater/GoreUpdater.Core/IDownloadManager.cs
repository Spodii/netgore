using System;
using System.Collections.Generic;
using System.Linq;

namespace GoreUpdater.Core
{
    public interface IDownloadManager : IDisposable
    {
        /// <summary>
        /// Notifies listeners when a file completely failed to be downloaded after <see cref="IDownloadManager.MaxAttempts"/>
        /// attempts.
        /// </summary>
        event DownloadManagerDownloadFailedEventHandler DownloadFailed;

        /// <summary>
        /// Notifies listeners when a file download has finished.
        /// </summary>
        event DownloadManagerFileEventHandler DownloadFinished;

        /// <summary>
        /// Notifies listeners when a file could not be moved to the target file path. Often times, this happens because
        /// the file is in use and cannot be deleted.
        /// </summary>
        event DownloadManagerFileMoveFailedEventHandler FileMoveFailed;

        /// <summary>
        /// Gets the available file download sources.
        /// </summary>
        IEnumerable<IDownloadSource> DownloadSources { get; }

        /// <summary>
        /// Gets the number of items in the list of files that failed to be downloaded.
        /// </summary>
        int FailedDownloadsCount { get; }

        /// <summary>
        /// Gets the number of items that have finished downloading in this <see cref="IDownloadManager"/>.
        /// </summary>
        int FinishedCount { get; }

        /// <summary>
        /// Gets if this <see cref="IDownloadManager"/> has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets the maximum times a single file will attempt to download. If a download failes more than this many times, it will
        /// be aborted and the <see cref="IDownloadManager.DownloadFailed"/> event will be raised.
        /// </summary>
        int MaxAttempts { get; }

        /// <summary>
        /// Gets the number of items remaining in the download queue.
        /// </summary>
        int QueueCount { get; }

        /// <summary>
        /// Gets the target path that the downloaded files will be moved to.
        /// </summary>
        string TargetPath { get; }

        /// <summary>
        /// Adds a <see cref="IDownloadSource"/> to this <see cref="IDownloadManager"/>.
        /// </summary>
        /// <param name="downloadSource">The <see cref="IDownloadSource"/> to add.</param>
        /// <returns>True if the <paramref name="downloadSource"/> was added; false if the <paramref name="downloadSource"/> was
        /// invalid or already in this <see cref="IDownloadManager"/>.</returns>
        bool AddSource(IDownloadSource downloadSource);

        /// <summary>
        /// Clears the failed downloads information.
        /// </summary>
        void ClearFailed();

        /// <summary>
        /// Clears the finished downloads information.
        /// </summary>
        void ClearFinished();

        /// <summary>
        /// Enqueues a file for download.
        /// </summary>
        /// <param name="file">The path of the file to download.</param>
        /// <returns>True if the <paramref name="file"/> was successfully added to the queue; false if the <paramref name="file"/>
        /// is invalid, is already in the download queue, or is in the finished downloads list.</returns>
        bool Enqueue(string file);

        /// <summary>
        /// Enqueues multiple files for download.
        /// </summary>
        /// <param name="files">The files to enqueue for download.</param>
        void Enqueue(IEnumerable<string> files);

        /// <summary>
        /// Gets the files that failed to be downloaded. These files will not be re-attempted automatically since they had already
        /// passed the <see cref="MaxAttempts"/> limit.
        /// </summary>
        /// <returns>The files that failed to be downloaded</returns>
        IEnumerable<string> GetFailedDownloads();

        /// <summary>
        /// Gets the current collection of finished downloads.
        /// </summary>
        /// <returns>The current collection of finished downloads.</returns>
        IEnumerable<string> GetFinished();

        /// <summary>
        /// Gets the queue of remaining downloads.
        /// </summary>
        /// <returns>The queue of remaining downloads.</returns>
        IEnumerable<string> GetQueue();

        /// <summary>
        /// Removes a <see cref="IDownloadSource"/> from this <see cref="IDownloadManager"/>.
        /// </summary>
        /// <param name="downloadSource">The <see cref="IDownloadSource"/> to remove.</param>
        /// <returns>True if the <paramref name="downloadSource"/> was removed; false if the <paramref name="downloadSource"/> was
        /// invalid or not in this <see cref="IDownloadManager"/>.</returns>
        bool RemoveSource(IDownloadSource downloadSource);
    }
}