using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NetGore.Collections
{
    /// <summary>
    /// A thread-safe implementation of a List.
    /// </summary>
    /// <typeparam name="T">Type of object to store.</typeparam>
    public class TSList<T> : IList<T>
    {
        readonly List<T> _list;
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public int Capacity
        {
            get { return _list.Capacity; }
        }

        public TSList()
        {
            _list = new List<T>();
        }

        public TSList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        public TSList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            _lock.EnterWriteLock();
            _list.AddRange(collection);
            _lock.ExitWriteLock();
        }

        public int BinarySearch(T item)
        {
            _lock.EnterReadLock();
            int ret = _list.BinarySearch(item);
            _lock.ExitReadLock();

            return ret;
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            _lock.EnterReadLock();
            int ret = _list.BinarySearch(item, comparer);
            _lock.ExitReadLock();

            return ret;
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            _lock.EnterReadLock();
            int ret = _list.BinarySearch(index, count, item, comparer);
            _lock.ExitReadLock();

            return ret;
        }

        public TSList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            _lock.EnterReadLock();
            var ret = new TSList<TOutput>(_list.ConvertAll(converter));
            _lock.ExitReadLock();

            return ret;
        }

        public bool Exists(Predicate<T> match)
        {
            _lock.EnterReadLock();
            bool ret = _list.Exists(match);
            _lock.ExitReadLock();

            return ret;
        }

        public T Find(Predicate<T> match)
        {
            _lock.EnterReadLock();
            T ret = _list.Find(match);
            _lock.ExitReadLock();

            return ret;
        }

        public List<T> FindAll(Predicate<T> match)
        {
            _lock.EnterReadLock();
            var ret = _list.FindAll(match);
            _lock.ExitReadLock();

            return ret;
        }

        public T FindLast(Predicate<T> match)
        {
            _lock.EnterReadLock();
            T ret = _list.FindLast(match);
            _lock.ExitReadLock();

            return ret;
        }

        public int FindLastIndex(Predicate<T> match)
        {
            _lock.EnterReadLock();
            int ret = _list.FindLastIndex(match);
            _lock.ExitReadLock();

            return ret;
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            _lock.EnterReadLock();
            int ret = _list.FindLastIndex(startIndex, match);
            _lock.ExitReadLock();

            return ret;
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            _lock.EnterReadLock();
            int ret = _list.FindLastIndex(startIndex, count, match);
            _lock.ExitReadLock();

            return ret;
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            _lock.EnterWriteLock();
            _list.InsertRange(index, collection);
            _lock.ExitWriteLock();
        }

        public void RemoveAll(Predicate<T> match)
        {
            _lock.EnterWriteLock();
            _list.RemoveAll(match);
            _lock.ExitWriteLock();
        }

        public void RemoveRange(int index, int count)
        {
            _lock.EnterWriteLock();
            _list.RemoveRange(index, count);
            _lock.ExitWriteLock();
        }

        public void Reverse()
        {
            _lock.EnterWriteLock();
            _list.Reverse();
            _lock.ExitWriteLock();
        }

        public void Sort()
        {
            _lock.EnterWriteLock();
            _list.Sort();
            _lock.ExitWriteLock();
        }

        public void Sort(Comparison<T> comparison)
        {
            _lock.EnterWriteLock();
            _list.Sort(comparison);
            _lock.ExitWriteLock();
        }

        public void Sort(IComparer<T> comparer)
        {
            _lock.EnterWriteLock();
            _list.Sort(comparer);
            _lock.ExitWriteLock();
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            _lock.EnterWriteLock();
            _list.Sort(index, count, comparer);
            _lock.ExitWriteLock();
        }

        public T[] ToArray()
        {
            _lock.EnterReadLock();
            var ret = _list.ToArray();
            _lock.ExitReadLock();

            return ret;
        }

        public void TrimExcess()
        {
            _lock.EnterWriteLock();
            _list.TrimExcess();
            _lock.ExitWriteLock();
        }

        public bool TrueForAll(Predicate<T> match)
        {
            _lock.EnterWriteLock();
            bool ret = _list.TrueForAll(match);
            _lock.ExitWriteLock();

            return ret;
        }

        #region IList<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _lock.EnterWriteLock();
            _list.Add(item);
            _lock.ExitWriteLock();
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            _list.Clear();
            _lock.ExitWriteLock();
        }

        public bool Contains(T item)
        {
            _lock.EnterReadLock();
            bool ret = _list.Contains(item);
            _lock.ExitReadLock();

            return ret;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _lock.EnterReadLock();
            _list.CopyTo(array, arrayIndex);
            _lock.ExitReadLock();
        }

        public bool Remove(T item)
        {
            _lock.EnterWriteLock();
            bool ret = _list.Remove(item);
            _lock.ExitWriteLock();

            return ret;
        }

        public int Count
        {
            get { return _list.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)_list).IsReadOnly; }
        }

        public int IndexOf(T item)
        {
            _lock.EnterReadLock();
            int ret = _list.IndexOf(item);
            _lock.ExitReadLock();

            return ret;
        }

        public void Insert(int index, T item)
        {
            _lock.EnterWriteLock();
            _list.Insert(index, item);
            _lock.ExitWriteLock();
        }

        public void RemoveAt(int index)
        {
            _lock.EnterWriteLock();
            _list.RemoveAt(index);
            _lock.ExitWriteLock();
        }

        public T this[int index]
        {
            get
            {
                _lock.EnterReadLock();
                T ret = _list[index];
                _lock.ExitReadLock();

                return ret;
            }
            set
            {
                _lock.EnterWriteLock();
                _list[index] = value;
                _lock.ExitWriteLock();
            }
        }

        #endregion

        struct Enumerator : IEnumerator<T>
        {
            readonly IEnumerator _baseEnumerator;
            readonly ReaderWriterLockSlim _lock;

            public Enumerator(TSList<T> list)
            {
                _lock = list._lock;
                _lock.EnterReadLock();
                _baseEnumerator = list._list.GetEnumerator();
            }

            #region IEnumerator<T> Members

            public void Dispose()
            {
                _lock.ExitReadLock();
            }

            public bool MoveNext()
            {
                return _baseEnumerator.MoveNext();
            }

            public void Reset()
            {
                _baseEnumerator.Reset();
            }

            public T Current
            {
                get { return ((IEnumerator<T>)_baseEnumerator).Current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion
        }
    }
}