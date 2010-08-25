namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling an event from the <see cref="IDownloadManager"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDownloadManager"/> that the event came from.</param>
    public delegate void DownloadManagerEventHandler(IDownloadManager sender);
}