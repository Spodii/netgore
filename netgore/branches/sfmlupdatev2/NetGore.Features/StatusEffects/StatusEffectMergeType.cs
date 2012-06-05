using System.Linq;

namespace NetGore.Features.StatusEffects
{
    /// <summary>
    /// Describes how a StatusEffect handles when the Character the StatusEffect is being added to
    /// already has a StatusEffect of the same type.
    /// </summary>
    public enum StatusEffectMergeType : byte
    {
        /// <summary>
        /// The newer StatusEffect (the one just added) will be discarded, leaving the old StatusEffect.
        /// </summary>
        DiscardNewer,

        /// <summary>
        /// The older StatusEffect (the one that already existed) will be discarded and the new StatusEffect
        /// will take its place.
        /// </summary>
        DiscardOlder,

        /// <summary>
        /// The StatusEffect with the least power is discarded. If the power is equal, the StatusEffect with the least
        /// amount of remaining time is discarded.
        /// </summary>
        DiscardWeakest,

        /// <summary>
        /// If the stronger StatusEffect has less than 30 seconds remaining, and the time of the stronger StatusEffect is
        /// less than the time of the weaker StatusEffect, then the stronger StatusEffect will be discarded. Otherwise, the
        /// StatusEffect with the least power is discarded. If the power is equal, the StatusEffect with the least
        /// amount of remaining time is discarded. 
        /// </summary>
        DiscardWeakestUnlessTimeUnder30Secs,

        /// <summary>
        /// The StatusEffect with the greatest power is discarded. If the power is equal, the StatusEffect with the least
        /// amount of remaining time is discarded.
        /// </summary>
        DiscardStrongest,

        /// <summary>
        /// The StatusEffects are joined together, with the result being a StatusEffect with the greater amount of remaining
        /// time and power.
        /// </summary>
        UseGreatestTimeAndPower,

        /// <summary>
        /// The StatusEffects are joined together, with the result being a StatusEffect with the least amount of remaining
        /// time and power.
        /// </summary>
        UseLeastTimeAndPower,

        /// <summary>
        /// The time of the two StatusEffects are added together, and the greater power value of the two is used.
        /// </summary>
        CombineTimeOnGreaterPower,

        /// <summary>
        /// The time of the two StatusEffects are added together, and the weaker power value of the two is used.
        /// </summary>
        CombineTimeOnWeakerPower,

        /// <summary>
        /// The power of the two StatusEffects are added together, and the greater remaining time of the two is used.
        /// </summary>
        CombinePowerOnGreaterTime,

        /// <summary>
        /// The power of the two StatusEffects are added together, and the least remaining time of the two is used.
        /// </summary>
        CombinePowerOnLeastTime
    }
}