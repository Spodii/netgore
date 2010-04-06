using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling events from the <see cref="GameControl"/>.
    /// </summary>
    /// <param name="gameControl">The <see cref="GameControl"/> the event came from.</param>
    public delegate void GameControlEventHandler(GameControl gameControl);
}