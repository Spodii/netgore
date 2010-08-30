using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// Delegate for handling events from the <see cref="ManagerSettings"/>.
    /// </summary>
    /// <param name="sender">The <see cref="ManagerSettings"/> that the event occured on.</param>
    public delegate void ManagerSettingsEventHandler(ManagerSettings sender);

    /// <summary>
    /// Contains settings for the GoreUpdater manager.
    /// This class is not thread-safe.
    /// </summary>
    public class ManagerSettings
    {
        const string _headerDelimiter = "=";
        const string _headerFileServer = "FILESERVER";
        const string _headerLiveVersion = "LIVEVERSION";
        const string _headerMasterServer = "MASTERSERVER";
        const string _settingsFile = "settings.txt";

        static readonly ManagerSettings _instance;

        readonly string _filePath;
        readonly List<FileServerInfo> _fileServers = new List<FileServerInfo>();
        readonly object _fileServersSync = new object();
        readonly string _liveVersionFilePath;
        readonly List<MasterServerInfo> _masterServers = new List<MasterServerInfo>();
        readonly object _masterServersSync = new object();
        readonly object _saveSync = new object();
        readonly object _versionSync = new object();

        int _liveVersion = 0;

        /// <summary>
        /// Initializes the <see cref="ManagerSettings"/> class.
        /// </summary>
        static ManagerSettings()
        {
            var filePath = PathHelper.CombineDifferentPaths(Application.StartupPath, _settingsFile);
            _instance = new ManagerSettings(filePath);
            _instance.Load(filePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerSettings"/> class.
        /// </summary>
        /// <param name="filePath">The settings file path.</param>
        ManagerSettings(string filePath)
        {
            _liveVersionFilePath = PathHelper.CombineDifferentPaths(Application.StartupPath, "liveversion");
            _filePath = filePath;
        }

        /// <summary>
        /// Notifies listeners when a file server has been added or removed from the list.
        /// </summary>
        public event ManagerSettingsEventHandler FileServerListChanged;

        /// <summary>
        /// Notifies listeners when the live version has changed.
        /// </summary>
        public event ManagerSettingsEventHandler LiveVersionChanged;

        /// <summary>
        /// Notifies listeners when a master server has been added or removed from the list.
        /// </summary>
        public event ManagerSettingsEventHandler MasterServerListChanged;

        /// <summary>
        /// Notifies listeners when the next version (the version after the live version) to release has been created, or re-created.
        /// </summary>
        public event ManagerSettingsEventHandler NextVersionCreated;

        /// <summary>
        /// Gets the path to the settings file.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// Gets the list of file servers.
        /// </summary>
        public IEnumerable<FileServerInfo> FileServers
        {
            get
            {
                lock (_fileServersSync)
                {
                    return _fileServers.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets the global <see cref="ManagerSettings"/> instance.
        /// </summary>
        public static ManagerSettings Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the current live version number.
        /// </summary>
        public int LiveVersion
        {
            get
            {
                lock (_versionSync)
                {
                    return _liveVersion;
                }
            }
        }

        /// <summary>
        /// Gets the file path to the live version file. This is simply just a file that contains the live version.
        /// </summary>
        public string LiveVersionFilePath
        {
            get { return _liveVersionFilePath; }
        }

        /// <summary>
        /// Gets the list of master servers.
        /// </summary>
        public IEnumerable<MasterServerInfo> MasterServers
        {
            get
            {
                lock (_masterServersSync)
                {
                    return _masterServers.ToArray();
                }
            }
        }

        /// <summary>
        /// Adds to the file server list.
        /// </summary>
        /// <param name="server">The <see cref="FileServerInfo"/> to add.</param>
        /// <returns>True if added; otherwise false.</returns>
        public bool AddFileServer(FileServerInfo server)
        {
            lock (_fileServersSync)
            {
                if (_fileServers.Contains(server))
                    return false;

                _fileServers.Add(server);
            }

            Save();

            if (FileServerListChanged != null)
                FileServerListChanged(this);

            return true;
        }

        /// <summary>
        /// Adds to the master server list.
        /// </summary>
        /// <param name="server">The <see cref="MasterServerInfo"/> to add.</param>
        /// <returns>True if added; otherwise false.</returns>
        public bool AddMasterServer(MasterServerInfo server)
        {
            lock (_masterServersSync)
            {
                if (_masterServers.Contains(server))
                    return false;

                _masterServers.Add(server);
            }

            Save();

            if (MasterServerListChanged != null)
                MasterServerListChanged(this);

            return true;
        }

        /// <summary>
        /// Adds a settings line to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/>.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        static void AddSetting(StringBuilder sb, string key, string value)
        {
            sb.AppendLine(key + _headerDelimiter + value);
        }

        /// <summary>
        /// Gets if the next version (the version after the live version) exists.
        /// </summary>
        /// <returns>True if the next version exists; otherwise false.</returns>
        public bool DoesNextVersionExist()
        {
            var nextVersionPath = VersionHelper.GetVersionFileListPath(LiveVersion + 1);
            return File.Exists(nextVersionPath);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ManagerSettings"/> is reclaimed by garbage collection.
        /// </summary>
        ~ManagerSettings()
        {
            Save();
        }

        /// <summary>
        /// Reads a settings line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>The settings line.</returns>
        static KeyValuePair<string, string> GetSettingLine(string line)
        {
            var s = line.Split(new string[] { _headerDelimiter }, 2, StringSplitOptions.None);
            return new KeyValuePair<string, string>(s[0], s[1]);
        }

        /// <summary>
        /// Loads the settings from file.
        /// </summary>
        void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            var lines = File.ReadAllLines(filePath);
            var values = lines.Select(x => x.Trim()).Where(x => x.Length > 0).Select(GetSettingLine);

            foreach (var v in values)
            {
                switch (v.Key)
                {
                    case _headerLiveVersion:
                        int i;
                        if (!int.TryParse(v.Value, out i))
                        {
                            const string errmsg = "Encountered invalid {0} value in settings file.";
                            MessageBox.Show(string.Format(errmsg, _headerLiveVersion));
                            break;
                        }

                        _liveVersion = i;
                        UpdateLiveVersionFile();
                        break;

                    case _headerFileServer:
                        try
                        {
                            var server = FileServerInfo.Create(v.Value);
                            lock (_fileServersSync)
                            {
                                _fileServers.Add(server);
                            }
                        }
                        catch (Exception ex)
                        {
                            const string errmsg = "Encountered invalid {0} value in settings file: {1}";
                            MessageBox.Show(string.Format(errmsg, _headerFileServer, ex));
                        }
                        break;

                    case _headerMasterServer:
                        try
                        {
                            var server = MasterServerInfo.Create(v.Value);
                            lock (_masterServersSync)
                            {
                                _masterServers.Add(server);
                            }
                        }
                        catch (Exception ex)
                        {
                            const string errmsg = "Encountered invalid {0} value in settings file: {1}";
                            MessageBox.Show(string.Format(errmsg, _headerMasterServer, ex));
                        }
                        break;

                    default:
                        MessageBox.Show("Encountered unknown setting: " + v.Key);
                        break;
                }
            }
        }

        /// <summary>
        /// Removes from the file server list.
        /// </summary>
        /// <param name="server">The <see cref="FileServerInfo"/> to remove.</param>
        /// <returns>True if removed; otherwise false.</returns>
        public bool RemoveFileServer(FileServerInfo server)
        {
            bool removed;
            lock (_fileServersSync)
            {
                removed = _fileServers.Remove(server);
            }

            if (removed)
            {
                Save();

                if (FileServerListChanged != null)
                    FileServerListChanged(this);
            }

            return removed;
        }

        /// <summary>
        /// Removes from the master server list.
        /// </summary>
        /// <param name="server">The <see cref="MasterServerInfo"/> to remove.</param>
        /// <returns>True if removed; otherwise false.</returns>
        public bool RemoveMasterServer(MasterServerInfo server)
        {
            bool removed;
            lock (_masterServersSync)
            {
                removed = _masterServers.Remove(server);
            }

            if (removed)
            {
                Save();

                if (MasterServerListChanged != null)
                    MasterServerListChanged(this);
            }

            return removed;
        }

        /// <summary>
        /// Saves the settings to file. This method is invoked automatically internally when changes are detected, but can be called
        /// externally to ensure that changes are saved properly.
        /// </summary>
        public void Save()
        {
            lock (_saveSync)
            {
                var sb = new StringBuilder();

                // Write the settings
                AddSetting(sb, _headerLiveVersion, LiveVersion.ToString());

                lock (_fileServersSync)
                {
                    foreach (var s in _fileServers)
                    {
                        AddSetting(sb, _headerFileServer, s.GetCreationString());
                    }
                }

                lock (_masterServersSync)
                {
                    foreach (var s in _masterServers)
                    {
                        AddSetting(sb, _headerMasterServer, s.GetCreationString());
                    }
                }

                // Save the file
                var tmpFile = FilePath + ".tmp";
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);

                File.WriteAllText(tmpFile, sb.ToString());

                File.Copy(tmpFile, FilePath, true);

                try
                {
                    if (File.Exists(tmpFile))
                        File.Delete(tmpFile);
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Uses the given <see cref="VersionFileList"/> to set up the next version.
        /// </summary>
        /// <param name="vfl">The <see cref="VersionFileList"/> for the next version..</param>
        /// <exception cref="ArgumentException">The <paramref name="vfl"/> was invalid or the version failed
        /// to be created.</exception>
        public void SetNextVersion(VersionFileList vfl)
        {
            var nextVersion = LiveVersion + 1;

            // Save the file listing
            var outFilePath = VersionHelper.GetVersionFileListPath(nextVersion);
            vfl.Write(outFilePath);

            // Save the hash for the file listing
            var hash = Hasher.GetFileHash(outFilePath);
            var outHashPath = VersionHelper.GetVersionFileListHashPath(nextVersion);
            if (File.Exists(outHashPath))
                File.Delete(outHashPath);

            File.WriteAllText(outHashPath, hash);

            // Notify listeners
            if (NextVersionCreated != null)
                NextVersionCreated(this);
        }

        /// <summary>
        /// Tries to set the live version to a new value.
        /// </summary>
        /// <param name="newVersion">The new live version.</param>
        /// <param name="showMessageBox">If true, a <see cref="MessageBox"/> is shown why the change failed.</param>
        /// <returns>True if the live version was successfully changed; otherwise false.</returns>
        public bool TrySetLiveVersion(int newVersion, bool showMessageBox = true)
        {
            // Check for a valid new version number
            if (newVersion < LiveVersion)
            {
                if (showMessageBox)
                    MessageBox.Show("Cannot set the live version to a version earlier than the current live version!");
                return false;
            }

            if (newVersion == LiveVersion)
                return false;

            // Make sure the version exists
            var newVersionPath = VersionHelper.GetVersionFileListPath(newVersion);
            if (!File.Exists(newVersionPath))
            {
                if (showMessageBox)
                    MessageBox.Show("Cannot set the live version to the specified new version since that version does not exist!");
                return false;
            }

            // Set the new live version
            _liveVersion = newVersion;

            UpdateLiveVersionFile();

            // Save
            Save();

            // Raise event
            if (LiveVersionChanged != null)
                LiveVersionChanged(this);

            return true;
        }

        /// <summary>
        /// Updates the live version file.
        /// </summary>
        void UpdateLiveVersionFile()
        {
            var tmpPath = _liveVersionFilePath + ".tmp";

            if (File.Exists(tmpPath))
                File.Delete(tmpPath);

            File.WriteAllText(tmpPath, LiveVersion.ToString());

            File.Copy(tmpPath, _liveVersionFilePath, true);

            if (File.Exists(tmpPath))
                File.Delete(tmpPath);
        }
    }
}