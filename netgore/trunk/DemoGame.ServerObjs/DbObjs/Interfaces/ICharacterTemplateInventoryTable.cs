using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character_template_inventory`.
/// </summary>
public interface ICharacterTemplateInventoryTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
ICharacterTemplateInventoryTable DeepCopy();

/// <summary>
/// Gets the value of the database column `chance`.
/// </summary>
DemoGame.Server.ItemChance Chance
{
get;
}
/// <summary>
/// Gets the value of the database column `character_template_id`.
/// </summary>
DemoGame.Server.CharacterTemplateID CharacterTemplateID
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
/// Gets the value of the database column `item_template_id`.
/// </summary>
DemoGame.Server.ItemTemplateID ItemTemplateID
{
get;
}
/// <summary>
/// Gets the value of the database column `max`.
/// </summary>
System.Byte Max
{
get;
}
/// <summary>
/// Gets the value of the database column `min`.
/// </summary>
System.Byte Min
{
get;
}
}

}
