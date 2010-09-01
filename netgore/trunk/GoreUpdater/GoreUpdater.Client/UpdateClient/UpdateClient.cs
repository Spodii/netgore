﻿using System;
using System.Diagnostics;
using System.Linq;

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
        /// Notifies listeners when the live version has been found.
        /// </summary>
        public event UpdateClientEventHandler LiveVersionFound;

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
            if (_dm.QueueCount > 0)
                return;

            // TODO: Check if complete
        }

        /// <summary>
        /// The callback method for the <see cref="IMasterServerReader.BeginRead"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IMasterServerReader"/> this event came from.</param>
        /// <param name="info">The information from the master server(s).</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        void MSRCallback(IMasterServerReader sender, IMasterServerReadInfo info, object userState)
        {
            State = UpdateClientState.DoneReadingMasterServers;

            // Set the found live version
            LiveVersion = info.Version;

            // TODO: Be able to abort here if the live version == current version

            // Create the DownloadManager
            _dm = new DownloadManager(Settings.TargetPath, Settings.TempPath, info.Version);
            _dm.DownloadFinished += _dm_DownloadFinished;
            _dm.FileMoveFailed += _dm_FileMoveFailed;
            _dm.DownloadFailed += _dm_DownloadFailed;
            _dm.Finished += _dm_Finished;

            State = UpdateClientState.UpdatingFiles;

            // Add the sources to the DownloadManager
            var sources = info.DownloadSources.Select(x => x.Instantiate());
            _dm.AddSources(sources);

            // Get the files to update
            // TODO: Only update files needing to be updated
            _dm.Enqueue(new string[] { "11.png", "12.png", "13.png" });
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

            State = UpdateClientState.Initializing;

            // Create the objects
            _msr = new MasterServerReader(Settings.LocalFileServerPath, Settings.LocalMasterServerPath);
            _fileReplacer = Settings.CreateOfflineFileReplacer();

            State = UpdateClientState.ReadingMasterServers;

            // Start grabbing from the master server
            _msr.BeginRead(MSRCallback, this);
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

        void _dm_DownloadFailed(IDownloadManager sender, string remoteFile)
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

            CheckIfDownloadManagerComplete();
        }

        void _dm_DownloadFinished(IDownloadManager sender, string remoteFile, string localFilePath)
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

            CheckIfDownloadManagerComplete();
        }

        void _dm_FileMoveFailed(IDownloadManager sender, string remoteFile, string localFilePath, string targetFilePath)
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

            CheckIfDownloadManagerComplete();
        }

        void _dm_Finished(IDownloadManager sender)
        {
            Debug.Assert(sender == _dm, "Why did we get an event from a different IDownloadManager?");

            CheckIfDownloadManagerComplete();
        }
    }
}