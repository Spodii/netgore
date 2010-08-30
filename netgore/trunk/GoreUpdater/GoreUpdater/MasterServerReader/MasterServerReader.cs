using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GoreUpdater
{
    /// <summary>
    /// Reads the information from the master server(s).
    /// </summary>
    public class MasterServerReader : IMasterServerReader
    {
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
            get { return "current.sourcelist"; }
        }

        /// <summary>
        /// Gets the relative path on the master server to the file containing the latest master servers.
        /// </summary>
        public static string CurrentMasterServersFilePath
        {
            get { return "current.masterlist"; }
        }

        /// <summary>
        /// Gets the relative path on the master server to the file containing the current version.
        /// </summary>
        public static string CurrentVersionFilePath
        {
            get { return "current.version"; }
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

            var info = new MasterServerReadInfo();

            // Create the master server readers from the file
            IEnumerable<DownloadSourceDescriptor> descriptors;
            lock (_ioSync)
            {
                descriptors = DownloadSourceDescriptor.FromDescriptorFile(LocalMasterServerListPath);
            }

            // Ensure we have descriptors
            if (descriptors.Count() == 0)
            {
                info.AppendError("No DownloadSourceDescriptors could be found.");
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
                    info.AppendError(string.Format("Failed to instantiate DownloadSourceDescriptor `{0}`: {1}", desc, ex));
                }
            }

            // Ensure we have at least one source
            if (sources.Count == 0)
            {
                info.AppendError("All DownloadSourceDescriptors failed to be instantiated - no servers available to use.");
                callback(this, info, userState);
                return;
            }

            // Start the downloader
            using (var msd = new MasterServerDownloader(info, sources) { DisposeSources = true })
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
                        Debug.Fail(ex.ToString());
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
                        Debug.Fail(ex.ToString());
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
        /// Begins reading the master server information.
        /// </summary>
        /// <param name="callback">The <see cref="MasterServerReaderReadCallback"/> to invoke with the results when complete.</param>
        /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
        /// from the method call.</param>
        public void BeginRead(MasterServerReaderReadCallback callback, object userState)
        {
            // Create the worker
            var t = new Thread(ReadThreadWorker) { IsBackground = true };

            try
            {
                t.Name = "MasterServerReader read thread.";
            }
            catch (InvalidOperationException)
            {
            }

            // Start it
            t.Start(new ThreadWorkerArgs(callback, userState));
        }

        #endregion

        class MasterServerDownloader : IDisposable
        {
            readonly MasterServerReadInfo _masterReadInfo;
            readonly List<IDownloadSource> _sources;
            readonly object _sourcesSync = new object();
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
            public MasterServerDownloader(MasterServerReadInfo masterReadInfo, IEnumerable<IDownloadSource> sources)
            {
                _masterReadInfo = masterReadInfo;
                _sources = new List<IDownloadSource>(sources);
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
            /// <param name="timeout">How many milliseconds to wait before giving up.</param>
            /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
            public void Execute(int timeout = 10000)
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("this");

                // Set the event hooks on the sources and start the downloads on each source
                lock (_sourcesSync)
                {
                    foreach (var src in _sources)
                    {
                        ExecuteSource(src);
                    }
                }

                var startTime = Environment.TickCount;

                // Wait until all sources finish
                while (!CheckIfComplete())
                {
                    Thread.Sleep(150);

                    // Check if its time to timeout
                    if (Environment.TickCount - startTime > timeout)
                        break;
                }
            }

            void ExecuteSource(IDownloadSource source)
            {
                source.DownloadFinished += source_DownloadFinished;
                source.DownloadFailed += source_DownloadFailed;

                // Master servers file
                var tempFile = Path.GetTempFileName();
                Interlocked.Increment(ref _numBusyMasterServersFile);
                if (!source.Download(CurrentMasterServersFilePath, tempFile, null))
                {
                    Interlocked.Decrement(ref _numBusyMasterServersFile);
                    PathHelper.SafeDeleteTempFile(tempFile);
                }

                // Version file
                tempFile = Path.GetTempFileName();
                Interlocked.Increment(ref _numBusyVersionFile);
                if (!source.Download(CurrentVersionFilePath, tempFile, null))
                {
                    Interlocked.Decrement(ref _numBusyVersionFile);
                    PathHelper.SafeDeleteTempFile(tempFile);
                }

                // Download sources file
                tempFile = Path.GetTempFileName();
                Interlocked.Increment(ref _numBusyDownloadSourcesFile);
                if (!source.Download(CurrentDownloadSourcesFilePath, tempFile, null))
                {
                    Interlocked.Decrement(ref _numBusyDownloadSourcesFile);
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
                    _masterReadInfo.AppendError("Failed to read temp file: " + ex);
                }

                return null;
            }

            void source_DownloadFailed(IDownloadSource sender, string remoteFile, string localFilePath)
            {
                // Handle based on what remote file it was
                if (remoteFile == CurrentVersionFilePath)
                    Interlocked.Decrement(ref _numBusyVersionFile);
                else if (remoteFile == CurrentMasterServersFilePath)
                    Interlocked.Decrement(ref _numBusyMasterServersFile);
                else if (remoteFile == CurrentDownloadSourcesFilePath)
                    Interlocked.Decrement(ref _numBusyDownloadSourcesFile);
                else
                {
                    Debug.Fail("Unexpected remote file downloaded: " + remoteFile);
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
                    try
                    {
                        var txt = TryReadAllText(localFilePath);
                        if (txt != null)
                        {
                            // Try to parse and add the version
                            int version;
                            if (!int.TryParse(txt, out version))
                                _masterReadInfo.AppendError(
                                    string.Format("Failed to parse version file to integer. Contents: `{0}`", txt));
                            else
                                _masterReadInfo.AddVersion(version);
                        }
                    }
                    catch (Exception ex)
                    {
                        _masterReadInfo.AppendError("Unexpected error while handling version file: " + ex);
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
                                        Debug.Fail(ex.ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _masterReadInfo.AppendError("Unexpected error while handling master servers file: " + ex);
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
                        _masterReadInfo.AppendError("Unexpected error while handling download sources file: " + ex);
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _numBusyDownloadSourcesFile);
                    }
                }
                else
                {
                    Debug.Fail("Unexpected remote file downloaded: " + remoteFile);
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

        class ThreadWorkerArgs
        {
            readonly MasterServerReaderReadCallback _callback;
            readonly object _userState;

            /// <summary>
            /// Initializes a new instance of the <see cref="ThreadWorkerArgs"/> class.
            /// </summary>
            /// <param name="callback">The <see cref="MasterServerReaderReadCallback"/> to invoke with the results when complete.</param>
            /// <param name="userState">An optional state object passed by the caller to supply information to the callback method
            /// from the method call.</param>
            public ThreadWorkerArgs(MasterServerReaderReadCallback callback, object userState)
            {
                _callback = callback;
                _userState = userState;
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
        }
    }
}