using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character_inventory`.
/// </summary>
public interface ICharacterInventoryTable
{
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
}

}
