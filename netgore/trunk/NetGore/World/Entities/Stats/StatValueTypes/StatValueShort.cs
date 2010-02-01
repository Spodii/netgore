using System.Data;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// A stat value type with an underlying value type of a short.
    /// </summary>
    public struct StatValueShort : IStatValueType
    {
        /// <summary>
        /// The underlying value.
        /// </summary>
        readonly short _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatValueShort"/> struct.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public StatValueShort(short value)
        {
            _value = value;
        }

        #region IStatValueType Members

        /// <summary>
        /// Creates a deep copy of the <see cref="IStatValueType"/>. The returned <see cref="IStatValueType"/> is of the
        /// same type of the object that this method was called on, and contains the same value.
        /// </summary>
        /// <returns>
        /// The deep copy of this <see cref="IStatValueType"/>.
        /// </returns>
        public IStatValueType DeepCopy()
        {
            return new StatValueShort(_value);
        }

        /// <summary>
        /// Gets the value of this <see cref="IStatValueType"/> as an integer.
        /// </summary>
        /// <returns>
        /// The value of this <see cref="IStatValueType"/> as an integer.
        /// </returns>
        public int GetValue()
        {
            return _value;
        }

        /// <summary>
        /// Reads the value of this <see cref="IStatValueType"/> from a <paramref name="bitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to read the value from.</param>
        /// <returns></returns>
        public IStatValueType Read(BitStream bitStream)
        {
            short value = bitStream.ReadShort();
            return new StatValueShort(value);
        }

        /// <summary>
        /// Reads the value of this <see cref="IStatValueType"/> from a <paramref name="dataRecord"/> at the given
        /// <paramref name="ordinal"/>.
        /// </summary>
        /// <param name="dataRecord">The <see cref="IDataRecord"/> to read from.</param>
        /// <param name="ordinal">The ordinal in the <paramref name="dataRecord"/> to read from.</param>
        /// <returns></returns>
        public IStatValueType Read(IDataRecord dataRecord, int ordinal)
        {
            short value = dataRecord.GetInt16(ordinal);
            return new StatValueShort(value);
        }

        /// <summary>
        /// Sets the value of this <see cref="IStatValueType"/>.
        /// </summary>
        /// <param name="value">The integer value to set this <see cref="IStatValueType"/>.</param>
        /// <returns></returns>
        public IStatValueType SetValue(int value)
        {
            Debug.Assert(value >= short.MinValue && value <= short.MaxValue);
            return new StatValueShort((short)value);
        }

        /// <summary>
        /// Writes this <see cref="IStatValueType"/>'s value to the given <paramref name="bitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <paramref name="bitStream"/> to write this <see cref="IStatValueType"/> to.</param>
        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        #endregion
    }
}