using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Quests
{
    public class QuestRewardCollection<TCharacter> : IQuestRewardCollection<TCharacter> where TCharacter : DynamicEntity
    {
        static readonly IEnumerable<IQuestReward<TCharacter>> _emptyQuestRewards = new IQuestReward<TCharacter>[0];

        readonly IEnumerable<IQuestReward<TCharacter>> _questRewards;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestRewardCollection&lt;TCharacter&gt;"/> class.
        /// </summary>
        /// <param name="questRewards">The quest rewards.</param>
        public QuestRewardCollection(IEnumerable<IQuestReward<TCharacter>> questRewards)
        {
            if (questRewards == null || questRewards.Count() == 0)
                _questRewards = _emptyQuestRewards;
            else
                _questRewards = questRewards.ToCompact();
        }

        /// <summary>
        /// Checks if the quest rewards can be given to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the quest rewards to.</param>
        /// <returns>True if the <paramref name="character"/> is able to receive the quest rewards; otherwise false.</returns>
        public bool CanGive(TCharacter character)
        {
            return this.All(x => x.CanGive(character));
        }

        /// <summary>
        /// Gives the quest rewards to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the quest rewards to.</param>
        public void Give(TCharacter character)
        {
            foreach (var reward in this)
                reward.Give(character);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IQuestReward<TCharacter>> GetEnumerator()
        {
            return _questRewards.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}