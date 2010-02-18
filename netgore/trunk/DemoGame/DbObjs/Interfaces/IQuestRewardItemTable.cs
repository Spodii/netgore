using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `quest_reward_item`.
/// </summary>
public interface IQuestRewardItemTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IQuestRewardItemTable DeepCopy();

/// <summary>
/// Gets the value of the database column `amount`.
/// </summary>
System.Byte Amount
{
get;
}
/// <summary>
/// Gets the value of the database column `item_id`.
/// </summary>
DemoGame.ItemID ItemID
{
get;
}
/// <summary>
/// Gets the value of the database column `quest_id`.
/// </summary>
System.Int32 QuestId
{
get;
}
}

}
