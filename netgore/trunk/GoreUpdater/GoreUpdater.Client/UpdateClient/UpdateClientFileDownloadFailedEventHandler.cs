using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling the <see cref="UpdateClient.FileDownloaded"/> from the <see cref="UpdateClient"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UpdateClient"/> that this event came from.</param>
    /// <param name="remoteFile">The remote file that failed to download.</param>
    public delegate void UpdateClientFileDownloadFailedEventHandler(UpdateClient sender, string remoteFile);
}