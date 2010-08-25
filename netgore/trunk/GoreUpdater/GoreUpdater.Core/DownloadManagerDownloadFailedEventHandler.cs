using System.Linq;

namespace GoreUpdater.Core
{
    /// <summary>
    /// Delegate for handling a file download failure event from the <see cref="IDownloadManager"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDownloadManager"/> that failed to download the file.</param>
    /// <param name="remoteFile">The remote file that failed to be downloaded.</param>
    public delegate void DownloadManagerDownloadFailedEventHandler(IDownloadManager sender, string remoteFile);
}