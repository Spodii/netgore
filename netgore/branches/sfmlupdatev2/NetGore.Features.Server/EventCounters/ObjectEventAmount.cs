using System.Linq;

namespace NetGore.Features.EventCounters
{
    /// <summary>
    /// A tuple of an object, event, and amount for an <see cref="IEventCounter{T,U}"/>.
    /// </summary>
    /// <typeparam name="TObjectID">The type of object ID.</typeparam>
    /// <typeparam name="TEventID">The type of event ID.</typeparam>
    public struct ObjectEventAmount<TObjectID, TEventID>
    {
        readonly TObjectID _objectID;
        readonly TEventID _eventID;
        readonly int _amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectEventAmount{TObjectID, TEventID}"/> struct.
        /// </summary>
        /// <param name="objectID">The object for the event to be incremented.</param>
        /// <param name="eventID">The event to be incremented.</param>
        /// <param name="amount">The amount to increment.</param>
        public ObjectEventAmount(TObjectID objectID, TEventID eventID, int amount)
        {
            _objectID = objectID;
            _eventID = eventID;
            _amount = amount;
        }

        /// <summary>
        /// Gets the object for the event to be incremented.
        /// </summary>
        public TObjectID ObjectID
        {
            get { return _objectID; }
        }

        /// <summary>
        /// Gets the event to be incremented.
        /// </summary>
        public TEventID EventID
        {
            get { return _eventID; }
        }

        /// <summary>
        /// Gets the amount to increment.
        /// </summary>
        public int Amount
        {
            get { return _amount; }
        }
    }
}