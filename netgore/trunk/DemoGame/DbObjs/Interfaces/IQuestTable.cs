using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `quest`.
/// </summary>
public interface IQuestTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IQuestTable DeepCopy();

/// <summary>
/// Gets the value of the database column `description`.
/// </summary>
System.String Description
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
NetGore.Features.Quests.QuestID ID
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
/// Gets the value of the database column `repeatable`.
/// </summary>
System.Boolean Repeatable
{
get;
}
/// <summary>
/// Gets the value of the database column `reward_cash`.
/// </summary>
System.Int32 RewardCash
{
get;
}
/// <summary>
/// Gets the value of the database column `reward_exp`.
/// </summary>
System.Int32 RewardExp
{
get;
}
}

}
