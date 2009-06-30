using System;
using System.Data;

namespace NetGore.Db
{
    /// <summary>
    /// Extensions for the IDataReader.
    /// </summary>
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// Checks if the current row of the IDataReader contains a field of the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="dataReader">IDataReader to check.</param>
        /// <param name="name">Name of the field to check if exists.</param>
        /// <returns>True if a field of the specified <paramref name="name"/> exists, else false.</returns>
        public static bool ContainsField(this IDataReader dataReader, string name)
        {
            // If the field name does not exist, GetOrdinal() should throw a IndexOutOfRangeException
            try
            {
                dataReader.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the current row of the IDataReader contains a field of the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="dataReader">IDataReader to check.</param>
        /// <param name="name">Name of the field to check if exists.</param>
        /// <param name="ordinal">If the field exists, contains the ordinal of the field. Otherwise this value
        /// is -1.</param>
        /// <returns>True if a field of the specified <paramref name="name"/> exists, else false.</returns>
        public static bool ContainsField(this IDataReader dataReader, string name, out int ordinal)
        {
            ordinal = -1;

            // If the field name does not exist, GetOrdinal() should throw a IndexOutOfRangeException
            try
            {
                ordinal = dataReader.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the boolean value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The boolean value of the specified field.</returns>
        public static bool GetBoolean(this IDataReader dataReader, string name)
        {
            return dataReader.GetBoolean(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 8-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 8-bit signed integer value of the specified field.</returns>
        public static byte GetByte(this IDataReader dataReader, string name)
        {
            return dataReader.GetByte(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array,
        /// starting at the given buffer offset.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferOffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        public static long GetBytes(this IDataReader dataReader, string name, long fieldOffset, byte[] buffer, int bufferOffset,
                                    int length)
        {
            return dataReader.GetBytes(dataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets the character value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The character value of the specified field.</returns>
        public static char GetChar(this IDataReader dataReader, string name)
        {
            return dataReader.GetChar(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array,
        /// starting at the given buffer offset.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of characters.</param>
        /// <param name="bufferOffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        public static long GetChars(this IDataReader dataReader, string name, long fieldOffset, char[] buffer, int bufferOffset,
                                    int length)
        {
            return dataReader.GetChars(dataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The fixed-position numeric value of the specified field.</returns>
        public static decimal GetDecimal(this IDataReader dataReader, string name)
        {
            return dataReader.GetDecimal(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the single-precision floating point value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The single-precision floating point of the specified field.</returns>
        public static float GetFloat(this IDataReader dataReader, string name)
        {
            return dataReader.GetFloat(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        public static short GetInt16(this IDataReader dataReader, string name)
        {
            return dataReader.GetInt16(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 32-bit signed integer value of the specified field.</returns>
        public static int GetInt32(this IDataReader dataReader, string name)
        {
            return dataReader.GetInt32(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 64-bit signed integer value of the specified field.</returns>
        public static long GetInt64(this IDataReader dataReader, string name)
        {
            return dataReader.GetInt64(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 8-bit unsigned integer value of the specified field.</returns>
        public static sbyte GetSByte(this IDataReader dataReader, string name)
        {
            object value = GetValue(dataReader, name);
            if (value is sbyte)
                return (sbyte)value;

            return Convert.ToSByte(value);
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        public static string GetString(this IDataReader dataReader, string name)
        {
            return dataReader.GetString(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 16-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 16-bit unsigned integer value of the specified field.</returns>
        public static ushort GetUInt16(this IDataReader dataReader, string name)
        {
            return dataReader.GetUInt16(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 16-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 16-bit unsigned integer value of the specified field.</returns>
        public static ushort GetUInt16(this IDataReader dataReader, int i)
        {
            object value = dataReader.GetValue(i);
            if (value is ushort)
                return (ushort)value;

            return Convert.ToUInt16(value);
        }

        /// <summary>
        /// Gets the 32-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 32-bit unsigned integer value of the specified field.</returns>
        public static uint GetUInt32(this IDataReader dataReader, string name)
        {
            return dataReader.GetUInt32(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 32-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 32-bit unsigned integer value of the specified field.</returns>
        public static uint GetUInt32(this IDataReader dataReader, int i)
        {
            object value = dataReader.GetValue(i);
            if (value is uint)
                return (uint)value;

            return Convert.ToUInt32(value);
        }

        /// <summary>
        /// Gets the 64-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The 64-bit unsigned integer value of the specified field.</returns>
        public static ulong GetUInt64(this IDataReader dataReader, string name)
        {
            return dataReader.GetUInt64(dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the 64-bit unsigned integer value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 64-bit unsigned integer value of the specified field.</returns>
        public static ulong GetUInt64(this IDataReader dataReader, int i)
        {
            object value = dataReader.GetValue(i);
            if (value is ulong)
                return (ulong)value;

            return Convert.ToUInt64(value);
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="dataReader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The System.Object which will contain the field value upon return.</returns>
        public static object GetValue(this IDataReader dataReader, string name)
        {
            return dataReader.GetValue(dataReader.GetOrdinal(name));
        }
    }
}