using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling an event from the <see cref="IScreenManager"/>.
    /// </summary>
    /// <param name="screenManager">The <see cref="IScreenManager"/> the event came from.</param>
    public delegate void IScreenManagerEventHandler(IScreenManager screenManager);
}