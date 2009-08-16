using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
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
