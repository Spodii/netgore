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
    http://www.netgore.com/wiki/DbClassCreator
********************************************************************/

using System;
using System.Linq;
using NetGore;
using NetGore.IO;
using System.Collections.Generic;
using System.Collections;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `guild_member`.
/// </summary>
public class GuildMemberTable : IGuildMemberTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"character_id", "guild_id", "joined", "rank" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
}
}
/// <summary>
/// Array of the database column names for columns that are primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsKeys = new string[] {"character_id" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsKeys;
}
}
/// <summary>
/// Array of the database column names for columns that are not primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"guild_id", "joined", "rank" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbNonKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsNonKey;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "guild_member";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 4;
/// <summary>
/// The field that maps onto the database column `character_id`.
/// </summary>
System.Int32 _characterID;
/// <summary>
/// The field that maps onto the database column `guild_id`.
/// </summary>
System.UInt16 _guildID;
/// <summary>
/// The field that maps onto the database column `joined`.
/// </summary>
System.DateTime _joined;
/// <summary>
/// The field that maps onto the database column `rank`.
/// </summary>
System.Byte _rank;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `character_id`.
/// The underlying database type is `int(11)`.The database column contains the comment: 
/// "The character that is a member of the guild.".
/// </summary>
[System.ComponentModel.Description("The character that is a member of the guild.")]
[NetGore.SyncValueAttribute()]
public DemoGame.CharacterID CharacterID
{
get
{
return (DemoGame.CharacterID)_characterID;
}
set
{
this._characterID = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `guild_id`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "The guild the member is a part of.".
/// </summary>
[System.ComponentModel.Description("The guild the member is a part of.")]
[NetGore.SyncValueAttribute()]
public NetGore.Features.Guilds.GuildID GuildID
{
get
{
return (NetGore.Features.Guilds.GuildID)_guildID;
}
set
{
this._guildID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `joined`.
/// The underlying database type is `datetime`.The database column contains the comment: 
/// "When the member joined the guild.".
/// </summary>
[System.ComponentModel.Description("When the member joined the guild.")]
[NetGore.SyncValueAttribute()]
public System.DateTime Joined
{
get
{
return (System.DateTime)_joined;
}
set
{
this._joined = (System.DateTime)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `rank`.
/// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
/// "The member's ranking in the guild.".
/// </summary>
[System.ComponentModel.Description("The member's ranking in the guild.")]
[NetGore.SyncValueAttribute()]
public NetGore.Features.Guilds.GuildRank Rank
{
get
{
return (NetGore.Features.Guilds.GuildRank)_rank;
}
set
{
this._rank = (System.Byte)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public virtual IGuildMemberTable DeepCopy()
{
return new GuildMemberTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="GuildMemberTable"/> class.
/// </summary>
public GuildMemberTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="GuildMemberTable"/> class.
/// </summary>
/// <param name="characterID">The initial value for the corresponding property.</param>
/// <param name="guildID">The initial value for the corresponding property.</param>
/// <param name="joined">The initial value for the corresponding property.</param>
/// <param name="rank">The initial value for the corresponding property.</param>
public GuildMemberTable(DemoGame.CharacterID @characterID, NetGore.Features.Guilds.GuildID @guildID, System.DateTime @joined, NetGore.Features.Guilds.GuildRank @rank)
{
this.CharacterID = (DemoGame.CharacterID)@characterID;
this.GuildID = (NetGore.Features.Guilds.GuildID)@guildID;
this.Joined = (System.DateTime)@joined;
this.Rank = (NetGore.Features.Guilds.GuildRank)@rank;
}
/// <summary>
/// Initializes a new instance of the <see cref="GuildMemberTable"/> class.
/// </summary>
/// <param name="source">IGuildMemberTable to copy the initial values from.</param>
public GuildMemberTable(IGuildMemberTable source)
{
CopyValuesFrom(source);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(IGuildMemberTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["character_id"] = (DemoGame.CharacterID)source.CharacterID;
dic["guild_id"] = (NetGore.Features.Guilds.GuildID)source.GuildID;
dic["joined"] = (System.DateTime)source.Joined;
dic["rank"] = (NetGore.Features.Guilds.GuildRank)source.Rank;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this GuildMemberTable.
/// </summary>
/// <param name="source">The IGuildMemberTable to copy the values from.</param>
public void CopyValuesFrom(IGuildMemberTable source)
{
this.CharacterID = (DemoGame.CharacterID)source.CharacterID;
this.GuildID = (NetGore.Features.Guilds.GuildID)source.GuildID;
this.Joined = (System.DateTime)source.Joined;
this.Rank = (NetGore.Features.Guilds.GuildRank)source.Rank;
}

/// <summary>
/// Gets the value of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the value for.</param>
/// <returns>
/// The value of the column with the name <paramref name="columnName"/>.
/// </returns>
public System.Object GetValue(System.String columnName)
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
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Sets the <paramref name="value"/> of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
/// <param name="value">Value to assign to the column.</param>
public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "character_id":
this.CharacterID = (DemoGame.CharacterID)value;
break;

case "guild_id":
this.GuildID = (NetGore.Features.Guilds.GuildID)value;
break;

case "joined":
this.Joined = (System.DateTime)value;
break;

case "rank":
this.Rank = (NetGore.Features.Guilds.GuildRank)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Gets the data for the database column that this table represents.
/// </summary>
/// <param name="columnName">The database name of the column to get the data for.</param>
/// <returns>
/// The data for the database column with the name <paramref name="columnName"/>.
/// </returns>
public static ColumnMetadata GetColumnData(System.String columnName)
{
switch (columnName)
{
case "character_id":
return new ColumnMetadata("character_id", "The character that is a member of the guild.", "int(11)", null, typeof(System.Int32), false, true, false);

case "guild_id":
return new ColumnMetadata("guild_id", "The guild the member is a part of.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, true);

case "joined":
return new ColumnMetadata("joined", "When the member joined the guild.", "datetime", null, typeof(System.DateTime), false, false, false);

case "rank":
return new ColumnMetadata("rank", "The member's ranking in the guild.", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public virtual void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public virtual void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
