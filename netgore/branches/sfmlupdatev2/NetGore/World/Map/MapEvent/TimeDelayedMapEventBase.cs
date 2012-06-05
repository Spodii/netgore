using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Base class for a simple <see cref="IDelayedMapEvent"/> that executes after an amount of time has elapsed.
    /// </summary>
    public abstract class TimeDelayedMapEventBase : IDelayedMapEvent
    {
        readonly TickCount _readyTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDelayedMapEventBase"/> class.
        /// </summary>
        /// <param name="delay">The delay in milliseconds before this event executes.</param>
        protected TimeDelayedMapEventBase(int delay)
        {
            _readyTime = (TickCount)(TickCount.Now + delay);
        }

        /// <summary>
        /// When overridden in the derived class, handles executing the event.
        /// </summary>
        protected abstract void Execute();

        #region IDelayedMapEvent Members

        /// <summary>
        /// Executes the event.
        /// </summary>
        void IDelayedMapEvent.Execute()
        {
            Execute();
        }

        /// <summary>
        /// Gets if this <see cref="IDelayedMapEvent"/> is ready to be executed.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public virtual bool IsReady(TickCount currentTime)
        {
            return _readyTime <= currentTime;
        }

        #endregion
    }
}