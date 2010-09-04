using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling the <see cref="UpdateClient.FileDownloaded"/> from the <see cref="UpdateClient"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UpdateClient"/> that this event came from.</param>
    /// <param name="remoteFile">The remote file that was downloaded.</param>
    /// <param name="localFilePath">The local file path that the file was downloaded to.</param>
    public delegate void UpdateClientFileDownloadedEventHandler(UpdateClient sender, string remoteFile, string localFilePath);
}