namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling upload events from the <see cref="IFileUploader"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IFileUploader"/> that the event came from.</param>
    /// <param name="path">The relative path on the remote system of the directory deleted.</param>
    public delegate void FileUploaderDeleteDirEventHandler(IFileUploader sender, string path);
}