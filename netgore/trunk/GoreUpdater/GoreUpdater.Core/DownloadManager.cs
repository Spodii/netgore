using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace GoreUpdater.Core
{
    public class DownloadManager : IDownloadManager
    {
        /// <summary>
        /// For how long to sleep the worker thread when there are no jobs available.
        /// </summary>
        const int _workerThreadNoJobsTimeout = 500;

        /// <summary>
        /// For how long to sleep when all of the <see cref="IDownloadSource"/>s are busy.
        /// </summary>
        const int _workerThreadSourcesBusyTimeout = 500;

        /// <summary>
        /// A counter for counting what <see cref="DownloadManager"/> instance number this is. Mostly just used for when
        /// naming the worker threads.
        /// </summary>
        static int _downloadManagerCount = -1;

        readonly List<string> _downloadQueue = new List<string>();
        readonly object _downloadQueueSync = new object();
        readonly List<IDownloadSource> _downloadSources = new List<IDownloadSource>();
        readonly object _downloadSourcesSync = new object();
        readonly List<string> _finishedDownloads = new List<string>();
        readonly object _finishedDownloadsSync = new object();
        readonly Queue<string> _notStartedQueue = new Queue<string>();
        readonly object _notStartedQueueSync = new object();
        readonly string _targetPath;

        readonly string _tempPath;
        readonly List<Thread> _workerThreads = new List<Thread>();

        bool _isDisposed = false;

        public DownloadManager(string targetPath, string tempPath)
        {
            _targetPath = targetPath;
            _tempPath = tempPath;

            var id = Interlocked.Increment(ref _downloadManagerCount);

            // Spawn the worker threads
            var numWorkers = GetNumWorkerThreads();
            for (var i = 0; i < numWorkers; i++)
            {
                var workerThread = new Thread(WorkerThreadLoop) { IsBackground = true };

                try
                {
                    workerThread.Name = string.Format("DownloadManager [{0}] worker thread [{1}", id, i);
                }
                catch (InvalidOperationException ex)
                {
                    Debug.Fail(ex.ToString());
                }

                workerThread.Start();
                _workerThreads.Add(workerThread);
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DownloadManager"/> is reclaimed by garbage collection.
        /// </summary>
        ~DownloadManager()
        {
            HandleDispose(true);
            _isDisposed = true;
        }

        /// <summary>
        /// Gets the number of threads to use for the <see cref="WorkerThreadLoop"/>.
        /// </summary>
        /// <returns>The number of threads to use for the <see cref="WorkerThreadLoop"/>.</returns>
        static int GetNumWorkerThreads()
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
        }

        void WorkerThreadLoop()
        {
            while (!IsDisposed)
            {
                string workItem = null;

                // Grab the next job from the queue
                lock (_notStartedQueueSync)
                {
                    if (_notStartedQueue.Count > 0)
                        workItem = _notStartedQueue.Dequeue();
                }

                // If we grabbed nothing, then idle for a while and try again
                if (workItem == null)
                {
                    Thread.Sleep(_workerThreadNoJobsTimeout);
                    continue;
                }

                var downloadTo = GetTempPath(workItem);

                // Push the work item in to the next free IDownloadSource
                var added = false;
                lock (_downloadSourcesSync)
                {
                    foreach (var ds in _downloadSources)
                    {
                        if (ds.Download(workItem, downloadTo))
                        {
                            added = true;
                            break;
                        }
                    }
                }

                // If it couldn't be added into any of the download sources, then add it back into the queue and sleep a while
                if (!added)
                {
                    lock (_notStartedQueueSync)
                    {
                        _notStartedQueue.Enqueue(workItem);
                    }

                    Thread.Sleep(_workerThreadSourcesBusyTimeout);
                    continue;
                }
            }
        }

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

        #endregion

        #region Implementation of IDownloadManager

        /// <summary>
        /// Gets the temporary download path.
        /// </summary>
        public string TempPath
        {
            get { return _tempPath; }
        }

        /// <summary>
        /// Gets the target path for a remote file.
        /// </summary>
        /// <param name="remoteFile">The remote file being downloaded.</param>
        /// <returns>The target path for the <paramref name="remoteFile"/>.</returns>
        protected string GetTargetPath(string remoteFile)
        {
            return Path.Combine(TargetPath, remoteFile);
        }

        /// <summary>
        /// Gets the temporary path to use to download a remote file. The file is downloaded to here completely first before
        /// moving it to the real target path.
        /// </summary>
        /// <param name="remoteFile">The remote file being downloaded.</param>
        /// <returns>The temporary path for the <paramref name="remoteFile"/>.</returns>
        protected string GetTempPath(string remoteFile)
        {
            return Path.Combine(TempPath, remoteFile);
        }

        void downloadSource_DownloadFailed(IDownloadSource sender, string remoteFile)
        {
            // TODO: Handle failed downloads
        }

        void downloadSource_DownloadFinished(IDownloadSource sender, string remoteFile)
        {
            // Remove from the download queue
            lock (_downloadQueueSync)
            {
                var removed = _downloadQueue.Remove(remoteFile);
                Debug.Assert(removed);
            }

            // Add to the finished queue
            lock (_finishedDownloadsSync)
            {
                _finishedDownloads.Add(remoteFile);
            }

            var tempPath = GetTempPath(remoteFile);
            var targetPath = GetTargetPath(remoteFile);

            // Try to move the file
            try
            {
                File.Move(tempPath, targetPath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                if (FileMoveFailed != null)
                    FileMoveFailed(this, remoteFile, tempPath, targetPath);
                return;
            }

            // Notify that the file successfully downloaded and was moved
            if (DownloadFinished != null)
                DownloadFinished(this, remoteFile, targetPath);
        }

        /// <summary>
        /// Notifies listeners when a file download has finished.
        /// </summary>
        public event DownloadManagerFileEventHandler DownloadFinished;

        /// <summary>
        /// Notifies listeners when a file could not be moved to the target file path. Often times, this happens because
        /// the file is in use and cannot be deleted.
        /// </summary>
        public event DownloadManagerFileMoveFailedEventHandler FileMoveFailed;

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
        /// Gets if this <see cref="IDownloadManager"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
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

                downloadSource.DownloadFinished += downloadSource_DownloadFinished;
                downloadSource.DownloadFailed += downloadSource_DownloadFailed;

                _downloadSources.Add(downloadSource);
            }

            return true;
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

                _downloadQueue.Add(file);

                lock (_notStartedQueueSync)
                {
                    _notStartedQueue.Enqueue(file);
                }
            }

            return true;
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
                if (!_downloadSources.Remove(downloadSource))
                    return false;
            }

            downloadSource.DownloadFinished -= downloadSource_DownloadFinished;
            downloadSource.DownloadFailed -= downloadSource_DownloadFailed;

            return true;
        }

        #endregion
    }
}