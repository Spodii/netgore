/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/dbclasscreator.html

This file was generated on (UTC): 5/21/2010 1:39:24 AM
********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `world_stats_user_level`.
    /// </summary>
    public class WorldStatsUserLevelTable : IWorldStatsUserLevelTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 6;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "world_stats_user_level";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "character_id", "level", "map_id", "when", "x", "y" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "character_id", "level", "map_id", "when", "x", "y" };

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        Int32 _characterID;

        /// <summary>
        /// The field that maps onto the database column `level`.
        /// </summary>
        Byte _level;

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        ushort? _mapID;

        /// <summary>
        /// The field that maps onto the database column `when`.
        /// </summary>
        DateTime _when;

        /// <summary>
        /// The field that maps onto the database column `x`.
        /// </summary>
        UInt16 _x;

        /// <summary>
        /// The field that maps onto the database column `y`.
        /// </summary>
        UInt16 _y;

        /// <summary>
        /// WorldStatsUserLevelTable constructor.
        /// </summary>
        public WorldStatsUserLevelTable()
        {
        }

        /// <summary>
        /// WorldStatsUserLevelTable constructor.
        /// </summary>
        /// <param name="characterID">The initial value for the corresponding property.</param>
        /// <param name="level">The initial value for the corresponding property.</param>
        /// <param name="mapID">The initial value for the corresponding property.</param>
        /// <param name="when">The initial value for the corresponding property.</param>
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public WorldStatsUserLevelTable(CharacterID @characterID, Byte @level, MapID? @mapID, DateTime @when, UInt16 @x, UInt16 @y)
        {
            CharacterID = @characterID;
            Level = @level;
            MapID = @mapID;
            When = @when;
            X = @x;
            Y = @y;
        }

        /// <summary>
        /// WorldStatsUserLevelTable constructor.
        /// </summary>
        /// <param name="source">IWorldStatsUserLevelTable to copy the initial values from.</param>
        public WorldStatsUserLevelTable(IWorldStatsUserLevelTable source)
        {
            CopyValuesFrom(source);
        }

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
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IWorldStatsUserLevelTable source, IDictionary<String, Object> dic)
        {
            dic["@character_id"] = source.CharacterID;
            dic["@level"] = source.Level;
            dic["@map_id"] = source.MapID;
            dic["@when"] = source.When;
            dic["@x"] = source.X;
            dic["@y"] = source.Y;
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
        /// Copies the values from the given <paramref name="source"/> into this WorldStatsUserLevelTable.
        /// </summary>
        /// <param name="source">The IWorldStatsUserLevelTable to copy the values from.</param>
        public void CopyValuesFrom(IWorldStatsUserLevelTable source)
        {
            CharacterID = source.CharacterID;
            Level = source.Level;
            MapID = source.MapID;
            When = source.When;
            X = source.X;
            Y = source.Y;
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
                    return new ColumnMetadata("character_id", "The ID of the character that leveled up.", "int(11)", null,
                                              typeof(Int32), false, false, false);

                case "level":
                    return new ColumnMetadata("level", "The level that the character leveled up to (their new level).",
                                              "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "map_id":
                    return new ColumnMetadata("map_id", "The ID of the map this event took place on.", "smallint(5) unsigned",
                                              null, typeof(ushort?), true, false, false);

                case "when":
                    return new ColumnMetadata("when", "When this event took place.", "timestamp", "CURRENT_TIMESTAMP",
                                              typeof(DateTime), false, false, false);

                case "x":
                    return new ColumnMetadata("x",
                                              "The map x coordinate of the user when this event took place. Only valid when the map_id is not null.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "y":
                    return new ColumnMetadata("y",
                                              "The map y coordinate of the user when this event took place. Only valid when the map_id is not null.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

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

                case "level":
                    return Level;

                case "map_id":
                    return MapID;

                case "when":
                    return When;

                case "x":
                    return X;

                case "y":
                    return Y;

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

                case "level":
                    Level = (Byte)value;
                    break;

                case "map_id":
                    MapID = (MapID?)value;
                    break;

                case "when":
                    When = (DateTime)value;
                    break;

                case "x":
                    X = (UInt16)value;
                    break;

                case "y":
                    Y = (UInt16)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion

        #region IWorldStatsUserLevelTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The ID of the character that leveled up.".
        /// </summary>
        [Description("The ID of the character that leveled up.")]
        [SyncValue]
        public CharacterID CharacterID
        {
            get { return (CharacterID)_characterID; }
            set { _characterID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `level`.
        /// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
        /// "The level that the character leveled up to (their new level).".
        /// </summary>
        [Description("The level that the character leveled up to (their new level).")]
        [SyncValue]
        public Byte Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `map_id`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The ID of the map this event took place on.".
        /// </summary>
        [Description("The ID of the map this event took place on.")]
        [SyncValue]
        public MapID? MapID
        {
            get { return (Nullable<MapID>)_mapID; }
            set { _mapID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `when`.
        /// The underlying database type is `timestamp` with the default value of `CURRENT_TIMESTAMP`. The database column contains the comment: 
        /// "When this event took place.".
        /// </summary>
        [Description("When this event took place.")]
        [SyncValue]
        public DateTime When
        {
            get { return _when; }
            set { _when = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `x`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The map x coordinate of the user when this event took place. Only valid when the map_id is not null.".
        /// </summary>
        [Description("The map x coordinate of the user when this event took place. Only valid when the map_id is not null.")]
        [SyncValue]
        public UInt16 X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `y`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The map y coordinate of the user when this event took place. Only valid when the map_id is not null.".
        /// </summary>
        [Description("The map y coordinate of the user when this event took place. Only valid when the map_id is not null.")]
        [SyncValue]
        public UInt16 Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IWorldStatsUserLevelTable DeepCopy()
        {
            return new WorldStatsUserLevelTable(this);
        }

        #endregion
    }
}