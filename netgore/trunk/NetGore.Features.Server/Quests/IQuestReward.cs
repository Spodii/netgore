using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Features.Quests
{
    public interface IQuestReward<TCharacter> where TCharacter : DynamicEntity
    {
        /// <summary>
        /// Gives the quest reward to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the quest reward to.</param>
        void Give(TCharacter character);

        /// <summary>
        /// Checks if the <paramref name="character"/> is able to receive this quest reward.
        /// </summary>
        /// <param name="character">The character to check if able to receive this quest reward.</param>
        /// <returns>True if the <paramref name="character"/> can receive this quest reward; otherwise false.</returns>
        bool CanGive(TCharacter character);
    }
}
