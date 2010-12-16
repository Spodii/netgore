using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.NPC"/>.
    /// </summary>
    public enum NPCEventCounterType : byte
    {
        /// <summary>
        /// NPC killed a <see cref="Character"/> that was not a <see cref="User"/>.
        /// </summary>
        KillNonUser = 1,

        /// <summary>
        /// NPC killed a user.
        /// </summary>
        KillUser = 2,

        /// <summary>
        /// The damage the NPC has dealt to a <see cref="User"/>.
        /// </summary>
        DamageDealtToUser = 3,

        /// <summary>
        /// The damage the NPC has dealt to a <see cref="Character"/> other than a <see cref="User"/>.
        /// </summary>
        DamageDealtToNonUser = 4,

        /// <summary>
        /// A <see cref="User"/> dealt damage to this NPC.
        /// </summary>
        DamageTakenFromUser = 5,

        /// <summary>
        /// A <see cref="Character"/> or than a <see cref="User"/> dealt damage to this NPC.
        /// </summary>
        DamageTakenFromNonUser = 6,
    }
}