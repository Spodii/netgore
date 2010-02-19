using System;
using System.Collections.Generic;
using System.Text;

namespace NetGore.Features.Quests
{
    public interface IQuestRewardCollection<TCharacter> : IEnumerable<IQuestReward<TCharacter>> where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Checks if the quest rewards can be given to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the quest rewards to.</param>
        /// <returns>True if the <paramref name="character"/> is able to receive the quest rewards; otherwise false.</returns>
        bool CanGive(TCharacter character);

        /// <summary>
        /// Gives the quest rewards to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the quest rewards to.</param>
        void Give(TCharacter character);
    }
}
