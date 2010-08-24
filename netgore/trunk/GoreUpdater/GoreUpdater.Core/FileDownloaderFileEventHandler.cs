namespace GoreUpdater.Core
{
    /// <summary>
    /// Delegate for handling a file download event from the <see cref="IDownloadManager"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDownloadManager"/> that downloaded the file.</param>
    /// <param name="remoteFile">The remote file that was downloaded.</param>
    /// <param name="localFilePath">The path to the local file where the downloaded file is stored.</param>
    public delegate void FileDownloaderFileEventHandler(IDownloadManager sender, string remoteFile, string localFilePath);
}