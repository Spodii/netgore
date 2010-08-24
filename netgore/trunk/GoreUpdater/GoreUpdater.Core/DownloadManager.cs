using System.Collections.Generic;

namespace GoreUpdater.Core
{
    public class DownloadManager : IDownloadManager
    {
        readonly string _targetPath;
        readonly Queue<string> _downloadQueue = new Queue<string>();
        readonly object _downloadQueueSync = new object();
        readonly List<string> _finishedDownloads = new List<string>();
        readonly object _finishedDownloadsSync = new object();
        readonly List<IDownloadSource> _downloadSources = new List<IDownloadSource>();
        readonly object _downloadSourcesSync = new object();

        public DownloadManager(string targetPath)
        {
            _targetPath = targetPath;
        }

        #region Implementation of IDownloadManager

        /// <summary>
        /// Gets the number of items that have finished downloading in this <see cref="IDownloadManager"/>.
        /// </summary>
        public int FinishedCount
        {
            get
            {
                lock (_finishedDownloadsSync)
                {
                    return _finishedDownloads.Count;
                }
            }
        }

        /// <summary>
        /// Gets the current collection of finished downloads.
        /// </summary>
        /// <returns>The current collection of finished downloads.</returns>
        public IEnumerable<string> GetFinished()
        {
            lock (_finishedDownloadsSync)
            {
                return _finishedDownloads.ToArray();
            }
        }

        /// <summary>
        /// Clears the finished downloads information.
        /// </summary>
        public void ClearFinished()
        {
            lock (_finishedDownloadsSync)
            {
                _finishedDownloads.Clear();
            }
        }

        /// <summary>
        /// Notifies listeners when a file download has finished.
        /// </summary>
        public event FileDownloaderFileEventHandler DownloadFinished;

        /// <summary>
        /// Notifies listeners when a file could not be moved to the target file path. Often times, this happens because
        /// the file is in use and cannot be deleted.
        /// </summary>
        public event FileDownloaderFileMoveFailedEventHandler FileMoveFailed;

        /// <summary>
        /// Gets the available file download sources.
        /// </summary>
        public IEnumerable<IDownloadSource> DownloadSources
        {
            get
            {
                lock (_downloadSourcesSync)
                {
                    return _downloadSources.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets the number of items remaining in the download queue.
        /// </summary>
        public int QueueCount
        {
            get
            {
                lock (_downloadQueueSync)
                {
                    return _downloadQueue.Count;
                }
            }
        }

        /// <summary>
        /// Gets the target path that the downloaded files will be moved to.
        /// </summary>
        public string TargetPath
        {
            get { return _targetPath; }
        }

        /// <summary>
        /// Adds a <see cref="IDownloadSource"/> to this <see cref="IDownloadManager"/>.
        /// </summary>
        /// <param name="downloadSource">The <see cref="IDownloadSource"/> to add.</param>
        /// <returns>True if the <paramref name="downloadSource"/> was added; false if the <paramref name="downloadSource"/> was
        /// invalid or already in this <see cref="IDownloadManager"/>.</returns>
        public bool AddSource(IDownloadSource downloadSource)
        {
            if (downloadSource == null)
                return false;

            lock (_downloadSourcesSync)
            {
                if (_downloadSources.Contains(downloadSource))
                    return false;

                _downloadSources.Add(downloadSource);
            }

            return true;
        }

        /// <summary>
        /// Enqueues a file for download.
        /// </summary>
        /// <param name="file">The path of the file to download.</param>
        /// <returns>True if the <paramref name="file"/> was successfully added to the queue; false if the <paramref name="file"/>
        /// is invalid, is already in the download queue, or is in the finished downloads list.</returns>
        public bool Enqueue(string file)
        {
            if (string.IsNullOrEmpty(file))
                return false;

            lock (_downloadQueueSync)
            {
                lock (_finishedDownloadsSync)
                {
                    if (_downloadQueue.Contains(file))
                        return false;

                    if (_finishedDownloads.Contains(file))
                        return false;
                }

                _downloadQueue.Enqueue(file);
            }

            return true;
        }

        /// <summary>
        /// Gets the queue of remaining downloads.
        /// </summary>
        /// <returns>The queue of remaining downloads.</returns>
        public IEnumerable<string> GetQueue()
        {
            lock (_downloadQueueSync)
            {
                return _downloadQueue.ToArray();
            }
        }

        /// <summary>
        /// Removes a <see cref="IDownloadSource"/> from this <see cref="IDownloadManager"/>.
        /// </summary>
        /// <param name="downloadSource">The <see cref="IDownloadSource"/> to remove.</param>
        /// <returns>True if the <paramref name="downloadSource"/> was removed; false if the <paramref name="downloadSource"/> was
        /// invalid or not in this <see cref="IDownloadManager"/>.</returns>
        public bool RemoveSource(IDownloadSource downloadSource)
        {
            if (downloadSource == null)
                return false;

            lock (_downloadSourcesSync)
            {
                return _downloadSources.Remove(downloadSource);
            }
        }

        #endregion
    }
}