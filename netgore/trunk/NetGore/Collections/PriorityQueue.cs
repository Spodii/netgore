using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    public class PriorityQueue<T>
    {
        readonly IComparer<T> _comparer;
        readonly List<T> _list = new List<T>();

        /// <summary>
        /// Sets up a default PriorityQueue.
        /// </summary>
        public PriorityQueue()
        {
            _comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Sets up a PriorityQueue with a specified <see cref="IComparer"/>
        /// </summary>
        public PriorityQueue(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        /// <summary>
        /// Sets up a PriorityQueue with a specified <see cref="IComparer"/> and an initial capacity.
        /// </summary>
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
            T Result = _list[0];
            int A, B, C, D;
            A = 0;

            _list[0] = _list[_list.Count - 1];
            _list.RemoveAt(_list.Count - 1);

            do
            {
                D = A;
                B = 2 * A + 1;
                C = 2 * A + 2;

                if (_list.Count > B && OnCompare(A, B) > 0)
                    A = B;

                if (_list.Count > C && OnCompare(A, C) > 0)
                    A = C;

                if (A == D)
                    break;

                Switch(A, D);
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
            int A = _list.Count;
            int B;

            _list.Add(Object);

            do
            {
                if (A == 0)
                    break;

                B = (int)((A - 1) * 0.5f);

                if (OnCompare(A, B) < 0)
                {
                    Switch(A, B);
                    A = B;
                }
                else
                    break;
            }
            while (true);

            return A;
        }

        /// <summary>
        /// Removes an object from the queue.
        /// </summary>
        /// <param name="Object">The object to remove.</param>
        public void RemoveLocation(T Object)
        {
            int idx = -1;

            for (int i = 0; i < _list.Count; i++)
            {
                if (_comparer.Compare(_list[i], Object) == 0)
                    idx = i;
            }

            if (idx != -1)
                _list.RemoveAt(idx);
        }

        void Switch(int A, int B)
        {
            T C = _list[A];
            _list[A] = _list[B];
            _list[B] = C;
        }

        /// <summary>
        /// Notifies the queue that the object at position i has changed and needs to update.
        /// </summary>
        /// <param name="i">Index that has changed.</param>
        public void Update(int i)
        {
            int A, B, C, D;
            A = i;

            do
            {
                // Index at top of queue.
                if (A == 0)
                    break;

                // Half of (index -1)
                D = ((A - 1) / 2);

                // If A is less than D then we switch the positions
                if (OnCompare(A, D) < 0)
                {
                    Switch(A, D);
                    A = D;
                }
                else
                    break;
            }
            while (true);

            if (A < i)
                return;

            do
            {
                B = A;

                C = 2 * A + 1;
                D = 2 * A + 2;

                // If C is less than _list.Count and if A is greater than C then A = C.
                if (_list.Count > C && OnCompare(A, C) > 0)
                    A = C;

                // If D is less than _list.Count and if A is greater than D then A = D.
                if (_list.Count > D && OnCompare(A, D) > 0)
                    A = D;

                // If A is equal to B then we've finished moving the elements.
                if (A == B)
                    break;

                // Switch the element A to position B and vice versa.
                Switch(A, B);
            }
            while (true);
        }
    }
}