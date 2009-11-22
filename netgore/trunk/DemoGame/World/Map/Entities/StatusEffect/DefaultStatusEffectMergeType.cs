using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Contains the default StatusEffectMergeType used for different kinds of StatusEffects.
    /// </summary>
    public static class DefaultStatusEffectMergeType
    {
        /// <summary>
        /// The default StatusEffectMergeType for buffs.
        /// </summary>
        public const StatusEffectMergeType Buff = StatusEffectMergeType.DiscardWeakestUnlessTimeUnder30Secs;
    }
}