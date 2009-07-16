using System;
using System.Data;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Interface for the primary holder object of stat information, containing the type of stat, events, and the
    /// actual value of the stat.
    /// </summary>
    public interface IStat
    {
        /// <summary>
        /// Notifies listeners that the value of the stat has changed.
        /// </summary>
        event StatChangeHandler OnChange;

        /// <summary>
        /// Gets the StatType of this IStat.
        /// </summary>
        StatType StatType { get; }

        /// <summary>
        /// Gets or sets the value of this IStat as an integer.
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// Creates a deep copy of the IStat, resulting in a new IStat object of the same type as this IStat, and
        /// containing the same IStatValueType with the same value, same StatType, and same CanWrite property value.
        /// </summary>
        /// <returns>The deep copy of the IStat.</returns>
        IStat DeepCopy();

        /// <summary>
        /// Creates a deep copy of the IStat's IStatValueType, resulting in a new IStatValueType object of the same
        /// type as this IStat's IStatValueType, and containing the same value.
        /// </summary>
        /// <returns>The deep copy of the IStat's IStatValueType.</returns>
        IStatValueType DeepCopyValueType();

        /// <summary>
        /// Reads the value for the IStat into the Value property using the specified BitStream. The BitStream
        /// must not be null and must already be positioned at the start (first byte) of the value to be read.
        /// </summary>
        /// <param name="bitStream">BitStream to acquire the value from.</param>
        void Read(BitStream bitStream);

        /// <summary>
        /// Reads the value for the IStat into the Value property using the specified IDataRecord. The IDataReader
        /// must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader">IDataRecord to acquire the value from.</param>
        /// <param name="ordinal">Ordinal of the field to read the value from.</param>
        void Read(IDataRecord dataReader, int ordinal);

        /// <summary>
        /// Reads the value for the IStat into the Value property using the specified IDataRecord. The IDataReader
        /// must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader">IDataRecord to acquire the value from.</param>
        /// <param name="name">Name of the field to read the value from.</param>
        void Read(IDataReader dataReader, string name);

        /// <summary>
        /// Writes the Value property of the IStat directly into the specified BitStream. The BitStream must not be null,
        /// be in Write mode, and be positioned at the location where the value is to be written.
        /// </summary>
        /// <param name="bitStream">BitStream to write the value of the IStat to.</param>
        void Write(BitStream bitStream);
    }
}