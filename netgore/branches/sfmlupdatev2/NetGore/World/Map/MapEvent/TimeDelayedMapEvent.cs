using System;
using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// A basic <see cref="IDelayedMapEvent"/> that executes an <see cref="Action"/> after an amount of time has elapsed.
    /// </summary>
    public class TimeDelayedMapEvent : TimeDelayedMapEventBase
    {
        readonly Action _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDelayedMapEvent"/> class.
        /// </summary>
        /// <param name="delay">The delay in milliseconds before this event executes.</param>
        /// <param name="action">The action for this event to perform.</param>
        public TimeDelayedMapEvent(int delay, Action action) : base(delay)
        {
            _action = action;
        }

        /// <summary>
        /// When overridden in the derived class, handles executing the event.
        /// </summary>
        protected override void Execute()
        {
            if (_action != null)
                _action();
        }
    }
}