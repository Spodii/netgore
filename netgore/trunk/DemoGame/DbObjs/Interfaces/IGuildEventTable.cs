using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `guild_event`.
/// </summary>
public interface IGuildEventTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IGuildEventTable DeepCopy();

/// <summary>
/// Gets the value of the database column `arg0`.
/// </summary>
System.String Arg0
{
get;
}
/// <summary>
/// Gets the value of the database column `arg1`.
/// </summary>
System.String Arg1
{
get;
}
/// <summary>
/// Gets the value of the database column `arg2`.
/// </summary>
System.String Arg2
{
get;
}
/// <summary>
/// Gets the value of the database column `character_id`.
/// </summary>
DemoGame.CharacterID CharacterID
{
get;
}
/// <summary>
/// Gets the value of the database column `created`.
/// </summary>
System.DateTime Created
{
get;
}
/// <summary>
/// Gets the value of the database column `event_id`.
/// </summary>
System.Byte EventId
{
get;
}
/// <summary>
/// Gets the value of the database column `guild_id`.
/// </summary>
System.UInt16 GuildId
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
System.Int32 ID
{
get;
}
/// <summary>
/// Gets the value of the database column `target_character_id`.
/// </summary>
System.Nullable<System.Int32> TargetCharacterId
{
get;
}
}

}
