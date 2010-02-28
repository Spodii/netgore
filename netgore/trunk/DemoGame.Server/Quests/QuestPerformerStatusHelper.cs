using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Features.Quests;

namespace DemoGame.Server.Quests
{
    public class QuestPerformerStatusHelper : QuestPerformerStatusHelper<User>
    {
        static readonly QuestManager _questManager = QuestManager.Instance;

        readonly QuestPerformerKillCounter _questKillCounter;

        /// <summary>
        /// Gets the <see cref="QuestPerformerKillCounter"/>.
        /// </summary>
        public QuestPerformerKillCounter QuestKillCounter { get { return _questKillCounter; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestPerformerStatusHelper"/> class.
        /// </summary>
        /// <param name="owner">The quest performer that this object will track the quest status of.</param>
        public QuestPerformerStatusHelper(User owner) : base(owner)
        {
            _questKillCounter = new QuestPerformerKillCounter(owner);

            // Send the initial quest status
            var completed = CompletedQuests.Select(x => x.QuestID);
            var active = ActiveQuests.Select(x => x.QuestID);
            using (var pw = ServerPacket.QuestInfo(x => UserQuestInformation.WriteQuestInfo(x, completed, active)))
            {
                Owner.Send(pw);
            }
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
        /// When overridden in the derived class, notifies the owner that they were unable to accept a quest
        /// because they have already completed it.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected override void NotifyCannotAcceptAlreadyCompleted(IQuest<User> quest)
        {
            Owner.Send(GameMessage.QuestAcceptFailedAlreadyCompleted);
        }

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to accept a quest
        /// because they have already started it.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected override void NotifyCannotAcceptAlreadyStarted(IQuest<User> quest)
        {
            Owner.Send(GameMessage.QuestAcceptFailedAlreadyStarted);
        }

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to accept a quest
        /// because they do not have the needed requirements.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected override void NotifyCannotAcceptDoNotHaveStartRequirements(IQuest<User> quest)
        {
            Owner.Send(GameMessage.QuestAcceptFailedDoNotHaveStartRequirements);
        }

        /// <summary>
        /// When overridden in the derived clas,s notifies the owner that they were unable to accept a quest
        /// because they have too many active quests.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected override void NotifyCannotAcceptTooManyActive(IQuest<User> quest)
        {
            Owner.Send(GameMessage.QuestAcceptFailedTooManyActive);
        }

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to turn in the quest
        /// because the cannot receive the quest rewards (e.g. full inventory).
        /// </summary>
        /// <param name="quest">The quest that caused the error.</param>
        protected override void NotifyCannotGiveQuestRewards(IQuest<User> quest)
        {
            Owner.Send(GameMessage.QuestFinishFailedCannotGiveRewards);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="QuestPerformerStatusHelper{TCharacter}.QuestAccepted"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was accepted.</param>
        protected override void OnQuestAccepted(IQuest<User> quest)
        {
            base.OnQuestAccepted(quest);

            using (var pw = ServerPacket.QuestInfo(x => UserQuestInformation.WriteAddActiveQuest(x, quest.QuestID)))
            {
                Owner.Send(pw);
            }

            Owner.Send(GameMessage.QuestAccepted);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="QuestPerformerStatusHelper{TCharacter}.QuestCanceled"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was canceled.</param>
        protected override void OnQuestCanceled(IQuest<User> quest)
        {
            base.OnQuestCanceled(quest);

            using (var pw = ServerPacket.QuestInfo(x => UserQuestInformation.WriteRemoveActiveQuest(x, quest.QuestID)))
            {
                Owner.Send(pw);
            }

            Owner.Send(GameMessage.QuestCanceled);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="QuestPerformerStatusHelper{TCharacter}.QuestFinished"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was finished.</param>
        protected override void OnQuestFinished(IQuest<User> quest)
        {
            base.OnQuestFinished(quest);

            using (var pw = ServerPacket.QuestInfo(x => UserQuestInformation.WriteRemoveActiveQuest(x, quest.QuestID)))
            {
                Owner.Send(pw);
            }

            using (var pw = ServerPacket.QuestInfo(x => UserQuestInformation.WriteAddCompletedQuest(x, quest.QuestID)))
            {
                Owner.Send(pw);
            }

            Owner.Send(GameMessage.QuestFinished);
        }
    }
}