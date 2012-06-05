using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    /// <summary>
    /// A quest requirement that requires killing a certain number of certain characters, identified by their
    /// template. Can only be used as a quest finish requirement.
    /// </summary>
    public class KillQuestRequirement : IQuestRequirement<User>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly DeleteCharacterQuestStatusKillsQuery _deleteCharacterQuestStatusKillsQuery;

        readonly IQuest<User> _quest;
        readonly IEnumerable<KeyValuePair<CharacterTemplateID, ushort>> _reqKills;

        /// <summary>
        /// Initializes the <see cref="KillQuestRequirement"/> class.
        /// </summary>
        static KillQuestRequirement()
        {
            var dbController = DbControllerBase.GetInstance();
            _deleteCharacterQuestStatusKillsQuery = dbController.GetQuery<DeleteCharacterQuestStatusKillsQuery>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KillQuestRequirement"/> class.
        /// </summary>
        /// <param name="quest">The quest that this requirement is for.</param>
        /// <param name="requiredKills">The character templates and kill amounts required by the quest.</param>
        /// <exception cref="ArgumentNullException"><paramref name="quest"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="requiredKills"/> is null or empty.</exception>
        public KillQuestRequirement(IQuest<User> quest, IEnumerable<KeyValuePair<CharacterTemplateID, ushort>> requiredKills)
        {
            if (quest == null)
                throw new ArgumentNullException("quest");
            if (requiredKills == null || requiredKills.IsEmpty())
                throw new ArgumentNullException("requiredKills");

            _quest = quest;

            // Store the valid kills
            _reqKills = requiredKills.Where(AssertValidValue).ToCompact();
        }

        /// <summary>
        /// Gets the <see cref="KeyValuePair{T,U}"/>s for the <see cref="CharacterTemplateID"/>s required to be killed,
        /// and the minimum amount of kills required for the respective <see cref="CharacterTemplateID"/>.
        /// </summary>
        public IEnumerable<KeyValuePair<CharacterTemplateID, ushort>> RequiredKills
        {
            get { return _reqKills; }
        }

        /// <summary>
        /// Checks if a <see cref="KeyValuePair{T,U}"/> for a <see cref="CharacterTemplateID"/> and amount is
        /// valid for a quest kill requirement.
        /// </summary>
        /// <param name="v">The <see cref="CharacterTemplateID"/> and amount to kill..</param>
        /// <returns>True if valid; otherwise false.</returns>
        bool AssertValidValue(KeyValuePair<CharacterTemplateID, ushort> v)
        {
            if (!v.Key.TemplateExists())
            {
                const string errmsg =
                    "Could not use kill requirement for character template `{0}` in quest `{1}` because the template ID is invalid.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, v.Key, Quest);
                Debug.Fail(string.Format(errmsg, v.Key, Quest));
                return false;
            }

            if (v.Value <= 0)
            {
                const string errmsg =
                    "Could not use kill requirement for character template `{0}` in quest `{1}` because the required kill count is invalid ({2}).";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, v.Key, Quest, v.Value);
                Debug.Fail(string.Format(errmsg, v.Key, Quest, v.Value));
                return false;
            }

            return true;
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
        /// <returns>
        /// True if the <paramref name="character"/> meets the requirements defined by this
        /// <see cref="IQuestRequirement{TCharacter}"/>; otherwise false.
        /// </returns>
        public bool HasRequirements(User character)
        {
            return character.QuestInfo.QuestKillCounter.HasAllKills(Quest);
        }

        /// <summary>
        /// Takes the quest requirements from the <paramref name="character"/>, if applicable. Not required,
        /// and only applies for when turning in a quest and not starting a quest.
        /// </summary>
        /// <param name="character">The <paramref name="character"/> to take the requirements from.</param>
        public void TakeRequirements(User character)
        {
            _deleteCharacterQuestStatusKillsQuery.Execute(character.ID, Quest.QuestID);
        }

        #endregion
    }
}