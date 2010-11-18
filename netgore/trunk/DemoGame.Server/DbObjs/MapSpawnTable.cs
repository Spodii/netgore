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
********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.IO;
using NetGore.World;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `map_spawn`.
    /// </summary>
    public class MapSpawnTable : IMapSpawnTable, IPersistable
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
        /// Initializes a new instance of the <see cref="MapSpawnTable"/> class.
        /// </summary>
        public MapSpawnTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnTable"/> class.
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
                             MapID @mapID, ushort? @width, ushort? @x, ushort? @y)
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
        /// Initializes a new instance of the <see cref="MapSpawnTable"/> class.
        /// </summary>
        /// <param name="source">IMapSpawnTable to copy the initial values from.</param>
        public MapSpawnTable(IMapSpawnTable source)
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
        public static void CopyValues(IMapSpawnTable source, IDictionary<String, Object> dic)
        {
            dic["amount"] = source.Amount;
            dic["character_template_id"] = source.CharacterTemplateID;
            dic["height"] = source.Height;
            dic["id"] = source.ID;
            dic["map_id"] = source.MapID;
            dic["width"] = source.Width;
            dic["x"] = source.X;
            dic["y"] = source.Y;
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
        /// Copies the values from the given <paramref name="source"/> into this MapSpawnTable.
        /// </summary>
        /// <param name="source">The IMapSpawnTable to copy the values from.</param>
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
                case "amount":
                    return new ColumnMetadata("amount", "The total number of NPCs this spawner will spawn.", "tinyint(3) unsigned",
                        null, typeof(Byte), false, false, false);

                case "character_template_id":
                    return new ColumnMetadata("character_template_id",
                        "The character template used to instantiate the spawned NPCs.", "smallint(5) unsigned", null,
                        typeof(UInt16), false, false, true);

                case "height":
                    return new ColumnMetadata("height", "The height of the spawner (NULL indicates the bottom- side of the map).",
                        "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "id":
                    return new ColumnMetadata("id", "The unique ID of this NPC spawn.", "int(11)", null, typeof(Int32), false,
                        true, false);

                case "map_id":
                    return new ColumnMetadata("map_id", "The map that this spawn takes place on.", "smallint(5) unsigned", null,
                        typeof(UInt16), false, false, true);

                case "width":
                    return new ColumnMetadata("width", "The width of the spawner (NULL indicates the right-most side of the map).",
                        "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "x":
                    return new ColumnMetadata("x",
                        "The x coordinate of the spawner (NULL indicates the left-most side of the map). Example: All x/y/width/height set to NULL spawns NPCs anywhere on the map.",
                        "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "y":
                    return new ColumnMetadata("y",
                        "The y coordinate of the spawner (NULL indicates the top-most side of the map).", "smallint(5) unsigned",
                        null, typeof(ushort?), true, false, false);

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
        /// Sets the <paramref name="value"/> of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
        /// <param name="value">Value to assign to the column.</param>
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
                    MapID = (MapID)value;
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

        #region IMapSpawnTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `amount`.
        /// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
        /// "The total number of NPCs this spawner will spawn.".
        /// </summary>
        [Description("The total number of NPCs this spawner will spawn.")]
        [SyncValue]
        public Byte Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
        /// "The character template used to instantiate the spawned NPCs.".
        /// </summary>
        [Description("The character template used to instantiate the spawned NPCs.")]
        [SyncValue]
        public CharacterTemplateID CharacterTemplateID
        {
            get { return (CharacterTemplateID)_characterTemplateID; }
            set { _characterTemplateID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `height`.
        /// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
        /// "The height of the spawner (NULL indicates the bottom- side of the map).".
        /// </summary>
        [Description("The height of the spawner (NULL indicates the bottom- side of the map).")]
        [SyncValue]
        public ushort? Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`.The database column contains the comment: 
        /// "The unique ID of this NPC spawn.".
        /// </summary>
        [Description("The unique ID of this NPC spawn.")]
        [SyncValue]
        public MapSpawnValuesID ID
        {
            get { return (MapSpawnValuesID)_iD; }
            set { _iD = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `map_id`.
        /// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
        /// "The map that this spawn takes place on.".
        /// </summary>
        [Description("The map that this spawn takes place on.")]
        [SyncValue]
        public MapID MapID
        {
            get { return (MapID)_mapID; }
            set { _mapID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `width`.
        /// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
        /// "The width of the spawner (NULL indicates the right-most side of the map).".
        /// </summary>
        [Description("The width of the spawner (NULL indicates the right-most side of the map).")]
        [SyncValue]
        public ushort? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `x`.
        /// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
        /// "The x coordinate of the spawner (NULL indicates the left-most side of the map). Example: All x/y/width/height set to NULL spawns NPCs anywhere on the map.".
        /// </summary>
        [Description(
            "The x coordinate of the spawner (NULL indicates the left-most side of the map). Example: All x/y/width/height set to NULL spawns NPCs anywhere on the map."
            )]
        [SyncValue]
        public ushort? X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `y`.
        /// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
        /// "The y coordinate of the spawner (NULL indicates the top-most side of the map).".
        /// </summary>
        [Description("The y coordinate of the spawner (NULL indicates the top-most side of the map).")]
        [SyncValue]
        public ushort? Y
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
        public virtual IMapSpawnTable DeepCopy()
        {
            return new MapSpawnTable(this);
        }

        #endregion

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public virtual void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public virtual void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}