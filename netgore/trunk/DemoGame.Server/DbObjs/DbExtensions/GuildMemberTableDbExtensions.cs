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
/// Contains extension methods for class GuildMemberTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class GuildMemberTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IGuildMemberTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@character_id"] = (System.Int32)source.CharacterID;
paramValues["@guild_id"] = (System.UInt16)source.GuildId;
paramValues["@joined"] = (System.DateTime)source.Joined;
paramValues["@rank"] = (System.Byte)source.Rank;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this GuildMemberTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("character_id");

source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("guild_id");

source.GuildId = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("joined");

source.Joined = (System.DateTime)(System.DateTime)dataReader.GetDateTime(i);

i = dataReader.GetOrdinal("rank");

source.Rank = (System.Byte)(System.Byte)dataReader.GetByte(i);
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the IDataReader, but also does not require the values in
/// the IDataReader to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this GuildMemberTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "character_id":
source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataReader.GetInt32(i);
break;


case "guild_id":
source.GuildId = (System.UInt16)(System.UInt16)dataReader.GetUInt16(i);
break;


case "joined":
source.Joined = (System.DateTime)(System.DateTime)dataReader.GetDateTime(i);
break;


case "rank":
source.Rank = (System.Byte)(System.Byte)dataReader.GetByte(i);
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
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void TryCopyValues(this IGuildMemberTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@character_id":
paramValues[i] = (System.Int32)source.CharacterID;
break;


case "@guild_id":
paramValues[i] = (System.UInt16)source.GuildId;
break;


case "@joined":
paramValues[i] = (System.DateTime)source.Joined;
break;


case "@rank":
paramValues[i] = (System.Byte)source.Rank;
break;


}

}
}

}

}
