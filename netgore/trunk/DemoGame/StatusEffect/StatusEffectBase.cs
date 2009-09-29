using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.RPGComponents;

namespace DemoGame
{
    public abstract class StatusEffectBase : StatusEffectBase<StatType, StatusEffectType>
    {
        /// <summary>
        /// StatusEffectBase constructor.
        /// </summary>
        /// <param name="statusEffectType">The StatusEffectType that this StatusEffectBase handles.</param>
        /// <param name="mergeType">The StatusEffectMergeType that describes how to handle merging multiple
        /// applications of this StatusEffect onto the same object.</param>
        protected StatusEffectBase(StatusEffectType statusEffectType, StatusEffectMergeType mergeType)
            : base(statusEffectType, mergeType, GameData.MaxStatusEffectPower)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of all the values in the
        /// <see cref="StatType"/> Enum.
        /// </summary>
        /// <returns>
        /// An IEnumerable of all the values in the <see cref="StatType"/> Enum.
        /// </returns>
        protected override IEnumerable<StatType> GetStatTypes()
        {
            return StatTypeHelper.Values;
        }
    }
}
