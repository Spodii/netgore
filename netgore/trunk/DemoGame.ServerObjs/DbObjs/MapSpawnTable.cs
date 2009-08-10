using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
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
                                              { "amount", "character_template_id", "height", "id", "map_id", "width", "x", "y" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
                                                    { "amount", "character_template_id", "height", "map_id", "width", "x", "y" };

        /// <summary>
        /// The field that maps onto the database column `amount`.
        /// </summary>
        Byte _amount;

        /// <summary>
        /// The field that maps onto the database column `character_template_id`.
        /// </summary>
        UInt16 _characterTemplateID;

        /// <summary>
        /// The field that maps onto the database column `height`.
        /// </summary>
        ushort? _height;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        UInt16 _mapID;

        /// <summary>
        /// The field that maps onto the database column `width`.
        /// </summary>
        ushort? _width;

        /// <summary>
        /// The field that maps onto the database column `x`.
        /// </summary>
        ushort? _x;

        /// <summary>
        /// The field that maps onto the database column `y`.
        /// </summary>
        ushort? _y;

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
        /// MapSpawnTable constructor.
        /// </summary>
        public MapSpawnTable()
        {
        }

        /// <summary>
        /// MapSpawnTable constructor.
        /// </summary>
        /// <param name="amount">The initial value for the corresponding property.</param>
        /// <param name="characterTemplateID">The initial value for the corresponding property.</param>
        /// <param name="height">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="mapID">The initial value for the corresponding property.</param>
        /// <param name="width">The initial value for the corresponding property.</param>
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public MapSpawnTable(Byte @amount, CharacterTemplateID @characterTemplateID, ushort? @height, MapSpawnValuesID @iD,
                             MapIndex @mapID, ushort? @width, ushort? @x, ushort? @y)
        {
            Amount = @amount;
            CharacterTemplateID = @characterTemplateID;
            Height = @height;
            ID = @iD;
            MapID = @mapID;
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

        public MapSpawnTable(IMapSpawnTable source)
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
        public static void CopyValues(IMapSpawnTable source, IDictionary<String, Object> dic)
        {
            dic["@amount"] = source.Amount;
            dic["@character_template_id"] = source.CharacterTemplateID;
            dic["@height"] = source.Height;
            dic["@id"] = source.ID;
            dic["@map_id"] = source.MapID;
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
            paramValues["@character_template_id"] = source.CharacterTemplateID;
            paramValues["@height"] = source.Height;
            paramValues["@id"] = source.ID;
            paramValues["@map_id"] = source.MapID;
            paramValues["@width"] = source.Width;
            paramValues["@x"] = source.X;
            paramValues["@y"] = source.Y;
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
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void CopyValues(DbParameterValues paramValues)
        {
            CopyValues(this, paramValues);
        }

        public void CopyValuesFrom(IMapSpawnTable source)
        {
            Amount = source.Amount;
            CharacterTemplateID = source.CharacterTemplateID;
            Height = source.Height;
            ID = source.ID;
            MapID = source.MapID;
            Width = source.Width;
            X = source.X;
            Y = source.Y;
        }

        public static ColumnMetadata GetColumnData(String fieldName)
        {
            switch (fieldName)
            {
                case "amount":
                    return new ColumnMetadata("amount", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "character_template_id":
                    return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(UInt16), false,
                                              false, true);

                case "height":
                    return new ColumnMetadata("height", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "id":
                    return new ColumnMetadata("id", "", "int(11)", null, typeof(Int32), false, true, false);

                case "map_id":
                    return new ColumnMetadata("map_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, false, true);

                case "width":
                    return new ColumnMetadata("width", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "x":
                    return new ColumnMetadata("x", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "y":
                    return new ColumnMetadata("y", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                default:
                    throw new ArgumentException("Field not found.", "fieldName");
            }
        }

        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "amount":
                    return Amount;

                case "character_template_id":
                    return CharacterTemplateID;

                case "height":
                    return Height;

                case "id":
                    return ID;

                case "map_id":
                    return MapID;

                case "width":
                    return Width;

                case "x":
                    return X;

                case "y":
                    return Y;

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

            i = dataReader.GetOrdinal("amount");
            Amount = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("character_template_id");
            CharacterTemplateID = (CharacterTemplateID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("height");
            Height = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("id");
            ID = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("map_id");
            MapID = (MapIndex)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("width");
            Width = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("x");
            X = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("y");
            Y = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
        }

        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "amount":
                    Amount = (Byte)value;
                    break;

                case "character_template_id":
                    CharacterTemplateID = (CharacterTemplateID)value;
                    break;

                case "height":
                    Height = (ushort?)value;
                    break;

                case "id":
                    ID = (MapSpawnValuesID)value;
                    break;

                case "map_id":
                    MapID = (MapIndex)value;
                    break;

                case "width":
                    Width = (ushort?)value;
                    break;

                case "x":
                    X = (ushort?)value;
                    break;

                case "y":
                    Y = (ushort?)value;
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
        public static void TryCopyValues(IMapSpawnTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@amount":
                        paramValues[i] = source.Amount;
                        break;

                    case "@character_template_id":
                        paramValues[i] = source.CharacterTemplateID;
                        break;

                    case "@height":
                        paramValues[i] = source.Height;
                        break;

                    case "@id":
                        paramValues[i] = source.ID;
                        break;

                    case "@map_id":
                        paramValues[i] = source.MapID;
                        break;

                    case "@width":
                        paramValues[i] = source.Width;
                        break;

                    case "@x":
                        paramValues[i] = source.X;
                        break;

                    case "@y":
                        paramValues[i] = source.Y;
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
                    case "amount":
                        Amount = dataReader.GetByte(i);
                        break;

                    case "character_template_id":
                        CharacterTemplateID = (CharacterTemplateID)dataReader.GetUInt16(i);
                        break;

                    case "height":
                        Height = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "id":
                        ID = dataReader.GetInt32(i);
                        break;

                    case "map_id":
                        MapID = (MapIndex)dataReader.GetUInt16(i);
                        break;

                    case "width":
                        Width = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "x":
                        X = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "y":
                        Y = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;
                }
            }
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
        /// Gets or sets the value for the field that maps onto the database column `character_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public CharacterTemplateID CharacterTemplateID
        {
            get { return (CharacterTemplateID)_characterTemplateID; }
            set { _characterTemplateID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `height`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ushort? Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public MapSpawnValuesID ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `map_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public MapIndex MapID
        {
            get { return (MapIndex)_mapID; }
            set { _mapID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `width`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ushort? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `x`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ushort? X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `y`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ushort? Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion
    }
}