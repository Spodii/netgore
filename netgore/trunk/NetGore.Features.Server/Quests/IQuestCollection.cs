using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for a collection of <see cref="IQuest{TCharacter}"/>s.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    public interface IQuestCollection<TCharacter> : IEnumerable<IQuest<TCharacter>> where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Gets the <see cref="IQuest{TCharacter}"/> for a given <see cref="QuestID"/>.
        /// </summary>
        /// <param name="questID">The ID of the quest.</param>
        /// <returns>The <see cref="IQuest{TCharacter}"/> for the given <paramref name="questID"/>.</returns>
        IQuest<TCharacter> GetQuest(QuestID questID);
    }
}