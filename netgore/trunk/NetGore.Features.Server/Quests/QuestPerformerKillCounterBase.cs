using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Keeps track of how many kills remain for quests that require killing a
    /// certain number of characters.
    /// </summary>
    /// <typeparam name="TCharacter">The type of quest performer.</typeparam>
    /// <typeparam name="TKillID">The type of identifier for the targets to kill.</typeparam>
    public abstract class QuestPerformerKillCounterBase<TCharacter, TKillID> : IQuestPerformerKillCounter<TCharacter, TKillID>
        where TCharacter : IQuestPerformer<TCharacter> where TKillID : IEquatable<TKillID>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Pool for the <see cref="KillCounterCollection"/>s.
        /// </summary>
        static readonly ObjectPool<KillCounterCollection> _counterCollectionPool =
            new ObjectPool<KillCounterCollection>(x => new KillCounterCollection(), null, x => x.Dispose(), true);

        readonly List<KillCounterCollection> _counters = new List<KillCounterCollection>();
        readonly TCharacter _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestPerformerKillCounterBase{TCharacter, TKillID}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        protected QuestPerformerKillCounterBase(TCharacter owner)
        {
            _owner = owner;
            _owner.QuestAccepted += Owner_QuestAccepted;
            _owner.QuestCanceled += Owner_QuestFinished;
            _owner.QuestFinished += Owner_QuestFinished;
        }

        /// <summary>
        /// Gets the <see cref="KillCounterCollection"/> for the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to get the <see cref="KillCounterCollection"/> for.</param>
        /// <returns>The <see cref="KillCounterCollection"/> for the given <paramref name="quest"/>.</returns>
        KillCounterCollection GetKillCounter(IQuest<TCharacter> quest)
        {
            return _counters.FirstOrDefault(x => x.Quest == quest);
        }

        /// <summary>
        /// Gets the <see cref="KillCounterCollection"/> index for the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to get the <see cref="KillCounterCollection"/> index for.</param>
        /// <returns>The <see cref="KillCounterCollection"/> index for the given <paramref name="quest"/>.</returns>
        int GetKillCounterIndex(IQuest<TCharacter> quest)
        {
            return _counters.FindIndex(x => x.Quest == quest);
        }

        /// <summary>
        /// When overridden in the derived class, gets the required kills for the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to get the required kills for.</param>
        /// <returns>The required kills for the given <paramref name="quest"/>.</returns>
        protected abstract IEnumerable<KeyValuePair<TKillID, ushort>> GetRequiredKills(IQuest<TCharacter> quest);

        /// <summary>
        /// Increments the kill counter for the specified <paramref name="target"/> in all of the active quests.
        /// This should be called once for each kill made by the owner.
        /// </summary>
        /// <param name="target">The target to increment the kill counter for.</param>
        protected void IncrementCounter(TKillID target)
        {
            // Attempt the increment on all of the quest counters
            foreach (var counter in _counters)
            {
                ushort count;
                ushort reqCount;

                // Try to increment the target on the quest
                if (counter.Increment(target, out count, out reqCount))
                {
                    // Increment successful, so invoke events
                    OnKillCountIncremented(counter.Quest, target, count, reqCount);
                    if (KillCountIncremented != null)
                        KillCountIncremented(this, counter.Quest, target, count, reqCount);
                }
            }
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
        protected virtual void OnKillCountIncremented(IQuest<TCharacter> quest, TKillID target, ushort count, ushort reqCount)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when a quest has been added
        /// to this collection.
        /// </summary>
        /// <param name="quest">The quest that was added.</param>
        protected virtual void OnQuestAdded(IQuest<TCharacter> quest)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when a quest has been removed
        /// from this collection.
        /// </summary>
        /// <param name="quest">The quest that was removed.</param>
        protected virtual void OnQuestRemoved(IQuest<TCharacter> quest)
        {
        }

        /// <summary>
        /// Handles the <see cref="IQuestPerformer{T}.QuestAccepted"/> event.
        /// </summary>
        /// <param name="questPerformer">The quest performer the event came from.</param>
        /// <param name="quest">The quest that was accepted.</param>
        void Owner_QuestAccepted(TCharacter questPerformer, IQuest<TCharacter> quest)
        {
            Debug.Assert(Equals(questPerformer, Owner));

            // Ensure the counter does not already exist for this quest
            var existingCounter = GetKillCounter(quest);
            if (existingCounter != null)
            {
                const string errmsg = "Quest list already contains the quest `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, quest);
                Debug.Fail(string.Format(errmsg, quest));
                return;
            }

            // Create the counter
            var counter = _counterCollectionPool.Acquire();
            var reqKills = GetRequiredKills(quest);
            counter.Initialize(quest, reqKills);

            _counters.Add(counter);

            OnQuestAdded(quest);
        }

        /// <summary>
        /// Handles the <see cref="IQuestPerformer{T}.QuestFinished"/> and <see cref="IQuestPerformer{T}.QuestCanceled"/>
        /// events.
        /// </summary>
        /// <param name="questPerformer">The quest performer the event came from.</param>
        /// <param name="quest">The quest that was finished or canceled.</param>
        void Owner_QuestFinished(TCharacter questPerformer, IQuest<TCharacter> quest)
        {
            Debug.Assert(Equals(questPerformer, Owner));

            var counterIndex = GetKillCounterIndex(quest);

            // Make sure the counter exists
            if (counterIndex < 0)
            {
                const string errmsg = "Quest list does not contain the quest `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, quest);
                Debug.Fail(string.Format(errmsg, quest));
                return;
            }

            // Remove the counter
            _counters.RemoveAt(counterIndex);

            OnQuestRemoved(quest);
        }

        #region IQuestPerformerKillCounter<TCharacter,TKillID> Members

        /// <summary>
        /// Notifies listeners when the kill counter for a quest has been incremented. This event will only be invoked
        /// if, when the target was killed, the kill count was less than the required kill count. As a result, the
        /// kill count will never be zero.
        /// </summary>
        public event QuestPerformerKillCounterKillIncrementEventHandler<TCharacter, TKillID> KillCountIncremented;

        /// <summary>
        /// Gets the quest performer that this collection belongs to.
        /// </summary>
        public TCharacter Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var counter in _counters)
            {
                _counterCollectionPool.Free(counter);
            }

            _counters.Clear();
        }

        /// <summary>
        /// Gets the kill counts for the given <paramref name="quest"/> for each of the required kill counters. The
        /// returned counts will not exceed the number of required kills for the <paramref name="quest"/> for the
        /// respective target.
        /// </summary>
        /// <param name="quest">The quest to get the kill counts for.</param>
        /// <returns>
        /// The kill counts for the <paramref name="quest"/> for each of the required characters to kill
        /// for the quest.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="quest"/> has not been added to the collection.</exception>
        public IEnumerable<KeyValuePair<TKillID, ushort>> GetKillCounts(IQuest<TCharacter> quest)
        {
            var killCounter = GetKillCounter(quest);
            if (killCounter == null)
                throw new ArgumentException(string.Format("The quest `{0}` is not in this collection.", quest), "quest");

            return killCounter.GetKillCounts();
        }

        /// <summary>
        /// Checks if the required kill count has been reached for all of the required targets for the given
        /// <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to check if the required kills have been made.</param>
        /// <returns>
        /// True if the required kill count has been reached for all of the required targets for the given
        /// <paramref name="quest"/>; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="quest"/> has not been added to the collection.</exception>
        public bool HasAllKills(IQuest<TCharacter> quest)
        {
            var killCounter = GetKillCounter(quest);
            if (killCounter == null)
                throw new ArgumentException(string.Format("The quest `{0}` is not in this collection.", quest), "quest");

            return killCounter.HasAllKills();
        }

        #endregion

        /// <summary>
        /// A collection of <see cref="KillCounter"/>s for a single quest.
        /// </summary>
        class KillCounterCollection : IPoolable, IDisposable
        {
            /// <summary>
            /// Pool used for creating the <see cref="KillCounter"/>s.
            /// </summary>
            static readonly ObjectPool<KillCounter> _killCounterPool = new ObjectPool<KillCounter>(x => new KillCounter(), true);

            /// <summary>
            /// The <see cref="KillCounter"/>s for this collection.
            /// </summary>
            readonly List<KillCounter> _killCounters = new List<KillCounter>();

            IQuest<TCharacter> _quest;

            /// <summary>
            /// Gets the quest that this <see cref="KillCounterCollection"/> is for.
            /// </summary>
            public IQuest<TCharacter> Quest
            {
                get { return _quest; }
            }

            /// <summary>
            /// Gets the kill counts for this collection for each of the required kill counters. The
            /// returned counts will not exceed the number of required kills for this collection for the
            /// respective target.
            /// </summary>
            /// <returns>
            /// The kill counts for this colleection for each of the required characters to kill
            /// for the quest.
            /// </returns>
            public IEnumerable<KeyValuePair<TKillID, ushort>> GetKillCounts()
            {
                return _killCounters.Select(x => new KeyValuePair<TKillID, ushort>(x.KillID, x.KillCount)).ToImmutable();
            }

            /// <summary>
            /// Checks if the required kill count has been reached for all of the required targets for
            /// this collection.
            /// </summary>
            /// <returns>True if the required kill count has been reached for all of the required targets for this
            /// collection; otherwise false.</returns>
            public bool HasAllKills()
            {
                return _killCounters.All(x => x.KillCount >= x.RequiredKills);
            }

            /// <summary>
            /// Increments the kill count of the <paramref name="target"/>.
            /// </summary>
            /// <param name="target">The ID of the target to increment the kill count of.</param>
            /// <param name="count">When this method returns true, contains the current kill count for
            /// the <paramref name="target"/>.</param>
            /// <param name="reqCount">When this method returns true, contains the required kill count for
            /// the <paramref name="target"/>.</param>
            /// <returns>
            /// True if the count was incremented; false if the <paramref name="target"/> is not required
            /// by this quest or the required kill count has been reached.
            /// </returns>
            public bool Increment(TKillID target, out ushort count, out ushort reqCount)
            {
                foreach (var counter in _killCounters)
                {
                    if (counter.KillID.Equals(target))
                    {
                        if (counter.KillCount < counter.RequiredKills)
                        {
                            counter.KillCount++;
                            count = counter.KillCount;
                            reqCount = counter.RequiredKills;
                            return true;
                        }

                        break;
                    }
                }

                count = 0;
                reqCount = 0;
                return false;
            }

            /// <summary>
            /// Initializes the object.
            /// </summary>
            /// <param name="quest">The quest.</param>
            /// <param name="requiredKills">The required targets to kill and the amount of them to kill.</param>
            public void Initialize(IQuest<TCharacter> quest, IEnumerable<KeyValuePair<TKillID, ushort>> requiredKills)
            {
                Debug.Assert(_killCounters.IsEmpty());
                Debug.Assert(_quest == null);
                Debug.Assert(quest != null);

                _quest = quest;

                foreach (var reqKill in requiredKills)
                {
                    var killCounter = _killCounterPool.Acquire();
                    killCounter.Initialize(reqKill.Key, reqKill.Value);
                    _killCounters.Add(killCounter);
                }
            }

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Debug.Assert(_quest != null);

                foreach (var kc in _killCounters)
                {
                    _killCounterPool.Free(kc);
                }

                _killCounters.Clear();

                _quest = null;
            }

            #endregion

            #region IPoolable Members

            /// <summary>
            /// Gets or sets the index of the object in the pool. This value should never be used by anything
            /// other than the pool that owns this object.
            /// </summary>
            int IPoolable.PoolIndex { get; set; }

            #endregion

            /// <summary>
            /// Contains the kill count for a single target.
            /// </summary>
            class KillCounter : IPoolable
            {
                ushort _killCount;
                TKillID _killID;
                ushort _requiredKills;

                /// <summary>
                /// Gets or sets the current number of kills.
                /// </summary>
                public ushort KillCount
                {
                    get { return _killCount; }
                    set { _killCount = value; }
                }

                /// <summary>
                /// Gets the ID of the target.
                /// </summary>
                public TKillID KillID
                {
                    get { return _killID; }
                }

                /// <summary>
                /// Gets the required number of kills to complete the quest.
                /// </summary>
                public ushort RequiredKills
                {
                    get { return _requiredKills; }
                }

                /// <summary>
                /// Initializes this object.
                /// </summary>
                /// <param name="killID">The ID of the character to kill.</param>
                /// <param name="requiredKills">The required number of kills to complete the quest.</param>
                public void Initialize(TKillID killID, ushort requiredKills)
                {
                    _killID = killID;
                    _requiredKills = requiredKills;
                    _killCount = 0;
                }

                #region IPoolable Members

                /// <summary>
                /// Gets or sets the index of the object in the pool. This value should never be used by anything
                /// other than the pool that owns this object.
                /// </summary>
                int IPoolable.PoolIndex { get; set; }

                #endregion
            }
        }
    }
}