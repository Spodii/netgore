using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// Base class for the <see cref="FileServerInfo"/> and <see cref="MasterServerInfo"/>.
    /// </summary>
    public abstract class ServerInfoBase : IDisposable
    {
        /// <summary>
        /// The delimiter used on the creation strings for the <see cref="ServerInfoBase"/>.
        /// </summary>
        public const string CreationStringDelimiter = "|";

        /// <summary>
        /// How long, in milliseconds, the worker thread will sleep when there are no jobs available.
        /// </summary>
        const int _workerThreadNoJobTimeout = 1000;

        protected static readonly ManagerSettings _settings = ManagerSettings.Instance;

        readonly object _infoSync = new object();
        readonly object _syncVersionSync = new object();
        readonly Timer _updateJobCountTimer;

        /// <summary>
        /// A queue of versions to check if synchronized and, if not, to start synchronizing.
        /// </summary>
        readonly Queue<int> _versionSyncQueue = new Queue<int>();

        readonly Thread _workerThread;
        string _downloadHost;
        DownloadSourceType _fileDownloaderType;

        IFileUploader _fileUploader;
        FileUploaderType _fileUploaderType;
        string _host;
        bool _isBusySyncing;
        bool _isDisposed = false;
        int _lastJobsRemaining = int.MinValue;
        string _password;
        string _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerInfoBase"/> class.
        /// </summary>
        /// <param name="type">The type of file uploader to use.</param>
        /// <param name="host">The host address.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <param name="downloadType">The type of file downloader to use.</param>
        /// <param name="downloadHost">The download host.</param>
        protected ServerInfoBase(FileUploaderType type, string host, string user, string password, DownloadSourceType downloadType,
                                 string downloadHost)
        {
            _fileUploaderType = type;
            _host = host;
            _user = user;
            _password = password;
            _fileDownloaderType = downloadType;
            _downloadHost = downloadHost;

            _updateJobCountTimer = new Timer(InvokeProgressChanged, null, 1000, 1000);

            // Listen for when any versions change
            _settings.LiveVersionChanged += _settings_LiveVersionChanged;
            _settings.NextVersionCreated += _settings_NextVersionCreated;

            // Enqueue the live version and, if it exists, the next version, to ensure they are synchronized
            EnqueueSyncVersion(_settings.LiveVersion);
            if (_settings.DoesNextVersionExist())
                EnqueueSyncVersion(_settings.LiveVersion + 1);

            // Create the initial file uploader
            RecreateFileUploader();

            // Create the worker thread and start it
            _workerThread = new Thread(WorkerThreadLoop) { IsBackground = true };

            try
            {
                _workerThread.Name = "MasterServerInfo [" + GetHashCode() + "] worker thread.";
            }
            catch (InvalidOperationException)
            {
            }

            _workerThread.Start();
        }

        /// <summary>
        /// Notifies listeners when the progress of the server has changed.
        /// </summary>
        public event ServerInfoEventHandler ProgressChanged;

        /// <summary>
        /// Notifies listeners when the server settings or state has changed.
        /// </summary>
        public event ServerInfoEventHandler ServerChanged;

        /// <summary>
        /// Gets the host to use to download.
        /// </summary>
        public string DownloadHost
        {
            get { return _downloadHost; }
        }

        /// <summary>
        /// Gets the type of <see cref="IDownloadSource"/> to use.
        /// </summary>
        public DownloadSourceType DownloadSourceType
        {
            get { return _fileDownloaderType; }
        }

        /// <summary>
        /// Gets the type of <see cref="IFileUploader"/> to use.
        /// </summary>
        public FileUploaderType FileUploaderType
        {
            get { return _fileUploaderType; }
        }

        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        public string Host
        {
            get { return _host; }
        }

        /// <summary>
        /// Gets if this <see cref="FileServerInfo"/> is busy synchronizing to the server.
        /// </summary>
        public bool IsBusySyncing
        {
            get { return _isBusySyncing; }
            private set
            {
                if (_isBusySyncing == value)
                    return;

                _isBusySyncing = value;

                try
                {
                    if (ServerChanged != null)
                        ServerChanged(this);
                }
                catch (NullReferenceException)
                {
                }
            }
        }

        /// <summary>
        /// Gets if this object instance has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets the number of jobs remaining. Includes both queued and in-progress jobs.
        /// </summary>
        public int JobsRemaining
        {
            get { return _fileUploader.JobsRemaining; }
        }

        /// <summary>
        /// Gets the password used to use to log into the server.
        /// </summary>
        public string Password
        {
            get { return _password; }
        }

        /// <summary>
        /// Gets the user name to use to log into the server.
        /// </summary>
        public string User
        {
            get { return _user; }
        }

        /// <summary>
        /// Changes the server information.
        /// </summary>
        /// <param name="newHost">The new host.</param>
        /// <param name="newUser">The new user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="newType">The new type of <see cref="IFileUploader"/>.</param>
        /// <param name="newDownloadType">The new type of <see cref="IDownloadSource"/>.</param>
        /// <param name="newDownloadHost">The new download host.</param>
        /// <exception cref="ArgumentNullException"><paramref name="newHost"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newUser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newPassword"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newDownloadHost"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="newType"/> is not a valid enum value.</exception>
        /// <exception cref="ArgumentException"><paramref name="newDownloadType"/> is not a valid enum value.</exception>
        public void ChangeInfo(string newHost, string newUser, string newPassword, FileUploaderType newType,
                               DownloadSourceType newDownloadType, string newDownloadHost)
        {
            if (newHost == null)
                throw new ArgumentNullException("newHost");
            if (newUser == null)
                throw new ArgumentNullException("newUser");
            if (newPassword == null)
                throw new ArgumentNullException("newPassword");
            if (newDownloadHost == null)
                throw new ArgumentNullException("newDownloadHost");

            if (!Enum.IsDefined(typeof(FileUploaderType), newType))
                throw new ArgumentException("Invalid FileUploaderType value: " + newType, "newType");
            if (!Enum.IsDefined(typeof(DownloadSourceType), newDownloadType))
                throw new ArgumentException("Invalid DownloadSourceType value: " + newDownloadType, "newDownloadType");

            lock (_infoSync)
            {
                // Check if any values are different
                var sc = StringComparer.Ordinal;

                if (sc.Equals(newHost, Host) && sc.Equals(newUser, User) && sc.Equals(newPassword, Password) &&
                    newType == FileUploaderType && newDownloadType == DownloadSourceType &&
                    sc.Equals(newDownloadHost, DownloadHost))
                    return;

                // Set the new values
                _host = newHost;
                _user = newUser;
                _password = newPassword;
                _fileUploaderType = newType;
                _downloadHost = newDownloadHost;
                _fileDownloaderType = newDownloadType;

                // Force saving
                _settings.Save();

                // Recreate the uploader
                RecreateFileUploader();

                try
                {
                    if (ServerChanged != null)
                        ServerChanged(this);
                }
                catch (NullReferenceException)
                {
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="IFileUploader"/> instance.
        /// </summary>
        /// <param name="type">The <see cref="FileUploaderType"/>.</param>
        /// <param name="host">The host.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>The <see cref="IFileUploader"/> instance.</returns>
        public static IFileUploader CreateFileUploader(FileUploaderType type, string host, string user, string password)
        {
            switch (type)
            {
                case FileUploaderType.Ftp:
                    return new FtpFileUploader(host, user, password);

                default:
                    const string errmsg = "FileUploaderType `{0}` not supported.";
                    Debug.Fail(string.Format(errmsg, type));
                    throw new NotSupportedException(string.Format(errmsg, type));
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles synchronizing the given version.
        /// </summary>
        /// <param name="fu">The <see cref="IFileUploader"/> to use.</param>
        /// <param name="v">The version to synchronize.</param>
        /// <param name="reEnqueue">True if the <paramref name="v"/> should be re-enqueued so it can be re-attempted.
        /// If the method throws an <see cref="Exception"/>, the <paramref name="v"/> will be re-enqueued no matter what.</param>
        /// <returns>The error string, or null if the synchronization was successful.</returns>
        protected abstract string DoSync(IFileUploader fu, int v, out bool reEnqueue);

        /// <summary>
        /// Enqueues a version to be checked if synchronized
        /// </summary>
        /// <param name="version">The version to synchronize.</param>
        protected void EnqueueSyncVersion(int version)
        {
            if (version <= 0)
                return;

            lock (_syncVersionSync)
            {
                if (!_versionSyncQueue.Contains(version))
                    _versionSyncQueue.Enqueue(version);
            }
        }

        /// <summary>
        /// Gets a string that can be used to recreate this object instance.
        /// </summary>
        /// <returns>A string that can be used to recreate this object instance.</returns>
        public virtual string GetCreationString()
        {
            lock (_infoSync)
            {
                return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", CreationStringDelimiter, FileUploaderType, Host, User,
                                     Password, DownloadSourceType, DownloadHost);
            }
        }

        /// <summary>
        /// Gets the path to a file for a specific version on the remote server.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="remotePath">The relative remote file or folder path. Use null or empty string for the root directory.</param>
        /// <returns>The <paramref name="remotePath"/> for the given <paramref name="version"/>.</returns>
        protected static string GetVersionRemoteFilePath(int version, string remotePath)
        {
            var path = PathHelper.GetVersionString(version);
            if (string.IsNullOrEmpty(remotePath))
                return path + "/";

            if (!remotePath.StartsWith("/"))
                path += "/";

            path += remotePath;

            return path;
        }

        /// <summary>
        /// Checks if the FileUploader's jobs remaining count has changed and, if it does, invokes ProgressChanged.
        /// </summary>
        /// <param name="dummy">Dummy argument - unused.</param>
        void InvokeProgressChanged(object dummy = null)
        {
            var fu = _fileUploader;
            if (fu == null)
                return;

            var jr = fu.JobsRemaining;
            if (jr == _lastJobsRemaining)
                return;

            _lastJobsRemaining = jr;

            try
            {
                if (ProgressChanged != null)
                    ProgressChanged(this);
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// Recreates the <see cref="IFileUploader"/> and starts updating again.
        /// </summary>
        void RecreateFileUploader()
        {
            // Dispose of the old uploader
            if (_fileUploader != null)
                _fileUploader.Dispose();

            // Create the new uploader
            _fileUploader = CreateFileUploader(FileUploaderType, Host, User, Password);

            _lastJobsRemaining = int.MinValue;

            // Start the synchronization of the live and next version
            EnqueueSyncVersion(_settings.LiveVersion);
            if (_settings.DoesNextVersionExist())
                EnqueueSyncVersion(_settings.LiveVersion + 1);
        }

        /// <summary>
        /// The main loop for the worker thread.
        /// </summary>
        void WorkerThreadLoop()
        {
            while (!IsDisposed)
            {
                // Get the file uploader
                IFileUploader fu;
                lock (_infoSync)
                {
                    fu = _fileUploader;
                }

                if (fu == null)
                {
                    // Hopefully a null IFileUploader scenario fixes itself over time
                    Thread.Sleep(500);
                    continue;
                }

                // Grab the next version to check if synced
                var v = int.MinValue;
                lock (_syncVersionSync)
                {
                    if (_versionSyncQueue.Count > 0)
                        v = _versionSyncQueue.Dequeue();
                }

                // If no jobs, sleep for a while
                if (v == int.MinValue)
                {
                    IsBusySyncing = false;
                    Thread.Sleep(_workerThreadNoJobTimeout);
                    continue;
                }

                IsBusySyncing = true;

                bool reEnqueue;

                try
                {
                    var err = DoSync(fu, v, out reEnqueue);
                    if (!string.IsNullOrEmpty(err))
                        Debug.Fail(err);
                }
                catch (Exception ex)
                {
                    reEnqueue = true;

                    // Ignore printing error messages when the uploader is null
                    if (!fu.IsDisposed)
                        Debug.Fail(ex.ToString());
                }

                if (reEnqueue)
                {
                    // Re-enqueue the version so we can try again
                    lock (_syncVersionSync)
                    {
                        if (!_versionSyncQueue.Contains(v))
                            _versionSyncQueue.Enqueue(v);
                    }
                }
            }
        }

        void _settings_LiveVersionChanged(ManagerSettings sender)
        {
            EnqueueSyncVersion(sender.LiveVersion);
        }

        void _settings_NextVersionCreated(ManagerSettings sender)
        {
            EnqueueSyncVersion(sender.LiveVersion + 1);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            _isDisposed = true;

            if (_updateJobCountTimer != null)
                _updateJobCountTimer.Dispose();

            // Remove the event listeners
            _settings.NextVersionCreated -= _settings_NextVersionCreated;
            _settings.LiveVersionChanged -= _settings_LiveVersionChanged;

            // Kill the uploader
            if (_fileUploader != null)
            {
                try
                {
                    _fileUploader.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.ToString());
                }
            }
        }

        #endregion
    }
}