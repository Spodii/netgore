using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace GoreUpdater
{
    public class FtpFileUploader : IFileUploader
    {
        /// <summary>
        /// For how long each thread will time out when the job queue is empty.
        /// </summary>
        const int _emptyJobQueueTimeout = 500;

        /// <summary>
        /// For how long to wait after failing a job.
        /// </summary>
        const int _jobFailedTimeout = 200;

        /// <summary>
        /// The number of uploading threads for each <see cref="FtpFileUploader"/> instance.
        /// </summary>
        const int _numThreads = 3;

        static readonly object _webRequestSync = new object();
        readonly NetworkCredential _credentials;
        readonly string _host;

        readonly Queue<KeyValuePair<string, string>> _jobQueue = new Queue<KeyValuePair<string, string>>();
        readonly object _jobQueueSync = new object();
        readonly List<string> _remaining = new List<string>();
        readonly object _remainingSync = new object();
        readonly List<Thread> _threads = new List<Thread>();
        readonly object _threadsSync = new object();
        bool _isDisposed = false;

        int _outstanding = 0;

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            HandleDispose(true);
            _isDisposed = true;
        }

        #endregion

        public FtpFileUploader(string host, string username, string password)
        {
            UsePassive = true;

            _host = host;
            _credentials = new NetworkCredential(username, password);

            lock (_threadsSync)
            {
                for (var i = 0; i < _numThreads; i++)
                {
                    var t = new Thread(WorkerThreadLoop) { IsBackground = true };
                    try
                    {
                        t.Name = "FtpFileUploader worker " + i + " for: " + host;
                    }
                    catch (Exception ex)
                    {
                        Debug.Fail(ex.ToString());
                    }

                    t.Start();

                    _threads.Add(t);
                }
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="FtpFileUploader"/> is reclaimed by garbage collection.
        /// </summary>
        ~FtpFileUploader()
        {
            HandleDispose(false);
        }

        /// <summary>
        /// Handles disposing this object.
        /// </summary>
        /// <param name="disposeManaged">If managed resources should be disposed.</param>
        protected virtual void HandleDispose(bool disposeManaged)
        {
            lock (_threadsSync)
            {
                foreach (var t in _threads)
                {
                    // ReSharper disable EmptyGeneralCatchClause
                    try
                    {
                        t.Abort();
                    }
                    catch (Exception)
                    {
                    }
                    // ReSharper restore EmptyGeneralCatchClause
                }

                _threads.Clear();
            }
        }

        /// <summary>
        /// The method for worker threads.
        /// </summary>
        void WorkerThreadLoop()
        {
            while (true)
            {
                KeyValuePair<string, string>? job = null;

                // Get the next job
                lock (_jobQueueSync)
                {
                    if (_jobQueue.Count > 0)
                    {
                        Interlocked.Increment(ref _outstanding);
                        job = _jobQueue.Dequeue();
                    }
                }

                // Sleep if there was no jobs free
                if (!job.HasValue)
                {
                    Thread.Sleep(_emptyJobQueueTimeout);
                    continue;
                }

                var fullRemotePath = PathHelper.CombineDifferentPaths(_host, job.Value.Value);
                fullRemotePath = fullRemotePath.Replace('\\', '/');

                // Process the job
                FtpWebRequest req;
                lock (_webRequestSync)
                {
                    req = (FtpWebRequest)WebRequest.Create(fullRemotePath);
                }

                try
                {
                    // Ensure the local file exists
                    if (!File.Exists(job.Value.Key))
                    {
                        if (UploadError != null)
                            UploadError(this, job.Value.Key, job.Value.Value, "Local file does not exist.");

                        lock (_jobQueueSync)
                        {
                            _jobQueue.Enqueue(job.Value);
                            Interlocked.Decrement(ref _outstanding);
                        }

                        Thread.Sleep(_jobFailedTimeout);
                        continue;
                    }

                    // TODO: Check if the remote file exists already

                    // Read the file to upload
                    var data = File.ReadAllBytes(job.Value.Key);

                    // Set up the request
                    req.Method = WebRequestMethods.Ftp.UploadFile;
                    req.UsePassive = false;
                    req.KeepAlive = false;
                    req.Credentials = _credentials;
                    req.ContentLength = data.Length;
                    req.ContentOffset = 0;

                    var reqStream = req.GetRequestStream();
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                catch (Exception ex)
                {
                    if (UploadError != null)
                        UploadError(this, job.Value.Key, job.Value.Value, ex.ToString());

                    lock (_jobQueueSync)
                    {
                        _jobQueue.Enqueue(job.Value);
                        Interlocked.Decrement(ref _outstanding);
                    }

                    Thread.Sleep(_jobFailedTimeout);
                    continue;
                }

                // File uploaded
                Interlocked.Decrement(ref _outstanding);
            }
        }

        #region Implementation of IFileUploader

        public event FileUploaderErrorEventHandler UploadError;

        /// <summary>
        /// Gets if the <see cref="IFileUploader"/> is currently busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                lock (_remainingSync)
                {
                    return _remaining.Count > 0 || _outstanding > 0;
                }
            }
        }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets or sets if files that already exist on the destination will be skipped.
        /// </summary>
        public bool SkipIfExists { get; set; }

        /// <summary>
        /// Gets or sets if passive FTP will be used. Default is true. Note that you may have to change this value to get
        /// the FTP connection to work.
        /// </summary>
        public bool UsePassive { get; set; }

        /// <summary>
        /// Enqueues a file for uploading.
        /// </summary>
        /// <param name="sourcePath">The path to the local file to upload.</param>
        /// <param name="targetPath">The path to upload the file to on the destionation.</param>
        public bool Enqueue(string sourcePath, string targetPath)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath))
                return false;

            lock (_remainingSync)
            {
                if (_remaining.Contains(sourcePath, StringComparer.Ordinal))
                    return false;

                _remaining.Add(sourcePath);

                var kvp = new KeyValuePair<string, string>(sourcePath, targetPath);

                lock (_jobQueueSync)
                {
                    _jobQueue.Enqueue(kvp);
                }
            }

            return true;
        }

        /// <summary>
        /// Enqueues multiple files for uploading.
        /// </summary>
        /// <param name="files">The files to upload, where the key is the source path, and the value is the
        /// path to upload the file on the destination.</param>
        public void Enqueue(IEnumerable<KeyValuePair<string, string>> files)
        {
            lock (_remainingSync)
            {
                foreach (var kvp in files)
                {
                    if (string.IsNullOrEmpty(kvp.Key) || string.IsNullOrEmpty(kvp.Value))
                        continue;

                    if (_remaining.Contains(kvp.Key, StringComparer.Ordinal))
                        continue;

                    _remaining.Add(kvp.Key);

                    lock (_jobQueueSync)
                    {
                        _jobQueue.Enqueue(kvp);
                    }
                }
            }
        }

        #endregion
    }
}