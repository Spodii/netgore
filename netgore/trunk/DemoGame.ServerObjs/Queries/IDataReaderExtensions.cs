using System;
using System.Data;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// Extensions for the IDataReader for reading special data types unique to the game.
    /// </summary>
    public static class IDataReaderExtensions
    {
        static void CheckIfDefined<T>(T value)
        {
            if (!Enum.IsDefined(typeof(T), value))
                throw new InvalidCastException(string.Format("Value `{0}` is not defined for enum `{1}`.", value, typeof(T)));
        }

        /// <summary>
        /// Gets the EquipmentSlot value from the specified column.
        /// </summary>
        /// <param name="dataReader">DataReader to read the value from.</param>
        /// <param name="ordinal">The zero-based column ordinal to read from.</param>
        /// <returns>The EquipmentSlot value from the specified column.</returns>
        public static EquipmentSlot GetEquipmentSlot(this IDataReader dataReader, int ordinal)
        {
            EquipmentSlot ret = (EquipmentSlot)dataReader.GetByte(ordinal);
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
            ItemType ret = (ItemType)dataReader.GetByte(ordinal);
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