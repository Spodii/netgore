namespace GoreUpdater.Core
{
    /// <summary>
    /// Delegate for handling a file download failure event from the <see cref="IDownloadSource"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDownloadSource"/> that failed to download the file.</param>
    /// <param name="remoteFile">The remote file that failed to be downloaded.</param>
    public delegate void DownloadSourceFileFailedEventHandler(IDownloadSource sender, string remoteFile);
}