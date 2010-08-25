using System.Linq;

namespace GoreUpdater.Core
{
    /// <summary>
    /// Delegate for handling a file download event from the <see cref="IDownloadSource"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IDownloadSource"/> that downloaded the file.</param>
    /// <param name="remoteFile">The remote file that was downloaded.</param>
    public delegate void DownloadSourceFileEventHandler(IDownloadSource sender, string remoteFile);
}