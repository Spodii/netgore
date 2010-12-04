using System;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// <see cref="EventArgs"/> for when a kill counter has incremented on a
    /// <see cref="IQuestPerformerKillCounter{TCharacter, TKillID}"/>.
    /// </summary>
    /// <typeparam name="TCharacter"></typeparam>
    /// <typeparam name="TKillID"></typeparam>
    public class QuestPerformerKillCounterKillIncrementEventArgs<TCharacter, TKillID> : EventArgs
         where TCharacter : IQuestPerformer<TCharacter>
    {
        readonly IQuest<TCharacter> _quest;
        readonly TKillID _target;
        readonly ushort _count;
        readonly ushort _reqCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestPerformerKillCounterKillIncrementEventArgs{TCharacter, TKillID}"/> class.
        /// </summary>
        /// <param name="quest">The quest that the kill count was incremented on.</param>
        /// <param name="target">The target that the kill count was incremented for.</param>
        /// <param name="count">The current kill count for the <paramref name="target"/> for the
        /// <paramref name="quest"/>.</param>
        /// <param name="reqCount">The required kill count for completing the quest.</param>
        public QuestPerformerKillCounterKillIncrementEventArgs(IQuest<TCharacter> quest, TKillID target, ushort count,
        ushort reqCount)
        {
            _quest = quest;
            _target = target;
            _count = count;
            _reqCount = reqCount;
        }

        /// <summary>
        /// Gets the quest that the kill count was incremented on.
        /// </summary>
        public IQuest<TCharacter> Quest { get { return _quest; } }

        /// <summary>
        /// Gets the target that the kill count was incremented for.
        /// </summary>
        public TKillID Target { get { return _target; } }

        /// <summary>
        /// Gets the current kill count for the <see cref="Target"/> for the <see cref="Quest"/>.
        /// </summary>
        public ushort Count { get { return _count; } }

        /// <summary>
        /// Gets the required kill count for completing the quest.
        /// </summary>
        public ushort ReqCount { get { return _reqCount; } }
    }
}