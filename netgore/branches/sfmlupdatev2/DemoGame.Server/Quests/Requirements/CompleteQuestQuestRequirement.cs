using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    /// <summary>
    /// A quest requirement that requires one or more other quests to be finished.
    /// Can be used as both a requirement to start or complete a quest.
    /// </summary>
    public class CompleteQuestQuestRequirement : IQuestRequirement<User>
    {
        readonly IQuest<User> _quest;
        readonly IEnumerable<IQuest<User>> _reqQuests;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteQuestQuestRequirement"/> class.
        /// </summary>
        /// <param name="quest">The quest that this requirement is for.</param>
        /// <param name="reqQuests">The required quests.</param>
        /// <exception cref="ArgumentNullException"><paramref name="quest"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="reqQuests"/> is null or empty.</exception>
        public CompleteQuestQuestRequirement(IQuest<User> quest, IEnumerable<IQuest<User>> reqQuests)
        {
            if (quest == null)
                throw new ArgumentNullException("quest");
            if (reqQuests == null || reqQuests.IsEmpty())
                throw new ArgumentNullException("reqQuests");

            _quest = quest;

            _reqQuests = reqQuests.Where(x => x != null).ToCompact();
        }

        /// <summary>
        /// Gets the quests required to be completed for this quest requirement.
        /// </summary>
        public IEnumerable<IQuest<User>> RequiredQuests
        {
            get { return _reqQuests; }
        }

        #region IQuestRequirement<User> Members

        /// <summary>
        /// Gets the <see cref="IQuest{TCharacter}"/> that this quest requirement is for.
        /// </summary>
        public IQuest<User> Quest
        {
            get { return _quest; }
        }

        /// <summary>
        /// Checks if the <paramref name="character"/> meets this test requirement.
        /// </summary>
        /// <param name="character">The character to check if they meet the requirements.</param>
        /// <returns>True if the <paramref name="character"/> meets the requirements defined by this
        /// <see cref="IQuestRequirement{TCharacter}"/>; otherwise false.</returns>
        public bool HasRequirements(User character)
        {
            return RequiredQuests.All(character.HasCompletedQuest);
        }

        /// <summary>
        /// Takes the quest requirements from the <paramref name="character"/>, if applicable. Not required,
        /// and only applies for when turning in a quest and not starting a quest.
        /// </summary>
        /// <param name="character">The <paramref name="character"/> to take the requirements from.</param>
        public void TakeRequirements(User character)
        {
            // Nothing to "take"
        }

        #endregion
    }
}