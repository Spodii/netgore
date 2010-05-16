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

This file was generated on (UTC): 5/16/2010 7:45:19 PM
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
    /// Provides a strongly-typed structure for the database table `guild_member`.
    /// </summary>
    public class GuildMemberTable : IGuildMemberTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 4;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "guild_member";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "character_id", "guild_id", "joined", "rank" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "character_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "guild_id", "joined", "rank" };

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        Int32 _characterID;

        /// <summary>
        /// The field that maps onto the database column `guild_id`.
        /// </summary>
        UInt16 _guildID;

        /// <summary>
        /// The field that maps onto the database column `joined`.
        /// </summary>
        DateTime _joined;

        /// <summary>
        /// The field that maps onto the database column `rank`.
        /// </summary>
        Byte _rank;

        /// <summary>
        /// GuildMemberTable constructor.
        /// </summary>
        public GuildMemberTable()
        {
        }

        /// <summary>
        /// GuildMemberTable constructor.
        /// </summary>
        /// <param name="characterID">The initial value for the corresponding property.</param>
        /// <param name="guildID">The initial value for the corresponding property.</param>
        /// <param name="joined">The initial value for the corresponding property.</param>
        /// <param name="rank">The initial value for the corresponding property.</param>
        public GuildMemberTable(CharacterID @characterID, GuildID @guildID, DateTime @joined, GuildRank @rank)
        {
            CharacterID = @characterID;
            GuildID = @guildID;
            Joined = @joined;
            Rank = @rank;
        }

        /// <summary>
        /// GuildMemberTable constructor.
        /// </summary>
        /// <param name="source">IGuildMemberTable to copy the initial values from.</param>
        public GuildMemberTable(IGuildMemberTable source)
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
        public static void CopyValues(IGuildMemberTable source, IDictionary<String, Object> dic)
        {
            dic["@character_id"] = source.CharacterID;
            dic["@guild_id"] = source.GuildID;
            dic["@joined"] = source.Joined;
            dic["@rank"] = source.Rank;
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
        /// Copies the values from the given <paramref name="source"/> into this GuildMemberTable.
        /// </summary>
        /// <param name="source">The IGuildMemberTable to copy the values from.</param>
        public void CopyValuesFrom(IGuildMemberTable source)
        {
            CharacterID = source.CharacterID;
            GuildID = source.GuildID;
            Joined = source.Joined;
            Rank = source.Rank;
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
                    return new ColumnMetadata("character_id", "The character that is a member of the guild.", "int(11)", null,
                                              typeof(Int32), false, true, false);

                case "guild_id":
                    return new ColumnMetadata("guild_id", "The guild the member is a part of.", "smallint(5) unsigned", null,
                                              typeof(UInt16), false, false, true);

                case "joined":
                    return new ColumnMetadata("joined", "When the member joined the guild.", "datetime", null, typeof(DateTime),
                                              false, false, false);

                case "rank":
                    return new ColumnMetadata("rank", "The member's ranking in the guild.", "tinyint(3) unsigned", null,
                                              typeof(Byte), false, false, false);

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

                case "guild_id":
                    return GuildID;

                case "joined":
                    return Joined;

                case "rank":
                    return Rank;

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

                case "guild_id":
                    GuildID = (GuildID)value;
                    break;

                case "joined":
                    Joined = (DateTime)value;
                    break;

                case "rank":
                    Rank = (GuildRank)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IGuildMemberTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The character that is a member of the guild.".
        /// </summary>
        [Description("The character that is a member of the guild.")]
        [SyncValue]
        public CharacterID CharacterID
        {
            get { return (CharacterID)_characterID; }
            set { _characterID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `guild_id`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The guild the member is a part of.".
        /// </summary>
        [Description("The guild the member is a part of.")]
        [SyncValue]
        public GuildID GuildID
        {
            get { return (GuildID)_guildID; }
            set { _guildID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `joined`.
        /// The underlying database type is `datetime`. The database column contains the comment: 
        /// "When the member joined the guild.".
        /// </summary>
        [Description("When the member joined the guild.")]
        [SyncValue]
        public DateTime Joined
        {
            get { return _joined; }
            set { _joined = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `rank`.
        /// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
        /// "The member's ranking in the guild.".
        /// </summary>
        [Description("The member's ranking in the guild.")]
        [SyncValue]
        public GuildRank Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IGuildMemberTable DeepCopy()
        {
            return new GuildMemberTable(this);
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