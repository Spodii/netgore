using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GoreUpdater
{
    // TODO: Run the delete routine after successfully updating
    // TODO: Delete the _temp directory when done

    /// <summary>
    /// A master class for the updater client that performs the whole update process.
    /// This class is thread-safe.
    /// </summary>
    public class UpdateClient
    {
        readonly object _isRunningSync = new object();
        readonly UpdateClientSettings _settings;
        readonly object _stateSync = new object();

        DownloadManager _dm;
        IOfflineFileReplacer _fileReplacer;
        bool _hasErrors = false;
        bool _isRunning = false;
        int? _liveVersion;
        MasterServerReader _msr;
        UpdateClientState _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateClient"/> class.
        /// </summary>
        /// <param name="settings">The <see cref="UpdateClientSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        public UpdateClient(UpdateClientSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            // Get a read-only deep copy of the settings so that nobody can accidentally change it while in use, including us
            _settings = settings.ReadOnlyDeepCopy();
        }

        /// <summary>
        /// Notifies listeners when a file has completely failed to be downloaded after multiple attempts. This is usually
        /// a breaking issue since all files need to be updated for the update to finish but this file could not download at all.
        /// </summary>
        public event UpdateClientFileDownloadFailedEventHandler FileDownloadFailed;

        /// <summary>
        /// Notifies listeners when a file that needs to be updated has finished being downloaded.
        /// </summary>
        public event UpdateClientFileDownloadedEventHandler FileDownloaded;

        /// <summary>
        /// Notifies listeners when a file was successfully downloaded to the temporary directory, but failed to be moved to
        /// the target directory. This is often the result of trying to replace a file that is currently in use.
        /// </summary>
        public event UpdateClientFileMoveFailedEventHandler FileMoveFailed;

        /// <summary>
        /// Notifies listeners when the <see cref="UpdateClient.HasErrors"/> property has changed.
        /// </summary>
        public event UpdateClientEventHandler HasErrorsChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="UpdateClient.IsRunning"/> property has changed.
        /// </summary>
        public event UpdateClientEventHandler IsRunningChanged;

        /// <summary>
        /// Notifies listeners when the live version has been found.
        /// </summary>
        public event UpdateClientEventHandler LiveVersionFound;

        /// <summary>
        /// Notifies listeners when there has been a critical error reading from the master server(s) that result in the update
        /// process not being able to continue.
        /// </summary>
        public event UpdateClientMasterServerReaderErrorEventHandler MasterServerReaderError;

        /// <summary>
        /// Notifies listeners when the <see cref="UpdateClient.State"/> property has changed.
        /// </summary>
        public event UpdateClientStateChangedEventHandler StateChanged;

        /// <summary>
        /// Gets if any breaking errors have occured during the update process. If true, then the update process most likely
        /// cannot be recovered and would have to be attempted again from the start.
        /// </summary>
        public bool HasErrors
        {
            get { return _hasErrors; }
            set
            {
                if (_hasErrors == value)
                    return;

                _hasErrors = value;

                try
                {
                    if (HasErrorsChanged != null)
                        HasErrorsChanged(this);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Gets if this <see cref="UpdateClient"/> is currently running.
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        /// <summary>
        /// Gets the live version that was found from the master server(s). Will be null if the live version has not yet been found.
        /// </summary>
        public int? LiveVersion
        {
            get { return _liveVersion; }
            set
            {
                if (_liveVersion == value)
                    return;

                _liveVersion = value;

                try
                {
                    if (LiveVersionFound != null)
                        LiveVersionFound(this);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="UpdateClientSettings"/> used to create this <see cref="UpdateClient"/>.
        /// </summary>
        public UpdateClientSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the current <see cref="UpdateClientState"/> that this instance is in.
        /// </summary>
        public UpdateClientState State
        {
            get
            {
                lock (_stateSync)
                {
                    return _state;
                }
            }
            private set
            {
                UpdateClientState oldState;

                lock (_stateSync)
                {
                    if (_state == value)
                        return;

                    oldState = _state;

                    _state = value;
                }

                try
                {
                    if (StateChanged != null)
                        StateChanged(this, oldState, _state);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Checks if the downloading with the <see cref="DownloadManager"/> has been completed.
        /// </summary>
        void CheckIfDownloadManagerComplete()
        {
            // If the download manager is null, then skip checking it
            if (_dm != null)
            {
                if (!_dm.IsDisposed)
                {
                    // Do not continue if items are still enqueued for download
                    if (_dm.QueueCount > 0)
                    {
                        Debug.Fail("Why are items still in the download queue?");
                        return;
                    }

                    // Clean up
                    try
                    {
                        _dm.Dispose();
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to dispose DownloadManager `{0}`. Reason: {1}";
                        Debug.Fail(string.Format(errmsg, _dm, ex));
                    }
                }

                _dm = null;
            }

            // Abort if HasErrors is true
            if (HasErrors)
            {
                State = UpdateClientState.Completed;
                return;
            }

            // If done, update the version
            TrySetClientVersionToLive();

            // Change the state
            State = UpdateClientState.Completed;

            // Set to not running
            lock (_isRunningSync)
            {
                Debug.Assert(_isRunning);

                _isRunning = false;

                try
                {
                    if (IsRunningChanged != null)
                        IsRunningChanged(this);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="IDownloadManager.DownloadFailed"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IDownloadManager"/> the event came from.</param>
        /// <param name="remoteFile">The remote file that was downloaded.</param>
        void DownloadManager_DownloadFailed(IDownloadManager sender, string remoteFile)
        {
            Debug.Assert(sender == _dm, "Why did we get an event from a different IDownloadManager?");

            HasErrors = true;

            try
            {
                if (FileDownloadFailed != null)
                    FileDownloadFailed(this, remoteFile);
            }
            catch (NullReferenceException ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        /// <summary>
        /// Handles the <see cref="IDownloadManager.DownloadFinished"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IDownloadManager"/> the event came from.</param>
        /// <param name="remoteFile">The remote file that was downloaded.</param>
        /// <param name="localFilePath">The path to the local file where the downloaded file is stored.</param>
        void DownloadManager_DownloadFinished(IDownloadManager sender, string remoteFile, string localFilePath)
        {
            Debug.Assert(sender == _dm, "Why did we get an event from a different IDownloadManager?");

            try
            {
                if (FileDownloaded != null)
                    FileDownloaded(this, remoteFile, localFilePath);
            }
            catch (NullReferenceException ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        /// <summary>
        /// Handles the <see cref="IDownloadManager.FileMoveFailed"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IDownloadManager"/> the event came from.</param>
        /// <param name="remoteFile">The remote file that was downloaded.</param>
        /// <param name="localFilePath">The path to the local file where the downloaded file is stored.</param>
        /// <param name="targetFilePath">The path where the file is supposed to be, but failed to be moved to.</param>
        void DownloadManager_FileMoveFailed(IDownloadManager sender, string remoteFile, string localFilePath,
                                            string targetFilePath)
        {
            Debug.Assert(sender == _dm, "Why did we get an event from a different IDownloadManager?");

            _fileReplacer.AddJob(localFilePath, targetFilePath);

            try
            {
                if (FileMoveFailed != null)
                    FileMoveFailed(this, remoteFile, localFilePath, targetFilePath);
            }
            catch (NullReferenceException ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        /// <summary>
        /// Handles the <see cref="IDownloadManager.Finished"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IDownloadManager"/> the event came from.</param>
        void DownloadManager_Finished(IDownloadManager sender)
        {
            Debug.Assert(sender == _dm, "Why did we get an event from a different IDownloadManager?");

            CheckIfDownloadManagerComplete();
        }

        /// <summary>
        /// Checks the files provided in the given <see cref="VersionFileList"/> and compares them to the local files
        /// to see what files need to be updated.
        /// </summary>
        /// <param name="vfl">The <see cref="VersionFileList"/> containing the files to be updated.</param>
        /// <returns>The relative paths to the files that need to be updated.</returns>
        IEnumerable<string> FindFilesToUpdate(VersionFileList vfl)
        {
            var ret = new List<string>();

            // Loop through each file
            foreach (var updateFileInfo in vfl.Files)
            {
                // Get the local file path
                var localFilePath = PathHelper.CombineDifferentPaths(Settings.TargetPath, updateFileInfo.FilePath);

                // If the file does not exist, add it
                if (!File.Exists(localFilePath))
                {
                    ret.Add(updateFileInfo.FilePath);
                    continue;
                }

                // Get the info for the local file
                try
                {
                    var localFileInfo = new FileInfo(localFilePath);

                    // If the size of the local file doesn't equal the size of the updated file, avoid
                    // checking the hash and just update it
                    if (localFileInfo.Length != updateFileInfo.Size)
                    {
                        ret.Add(updateFileInfo.FilePath);
                        continue;
                    }

                    // File exists and is of the correct size, so compare the hash of the local file to the expected hash
                    var localFileHash = Hasher.GetFileHash(localFilePath);
                    if (!StringComparer.Ordinal.Equals(localFileHash, updateFileInfo.Hash))
                    {
                        ret.Add(updateFileInfo.FilePath);
                        continue;
                    }
                }
                catch (IOException ex)
                {
                    // On an exception, assume the file needs to be updated
                    Debug.Fail(ex.ToString());
                    ret.Add(updateFileInfo.FilePath);
                    continue;
                }

                // LOG: File passed hash check
            }

            // LOG: Number of files to update
            return ret;
        }

        /// <summary>
        /// The callback method for the <see cref="IMasterServerReader.BeginReadVersion"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IMasterServerReader"/> this event came from.</param>
        /// <param name="info">The information from the master server(s).</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        void MasterServerReader_Callback(IMasterServerReader sender, IMasterServerReadInfo info, object userState)
        {
            State = UpdateClientState.ReadingLiveVersionDone;

            // Check for errors
            if (info.Error != null)
            {
                HasErrors = true;

                // Raise error event
                try
                {
                    if (MasterServerReaderError != null)
                        MasterServerReaderError(this, info.Error);
                }
                catch (NullReferenceException ex)
                {
                    Debug.Fail(ex.ToString());
                }

                // Change the state
                State = UpdateClientState.Completed;

                // Set to not running
                lock (_isRunningSync)
                {
                    Debug.Assert(_isRunning);

                    _isRunning = false;

                    try
                    {
                        if (IsRunningChanged != null)
                            IsRunningChanged(this);
                    }
                    catch (NullReferenceException ex)
                    {
                        Debug.Fail(ex.ToString());
                    }
                }

                return;
            }

            // Set the found live version
            LiveVersion = info.Version;

            // Check if the live version equals our version and, if so, there is no need to continue with the update process
            var currentVersion = Settings.GetCurrentVersion();
            if (currentVersion.HasValue && currentVersion.Value == LiveVersion.Value)
            {
                // Change the state
                State = UpdateClientState.Completed;

                // Set to not running
                lock (_isRunningSync)
                {
                    Debug.Assert(_isRunning);

                    _isRunning = false;

                    try
                    {
                        if (IsRunningChanged != null)
                            IsRunningChanged(this);
                    }
                    catch (NullReferenceException ex)
                    {
                        Debug.Fail(ex.ToString());
                    }
                }

                return;
            }

            // Grab the VersionFileList
            State = UpdateClientState.ReadingLiveVersionFileList;

            _msr.BeginReadVersionFileList(MasterServerReader_Callback_VersionFileList, info.Version, this);
        }

        /// <summary>
        /// The callback method for the <see cref="IMasterServerReader.BeginReadVersionFileList"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IMasterServerReader"/> this event came from.</param>
        /// <param name="info">The information from the master server(s).</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        void MasterServerReader_Callback_VersionFileList(IMasterServerReader sender, IMasterServerReadInfo info, object userState)
        {
            State = UpdateClientState.ReadingLiveVersionFileListDone;

            // Check for a valid VersionFileList
            if (string.IsNullOrEmpty(info.VersionFileListText))
            {
                const string errmsg =
                    "Could not get a valid VersionFileList file from the master servers for version `{0}` - download failed.";
                if (MasterServerReaderError != null)
                    MasterServerReaderError(this, string.Format(errmsg, info.Version));
                HasErrors = true;
                return;
            }

            VersionFileList vfl;
            try
            {
                vfl = VersionFileList.CreateFromString(info.VersionFileListText);
            }
            catch (Exception ex)
            {
                const string errmsg =
                    "Could not get a valid VersionFileList file from the master servers for version `{0}`. Details: {1}";
                if (MasterServerReaderError != null)
                    MasterServerReaderError(this, string.Format(errmsg, info.Version, ex));
                HasErrors = true;
                return;
            }

            // Find the files to update
            var toUpdate = FindFilesToUpdate(vfl);

            // If all file hashes match, then we are good to go
            if (toUpdate.Count() == 0)
            {
                CheckIfDownloadManagerComplete();
                return;
            }

            // There was one or more files to update, so start the updating...

            // Create the DownloadManager
            _dm = new DownloadManager(Settings.TargetPath, Settings.TempPath, info.Version);
            _dm.DownloadFinished += DownloadManager_DownloadFinished;
            _dm.FileMoveFailed += DownloadManager_FileMoveFailed;
            _dm.DownloadFailed += DownloadManager_DownloadFailed;
            _dm.Finished += DownloadManager_Finished;

            State = UpdateClientState.UpdatingFiles;

            // Add the sources to the DownloadManager
            var sources = info.DownloadSources.Select(x => x.Instantiate());
            _dm.AddSources(sources);

            // Enqueue the files that need to be downloaded
            _dm.Enqueue(toUpdate);
        }

        /// <summary>
        /// Starts the asynchronous update process.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsRunning"/> was true.</exception>
        public void Start()
        {
            // Check the current IsRunning state, and set it to true
            bool isAlreadyRunning;
            lock (_isRunningSync)
            {
                isAlreadyRunning = IsRunning;
                if (!isAlreadyRunning)
                    _isRunning = true;
            }

            // Throw an exception if the IsRunning was already true
            if (isAlreadyRunning)
                throw new InvalidOperationException("The UpdateClient is already running.");

            try
            {
                if (IsRunningChanged != null)
                    IsRunningChanged(this);
            }
            catch (NullReferenceException ex)
            {
                Debug.Fail(ex.ToString());
            }

            State = UpdateClientState.Initializing;

            // Create the objects
            _msr = new MasterServerReader(Settings.LocalFileServerPath, Settings.LocalMasterServerPath);
            _fileReplacer = Settings.CreateOfflineFileReplacer();

            State = UpdateClientState.ReadingLiveVersion;

            // Start grabbing from the master server
            _msr.BeginReadVersion(MasterServerReader_Callback, this);
        }

        /// <summary>
        /// Tries to execute the <see cref="IOfflineFileReplacer"/> so that files that are currently in use by this application
        /// can be replaced. If this method returns true, it is vital that this application terminates itself so that the
        /// files can be replaced.
        /// </summary>
        /// <returns>True if the <see cref="IOfflineFileReplacer"/> has been executed and this application needs to reset; otherwise
        /// false.</returns>
        public bool TryExecuteOfflineFileReplacer()
        {
            // Check that the update process has completed (no use to reset while in the middle of updating)
            if (State != UpdateClientState.Completed)
                return false;

            // Check that there are files that need to be replaced
            if (_fileReplacer.JobCount <= 0)
                return false;

            // Execute
            return OfflineFileReplacerHelper.TryExecute(_fileReplacer.FilePath);
        }

        /// <summary>
        /// Tries to set the client's version to the live version. That is, sets the update as being completed successfully.
        /// </summary>
        void TrySetClientVersionToLive()
        {
            // Do not set if we have files that need to be copied
            if (_fileReplacer.JobCount > 0)
                return;

            // Do not update version if there were errors
            if (HasErrors)
                return;

            if (!_liveVersion.HasValue)
            {
                Debug.Fail("Why is the LiveVersion not set?");
                return;
            }

            // Update the version file
            File.WriteAllText(Settings.VersionFilePath, _liveVersion.Value.ToString());
        }
    }
}