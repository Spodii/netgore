using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.Quests
{
    public interface IQuestDescriptionCollection : IPersistable, ICollection<IQuestDescription>
    {
        /// <summary>
        /// Gets the <see cref="IQuestDescription"/> for a quest.
        /// </summary>
        /// <param name="questID">The ID of the quest to get the <see cref="IQuestDescription"/> for.</param>
        /// <returns>The <see cref="IQuestDescription"/> for the given <see cref="QuestID"/>.</returns>
        IQuestDescription this[QuestID questID] { get; }
    }
}