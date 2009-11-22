using System.Data;
using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Interface for a value type for an IStat. This interface allows for the underlying value type to be of any type.
    /// This interface is designed to be implemented on immutable structs or objects.
    /// </summary>
    public interface IStatValueType
    {
        /// <summary>
        /// Creates a deep copy of the IStatValueType. The returned IStatValueType is of the same Type of the object that
        /// this method was called on, and contains the same value.
        /// </summary>
        /// <returns>The deep copy of this IStatValueType.</returns>
        IStatValueType DeepCopy();

        /// <summary>
        /// Gets the value of this IStatValueType as an integer.
        /// </summary>
        /// <returns>The value of this IStatValueType as an integer.</returns>
        int GetValue();

        /// <summary>
        /// Reads the value of this IStatValueType from a <paramref name="bitStream"/>.
        /// </summary>
        /// <param name="bitStream">The BitStream to read the value from.</param>
        IStatValueType Read(BitStream bitStream);

        /// <summary>
        /// Reads the value of this IStatValueType from a <paramref name="dataRecord"/> at the given <paramref name="ordinal"/>.
        /// </summary>
        /// <param name="dataRecord">The IDataReader to read from.</param>
        /// <param name="ordinal">The ordinal in the <paramref name="dataRecord"/> to read from.</param>
        IStatValueType Read(IDataRecord dataRecord, int ordinal);

        /// <summary>
        /// Sets the value of this IStatValueType.
        /// </summary>
        /// <param name="value">The integer value to set this IStatValueType.</param>
        IStatValueType SetValue(int value);

        /// <summary>
        /// Writes this IStatValueType's value to the given <paramref name="bitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <paramref name="bitStream"/> to write this IStatValueType to.</param>
        void Write(BitStream bitStream);
    }
}