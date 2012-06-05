using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// Reads the information from the master server(s).
    /// </summary>
    public class MasterServerReader : IMasterServerReader
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly object _ioSync = new object();

        readonly string _localDownloadSourceListPath;
        readonly string _localMasterServerListPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterServerReader"/> class.
        /// </summary>
        /// <param name="localDownloadSourceListPath">The local download source list path.</param>
        /// <param name="localMasterServerListPath">The local master server list path.</param>
        public MasterServerReader(string localDownloadSourceListPath, string localMasterServerListPath)
        {
            _localDownloadSourceListPath = localDownloadSourceListPath;
            _localMasterServerListPath = localMasterServerListPath;
        }

        /// <summary>
        /// Gets the relative path on the master server to the file containing the latest download sources.
        /// </summary>
        public static string CurrentDownloadSourcesFilePath
        {
            get { return "file_servers"; }
        }

        /// <summary>
        /// Gets the relative path on the master server to the file containing the latest master servers.
        /// </summary>
        public static string CurrentMasterServersFilePath
        {
            get { return "master_servers"; }
        }

        /// <summary>
        /// Gets the relative path on the master server to the file containing the current version.
        /// </summary>
        public static string CurrentVersionFilePath
        {
            get { return "live_version"; }
        }

        /// <summary>
        /// The worker method for the reader threads.
        /// </summary>
        /// <param name="o">The arguments.</param>
        void ReadThreadWorker(object o)
        {
            var stateObj = (ThreadWorkerArgs)o;

            var callback = stateObj.Callback;
            var userState = stateObj.UserState;
            var readVersion = stateObj.Version;

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Running MasterServerReader worker. UserState: {0}. ReadVersion: {1}. Callback: {2}", userState,
                    readVersion.HasValue ? readVersion.Value.ToString() : "[NULL]", callback);
            }

            var info = new MasterServerReadInfo();
            if (readVersion.HasValue)
                info.AddVersion(readVersion.Value);

            // Create the master server readers from the file
            IEnumerable<DownloadSourceDescriptor> descriptors;
            lock (_ioSync)
            {
                descriptors = DownloadSourceDescriptor.FromDescriptorFile(LocalMasterServerListPath);
            }

            // Ensure we have descriptors
            if (descriptors.Count() == 0)
            {
                const string errmsg = "No DownloadSourceDescriptors could be found.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                info.AppendError(errmsg);
                callback(this, info, userState);
                return;
            }

            // Create the source instances
            var sources = new List<IDownloadSource>();
            foreach (var desc in descriptors)
            {
                try
                {
                    var src = desc.Instantiate();
                    sources.Add(src);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to instantiate DownloadSourceDescriptor `{0}`: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, desc, ex);
                    info.AppendError(string.Format(errmsg, desc, ex));
                }
            }

            // Ensure we have at least one source
            if (sources.Count == 0)
            {
                const string errmsg = "All DownloadSourceDescriptors failed to be instantiated - no servers available to use.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg);
                info.AppendError(errmsg);
                callback(this, info, userState);
                return;
            }

            // Start the downloader
            using (var msd = new MasterServerDownloader(info, sources, readVersion) { DisposeSources = true })
            {
                // This will block until its complete
                msd.Execute();
            }

            // Save the new information to the files
            WriteMasterServersFile(info, LocalMasterServerListPath);
            WriteDownloadSourcesFile(info, LocalDownloadSourceListPath);

            // Invoke the callback
            if (callback != null)
                callback(this, info, userState);
        }

        /// <summary>
        /// Writes the download sources file.
        /// </summary>
        /// <param name="info">The <see cref="MasterServerReadInfo"/>.</param>
        /// <param name="filePath">The file path to write to.</param>
        static void WriteDownloadSourcesFile(MasterServerReadInfo info, string filePath)
        {
            // If we have nothing to write, just keep the old file instead of overwriting it with an empty file. Even though
            // the entries are probably outdated, its better to have outdated entries than nothing at all.
            if (info.DownloadSources.Count() == 0)
                return;

            var sb = new StringBuilder();

            foreach (var desc in info.DownloadSources)
            {
                sb.AppendLine(desc.GetDescriptorString());
            }

            lock (_ioSync)
            {
                // We will make 5 attempts to write the file before completely giving up
                for (var i = 0; i < 5; i++)
                {
                    var tmpFile = Path.GetTempFileName();

                    try
                    {
                        // Write the text to the file then copy it over to the destination
                        File.WriteAllText(tmpFile, sb.ToString());
                        File.Copy(tmpFile, filePath, true);

                        // Success!
                        break;
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to write download sources file `{0}`. Exception: {1}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, filePath, ex);
                        Debug.Fail(string.Format(errmsg, filePath, ex));
                    }
                    finally
                    {
                        // Delete the temporary file
                        PathHelper.SafeDeleteTempFile(tmpFile);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the master servers file.
        /// </summary>
        /// <param name="info">The <see cref="MasterServerReadInfo"/>.</param>
        /// <param name="filePath">The file path to write to.</param>
        static void WriteMasterServersFile(MasterServerReadInfo info, string filePath)
        {
            // If we have nothing to write, just keep the old file instead of overwriting it with an empty file. Even though
            // the entries are probably outdated, its better to have outdated entries than nothing at all.
            if (info.MasterServers.Count() == 0)
                return;

            var sb = new StringBuilder();

            foreach (var desc in info.MasterServers)
            {
                sb.AppendLine(desc.GetDescriptorString());
            }

            lock (_ioSync)
            {
                // We will make 5 attempts to write the file before completely giving up
                for (var i = 0; i < 5; i++)
                {
                    var tmpFile = Path.GetTempFileName();

                    try
                    {
                        // Write the text to the file then copy it over to the destination
                        File.WriteAllText(tmpFile, sb.ToString());
                        File.Copy(tmpFile, filePath, true);

                        // Success!
                        break;
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to write master servers file `{0}`. Exception: {1}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, filePath, ex);
                        Debug.Fail(string.Format(errmsg, filePath, ex));
                    }
                    finally
                    {
                        // Delete the temporary file
                        PathHelper.SafeDeleteTempFile(tmpFile);
                    }
                }
            }
        }

        #region IMasterServerReader Members

        /// <summary>
        /// Gets the path to the local file containing the list of download sources. This file will be updated automatically
        /// as new download sources are discovered by this <see cref="IMasterServerReader"/>.
        /// </summary>
        public string LocalDownloadSourceListPath
        {
            get { return _localDownloadSourceListPath; }
        }

        /// <summary>
        /// Gets the path to the local file containing the list of master servers. This file will be updated automatically
        /// as new master servers are discovered by this <see cref="IMasterServerReader"/>.
        /// </summary>
        public string LocalMasterServerListPath
        {
            get { return _localMasterServerListPath; }
        }

        /// <summary>
        /// Begins reading the version from the master server(s).
        /// </summary>
        /// <param name="callback">The <see cref="MasterServerReaderReadCallback"/> to invoke with the results when complete.</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        public void BeginReadVersion(MasterServerReaderReadCallback callback, object userState)
        {
            // Create the worker
            var t = new Thread(ReadThreadWorker) { IsBackground = true };

            try
            {
                t.Name = "MasterServerReader BeginReadVersion thread.";
            }
            catch (InvalidOperationException)
            {
            }

            // Start it
            var args = new ThreadWorkerArgs(callback, userState, null);
            t.Start(args);
        }

        /// <summary>
        /// Begins reading the version file list from the master server(s).
        /// </summary>
        /// <param name="callback">The <see cref="MasterServerReaderReadCallback"/> to invoke with the results when complete.</param>
        /// <param name="version">The version to get the <see cref="VersionFileList"/> for.</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        public void BeginReadVersionFileList(MasterServerReaderReadCallback callback, int version, object userState)
        {
            // Create the worker
            var t = new Thread(ReadThreadWorker) { IsBackground = true };

            try
            {
                t.Name = "MasterServerReader BeginReadVersionFileList thread.";
            }
            catch (InvalidOperationException)
            {
            }

            // Start it
            var args = new ThreadWorkerArgs(callback, userState, version);
            t.Start(args);
        }

        #endregion

        class MasterServerDownloader : IDisposable
        {
            /// <summary>
            /// The default value for "giveUpTime" for the <see cref="Execute"/> parameter
            /// for when reading the version file.
            /// </summary>
            const int _defaultGiveUpTimeVersion = 10000;

            /// <summary>
            /// The default value for "giveUpTime" for the <see cref="Execute"/> parameter
            /// for when reading the <see cref="VersionFileList"/> file.
            /// </summary>
            const int _defaultGiveUpTimeVersionFileList = 30000;

            /// <summary>
            /// The default value for "stallTime" for the <see cref="Execute"/> parameter
            /// for when reading the version file.
            /// </summary>
            const int _defaultStallTimeVersion = 3000;

            /// <summary>
            /// The default value for "stallTime" for the <see cref="Execute"/> parameter
            /// for when reading the <see cref="VersionFileList"/> file.
            /// </summary>
            const int _defaultStallTimeVersionFileList = 6000;

            readonly MasterServerReadInfo _masterReadInfo;
            readonly int? _readVersion;

            /// <summary>
            /// The remote path of the file to download.
            /// </summary>
            readonly string _remoteFileToDownload;

            readonly List<IDownloadSource> _sources;
            readonly object _sourcesSync = new object();

            /// <summary>
            /// If a version has been read from ANY of the master servers. This allows to avoid having to wait on very slow
            /// master servers when we already have the version.
            /// </summary>
            volatile bool _hasReadVersion = false;

            bool _isDisposed = false;

            /// <summary>
            /// The number of downloaders busy grabbing the download sources file.
            /// </summary>
            int _numBusyDownloadSourcesFile = 0;

            /// <summary>
            /// The number of downloaders busy grabbing the master servers file.
            /// </summary>
            int _numBusyMasterServersFile = 0;

            /// <summary>
            /// The number of downloaders busy grabbing the version file.
            /// </summary>
            int _numBusyVersionFile = 0;

            /// <summary>
            /// Initializes a new instance of the <see cref="MasterServerDownloader"/> class.
            /// </summary>
            /// <param name="masterReadInfo">The <see cref="MasterServerReadInfo"/> to add to.</param>
            /// <param name="sources">The <see cref="IDownloadSource"/> to use to download.</param>
            /// <param name="readVersion">The version of the <see cref="VersionFileList"/> to read, or null if reading
            /// the current version number.</param>
            public MasterServerDownloader(MasterServerReadInfo masterReadInfo, IEnumerable<IDownloadSource> sources,
                                          int? readVersion)
            {
                _readVersion = readVersion;
                _masterReadInfo = masterReadInfo;
                _sources = new List<IDownloadSource>(sources);

                // Cache the remote file path
                if (_readVersion.HasValue)
                    _remoteFileToDownload = PathHelper.GetVersionString(_readVersion.Value) + ".txt";
                else
                    _remoteFileToDownload = CurrentVersionFilePath;
            }

            /// <summary>
            /// Gets or sets if the sources passed in the constructor are also disposed when this object is disposed.
            /// </summary>
            public bool DisposeSources { get; set; }

            /// <summary>
            /// Gets if this object has been disposed.
            /// </summary>
            public bool IsDisposed
            {
                get { return _isDisposed; }
            }

            /// <summary>
            /// Checks if we have completed all downloads.
            /// </summary>
            /// <returns>If true, we have completed.</returns>
            bool CheckIfComplete()
            {
                if (_numBusyDownloadSourcesFile != 0 || _numBusyMasterServersFile != 0 || _numBusyVersionFile != 0)
                    return false;

                return true;
            }

            /// <summary>
            /// Executes the downloader and waits until all master servers finish to return the <see cref="IMasterServerReadInfo"/>.
            /// </summary>
            /// <param name="giveUpTime">How many milliseconds to wait before giving up completely even when nothing has been read.
            /// If less than 0, the default value will be used.</param>
            /// <param name="stallTime">How many milliseconds to wait for the results from other servers after the first server has been read.
            /// Although the results are only needed from one server, its best to get it from multiple servers to ensure accuracy. However,
            /// you do not want to wait too long for very slow servers.
            /// If less than 0, the default value will be used.</param>
            /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
            public void Execute(int giveUpTime = -1, int stallTime = -1)
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("this");

                _hasReadVersion = false;

                // Set the needed default values
                if (giveUpTime < 0)
                {
                    if (_readVersion.HasValue)
                        giveUpTime = _defaultGiveUpTimeVersionFileList;
                    else
                        giveUpTime = _defaultGiveUpTimeVersion;
                }

                if (stallTime < 0)
                {
                    if (_readVersion.HasValue)
                        stallTime = _defaultStallTimeVersionFileList;
                    else
                        stallTime = _defaultStallTimeVersion;
                }

                // Set the event hooks on the sources and start the downloads on each source
                lock (_sourcesSync)
                {
                    foreach (var src in _sources)
                    {
                        ExecuteSource(src);
                    }
                }

                var startTime = Environment.TickCount;
                var stallEndTime = int.MaxValue;

                // Wait until all sources finish
                while (!CheckIfComplete())
                {
                    Thread.Sleep(150);

                    var now = Environment.TickCount;

                    // Check to set the stallEndTime
                    if (stallEndTime == int.MaxValue && _hasReadVersion)
                        stallEndTime = now + stallTime;

                    // Check if we have exceeded the stall time
                    if (now > stallEndTime)
                        break;

                    // Check if its time to timeout
                    if (now - startTime > giveUpTime)
                        break;
                }
            }

            /// <summary>
            /// Executes a <see cref="IDownloadSource"/>, which will read the desired files.
            /// </summary>
            /// <param name="source">The <see cref="IDownloadSource"/> to use.</param>
            void ExecuteSource(IDownloadSource source)
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("Executing download source: {0}", source);

                source.DownloadFinished += source_DownloadFinished;
                source.DownloadFailed += source_DownloadFailed;

                // Master servers file (master server list)
                var tempFile = Path.GetTempFileName();
                Interlocked.Increment(ref _numBusyMasterServersFile);
                if (!source.Download(CurrentMasterServersFilePath, tempFile, null))
                {
                    Interlocked.Decrement(ref _numBusyMasterServersFile);
                    PathHelper.SafeDeleteTempFile(tempFile);
                }

                // Download sources file (file server list)
                tempFile = Path.GetTempFileName();
                Interlocked.Increment(ref _numBusyDownloadSourcesFile);
                if (!source.Download(CurrentDownloadSourcesFilePath, tempFile, null))
                {
                    Interlocked.Decrement(ref _numBusyDownloadSourcesFile);
                    PathHelper.SafeDeleteTempFile(tempFile);
                }

                // Version file (contains the current version number)
                //      -or-
                // VersionFileList file (contains the listing of all the files for the version)
                tempFile = Path.GetTempFileName();
                Interlocked.Increment(ref _numBusyVersionFile);
                if (!source.Download(_remoteFileToDownload, tempFile, null))
                {
                    Interlocked.Decrement(ref _numBusyVersionFile);
                    PathHelper.SafeDeleteTempFile(tempFile);
                }
            }

            /// <summary>
            /// Tries to read the text from a file.
            /// </summary>
            /// <param name="filePath">The path to the file to read.</param>
            /// <returns>The text from the <paramref name="filePath"/>, or null if it failed to read.</returns>
            string TryReadAllText(string filePath)
            {
                try
                {
                    return File.ReadAllText(filePath);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to read temp file `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, filePath, ex);
                    _masterReadInfo.AppendError(string.Format(errmsg, filePath, ex));
                }

                return null;
            }

            void source_DownloadFailed(IDownloadSource sender, string remoteFile, string localFilePath)
            {
                // Handle based on what remote file it was
                if (remoteFile == CurrentVersionFilePath)
                    Interlocked.Decrement(ref _numBusyVersionFile);
                else if (_readVersion.HasValue &&
                         StringComparer.OrdinalIgnoreCase.Equals(PathHelper.GetVersionString(_readVersion.Value) + ".txt",
                             remoteFile))
                    Interlocked.Decrement(ref _numBusyVersionFile);
                else if (remoteFile == CurrentMasterServersFilePath)
                    Interlocked.Decrement(ref _numBusyMasterServersFile);
                else if (remoteFile == CurrentDownloadSourcesFilePath)
                    Interlocked.Decrement(ref _numBusyDownloadSourcesFile);
                else
                {
                    const string errmsg = "Unexpected remote file `{0}` downloaded by `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, remoteFile, sender);
                    Debug.Fail(string.Format(errmsg, remoteFile, sender));
                    return;
                }

                // Delete the temp file
                PathHelper.SafeDeleteTempFile(localFilePath);

                CheckIfComplete();
            }

            void source_DownloadFinished(IDownloadSource sender, string remoteFile, string localFilePath)
            {
                // Handle based on what remote file it was
                if (remoteFile == CurrentVersionFilePath)
                {
                    // Version file
                    try
                    {
                        var txt = TryReadAllText(localFilePath);
                        if (txt != null)
                        {
                            // Try to parse and add the version
                            int version;
                            if (!int.TryParse(txt, out version))
                            {
                                const string errmsg =
                                    "Failed to parse version file to integer (remote path: {0}, local path: {1}). Contents: `{2}`";
                                if (log.IsErrorEnabled)
                                    log.ErrorFormat(errmsg, remoteFile, localFilePath, txt);
                                _masterReadInfo.AppendError(string.Format(errmsg, remoteFile, localFilePath, txt));
                            }
                            else
                            {
                                _masterReadInfo.AddVersion(version);
                                _hasReadVersion = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        const string errmsg =
                            "Unexpected error while handling version file (remote path: {0}, local path: {1}). Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, remoteFile, localFilePath, ex);
                        _masterReadInfo.AppendError(string.Format(errmsg, remoteFile, localFilePath, ex));
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _numBusyVersionFile);
                    }
                }
                else if (_readVersion.HasValue &&
                         StringComparer.OrdinalIgnoreCase.Equals(PathHelper.GetVersionString(_readVersion.Value) + ".txt",
                             remoteFile))
                {
                    // VersionFileList file
                    try
                    {
                        var txt = TryReadAllText(localFilePath);
                        _masterReadInfo.AddVersionFileListText(txt);
                    }
                    catch (Exception ex)
                    {
                        const string errmsg =
                            "Unexpected error while handling version file (remote path: {0}, local path: {1}). Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, remoteFile, localFilePath, ex);
                        _masterReadInfo.AppendError(string.Format(errmsg, remoteFile, localFilePath, ex));
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _numBusyVersionFile);
                    }
                }
                else if (remoteFile == CurrentMasterServersFilePath)
                {
                    try
                    {
                        // Add the list of master servers
                        var descriptors = DownloadSourceDescriptor.FromDescriptorFile(localFilePath);
                        foreach (var desc in descriptors)
                        {
                            _masterReadInfo.AddMasterServer(desc);
                        }

                        // If any of the added master servers are not in our list, grab from it, too
                        lock (_sourcesSync)
                        {
                            foreach (var desc in descriptors)
                            {
                                var d = desc;
                                if (!_sources.Any(x => x.IsIdenticalTo(d)))
                                {
                                    // None of our existing sources match the descriptor, so add it to our list and start
                                    // grabbing from that new source
                                    try
                                    {
                                        var newSource = desc.Instantiate();
                                        _sources.Add(newSource);
                                        ExecuteSource(newSource);
                                    }
                                    catch (Exception ex)
                                    {
                                        const string errmsg =
                                            "Failed to instantiate and/or execute downoaded master server using DownloadSourceDescriptor `{0}`. Exception: {1}";
                                        if (log.IsWarnEnabled)
                                            log.WarnFormat(errmsg, desc, ex);
                                        Debug.Fail(string.Format(errmsg, desc, ex));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        const string errmsg =
                            "Unexpected error while handling master servers file (remote path: {0}, local path: {1}). Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, remoteFile, localFilePath, ex);
                        _masterReadInfo.AppendError(string.Format(errmsg, remoteFile, localFilePath, ex));
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _numBusyMasterServersFile);
                    }
                }
                else if (remoteFile == CurrentDownloadSourcesFilePath)
                {
                    try
                    {
                        // Add the list of download sources
                        var descriptors = DownloadSourceDescriptor.FromDescriptorFile(localFilePath);
                        foreach (var desc in descriptors)
                        {
                            _masterReadInfo.AddDownloadSource(desc);
                        }
                    }
                    catch (Exception ex)
                    {
                        const string errmsg =
                            "Unexpected error while handling download sources file (remote path: {0}, local path: {1}). Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, remoteFile, localFilePath, ex);
                        _masterReadInfo.AppendError(string.Format(errmsg, remoteFile, localFilePath, ex));
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _numBusyDownloadSourcesFile);
                    }
                }
                else
                {
                    const string errmsg = "Unexpected remote file `{0}` downloaded by `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, remoteFile, sender);
                    Debug.Fail(string.Format(errmsg, remoteFile, sender));
                    return;
                }

                // Delete the temp file
                PathHelper.SafeDeleteTempFile(localFilePath);

                CheckIfComplete();
            }

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (IsDisposed)
                    return;

                _isDisposed = true;

                var disposeSources = DisposeSources;

                lock (_sourcesSync)
                {
                    foreach (var src in _sources)
                    {
                        // Remove our event hooks
                        src.DownloadFinished -= source_DownloadFinished;
                        src.DownloadFailed -= source_DownloadFailed;

                        // Check to dispose the source
                        if (disposeSources)
                        {
                            try
                            {
                                src.Dispose();
                            }
                            catch (Exception ex)
                            {
                                Debug.Fail(ex.ToString());
                            }
                        }
                    }

                    _sources.Clear();
                }
            }

            #endregion
        }

        /// <summary>
        /// Contains the arguments that are passed to the worker thread method.
        /// </summary>
        class ThreadWorkerArgs
        {
            readonly MasterServerReaderReadCallback _callback;
            readonly object _userState;
            readonly int? _version;

            /// <summary>
            /// Initializes a new instance of the <see cref="ThreadWorkerArgs"/> class.
            /// </summary>
            /// <param name="callback">The <see cref="MasterServerReaderReadCallback"/> to invoke with the results when complete.</param>
            /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
            /// from the method call.</param>
            /// <param name="version">When using <see cref="IMasterServerReader.BeginReadVersionFileList"/>, contains the verison
            /// to read. When null, assume using <see cref="IMasterServerReader.BeginReadVersion"/>.</param>
            public ThreadWorkerArgs(MasterServerReaderReadCallback callback, object userState, int? version)
            {
                _callback = callback;
                _userState = userState;
                _version = version;
            }

            /// <summary>
            /// Gets the callback delegate.
            /// </summary>
            public MasterServerReaderReadCallback Callback
            {
                get { return _callback; }
            }

            /// <summary>
            /// Gets the user state object.
            /// </summary>
            public object UserState
            {
                get { return _userState; }
            }

            /// <summary>
            /// Gets the version to read. When using <see cref="IMasterServerReader.BeginReadVersionFileList"/>, contains the verison
            /// to read. When null, assume using <see cref="IMasterServerReader.BeginReadVersion"/>.
            /// </summary>
            public int? Version
            {
                get { return _version; }
            }
        }
    }
}