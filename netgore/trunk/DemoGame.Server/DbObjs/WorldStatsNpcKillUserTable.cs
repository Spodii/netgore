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

This file was generated on (UTC): 5/31/2010 6:31:05 PM
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
    /// Provides a strongly-typed structure for the database table `world_stats_npc_kill_user`.
    /// </summary>
    public class WorldStatsNpcKillUserTable : IWorldStatsNpcKillUserTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 9;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "world_stats_npc_kill_user";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        { "map_id", "npc_template_id", "npc_x", "npc_y", "user_id", "user_level", "user_x", "user_y", "when" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "map_id", "npc_template_id", "npc_x", "npc_y", "user_id", "user_level", "user_x", "user_y", "when" };

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        ushort? _mapID;

        /// <summary>
        /// The field that maps onto the database column `npc_template_id`.
        /// </summary>
        ushort? _npcTemplateId;

        /// <summary>
        /// The field that maps onto the database column `npc_x`.
        /// </summary>
        UInt16 _npcX;

        /// <summary>
        /// The field that maps onto the database column `npc_y`.
        /// </summary>
        UInt16 _npcY;

        /// <summary>
        /// The field that maps onto the database column `user_id`.
        /// </summary>
        Int32 _userId;

        /// <summary>
        /// The field that maps onto the database column `user_level`.
        /// </summary>
        Byte _userLevel;

        /// <summary>
        /// The field that maps onto the database column `user_x`.
        /// </summary>
        UInt16 _userX;

        /// <summary>
        /// The field that maps onto the database column `user_y`.
        /// </summary>
        UInt16 _userY;

        /// <summary>
        /// The field that maps onto the database column `when`.
        /// </summary>
        DateTime _when;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsNpcKillUserTable"/> class.
        /// </summary>
        public WorldStatsNpcKillUserTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsNpcKillUserTable"/> class.
        /// </summary>
        /// <param name="mapID">The initial value for the corresponding property.</param>
        /// <param name="npcTemplateId">The initial value for the corresponding property.</param>
        /// <param name="npcX">The initial value for the corresponding property.</param>
        /// <param name="npcY">The initial value for the corresponding property.</param>
        /// <param name="userId">The initial value for the corresponding property.</param>
        /// <param name="userLevel">The initial value for the corresponding property.</param>
        /// <param name="userX">The initial value for the corresponding property.</param>
        /// <param name="userY">The initial value for the corresponding property.</param>
        /// <param name="when">The initial value for the corresponding property.</param>
        public WorldStatsNpcKillUserTable(MapID? @mapID, CharacterTemplateID? @npcTemplateId, UInt16 @npcX, UInt16 @npcY,
                                          CharacterID @userId, Byte @userLevel, UInt16 @userX, UInt16 @userY, DateTime @when)
        {
            MapID = @mapID;
            NpcTemplateId = @npcTemplateId;
            NpcX = @npcX;
            NpcY = @npcY;
            UserId = @userId;
            UserLevel = @userLevel;
            UserX = @userX;
            UserY = @userY;
            When = @when;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsNpcKillUserTable"/> class.
        /// </summary>
        /// <param name="source">IWorldStatsNpcKillUserTable to copy the initial values from.</param>
        public WorldStatsNpcKillUserTable(IWorldStatsNpcKillUserTable source)
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
        public static void CopyValues(IWorldStatsNpcKillUserTable source, IDictionary<String, Object> dic)
        {
            dic["@map_id"] = source.MapID;
            dic["@npc_template_id"] = source.NpcTemplateId;
            dic["@npc_x"] = source.NpcX;
            dic["@npc_y"] = source.NpcY;
            dic["@user_id"] = source.UserId;
            dic["@user_level"] = source.UserLevel;
            dic["@user_x"] = source.UserX;
            dic["@user_y"] = source.UserY;
            dic["@when"] = source.When;
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
        /// Copies the values from the given <paramref name="source"/> into this WorldStatsNpcKillUserTable.
        /// </summary>
        /// <param name="source">The IWorldStatsNpcKillUserTable to copy the values from.</param>
        public void CopyValuesFrom(IWorldStatsNpcKillUserTable source)
        {
            MapID = source.MapID;
            NpcTemplateId = source.NpcTemplateId;
            NpcX = source.NpcX;
            NpcY = source.NpcY;
            UserId = source.UserId;
            UserLevel = source.UserLevel;
            UserX = source.UserX;
            UserY = source.UserY;
            When = source.When;
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
                case "map_id":
                    return new ColumnMetadata("map_id", "The ID of the map this event took place on.", "smallint(5) unsigned",
                                              null, typeof(ushort?), true, false, true);

                case "npc_template_id":
                    return new ColumnMetadata("npc_template_id",
                                              "The template ID of the NPC. Only valid when the NPC has a template ID set.",
                                              "smallint(5) unsigned", null, typeof(ushort?), true, false, true);

                case "npc_x":
                    return new ColumnMetadata("npc_x",
                                              "The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "npc_y":
                    return new ColumnMetadata("npc_y",
                                              "The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "user_id":
                    return new ColumnMetadata("user_id", "The ID of the user.", "int(11)", null, typeof(Int32), false, false, true);

                case "user_level":
                    return new ColumnMetadata("user_level", "The level of the user was when this event took place.",
                                              "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "user_x":
                    return new ColumnMetadata("user_x", "The map x coordinate of the user when this event took place.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "user_y":
                    return new ColumnMetadata("user_y", "The map y coordinate of the user when this event took place.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "when":
                    return new ColumnMetadata("when", "When this event took place.", "timestamp", "CURRENT_TIMESTAMP",
                                              typeof(DateTime), false, false, false);

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
                case "map_id":
                    return MapID;

                case "npc_template_id":
                    return NpcTemplateId;

                case "npc_x":
                    return NpcX;

                case "npc_y":
                    return NpcY;

                case "user_id":
                    return UserId;

                case "user_level":
                    return UserLevel;

                case "user_x":
                    return UserX;

                case "user_y":
                    return UserY;

                case "when":
                    return When;

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
                case "map_id":
                    MapID = (MapID?)value;
                    break;

                case "npc_template_id":
                    NpcTemplateId = (CharacterTemplateID?)value;
                    break;

                case "npc_x":
                    NpcX = (UInt16)value;
                    break;

                case "npc_y":
                    NpcY = (UInt16)value;
                    break;

                case "user_id":
                    UserId = (CharacterID)value;
                    break;

                case "user_level":
                    UserLevel = (Byte)value;
                    break;

                case "user_x":
                    UserX = (UInt16)value;
                    break;

                case "user_y":
                    UserY = (UInt16)value;
                    break;

                case "when":
                    When = (DateTime)value;
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

        #region IWorldStatsNpcKillUserTable Members

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
        /// Gets or sets the value for the field that maps onto the database column `npc_template_id`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The template ID of the NPC. Only valid when the NPC has a template ID set.".
        /// </summary>
        [Description("The template ID of the NPC. Only valid when the NPC has a template ID set.")]
        [SyncValue]
        public CharacterTemplateID? NpcTemplateId
        {
            get { return (Nullable<CharacterTemplateID>)_npcTemplateId; }
            set { _npcTemplateId = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `npc_x`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.".
        /// </summary>
        [Description("The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.")]
        [SyncValue]
        public UInt16 NpcX
        {
            get { return _npcX; }
            set { _npcX = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `npc_y`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.".
        /// </summary>
        [Description("The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.")]
        [SyncValue]
        public UInt16 NpcY
        {
            get { return _npcY; }
            set { _npcY = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `user_id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The ID of the user.".
        /// </summary>
        [Description("The ID of the user.")]
        [SyncValue]
        public CharacterID UserId
        {
            get { return (CharacterID)_userId; }
            set { _userId = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `user_level`.
        /// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
        /// "The level of the user was when this event took place.".
        /// </summary>
        [Description("The level of the user was when this event took place.")]
        [SyncValue]
        public Byte UserLevel
        {
            get { return _userLevel; }
            set { _userLevel = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `user_x`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The map x coordinate of the user when this event took place.".
        /// </summary>
        [Description("The map x coordinate of the user when this event took place.")]
        [SyncValue]
        public UInt16 UserX
        {
            get { return _userX; }
            set { _userX = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `user_y`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The map y coordinate of the user when this event took place.".
        /// </summary>
        [Description("The map y coordinate of the user when this event took place.")]
        [SyncValue]
        public UInt16 UserY
        {
            get { return _userY; }
            set { _userY = value; }
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
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public virtual IWorldStatsNpcKillUserTable DeepCopy()
        {
            return new WorldStatsNpcKillUserTable(this);
        }

        #endregion
    }
}