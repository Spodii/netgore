using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `character_inventory`.
    /// </summary>
    public interface ICharacterInventoryTable
    {
        /// <summary>
        /// Gets the value of the database column `character_id`.
        /// </summary>
        CharacterID CharacterId { get; }

        /// <summary>
        /// Gets the value of the database column `item_id`.
        /// </summary>
        ItemID ItemId { get; }
    }

    /// <summary>
    /// Provides a strongly-typed structure for the database table `character_inventory`.
    /// </summary>
    public class CharacterInventoryTable : ICharacterInventoryTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 2;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_inventory";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "character_id", "item_id" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "character_id", "item_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { };

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        Int32 _characterId;

        /// <summary>
        /// The field that maps onto the database column `item_id`.
        /// </summary>
        Int32 _itemId;

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
        /// <param name="characterId">The initial value for the corresponding property.</param>
        /// <param name="itemId">The initial value for the corresponding property.</param>
        public CharacterInventoryTable(CharacterID @characterId, ItemID @itemId)
        {
            CharacterId = @characterId;
            ItemId = @itemId;
        }

        /// <summary>
        /// CharacterInventoryTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public CharacterInventoryTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

        public CharacterInventoryTable(ICharacterInventoryTable source)
        {
            CopyValuesFrom(source);
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(ICharacterInventoryTable source, IDictionary<String, Object> dic)
        {
            dic["@character_id"] = source.CharacterId;
            dic["@item_id"] = source.ItemId;
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(ICharacterInventoryTable source, DbParameterValues paramValues)
        {
            paramValues["@character_id"] = source.CharacterId;
            paramValues["@item_id"] = source.ItemId;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void CopyValues(DbParameterValues paramValues)
        {
            CopyValues(this, paramValues);
        }

        public void CopyValuesFrom(ICharacterInventoryTable source)
        {
            CharacterId = source.CharacterId;
            ItemId = source.ItemId;
        }

        public static ColumnMetadata GetColumnData(String fieldName)
        {
            switch (fieldName)
            {
                case "character_id":
                    return new ColumnMetadata("character_id", "", "int(11)", null, typeof(Int32), false, true, false);

                case "item_id":
                    return new ColumnMetadata("item_id", "", "int(11)", null, typeof(Int32), false, true, false);

                default:
                    throw new ArgumentException("Field not found.", "fieldName");
            }
        }

        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "character_id":
                    return CharacterId;

                case "item_id":
                    return ItemId;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("character_id");
            CharacterId = (CharacterID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("item_id");
            ItemId = (ItemID)dataReader.GetInt32(i);
        }

        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "character_id":
                    CharacterId = (CharacterID)value;
                    break;

                case "item_id":
                    ItemId = (ItemID)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void TryCopyValues(ICharacterInventoryTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@character_id":
                        paramValues[i] = source.CharacterId;
                        break;

                    case "@item_id":
                        paramValues[i] = source.ItemId;
                        break;
                }
            }
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void TryCopyValues(DbParameterValues paramValues)
        {
            TryCopyValues(this, paramValues);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. Unlike ReadValues(), this method not only doesn't require
        /// all values to be in the IDataReader, but also does not require the values in
        /// the IDataReader to be a defined field for the table this class represents.
        /// Because of this, you need to be careful when using this method because values
        /// can easily be skipped without any indication.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void TryReadValues(IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "character_id":
                        CharacterId = (CharacterID)dataReader.GetInt32(i);
                        break;

                    case "item_id":
                        ItemId = (ItemID)dataReader.GetInt32(i);
                        break;
                }
            }
        }

        #region ICharacterInventoryTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public CharacterID CharacterId
        {
            get { return (CharacterID)_characterId; }
            set { _characterId = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public ItemID ItemId
        {
            get { return (ItemID)_itemId; }
            set { _itemId = (Int32)value; }
        }

        #endregion
    }
}