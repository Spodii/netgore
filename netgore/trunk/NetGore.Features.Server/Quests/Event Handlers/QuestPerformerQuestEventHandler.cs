using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IQuestPerformer{TCharacter}"/>.
    /// </summary>
    /// <typeparam name="TCharacter">The type of <see cref="IQuestPerformer{TCharacter}"/>.</typeparam>
    /// <param name="questPerformer">The quest performer that the event came from.</param>
    /// <param name="quest">The quest related to the event.</param>
    public delegate void QuestPerformerQuestEventHandler<TCharacter>(TCharacter questPerformer, IQuest<TCharacter> quest)
        where TCharacter : IQuestPerformer<TCharacter>;
}