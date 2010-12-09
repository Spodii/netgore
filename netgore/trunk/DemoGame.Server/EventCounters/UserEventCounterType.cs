using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.User"/>.
    /// </summary>
    public enum UserEventCounterType : byte
    {
        /// <summary>
        /// User killed another <see cref="User"/>.
        /// </summary>
        KillUser = 1,

        /// <summary>
        /// User killed a <see cref="Character"/> other than a <see cref="User"/>.
        /// </summary>
        KillNonUser = 2,
    }
}