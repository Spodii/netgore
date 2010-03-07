using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Collections
{
    public class PriorityQueue<T>
    {
        List<T> _list = new List<T>();
        IComparer<T> _comparer;

        public PriorityQueue()
        {
            _comparer = Comparer<T>.Default;
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public PriorityQueue(IComparer<T> comparer, int capacity)
        {
            _list.Capacity = capacity;
            _comparer = comparer;
        }

        void Switch(int A, int B)
        {
            T C = _list[A];
            _list[A] = _list[B];
            _list[B] = C;
        }

        public T Peek()
        {

            if (_list.Count > 0)
                return _list[0];
            return default(T);
        }
        
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
                {
                    A = B;
                }

                if (_list.Count > C && OnCompare(A, C) > 0)
                {
                    A = C;
                }

                if (A == D)
                    break;

                Switch(A, D);
            } while (true);

            return Result;


        }
        
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
                {
                    break;
                }
            } while (true);

            return A;
        }
       
        public void RemoveLocation(T Object)
        {
            int idx = -1;

            for (int i = 0; i < _list.Count; i++)
            {

                if (_comparer.Compare(_list[i], Object) == 0)
                {
                    idx = i;
                }

            }

            if (idx != -1)
                _list.RemoveAt(idx);
            }

        public void Update(int i)
        {
            int A, B, C, D;
            A = i;

            do
            {
                if (A == 0)
                    break;

                D = (int)((A - 1) * 0.5f);

                if (OnCompare(A, D) < 0)
                {
                    Switch(A, D);
                    A = D;
                }
                else
                {
                    break;
                }
            } while (true);

            if (A < i)
                return;

            do
            {
                B = A;

                C = 2 * A + 1;
                D = 2 * A + 2;

                if (_list.Count > C && OnCompare(A, C) > 0)
                {
                    A = C;
                }

                if (_list.Count > D && OnCompare(A, D) > 0)
                {
                    A = D;
                }

                if (A == B)
                    break;

                Switch(A, B);
            } while (true);


        }

        public void Clear()
        {
            _list.Clear();
        }

        public int Count
        {
            get { return _list.Count; }
        }

        private int OnCompare(int A, int B)
        {
            return _comparer.Compare(_list[A], _list[B]);
        }

        public T this[int idx]
        {
            get 
            { 
                return _list[idx]; 
            }
            set
            {
                _list[idx] = value;
                Update(idx);
            }
        }

    }
}
