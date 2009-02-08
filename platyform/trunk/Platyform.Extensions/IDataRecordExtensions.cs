using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace Platyform.Extensions
{
    /// <summary>
    /// Extensions for the System.Data.IDataRecord.
    /// </summary>
    public static class IDataRecordExtensions
    {
        /// <summary>
        /// Gets the ordinal for an IDataRecord as a byte, throwing an InvalidCastException if the
        /// retreived ordinal is not able to convert to a byte without data loss.
        /// </summary>
        /// <param name="dataRecord">IDataRecord to load the ordinal from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>Index of the named field.</returns>
        public static byte GetOrdinalAsByte(this IDataRecord dataRecord, string name)
        {
            int ordinal = dataRecord.GetOrdinal(name);

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