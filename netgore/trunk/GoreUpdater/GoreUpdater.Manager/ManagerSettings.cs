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
        const string _headerLiveVersion = "LIVEVERSION";
        const string _headerFileServer = "FILESERVER";
        const string _settingsFile = "settings.txt";

        static readonly ManagerSettings _instance;

        readonly string _filePath;
        readonly object _saveSync = new object();
        readonly object _versionSync = new object();

        /// <summary>
        /// Notifies listeners when a file server has been added or removed from the list.
        /// </summary>
        public event ManagerSettingsEventHandler FileServerListChanged;

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
        /// Removes from the file server list.
        /// </summary>
        /// <param name="fileServer">The <see cref="FileServerInfo"/> to remove.</param>
        /// <returns>True if removed; otherwise false.</returns>
        public bool RemoveFileServer(FileServerInfo fileServer)
        {
            bool removed;
            lock (_fileServersSync)
            {
                removed = _fileServers.Remove(fileServer);
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
        /// Adds to the file server list.
        /// </summary>
        /// <param name="fileServer">The <see cref="FileServerInfo"/> to add.</param>
        /// <returns>True if added; otherwise false.</returns>
        public bool AddFileServer(FileServerInfo fileServer)
        {
            lock (_fileServersSync)
            {
                if (_fileServers.Contains(fileServer))
                    return false;

                _fileServers.Add(fileServer);
            }

            Save();

            if (FileServerListChanged != null)
                FileServerListChanged(this);

            return true;
        }

        readonly object _fileServersSync = new object();

        readonly List<FileServerInfo> _fileServers = new List<FileServerInfo>();

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
            _filePath = filePath;
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
        /// Notifies listeners when the next version (the version after the live version) to release has been created, or re-created.
        /// </summary>
        public event ManagerSettingsEventHandler NextVersionCreated;

        /// <summary>
        /// Notifies listeners when the live version has changed.
        /// </summary>
        public event ManagerSettingsEventHandler LiveVersionChanged;

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
        /// Gets the path to the settings file.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
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
                            break;

                        case _headerFileServer:
                            try
                            {
                                var fs = FileServerInfo.Create(v.Value);
                                lock (_fileServersSync)
                                {
                                    _fileServers.Add(fs);
                                }
                            }
                            catch (Exception ex)
                            {
                                const string errmsg = "Encountered invalid {0} value in settings file: {1}";
                                MessageBox.Show(string.Format(errmsg, _headerLiveVersion, ex));
                            }
                            break;

                        default:
                            MessageBox.Show("Encountered unknown setting: " + v.Key);
                            break;
                    }
            }
        }

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        void Save()
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
        /// Tries to set the live version to a new value.
        /// </summary>
        /// <param name="newVersion">The new live version.</param>
        /// <param name="showMessageBox">If true, a <see cref="MessageBox"/> is shown why the change failed.</param>
        /// <returns>True if the live version was successfully changed; otherwise false.</returns>
        public bool TrySetLiveVersion(int newVersion, bool showMessageBox = true)
        {
            if (newVersion < LiveVersion)
            {
                if (showMessageBox)
                    MessageBox.Show("Cannot set the live version to a version earlier than the current live version!");
                return false;
            }

            if (newVersion == LiveVersion)
                return false;

            // TODO: Make sure the version exists

            _liveVersion = newVersion;

            Save();

            if (LiveVersionChanged != null)
                LiveVersionChanged(this);

            return true;
        }
    }
}