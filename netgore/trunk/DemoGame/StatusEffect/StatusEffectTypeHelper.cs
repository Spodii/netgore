using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace DemoGame
{
    public class StatusEffectTypeHelper : EnumHelper<StatusEffectType>
    {
        static readonly StatusEffectTypeHelper _instance;

        /// <summary>
        /// Gets the <see cref="StatusEffectTypeHelper"/> instance.
        /// </summary>
        public static StatusEffectTypeHelper Instance { get { return _instance; } }

        /// <summary>
        /// Initializes the <see cref="StatusEffectTypeHelper"/> class.
        /// </summary>
        static StatusEffectTypeHelper()
        {
            _instance = new StatusEffectTypeHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectTypeHelper"/> class.
        /// </summary>
        StatusEffectTypeHelper()
        {
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <typeparamref name="T"/>.</returns>
        protected override StatusEffectType FromInt(int value)
        {
            return (StatusEffectType)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <typeparamref name="T"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(StatusEffectType value)
        {
            return (int)value;
        }
    }
}