using System;
using System.Data;
using System.Linq;
using NetGore.IO;

namespace NetGore.Stats
{
    /// <summary>
    /// Interface for the primary holder object of stat information, containing the type of stat, events, and the
    /// actual value of the stat.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public interface IStat<TStatType> where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Notifies listeners that the value of this <see cref="IStat{TStatType}"/> has changed.
        /// </summary>
        event IStatEventHandler<TStatType> Changed;

        /// <summary>
        /// Gets the <typeparamref name="TStatType"/> of this <see cref="IStat{TStatType}"/>.
        /// </summary>
        TStatType StatType { get; }

        /// <summary>
        /// Gets or sets the value of this <see cref="IStat{TStatType}"/> as an integer.
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// Creates a deep copy of the <see cref="IStat{TStatType}"/>, resulting in a new <see cref="IStat{TStatType}"/>
        /// object of the same type as this <see cref="IStat{TStatType}"/>, and containing the same <see cref="IStatValueType"/>
        /// with the same value, and same <typeparamref name="TStatType"/>.
        /// </summary>
        /// <returns>The deep copy of the <see cref="IStat{TStatType}"/>.</returns>
        IStat<TStatType> DeepCopy();

        /// <summary>
        /// Creates a deep copy of the <see cref="IStat{TStatType}"/>'s <see cref="IStatValueType"/>, resulting in a new
        /// <see cref="IStatValueType"/> object of the same type as this <see cref="IStat{TStatType}"/>'s
        /// <see cref="IStatValueType"/>, and containing the same value.
        /// </summary>
        /// <returns>The deep copy of the <see cref="IStat{TStatType}"/>'s <see cref="IStatValueType"/>.</returns>
        IStatValueType DeepCopyValueType();

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the <see cref="IStat{StatType}.Value"/> property using
        /// the specified <see cref="BitStream"/>. The <see cref="BitStream"/> must not be null, be in
        /// <see cref="BitStreamMode.Read"/> mode, and must already be positioned at the start of the value to be read.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to acquire the value from.</param>
        void Read(BitStream bitStream);

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the Value property using the specified
        /// <see cref="IDataRecord"/>. The <see cref="IDataRecord"/> must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader"><see cref="IDataRecord"/> to acquire the value from.</param>
        /// <param name="ordinal">Ordinal of the field to read the value from.</param>
        void Read(IDataRecord dataReader, int ordinal);

        /// <summary>
        /// Reads the value for the <see cref="IStat{TStatType}"/> into the Value property using the specified
        /// <see cref="IDataReader"/>. The <see cref="IDataReader"/> must not be null and currently have a row being read.
        /// </summary>
        /// <param name="dataReader"><see cref="IDataReader"/> to acquire the value from.</param>
        /// <param name="name">Name of the field to read the value from.</param>
        void Read(IDataReader dataReader, string name);

        /// <summary>
        /// Writes the <see cref="IStat{StatType}.Value"/> property of the <see cref="IStat{TStatType}"/> directly into the
        /// specified <see cref="BitStream"/>. The <see cref="BitStream"/> must not be null, be in
        /// <see cref="BitStreamMode.Write"/> mode, and be positioned at the location where the value is to be written.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to write the value of the <see cref="IStat{TStatType}"/> to.</param>
        void Write(BitStream bitStream);
    }
}