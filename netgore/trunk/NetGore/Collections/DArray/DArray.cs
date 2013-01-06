using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Collections
{
    // TODO: Stop using DArray

    /// <summary>
    /// Dynamic, auto-expanding array that preserves and recycles indices.
    /// </summary>
    /// <typeparam name="T">Type of object to store.</typeparam>
    public class DArray<T> : IList<T>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default size of the free indices stack.
        /// </summary>
        const int _defaultFreeStackSize = 8;

        /// <summary>
        /// Default size of the internal array.
        /// </summary>
        const int _defaultSize = 8;

        readonly BitArray _isIndexUsed = new BitArray(_defaultSize, false);

        readonly bool _trackFree;

        T[] _buffer;
        Stack<int> _freeIndices;
        int _highestIndex = -1;
        int _version = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DArray{T}"/> class.
        /// </summary>
        public DArray() : this(_defaultSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DArray{T}"/> class.
        /// </summary>
        /// <param name="trackFree">If free indices will be tracked and Add can be used</param>
        public DArray(bool trackFree) : this(_defaultSize, trackFree)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DArray{T}"/> class.
        /// </summary>
        /// <param name="initialSize">Initial size of the internal buffer (default 8)</param>
        /// <param name="trackFree">If free indices will be tracked and Add can be used</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="initialSize"/> is less than or equal to 0.</exception>
        public DArray(int initialSize, bool trackFree = false)
        {
            if (initialSize <= 0)
                throw new ArgumentOutOfRangeException("initialSize", "Invalid initialSize - must be greater than 0.");

            ResizeBuffer(initialSize);
            _trackFree = trackFree;

            // Only create the free indicies stack if we are tracking them
            if (_trackFree)
                _freeIndices = new Stack<int>(_defaultFreeStackSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DArray{T}"/> class.
        /// </summary>
        /// <param name="content">Initial content to put in the DArray</param>
        /// <param name="trackFree">If free indices will be tracked and Add can be used</param>
        public DArray(IEnumerable<T> content, bool trackFree) : this(content.Count(), trackFree)
        {
            foreach (var item in content)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Notifies listeners when an item has been added to the <see cref="DArray{T}"/>.
        /// </summary>
        public event TypedEventHandler<DArray<T>, DArrayEventArgs<T>> ItemAdded;

        /// <summary>
        /// Notifies listeners when an item has been removed from the <see cref="DArray{T}"/>.
        /// </summary>
        public event TypedEventHandler<DArray<T>, DArrayEventArgs<T>> ItemRemoved;

        /// <summary>
        /// Gets the length of the DArray.
        /// </summary>
        public int Length
        {
            get { return _highestIndex + 1; }
        }

        /// <summary>
        /// Gets if free indices are tracked. If disabled, you will not be able to use 
        /// Add() and will have to use the indices directly through this[index]. Must
        /// be set in the constructor.
        /// </summary>
        public bool TrackFree
        {
            get { return _trackFree; }
        }

        /// <summary>
        /// Checks if an index can be accessed without causing an ArgumentOutOfRangeException.
        /// </summary>
        /// <param name="index">Index to access.</param>
        /// <returns>True if access is allowed for the index, else false.</returns>
        public bool CanGet(int index)
        {
            return (index >= 0 && index <= _highestIndex);
        }

        /// <summary>
        /// Inserts an item to the array. This operation is significantly faster if TrackFree is set
        /// because the DArray can then use NextFreeIndex().
        /// </summary>
        /// <param name="item">Item to add to the array.</param>
        /// <returns>Index the item was inserted at.</returns>
        public int Insert(T item)
        {
            // Find the next free index
            var index = NextFreeIndex(true);

            if (CanGet(index))
                Debug.Assert(!_isIndexUsed[index], "Oh shit, we got an index is in use!");

            // Set the item to the index
            this[index] = item;

            // Items changed, so change the version
            _version++;

            // Return the index used
            return index;
        }

        /// <summary>
        /// Finds the next free index that can be used.
        /// </summary>
        /// <returns>Next free index that can be used.</returns>
        public int NextFreeIndex()
        {
            return NextFreeIndex(false);
        }

        /// <summary>
        /// Finds the next free index that can be used.
        /// </summary>
        /// <param name="popInsteadOfPeek">If true, the index will be Popped out of the FreeIndicies
        /// stack instead of using Peek. Set this to true if you plan on using the index immediately
        /// after calling this.</param>
        /// <returns>Next free index that can be used.</returns>
        /// <exception cref="InvalidOperationException">The collection has changed while enumerating over it.</exception>
        int NextFreeIndex(bool popInsteadOfPeek)
        {
            // Check if we can use the free indicies tracker
            if (_trackFree)
            {
                // If the freeIndices stack contains any values, grab from there, else return the next new index
                if (_freeIndices.Count > 0)
                {
                    int index;

                    // Get the next free index
                    if (popInsteadOfPeek)
                        index = _freeIndices.Pop();
                    else
                        index = _freeIndices.Peek();

                    // Ensure the index is actually free
                    if (_isIndexUsed[index])
                    {
                        // Make sure the invalid index is removed
                        if (!popInsteadOfPeek)
                            _freeIndices.Pop();

                        // Use recursion to get a valid one
                        index = NextFreeIndex(popInsteadOfPeek);
                    }

                    // Valid index found
                    return index;
                }
            }
            else
            {
                var startVersion = _version;

                // Can not use the free indicies tracker, so manually look for a free index
                for (var i = 0; i < _buffer.Length; i++)
                {
                    if (!_isIndexUsed[i])
                        return i;
                }

                if (startVersion != _version)
                    throw new InvalidOperationException("The collection has changed while enumerating over it.");
            }

            // If we have no free indices, we simply just make a new index
            return _highestIndex + 1;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="item">The item that was added.</param>
        /// <param name="index">The index the item that was added.</param>
        protected virtual void OnItemAdded(T item, int index)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="item">The item that was removed.</param>
        /// <param name="index">The index the item that was removed.</param>
        protected virtual void OnItemRemoved(T item, int index)
        {
        }

        /// <summary>
        /// Rebuilds the stack of free indicies.
        /// </summary>
        void RebuildFreeIndiciesStack()
        {
            // Clear all the old values
            _freeIndices.Clear();

            // Add all the free indicies in reverse order so the lowest indicies pop first
            for (var i = _buffer.Length - 1; i >= 0; i--)
            {
                if (!_isIndexUsed[i])
                    _freeIndices.Push(i);
            }
        }

        /// <summary>
        /// Removes an item from the DArray with tracking the free indices.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        void RemoveAtTracked(int index)
        {
            var item = _buffer[index];

            _buffer[index] = default(T);
            _isIndexUsed[index] = false;
            _freeIndices.Push(index);

            // Notify listeners
            OnItemRemoved(item, index);

            if (ItemRemoved != null)
                ItemRemoved.Raise(this, new DArrayEventArgs<T>(item, index));
        }

        /// <summary>
        /// Removes an item from the DArray without tracking the free indices.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        void RemoveAtUntracked(int index)
        {
            var item = _buffer[index];

            _buffer[index] = default(T);
            _isIndexUsed[index] = false;

            // Notify listeners
            OnItemRemoved(item, index);

            if (ItemRemoved != null)
                ItemRemoved.Raise(this, new DArrayEventArgs<T>(item, index));
        }

        void ResizeBuffer(int size)
        {
            Array.Resize(ref _buffer, size);
            _isIndexUsed.Length = size;
        }

        /// <summary>
        /// Creates an array containing the items in the DArray
        /// </summary>
        /// <returns>Array containing the items in the DArray</returns>
        public T[] ToArray()
        {
            var ret = new T[Length];
            Array.Copy(_buffer, ret, ret.Length);
            return ret;
        }

        /// <summary>
        /// Trims down the size of the internal buffer to reduce memory usage.
        /// </summary>
        public void Trim()
        {
            // Check that we can even trim
            if (_highestIndex >= _buffer.Length)
                return;

            // Size that the new buffer will be
            var newSize = _highestIndex + 1;

            // Resize the buffer
            ResizeBuffer(newSize);

            // If we are tracking the free indices, and we have indices tracked, we will likely
            // end up trimming some of the free indicies. It will be easiest to just rebuild the
            // stack of free indicies.
            if (_trackFree && _freeIndices.Count > 0)
                RebuildFreeIndiciesStack();

            _version++;
        }

        #region IList<T> Members

        /// <summary>
        /// Gets or sets an item to the DArray.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>Item at the given index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index. Use
        /// <see cref="CanGet"/> to make sure the index is valid.</exception>
        public T this[int index]
        {
            get
            {
                // Check for if out of range
                if (index > _highestIndex || index < 0)
                    throw new ArgumentOutOfRangeException("index");

                // Return the item at the index
                return _buffer[index];
            }
            set
            {
                // Check if desired value exceeds the current buffer size - if so, resize
                if (index >= _buffer.Length)
                    ResizeBuffer(Math.Max(_buffer.Length * 2, index + 1));

                // If greater than the highest index, update the new highest index
                if (index > _highestIndex)
                {
                    if (_trackFree)
                    {
                        // Since we created a new value of the last highest value, we will
                        // need to push all indices skipped into the stack of free indices
                        for (var i = _highestIndex + 1; i < index; i++)
                        {
                            _freeIndices.Push(i);
                        }
                    }
                    _highestIndex = index;
                }

                // Set the item and increase the version
                _isIndexUsed[index] = true;
                _buffer[index] = value;
                _version++;

                // Notify any listeners an item has been added
                OnItemAdded(value, index);

                if (ItemAdded != null)
                    ItemAdded.Raise(this, new DArrayEventArgs<T>(value, index));
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the DArray. Unlike Length, this counts the
        /// number of actual items in the DArray, not just the size of the DArray. Therefore, Count
        /// will always be less than or equal to Length.
        /// </summary>
        public int Count
        {
            get
            {
                // FUTURE: Can greatly improve the performance of this by doing the count when adding/removing
                var count = 0;
                for (var i = 0; i <= _highestIndex; i++)
                {
                    if (_isIndexUsed[i])
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the DArray is read-only.
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds an item into the DArray. Use Insert() instead if you want to add an item to the DArray but
        /// also get the index that the item was added at. This operation is significantly faster if TrackFree is set
        /// because the DArray can then use NextFreeIndex().
        /// </summary>
        /// <param name="item">Item to add to the DArray.</param>
        public void Add(T item)
        {
            Insert(item);
        }

        /// <summary>
        /// Clears all the elements and values of the DArray so it can be reused.
        /// </summary>
        public void Clear()
        {
            _version++;
            _highestIndex = -1;
            ResizeBuffer(_defaultSize);

            if (_trackFree)
                _freeIndices = new Stack<int>(_defaultFreeStackSize);
        }

        /// <summary>
        /// Checks if the DArray contains the specified item
        /// </summary>
        /// <param name="item">Item to check for in the DArray</param>
        /// <returns>True if the DArray contains the specified item, else false</returns>
        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            foreach (var element in _buffer)
            {
                if (comparer.Equals(element, item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Copies the elements of the DArray to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied 
        /// from DArray. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>arrayIndex</c> is out of range.</exception>
        /// <exception cref="ArgumentException">May not use a multi-dimensional array</exception>
        /// <exception cref="ArgumentException">Not enough room to fit all the DArray into the Array from the specified arrayIndex</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex must be greater or equal to 0");
            if (array.Rank > 1)
                throw new ArgumentException("May not use a multi-dimensional array", "array");
            if (arrayIndex + array.Length < Count)
                throw new ArgumentException("Not enough room to fit all the DArray into the Array from the specified arrayIndex",
                    "array");

            var j = 0;
            for (var i = 0; i <= _highestIndex; i++)
            {
                if (_isIndexUsed[i])
                {
                    array[arrayIndex + j] = this[i];
                    j++;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A IEnumerator that can be used to iterate through the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A IEnumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Finds the index of an item in the internal array (very slow!)
        /// </summary>
        /// <param name="item">Item to find</param>
        /// <returns>Index of the item if found, or -1 if not found</returns>
        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;

            // Iterate through the whole list
            for (var i = 0; i <= _highestIndex; i++)
            {
                // Check for the requested item, returning the index if it matches
                if (comparer.Equals(_buffer[i], item))
                    return i;
            }

            // Item not found, return -1
            return -1;
        }

        /// <summary>
        /// Inserts an item to the DArray at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the DArray.</param>
        public void Insert(int index, T item)
        {
            this[index] = item;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the DArray.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if item was successfully removed from the DArray; otherwise, false. This method also
        /// returns false if item is not found in the original DArray.</returns>
        public bool Remove(T item)
        {
            var index = IndexOf(item);

            if (!CanGet(index) || !_isIndexUsed[index])
                return false;

            RemoveAt(index);

            return true;
        }

        /// <summary>
        /// Removes the DArray item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            // Check if there is anything to remove
            if (!_isIndexUsed[index])
                return;

            // Call the sub-handler
            if (_trackFree)
                RemoveAtTracked(index);
            else
                RemoveAtUntracked(index);

            // Items changed, so change the version
            _version++;
        }

        #endregion

        /// <summary>
        /// DArray enumerator
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            readonly DArray<T> _items;
            readonly int _version;
            T _current;
            int _index;

            internal Enumerator(DArray<T> source)
            {
                _items = source;
                _version = source._version;
                _index = 0;
                _current = default(T);
            }

            #region IEnumerator<T> Members

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            object IEnumerator.Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>True if the enumerator was successfully advanced to the next element; 
            /// False if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="InvalidOperationException">Enumeration failed - the DArray's contents have changed.</exception>
            [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DArray's")]
            [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DArray")]
            public bool MoveNext()
            {
                // Moved past the highest index
                if (_index > _items._highestIndex)
                    return false;

                // Version has changed
                if (_version != _items._version)
                {
                    const string errmsg = "Enumeration failed - the DArray's contents have changed.";
                    if (log.IsErrorEnabled)
                        log.Error(errmsg);
                    throw new InvalidOperationException(errmsg);
                }

                // Set the current item, and increase the index
                _current = _items[_index];

                // Skip unused items
                if (!_items._isIndexUsed[_index++])
                    return MoveNext();

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            void IEnumerator.Reset()
            {
                _index = 0;
                _current = default(T);
            }

            #endregion
        }
    }
}