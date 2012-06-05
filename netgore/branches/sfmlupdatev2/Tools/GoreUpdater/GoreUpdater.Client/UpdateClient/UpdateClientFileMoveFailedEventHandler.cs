using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling the <see cref="UpdateClient.FileMoveFailed"/> from the <see cref="UpdateClient"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UpdateClient"/> that this event came from.</param>
    /// <param name="remoteFile">The remote file that was downloaded.</param>
    /// <param name="localFilePath">The local file path that the file was downloaded to.</param>
    /// <param name="targetFilePath">The local file path that the file was to be moved to, but couldn't move to.</param>
    public delegate void UpdateClientFileMoveFailedEventHandler(
        UpdateClient sender, string remoteFile, string localFilePath, string targetFilePath);
}