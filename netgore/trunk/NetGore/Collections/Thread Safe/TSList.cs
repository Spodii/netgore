using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="TSList{T}"/> class.
        /// </summary>
        public TSList()
        {
            _list = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSList{T}"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public TSList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TSList{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public TSList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }

        /// <summary>
        /// Gets the number of elements actually contained in the <see cref="TSList{T}"/>.
        /// </summary>
        public int Capacity
        {
            get { return _list.Capacity; }
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="TSList{T}"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the
        /// <see cref="TSList{T}"/>. The collection itself cannot be null, but it can contain elements that are null,
        /// if type <typeparamref name="T"/> is a reference type.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.AddRange(collection);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Searches the entire sorted <see cref="TSList{T}"/> for an element using the default comparer and
        /// returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <returns>The zero-based index of item in the sorted <see cref="TSList{T}"/>, if item is found;
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger
        /// than item or, if there is no larger element, the bitwise complement of <see cref="TSList{T}.Count"/>.</returns>
        public int BinarySearch(T item)
        {
            int ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.BinarySearch(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Searches the entire sorted <see cref="TSList{T}"/> for an element using the default comparer and
        /// returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements,
        /// or null to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>
        /// The zero-based index of item in the sorted <see cref="TSList{T}"/>, if item is found;
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger
        /// than item or, if there is no larger element, the bitwise complement of <see cref="TSList{T}.Count"/>.
        /// </returns>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            int ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.BinarySearch(item, comparer);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Searches the entire sorted <see cref="TSList{T}"/> for an element using the default comparer and
        /// returns the zero-based index of the element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements,
        /// or null to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>
        /// The zero-based index of item in the sorted <see cref="TSList{T}"/>, if item is found;
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger
        /// than item or, if there is no larger element, the bitwise complement of <see cref="TSList{T}.Count"/>.
        /// </returns>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            int ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.BinarySearch(index, count, item, comparer);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Converts the elements in the current <see cref="List{T}"/> to another type, and returns a list containing the
        /// converted elements.
        /// </summary>
        /// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
        /// <param name="converter">A <see cref="Converter{TInput,TOutput}"/> delegate that converts each element from
        /// one type to another type.</param>
        /// <returns>A <see cref="TSList{T}"/> of the target type containing the converted elements from the current
        /// <see cref="TSList{T}"/>.</returns>
        public TSList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            TSList<TOutput> ret;

            _lock.EnterReadLock();
            try
            {
                ret = new TSList<TOutput>(_list.ConvertAll(converter));
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Determines whether the <see cref="TSList{T}"/> contains elements that match the conditions defined by the
        /// specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements
        /// to search for.</param>
        /// <returns>True if the <see cref="TSList{T}"/> contains one or more elements that match the conditions defined
        /// by the specified predicate; otherwise, false.</returns>
        public bool Exists(Predicate<T> match)
        {
            bool ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.Exists(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first
        /// occurrence within the entire <see cref="TSList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to
        /// search for.</param>
        /// <returns>The first element that matches the conditions defined by the specified predicate, if found;
        /// otherwise, the default value for type <typeparamref name="T"/>.</returns>
        public T Find(Predicate<T> match)
        {
            T ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.Find(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements to
        /// search for.</param>
        /// <returns>A <see cref="List{T}"/> containing all the elements that match the conditions defined by the
        /// specified predicate, if found; otherwise, an empty <see cref="List{T}"/>.</returns>
        public List<T> FindAll(Predicate<T> match)
        {
            List<T> ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.FindAll(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the last
        /// occurrence within the entire <see cref="TSList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to
        /// search for.</param>
        /// <returns>The last element that matches the conditions defined by the specified predicate, if found;
        /// otherwise, the default value for type <typeparamref name="T"/>.</returns>
        public T FindLast(Predicate<T> match)
        {
            T ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.FindLast(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the
        /// zero-based index of the last occurrence within the entire <see cref="TSList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to
        /// search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match,
        /// if found; otherwise, –1.</returns>
        public int FindLastIndex(Predicate<T> match)
        {
            int ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.FindLastIndex(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the
        /// zero-based index of the last occurrence within the entire <see cref="TSList{T}"/>.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to
        /// search for.</param>
        /// <returns>
        /// The zero-based index of the last occurrence of an element that matches the conditions defined by match,
        /// if found; otherwise, –1.
        /// </returns>
        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            int ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.FindLastIndex(startIndex, match);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the
        /// zero-based index of the last occurrence within the entire <see cref="TSList{T}"/>.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to
        /// search for.</param>
        /// <returns>
        /// The zero-based index of the last occurrence of an element that matches the conditions defined by match,
        /// if found; otherwise, –1.
        /// </returns>
        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            int ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.FindLastIndex(startIndex, count, match);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Inserts the elements of a collection into the <see cref="TSList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the <see cref="TSList{T}"/>.
        /// The collection itself cannot be null, but it can contain elements that are null,
        /// if type <typeparamref name="T"/> is a reference type.</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.InsertRange(index, collection);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements
        /// to remove.</param>
        public void RemoveAll(Predicate<T> match)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.RemoveAll(match);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes a range of elements from the <see cref="TSList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void RemoveRange(int index, int count)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.RemoveRange(index, count);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Reverses the order of the elements in the entire <see cref="TSList{T}"/>.
        /// </summary>
        public void Reverse()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Reverse();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="TSList{T}"/> using the default comparer.
        /// </summary>
        public void Sort()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Sort();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="TSList{T}"/> using the specified <see cref="Comparison{T}"/>.
        /// </summary>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements</param>
        public void Sort(Comparison<T> comparison)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Sort(comparison);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="TSList{T}"/> using the specified comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements,
        /// or null to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        public void Sort(IComparer<T> comparer)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Sort(comparer);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="TSList{T}"/> using the specified comparer.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements,
        /// or null to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Sort(index, count, comparer);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="TSList{T}"/> to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the <see cref="TSList{T}"/>.</returns>
        public T[] ToArray()
        {
            T[] ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="TSList{T}"/>, if that number is less than a
        /// threshold value.
        /// </summary>
        public void TrimExcess()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.TrimExcess();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether every element in the <see cref="TSList{T}"/> matches the conditions defined by the
        /// specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions to check against
        /// the elements.</param>
        /// <returns>True if every element in the <see cref="TSList{T}"/> matches the conditions defined by the specified
        /// predicate; otherwise, false. If the list has no elements, the return value is true.</returns>
        public bool TrueForAll(Predicate<T> match)
        {
            bool ret;

            _lock.EnterWriteLock();
            try
            {
                ret = _list.TrueForAll(match);
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            return ret;
        }

        #region IList<T> Members

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                T ret;

                _lock.EnterReadLock();
                try
                {
                    ret = _list[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }

                return ret;
            }
            set
            {
                _lock.EnterWriteLock();
                try
                {
                    _list[index] = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise,
        /// false.</returns>
        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)_list).IsReadOnly; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        public void Add(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.</exception>
        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
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
        public bool Contains(T item)
        {
            bool ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an
        /// <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the
        /// elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/>
        /// must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or
        /// 	greater than the length of <paramref name="array"/>.-or-The number of elements in the source
        /// 	<see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from
        /// 	<paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type
        /// 	<typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// 	</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _lock.EnterReadLock();
            try
            {
                _list.CopyTo(array, arrayIndex);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<T> ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret.GetEnumerator();
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
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            int ret;

            _lock.EnterReadLock();
            try
            {
                ret = _list.IndexOf(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return ret;
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the
        /// 	<see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/>
        /// is read-only.</exception>
        public void Insert(int index, T item)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Insert(index, item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if
        /// <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is
        /// read-only.</exception>
        public bool Remove(T item)
        {
            bool ret;

            _lock.EnterWriteLock();
            try
            {
                ret = _list.Remove(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            return ret;
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the
        /// 	<see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/>
        /// is read-only.</exception>
        public void RemoveAt(int index)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.RemoveAt(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        #endregion
    }
}