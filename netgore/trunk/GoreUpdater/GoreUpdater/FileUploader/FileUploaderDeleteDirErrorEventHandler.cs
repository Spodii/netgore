using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling upload events from the <see cref="IFileUploader"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IFileUploader"/> that the event came from.</param>
    /// <param name="path">The relative path on the remote system of the directory deleted.</param>
    /// <param name="error">The error message for why the directory could not be deleted.</param>
    /// <param name="attemptCount">The number of times this particular job has been attempted. This value is incremented every
    /// time the job is attempted, even if it fails for a different reason.
    /// Once this value reaches 255, it will no longer increment.</param>
    public delegate void FileUploaderDeleteDirErrorEventHandler(IFileUploader sender, string path, string error, byte attemptCount
        );
}