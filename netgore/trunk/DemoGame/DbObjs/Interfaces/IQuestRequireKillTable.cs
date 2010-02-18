using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `quest_require_kill`.
/// </summary>
public interface IQuestRequireKillTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IQuestRequireKillTable DeepCopy();

/// <summary>
/// Gets the value of the database column `amount`.
/// </summary>
System.UInt16 Amount
{
get;
}
/// <summary>
/// Gets the value of the database column `character_template_id`.
/// </summary>
System.Nullable<DemoGame.CharacterTemplateID> CharacterTemplateID
{
get;
}
/// <summary>
/// Gets the value of the database column `quest_id`.
/// </summary>
System.Nullable<NetGore.Features.Quests.QuestID> QuestID
{
get;
}
}

}
