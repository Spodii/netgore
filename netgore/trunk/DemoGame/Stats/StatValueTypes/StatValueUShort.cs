using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    public class StatValueUShort : IStatValueType
    {
        ushort _value;

        public StatValueUShort()
        {
        }

        public StatValueUShort(ushort value)
        {
            _value = value;
        }

        #region IStatValueType Members

        /// <summary>
        /// Gets the value of this IStatValueType as an integer.
        /// </summary>
        /// <returns>The value of this IStatValueType as an integer.</returns>
        public int GetValue()
        {
            return _value;
        }

        /// <summary>
        /// Sets the value of this IStatValueType.
        /// </summary>
        /// <param name="value">The integer value to set this IStatValueType.</param>
        public void SetValue(int value)
        {
            Debug.Assert(value >= ushort.MinValue && value <= ushort.MaxValue);
            _value = (ushort)value;
        }

        /// <summary>
        /// Writes this IStatValueType's value to the given <paramref name="bitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <paramref name="bitStream"/> to write this IStatValueType to.</param>
        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        /// <summary>
        /// Reads the value of this IStatValueType from a <paramref name="bitStream"/>.
        /// </summary>
        /// <param name="bitStream">The BitStream to read the value from.</param>
        public void Read(BitStream bitStream)
        {
            _value = bitStream.ReadUShort();
        }

        /// <summary>
        /// Reads the value of this IStatValueType from a <paramref name="dataRecord"/> at the given <paramref name="ordinal"/>.
        /// </summary>
        /// <param name="dataRecord">The IDataReader to read from.</param>
        /// <param name="ordinal">The ordinal in the <paramref name="dataRecord"/> to read from.</param>
        public void Read(IDataRecord dataRecord, int ordinal)
        {
            object v = dataRecord.GetValue(ordinal);
            _value = Convert.ToUInt16(v);
        }

        /// <summary>
        /// Creates a deep copy of the IStatValueType. The returned IStatValueType is of the same Type of the object that
        /// this method was called on, and contains the same value.
        /// </summary>
        /// <returns>The deep copy of this IStatValueType.</returns>
        public IStatValueType DeepCopy()
        {
            return new StatValueUShort(_value);
        }

        #endregion

        public static implicit operator int(StatValueUShort v)
        {
            // TODO: Have to decide if I want to do this implicit operator to int on all of these...
            return v.GetValue();
        }
    }
}