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
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <param name="world">The <see cref="World"/> the event came from.</param>
    /// <param name="arg1">The first event argument.</param>
    public delegate void WorldEventHandler<in T1>(World world, T1 arg1);

    /// <summary>
    /// Handles events from the <see cref="World"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second event argument.</typeparam>
    /// <param name="world">The <see cref="World"/> the event came from.</param>
    /// <param name="arg1">The first event argument.</param>
    /// <param name="arg2">The second event argument.</param>
    public delegate void WorldEventHandler<in T1, in T2>(World world, T1 arg1, T2 arg2);
}