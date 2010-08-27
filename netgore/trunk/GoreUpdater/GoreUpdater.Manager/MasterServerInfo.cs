using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// Describes a master server instance.
    /// </summary>
    public class MasterServerInfo : IDisposable
    {
        static readonly ManagerSettings _settings = ManagerSettings.Instance;

        readonly object _infoSync = new object();
        readonly object _syncVersionSync = new object();
        readonly Thread _workerThread;

        IFileUploader _fileUploader;
        FileUploaderType _fileUploaderType;
        string _host;
        string _password;
        string _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterServerInfo"/> class.
        /// </summary>
        /// <param name="host">The host address.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public MasterServerInfo(string host, string user, string password)
        {
            _host = host;
            _user = user;
            _password = password;

            _settings.LiveVersionChanged += _settings_LiveVersionChanged;
            _settings.NextVersionCreated += _settings_NextVersionCreated;

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
        /// How long, in milliseconds, the worker thread will sleep when there are no jobs available.
        /// </summary>
        const int _workerThreadNoJobTimeout = 1000;

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
                    Thread.Sleep(_workerThreadNoJobTimeout);
                    continue;
                }

                // Grab the next version to check if synced
                int v = int.MinValue;
                lock (_syncVersionSync)
                {
                    if (_versionSyncQueue.Count > 0)
                        v = _versionSyncQueue.Dequeue();
                }

                // If no jobs, sleep for a while
                if (v == int.MinValue)
                {
                    Thread.Sleep(_workerThreadNoJobTimeout);
                    continue;
                }

                try
                {
                    // Load the VersionFileList for the version to check
                    var vflPath =VersionHelper.GetVersionFileListPath(v);
                    if (!File.Exists(vflPath))
                    {
                        // Version doesn't exist at all
                        continue;
                    }

                    var vfl = VersionFileList.CreateFromFile(vflPath);

                    // Try to download the version's file list hash
                    var fileListHashPath = GetVersionRemoteFilePath(v, PathHelper.RemoteFileListHashFileName);
                    string vflHahs = fu.DownloadAsString(fileListHashPath);

                    // Check if the hash matches the current version's hash
                    var expectedVflHash = File.ReadAllText(VersionHelper.GetVersionFileListHashPath(v));
                    if (vflHahs != expectedVflHash)
                    {
                        // Delete the whole version folder first
                        fu.DeleteDirectory(GetVersionRemoteFilePath(v, null));
                    }

                    // Check the hashes of the local files
                    foreach (var f in vfl.Files)
                    {
                        // Get the local file path
                        var localPath = VersionHelper.GetVersionFile(v, f.FilePath);

                        // Confirm the hash of the file
                        var fileHash = Hasher.GetFileHash(localPath);
                        if (fileHash != f.Hash)
                        {
                            const string errmsg = "The cached hash ({0}) of file `{1}` does not match the real hash ({2}) for version {3}." 
                                + " Possible version corruption.";
                            throw new Exception(string.Format(errmsg, f.Hash, f.FilePath, fileHash, v));
                        }
                    }

                    // Hashes check out, start uploading
                    foreach (var f in vfl.Files)
                    {
                        // Get the local file path
                        var localPath = VersionHelper.GetVersionFile(v, f.FilePath);

                        var remotePath =GetVersionRemoteFilePath(v, f.FilePath);
                        fu.UploadAsync(localPath, remotePath);
                    }

                    // Wait for uploads to finish
                    while (fu.IsBusy)
                    {
                        Thread.Sleep(1000);
                    }

                    // All uploads have finished, so upload the VersionFileList hash
                    var vflHashRemoteFilePath = GetVersionRemoteFilePath(v, PathHelper.RemoteFileListHashFileName);
                    fu.UploadAsync(vflPath, vflHashRemoteFilePath);

                    // All done! That was easy enough, eh? *sigh*
                }
                catch (Exception ex)
                {
                    // When the FileUploader has been disposed, just ignore whatever exception
                    if (!fu.IsDisposed)
                    {
                    Debug.Fail(ex.ToString());

                    // Certain exceptions we want to be rethrown
                    if (ex.Message.StartsWith("The cached hash"))
                        throw;

                    // Re-enqueue the version so we can try again
                    lock (_syncVersionSync)
                    {
                        if (!_versionSyncQueue.Contains(v))
                            _versionSyncQueue.Enqueue(v);
                    }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the path to a file for a specific version on the remote server from the root.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="remotePath">The remote file or folder path. Use null or empty string for the root directory.</param>
        /// <returns>The <paramref name="remotePath"/> for the given <paramref name="version"/>.</returns>
        static string GetVersionRemoteFilePath(int version, string remotePath)
        {
            var path = PathHelper.GetVersionString(version);
            if (string.IsNullOrEmpty(remotePath))
                return path + "/";

            if (!remotePath.StartsWith("/"))
                path += "/";

            path += remotePath;

            return path;
        }

        void _settings_NextVersionCreated(ManagerSettings sender)
        {
            EnqueueSyncVersion(sender.LiveVersion + 1);
        }

        void _settings_LiveVersionChanged(ManagerSettings sender)
        {
            EnqueueSyncVersion(sender.LiveVersion);
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
        /// <exception cref="ArgumentNullException"><paramref name="newHost"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newUser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newPassword"/> is null.</exception>
        public void ChangeInfo(string newHost, string newUser, string newPassword, FileUploaderType newType)
        {
            if (newHost == null)
                throw new ArgumentNullException("newHost");
            if (newUser == null)
                throw new ArgumentNullException("newUser");
            if (newPassword == null)
                throw new ArgumentNullException("newPassword");

            lock (_infoSync)
            {
                // Check if any values are different
                var sc = StringComparer.Ordinal;

                if (sc.Equals(newHost, Host) && sc.Equals(newUser, User) && sc.Equals(newPassword, Password) &&
                    newType == FileUploaderType)
                    return;

                // Set the new values
                _host = newHost;
                _user = newUser;
                _password = newPassword;
                _fileUploaderType = newType;

                // Recreate the uploader
                RecreateFileUploader();
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
            switch (FileUploaderType)
            {
                case FileUploaderType.Ftp:
                    _fileUploader = new FtpFileUploader(Host, User, Password);
                    break;

                default:
                    const string errmsg = "FileUploaderType `{0}` not supported.";
                    Debug.Fail(string.Format(errmsg, FileUploaderType));
                    throw new NotSupportedException(string.Format(errmsg, FileUploaderType));
            }

            // Start the synchronization of the live and next version
            EnqueueSyncVersion(_settings.LiveVersion);
            if (_settings.DoesNextVersionExist())
                EnqueueSyncVersion(_settings.LiveVersion + 1);

            // TODO: Start checking if the server is up-to-date
        }

        /// <summary>
        /// A queue of versions to check if synchronized and, if not, to start synchronizing.
        /// </summary>
        readonly Queue<int> _versionSyncQueue = new Queue<int>();

        /// <summary>
        /// Enqueues a version to be checked if synchronized
        /// </summary>
        /// <param name="version">The version to synchronize.</param>
        void EnqueueSyncVersion(int version)
        {
            lock (_syncVersionSync)
            {
                if (!_versionSyncQueue.Contains(version))
                    _versionSyncQueue.Enqueue(version);
            }
        }

        bool _isDisposed = false;

        /// <summary>
        /// Gets if this object instance has been disposed.
        /// </summary>
        public bool IsDisposed { get { return _isDisposed; } }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            _isDisposed = true;

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