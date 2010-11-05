using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Delegate for handling events from the <see cref="UpdateClient"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UpdateClient"/> that this event came from.</param>
    public delegate void UpdateClientEventHandler(UpdateClient sender);
}