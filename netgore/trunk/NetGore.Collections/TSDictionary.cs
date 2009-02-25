using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NetGore.Collections
{
    /// <summary>
    /// A thread-safe implementation of a Dictionary.
    /// </summary>
    /// <typeparam name="TKey">Type of the key to use.</typeparam>
    /// <typeparam name="TValue">Type of the value to store.</typeparam>
    public class TSDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        readonly Dictionary<TKey, TValue> _dict;
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public TSDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        public TSDictionary(int capacity)
        {
            _dict = new Dictionary<TKey, TValue>(capacity);
        }

        public TSDictionary(IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(comparer);
        }

        public TSDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dict = new Dictionary<TKey, TValue>(dictionary);
        }

        public TSDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public TSDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item, IEqualityComparer<KeyValuePair<TKey, TValue>> comparer)
        {
            _lock.EnterReadLock();
            bool ret = _dict.Contains(item, comparer);
            _lock.ExitReadLock();

            return ret;
        }

        public bool ContainsValue(TValue value)
        {
            _lock.EnterReadLock();
            bool ret = _dict.ContainsValue(value);
            _lock.ExitReadLock();

            return ret;
        }

        #region IDictionary<TKey,TValue> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            _lock.EnterWriteLock();
            _dict.Add(item.Key, item.Value);
            _lock.ExitWriteLock();
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            _dict.Clear();
            _lock.ExitWriteLock();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            _lock.EnterReadLock();
            bool ret = _dict.Contains(item);
            _lock.ExitReadLock();

            return ret;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _lock.EnterWriteLock();
            ((ICollection<KeyValuePair<TKey, TValue>>)_dict).CopyTo(array, arrayIndex);
            _lock.ExitWriteLock();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            _lock.EnterWriteLock();
            bool ret = ((ICollection<KeyValuePair<TKey, TValue>>)_dict).Remove(item);
            _lock.ExitWriteLock();

            return ret;
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)_dict).IsReadOnly; }
        }

        public bool ContainsKey(TKey key)
        {
            _lock.EnterReadLock();
            bool ret = _dict.ContainsKey(key);
            _lock.ExitReadLock();

            return ret;
        }

        public void Add(TKey key, TValue value)
        {
            _lock.EnterWriteLock();
            _dict.Add(key, value);
            _lock.ExitWriteLock();
        }

        public bool Remove(TKey key)
        {
            _lock.EnterWriteLock();
            bool ret = _dict.Remove(key);
            _lock.ExitWriteLock();

            return ret;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            _lock.EnterReadLock();
            bool ret = _dict.TryGetValue(key, out value);
            _lock.ExitReadLock();

            return ret;
        }

        public TValue this[TKey key]
        {
            get
            {
                _lock.EnterReadLock();
                TValue ret = _dict[key];
                _lock.ExitReadLock();

                return ret;
            }
            set
            {
                _lock.EnterWriteLock();
                _dict[key] = value;
                _lock.ExitWriteLock();
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                _lock.EnterReadLock();
                var ret = new List<TKey>(_dict.Keys); // TODO: Use TSList<,>
                _lock.ExitReadLock();

                return ret;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                _lock.EnterReadLock();
                var ret = new List<TValue>(_dict.Values); // TODO: Use TSList<,>
                _lock.ExitReadLock();

                return ret;
            }
        }

        #endregion

        struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            readonly IEnumerator _baseEnumerator;
            readonly ReaderWriterLockSlim _lock;

            public Enumerator(TSDictionary<TKey, TValue> dict)
            {
                _lock = dict._lock;
                _lock.EnterReadLock();
                _baseEnumerator = dict._dict.GetEnumerator();
            }

            #region IEnumerator<KeyValuePair<TKey,TValue>> Members

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

            public KeyValuePair<TKey, TValue> Current
            {
                get { return ((IEnumerator<KeyValuePair<TKey, TValue>>)_baseEnumerator).Current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion
        }
    }
}