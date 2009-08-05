using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `map_spawn`.
    /// </summary>
    public interface IMapSpawnTable
    {
        /// <summary>
        /// Gets the value for the database column `amount`.
        /// </summary>
        Byte Amount { get; }

        /// <summary>
        /// Gets the value for the database column `character_id`.
        /// </summary>
        UInt16 CharacterId { get; }

        /// <summary>
        /// Gets the value for the database column `height`.
        /// </summary>
        UInt16 Height { get; }

        /// <summary>
        /// Gets the value for the database column `id`.
        /// </summary>
        Int32 Id { get; }

        /// <summary>
        /// Gets the value for the database column `map_id`.
        /// </summary>
        UInt16 MapId { get; }

        /// <summary>
        /// Gets the value for the database column `width`.
        /// </summary>
        UInt16 Width { get; }

        /// <summary>
        /// Gets the value for the database column `x`.
        /// </summary>
        UInt16 X { get; }

        /// <summary>
        /// Gets the value for the database column `y`.
        /// </summary>
        UInt16 Y { get; }
    }

    /// <summary>
    /// Provides a strongly-typed structure for the database table `map_spawn`.
    /// </summary>
    public class MapSpawnTable : IMapSpawnTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 8;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "map_spawn";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
                                              { "amount", "character_id", "height", "id", "map_id", "width", "x", "y" };

        /// <summary>
        /// The field that maps onto the database column `amount`.
        /// </summary>
        Byte _amount;

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        UInt16 _characterId;

        /// <summary>
        /// The field that maps onto the database column `height`.
        /// </summary>
        UInt16 _height;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _id;

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        UInt16 _mapId;

        /// <summary>
        /// The field that maps onto the database column `width`.
        /// </summary>
        UInt16 _width;

        /// <summary>
        /// The field that maps onto the database column `x`.
        /// </summary>
        UInt16 _x;

        /// <summary>
        /// The field that maps onto the database column `y`.
        /// </summary>
        UInt16 _y;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// MapSpawnTable constructor.
        /// </summary>
        public MapSpawnTable()
        {
        }

        /// <summary>
        /// MapSpawnTable constructor.
        /// </summary>
        /// <param name="amount">The initial value for the corresponding property.</param>
        /// <param name="characterId">The initial value for the corresponding property.</param>
        /// <param name="height">The initial value for the corresponding property.</param>
        /// <param name="id">The initial value for the corresponding property.</param>
        /// <param name="mapId">The initial value for the corresponding property.</param>
        /// <param name="width">The initial value for the corresponding property.</param>
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public MapSpawnTable(Byte @amount, UInt16 @characterId, UInt16 @height, Int32 @id, UInt16 @mapId, UInt16 @width, UInt16 @x,
                             UInt16 @y)
        {
            Amount = @amount;
            CharacterId = @characterId;
            Height = @height;
            Id = @id;
            MapId = @mapId;
            Width = @width;
            X = @x;
            Y = @y;
        }

        /// <summary>
        /// MapSpawnTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public MapSpawnTable(IDataReader dataReader)
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
        public static void CopyValues(IMapSpawnTable source, IDictionary<String, Object> dic)
        {
            dic["@amount"] = source.Amount;
            dic["@character_id"] = source.CharacterId;
            dic["@height"] = source.Height;
            dic["@id"] = source.Id;
            dic["@map_id"] = source.MapId;
            dic["@width"] = source.Width;
            dic["@x"] = source.X;
            dic["@y"] = source.Y;
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(IMapSpawnTable source, DbParameterValues paramValues)
        {
            paramValues["@amount"] = source.Amount;
            paramValues["@character_id"] = source.CharacterId;
            paramValues["@height"] = source.Height;
            paramValues["@id"] = source.Id;
            paramValues["@map_id"] = source.MapId;
            paramValues["@width"] = source.Width;
            paramValues["@x"] = source.X;
            paramValues["@y"] = source.Y;
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
            Amount = (Byte)dataReader.GetValue(dataReader.GetOrdinal("amount"));
            CharacterId = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("character_id"));
            Height = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("height"));
            Id = (Int32)dataReader.GetValue(dataReader.GetOrdinal("id"));
            MapId = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("map_id"));
            Width = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("width"));
            X = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("x"));
            Y = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("y"));
        }

        #region IMapSpawnTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `amount`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte Amount
        {
            get { return _amount; }
            set { _amount = value; }
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
        /// Gets or sets the value for the field that maps onto the database column `height`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `map_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 MapId
        {
            get { return _mapId; }
            set { _mapId = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `width`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `x`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `y`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion
    }
}