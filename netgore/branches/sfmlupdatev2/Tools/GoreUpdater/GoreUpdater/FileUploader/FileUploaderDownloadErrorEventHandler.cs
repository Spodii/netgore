using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling upload error events from the <see cref="IFileUploader"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IFileUploader"/> that the event came from.</param>
    /// <param name="localFile">The local file for the job related to the error.</param>
    /// <param name="remoteFile">The remote file for the job related to the error.</param>
    /// <param name="error">A string containing the error message.</param>
    /// <param name="attemptCount">The number of times this particular job has been attempted. This value is incremented every
    /// time the job is attempted, even if it fails for a different reason.
    /// Once this value reaches 255, it will no longer increment.</param>
    public delegate void FileUploaderDownloadErrorEventHandler(
        IFileUploader sender, string localFile, string remoteFile, string error, byte attemptCount);
}