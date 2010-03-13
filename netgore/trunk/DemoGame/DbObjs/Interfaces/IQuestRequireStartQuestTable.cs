using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `quest_require_start_quest`.
/// </summary>
public interface IQuestRequireStartQuestTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IQuestRequireStartQuestTable DeepCopy();

/// <summary>
/// Gets the value of the database column `quest_id`.
/// </summary>
NetGore.Features.Quests.QuestID QuestID
{
get;
}
/// <summary>
/// Gets the value of the database column `req_quest_id`.
/// </summary>
NetGore.Features.Quests.QuestID ReqQuestID
{
get;
}
}

}
