using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A semi-sorted queue that holds it's highest value at the head of the array at all times.
    /// </summary>
    public class PriorityQueue<T>
    {
        readonly IComparer<T> _comparer;
        readonly SlimList<T> _list = new SlimList<T>();

        /// <summary>
        /// Sets up a default PriorityQueue.
        /// </summary>
        public PriorityQueue()
        {
            _comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Sets up a PriorityQueue with a specified <see cref="IComparer{T}"/>
        /// </summary>
        public PriorityQueue(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        /// <summary>
        /// Sets up a PriorityQueue with a specified <see cref="IComparer{T}"/> and an initial capacity.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> used to compare elements.</param>
        /// <param name="capacity">Inital capacity of queue.</param>
        public PriorityQueue(IComparer<T> comparer, int capacity)
        {
            _list.Capacity = capacity;
            _comparer = comparer;
        }

        /// <summary>
        /// Used for getting and setting values in the queue.
        /// </summary>
        /// <param name="idx">Index in queue.</param>
        public T this[int idx]
        {
            get { return _list[idx]; }
            set
            {
                _list[idx] = value;
                Update(idx);
            }
        }

        /// <summary>
        /// Returns the number of elements in the queue.
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Clears the queue.
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        int OnCompare(int A, int B)
        {
            return _comparer.Compare(_list[A], _list[B]);
        }

        /// <summary>
        /// Gets the smallest object without removing it from the queue.
        /// </summary>
        /// <returns>The smallest object in the queue.</returns>
        public T Peek()
        {
            if (_list.Count > 0)
                return _list[0];
            return default(T);
        }

        /// <summary>
        /// Gets the smallest object and removes it from the queue.
        /// </summary>
        /// <returns>The smallest object.</returns>
        public T Pop()
        {
            var Result = _list[0];

            var a = 0;

            _list[0] = _list[_list.Count - 1];
            _list.RemoveAt(_list.Count - 1);

            do
            {
                var d = a;
                var b = (a << 1) + 1;
                var c = (a << 1) + 2;

                if (_list.Count > b && OnCompare(a, b) > 0)
                    a = b;

                if (_list.Count > c && OnCompare(a, c) > 0)
                    a = c;

                if (a == d)
                    break;

                Switch(a, d);
            }
            while (true);

            return Result;
        }

        /// <summary>
        /// Pushes an object onto the queue.
        /// </summary>
        /// <param name="Object">The object to be put onto the queue.</param>
        /// <returns>The position of the Object when it is pushed onto the queue.</returns>
        public int Push(T Object)
        {
            var a = _list.Count;

            _list.Add(Object);

            do
            {
                if (a == 0)
                    break;

                var b = (int)((a - 1) * 0.5f);

                if (OnCompare(a, b) >= 0)
                    break;

                Switch(a, b);
                a = b;
            }
            while (true);

            return a;
        }

        /// <summary>
        /// Removes an object from the queue.
        /// </summary>
        /// <param name="Object">The object to remove.</param>
        public void RemoveLocation(T Object)
        {
            var idx = -1;

            for (var i = 0; i < _list.Count; i++)
            {
                if (_comparer.Compare(_list[i], Object) == 0)
                    idx = i;
            }

            if (idx != -1)
                _list.RemoveAt(idx);
        }

        void Switch(int A, int B)
        {
            var C = _list[A];
            _list[A] = _list[B];
            _list[B] = C;
        }

        /// <summary>
        /// Notifies the queue that the object at position i has changed and needs to update.
        /// </summary>
        /// <param name="i">Index that has changed.</param>
        public void Update(int i)
        {
            int d;
            var a = i;

            do
            {
                // Index at top of queue.
                if (a == 0)
                    break;

                // Half of (index -1)
                d = ((a - 1) >> 1);

                // If A is less than D then we switch the positions
                if (OnCompare(a, d) >= 0)
                    break;

                Switch(a, d);
                a = d;
            }
            while (true);

            if (a < i)
                return;

            do
            {
                var b = a;

                var c = (a << 1) + 1;
                d = (a << 1) + 2;

                // If C is less than _list.Count and if A is greater than C then A = C.
                if (_list.Count > c && OnCompare(a, c) > 0)
                    a = c;

                // If D is less than _list.Count and if A is greater than D then A = D.
                if (_list.Count > d && OnCompare(a, d) > 0)
                    a = d;

                // If A is equal to B then we've finished moving the elements.
                if (a == b)
                    break;

                // Switch the element A to position B and vice versa.
                Switch(a, b);
            }
            while (true);
        }
    }
}