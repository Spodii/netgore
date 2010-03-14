using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Features.Guilds;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `guild_event`.
    /// </summary>
    public class GuildEventTable : IGuildEventTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 9;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "guild_event";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        { "arg0", "arg1", "arg2", "character_id", "created", "event_id", "guild_id", "id", "target_character_id" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "arg0", "arg1", "arg2", "character_id", "created", "event_id", "guild_id", "target_character_id" };

        /// <summary>
        /// The field that maps onto the database column `arg0`.
        /// </summary>
        String _arg0;

        /// <summary>
        /// The field that maps onto the database column `arg1`.
        /// </summary>
        String _arg1;

        /// <summary>
        /// The field that maps onto the database column `arg2`.
        /// </summary>
        String _arg2;

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        Int32 _characterID;

        /// <summary>
        /// The field that maps onto the database column `created`.
        /// </summary>
        DateTime _created;

        /// <summary>
        /// The field that maps onto the database column `event_id`.
        /// </summary>
        Byte _eventID;

        /// <summary>
        /// The field that maps onto the database column `guild_id`.
        /// </summary>
        UInt16 _guildID;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `target_character_id`.
        /// </summary>
        int? _targetCharacterID;

        /// <summary>
        /// GuildEventTable constructor.
        /// </summary>
        public GuildEventTable()
        {
        }

        /// <summary>
        /// GuildEventTable constructor.
        /// </summary>
        /// <param name="arg0">The initial value for the corresponding property.</param>
        /// <param name="arg1">The initial value for the corresponding property.</param>
        /// <param name="arg2">The initial value for the corresponding property.</param>
        /// <param name="characterID">The initial value for the corresponding property.</param>
        /// <param name="created">The initial value for the corresponding property.</param>
        /// <param name="eventID">The initial value for the corresponding property.</param>
        /// <param name="guildID">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="targetCharacterID">The initial value for the corresponding property.</param>
        public GuildEventTable(String @arg0, String @arg1, String @arg2, CharacterID @characterID, DateTime @created,
                               Byte @eventID, GuildID @guildID, Int32 @iD, CharacterID? @targetCharacterID)
        {
            Arg0 = @arg0;
            Arg1 = @arg1;
            Arg2 = @arg2;
            CharacterID = @characterID;
            Created = @created;
            EventID = @eventID;
            GuildID = @guildID;
            ID = @iD;
            TargetCharacterID = @targetCharacterID;
        }

        /// <summary>
        /// GuildEventTable constructor.
        /// </summary>
        /// <param name="source">IGuildEventTable to copy the initial values from.</param>
        public GuildEventTable(IGuildEventTable source)
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
        public static void CopyValues(IGuildEventTable source, IDictionary<String, Object> dic)
        {
            dic["@arg0"] = source.Arg0;
            dic["@arg1"] = source.Arg1;
            dic["@arg2"] = source.Arg2;
            dic["@character_id"] = source.CharacterID;
            dic["@created"] = source.Created;
            dic["@event_id"] = source.EventID;
            dic["@guild_id"] = source.GuildID;
            dic["@id"] = source.ID;
            dic["@target_character_id"] = source.TargetCharacterID;
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
        /// Copies the values from the given <paramref name="source"/> into this GuildEventTable.
        /// </summary>
        /// <param name="source">The IGuildEventTable to copy the values from.</param>
        public void CopyValuesFrom(IGuildEventTable source)
        {
            Arg0 = source.Arg0;
            Arg1 = source.Arg1;
            Arg2 = source.Arg2;
            CharacterID = source.CharacterID;
            Created = source.Created;
            EventID = source.EventID;
            GuildID = source.GuildID;
            ID = source.ID;
            TargetCharacterID = source.TargetCharacterID;
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
                case "arg0":
                    return new ColumnMetadata("arg0", "The first optional event argument.", "varchar(0)", null, typeof(String),
                                              true, false, false);

                case "arg1":
                    return new ColumnMetadata("arg1", "The second optional event argument.", "varchar(0)", null, typeof(String),
                                              true, false, false);

                case "arg2":
                    return new ColumnMetadata("arg2", "The third optional event argument.", "varchar(0)", null, typeof(String),
                                              true, false, false);

                case "character_id":
                    return new ColumnMetadata("character_id", "The character that invoked the event.", "int(11)", null,
                                              typeof(Int32), false, false, true);

                case "created":
                    return new ColumnMetadata("created", "When the event was created.", "datetime", null, typeof(DateTime), false,
                                              false, false);

                case "event_id":
                    return new ColumnMetadata("event_id", "The ID of the event that took place.", "tinyint(3) unsigned", null,
                                              typeof(Byte), false, false, false);

                case "guild_id":
                    return new ColumnMetadata("guild_id", "The guild the event took place on.", "smallint(5) unsigned", null,
                                              typeof(UInt16), false, false, true);

                case "id":
                    return new ColumnMetadata("id", "The ID of the event.", "int(11)", null, typeof(Int32), false, true, false);

                case "target_character_id":
                    return new ColumnMetadata("target_character_id", "The optional character that the event involves.", "int(11)",
                                              null, typeof(int?), true, false, true);

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
                case "arg0":
                    return Arg0;

                case "arg1":
                    return Arg1;

                case "arg2":
                    return Arg2;

                case "character_id":
                    return CharacterID;

                case "created":
                    return Created;

                case "event_id":
                    return EventID;

                case "guild_id":
                    return GuildID;

                case "id":
                    return ID;

                case "target_character_id":
                    return TargetCharacterID;

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
                case "arg0":
                    Arg0 = (String)value;
                    break;

                case "arg1":
                    Arg1 = (String)value;
                    break;

                case "arg2":
                    Arg2 = (String)value;
                    break;

                case "character_id":
                    CharacterID = (CharacterID)value;
                    break;

                case "created":
                    Created = (DateTime)value;
                    break;

                case "event_id":
                    EventID = (Byte)value;
                    break;

                case "guild_id":
                    GuildID = (GuildID)value;
                    break;

                case "id":
                    ID = (Int32)value;
                    break;

                case "target_character_id":
                    TargetCharacterID = (CharacterID?)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IGuildEventTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `arg0`.
        /// The underlying database type is `varchar(0)`. The database column contains the comment: 
        /// "The first optional event argument.".
        /// </summary>
        [Description("The first optional event argument.")]
        [SyncValue]
        public String Arg0
        {
            get { return _arg0; }
            set { _arg0 = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `arg1`.
        /// The underlying database type is `varchar(0)`. The database column contains the comment: 
        /// "The second optional event argument.".
        /// </summary>
        [Description("The second optional event argument.")]
        [SyncValue]
        public String Arg1
        {
            get { return _arg1; }
            set { _arg1 = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `arg2`.
        /// The underlying database type is `varchar(0)`. The database column contains the comment: 
        /// "The third optional event argument.".
        /// </summary>
        [Description("The third optional event argument.")]
        [SyncValue]
        public String Arg2
        {
            get { return _arg2; }
            set { _arg2 = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The character that invoked the event.".
        /// </summary>
        [Description("The character that invoked the event.")]
        [SyncValue]
        public CharacterID CharacterID
        {
            get { return (CharacterID)_characterID; }
            set { _characterID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `created`.
        /// The underlying database type is `datetime`. The database column contains the comment: 
        /// "When the event was created.".
        /// </summary>
        [Description("When the event was created.")]
        [SyncValue]
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `event_id`.
        /// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
        /// "The ID of the event that took place.".
        /// </summary>
        [Description("The ID of the event that took place.")]
        [SyncValue]
        public Byte EventID
        {
            get { return _eventID; }
            set { _eventID = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `guild_id`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The guild the event took place on.".
        /// </summary>
        [Description("The guild the event took place on.")]
        [SyncValue]
        public GuildID GuildID
        {
            get { return (GuildID)_guildID; }
            set { _guildID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The ID of the event.".
        /// </summary>
        [Description("The ID of the event.")]
        [SyncValue]
        public Int32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `target_character_id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The optional character that the event involves.".
        /// </summary>
        [Description("The optional character that the event involves.")]
        [SyncValue]
        public CharacterID? TargetCharacterID
        {
            get { return (CharacterID?)_targetCharacterID; }
            set { _targetCharacterID = (int?)value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IGuildEventTable DeepCopy()
        {
            return new GuildEventTable(this);
        }

        #endregion

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
    }
}