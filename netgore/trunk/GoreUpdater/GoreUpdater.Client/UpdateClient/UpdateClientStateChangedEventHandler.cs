using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling when the <see cref="UpdateClient"/>'s state has changed.
    /// </summary>
    /// <param name="sender">The <see cref="UpdateClient"/> that this event came from.</param>
    /// <param name="oldState">The old <see cref="UpdateClientState"/>.</param>
    /// <param name="newState">The new (and current) <see cref="UpdateClientState"/>.</param>
    public delegate void UpdateClientStateChangedEventHandler(
        UpdateClient sender, UpdateClientState oldState, UpdateClientState newState);
}