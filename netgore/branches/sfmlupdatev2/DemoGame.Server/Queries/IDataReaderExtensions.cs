using System;
using System.Data;
using System.Linq;
using NetGore;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// Extensions for the IDataReader for reading special data types unique to the game.
    /// </summary>
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// Checks if the enum value is defined.
        /// </summary>
        /// <typeparam name="T">The type of enum to check.</typeparam>
        /// <param name="value">The enum value to check if defined.</param>
        /// <exception cref="InvalidCastException">The <paramref name="value"/> is not a defined enum value for enum type
        /// <typeparamref name="T"/>.</exception>
        static void CheckIfDefined<T>(T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (!EnumHelper<T>.IsDefined(value))
            {
                const string errmsg = "Value `{0}` is not defined for enum `{1}`.";
                throw new InvalidCastException(string.Format(errmsg, value, typeof(T)));
            }
        }

        /// <summary>
        /// Gets the EquipmentSlot value from the specified column.
        /// </summary>
        /// <param name="dataReader">DataReader to read the value from.</param>
        /// <param name="ordinal">The zero-based column ordinal to read from.</param>
        /// <returns>The EquipmentSlot value from the specified column.</returns>
        public static EquipmentSlot GetEquipmentSlot(this IDataReader dataReader, int ordinal)
        {
            var ret = (EquipmentSlot)dataReader.GetByte(ordinal);
            CheckIfDefined(ret);
            return ret;
        }

        /// <summary>
        /// Gets the EquipmentSlot value from the specified column.
        /// </summary>
        /// <param name="dataReader">DataReader to read the value from.</param>
        /// <param name="name">The name of the column to read from.</param>
        /// <returns>The EquipmentSlot value from the specified column.</returns>
        public static EquipmentSlot GetEquipmentSlot(this IDataReader dataReader, string name)
        {
            return GetEquipmentSlot(dataReader, dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the ItemType value from the specified column.
        /// </summary>
        /// <param name="dataReader">DataReader to read the value from.</param>
        /// <param name="ordinal">The zero-based column ordinal to read from.</param>
        /// <returns>The ItemType value from the specified column.</returns>
        public static ItemType GetItemType(this IDataReader dataReader, int ordinal)
        {
            var ret = (ItemType)dataReader.GetByte(ordinal);
            CheckIfDefined(ret);
            return ret;
        }

        /// <summary>
        /// Gets the ItemType value from the specified column.
        /// </summary>
        /// <param name="dataReader">DataReader to read the value from.</param>
        /// <param name="name">The name of the column to read from.</param>
        /// <returns>The ItemType value from the specified column.</returns>
        public static ItemType GetItemType(this IDataReader dataReader, string name)
        {
            return GetItemType(dataReader, dataReader.GetOrdinal(name));
        }
    }
}