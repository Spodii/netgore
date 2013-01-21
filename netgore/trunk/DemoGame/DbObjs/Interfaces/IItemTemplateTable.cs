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
/// Interface for a class that can be used to serialize values to the database table `item_template`.
/// </summary>
public interface IItemTemplateTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IItemTemplateTable DeepCopy();

/// <summary>
/// Gets the value of the database column `action_display_id`.
/// </summary>
System.Nullable<NetGore.Features.ActionDisplays.ActionDisplayID> ActionDisplayID
{
get;
}
/// <summary>
/// Gets the value of the database column `description`.
/// </summary>
System.String Description
{
get;
}
/// <summary>
/// Gets the value of the database column `equipped_body`.
/// </summary>
System.String EquippedBody
{
get;
}
/// <summary>
/// Gets the value of the database column `graphic`.
/// </summary>
NetGore.GrhIndex Graphic
{
get;
}
/// <summary>
/// Gets the value of the database column `height`.
/// </summary>
System.Byte Height
{
get;
}
/// <summary>
/// Gets the value of the database column `hp`.
/// </summary>
DemoGame.SPValueType HP
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.ItemTemplateID ID
{
get;
}
/// <summary>
/// Gets the value of the database column `mp`.
/// </summary>
DemoGame.SPValueType MP
{
get;
}
/// <summary>
/// Gets the value of the database column `name`.
/// </summary>
System.String Name
{
get;
}
/// <summary>
/// Gets the value of the database column `range`.
/// </summary>
System.UInt16 Range
{
get;
}
/// <summary>
/// Gets the value of the database column `skill_id`.
/// </summary>
System.Nullable<DemoGame.SkillType> SkillID
{
get;
}
/// <summary>
/// Gets the value of the database column in the column collection `Stat`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <returns>
/// The value of the database column with the corresponding <paramref name="key"/>.
/// </returns>
System.Int32 GetStat(DemoGame.StatType key);

/// <summary>
/// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
/// key is the collection's key and the value is the value for that corresponding key.
/// </summary>
System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> Stats
{
get;
}
/// <summary>
/// Gets the value of the database column in the column collection `ReqStat`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <returns>
/// The value of the database column with the corresponding <paramref name="key"/>.
/// </returns>
System.Int32 GetReqStat(DemoGame.StatType key);

/// <summary>
/// Gets an IEnumerable of KeyValuePairs containing the values in the `ReqStat` collection. The
/// key is the collection's key and the value is the value for that corresponding key.
/// </summary>
System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> ReqStats
{
get;
}
/// <summary>
/// Gets the value of the database column `type`.
/// </summary>
DemoGame.ItemType Type
{
get;
}
/// <summary>
/// Gets the value of the database column `value`.
/// </summary>
System.Int32 Value
{
get;
}
/// <summary>
/// Gets the value of the database column `weapon_type`.
/// </summary>
DemoGame.WeaponType WeaponType
{
get;
}
/// <summary>
/// Gets the value of the database column `width`.
/// </summary>
System.Byte Width
{
get;
}
}

}
