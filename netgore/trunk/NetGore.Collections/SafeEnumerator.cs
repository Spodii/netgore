using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Collections
{
    /// <summary>
    /// Wrapper for safely enumerating over a collection. This is primarily intended for collections that
    /// can be altered while they are being enumerated over, especially when the collection's items are the ones
    /// that alter the collection. This is not thread-safe, and each instance should not be enumerated over more than
    /// once at the same time.
    /// </summary>
    /// <typeparam name="T">Type of object to enumerate over.</typeparam>
    public class SafeEnumerator<T> : IEnumerable<T>
    {
        /// <summary>
        /// Contains the _source casted to an ICollection.
        /// </summary>
        readonly ICollection<T> _sourceAsCollection;

        /// <summary>
        /// Contains the source IEnumerable to enumerate over.
        /// </summary>
        readonly IEnumerable<T> _source;

        /// <summary>
        /// If true, we can optimize for using the _sourceAsCollection instead of _source to make use
        /// of all the extra stuff an ICollection provides.
        /// </summary>
        readonly bool _useCollection;

        /// <summary>
        /// If true, the source is a read-only ICollection so the buffer does not need to be updated
        /// every call to GetEnumerator().
        /// </summary>
        readonly bool _isSourceReadonly;

        /// <summary>
        /// Enumerator used to enumerate over the buffer.
        /// </summary>
        readonly Enumerator _bufferEnumerator;

        /// <summary>
        /// Buffer containing all of the items to be enumerated over. This is what is actually ultimately enumerated
        /// over instead of the _source.
        /// </summary>
        T[] _buffer;

        /// <summary>
        /// Length to enumerate over in the buffer. Allows us to avoid having to clear the buffer.
        /// </summary>
        int _sourceLength;

        /// <summary>
        /// Gets the source that is being enumerated over.
        /// </summary>
        public IEnumerable<T> Source { get { return _source; } }

        /// <summary>
        /// SafeEnumerator constructor
        /// </summary>
        /// <param name="source">Source to enumerate over.</param>
        public SafeEnumerator(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _source = source;
            _sourceAsCollection = source as ICollection<T>;

            if (_sourceAsCollection != null)
            {
                // Source is a collection, so optimize for collection usage
                _useCollection = true;
                _isSourceReadonly = _sourceAsCollection.IsReadOnly;

                // If the source is readonly, we only need to populate the buffer once (now)
                if (_isSourceReadonly)
                    _buffer = _source.ToArray();
                else
                    _buffer = new T[_sourceAsCollection.Count];
            }
            else
            {
                // Source is not a collection, so we have to do it the slow way
                _useCollection = false;
                _buffer = new T[_source.Count()];
            }

            // Cast the buffer as an IEnumerable<T> to avoid casting later
            _bufferEnumerator = new Enumerator(_buffer);
        }

        /// <summary>
        /// Copies over the source elements into the buffer and updates the source length.
        /// </summary>
        void UpdateBuffer()
        {
            if (_useCollection)
            {
                // Get the size of the source
                _sourceLength = _sourceAsCollection.Count;

                // Resize the buffer if needed
                if (_buffer.Length < _sourceLength)
                {
                    Array.Resize(ref _buffer, _sourceLength + 32);
                    _bufferEnumerator.ChangeArray(_buffer);
                }

                // Copy the elements from the source into the buffer
                _sourceAsCollection.CopyTo(_buffer, 0);
            }
            else
            {
                // Get the size of the source
                _sourceLength = _source.Count();

                // Resize the buffer if needed
                if (_buffer.Length < _sourceLength)
                {
                    Array.Resize(ref _buffer, _sourceLength + 32);
                    _bufferEnumerator.ChangeArray(_buffer);
                }

                // Copy the elements from the source into the buffer
                int i = -1;
                foreach (var item in _source)
                    _buffer[++i] = item;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            // Update the buffer for a non-readonly source
            if (!_isSourceReadonly)
                UpdateBuffer();

            // Return the enumerator for the buffer
            _bufferEnumerator.Initialize(_sourceLength);
            return _bufferEnumerator;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Enumerator for enumerating over only part of an array, not the whole thing. Specialized to be reused
        /// to enumerate over the same array numerous times.
        /// </summary>
        class Enumerator : IEnumerator<T>
        {
            T[] _array;
            int _endIndex;
            int _index;

            /// <summary>
            /// Enumerator constructor.
            /// </summary>
            /// <param name="array">Array to be enumerated over.</param>
            public Enumerator(T[] array)
            {
                _array = array;
            }

            /// <summary>
            /// Changes the array used by this Enumerator.
            /// </summary>
            /// <param name="newArray">New array to use.</param>
            public void ChangeArray(T[] newArray)
            {
                _array = newArray;
            }

            /// <summary>
            /// Readies the Enumerator for usage. Must be called before being returned.
            /// </summary>
            /// <param name="endIndex">Index to stop at when reached. Synonymous with Array.Length.</param>
            public void Initialize(int endIndex)
            {
                Reset();
                _endIndex = endIndex;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public void Dispose()
            {
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. 
            ///                 </exception><filterpriority>2</filterpriority>
            public bool MoveNext()
            {
                if (_index < _endIndex)
                {
                    _index++;
                    return (_index < _endIndex);
                }
                return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. 
            ///                 </exception><filterpriority>2</filterpriority>
            public void Reset()
            {
                _index = -1;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public T Current
            {
                get
                {
                    if (_index < 0 || _index >= _endIndex)
                        throw new InvalidOperationException("Invalid index position.");

                    return _array[_index];
                }
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <returns>
            /// The current element in the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.
            ///                 </exception><filterpriority>2</filterpriority>
            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
