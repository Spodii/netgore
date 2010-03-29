using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Collections
{
    public class SlimList<T>
    {
        private T[] _items;
        private short _size;
        private static T[] _emptyArray = null;

        /// <summary>
        /// Constructs a new list with null array.
        /// </summary>
        public SlimList()
        {
            _items = _emptyArray;
        }

        /// <summary>
        /// Appends an object T to the end of the array.
        /// </summary>
        /// <param name="item">The object to append to the array.</param>
        public void Add(T item)
        {
            if (_size == _items.Length)
            {
                EnsureCapacity((short)(_size+1));
            }
            _items[_size++] = item;
        }

        /// <summary>
        /// Clears the array and resets the capacity to 0.
        /// </summary>
        public void Clear()
        {
            if (this._size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
        }

        /// <summary>
        /// Makes sure that the cpacity of the array is at least the set minimum.
        /// </summary>
        /// <param name="min">The minimum size.</param>
        private void EnsureCapacity(short min)
        {
            if (_items.Length < min)
            {
                int num = (_items.Length == 0) ? 4 : (_items.Length * 2);

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
            if (index >= this._size)
                return;
            
            _size--;

            if (index < _size)
            {
                Array.Copy(this._items, index + 1, this._items, index, this._size - index);
            }

            _items[_size] = default(T);
        }


        /// <summary>
        /// The length of the array.
        /// </summary>
        public int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value == _items.Length)
                    return;

                if (value < _size)
                    return;

                if (value > 0)
                {
                    T[] destArray = new T[value];
                    Array.Copy(_items, 0, destArray, 0, _size);
                    _items = destArray;
                }
                else
                {
                    _items = _emptyArray;
                }
            }
        }

        /// <summary>
        /// Index of the array.
        /// </summary>
        public T this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (!(value is T) && ((value != null) || typeof(T).IsValueType))
                    return;

                this[index] = (T)value;
            }
        }

        /// <summary>
        /// Size of the array.
        /// </summary>
        public int Count
        {
            get
            {
                return _size;
            }
        }







    }
}
