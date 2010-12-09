using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.Quest"/>.
    /// </summary>
    public enum QuestEventCounterType : byte
    {
        /// <summary>
        /// Quest was accepted.
        /// </summary>
        Accepted = 1,

        /// <summary>
        /// Quest was completed.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Quest was canceled.
        /// </summary>
        Canceled = 3,
    }
}