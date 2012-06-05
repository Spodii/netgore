using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for a reward given for completing a quest.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    public interface IQuestReward<in TCharacter> where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Checks if the <paramref name="character"/> is able to receive this quest reward.
        /// </summary>
        /// <param name="character">The character to check if able to receive this quest reward.</param>
        /// <returns>True if the <paramref name="character"/> can receive this quest reward; otherwise false.</returns>
        bool CanGive(TCharacter character);

        /// <summary>
        /// Gives the quest reward to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the quest reward to.</param>
        void Give(TCharacter character);
    }
}