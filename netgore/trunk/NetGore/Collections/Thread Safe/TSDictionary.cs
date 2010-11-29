using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        readonly object _threadSync = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="TSDictionary{TKey, TValue}"/> class.
        /// </summary>
        public TSDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial capacity.</param>
        public TSDictionary(int capacity)
        {
            _dict = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The key comparer to use.</param>
        public TSDictionary(IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">A <see cref="IDictionary{TKey, TValue}"/> to copy the values from.</param>
        public TSDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dict = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial capacity.</param>
        /// <param name="comparer">The key comparer to use.</param>
        public TSDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">A <see cref="IDictionary{TKey, TValue}"/> to copy the values from.</param>
        /// <param name="comparer">The key comparer to use.</param>
        public TSDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element by using a specified
        /// <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>
        /// True if the source sequence contains an element that has the specified value; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> value, IEqualityComparer<KeyValuePair<TKey, TValue>> comparer)
        {
            lock (_threadSync)
                return _dict.Contains(value, comparer);
        }

        /// <summary>
        /// Determines whether the <see cref="TSDictionary{TKey,TValue}"/> contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="TSDictionary{TKey,TValue}"/>.
        /// The value can be null for reference types.</param>
        /// <returns>True if the <see cref="TSDictionary{TKey,TValue}"/> contains an element with the specified value;
        /// otherwise, false.</returns>
        public bool ContainsValue(TValue value)
        {
            lock (_threadSync)
            {
                return _dict.ContainsValue(value);
            }
        }

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Gets or sets the <see cref="TValue"/> with the specified key.
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                lock (_threadSync)
                {
                    return _dict[key];
                }
            }
            set
            {
                lock (_threadSync)
                {
                    _dict[key] = value;
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        public int Count
        {
            get
            {
                lock (_threadSync)
                {
                    return _dict.Count;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only;
        /// otherwise, false.</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that
        /// implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
        public ICollection<TKey> Keys
        {
            get
            {
                lock (_threadSync)
                {
                    return new List<TKey>(_dict.Keys);
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the
        /// object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
        public ICollection<TValue> Values
        {
            get
            {
                lock (_threadSync)
                {
                    return new List<TValue>(_dict.Values);
                }
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            lock (_threadSync)
            {
                _dict.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/>
        /// is read-only.</exception>
        public void Add(TKey key, TValue value)
        {
            lock (_threadSync)
            {
                _dict.Add(key, value);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only. </exception>
        public void Clear()
        {
            lock (_threadSync)
            {
                _dict.Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>;
        /// otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_threadSync)
            {
                return _dict.Contains(item);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the
        /// specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key;
        /// otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            lock (_threadSync)
            {
                return _dict.ContainsKey(key);
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an
        /// <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements
        /// copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/>
        /// must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than
        /// 	the length of <paramref name="array"/>.-or-The number of elements in the source
        /// 	<see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from
        /// 	<paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-The type
        /// 	cannot be cast automatically to the type of the destination
        /// 	<paramref name="array"/>.</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_threadSync)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)_dict).CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (_threadSync)
            {
                return this.ToImmutable().GetEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false
        /// if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_threadSync)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)_dict).Remove(item);
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if
        /// <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/>
        /// is read-only.</exception>
        public bool Remove(TKey key)
        {
            lock (_threadSync)
            {
                return _dict.Remove(key);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter
        /// is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>
        /// contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_threadSync)
            {
                return _dict.TryGetValue(key, out value);
            }
        }

        #endregion
    }
}