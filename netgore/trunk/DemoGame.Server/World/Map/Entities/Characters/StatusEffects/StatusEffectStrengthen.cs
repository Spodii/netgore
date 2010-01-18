using System.Linq;
using NetGore.Features.StatusEffects;

namespace DemoGame.Server
{
    public class StatusEffectStrengthen : StatusEffect<StatType, StatusEffectType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectStrengthen"/> class.
        /// </summary>
        StatusEffectStrengthen() : base(DemoGame.StatusEffectType.Strengthen, DefaultStatusEffectMergeType.Buff)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the time, in milliseconds, that the
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> will last.
        /// </summary>
        /// <param name="power">The power of the <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// to get the time of.</param>
        /// <returns>
        /// The time a <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> with the given
        /// <paramref name="power"/> will last.
        /// </returns>
        public override int GetEffectTime(ushort power)
        {
            return CalculateEffectTime(0, 10);
        }

        /// <summary>
        /// When overridden in the derived class, gets the stat bonus for the given <paramref name="statType"/> for
        /// a StatusEffect with the given <paramref name="power"/>. All return values must be the same for each
        /// <paramref name="statType"/> and <paramref name="power"/> pair. That is, the same two input values must
        /// always result in the exact same output value.
        /// </summary>
        /// <param name="statType">The stat type to get the modifier bonus of.</param>
        /// <param name="power">The power of the StatusEffect.</param>
        /// <returns>
        /// The modifier bonus from this StatusEffect on the given <paramref name="statType"/> with
        /// the given <paramref name="power"/>, or null if the <paramref name="statType"/> is not altered
        /// by this StatusEffect.
        /// </returns>
        protected override int? InternalTryGetStatModifier(StatType statType, ushort power)
        {
            switch (statType)
            {
                case StatType.Str:
                    return 1 + (power / 5);

                default:
                    return null;
            }
        }
    }
}