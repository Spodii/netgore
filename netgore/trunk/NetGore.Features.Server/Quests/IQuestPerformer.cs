using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Interface for an object that is able to accept and complete quests.
    /// </summary>
    /// <typeparam name="TCharacter">The type of the <see cref="IQuestPerformer{TCharacter}"/>
    /// that can perform the quest.</typeparam>
    public interface IQuestPerformer<TCharacter> where TCharacter : IQuestPerformer<TCharacter>
    {
        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has accepted a new quest.
        /// </summary>
        event TypedEventHandler<TCharacter, EventArgs<IQuest<TCharacter>>> QuestAccepted;

        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has canceled an active quest.
        /// </summary>
        event TypedEventHandler<TCharacter, EventArgs<IQuest<TCharacter>>> QuestCanceled;

        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has finished a quest.
        /// </summary>
        event TypedEventHandler<TCharacter, EventArgs<IQuest<TCharacter>>> QuestFinished;

        /// <summary>
        /// Gets the incomplete quests that this <see cref="IQuestPerformer{TCharacter}"/> is currently working on.
        /// </summary>
        IEnumerable<IQuest<TCharacter>> ActiveQuests { get; }

        /// <summary>
        /// Gets the quests that this <see cref="IQuestPerformer{TCharacter}"/> has completed.
        /// </summary>
        IEnumerable<IQuest<TCharacter>> CompletedQuests { get; }

        /// <summary>
        /// Gets the quests that this <see cref="IQuestPerformer{TCharacter}"/> has repeatable.
        /// </summary>
        IEnumerable<IQuest<TCharacter>> RepeatableQuests { get; }

        /// <summary>
        /// Gets if this <see cref="IQuestPerformer{TCharacter}"/> can accept the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to check if this <see cref="IQuestPerformer{TCharacter}"/> can accept.</param>
        /// <returns>True if this <see cref="IQuestPerformer{TCharacter}"/> can accept the given <paramref name="quest"/>;
        /// otherwise false.</returns>
        bool CanAcceptQuest(IQuest<TCharacter> quest);

        /// <summary>
        /// Cancels an active quest.
        /// </summary>
        /// <param name="quest">The active quest to cancel.</param>
        /// <returns>True if the <paramref name="quest"/> was canceled; false if the <paramref name="quest"/> failed to
        /// be canceled, such as if the <paramref name="quest"/> was not in the list of active quests.</returns>
        bool CancelQuest(IQuest<TCharacter> quest);

        /// <summary>
        /// Gets if this <see cref="IQuestPerformer{TCharacter}"/> has finished the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to check if this <see cref="IQuestPerformer{TCharacter}"/> has completed.</param>
        /// <returns>True if this <see cref="IQuestPerformer{TCharacter}"/> has completed the given <paramref name="quest"/>;
        /// otherwise false.</returns>
        bool HasCompletedQuest(IQuest<TCharacter> quest);

        /// <summary>
        /// Gets if this <see cref="IQuestPerformer{TCharacter}"/> can repeat the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to check if this <see cref="IQuestPerformer{TCharacter}"/> can repeat.</param>
        /// <returns>True if this <see cref="IQuestPerformer{TCharacter}"/> can repeat the given <paramref name="quest"/>;
        /// otherwise false.</returns>
        bool IsRepeatableQuest(IQuest<TCharacter> quest);

        /// <summary>
        /// Tries to add the given <paramref name="quest"/> to this <see cref="IQuestPerformer{TCharacter}"/>'s list
        /// of active quests.
        /// </summary>
        /// <param name="quest">The quest to try to add to this <see cref="IQuestPerformer{TCharacter}"/>'s list
        /// of active quests.</param>
        /// <returns>True if the <paramref name="quest"/> was successfully added; otherwise false.</returns>
        bool TryAddQuest(IQuest<TCharacter> quest);

        /// <summary>
        /// Tries to finish the given <paramref name="quest"/> that this <see cref="IQuestPerformer{TCharacter}"/>
        /// has started.
        /// </summary>
        /// <param name="quest">The quest to turn in.</param>
        /// <returns>True if the <paramref name="quest"/> was successfully finished; otherwise false.</returns>
        bool TryFinishQuest(IQuest<TCharacter> quest);
    }
}