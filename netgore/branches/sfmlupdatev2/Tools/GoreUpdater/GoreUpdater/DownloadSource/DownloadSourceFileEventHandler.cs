using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling a file download event from the <see cref="IDownloadSource"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDownloadSource"/> that downloaded the file.</param>
    /// <param name="remoteFile">The remote file that was downloaded.</param>
    /// <param name="localFilePath">The complete file path to the local file containing the downloaded contents.</param>
    public delegate void DownloadSourceFileEventHandler(IDownloadSource sender, string remoteFile, string localFilePath);
}