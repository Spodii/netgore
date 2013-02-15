using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Assists in keeping track of the quests for a quest performer.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    public abstract class QuestPerformerStatusHelper<TCharacter> where TCharacter : IQuestPerformer<TCharacter>
    {
        static readonly QuestSettings _questSettings = QuestSettings.Instance;

        readonly List<IQuest<TCharacter>> _activeQuests = new List<IQuest<TCharacter>>();
        readonly List<IQuest<TCharacter>> _completedQuests = new List<IQuest<TCharacter>>();
        readonly List<IQuest<TCharacter>> _repeatableQuests = new List<IQuest<TCharacter>>();
        readonly TCharacter _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestPerformerStatusHelper{TCharacter}"/> class.
        /// </summary>
        /// <param name="owner">The quest performer that this object will track the quest status of.</param>
        protected QuestPerformerStatusHelper(TCharacter owner)
        {
            _owner = owner;

            _completedQuests.AddRange(LoadCompletedQuests());
            _activeQuests.AddRange(LoadActiveQuests());
            _repeatableQuests.AddRange(LoadRepeatableQuests());
        }

        /// <summary>
        /// Notifies listeners when a quest has been accepted.
        /// </summary>
        public event TypedEventHandler<QuestPerformerStatusHelper<TCharacter>, EventArgs<IQuest<TCharacter>>> QuestAccepted;

        /// <summary>
        /// Notifies listeners when a quest has been canceled.
        /// </summary>
        public event TypedEventHandler<QuestPerformerStatusHelper<TCharacter>, EventArgs<IQuest<TCharacter>>> QuestCanceled;

        /// <summary>
        /// Notifies listeners when a quest has been finished.
        /// </summary>
        public event TypedEventHandler<QuestPerformerStatusHelper<TCharacter>, EventArgs<IQuest<TCharacter>>> QuestFinished;

        /// <summary>
        /// Gets the active quests.
        /// </summary>
        public IEnumerable<IQuest<TCharacter>> ActiveQuests
        {
            get { return _activeQuests; }
        }

        /// <summary>
        /// Gets the completed quests.
        /// </summary>
        public IEnumerable<IQuest<TCharacter>> CompletedQuests
        {
            get { return _completedQuests; }
        }

        /// <summary>
        /// Gets the repeatable quests.
        /// </summary>
        public IEnumerable<IQuest<TCharacter>> RepeatableQuests
        {
            get { return _repeatableQuests; }
        }

        /// <summary>
        /// Gets the quest performer that this object tracks the quest status of.
        /// </summary>
        public TCharacter Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Checks if the <see cref="QuestPerformerStatusHelper{TCharacter}.Owner"/> can accept an
        /// <see cref="IQuest{TCharacter}"/>.
        /// </summary>
        /// <param name="quest">The quest to see if the owner can accept.</param>
        /// <param name="notifyOwner">If true, the owner will be notified that they failed to accept the quest. If false,
        /// they will not receive any notification. Default is false.</param>
        /// <returns>
        /// True if the owner can accept the <paramref name="quest"/>; otherwise false.
        /// </returns>
        public bool CanAcceptQuest(IQuest<TCharacter> quest, bool notifyOwner = false)
        {
            // Cannot accept the quest if we have reached the active quest limit, it is already in the active quests,
            // or it is a non-repeatable quest and we have finished it
            if (_activeQuests.Contains(quest))
            {
                if (notifyOwner)
                    NotifyCannotAcceptAlreadyStarted(quest);
                return false;
            }

            if (!quest.Repeatable && HasCompletedQuest(quest))
            {
                if (notifyOwner)
                    NotifyCannotAcceptAlreadyCompleted(quest);
                return false;
            }

            if (_activeQuests.Count >= _questSettings.MaxActiveQuests)
            {
                if (notifyOwner)
                    NotifyCannotAcceptTooManyActive(quest);
                return false;
            }

            if (!quest.StartRequirements.HasRequirements(Owner))
            {
                if (notifyOwner)
                    NotifyCannotAcceptDoNotHaveStartRequirements(quest);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Cancels an active quest.
        /// </summary>
        /// <param name="quest">The quest to cancel.</param>
        /// <returns>True if the quest was successfully canceled; otherwise false.</returns>
        public bool CancelQuest(IQuest<TCharacter> quest)
        {
            var ret = _activeQuests.Remove(quest);

            // Ensure the quest was even in the collection
            if (!ret)
                return false;

            // Raise events
            OnQuestCanceled(quest);

            if (QuestCanceled != null)
                QuestCanceled.Raise(this, EventArgsHelper.Create(quest));

            return true;
        }

        /// <summary>
        /// Checks if the owner has completed a <see cref="IQuest{TCharacter}"/>.
        /// </summary>
        /// <param name="quest">The quest to check if the owner has completed.</param>
        /// <returns>True if the owner has completed the <paramref name="quest"/>; otherwise false.</returns>
        public bool HasCompletedQuest(IQuest<TCharacter> quest)
        {
            return _completedQuests.Contains(quest);
        }

        /// <summary>
        /// Checks if the owner has a repeatable <see cref="IQuest{TCharacter}"/>.
        /// </summary>
        /// <param name="quest">The quest to check if the owner has this repeatable.</param>
        /// <returns>True if the owner can repeat the <paramref name="quest"/>; otherwise false.</returns>
        public bool IsRepeatableQuest(IQuest<TCharacter> quest)
        {
            return _repeatableQuests.Contains(quest);
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
        /// When overridden in the derived class, loads the repeatable quests.
        /// </summary>
        /// <returns>The loaded repeatable quests.</returns>
        protected abstract IEnumerable<IQuest<TCharacter>> LoadRepeatableQuests();

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to accept a quest
        /// because they have already completed it.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected abstract void NotifyCannotAcceptAlreadyCompleted(IQuest<TCharacter> quest);

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to accept a quest
        /// because they have already started it.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected abstract void NotifyCannotAcceptAlreadyStarted(IQuest<TCharacter> quest);

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to accept a quest
        /// because they do not have the needed requirements.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected abstract void NotifyCannotAcceptDoNotHaveStartRequirements(IQuest<TCharacter> quest);

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to accept a quest
        /// because they have too many active quests.
        /// </summary>
        /// <param name="quest">The quest that could not be accepted.</param>
        protected abstract void NotifyCannotAcceptTooManyActive(IQuest<TCharacter> quest);

        /// <summary>
        /// When overridden in the derived class, notifies the owner that they were unable to turn in the quest
        /// because the cannot receive the quest rewards (e.g. full inventory).
        /// </summary>
        /// <param name="quest">The quest that caused the error.</param>
        protected abstract void NotifyCannotGiveQuestRewards(IQuest<TCharacter> quest);

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="QuestPerformerStatusHelper{TCharacter}.QuestAccepted"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was accepted.</param>
        protected virtual void OnQuestAccepted(IQuest<TCharacter> quest)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="QuestPerformerStatusHelper{TCharacter}.QuestCanceled"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was canceled.</param>
        protected virtual void OnQuestCanceled(IQuest<TCharacter> quest)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="QuestPerformerStatusHelper{TCharacter}.QuestFinished"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was finished.</param>
        protected virtual void OnQuestFinished(IQuest<TCharacter> quest)
        {
        }

        /// <summary>
        /// Tries to add a quest to the active quest list.
        /// </summary>
        /// <param name="quest">The quest to add to the owner's active quest list.</param>
        /// <returns>True if the <paramref name="quest"/> was successfully added to the active quest list;
        /// false if the <paramref name="quest"/> was invalid, if the owner has too many active quests, or if
        /// the owner does not have the requirements needed to start the quest.</returns>
        public bool TryAddQuest(IQuest<TCharacter> quest)
        {
            if (!CanAcceptQuest(quest, true))
                return false;

            _activeQuests.Add(quest);

            // Raise events
            OnQuestAccepted(quest);

            if (QuestAccepted != null)
                QuestAccepted.Raise(this, EventArgsHelper.Create(quest));

            return true;
        }

        /// <summary>
        /// Tries to finish a quest for the owner.
        /// </summary>
        /// <param name="quest">The quest to turn in.</param>
        /// <returns>True if the quest was successfully turned in; false if the owner did not have the <paramref name="quest"/>
        /// in their active quest list, if the quest was invalid, or if they did not have the requirements needed
        /// to finish the quest.</returns>
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
            var removed = _activeQuests.Remove(quest);
            Debug.Assert(removed);

            // Add the quest to the completed quests list
            _completedQuests.Add(quest);

            quest.Rewards.Give(Owner);

            // Raise events
            OnQuestFinished(quest);

            if (QuestFinished != null)
                QuestFinished.Raise(this, EventArgsHelper.Create(quest));

            return true;
        }
    }
}