using System.Linq;
using NetGore.Features.StatusEffects;

namespace DemoGame
{
    /// <summary>
    /// Contains the default <see cref="StatusEffectMergeType"/> used for different kinds of
    /// <see cref="IStatusEffect{TStatType, TSkillType}"/>s.
    /// </summary>
    public static class DefaultStatusEffectMergeType
    {
        /// <summary>
        /// The default <see cref="StatusEffectMergeType"/> for buffs.
        /// </summary>
        public const StatusEffectMergeType Buff = StatusEffectMergeType.DiscardWeakestUnlessTimeUnder30Secs;
    }
}