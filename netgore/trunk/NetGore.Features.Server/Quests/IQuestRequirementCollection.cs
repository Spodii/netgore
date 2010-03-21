using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for a collection of <see cref="IQuestRequirement{TCharacter}"/>s.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    public interface IQuestRequirementCollection<TCharacter> : IEnumerable<IQuestRequirement<TCharacter>>
        where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Checks if the <paramref name="character"/> meets this test requirement.
        /// </summary>
        /// <param name="character">The character to check if they meet the requirements.</param>
        /// <returns>True if the <paramref name="character"/> meets the requirements defined by this
        /// <see cref="IQuestRequirement{TCharacter}"/>; otherwise false.</returns>
        bool HasRequirements(TCharacter character);

        /// <summary>
        /// Takes the quest requirements from the <paramref name="character"/>, if applicable. Not required,
        /// and only applies for when turning in a quest and not starting a quest.
        /// </summary>
        /// <param name="character">The <paramref name="character"/> to take the requirements from.</param>
        void TakeRequirements(TCharacter character);
    }
}