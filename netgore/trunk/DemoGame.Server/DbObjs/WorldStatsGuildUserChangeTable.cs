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
using NetGore.Features.Guilds;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `world_stats_guild_user_change`.
    /// </summary>
    public class WorldStatsGuildUserChangeTable : IWorldStatsGuildUserChangeTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 3;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "world_stats_guild_user_change";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "guild_id", "user_id", "when" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "guild_id", "user_id", "when" };

        /// <summary>
        /// The field that maps onto the database column `guild_id`.
        /// </summary>
        ushort? _guildID;

        /// <summary>
        /// The field that maps onto the database column `user_id`.
        /// </summary>
        Int32 _userId;

        /// <summary>
        /// The field that maps onto the database column `when`.
        /// </summary>
        DateTime _when;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsGuildUserChangeTable"/> class.
        /// </summary>
        public WorldStatsGuildUserChangeTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsGuildUserChangeTable"/> class.
        /// </summary>
        /// <param name="guildID">The initial value for the corresponding property.</param>
        /// <param name="userId">The initial value for the corresponding property.</param>
        /// <param name="when">The initial value for the corresponding property.</param>
        public WorldStatsGuildUserChangeTable(GuildID? @guildID, CharacterID @userId, DateTime @when)
        {
            GuildID = @guildID;
            UserId = @userId;
            When = @when;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsGuildUserChangeTable"/> class.
        /// </summary>
        /// <param name="source">IWorldStatsGuildUserChangeTable to copy the initial values from.</param>
        public WorldStatsGuildUserChangeTable(IWorldStatsGuildUserChangeTable source)
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
        public static void CopyValues(IWorldStatsGuildUserChangeTable source, IDictionary<String, Object> dic)
        {
            dic["@guild_id"] = source.GuildID;
            dic["@user_id"] = source.UserId;
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
        /// Copies the values from the given <paramref name="source"/> into this WorldStatsGuildUserChangeTable.
        /// </summary>
        /// <param name="source">The IWorldStatsGuildUserChangeTable to copy the values from.</param>
        public void CopyValuesFrom(IWorldStatsGuildUserChangeTable source)
        {
            GuildID = source.GuildID;
            UserId = source.UserId;
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
                case "guild_id":
                    return new ColumnMetadata("guild_id", "The ID of the guild, or null if the user left a guild.",
                                              "smallint(5) unsigned", null, typeof(ushort?), true, false, true);

                case "user_id":
                    return new ColumnMetadata("user_id", "The ID of the user who changed the guild they are part of.", "int(11)",
                                              null, typeof(Int32), false, false, true);

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
                case "guild_id":
                    return GuildID;

                case "user_id":
                    return UserId;

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
                case "guild_id":
                    GuildID = (GuildID?)value;
                    break;

                case "user_id":
                    UserId = (CharacterID)value;
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

        #region IWorldStatsGuildUserChangeTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `guild_id`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The ID of the guild, or null if the user left a guild.".
        /// </summary>
        [Description("The ID of the guild, or null if the user left a guild.")]
        [SyncValue]
        public GuildID? GuildID
        {
            get { return (Nullable<GuildID>)_guildID; }
            set { _guildID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `user_id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The ID of the user who changed the guild they are part of.".
        /// </summary>
        [Description("The ID of the user who changed the guild they are part of.")]
        [SyncValue]
        public CharacterID UserId
        {
            get { return (CharacterID)_userId; }
            set { _userId = (Int32)value; }
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
        public virtual IWorldStatsGuildUserChangeTable DeepCopy()
        {
            return new WorldStatsGuildUserChangeTable(this);
        }

        #endregion
    }
}