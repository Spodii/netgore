using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace GoreUpdater
{
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
        /// The <see cref="VersionFileList"/>. Can, and often will, be null. Only set when grabbing the <see cref="VersionFileList"/>
        /// from the server (that is, actually making an update).
        /// </summary>
        VersionFileList _versionFileList;

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

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
                        const string errmsg = "CheckIfDownloadManagerComplete() called, but items still in the download queue?";
                        if (log.IsWarnEnabled)
                            log.Warn(errmsg);
                        Debug.Fail(errmsg);
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
                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg, _dm, ex);
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

            // Clean up
            Cleanup();

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
        /// Runs the clean-up routines.
        /// </summary>
        void Cleanup()
        {
            State = UpdateClientState.CleaningUp;

            // Delete files from the update process
            try
            {
                if (_versionFileList != null)
                {
                    if (Directory.Exists(Settings.TargetPath))
                    {
                        var ffc = FileFilterHelper.CreateCollection(_versionFileList.Filters);
                        if (ffc.Count > 0)
                        {
                            var pathTrimLen = Settings.TargetPath.Length;
                            if (!Settings.TargetPath.EndsWith(Path.DirectorySeparatorChar.ToString()) &&
                                !Settings.TargetPath.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                                pathTrimLen++;

                            var files = Directory.GetFiles(Settings.TargetPath);
                            foreach (var f in files)
                            {
                                var relativePath = f.Substring(pathTrimLen);

                                try
                                {
                                    if (_versionFileList.ContainsFile(relativePath))
                                        continue;

                                    if (ffc.IsMatch("\\" + relativePath))
                                    {
                                        if (log.IsDebugEnabled)
                                            log.DebugFormat("Skipping deleting outdated client file `{0}` - matches one or more delete skip filters.", f);
                                        continue;
                                    }

                                    if (log.IsInfoEnabled)
                                        log.InfoFormat("Deleting outdated client file: {0}", f);

                                    File.Delete(f);
                                }
                                catch (Exception ex)
                                {
                                    const string errmsg = "Unexpected error while checking if file `{0}` should be deleted. Exception: {1}";
                                    if (log.IsErrorEnabled)
                                        log.ErrorFormat(errmsg, f, ex);
                                    Debug.Fail(string.Format(errmsg, f, ex));
                                }
                            }
                        }
                    }
                }
                else
                {
                    const string errmsg = "_versionFileList was null, but wasn't expected to be.";
                    if (log.IsErrorEnabled)
                        log.Error(errmsg);
                    Debug.Fail(errmsg);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to delete old files. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }

            // Delete temp files
            try
            {
                if (Directory.Exists(Settings.TempPath))
                {
                    var tempFiles = Directory.GetFiles(Settings.TempPath);

                    // Delete each file individually first, even though Directory.Delete can do this, just to make sure that
                    // we delete as much as we possibly can if there are errors
                    foreach (var f in tempFiles)
                    {
                        try
                        {
                            if (log.IsDebugEnabled)
                                log.DebugFormat("Deleting temp file `{0}`.", f);
                            File.Delete(f);
                        }
                        catch (Exception ex)
                        {
                            const string errmsg = "Failed to delete temporary file `{0}`. Exception: {1}";
                            Debug.Fail(string.Format(errmsg, f, ex));
                        }
                    }

                    // Delete the directory
                    if (log.IsDebugEnabled)
                        log.DebugFormat("Deleting directory `{0}`.", Settings.TempPath);
                    Directory.Delete(Settings.TempPath, true);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to delete temp files from path `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, Settings.TempPath, ex);
                Debug.Fail(string.Format(errmsg, Settings.TempPath, ex));
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

            if (log.IsWarnEnabled)
                log.WarnFormat("Failed to download remote file `{0}` using `{1}`.", remoteFile, sender);

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

            if (log.IsInfoEnabled)
                log.InfoFormat("Successfully downloaded remote file `{0}` to `{1}` using `{2}`.", remoteFile, localFilePath, sender);

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

            if (log.IsInfoEnabled)
                log.InfoFormat("Failed to move file `{0}` to `{1}` using `{2}`; adding job to IOfflineFileReplacer `{3}`.", localFilePath, 
                    targetFilePath, sender, _fileReplacer);

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

            if (log.IsInfoEnabled)
                log.InfoFormat("DownloadManager `{0}` returned Finished.", sender);

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
                        if (log.IsDebugEnabled)
                            log.DebugFormat("Update check on file `{0}`: File needs update - size mismatch (current: {1}, expected: {2}).",
                                updateFileInfo.FilePath, localFileInfo.Length, updateFileInfo.Size);

                        ret.Add(updateFileInfo.FilePath);
                        continue;
                    }

                    // File exists and is of the correct size, so compare the hash of the local file to the expected hash
                    var localFileHash = Hasher.GetFileHash(localFilePath);
                    if (!StringComparer.Ordinal.Equals(localFileHash, updateFileInfo.Hash))
                    {
                        if (log.IsDebugEnabled)
                            log.DebugFormat("Update check on file `{0}`: File needs update - hash mismatch (current: {1}, expected: {2}).",
                                updateFileInfo.FilePath, localFileHash, updateFileInfo.Hash);

                        ret.Add(updateFileInfo.FilePath);
                        continue;
                    }
                }
                catch (IOException ex)
                {
                    const string errmsg = "Failed to analyze file `{0}` to see if it needs to be updated. Will assume update is required. Exception: {1}";
                    
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, updateFileInfo.FilePath, ex);

                    Debug.Fail(string.Format(errmsg, updateFileInfo.FilePath, ex));

                    // On an IOException, assume the file needs to be updated
                    ret.Add(updateFileInfo.FilePath);
                    continue;
                }

                if (log.IsDebugEnabled)
                    log.DebugFormat("Update check on file `{0}`: file is up-to-date.", updateFileInfo.FilePath);
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Found `{0}` files needing to be updated.", ret.Count);

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

                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, info.Version);

                if (MasterServerReaderError != null)
                    MasterServerReaderError(this, string.Format(errmsg, info.Version));

                HasErrors = true;
                return;
            }

            try
            {
                _versionFileList = VersionFileList.CreateFromString(info.VersionFileListText);
            }
            catch (Exception ex)
            {
                const string errmsg =
                    "Could not get a valid VersionFileList file from the master servers for version `{0}`. Exception: {1}";

                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, info.Version, ex);

                if (MasterServerReaderError != null)
                    MasterServerReaderError(this, string.Format(errmsg, info.Version, ex));

                HasErrors = true;
                return;
            }

            // Find the files to update
            var toUpdate = FindFilesToUpdate(_versionFileList);

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
            if (log.IsInfoEnabled)
                log.InfoFormat("Trying to execute IOfflineFileReplacer `{0}`.", _fileReplacer);

            // Check that the update process has completed (no use to reset while in the middle of updating)
            if (State != UpdateClientState.Completed)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Could not execute IOfflineFileReplacer `{0}` - state is `{1}` (expected: {2}).",
                        _fileReplacer, State, UpdateClientState.Completed);

                return false;
            }

            // Check that there are files that need to be replaced
            if (_fileReplacer.JobCount <= 0)
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Could not execute IOfflineFileReplacer `{0}` - file replace queue is empty.", _fileReplacer);

                return false;
            }

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