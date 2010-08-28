using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling test connection message events from the <see cref="IFileUploader"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IFileUploader"/> that the event came from.</param>
    /// <param name="message">The message related to the current state of the connection testing.</param>
    /// <param name="userState">The object that was passed to the <see cref="IFileUploader.TestConnection"/> method.</param>
    public delegate void FileUploaderTestConnectionMessageEventHandler(IFileUploader sender, string message, object userState);
}