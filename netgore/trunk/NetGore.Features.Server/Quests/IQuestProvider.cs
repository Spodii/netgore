using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for an object that is allowed to give out quests.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    public interface IQuestProvider<TCharacter> where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Gets the quests that this quest provider provides.
        /// </summary>
        IEnumerable<IQuest<TCharacter>> Quests { get; }
    }
}