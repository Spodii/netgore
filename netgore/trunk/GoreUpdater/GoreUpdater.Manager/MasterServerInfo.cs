using System;
using System.IO;
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
        public MasterServerInfo(FileUploaderType type, string host, string user, string password)
            : base(type, host, user, password)
        {
        }

        /// <summary>
        /// Creates a <see cref="MasterServerInfo"/> from a creation string.
        /// </summary>
        /// <param name="creationString">The <see cref="MasterServerInfo"/> creation string.</param>
        /// <returns>The <see cref="MasterServerInfo"/> instance.</returns>
        public static MasterServerInfo Create(string creationString)
        {
            var s = creationString.Split(new string[] { CreationStringDelimiter }, StringSplitOptions.None);
            if (s.Length != 4)
                throw new ArgumentException("Invalid creation string - incorrect number of arguments provided.");

            var type = (FileUploaderType)Enum.Parse(typeof(FileUploaderType), s[0]);
            var host = s[1];
            var user = s[2];
            var password = s[3];

            return new MasterServerInfo(type, host, user, password);
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

            string remoteFileListFilePath = PathHelper.GetVersionString(v) + ".txt";
            string remoteFileListHashFilePath = PathHelper.GetVersionString(v) + ".hash";

            // Ensure the live version is written. This is a very small but very important file, so just write it during
            // every synchronization.
            fu.UploadAsync(_settings.LiveVersionFilePath, "live_version");

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
            var fileListHashPath = GetVersionRemoteFilePath(v, PathHelper.RemoteFileListHashFileName);
            var vflHahs = fu.DownloadAsString(fileListHashPath);

            // Check if the hash file exists on the server
            if (vflHahs != null)
            {
                // Check if the hash matches the current version's hash
                var expectedVflHash = File.ReadAllText(VersionHelper.GetVersionFileListHashPath(v));
                if (vflHahs != expectedVflHash)
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