using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Turns a regular array into an immutable one while still allowing for array-style accessing.
    /// </summary>
    public class ImmutableArray<T> : ICollection<T>
    {
        readonly T[] _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}"/> class.
        /// </summary>
        /// <param name="source">The source array to make an immutable array from.</param>
        public ImmutableArray(T[] source)
        {
            _source = source;
        }

        /// <summary>
        /// Gets the element at the given index.
        /// </summary>
        /// <param name="index">The 0-based index of the element to get.</param>
        /// <returns>The element at the given index.</returns>
        public T this[int index]
        {
            get { return _source[index]; }
        }

        /// <summary>
        /// Gets a 32-bit integer that represents the total number of elements in all the dimensions of the System.Array.
        /// </summary>
        public int Length
        {
            get { return _source.Length; }
        }

        /// <summary>
        /// Copies all the elements of the current one-dimensional System.Array to the specified one-dimensional
        /// System.Array starting at the specified destination System.Array index.
        /// </summary>
        /// <param name="array">array: The one-dimensional System.Array that is the destination of the
        /// elements copied from the current System.Array.</param>
        /// <param name="index">A 32-bit integer that represents the index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than the lower bound of array.</exception>
        /// <exception cref="System.ArgumentException">array is multidimensional.
        /// -or- index is equal to or greater than the length of array and the source System.Array has a length greater than 0.
        /// -or- The number of elements in the source System.Array is greater than the available space from index to the end of
        /// the destination array.</exception>
        /// <exception cref="System.ArrayTypeMismatchException">The type of the source System.Array cannot be cast
        /// automatically to the type of the destination array.</exception>
        /// <exception cref="System.RankException">The source System.Array is multidimensional.</exception>
        /// <exception cref="System.InvalidCastException">At least one element in the source System.Array
        /// cannot be cast to the type of destination array.</exception>
        public void CopyTo(Array array, int index)
        {
            _source.CopyTo(array, index);
        }

        /// <summary>
        /// Copies all the elements of the current one-dimensional System.Array to the specified one-dimensional
        /// System.Array starting at the specified destination System.Array index.
        /// </summary>
        /// <param name="array">array: The one-dimensional System.Array that is the destination of the
        /// elements copied from the current System.Array.</param>
        /// <param name="index">A 64-bit integer that represents the index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than the lower bound of array.</exception>
        /// <exception cref="System.ArgumentException">array is multidimensional.
        /// -or- index is equal to or greater than the length of array and the source System.Array has a length greater than 0.
        /// -or- The number of elements in the source System.Array is greater than the available space from index to the end of
        /// the destination array.</exception>
        /// <exception cref="System.ArrayTypeMismatchException">The type of the source System.Array cannot be cast
        /// automatically to the type of the destination array.</exception>
        /// <exception cref="System.RankException">The source System.Array is multidimensional.</exception>
        /// <exception cref="System.InvalidCastException">At least one element in the source System.Array
        /// cannot be cast to the type of destination array.</exception>
        public void CopyTo(Array array, long index)
        {
            _source.CopyTo(array, index);
        }

        /// <summary>
        /// Copies all the elements of the current one-dimensional System.Array to the specified one-dimensional
        /// System.Array starting at the specified destination System.Array index.
        /// </summary>
        /// <param name="array">array: The one-dimensional System.Array that is the destination of the
        /// elements copied from the current System.Array.</param>
        /// <param name="index">A 64-bit integer that represents the index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than the lower bound of array.</exception>
        /// <exception cref="System.ArgumentException">array is multidimensional.
        /// -or- index is equal to or greater than the length of array and the source System.Array has a length greater than 0.
        /// -or- The number of elements in the source System.Array is greater than the available space from index to the end of
        /// the destination array.</exception>
        /// <exception cref="System.ArrayTypeMismatchException">The type of the source System.Array cannot be cast
        /// automatically to the type of the destination array.</exception>
        /// <exception cref="System.RankException">The source System.Array is multidimensional.</exception>
        /// <exception cref="System.InvalidCastException">At least one element in the source System.Array
        /// cannot be cast to the type of destination array.</exception>
        public void CopyTo(T[] array, long index)
        {
            _source.CopyTo(array, index);
        }

        #region ICollection<T> Members

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>The number of elements contained in the
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        public int Count
        {
            get { return Length; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>True.</returns>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Not supported by the <see cref="ImmutableArray{T}"/>.
        /// </summary>
        /// <param name="item">Unused.</param>
        /// <exception cref="NotSupportedException">Method is accessed.</exception>
        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported by the <see cref="ImmutableArray{T}"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">Method is accessed.</exception>
        public void Clear()
        {
            throw new NotSupportedException();
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
            return _source.Contains(item);
        }

        /// <summary>
        /// Copies all the elements of the current one-dimensional System.Array to the specified one-dimensional
        /// System.Array starting at the specified destination System.Array index.
        /// </summary>
        /// <param name="array">array: The one-dimensional System.Array that is the destination of the
        /// elements copied from the current System.Array.</param>
        /// <param name="index">A 32-bit integer that represents the index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than the lower bound of array.</exception>
        /// <exception cref="System.ArgumentException">array is multidimensional.
        /// -or- index is equal to or greater than the length of array and the source System.Array has a length greater than 0.
        /// -or- The number of elements in the source System.Array is greater than the available space from index to the end of
        /// the destination array.</exception>
        /// <exception cref="System.ArrayTypeMismatchException">The type of the source System.Array cannot be cast
        /// automatically to the type of the destination array.</exception>
        /// <exception cref="System.RankException">The source System.Array is multidimensional.</exception>
        /// <exception cref="System.InvalidCastException">At least one element in the source System.Array
        /// cannot be cast to the type of destination array.</exception>
        public void CopyTo(T[] array, int index)
        {
            _source.CopyTo(array, index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_source).GetEnumerator();
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
        /// Not supported by the <see cref="ImmutableArray{T}"/>.
        /// </summary>
        /// <param name="item">Unused.</param>
        /// <returns>None.</returns>
        /// <exception cref="NotSupportedException">Method is accessed.</exception>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}