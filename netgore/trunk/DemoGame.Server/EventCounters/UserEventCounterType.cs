using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.User"/>.
    /// </summary>
    public enum UserEventCounterType : byte
    {
        #region User and NPC events

        /// <summary>
        /// <see cref="User"/> killed another <see cref="User"/>.
        /// </summary>
        KillUser = 1,

        /// <summary>
        /// <see cref="User"/> killed a <see cref="Character"/> other than a <see cref="User"/>.
        /// </summary>
        KillNonUser = 2,

        /// <summary>
        /// The damage the <see cref="User"/> has dealt to another <see cref="User"/>.
        /// </summary>
        DamageDealtToUser = 3,

        /// <summary>
        /// The damage the <see cref="User"/> has dealt to a <see cref="Character"/> other than a <see cref="User"/>.
        /// </summary>
        DamageDealtToNonUser = 4,

        /// <summary>
        /// A <see cref="User"/> dealt damage to this <see cref="User"/>.
        /// </summary>
        DamageTakenFromUser = 5,

        /// <summary>
        /// A <see cref="Character"/> other than a <see cref="User"/> dealt damage to this <see cref="User"/>.
        /// </summary>
        DamageTakenFromNonUser = 6,

        /// <summary>
        /// The <see cref="User"/> has attacked something.
        /// </summary>
        Attack = 7,

        /// <summary>
        /// The <see cref="User"/> was attacked by something.
        /// </summary>
        Attacked = 8,

        /// <summary>
        /// The <see cref="User"/> used a use-once item.
        /// </summary>
        ItemConsumed = 9,

        #endregion

        #region User-only events

        /// <summary>
        /// The times a <see cref="User"/> sent a chat message to the local area.
        /// </summary>
        ChatLocalTimes = 100,

        /// <summary>
        /// The number of characters in messages sent to the local area.
        /// </summary>
        ChatLocalChars = 101,

        /// <summary>
        /// The times a <see cref="User"/> sent a chat message to everyone.
        /// </summary>
        ChatShoutTimes = 102,

        /// <summary>
        /// The number of characters in messages sent to everyone.
        /// </summary>
        ChatShoutChars = 103,

        /// <summary>
        /// The times a <see cref="User"/> sent a chat message to a single <see cref="User"/>.
        /// </summary>
        ChatTellTimes = 104,

        /// <summary>
        /// The number of characters in messages sent to a single <see cref="User"/>.
        /// </summary>
        ChatTellChars = 105,

        #endregion
    }
}