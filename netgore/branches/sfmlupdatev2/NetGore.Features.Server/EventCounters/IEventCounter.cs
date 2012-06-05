using System;
using System.Linq;

namespace NetGore.Features.EventCounters
{
    /// <summary>
    /// Interface for an object that is able to update a collection of event counters.
    /// </summary>
    /// <typeparam name="TObjectID">The type of object ID.</typeparam>
    /// <typeparam name="TEventID">The type of event ID.</typeparam>
    public interface IEventCounter<in TObjectID, in TEventID> : IDisposable
    {
        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Forces the internal cache to be synchronously flushed and executes the queries against the database.
        /// </summary>
        void Flush();

        /// <summary>
        /// Increments the counter.
        /// </summary>
        /// <param name="source">The source object to increment the counter for.</param>
        /// <param name="e">The event to increment.</param>
        /// <exception cref="ObjectDisposedException">This object has been disposed.</exception>
        void Increment(TObjectID source, TEventID e);

        /// <summary>
        /// Increments the counter.
        /// </summary>
        /// <param name="source">The source object to increment the counter for.</param>
        /// <param name="e">The event to increment.</param>
        /// <param name="amount">The amount to increment.</param>
        /// <exception cref="ObjectDisposedException">This object has been disposed.</exception>
        void Increment(TObjectID source, TEventID e, int amount);
    }
}