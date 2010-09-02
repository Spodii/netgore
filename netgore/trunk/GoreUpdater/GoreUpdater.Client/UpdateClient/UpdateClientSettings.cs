using System;
using System.Diagnostics;
using System.IO;

namespace GoreUpdater
{
    /// <summary>
    /// Contains the settings for an <see cref="UpdateClient"/> instance.
    /// </summary>
    public class UpdateClientSettings
    {
        bool _isReadOnly;
        string _localFileServerPath;
        string _localMasterServerPath;
        string _targetPath;
        string _tempPath;
        string _offlineFileReplacerPath;
        string _versionFilePath;
        Func<string, string, IOfflineFileReplacer> _offlineFileReplacer;

        /// <summary>
        /// The default method for creating the <see cref="IOfflineFileReplacer"/>.
        /// </summary>
        /// <param name="filePath">The path for the <see cref="IOfflineFileReplacer"/>'s output.</param>
        /// <param name="appPath">The path to the application to run after the <see cref="IOfflineFileReplacer"/> finishes.</param>
        /// <returns>The <see cref="IOfflineFileReplacer"/> instance.</returns>
        protected virtual IOfflineFileReplacer DefaultOfflineFileReplacer(string filePath, string appPath)
        {
            // FUTURE: Support Linux by using something other than batch files
            return new BatchOfflineFileReplacer(filePath, appPath);
        }

        /// <summary>
        /// Gets or sets the path to the file that the <see cref="IOfflineFileReplacer"/> instance will output to.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="IsReadOnly"/> is true.</exception>
        public string OfflineFileReplacerPath
        {
            get { return _offlineFileReplacerPath; }
            set
            {
                if (IsReadOnly)
                    throw ReadOnlyException(); 
                
                _offlineFileReplacerPath = value;
            }
        }

        /// <summary>
        /// Gets the current version of the client using the <see cref="VersionFilePath"/>.
        /// </summary>
        /// <returns>The current version, or null if the current version is unknown since the file at the <see cref="VersionFilePath"/>
        /// does not exist or contains invalid data.</returns>
        public int? GetCurrentVersion()
        {
            try
            {
                var f = VersionFilePath;

                // Check for the file
                if (!File.Exists(f))
                    return null;

                // Read the file
                var s = File.ReadAllText(f);

                // Parse the file's contents
                int version;
                if (!int.TryParse(s, out version))
                {
                    Debug.Fail("Could not parse version file. Contents: " + s);
                    return null;
                }

                return version;
            }
            catch (IOException ex)
            {
                Debug.Fail(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the path to the file containing the current version.
        /// </summary>
        public string VersionFilePath
        {
            get { return _versionFilePath; }
            set
            {
                if (IsReadOnly)
                    throw ReadOnlyException();

                _versionFilePath = value;
            }
        }

        /// <summary>
        /// Gets or sets the Func to use to create the <see cref="IOfflineFileReplacer"/> to be used. If set to null, the
        /// internal default implementation will be used. The first parameter is the output file path for the generated
        /// <see cref="IOfflineFileReplacer"/> program or script (such as a batch file, bash script, etc) and the
        /// second parameter is the path to the application that should be run after the script finishes.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="IsReadOnly"/> is true.</exception>
        public Func<string, string, IOfflineFileReplacer> OfflineFileReplacerCreator
        {
            get
            {
                if (_offlineFileReplacer == null)
                    return DefaultOfflineFileReplacer;

                return _offlineFileReplacer;
            }
            set
            {
                if (IsReadOnly)
                    throw ReadOnlyException();

                _offlineFileReplacer = value;
            }
        }

        string _resetAppPath;

        /// <summary>
        /// Gets or sets the full path to the application that will be run if this program ever needs to reset
        /// itself, such as to update itself. This is typically the path to the current application.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="IsReadOnly"/> is true.</exception>
        public string ResetAppPath
        {
            get { return _resetAppPath; }
            set
            {
                if (IsReadOnly)
                    throw ReadOnlyException();
                
                _resetAppPath = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateClientSettings"/> class.
        /// </summary>
        /// <param name="targetPath">The path that the updated files should ultimately be located. This is usually the same directory
        /// that the program is running in.</param>
        /// <param name="settingsPath">The path to the directory containing the settings.</param>
        /// <param name="resetAppPath">The full path to the application that will be run if this program ever needs to reset
        /// itself, such as to update itself. This is typically the path to the current application.</param>
        public UpdateClientSettings(string targetPath, string settingsPath, string resetAppPath)
        {
            TargetPath = targetPath;
            ResetAppPath = resetAppPath;

            // Set the default values
            TempPath = PathHelper.CombineDifferentPaths(settingsPath, "_temp");
            LocalFileServerPath = PathHelper.CombineDifferentPaths(settingsPath, MasterServerReader.CurrentDownloadSourcesFilePath);
            LocalMasterServerPath = PathHelper.CombineDifferentPaths(settingsPath, MasterServerReader.CurrentMasterServersFilePath);
            OfflineFileReplacerPath = PathHelper.CombineDifferentPaths(settingsPath, GlobalSettings.ReplacerFileName);
            VersionFilePath = PathHelper.CombineDifferentPaths(settingsPath, "current_version");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateClientSettings"/> class.
        /// </summary>
        UpdateClientSettings()
        {
        }

        /// <summary>
        /// Gets if this <see cref="UpdateClientSettings"/> instance is read-only. Once this value is true, it will never
        /// be false again. That is, an instance that is read-only cannot be changed to not read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Gets or sets the file path to the local file containing the list of file servers (aka file source servers).
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="IsReadOnly"/> is true.</exception>
        public string LocalFileServerPath
        {
            get { return _localFileServerPath; }
            set
            {
                if (IsReadOnly)
                    throw ReadOnlyException();

                _localFileServerPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the file path to the local file containing the list of master servers.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="IsReadOnly"/> is true.</exception>
        public string LocalMasterServerPath
        {
            get { return _localMasterServerPath; }
            set
            {
                if (IsReadOnly)
                    throw ReadOnlyException();

                _localMasterServerPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the path that the updated files should ultimately be located. This is usually the same directory
        /// that the program is running in.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="IsReadOnly"/> is true.</exception>
        public string TargetPath
        {
            get { return _targetPath; }
            set
            {
                if (IsReadOnly)
                    throw ReadOnlyException();

                _targetPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the path to store temporary files in, namely files that are being downloaded or have finished
        /// downloading but could not be copied to the target path. This should NOT be the system's temporary directory.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tried to set the value while <see cref="IsReadOnly"/> is true.</exception>
        public string TempPath
        {
            get { return _tempPath; }
            set
            {
                if (IsReadOnly)
                    throw ReadOnlyException();

                _tempPath = value;
            }
        }

        /// <summary>
        /// Creates a <see cref="IOfflineFileReplacer"/> instance using the settings provided by this object instance.
        /// </summary>
        /// <returns>The <see cref="IOfflineFileReplacer"/> instance using the settings provided by this object instance.</returns>
        public IOfflineFileReplacer CreateOfflineFileReplacer()
        {
            return OfflineFileReplacerCreator(OfflineFileReplacerPath, ResetAppPath);
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A deep copy of this instance.</returns>
        public UpdateClientSettings DeepCopy()
        {
            var ret = new UpdateClientSettings
            {
                LocalFileServerPath = LocalFileServerPath,
                LocalMasterServerPath = LocalMasterServerPath,
                TempPath = TempPath,
                TargetPath = TargetPath,
                OfflineFileReplacerCreator = OfflineFileReplacerCreator,
                OfflineFileReplacerPath = OfflineFileReplacerPath
            };

            return ret;
        }

        /// <summary>
        /// Creates a read-only deep copy of this instance.
        /// </summary>
        /// <returns>A read-only deep copy of this instance.</returns>
        public UpdateClientSettings ReadOnlyDeepCopy()
        {
            var ret = DeepCopy();
            ret.SetReadOnly();
            return ret;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> to use for when the read-only is violated.
        /// </summary>
        /// <returns>The <see cref="Exception"/> to use for when the read-only is violated.</returns>
        static InvalidOperationException ReadOnlyException()
        {
            const string msg = "Cannot alter the values of this object instance since it is set to read-only.";
            return new InvalidOperationException(msg);
        }

        /// <summary>
        /// Sets this <see cref="UpdateClientSettings"/> instance as read-only. Once set, it is forever read-only.
        /// </summary>
        void SetReadOnly()
        {
            _isReadOnly = true;
        }
    }
}