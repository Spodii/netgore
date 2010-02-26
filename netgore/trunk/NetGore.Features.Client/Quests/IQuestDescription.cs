using System.Linq;
using NetGore.IO;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for an object that contains the description of a quest.
    /// </summary>
    public interface IQuestDescription : IPersistable
    {
        /// <summary>
        /// Gets the quest's description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the name of the quest.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="QuestID"/> for the quest that this description is for.
        /// </summary>
        QuestID QuestID { get; }
    }
}