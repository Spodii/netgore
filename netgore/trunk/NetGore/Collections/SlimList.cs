using System;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A list with limited functionality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class SlimList<T>
    {
        static readonly T[] _emptyArray = new T[0];

        T[] _items;
        short _size;

        /// <summary>
        /// Constructs a new list with null array.
        /// </summary>
        public SlimList()
        {
            _items = _emptyArray;
        }

        /// <summary>
        /// Index of the array.
        /// </summary>
        public T this[int index]
        {
            get { return _items[index]; }
            set
            {
                EnsureCapacity(index + 1);

                _items[index] = value;
            }
        }

        /// <summary>
        /// The length of the array.
        /// </summary>
        public int Capacity
        {
            get { return _items.Length; }
            set
            {
                if (value == _items.Length)
                    return;

                if (value < _size)
                    return;

                if (value > 0)
                {
                    var destArray = new T[value];
                    Array.Copy(_items, 0, destArray, 0, _size);
                    _items = destArray;
                }
                else
                    _items = _emptyArray;
            }
        }

        /// <summary>
        /// Size of the array.
        /// </summary>
        public int Count
        {
            get { return _size; }
        }

        /// <summary>
        /// Appends an object T to the end of the array.
        /// </summary>
        /// <param name="item">The object to append to the array.</param>
        public void Add(T item)
        {
            if (_size == _items.Length)
                EnsureCapacity((short)(_size + 1));

            _items[_size++] = item;
        }

        /// <summary>
        /// Clears the array and resets the capacity to 0.
        /// </summary>
        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
        }

        /// <summary>
        /// Makes sure that the capacity of the array is at least the set minimum.
        /// </summary>
        /// <param name="min">The minimum size.</param>
        void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                var num = (_items.Length == 0) ? 4 : (_items.Length * 2);

                if (num < min)
                    num = min;

                Capacity = num;
            }
        }

        /// <summary>
        /// Remove an object at a specific position in the array.
        /// </summary>
        /// <param name="index">The position to remove at.</param>
        public void RemoveAt(int index)
        {
            if (index >= _size)
                return;

            _size--;

            if (index < _size)
                Array.Copy(_items, index + 1, _items, index, _size - index);

            _items[_size] = default(T);
        }
    }
}