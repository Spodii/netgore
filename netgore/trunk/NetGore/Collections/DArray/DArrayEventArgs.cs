using System;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// <see cref="EventArgs"/> for an event related to <see cref="DArray{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    [Serializable]
    public class DArrayEventArgs<T> : EventArgs
    {
        readonly int _index;
        readonly T _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="DArrayEventArgs{T}"/> class.
        /// </summary>
        public DArrayEventArgs(T item, int index)
        {
            _item = item;
            _index = index;
        }

        /// <summary>
        /// Gets the index related to the event.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Gets the item related to the event.
        /// </summary>
        public T Item
        {
            get { return _item; }
        }
    }
}