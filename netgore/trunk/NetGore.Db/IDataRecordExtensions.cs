using System;
using System.Data;

namespace NetGore.Db
{
    /// <summary>
    /// Extensions for the <see cref="IDataRecord"/> interface.
    /// </summary>
    public static class IDataRecordExtensions
    {
        /// <summary>
        /// Checks if the current row of the <see cref="IDataRecord"/> contains a field of the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to check.</param>
        /// <param name="name">Name of the field to check if exists.</param>
        /// <returns>True if a field of the specified <paramref name="name"/> exists, else false.</returns>
        public static bool ContainsField(this IDataRecord r, string name)
        {
            try
            {
                r.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                // If the field name does not exist, GetOrdinal() should throw a IndexOutOfRangeException
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the current row of the <see cref="IDataRecord"/> contains a field of the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to check.</param>
        /// <param name="name">Name of the field to check if exists.</param>
        /// <param name="ordinal">If the field exists, contains the ordinal of the field. Otherwise this value
        /// is -1.</param>
        /// <returns>True if a field of the specified <paramref name="name"/> exists, else false.</returns>
        public static bool ContainsField(this IDataRecord r, string name, out int ordinal)
        {
            ordinal = -1;

            try
            {
                ordinal = r.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                // If the field name does not exist, GetOrdinal() should throw a IndexOutOfRangeException
                return false;
            }

            return true;
        }
    }
}