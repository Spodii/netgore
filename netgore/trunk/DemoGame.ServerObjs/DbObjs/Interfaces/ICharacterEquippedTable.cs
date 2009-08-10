using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character_equipped`.
/// </summary>
public interface ICharacterEquippedTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
ICharacterEquippedTable DeepCopy();

/// <summary>
/// Gets the value of the database column `character_id`.
/// </summary>
DemoGame.Server.CharacterID CharacterID
{
get;
}
/// <summary>
/// Gets the value of the database column `item_id`.
/// </summary>
DemoGame.Server.ItemID ItemID
{
get;
}
/// <summary>
/// Gets the value of the database column `slot`.
/// </summary>
DemoGame.EquipmentSlot Slot
{
get;
}
}

}
