using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    public class QuestPerformerStatusHelper : QuestPerformerStatusHelper<User>
    {
        static readonly QuestManager _questManager = QuestManager.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestPerformerStatusHelper"/> class.
        /// </summary>
        /// <param name="owner">The quest performer that this object will track the quest status of.</param>
        public QuestPerformerStatusHelper(User owner) : base(owner)
        {
        }

        /// <summary>
        /// When overridden in the derived class, loads the active quests.
        /// </summary>
        /// <returns>The loaded active quests.</returns>
        protected override IEnumerable<IQuest<User>> LoadActiveQuests()
        {
            var activeQuests = Owner.DbController.GetQuery<SelectActiveQuestsQuery>().Execute(Owner.ID);
            return activeQuests.Select(x => _questManager.GetQuest(x));
        }

        /// <summary>
        /// When overridden in the derived class, loads the completed quests.
        /// </summary>
        /// <returns>The loaded completed quests.</returns>
        protected override IEnumerable<IQuest<User>> LoadCompletedQuests()
        {
            var activeQuests = Owner.DbController.GetQuery<SelectCompletedQuestsQuery>().Execute(Owner.ID);
            return activeQuests.Select(x => _questManager.GetQuest(x));
        }

        /// <summary>
        /// When overridden in the derived clas,s notifies the owner that they were unable to accept a quest
        /// because they have too many active quests.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected override void NotifyCannotAcceptTooManyActive(IQuest<User> quest)
        {
            // TODO: !! ...
        }

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to turn in the quest
        /// because the cannot receive the quest rewards (e.g. full inventory).
        /// </summary>
        /// <param name="quest">The quest that caused the error.</param>
        protected override void NotifyCannotGiveQuestRewards(IQuest<User> quest)
        {
            // TODO: !! ...
        }
    }
}