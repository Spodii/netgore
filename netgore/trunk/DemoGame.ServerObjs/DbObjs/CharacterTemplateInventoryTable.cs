using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `character_template_inventory`.
    /// </summary>
    public interface ICharacterTemplateInventoryTable
    {
        /// <summary>
        /// Gets the value of the database column `chance`.
        /// </summary>
        UInt16 Chance { get; }

        /// <summary>
        /// Gets the value of the database column `character_id`.
        /// </summary>
        CharacterID CharacterId { get; }

        /// <summary>
        /// Gets the value of the database column `item_id`.
        /// </summary>
        ItemID ItemId { get; }

        /// <summary>
        /// Gets the value of the database column `max`.
        /// </summary>
        Byte Max { get; }

        /// <summary>
        /// Gets the value of the database column `min`.
        /// </summary>
        Byte Min { get; }
    }

    /// <summary>
    /// Provides a strongly-typed structure for the database table `character_template_inventory`.
    /// </summary>
    public class CharacterTemplateInventoryTable : ICharacterTemplateInventoryTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 5;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_template_inventory";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "chance", "character_id", "item_id", "max", "min" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "chance", "character_id", "item_id", "max", "min" };

        /// <summary>
        /// The field that maps onto the database column `chance`.
        /// </summary>
        UInt16 _chance;

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        UInt16 _characterId;

        /// <summary>
        /// The field that maps onto the database column `item_id`.
        /// </summary>
        UInt16 _itemId;

        /// <summary>
        /// The field that maps onto the database column `max`.
        /// </summary>
        Byte _max;

        /// <summary>
        /// The field that maps onto the database column `min`.
        /// </summary>
        Byte _min;

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
        /// CharacterTemplateInventoryTable constructor.
        /// </summary>
        public CharacterTemplateInventoryTable()
        {
        }

        /// <summary>
        /// CharacterTemplateInventoryTable constructor.
        /// </summary>
        /// <param name="chance">The initial value for the corresponding property.</param>
        /// <param name="characterId">The initial value for the corresponding property.</param>
        /// <param name="itemId">The initial value for the corresponding property.</param>
        /// <param name="max">The initial value for the corresponding property.</param>
        /// <param name="min">The initial value for the corresponding property.</param>
        public CharacterTemplateInventoryTable(UInt16 @chance, CharacterID @characterId, ItemID @itemId, Byte @max, Byte @min)
        {
            Chance = @chance;
            CharacterId = @characterId;
            ItemId = @itemId;
            Max = @max;
            Min = @min;
        }

        /// <summary>
        /// CharacterTemplateInventoryTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public CharacterTemplateInventoryTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

        public CharacterTemplateInventoryTable(ICharacterTemplateInventoryTable source)
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
        public static void CopyValues(ICharacterTemplateInventoryTable source, IDictionary<String, Object> dic)
        {
            dic["@chance"] = source.Chance;
            dic["@character_id"] = source.CharacterId;
            dic["@item_id"] = source.ItemId;
            dic["@max"] = source.Max;
            dic["@min"] = source.Min;
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(ICharacterTemplateInventoryTable source, DbParameterValues paramValues)
        {
            paramValues["@chance"] = source.Chance;
            paramValues["@character_id"] = source.CharacterId;
            paramValues["@item_id"] = source.ItemId;
            paramValues["@max"] = source.Max;
            paramValues["@min"] = source.Min;
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

        public void CopyValuesFrom(ICharacterTemplateInventoryTable source)
        {
            Chance = source.Chance;
            CharacterId = source.CharacterId;
            ItemId = source.ItemId;
            Max = source.Max;
            Min = source.Min;
        }

        public static ColumnMetadata GetColumnData(String fieldName)
        {
            switch (fieldName)
            {
                case "chance":
                    return new ColumnMetadata("chance", "", "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "character_id":
                    return new ColumnMetadata("character_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, false, true);

                case "item_id":
                    return new ColumnMetadata("item_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, false, true);

                case "max":
                    return new ColumnMetadata("max", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "min":
                    return new ColumnMetadata("min", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                default:
                    throw new ArgumentException("Field not found.", "fieldName");
            }
        }

        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "chance":
                    return Chance;

                case "character_id":
                    return CharacterId;

                case "item_id":
                    return ItemId;

                case "max":
                    return Max;

                case "min":
                    return Min;

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

            i = dataReader.GetOrdinal("chance");
            Chance = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("character_id");
            CharacterId = (CharacterID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("item_id");
            ItemId = (ItemID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("max");
            Max = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("min");
            Min = dataReader.GetByte(i);
        }

        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "chance":
                    Chance = (UInt16)value;
                    break;

                case "character_id":
                    CharacterId = (CharacterID)value;
                    break;

                case "item_id":
                    ItemId = (ItemID)value;
                    break;

                case "max":
                    Max = (Byte)value;
                    break;

                case "min":
                    Min = (Byte)value;
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
        public static void TryCopyValues(ICharacterTemplateInventoryTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@chance":
                        paramValues[i] = source.Chance;
                        break;

                    case "@character_id":
                        paramValues[i] = source.CharacterId;
                        break;

                    case "@item_id":
                        paramValues[i] = source.ItemId;
                        break;

                    case "@max":
                        paramValues[i] = source.Max;
                        break;

                    case "@min":
                        paramValues[i] = source.Min;
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
                    case "chance":
                        Chance = dataReader.GetUInt16(i);
                        break;

                    case "character_id":
                        CharacterId = (CharacterID)dataReader.GetUInt16(i);
                        break;

                    case "item_id":
                        ItemId = (ItemID)dataReader.GetUInt16(i);
                        break;

                    case "max":
                        Max = dataReader.GetByte(i);
                        break;

                    case "min":
                        Min = dataReader.GetByte(i);
                        break;
                }
            }
        }

        #region ICharacterTemplateInventoryTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `chance`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 Chance
        {
            get { return _chance; }
            set { _chance = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public CharacterID CharacterId
        {
            get { return (CharacterID)_characterId; }
            set { _characterId = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ItemID ItemId
        {
            get { return (ItemID)_itemId; }
            set { _itemId = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `max`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `min`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte Min
        {
            get { return _min; }
            set { _min = value; }
        }

        #endregion
    }
}