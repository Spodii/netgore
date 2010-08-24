using System.Collections.Generic;

namespace GoreUpdater.Core
{
    public interface IDownloadManager
    {
        /// <summary>
        /// Notifies listeners when a file download has finished.
        /// </summary>
        event FileDownloaderFileEventHandler DownloadFinished;

        /// <summary>
        /// Notifies listeners when a file could not be moved to the target file path. Often times, this happens because
        /// the file is in use and cannot be deleted.
        /// </summary>
        event FileDownloaderFileMoveFailedEventHandler FileMoveFailed;

        /// <summary>
        /// Gets the available file download sources.
        /// </summary>
        IEnumerable<IDownloadSource> DownloadSources { get; }

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
        /// Enqueues a file for download.
        /// </summary>
        /// <param name="file">The path of the file to download.</param>
        void Enqueue(string file);

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