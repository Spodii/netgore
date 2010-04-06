using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling a generic event from the <see cref="IGUIManager"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IGUIManager"/> the event came from.</param>
    public delegate void GUIEventHandler(IGUIManager sender);
}