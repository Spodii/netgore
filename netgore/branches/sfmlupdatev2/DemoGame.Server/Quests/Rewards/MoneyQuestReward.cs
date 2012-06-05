using System.Linq;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    /// <summary>
    /// A quest reward of money.
    /// </summary>
    public class MoneyQuestReward : IQuestReward<User>
    {
        readonly int _cash;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyQuestReward"/> class.
        /// </summary>
        /// <param name="cash">The cash reward.</param>
        public MoneyQuestReward(int cash)
        {
            _cash = cash;
        }

        /// <summary>
        /// Gets the amount of cash given as a reward.
        /// </summary>
        public int Cash
        {
            get { return _cash; }
        }

        #region IQuestReward<User> Members

        /// <summary>
        /// Checks if the <paramref name="character"/> is able to receive this quest reward.
        /// </summary>
        /// <param name="character">The character to check if able to receive this quest reward.</param>
        /// <returns>True if the <paramref name="character"/> can receive this quest reward; otherwise false.</returns>
        public bool CanGive(User character)
        {
            return true;
        }

        /// <summary>
        /// Gives the quest reward to the <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character to give the quest reward to.</param>
        public void Give(User character)
        {
            character.Cash += Cash;
        }

        #endregion
    }
}