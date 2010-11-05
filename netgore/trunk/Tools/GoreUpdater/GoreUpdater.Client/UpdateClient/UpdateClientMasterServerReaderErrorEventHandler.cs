using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling the <see cref="UpdateClient.MasterServerReaderError"/> from the <see cref="UpdateClient"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UpdateClient"/> that this event came from.</param>
    /// <param name="error">The error message.</param>
    public delegate void UpdateClientMasterServerReaderErrorEventHandler(UpdateClient sender, string error);
}