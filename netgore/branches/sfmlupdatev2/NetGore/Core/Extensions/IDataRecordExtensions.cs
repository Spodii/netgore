using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Extensions for the <see cref="IDataRecord"/>.
    /// </summary>
    public static class IDataRecordExtensions
    {
        /// <summary>
        /// Gets the ordinal for an <see cref="IDataRecord"/> as a byte, throwing an <see cref="InvalidCastException"/> if the
        /// retreived ordinal is not able to convert to a byte without data loss.
        /// </summary>
        /// <param name="dataRecord"><see cref="IDataRecord"/> to load the ordinal from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>Index of the named field.</returns>
        /// <exception cref="InvalidCastException">If the ordinal could for the <paramref name="name"/> could not be safely casted
        /// into a <see cref="System.Byte"/> without data loss (ordinal is less than <see cref="byte.MinValue"/> or
        /// greater than <see cref="byte.MaxValue"/>).</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IDataRecordSet")]
        public static byte GetOrdinalAsByte(this IDataRecord dataRecord, string name)
        {
            var ordinal = dataRecord.GetOrdinal(name);

            if (!ordinal.IsBetween(byte.MinValue, byte.MaxValue))
            {
                const string errmsg =
                    "Could not convert ordinal value `{0}` from field `{1}` on IDataRecordSet `{2}` " +
                    "to a byte since the operation would result in data loss.";
                throw new InvalidCastException(string.Format(errmsg, ordinal, name, dataRecord));
            }

            return (byte)ordinal;
        }
    }
}