using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// An interface for an event that takes place on an <see cref="IMap"/> but has a delay.
    /// </summary>
    public interface IDelayedMapEvent
    {
        /// <summary>
        /// Executes the event.
        /// </summary>
        void Execute();

        /// <summary>
        /// Gets if this <see cref="IDelayedMapEvent"/> is ready to be executed.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        bool IsReady(TickCount currentTime);
    }
}