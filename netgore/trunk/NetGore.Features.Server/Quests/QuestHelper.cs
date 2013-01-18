using System.Linq;

namespace NetGore.Features.Quests
{
    /// <summary>
    /// Contains helper methods for quests.
    /// </summary>
    public static class QuestHelper
    {
        /// <summary>
        /// Gets the quests that a <see cref="IQuestPerformer{T}"/> can accept from or turn in to a
        /// <see cref="IQuestProvider{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of quest performer.</typeparam>
        /// <param name="performer">The quest performer.</param>
        /// <param name="provider">The quest provider.</param>
        /// <param name="availableQuests">The quests that the <paramref name="performer"/> can accept to start from
        /// the <paramref name="provider"/>.</param>
        /// <param name="turnInQuests">The quests that the <paramref name="performer"/> has completed and can be
        /// turned in to the <paramref name="provider"/>.</param>
        public static void GetAvailableQuests<T>(T performer, IQuestProvider<T> provider, out IQuest<T>[] availableQuests,
                                                 out IQuest<T>[] turnInQuests) where T : IQuestPerformer<T>
        {
            availableQuests = provider.Quests
                .Where(x => !performer.ActiveQuests.Contains(x) && x.StartRequirements.HasRequirements(performer) && (x.Repeatable || !performer.HasCompletedQuest(x))).ToArray();

            var canTurnInQuests = performer.ActiveQuests.Where(x => x.FinishRequirements.HasRequirements(performer)).ToImmutable();
            turnInQuests = provider.Quests.Where(x => canTurnInQuests.Contains(x)).ToArray();
        }
    }
}