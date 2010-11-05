using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling a file download failure event from the <see cref="IDownloadSource"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDownloadSource"/> that failed to download the file.</param>
    /// <param name="remoteFile">The remote file that failed to be downloaded.</param>
    /// <param name="localFilePath">The complete file path to the local file that was being used to download the contents to.</param>
    public delegate void DownloadSourceFileFailedEventHandler(IDownloadSource sender, string remoteFile, string localFilePath);
}