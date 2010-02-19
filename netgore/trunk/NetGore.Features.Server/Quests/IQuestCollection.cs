using System;
using System.Collections.Generic;
using System.Text;
using NetGore.Collections;

namespace NetGore.Features.Quests
{
    public interface IQuestCollection<TCharacter> : IEnumerable<IQuest<TCharacter>> where TCharacter:IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Gets the <see cref="IQuest{TCharacter}"/> for a given <see cref="QuestID"/>.
        /// </summary>
        /// <param name="questID">The ID of the quest.</param>
        /// <returns>The <see cref="IQuest{TCharacter}"/> for the given <paramref name="questID"/>.</returns>
        IQuest<TCharacter> GetQuest(QuestID questID);
    }
}
