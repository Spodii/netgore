using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// Good thing I only spent a few hours making this fucking this before realizing there is a "BitArray"... -.-

namespace NetGore.Collections
{
    /// <summary>
    /// An array that specializes in handling Boolean values.
    /// </summary>
    public class BoolArray : IEnumerable<bool>
    {
        const int _bufferBitSize = sizeof(byte) * 8;

        int _bitLength;
        byte[] _buffer;
        int _version;

        /// <summary>
        /// Gets or sets the boolean value at a given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index to set the value at.</param>
        /// <returns>Value at the given <paramref name="index"/>.</returns>
        public bool this[int index]
        {
            get
            {
                if (index < 0 || index >= _bitLength)
                    throw new IndexOutOfRangeException();

                int bufferIndex = GetBufferIndex(index);
                int bit = GetBufferBitIndex(index);
                int bitMask = 1 << bit;

                return (_buffer[bufferIndex] & bitMask) != 0;
            }

            set
            {
                if (index < 0 || index >= _bitLength)
                    throw new IndexOutOfRangeException();

                int bufferIndex = GetBufferIndex(index);
                int bit = GetBufferBitIndex(index);
                int bitMask = 1 << bit;

                _version++;

                if (value)
                {
                    // Set the bit
                    _buffer[bufferIndex] |= (byte)bitMask;
                }
                else
                {
                    // Unset the bit
                    _buffer[bufferIndex] &= (byte)(~bitMask);
                }
            }
        }

        /// <summary>
        /// Gets the total number of elements in the BoolArray.
        /// </summary>
        public int Length
        {
            get { return _bitLength; }
        }

        /// <summary>
        /// BoolArray constructor.
        /// </summary>
        /// <param name="length">The initial Length of the BoolArray.</param>
        public BoolArray(int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "The array length must be greater than 0.");

            _bitLength = length;
            _buffer = new byte[RequiredBufferSize(_bitLength)];
        }

        /// <summary>
        /// BoolArray constructor.
        /// </summary>
        /// <param name="values">Boolean array containing the initial values to use.</param>
        public BoolArray(bool[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            _bitLength = values.Length;
            _buffer = new byte[RequiredBufferSize(_bitLength)];

            for (int i = 0; i < _bitLength; i++)
            {
                this[i] = values[i];
            }
        }

        /// <summary>
        /// Gets the bit position in the buffer for a given bit index.
        /// </summary>
        /// <param name="bitIndex">Index of the bit.</param>
        /// <returns>Bit position in the buffer for the given <paramref name="bitIndex"/>.</returns>
        static int GetBufferBitIndex(int bitIndex)
        {
            return bitIndex % _bufferBitSize;
        }

        /// <summary>
        /// Gets the index of the buffer for a given bit index.
        /// </summary>
        /// <param name="bitIndex">Index of the bit.</param>
        /// <returns>Buffer index containing the given <paramref name="bitIndex"/>.</returns>
        static int GetBufferIndex(int bitIndex)
        {
            return bitIndex / _bufferBitSize;
        }

        /// <summary>
        /// Gets the required size for the buffer to fit a given number of bits into the buffer.
        /// </summary>
        /// <param name="bitLength">Number of bits needed to fit into the buffer.</param>
        /// <returns>Required size of the buffer to fit <paramref name="bitLength"/> bits.</returns>
        static int RequiredBufferSize(int bitLength)
        {
            return (int)Math.Ceiling(bitLength / (float)_bufferBitSize);
        }

        /// <summary>
        /// Resizes the BoolArray.
        /// </summary>
        /// <param name="length">New Length of the BoolArray.</param>
        public void Resize(int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "The array length must be greater than 0.");

            // No need to resize
            if (length == _bitLength)
                return;

            // If scaling up in size, make sure that new elements start as false by setting all bits
            // allocated, but not used, as false
            if (length > _bitLength)
            {
                // Index of the highest used buffer index
                int highestUsedBufferIndex = GetBufferIndex(_bitLength - 1);

                // Index of the highest used bit in that buffer index
                int highestUsedBufferBitIndex = GetBufferBitIndex(_bitLength - 1);

                // Mask for all the bits in that highest used buffer index
                int usedBitsMask = ((1 << (highestUsedBufferBitIndex + 1)) - 1);

                // Remove all the unused bits
                _buffer[highestUsedBufferIndex] &= (byte)usedBitsMask;

                // If our buffer contains elements greater than the highest used one, make sure they are false
                for (int i = highestUsedBufferIndex + 1; i < _buffer.Length; i++)
                {
                    _buffer[i] = 0;
                }
            }

            // Set the new buffer
            _bitLength = length;
            Array.Resize(ref _buffer, RequiredBufferSize(length));

            _version++;
        }

        /// <summary>
        /// Sets all values in the BoolArray to the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to set for all values in the BoolArray.</param>
        public void SetAll(bool value)
        {
            if (value)
                SetAllTrue();
            else
                SetAllFalse();
        }

        /// <summary>
        /// Sets all values in the buffer to false.
        /// </summary>
        void SetAllFalse()
        {
            _version++;
            for (int i = 0; i < _buffer.Length; i++)
            {
                _buffer[i] = 0;
            }
        }

        /// <summary>
        /// Sets all values in the buffer to true.
        /// </summary>
        void SetAllTrue()
        {
            _version++;
            for (int i = 0; i < _buffer.Length; i++)
            {
                _buffer[i] = (1 << _bufferBitSize) - 1;
            }
        }

        /// <summary>
        /// Creates a new boolean array containing all the values in this BoolArray.
        /// </summary>
        /// <returns>Boolean array containing all the values in this BoolArray.</returns>
        public bool[] ToArray()
        {
            var ret = new bool[Length];
            for (int i = 0; i < Length; i++)
            {
                ret[i] = this[i];
            }
            return ret;
        }

        #region IEnumerable<bool> Members

        ///<summary>
        ///
        ///                    Returns an enumerator that iterates through the collection.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///                
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public IEnumerator<bool> GetEnumerator()
        {
            return new Enumerator(this);
        }

        ///<summary>
        ///
        ///                    Returns an enumerator that iterates through a collection.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Enumerator for the BoolArray.
        /// </summary>
        struct Enumerator : IEnumerator<bool>
        {
            readonly BoolArray _boolArray;
            readonly int _version;
            bool _current;
            int _index;

            /// <summary>
            /// Enumerator constructor.
            /// </summary>
            /// <param name="boolArray">BoolArray to enumerate over.</param>
            internal Enumerator(BoolArray boolArray)
            {
                _boolArray = boolArray;
                _index = 0;
                _version = boolArray._version;
                _current = false;
            }

            #region IEnumerator<bool> Members

            ///<summary>
            ///
            ///                    Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            ///                
            ///</summary>
            ///<filterpriority>2</filterpriority>
            public void Dispose()
            {
            }

            ///<summary>
            ///
            ///                    Advances the enumerator to the next element of the collection.
            ///                
            ///</summary>
            ///
            ///<returns>
            ///true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            ///                
            ///</returns>
            ///
            ///<exception cref="T:System.InvalidOperationException">
            ///                    The collection was modified after the enumerator was created. 
            ///                </exception><filterpriority>2</filterpriority>
            public bool MoveNext()
            {
                // Make sure the BoolArray has not changed
                if (_version != _boolArray._version)
                    throw new InvalidOperationException("The collection has changed while enumerating over it.");

                // Make sure we have a valid index
                if (_index >= _boolArray._bitLength)
                    return false;

                // Move to the next item
                _current = _boolArray[_index];
                _index++;
                return true;
            }

            ///<summary>
            ///
            ///                    Sets the enumerator to its initial position, which is before the first element in the collection.
            ///                
            ///</summary>
            ///
            ///<exception cref="T:System.InvalidOperationException">
            ///                    The collection was modified after the enumerator was created. 
            ///                </exception><filterpriority>2</filterpriority>
            public void Reset()
            {
                _index = 0;
                _current = default(bool);
            }

            ///<summary>
            ///
            ///                    Gets the element in the collection at the current position of the enumerator.
            ///                
            ///</summary>
            ///
            ///<returns>
            ///
            ///                    The element in the collection at the current position of the enumerator.
            ///                
            ///</returns>
            ///
            public bool Current
            {
                get { return _current; }
            }

            ///<summary>
            ///
            ///                    Gets the current element in the collection.
            ///                
            ///</summary>
            ///
            ///<returns>
            ///
            ///                    The current element in the collection.
            ///                
            ///</returns>
            ///
            ///<exception cref="T:System.InvalidOperationException">
            ///                    The enumerator is positioned before the first element of the collection or after the last element.
            ///                </exception><filterpriority>2</filterpriority>
            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion
        }
    }
}