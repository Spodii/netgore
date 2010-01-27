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
/// Contains extension methods for class GuildEventTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class GuildEventTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IGuildEventTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@arg0"] = (System.String)source.Arg0;
paramValues["@arg1"] = (System.String)source.Arg1;
paramValues["@arg2"] = (System.String)source.Arg2;
paramValues["@character_id"] = (System.Int32)source.CharacterID;
paramValues["@created"] = (System.DateTime)source.Created;
paramValues["@event_id"] = (System.Byte)source.EventID;
paramValues["@guild_id"] = (System.UInt16)source.GuildID;
paramValues["@id"] = (System.Int32)source.ID;
paramValues["@target_character_id"] = (System.Nullable<System.Int32>)source.TargetCharacterID;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this GuildEventTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("arg0");

source.Arg0 = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));

i = dataReader.GetOrdinal("arg1");

source.Arg1 = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));

i = dataReader.GetOrdinal("arg2");

source.Arg2 = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));

i = dataReader.GetOrdinal("character_id");

source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("created");

source.Created = (System.DateTime)(System.DateTime)dataReader.GetDateTime(i);

i = dataReader.GetOrdinal("event_id");

source.EventID = (System.Byte)(System.Byte)dataReader.GetByte(i);

i = dataReader.GetOrdinal("guild_id");

source.GuildID = (NetGore.Features.Guilds.GuildID)(NetGore.Features.Guilds.GuildID)dataReader.GetUInt16(i);

i = dataReader.GetOrdinal("id");

source.ID = (System.Int32)(System.Int32)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("target_character_id");

source.TargetCharacterID = (System.Nullable<DemoGame.CharacterID>)(System.Nullable<DemoGame.CharacterID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.Int32>)null : dataReader.GetInt32(i));
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
public static void TryReadValues(this GuildEventTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "arg0":
source.Arg0 = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));
break;


case "arg1":
source.Arg1 = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));
break;


case "arg2":
source.Arg2 = (System.String)(System.String)(dataReader.IsDBNull(i) ? (System.String)null : dataReader.GetString(i));
break;


case "character_id":
source.CharacterID = (DemoGame.CharacterID)(DemoGame.CharacterID)dataReader.GetInt32(i);
break;


case "created":
source.Created = (System.DateTime)(System.DateTime)dataReader.GetDateTime(i);
break;


case "event_id":
source.EventID = (System.Byte)(System.Byte)dataReader.GetByte(i);
break;


case "guild_id":
source.GuildID = (NetGore.Features.Guilds.GuildID)(NetGore.Features.Guilds.GuildID)dataReader.GetUInt16(i);
break;


case "id":
source.ID = (System.Int32)(System.Int32)dataReader.GetInt32(i);
break;


case "target_character_id":
source.TargetCharacterID = (System.Nullable<DemoGame.CharacterID>)(System.Nullable<DemoGame.CharacterID>)(dataReader.IsDBNull(i) ? (System.Nullable<System.Int32>)null : dataReader.GetInt32(i));
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
public static void TryCopyValues(this IGuildEventTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@arg0":
paramValues[i] = (System.String)source.Arg0;
break;


case "@arg1":
paramValues[i] = (System.String)source.Arg1;
break;


case "@arg2":
paramValues[i] = (System.String)source.Arg2;
break;


case "@character_id":
paramValues[i] = (System.Int32)source.CharacterID;
break;


case "@created":
paramValues[i] = (System.DateTime)source.Created;
break;


case "@event_id":
paramValues[i] = (System.Byte)source.EventID;
break;


case "@guild_id":
paramValues[i] = (System.UInt16)source.GuildID;
break;


case "@id":
paramValues[i] = (System.Int32)source.ID;
break;


case "@target_character_id":
paramValues[i] = (System.Nullable<System.Int32>)source.TargetCharacterID;
break;


}

}
}

}

}
