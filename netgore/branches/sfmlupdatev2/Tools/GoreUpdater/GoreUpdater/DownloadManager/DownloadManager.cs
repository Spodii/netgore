using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// A basic implementation of the <see cref="IDownloadManager"/>.
    /// </summary>
    public class DownloadManager : IDownloadManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// For how long to sleep the worker thread when there are no jobs available.
        /// </summary>
        const int _workerThreadNoJobsTimeout = 500;

        /// <summary>
        /// For how long to sleep when all of the <see cref="IDownloadSource"/>s are busy.
        /// </summary>
        const int _workerThreadSourcesBusyTimeout = 500;

        /// <summary>
        /// A synchronization object for performing operations on the file system (creating directories, moving files, etc).
        /// </summary>
        static readonly object _fileSystemSync = new object();

        /// <summary>
        /// A counter for counting what <see cref="DownloadManager"/> instance number this is. Mostly just used for when
        /// naming the worker threads.
        /// </summary>
        static int _downloadManagerCount = -1;

        readonly byte _attemptsPerSource;

        readonly Dictionary<string, List<IDownloadSource>> _downloadFailedDict =
            new Dictionary<string, List<IDownloadSource>>(StringComparer.Ordinal);

        readonly Dictionary<string, int> _downloadFailedDictCount = new Dictionary<string, int>(StringComparer.Ordinal);
        readonly object _downloadFailedDictSync = new object();

        readonly List<string> _downloadQueue = new List<string>();
        readonly object _downloadQueueSync = new object();

        readonly List<IDownloadSource> _downloadSources = new List<IDownloadSource>();
        readonly object _downloadSourcesSync = new object();

        readonly int? _downloadVersion;

        readonly List<string> _failedDownloads = new List<string>();
        readonly object _failedDownloadsSync = new object();

        readonly List<string> _finishedDownloads = new List<string>();
        readonly object _finishedDownloadsSync = new object();

        readonly Queue<string> _notStartedQueue = new Queue<string>();
        readonly object _notStartedQueueSync = new object();

        readonly string _targetPath;
        readonly string _tempPath;
        readonly List<Thread> _workerThreads = new List<Thread>();

        bool _isDisposed = false;
        int _maxAttempts = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadManager"/> class.
        /// </summary>
        /// <param name="targetPath">The path that downloaded files will ultimately end up in.</param>
        /// <param name="tempPath">The path to store temporary download files until they finish.</param>
        /// <param name="downloadVersion">The version of the file to download.</param>
        /// <param name="attemptsPerSource">The number of times we will attempt each source before giving up completely on
        /// a file. While this value is quite high (3) by default, this is due to the fact that failing to download a file
        /// completely can be hard to recover from. If less than 1, this value will be set to 1.</param>
        public DownloadManager(string targetPath, string tempPath, int? downloadVersion, byte attemptsPerSource = (byte)3)
        {
            if (attemptsPerSource < 1)
                attemptsPerSource = 1;

            _downloadVersion = downloadVersion;
            _targetPath = targetPath;
            _tempPath = tempPath;

            _attemptsPerSource = attemptsPerSource;

            var id = Interlocked.Increment(ref _downloadManagerCount);

            // Spawn the worker threads
            var numWorkers = GetNumWorkerThreads();
            for (var i = 0; i < numWorkers; i++)
            {
                var workerThread = new Thread(WorkerThreadLoop) { IsBackground = true };

                if (log.IsInfoEnabled)
                    log.InfoFormat("Creating worker thread #{0}", i);

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

                if (log.IsInfoEnabled)
                    log.InfoFormat("Starting DownloadManager job: {0}", workItem);

                var downloadTo = GetTempPath(workItem);

                // Ensure the target directory exists
                lock (_fileSystemSync)
                {
                    var dir = Path.GetDirectoryName(downloadTo);
                    if (dir != null)
                    {
                        if (!Directory.Exists(dir))
                        {
                            if (log.IsDebugEnabled)
                                log.DebugFormat("Creating directory: {0}", dir);

                            Directory.CreateDirectory(dir);
                        }
                    }
                }

                // Push the work item in to the next free IDownloadSource
                var added = false;
                lock (_downloadSourcesSync)
                {
                    foreach (var ds in _downloadSources)
                    {
                        if (ds.Download(workItem, downloadTo, DownloadVersion))
                        {
                            if (log.IsDebugEnabled)
                                log.DebugFormat("Job `{0}` added to DownloadSource `{1}`.", workItem, ds);

                            added = true;
                            break;
                        }
                    }
                }

                // If it couldn't be added into any of the download sources, then add it back into the queue and sleep a while
                if (!added)
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Job `{0}` failed to be added to any DownloadSources.", workItem);

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
            return PathHelper.CombineDifferentPaths(TargetPath, remoteFile);
        }

        /// <summary>
        /// Gets the temporary path to use to download a remote file. The file is downloaded to here completely first before
        /// moving it to the real target path.
        /// </summary>
        /// <param name="remoteFile">The remote file being downloaded.</param>
        /// <returns>The temporary path for the <paramref name="remoteFile"/>.</returns>
        protected string GetTempPath(string remoteFile)
        {
            return PathHelper.CombineDifferentPaths(TempPath, remoteFile);
        }

        /// <summary>
        /// Removes the <paramref name="remoteFile"/> from the download failure dictionaries.
        /// </summary>
        /// <param name="remoteFile">The remote file.</param>
        /// <param name="getLock">If true, a lock will be aquired. Set to false when calling this from inside a
        /// <see cref="_downloadFailedDictSync"/> lock.</param>
        void RemoveFromFailedDicts(string remoteFile, bool getLock)
        {
            if (getLock)
            {
                lock (_downloadFailedDictSync)
                {
                    _downloadFailedDict.Remove(remoteFile);
                    _downloadFailedDictCount.Remove(remoteFile);
                }
            }
            else
            {
                _downloadFailedDict.Remove(remoteFile);
                _downloadFailedDictCount.Remove(remoteFile);
            }
        }

        /// <summary>
        /// Updates the <see cref="_maxAttempts"/> value. The <see cref="_downloadSourcesSync"/> needs to be acquired.
        /// </summary>
        void UpdateMaxAttempts()
        {
            var n = _downloadSources.Count * _attemptsPerSource;
            if (n < 3)
                n = 3;

            if (n > 255)
                n = 255;

            _maxAttempts = (byte)n;
        }

        /// <summary>
        /// Handles the <see cref="IDownloadSource.DownloadFailed"/> event for the <see cref="_downloadSources"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="remoteFile">The remote file.</param>
        /// <param name="localFilePath">The local file path.</param>
        void downloadSource_DownloadFailed(IDownloadSource sender, string remoteFile, string localFilePath)
        {
            var invokeFailed = false;
            var invokeFinished = false;

            lock (_downloadFailedDictSync)
            {
                // Increment the fail count for this file
                int fails;
                if (!_downloadFailedDictCount.TryGetValue(remoteFile, out fails))
                {
                    fails = 1;
                    _downloadFailedDictCount.Add(remoteFile, fails);
                }
                else
                    _downloadFailedDictCount[remoteFile]++;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Job `{0}` failed (attempt #{1}; retry? {2})", remoteFile, fails + 1, fails <= MaxAttempts);

                // Check if the failure has happened too many times and that we should just give up
                if (fails > MaxAttempts)
                {
                    RemoveFromFailedDicts(remoteFile, false);

                    lock (_downloadQueueSync)
                    {
                        var removed = _downloadQueue.Remove(remoteFile);
                        Debug.Assert(removed);

                        if (_downloadQueue.Count == 0)
                            invokeFinished = true;
                    }

                    lock (_failedDownloadsSync)
                    {
                        if (!_failedDownloads.Contains(remoteFile))
                            _failedDownloads.Add(remoteFile);
                    }

                    invokeFailed = true;
                }
                else
                {
                    // Add the downloader that just failed to the list of failed sources
                    List<IDownloadSource> failedSources;
                    if (!_downloadFailedDict.TryGetValue(remoteFile, out failedSources))
                    {
                        failedSources = new List<IDownloadSource>();
                        _downloadFailedDict.Add(remoteFile, failedSources);
                    }

                    failedSources.Add(sender);

                    // If every source we have has failed, empty the list so they can all try again. This forces
                    // failed files to rotate through all sources before trying the same one again.
                    var containsAllSources = true;
                    lock (_downloadSourcesSync)
                    {
                        foreach (var src in _downloadSources)
                        {
                            if (!failedSources.Contains(src))
                            {
                                containsAllSources = false;
                                break;
                            }
                        }
                    }

                    if (containsAllSources)
                        failedSources.Clear();

                    // Add back to the NotStartedQueue so it can be attempted again
                    lock (_notStartedQueueSync)
                    {
                        Debug.Assert(!_notStartedQueue.Contains(remoteFile), "Why is this item already in the NotStartedQueue?");
                        _notStartedQueue.Enqueue(remoteFile);
                    }
                }
            }

            // Invoke the DownloadFailed event if needed (done here to prevent invoking while in lock)
            if (invokeFailed)
            {
                try
                {
                    if (DownloadFailed != null)
                        DownloadFailed(this, remoteFile);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }

            // Invoke the DownloadFinished event if needed (done here to prevent invoking while in lock)
            if (invokeFinished)
            {
                try
                {
                    if (Finished != null)
                        Finished(this);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="IDownloadSource.DownloadFinished"/> event for the <see cref="_downloadSources"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="remoteFile">The remote file.</param>
        /// <param name="localFilePath">The local file path.</param>
        void downloadSource_DownloadFinished(IDownloadSource sender, string remoteFile, string localFilePath)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Job `{0}` finished successfully.", remoteFile);

            // Remove from the failed dictionaries
            RemoveFromFailedDicts(remoteFile, true);

            // Remove from failed list
            lock (_failedDownloadsSync)
            {
                _failedDownloads.Remove(remoteFile);
            }

            // Remove from the download queue
            var finished = false;
            lock (_downloadQueueSync)
            {
                var removed = _downloadQueue.Remove(remoteFile);
                Debug.Assert(removed);

                if (_downloadQueue.Count == 0)
                    finished = true;
            }

            // Add to the finished queue
            lock (_finishedDownloadsSync)
            {
                _finishedDownloads.Add(remoteFile);
            }

            var tempPath = GetTempPath(remoteFile);
            var targetPath = GetTargetPath(remoteFile);

            // Ensure the target path exists
            lock (_fileSystemSync)
            {
                var dir = Path.GetDirectoryName(targetPath);
                if (dir != null)
                {
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                }
            }

            // Try to move the file
            var fileCopied = true;
            try
            {
                lock (_fileSystemSync)
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Copying file from `{0}` to `{1}`.", tempPath, targetPath);

                    File.Copy(tempPath, targetPath, true);
                }
            }
            catch (Exception ex)
            {
                fileCopied = false;

                const string errmsg = "File copy from `{0}` to `{1}` failed: {2}";

                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, tempPath, targetPath, ex);

                Debug.Fail(string.Format(errmsg, tempPath, targetPath, ex));

                try
                {
                    if (FileMoveFailed != null)
                        FileMoveFailed(this, remoteFile, tempPath, targetPath);
                }
                catch (NullReferenceException ex2)
                {
                    Debug.Fail(ex2.ToString());
                }
            }

            // Delete the old file
            if (fileCopied)
            {
                lock (_fileSystemSync)
                {
                    try
                    {
                        if (File.Exists(tempPath))
                        {
                            if (log.IsDebugEnabled)
                                log.DebugFormat("Deleting temporary file: {0}", tempPath);

                            File.Delete(tempPath);
                        }
                    }
                    catch (IOException ex)
                    {
                        const string errmsg = "Failed to delete temporary file `{0}`: {1}";

                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg, tempPath, ex);

                        Debug.Fail(string.Format(errmsg, tempPath, ex));
                    }
                }

                // Notify that the file successfully downloaded and was moved
                try
                {
                    if (DownloadFinished != null)
                        DownloadFinished(this, remoteFile, targetPath);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }

            // Notify if all jobs have finished
            if (finished)
            {
                try
                {
                    if (Finished != null)
                        Finished(this);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Notifies listeners when a file completely failed to be downloaded after <see cref="IDownloadManager.MaxAttempts"/>
        /// attempts.
        /// </summary>
        public event DownloadManagerDownloadFailedEventHandler DownloadFailed;

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
        /// Notifies listeners when all of the jobs in this <see cref="IDownloadManager"/> have finished. This event will
        /// be raised whenever the job queue hits 0. So if some jobs are added, they finish, then move jobs are added and finish,
        /// this event will be raised twice.
        /// </summary>
        public event DownloadManagerEventHandler Finished;

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
        /// Gets the version of the file being downloaded. When not set, calls to the <see cref="IDownloadSource"/> will not specify
        /// a version number when downloading files.
        /// </summary>
        public int? DownloadVersion
        {
            get { return _downloadVersion; }
        }

        /// <summary>
        /// Gets the number of items in the list of files that failed to be downloaded.
        /// </summary>
        public int FailedDownloadsCount
        {
            get
            {
                lock (_failedDownloadsSync)
                {
                    return _failedDownloads.Count;
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
        /// Gets the maximum times a single file will attempt to download. If a download failes more than this many times, it will
        /// be aborted and the <see cref="IDownloadManager.DownloadFailed"/> event will be raised.
        /// </summary>
        public int MaxAttempts
        {
            get { return _maxAttempts; }
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

            if (log.IsDebugEnabled)
                log.DebugFormat("Adding DownloadSource: {0}", downloadSource);

            lock (_downloadSourcesSync)
            {
                if (_downloadSources.Contains(downloadSource))
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("DownloadSource `{0}` not added - already in collection.", downloadSource);

                    return false;
                }

                downloadSource.DownloadFinished += downloadSource_DownloadFinished;
                downloadSource.DownloadFailed += downloadSource_DownloadFailed;

                _downloadSources.Add(downloadSource);

                UpdateMaxAttempts();
            }

            return true;
        }

        /// <summary>
        /// Adds multiple <see cref="IDownloadSource"/>s to this <see cref="IDownloadManager"/>.
        /// </summary>
        /// <param name="downloadSources">The <see cref="IDownloadSource"/>s to add.</param>
        public void AddSources(IEnumerable<IDownloadSource> downloadSources)
        {
            foreach (var src in downloadSources)
            {
                AddSource(src);
            }
        }

        /// <summary>
        /// Clears the failed downloads information.
        /// </summary>
        public void ClearFailed()
        {
            if (log.IsDebugEnabled)
                log.Debug("Clearing all failed download statistics.");

            lock (_failedDownloadsSync)
            {
                _failedDownloads.Clear();
            }
        }

        /// <summary>
        /// Clears the finished downloads information.
        /// </summary>
        public void ClearFinished()
        {
            if (log.IsDebugEnabled)
                log.Debug("Clearing all finished download statistics.");

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
                // Check if the file is already waiting to be downloaded, or has already been downloaded
                lock (_finishedDownloadsSync)
                {
                    if (_downloadQueue.Contains(file))
                    {
                        if (log.IsDebugEnabled)
                            log.DebugFormat("Enqueue file `{0}` failed: already in _downloadQueue.", file);

                        return false;
                    }

                    if (_finishedDownloads.Contains(file))
                    {
                        if (log.IsDebugEnabled)
                            log.DebugFormat("Enqueue file `{0}` failed: already in _finishedDownloads.", file);

                        return false;
                    }
                }

                // Remove any information about this file failing, since adding a failed file is just like restarting it
                lock (_failedDownloadsSync)
                {
                    _failedDownloads.Remove(file);
                }

                RemoveFromFailedDicts(file, true);

                // Add to the download queue
                _downloadQueue.Add(file);

                // Add to the not started queue
                lock (_notStartedQueueSync)
                {
                    _notStartedQueue.Enqueue(file);
                }

                if (log.IsInfoEnabled)
                    log.InfoFormat("Enqueued download job: {0}", file);
            }

            return true;
        }

        /// <summary>
        /// Enqueues multiple files for download.
        /// </summary>
        /// <param name="files">The files to enqueue for download.</param>
        public void Enqueue(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                Enqueue(file);
            }
        }

        /// <summary>
        /// Gets the files that failed to be downloaded. These files will not be re-attempted automatically since they had already
        /// passed the <see cref="IDownloadManager.MaxAttempts"/> limit.
        /// </summary>
        /// <returns>The files that failed to be downloaded</returns>
        public IEnumerable<string> GetFailedDownloads()
        {
            lock (_failedDownloadsSync)
            {
                return _failedDownloads.ToArray();
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
            // Check for a valid argument
            if (downloadSource == null)
            {
                if (log.IsInfoEnabled)
                    log.Info("RemoveSource failed since the DownloadSource parameter was null.");

                return false;
            }

            // Remove from the _downloadSources list
            lock (_downloadSourcesSync)
            {
                if (!_downloadSources.Remove(downloadSource))
                {
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Could not remove DownloadSource `{0}` - was not in the _downloadSources list.",
                            downloadSource);

                    return false;
                }

                UpdateMaxAttempts();
            }

            // Remove the event hooks
            downloadSource.DownloadFinished -= downloadSource_DownloadFinished;
            downloadSource.DownloadFailed -= downloadSource_DownloadFailed;

            if (log.IsDebugEnabled)
                log.DebugFormat("DownloadSource `{0}` removed from DownloadManager `{1}`.", downloadSource, this);

            return true;
        }

        #endregion
    }
}