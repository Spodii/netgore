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
        /// Gets the value for the database column `chance`.
        /// </summary>
        UInt16 Chance { get; }

        /// <summary>
        /// Gets the value for the database column `character_id`.
        /// </summary>
        UInt16 CharacterId { get; }

        /// <summary>
        /// Gets the value for the database column `item_id`.
        /// </summary>
        UInt16 ItemId { get; }

        /// <summary>
        /// Gets the value for the database column `max`.
        /// </summary>
        Byte Max { get; }

        /// <summary>
        /// Gets the value for the database column `min`.
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
        public IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
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
        public CharacterTemplateInventoryTable(UInt16 @chance, UInt16 @characterId, UInt16 @itemId, Byte @max, Byte @min)
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

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            Chance = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("chance"));
            CharacterId = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("character_id"));
            ItemId = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("item_id"));
            Max = (Byte)dataReader.GetValue(dataReader.GetOrdinal("max"));
            Min = (Byte)dataReader.GetValue(dataReader.GetOrdinal("min"));
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
        public UInt16 CharacterId
        {
            get { return _characterId; }
            set { _characterId = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
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