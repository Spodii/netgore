using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    public sealed class StatusEffectTypeHelper : EnumIOHelper<StatusEffectType>
    {
        static readonly StatusEffectTypeHelper _instance;

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
        /// Gets the <see cref="StatusEffectTypeHelper"/> instance.
        /// </summary>
        public static StatusEffectTypeHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <typeparamref name="T"/>.</returns>
        public override StatusEffectType FromInt(int value)
        {
            return (StatusEffectType)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <typeparamref name="T"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        public override int ToInt(StatusEffectType value)
        {
            return (int)value;
        }
    }
}