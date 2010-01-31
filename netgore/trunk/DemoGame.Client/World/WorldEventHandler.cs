using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles events from the <see cref="World"/>.
    /// </summary>
    /// <param name="world">The <see cref="World"/> the event came from.</param>
    public delegate void WorldEventHandler(World world);

    /// <summary>
    /// Handles events from the <see cref="World"/>.
    /// </summary>
    /// <typeparam name="T">The type of event argument.</typeparam>
    /// <param name="world">The <see cref="World"/> the event came from.</param>
    /// <param name="e">The event argument.</param>
    public delegate void WorldEventHandler<T>(World world, T e);
}