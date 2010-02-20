using System.Linq;
using System.Text;
using NetGore.IO;
using NetGore.IO.PropertySync;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for an object that contains the description of a quest.
    /// </summary>
    public interface IQuestDescription : IPersistable
    {
        /// <summary>
        /// Gets the <see cref="QuestID"/> for the quest that this description is for.
        /// </summary>
        QuestID QuestID { get; }

        /// <summary>
        /// Gets the name of the quest.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the quest's description.
        /// </summary>
        string Description { get; }
    }
}
