using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Assists in keeping track of the quests for a quest performer.
    /// </summary>
    /// <typeparam name="TCharacter">The type of <see cref="IQuestPerformer{TCharacter}"/>.</typeparam>
    public abstract class QuestPerformerStatusHelper<TCharacter> where TCharacter : IQuestPerformer<TCharacter>
    {
        public delegate void QuestEventHandler(QuestPerformerStatusHelper<TCharacter> sender, IQuest<TCharacter> quest);

        static readonly QuestSettings _questSettings = QuestSettings.Instance;

        readonly List<IQuest<TCharacter>> _activeQuests = new List<IQuest<TCharacter>>();
        readonly List<IQuest<TCharacter>> _completedQuests = new List<IQuest<TCharacter>>();
        readonly TCharacter _owner;

        /// <summary>
        /// Gets the completed quests.
        /// </summary>
        public IEnumerable<IQuest<TCharacter>> CompletedQuests { get { return _completedQuests; } }

        /// <summary>
        /// Gets the active quests.
        /// </summary>
        public IEnumerable<IQuest<TCharacter>> ActiveQuests { get { return _activeQuests; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestPerformerStatusHelper{TCharacter}"/> class.
        /// </summary>
        /// <param name="owner">The quest performer that this object will track the quest status of.</param>
        protected QuestPerformerStatusHelper(TCharacter owner)
        {
            _owner = owner;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _completedQuests.AddRange(LoadCompletedQuests());
            _activeQuests.AddRange(LoadActiveQuests());
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Notifies listeners when a quest has been accepted.
        /// </summary>
        public event QuestEventHandler QuestAccepted;

        /// <summary>
        /// Notifies listeners when a quest has been canceled.
        /// </summary>
        public event QuestEventHandler QuestCanceled;

        /// <summary>
        /// Notifies listeners when a quest has been finished.
        /// </summary>
        public event QuestEventHandler QuestFinished;

        /// <summary>
        /// Gets the quest performer that this object tracks the quest status of.
        /// </summary>
        public TCharacter Owner
        {
            get { return _owner; }
        }

        public bool CanAcceptQuest(IQuest<TCharacter> quest)
        {
            return CanAcceptQuest(quest, false);
        }

        public bool CanAcceptQuest(IQuest<TCharacter> quest, bool notifyOwner)
        {
            // Cannot accept the quest if we have reached the active quest limit, it is already in the active quests,
            // or it is a non-repeatable quest and we have finished it
            if (_activeQuests.Contains(quest))
                return false;

            if (!quest.Repeatable && HasCompletedQuest(quest))
                return false;

            if (_activeQuests.Count >= _questSettings.MaxActiveQuests)
            {
                if (notifyOwner)
                    NotifyCannotAcceptTooManyActive(quest);
                return false;
            }

            return true;
        }

        public bool CancelQuest(IQuest<TCharacter> quest)
        {
            bool ret = _activeQuests.Remove(quest);

            if (ret)
            {
                if (QuestCanceled != null)
                    QuestCanceled(this, quest);
            }

            return ret;
        }

        public bool HasCompletedQuest(IQuest<TCharacter> quest)
        {
            return _completedQuests.Contains(quest);
        }

        /// <summary>
        /// When overridden in the derived class, loads the active quests.
        /// </summary>
        /// <returns>The loaded active quests.</returns>
        protected abstract IEnumerable<IQuest<TCharacter>> LoadActiveQuests();

        /// <summary>
        /// When overridden in the derived class, loads the completed quests.
        /// </summary>
        /// <returns>The loaded completed quests.</returns>
        protected abstract IEnumerable<IQuest<TCharacter>> LoadCompletedQuests();

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to turn in the quest
        /// because the cannot receive the quest rewards (e.g. full inventory).
        /// </summary>
        /// <param name="quest">The quest that caused the error.</param>
        protected abstract void NotifyCannotGiveQuestRewards(IQuest<TCharacter> quest);

        /// <summary>
        /// When overridden in the derived clas,s notifies the owner that they were unable to accept a quest
        /// because they have too many active quests.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected abstract void NotifyCannotAcceptTooManyActive(IQuest<TCharacter> quest);

        public bool TryFinishQuest(IQuest<TCharacter> quest)
        {
            // Make sure they even have this in their active quests
            if (!_activeQuests.Contains(quest))
                return false;

            // Check for the finish requirements
            if (!quest.FinishRequirements.HasRequirements(Owner))
                return false;

            // Ensure there is room to give them the reward(s)
            if (!quest.Rewards.CanGive(Owner))
            {
                NotifyCannotGiveQuestRewards(quest);
                return false;
            }

            Debug.Assert(quest.Repeatable || !HasCompletedQuest(quest), "Uh-oh, this user has already completed this quest!");

            // Remove from the active quests and give the rewards
            bool removed = _activeQuests.Remove(quest);
            Debug.Assert(removed);

            quest.Rewards.Give(Owner);

            if (QuestFinished != null)
                QuestFinished(this, quest);

            return true;
        }

        public bool TryAddQuest(IQuest<TCharacter> quest)
        {
            if (!CanAcceptQuest(quest, true))
                return false;

            _activeQuests.Add(quest);

            if (QuestAccepted != null)
                QuestAccepted(this, quest);

            return true;
        }
    }
}