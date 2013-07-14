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
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `map_spawn`.
/// </summary>
public interface IMapSpawnTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IMapSpawnTable DeepCopy();

/// <summary>
/// Gets the value of the database column `amount`.
/// </summary>
System.Byte Amount
{
get;
}
/// <summary>
/// Gets the value of the database column `character_template_id`.
/// </summary>
DemoGame.CharacterTemplateID CharacterTemplateID
{
get;
}
/// <summary>
/// Gets the value of the database column `direction_id`.
/// </summary>
NetGore.Direction DirectionId
{
get;
}
/// <summary>
/// Gets the value of the database column `height`.
/// </summary>
System.Nullable<System.UInt16> Height
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.MapSpawnValuesID ID
{
get;
}
/// <summary>
/// Gets the value of the database column `map_id`.
/// </summary>
NetGore.World.MapID MapID
{
get;
}
/// <summary>
/// Gets the value of the database column `respawn`.
/// </summary>
System.UInt16 Respawn
{
get;
}
/// <summary>
/// Gets the value of the database column `width`.
/// </summary>
System.Nullable<System.UInt16> Width
{
get;
}
/// <summary>
/// Gets the value of the database column `x`.
/// </summary>
System.Nullable<System.UInt16> X
{
get;
}
/// <summary>
/// Gets the value of the database column `y`.
/// </summary>
System.Nullable<System.UInt16> Y
{
get;
}
}

}
