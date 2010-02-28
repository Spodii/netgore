using System;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Delegate for handling an event from the <see cref="IQuestPerformerKillCounter{TCharacter, TKillID}"/> for
    /// when a kill counter has incremented.
    /// </summary>
    /// <typeparam name="TCharacter">The type of quest performer.</typeparam>
    /// <typeparam name="TKillID">The type of identifier for the targets to kill.</typeparam>
    /// <param name="sender">The <see cref="IQuestPerformerKillCounter{TCharacter, TKillID}"/> the event came from.</param>
    /// <param name="quest">The quest that the kill count was incremented on.</param>
    /// <param name="target">The target that the kill count was incremented for.</param>
    /// <param name="count">The current kill count for the <paramref name="target"/> for the
    /// <paramref name="quest"/>.</param>
    /// <param name="reqCount">The required kill count for completing the quest.</param>
    public delegate void QuestPerformerKillCounterKillIncrementEventHandler<TCharacter, TKillID>(
        IQuestPerformerKillCounter<TCharacter, TKillID> sender, IQuest<TCharacter> quest, TKillID target, ushort count, ushort reqCount) where TCharacter : IQuestPerformer<TCharacter> where TKillID : IEquatable<TKillID>;
}