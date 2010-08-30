using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// Describes a master server instance.
    /// </summary>
    public class MasterServerInfo : ServerInfoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasterServerInfo"/> class.
        /// </summary>
        /// <param name="type">The type of file uploader to use.</param>
        /// <param name="host">The host address.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <param name="downloadType">The type of file downloader to use.</param>
        /// <param name="downloadHost">The download host.</param>
        public MasterServerInfo(FileUploaderType type, string host, string user, string password, DownloadSourceType downloadType,
            string downloadHost)
            : base(type, host, user, password, downloadType, downloadHost)
        {
            // For the master server, we also want to update whenever the list of servers changes
            _settings.FileServerListChanged += _settings_FileServerListChanged;
            _settings.MasterServerListChanged += _settings_MasterServerListChanged;
        }

        /// <summary>
        /// Handles when the master server list has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void _settings_MasterServerListChanged(ManagerSettings sender)
        {
            // Just sync the latest version, which will force the server list to update
            EnqueueSyncVersion(sender.LiveVersion);
        }

        /// <summary>
        /// Handles when the file server list has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void _settings_FileServerListChanged(ManagerSettings sender)
        {
            // Just sync the latest version, which will force the server list to update
            EnqueueSyncVersion(sender.LiveVersion);
        }

        /// <summary>
        /// Creates a <see cref="MasterServerInfo"/> from a creation string.
        /// </summary>
        /// <param name="creationString">The <see cref="MasterServerInfo"/> creation string.</param>
        /// <returns>The <see cref="MasterServerInfo"/> instance.</returns>
        public static MasterServerInfo Create(string creationString)
        {
            var s = creationString.Split(new string[] { CreationStringDelimiter }, StringSplitOptions.None);
            if (s.Length != 6)
                throw new ArgumentException("Invalid creation string - incorrect number of arguments provided.");

            var type = (FileUploaderType)Enum.Parse(typeof(FileUploaderType), s[0]);
            var host = s[1];
            var user = s[2];
            var password = s[3];
            var downloadType = (DownloadSourceType)Enum.Parse(typeof(DownloadSourceType), s[4]);
            var downloadHost = s[5];

            return new MasterServerInfo(type, host, user, password, downloadType, downloadHost);
        }

        #region Overrides of ServerInfoBase

        /// <summary>
        /// When overridden in the derived class, handles synchronizing the given version.
        /// </summary>
        /// <param name="fu">The <see cref="IFileUploader"/> to use.</param>
        /// <param name="v">The version to synchronize.</param>
        /// <param name="reEnqueue">True if the <paramref name="v"/> should be re-enqueued so it can be re-attempted.
        /// If the method throws an <see cref="Exception"/>, the <paramref name="v"/> will be re-enqueued no matter what.</param>
        /// <returns>The error string, or null if the synchronization was successful.</returns>
        protected override string DoSync(IFileUploader fu, int v, out bool reEnqueue)
        {
            fu.SkipIfExists = false;

            reEnqueue = false;

            var remoteFileListFilePath = PathHelper.GetVersionString(v) + ".txt";
            var remoteFileListHashFilePath = PathHelper.GetVersionString(v) + ".hash";

            // Ensure the live version is written. This is a very small but very important file, so just write it during
            // every synchronization.
            fu.UploadAsync(_settings.LiveVersionFilePath, MasterServerReader.CurrentVersionFilePath);

            // Also ensure the master server and file server lists are up-to-date. Again, we will just do this every time we
            // check to sync since they are relatively small lists but very important to keep up-to-date.
            fu.UploadAsync(_settings.FileServerListFilePath, MasterServerReader.CurrentDownloadSourcesFilePath);
            fu.UploadAsync(_settings.MasterServerListFilePath, MasterServerReader.CurrentMasterServersFilePath);

            // Load the VersionFileList for the version to check
            var vflPath = VersionHelper.GetVersionFileListPath(v);
            if (!File.Exists(vflPath))
            {
                // Version doesn't exist at all
                return null;
            }

            // Test the creation of the VersionFileList to ensure its valid
            VersionFileList.CreateFromFile(vflPath);

            // Try to download the version's file list hash
            var vflHash = fu.DownloadAsString(remoteFileListHashFilePath);

            // Check if the hash file exists on the server
            if (vflHash != null)
            {
                // Check if the hash matches the current version's hash
                var expectedVflHash = File.ReadAllText(VersionHelper.GetVersionFileListHashPath(v));
                if (vflHash != expectedVflHash)
                {
                    // We don't need to delete anything since we make the MasterServer overwrite instead
                }
                else
                {
                    // Hash existed and was correct - good enough for us!
                    return null;
                }
            }
            else
            {
                // Hash didn't exist at all, so we will have to update. As long as SkipIfExists is set to true, files
                // that already exist will be skipped, so we will only end up uploading the new files. In any case, its
                // the same process either way.
            }

            // Upload the files
            fu.UploadAsync(VersionHelper.GetVersionFileListPath(v), remoteFileListFilePath);
            fu.UploadAsync(VersionHelper.GetVersionFileListHashPath(v), remoteFileListHashFilePath);

            // Wait for uploads to finish
            while (fu.IsBusy)
            {
                Thread.Sleep(1000);
            }

            // All done!
            return null;
        }

        #endregion
    }
}