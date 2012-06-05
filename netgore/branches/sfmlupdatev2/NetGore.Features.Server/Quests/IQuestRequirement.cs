using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for a single requirement for a quest. Requirements can be used in multiple places for quests, such
    /// as the requirements to start a quest, to finish it, etc.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    public interface IQuestRequirement<TCharacter> where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Gets the <see cref="IQuest{TCharacter}"/> that this quest requirement is for.
        /// </summary>
        IQuest<TCharacter> Quest { get; }

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