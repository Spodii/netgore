using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `guild_member`.
/// </summary>
public interface IGuildMemberTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IGuildMemberTable DeepCopy();

/// <summary>
/// Gets the value of the database column `character_id`.
/// </summary>
DemoGame.CharacterID CharacterID
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
/// Gets the value of the database column `joined`.
/// </summary>
System.DateTime Joined
{
get;
}
/// <summary>
/// Gets the value of the database column `rank`.
/// </summary>
System.Byte Rank
{
get;
}
}

}
