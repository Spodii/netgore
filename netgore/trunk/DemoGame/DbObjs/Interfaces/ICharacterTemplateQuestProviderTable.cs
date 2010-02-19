using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `character_template_quest_provider`.
/// </summary>
public interface ICharacterTemplateQuestProviderTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
ICharacterTemplateQuestProviderTable DeepCopy();

/// <summary>
/// Gets the value of the database column `character_template_id`.
/// </summary>
DemoGame.CharacterTemplateID CharacterTemplateID
{
get;
}
/// <summary>
/// Gets the value of the database column `quest_id`.
/// </summary>
NetGore.Features.Quests.QuestID QuestID
{
get;
}
}

}
