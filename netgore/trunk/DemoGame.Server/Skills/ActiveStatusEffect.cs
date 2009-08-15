using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes an instance of an active StatusEffectBase on a Character.
    /// </summary>
    public class ActiveStatusEffect
    {
        readonly StatusEffectBase _statusEffect;
        readonly ushort _power;

        /// <summary>
        /// Gets the StatusEffectBase that this ActiveStatusEffect
        /// </summary>
        public StatusEffectBase StatusEffect
        {
            get { return _statusEffect; }
        }

        /// <summary>
        /// Gets the power of the StatusEffect. How the status effect's power is utilized depends completely
        /// on the StatusEffect used.
        /// </summary>
        public ushort Power
        {
            get
            {
                return _power;
            }
        }

        /// <summary>
        /// ActiveStatusEffect constructor.
        /// </summary>
        /// <param name="statusEffect">The StatusEffectBase to use.</param>
        /// <param name="power">The power of the StatusEffect.</param>
        public ActiveStatusEffect(StatusEffectBase statusEffect, ushort power)
        {
            _statusEffect = statusEffect;
            _power = power;
        }
    }
}
