using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// Delegate for handling an event from the <see cref="ServerInfoBase"/>.
    /// </summary>
    /// <param name="sender">The <see cref="ServerInfoBase"/> the event came from.</param>
    public delegate void ServerInfoEventHandler(ServerInfoBase sender);

    /// <summary>
    /// Describes a file server instance.
    /// </summary>
    public class FileServerInfo : ServerInfoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileServerInfo"/> class.
        /// </summary>
        /// <param name="type">The type of file uploader to use.</param>
        /// <param name="host">The host address.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public FileServerInfo(FileUploaderType type, string host, string user, string password) : base(type, host, user, password)
        {
        }

        /// <summary>
        /// Creates a <see cref="FileServerInfo"/> from a creation string.
        /// </summary>
        /// <param name="creationString">The <see cref="FileServerInfo"/> creation string.</param>
        /// <returns>The <see cref="FileServerInfo"/> instance.</returns>
        public static FileServerInfo Create(string creationString)
        {
            var s = creationString.Split(new string[] { CreationStringDelimiter }, StringSplitOptions.None);
            if (s.Length != 4)
                throw new ArgumentException("Invalid creation string - incorrect number of arguments provided.");

            var type = (FileUploaderType)Enum.Parse(typeof(FileUploaderType), s[0]);
            var host = s[1];
            var user = s[2];
            var password = s[3];

            return new FileServerInfo(type, host, user, password);
        }

        /// <summary>
        /// When overridden in the derived class, handles synchronizing the given version.
        /// </summary>
        /// <param name="fu">The <see cref="IFileUploader"/> to use.</param>
        /// <param name="v">The version to synchronize.</param>
        /// <param name="reEnqueue">True if the <paramref name="v"/> should be re-enqueued so it can be re-attempted.
        /// If the method throws an <see cref="Exception"/>, the <paramref name="v"/> will be re-enqueued no matter what.</param>
        /// <returns>
        /// The error string, or null if the synchronization was successful.
        /// </returns>
        protected override string DoSync(IFileUploader fu, int v, out bool reEnqueue)
        {
            reEnqueue = false;

            // Load the VersionFileList for the version to check
            var vflPath = VersionHelper.GetVersionFileListPath(v);
            if (!File.Exists(vflPath))
            {
                // Version doesn't exist at all
                return null;
            }

            var vfl = VersionFileList.CreateFromFile(vflPath);

            // Try to download the version's file list hash
            var fileListHashPath = GetVersionRemoteFilePath(v, PathHelper.RemoteFileListHashFileName);
            var vflHash = fu.DownloadAsString(fileListHashPath);

            // Check if the hash file exists on the server
            if (vflHash != null)
            {
                // Check if the hash matches the current version's hash
                var expectedVflHash = File.ReadAllText(VersionHelper.GetVersionFileListHashPath(v));
                if (vflHash != expectedVflHash)
                {
                    // Delete the whole version folder first
                    fu.DeleteDirectory(GetVersionRemoteFilePath(v, null));
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

            // Check the hashes of the local files
            foreach (var f in vfl.Files)
            {
                // Get the local file path
                var localPath = VersionHelper.GetVersionFile(v, f.FilePath);

                // Confirm the hash of the file
                var fileHash = Hasher.GetFileHash(localPath);
                if (fileHash != f.Hash)
                {
                    const string errmsg =
                        "The cached hash ({0}) of file `{1}` does not match the real hash ({2}) for version {3}." +
                        " Possible version corruption.";
                    return string.Format(errmsg, f.Hash, f.FilePath, fileHash, v);
                }
            }

            // Hashes check out, start uploading
            foreach (var f in vfl.Files)
            {
                // Get the local file path
                var localPath = VersionHelper.GetVersionFile(v, f.FilePath);

                var remotePath = GetVersionRemoteFilePath(v, f.FilePath);
                fu.UploadAsync(localPath, remotePath);
            }

            // Wait for uploads to finish
            while (fu.IsBusy)
            {
                Thread.Sleep(1000);
            }

            // All uploads have finished, so upload the VersionFileList hash
            fu.UploadAsync(VersionHelper.GetVersionFileListHashPath(v), fileListHashPath);

            // All done! That was easy enough, eh? *sigh*
            return null;
        }
    }
}