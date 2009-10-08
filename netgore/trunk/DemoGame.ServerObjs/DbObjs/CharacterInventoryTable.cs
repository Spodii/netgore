using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `character_inventory`.
    /// </summary>
    public class CharacterInventoryTable : ICharacterInventoryTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 3;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_inventory";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "character_id", "item_id", "slot" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "character_id", "slot" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "item_id" };

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        Int32 _characterID;

        /// <summary>
        /// The field that maps onto the database column `item_id`.
        /// </summary>
        Int32 _itemID;

        /// <summary>
        /// The field that maps onto the database column `slot`.
        /// </summary>
        Byte _slot;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public static IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public static IEnumerable<String> DbKeyColumns
        {
            get { return _dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public static IEnumerable<String> DbNonKeyColumns
        {
            get { return _dbColumnsNonKey; }
        }

        /// <summary>
        /// CharacterInventoryTable constructor.
        /// </summary>
        public CharacterInventoryTable()
        {
        }

        /// <summary>
        /// CharacterInventoryTable constructor.
        /// </summary>
        /// <param name="characterID">The initial value for the corresponding property.</param>
        /// <param name="itemID">The initial value for the corresponding property.</param>
        /// <param name="slot">The initial value for the corresponding property.</param>
        public CharacterInventoryTable(CharacterID @characterID, ItemID @itemID, InventorySlot @slot)
        {
            CharacterID = @characterID;
            ItemID = @itemID;
            Slot = @slot;
        }

        /// <summary>
        /// CharacterInventoryTable constructor.
        /// </summary>
        /// <param name="source">ICharacterInventoryTable to copy the initial values from.</param>
        public CharacterInventoryTable(ICharacterInventoryTable source)
        {
            CopyValuesFrom(source);
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(ICharacterInventoryTable source, IDictionary<String, Object> dic)
        {
            dic["@character_id"] = source.CharacterID;
            dic["@item_id"] = source.ItemID;
            dic["@slot"] = source.Slot;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the values from the given <paramref name="source"/> into this CharacterInventoryTable.
        /// </summary>
        /// <param name="source">The ICharacterInventoryTable to copy the values from.</param>
        public void CopyValuesFrom(ICharacterInventoryTable source)
        {
            CharacterID = source.CharacterID;
            ItemID = source.ItemID;
            Slot = source.Slot;
        }

        /// <summary>
        /// Gets the data for the database column that this table represents.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the data for.</param>
        /// <returns>
        /// The data for the database column with the name <paramref name="columnName"/>.
        /// </returns>
        public static ColumnMetadata GetColumnData(String columnName)
        {
            switch (columnName)
            {
                case "character_id":
                    return new ColumnMetadata("character_id", "", "int(11)", null, typeof(Int32), false, true, false);

                case "item_id":
                    return new ColumnMetadata("item_id", "", "int(11)", null, typeof(Int32), false, false, true);

                case "slot":
                    return new ColumnMetadata("slot", "", "tinyint(3) unsigned", null, typeof(Byte), false, true, false);

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Gets the value of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the value for.</param>
        /// <returns>
        /// The value of the column with the name <paramref name="columnName"/>.
        /// </returns>
        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "character_id":
                    return CharacterID;

                case "item_id":
                    return ItemID;

                case "slot":
                    return Slot;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Sets the <paramref name="value"/> of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
        /// <param name="value">Value to assign to the column.</param>
        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "character_id":
                    CharacterID = (CharacterID)value;
                    break;

                case "item_id":
                    ItemID = (ItemID)value;
                    break;

                case "slot":
                    Slot = (InventorySlot)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region ICharacterInventoryTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public CharacterID CharacterID
        {
            get { return (CharacterID)_characterID; }
            set { _characterID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public ItemID ItemID
        {
            get { return (ItemID)_itemID; }
            set { _itemID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `slot`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public InventorySlot Slot
        {
            get { return (InventorySlot)_slot; }
            set { _slot = (Byte)value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public ICharacterInventoryTable DeepCopy()
        {
            return new CharacterInventoryTable(this);
        }

        #endregion
    }
}