using System.Linq;

namespace NetGore.Features.Quests
{
    public interface IQuest<TCharacter> where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Gets the requirements for finishing this quest.
        /// </summary>
        IQuestRequirementCollection<TCharacter> FinishRequirements { get; }

        /// <summary>
        /// Gets the unique ID of the quest.
        /// </summary>
        QuestID QuestID { get; }

        /// <summary>
        /// Gets if this quest can be repeated.
        /// </summary>
        bool Repeatable { get; }

        /// <summary>
        /// Gets the rewards for completing this quest.
        /// </summary>
        IQuestRewardCollection<TCharacter> Rewards { get; }

        /// <summary>
        /// Gets the requirements for starting this quest.
        /// </summary>
        IQuestRequirementCollection<TCharacter> StartRequirements { get; }
    }
}