using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// A collection of <see cref="IQuest{TCharacter}"/>s.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    public abstract class QuestCollection<TCharacter> : IQuestCollection<TCharacter>
        where TCharacter : IQuestPerformer<TCharacter>
    {
        IQuest<TCharacter>[] _quests;

        /// <summary>
        /// Gets the quest for the given <see cref="QuestID"/>.
        /// </summary>
        /// <param name="id">The <see cref="QuestID"/> to get.</param>
        /// <returns>The quest for the given <paramref name="id"/>, or null if the <paramref name="id"/> is invalid.</returns>
        public IQuest<TCharacter> this[QuestID id]
        {
            get { return GetQuest(id); }
        }

        /// <summary>
        /// When overridden in the derived class, loads the quest with the specified <see cref="QuestID"/>.
        /// </summary>
        /// <param name="questID">The ID of the quest to load.</param>
        /// <returns>The <see cref="IQuest{TCharacter}"/> for the <paramref name="questID"/>.</returns>
        protected abstract IQuest<TCharacter> LoadQuest(QuestID questID);

        /// <summary>
        /// Loads all of the quests. This only needs to be called once.
        /// </summary>
        /// <param name="questIDs">The IDs of the quests to load.</param>
        protected void LoadQuests(IEnumerable<QuestID> questIDs)
        {
            if (questIDs == null)
            {
                _quests = new IQuest<TCharacter>[0];
                return;
            }

            _quests = new IQuest<TCharacter>[questIDs.Max().GetRawValue() + 1];

            foreach (var questID in questIDs.Distinct())
            {
                var quest = LoadQuest(questID);
                Debug.Assert(quest.QuestID == questID);

                _quests[questID.GetRawValue()] = quest;
            }
        }

        /// <summary>
        /// Forces a quest in this collection to be reloaded from the database.
        /// Note that reloading an object will create a new object, not update the existing object. As a result, anything referencing
        /// the old object will continue to reference the old values instead of the newly loaded values.
        /// </summary>
        /// <param name="questID">The ID of the quest to force to reload.</param>
        public virtual void Reload(QuestID questID)
        {
            // Check for a valid ID range
            if (questID < 0)
                return;

            // Resize the array if needed
            if (questID.GetRawValue() >= _quests.Length)
                Array.Resize(ref _quests, questID.GetRawValue() + 1);

            // Try to load the quest
            var q = LoadQuest(questID);
            _quests[questID.GetRawValue()] = q;
        }

        #region IQuestCollection<TCharacter> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IQuest<TCharacter>> GetEnumerator()
        {
            return _quests.Where(x => x != null).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the <see cref="IQuest{TCharacter}"/> for a given <see cref="QuestID"/>.
        /// </summary>
        /// <param name="questID">The ID of the quest.</param>
        /// <returns>The <see cref="IQuest{TCharacter}"/> for the given <paramref name="questID"/>.</returns>
        public IQuest<TCharacter> GetQuest(QuestID questID)
        {
            return _quests[questID.GetRawValue()];
        }

        #endregion
    }
}