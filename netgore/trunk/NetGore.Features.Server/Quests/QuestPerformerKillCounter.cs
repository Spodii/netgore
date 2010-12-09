using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for an object that keeps track of how many kills remain for quests that require killing a
    /// certain number of characters.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    /// <typeparam name="TKillID">The type of identifier for the targets to kill.</typeparam>
    public interface IQuestPerformerKillCounter<TCharacter, TKillID> : IDisposable where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Notifies listeners when the kill counter for a quest has been incremented. This event will only be invoked
        /// if, when the target was killed, the kill count was less than the required kill count. As a result, the
        /// kill count will never be zero.
        /// </summary>
        event
            TypedEventHandler
                <IQuestPerformerKillCounter<TCharacter, TKillID>,
                    QuestPerformerKillCounterKillIncrementEventArgs<TCharacter, TKillID>> KillCountIncremented;

        /// <summary>
        /// Gets the quest performer that this collection belongs to.
        /// </summary>
        TCharacter Owner { get; }

        /// <summary>
        /// Gets the kill counts for the given <paramref name="quest"/> for each of the required kill counters. The
        /// returned counts will not exceed the number of required kills for the <paramref name="quest"/> for the
        /// respective target.
        /// </summary>
        /// <param name="quest">The quest to get the kill counts for.</param>
        /// <returns>The kill counts for the <paramref name="quest"/> for each of the required characters to kill
        /// for the quest.</returns>
        /// <exception cref="ArgumentException"><paramref name="quest"/> has not been added to the collection.</exception>
        IEnumerable<KeyValuePair<TKillID, ushort>> GetKillCounts(IQuest<TCharacter> quest);

        /// <summary>
        /// Checks if the required kill count has been reached for all of the required targets for the given
        /// <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to check if the required kills have been made.</param>
        /// <returns>True if the required kill count has been reached for all of the required targets for the given
        /// <paramref name="quest"/>; otherwise false.</returns>
        /// <exception cref="ArgumentException"><paramref name="quest"/> has not been added to the collection.</exception>
        bool HasAllKills(IQuest<TCharacter> quest);
    }
}