using System.Linq;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling a generic event from the <see cref="GUIManagerBase"/>.
    /// </summary>
    /// <param name="sender">The <see cref="GUIManagerBase"/> the event came from.</param>
    public delegate void GUIEventHandler(GUIManagerBase sender);
}