using System.Linq;
using NetGore;

namespace DemoGame.Editor
{
    /// <summary>
    /// Delegate for handling the <see cref="GlobalState.Tick"/> event.
    /// </summary>
    /// <param name="currentTime">The current <see cref="TickCount"/>.</param>
    public delegate void GlobalStateTickEventHandler(TickCount currentTime);
}