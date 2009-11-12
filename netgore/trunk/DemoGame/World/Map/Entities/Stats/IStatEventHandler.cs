using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Handles basic IStat events.
    /// </summary>
    /// <param name="stat">The IStat that the event took place on.</param>
    public delegate void IStatEventHandler(IStat stat);
}