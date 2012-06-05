using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.NPC"/>.
    /// </summary>
    public enum NPCEventCounterType : byte
    {
        #region User and NPC events

        /// <summary>
        /// <see cref="NPC"/> killed a <see cref="Character"/> that was not a <see cref="User"/>.
        /// </summary>
        KillNonUser = 1,

        /// <summary>
        /// <see cref="NPC"/> killed a <see cref="User"/>.
        /// </summary>
        KillUser = 2,

        /// <summary>
        /// The damage the <see cref="NPC"/> has dealt to a <see cref="User"/>.
        /// </summary>
        DamageDealtToUser = 3,

        /// <summary>
        /// The damage the <see cref="NPC"/> has dealt to a <see cref="Character"/> other than a <see cref="User"/>.
        /// </summary>
        DamageDealtToNonUser = 4,

        /// <summary>
        /// A <see cref="User"/> dealt damage to this <see cref="NPC"/>.
        /// </summary>
        DamageTakenFromUser = 5,

        /// <summary>
        /// A <see cref="Character"/> "/> than a <see cref="User"/> dealt damage to this <see cref="NPC"/>.
        /// </summary>
        DamageTakenFromNonUser = 6,

        /// <summary>
        /// The <see cref="NPC"/> has attacked something.
        /// </summary>
        Attack = 7,

        /// <summary>
        /// The <see cref="NPC"/> was attacked by something.
        /// </summary>
        Attacked = 8,

        /// <summary>
        /// The <see cref="NPC"/> used a use-once item.
        /// </summary>
        ItemConsumed = 9,

        #endregion

        #region NPC-only events

        #endregion
    }
}