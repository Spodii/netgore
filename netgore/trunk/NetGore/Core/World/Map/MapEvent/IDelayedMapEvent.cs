using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// An interface for an event that takes place on an <see cref="IMap"/> but has a delay.
    /// </summary>
    public interface IDelayedMapEvent
    {
        /// <summary>
        /// Gets if this <see cref="IDelayedMapEvent"/> is ready to be executed.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        bool IsReady(TickCount currentTime);

        /// <summary>
        /// Executes the event.
        /// </summary>
        void Execute();
    }
}
