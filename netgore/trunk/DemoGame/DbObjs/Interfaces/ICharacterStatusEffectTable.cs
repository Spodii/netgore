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
/// Interface for a class that can be used to serialize values to the database table `character_status_effect`.
/// </summary>
public interface ICharacterStatusEffectTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
ICharacterStatusEffectTable DeepCopy();

/// <summary>
/// Gets the value of the database column `character_id`.
/// </summary>
DemoGame.CharacterID CharacterID
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.ActiveStatusEffectID ID
{
get;
}
/// <summary>
/// Gets the value of the database column `power`.
/// </summary>
System.UInt16 Power
{
get;
}
/// <summary>
/// Gets the value of the database column `status_effect_id`.
/// </summary>
DemoGame.StatusEffectType StatusEffect
{
get;
}
/// <summary>
/// Gets the value of the database column `time_left_secs`.
/// </summary>
System.UInt16 TimeLeftSecs
{
get;
}
}

}
