using System;
using System.Data;
using System.Linq;

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

        /// <summary>
        /// Gets the boolean value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The boolean value of the specified field.</returns>
        public static bool GetBoolean(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetBoolean(i);
        }

        /// <summary>
        /// Gets the 8-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 8-bit signed integer value of the specified field.</returns>
        public static byte GetByte(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetByte(i);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array,
        /// starting at the given buffer offset.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferOffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        public static long GetBytes(this IDataRecord r, string name, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            var i = r.GetOrdinal(name);
            return r.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets the character value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The character value of the specified field.</returns>
        public static char GetChar(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetChar(i);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array,
        /// starting at the given buffer offset.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of characters.</param>
        /// <param name="bufferOffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        public static long GetChars(this IDataRecord r, string name, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            var i = r.GetOrdinal(name);
            return r.GetChars(i, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The fixed-position numeric value of the specified field.</returns>
        public static decimal GetDecimal(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetDecimal(i);
        }

        /// <summary>
        /// Gets the single-precision floating point value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The single-precision floating point of the specified field.</returns>
        public static float GetFloat(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetFloat(i);
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        public static short GetInt16(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetInt16(i);
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 32-bit signed integer value of the specified field.</returns>
        public static int GetInt32(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetInt32(i);
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 64-bit signed integer value of the specified field.</returns>
        public static long GetInt64(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetInt64(i);
        }

        /// <summary>
        /// Gets the nullable Boolean of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable Boolean of the specified field.</returns>
        public static bool? GetNullableBoolean(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetBoolean(i);
        }

        /// <summary>
        /// Gets the nullable Boolean of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable Boolean of the specified field.</returns>
        public static bool? GetNullableBoolean(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableBoolean(i);
        }

        /// <summary>
        /// Gets the nullable Byte of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable Byte of the specified field.</returns>
        public static byte? GetNullableByte(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetByte(i);
        }

        /// <summary>
        /// Gets the nullable Byte of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable Byte of the specified field.</returns>
        public static byte? GetNullableByte(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableByte(i);
        }

        /// <summary>
        /// Gets the nullable Decimal of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable Decimal of the specified field.</returns>
        public static decimal? GetNullableDecimal(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetDecimal(i);
        }

        /// <summary>
        /// Gets the nullable Decimal of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable Decimal of the specified field.</returns>
        public static decimal? GetNullableDecimal(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableDecimal(i);
        }

        /// <summary>
        /// Gets the nullable Float of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable Float of the specified field.</returns>
        public static float? GetNullableFloat(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetFloat(i);
        }

        /// <summary>
        /// Gets the nullable Float of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable Float of the specified field.</returns>
        public static float? GetNullableFloat(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableFloat(i);
        }

        /// <summary>
        /// Gets the nullable Short of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable Short of the specified field.</returns>
        public static short? GetNullableInt16(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetInt16(i);
        }

        /// <summary>
        /// Gets the nullable Short of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable Short of the specified field.</returns>
        public static short? GetNullableInt16(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableInt16(i);
        }

        /// <summary>
        /// Gets the nullable Int32 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable Int32 of the specified field.</returns>
        public static int? GetNullableInt32(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetInt32(i);
        }

        /// <summary>
        /// Gets the nullable Int32 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable Int32 of the specified field.</returns>
        public static int? GetNullableInt32(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableInt32(i);
        }

        /// <summary>
        /// Gets the nullable Int64 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable Int64 of the specified field.</returns>
        public static long? GetNullableInt64(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetInt64(i);
        }

        /// <summary>
        /// Gets the nullable Int64 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable Int64 of the specified field.</returns>
        public static long? GetNullableInt64(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableInt64(i);
        }

        /// <summary>
        /// Gets the nullable SByte of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable SByte of the specified field.</returns>
        public static sbyte? GetNullableSByte(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableSByte(i);
        }

        /// <summary>
        /// Gets the nullable SByte of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable SByte of the specified field.</returns>
        public static sbyte? GetNullableSByte(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetSByte(i);
        }

        /// <summary>
        /// Gets the nullable UInt16 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable UInt16 of the specified field.</returns>
        public static ushort? GetNullableUInt16(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetUInt16(i);
        }

        /// <summary>
        /// Gets the nullable UInt16 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable UInt16 of the specified field.</returns>
        public static ushort? GetNullableUInt16(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableUInt16(i);
        }

        /// <summary>
        /// Gets the nullable UInt32 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable UInt32 of the specified field.</returns>
        public static ulong? GetNullableUInt32(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableUInt32(i);
        }

        /// <summary>
        /// Gets the nullable UInt32 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable UInt32 of the specified field.</returns>
        public static ulong? GetNullableUInt32(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetUInt32(i);
        }

        /// <summary>
        /// Gets the nullable UInt64 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The nullable UInt64 of the specified field.</returns>
        public static ulong? GetNullableUInt64(this IDataRecord r, int i)
        {
            if (r.IsDBNull(i))
                return null;

            return r.GetUInt64(i);
        }

        /// <summary>
        /// Gets the nullable UInt64 of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The nullable UInt64 of the specified field.</returns>
        public static ulong? GetNullableUInt64(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetNullableUInt64(i);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 8-bit unsigned integer value of the specified field.</returns>
        public static sbyte GetSByte(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetSByte(i);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 8-bit unsigned integer value of the specified field.</returns>
        public static sbyte GetSByte(this IDataRecord r, int i)
        {
            var value = r.GetValue(i);
            if (value is sbyte)
                return (sbyte)value;

            return Convert.ToSByte(value);
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        public static string GetString(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetString(i);
        }

        /// <summary>
        /// Gets the 16-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 16-bit unsigned integer value of the specified field.</returns>
        public static ushort GetUInt16(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetUInt16(i);
        }

        /// <summary>
        /// Gets the 16-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 16-bit unsigned integer value of the specified field.</returns>
        public static ushort GetUInt16(this IDataRecord r, int i)
        {
            var value = r.GetValue(i);
            if (value is ushort)
                return (ushort)value;

            return Convert.ToUInt16(value);
        }

        /// <summary>
        /// Gets the 32-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 32-bit unsigned integer value of the specified field.</returns>
        public static uint GetUInt32(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetUInt32(i);
        }

        /// <summary>
        /// Gets the 32-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 32-bit unsigned integer value of the specified field.</returns>
        public static uint GetUInt32(this IDataRecord r, int i)
        {
            var value = r.GetValue(i);
            if (value is uint)
                return (uint)value;

            return Convert.ToUInt32(value);
        }

        /// <summary>
        /// Gets the 64-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 64-bit unsigned integer value of the specified field.</returns>
        public static ulong GetUInt64(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetUInt64(i);
        }

        /// <summary>
        /// Gets the 64-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 64-bit unsigned integer value of the specified field.</returns>
        public static ulong GetUInt64(this IDataRecord r, int i)
        {
            var value = r.GetValue(i);
            if (value is ulong)
                return (ulong)value;

            return Convert.ToUInt64(value);
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The System.Object which will contain the field value upon return.</returns>
        public static object GetValue(this IDataRecord r, string name)
        {
            var i = r.GetOrdinal(name);
            return r.GetValue(i);
        }
    }
}