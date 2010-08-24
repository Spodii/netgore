using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        readonly Queue<string> _downloadQueue = new Queue<string>();
        readonly object _downloadQueueSync = new object();
        readonly List<IDownloadSource> _downloadSources = new List<IDownloadSource>();
        readonly object _downloadSourcesSync = new object();
        readonly List<string> _finishedDownloads = new List<string>();
        readonly object _finishedDownloadsSync = new object();
        readonly Queue<string> _notStartedQueue = new Queue<string>();
        readonly object _notStartedQueueSync = new object();
        readonly string _targetPath;

        readonly List<Thread> _workerThreads = new List<Thread>();

        bool _isDisposed = false;

        /// <summary>
        /// A counter for counting what <see cref="DownloadManager"/> instance number this is. Mostly just used for when
        /// naming the worker threads.
        /// </summary>
        static int _downloadManagerCount = -1;

        public DownloadManager(string targetPath)
        {
            _targetPath = targetPath;

            var id = Interlocked.Increment(ref _downloadManagerCount);

            // Spawn the worker threads
            var numWorkers = GetNumWorkerThreads();
            for (var i = 0; i < numWorkers; i++)
            {
                var workerThread = new Thread(WorkerThreadLoop)
                { IsBackground = true};

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

                // Push the work item in to the next free IDownloadSource
                var added = false;
                lock (_downloadSourcesSync)
                {
                    foreach (var ds in _downloadSources)
                    {
                        if (ds.CanDownload)
                        {
                            ds.Download(workItem, TargetPath + workItem);
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

                _downloadQueue.Enqueue(file);

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
                return _downloadSources.Remove(downloadSource);
            }
        }

        #endregion
    }
}