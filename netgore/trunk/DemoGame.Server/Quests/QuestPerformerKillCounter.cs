using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    public class QuestPerformerKillCounter : QuestPerformerKillCounterBase<User, CharacterTemplateID>
    {
        static readonly DeleteCharacterQuestStatusKillsQuery _deleteCharacterQuestStatusKillsQuery;

        static readonly IEnumerable<KeyValuePair<CharacterTemplateID, ushort>> _emptyKillReqs =
            Enumerable.Empty<KeyValuePair<CharacterTemplateID, ushort>>();

        static readonly QuestManager _questManager = QuestManager.Instance;
        static readonly SelectQuestStatusKillsQuery _selectQuestStatusKillsQuery;
        static readonly UpdateCharacterQuestStatusKillsQuery _updateCharacterQuestStatusKillsQuery;

        /// <summary>
        /// Initializes the <see cref="QuestPerformerKillCounter"/> class.
        /// </summary>
        static QuestPerformerKillCounter()
        {
            var dbController = DbControllerBase.GetInstance();
            _selectQuestStatusKillsQuery = dbController.GetQuery<SelectQuestStatusKillsQuery>();
            _updateCharacterQuestStatusKillsQuery = dbController.GetQuery<UpdateCharacterQuestStatusKillsQuery>();
            _updateCharacterQuestStatusKillsQuery = dbController.GetQuery<UpdateCharacterQuestStatusKillsQuery>();
            _deleteCharacterQuestStatusKillsQuery = dbController.GetQuery<DeleteCharacterQuestStatusKillsQuery>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestPerformerKillCounter"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public QuestPerformerKillCounter(User owner) : base(owner, LoadQuestStatuses(owner.ID))
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the required kills for the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to get the required kills for.</param>
        /// <returns>The required kills for the given <paramref name="quest"/>.</returns>
        protected override IEnumerable<KeyValuePair<CharacterTemplateID, ushort>> GetRequiredKills(IQuest<User> quest)
        {
            Debug.Assert(quest.FinishRequirements.OfType<KillQuestRequirement>().Count() <= 1,
                "There should only be ONE KillQuestRequirement per quest!");

            var reqs = quest.FinishRequirements.OfType<KillQuestRequirement>().FirstOrDefault();
            if (reqs == null)
                return _emptyKillReqs;

            return reqs.RequiredKills;
        }

        /// <summary>
        /// Gets the quest statuses for a character.
        /// </summary>
        /// <param name="characterID">The ID of the character.</param>
        /// <returns>The quest statuses for the character with the given <paramref name="characterID"/>.</returns>
        static IEnumerable<KeyValuePair<IQuest<User>, IEnumerable<KeyValuePair<CharacterTemplateID, ushort>>>> LoadQuestStatuses(CharacterID characterID)
        {
            var values = _selectQuestStatusKillsQuery.Execute(characterID);
            return values.Select(x => new KeyValuePair<IQuest<User>, IEnumerable<KeyValuePair<CharacterTemplateID, ushort>>>(
                _questManager.GetQuest(x.Key), x.Value));
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when the kill count for
        /// a quest target has been incremented.
        /// </summary>
        /// <param name="quest">The quest that the count was incremented for.</param>
        /// <param name="target">The target that the count was incremented for.</param>
        /// <param name="count">The current kill count for the <paramref name="target"/> for the
        /// <paramref name="quest"/>.</param>
        /// <param name="reqCount">The required kill count for completing the quest.</param>
        protected override void OnKillCountIncremented(IQuest<User> quest, CharacterTemplateID target, ushort count, ushort reqCount)
        {
            _updateCharacterQuestStatusKillsQuery.Execute(Owner.ID, quest.QuestID, target, count);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when a quest has been added
        /// to this collection. This does not include quests added when loading the collection.
        /// </summary>
        /// <param name="quest">The quest that was added.</param>
        protected override void OnQuestAdded(IQuest<User> quest)
        {
            base.OnQuestAdded(quest);

            // Ensure the quest's values have been removed from the database (such as if the user already did this quest)
            _deleteCharacterQuestStatusKillsQuery.Execute(Owner.ID, quest.QuestID);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when a quest has been removed
        /// from this collection. This does not include quests added when loading the collection.
        /// </summary>
        /// <param name="quest">The quest that was removed.</param>
        protected override void OnQuestRemoved(IQuest<User> quest)
        {
            base.OnQuestRemoved(quest);

            // Ensure the quest's values have been removed from the database (such as if the user already did this quest)
            _deleteCharacterQuestStatusKillsQuery.Execute(Owner.ID, quest.QuestID);
        }
    }
}