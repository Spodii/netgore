using System.Linq;
using NetGore.Features.StatusEffects;

namespace DemoGame
{
    /// <summary>
    /// Represents the unique ID of an active <see cref="StatusEffect{TStatType,TStatusEffectType}"/>.
    /// </summary>
    public struct ActiveStatusEffectID
    {
        readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveStatusEffectID"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public ActiveStatusEffectID(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DemoGame.ActiveStatusEffectID"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator int(ActiveStatusEffectID value)
        {
            return value._value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Int32"/> to <see cref="DemoGame.ActiveStatusEffectID"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator ActiveStatusEffectID(int value)
        {
            return new ActiveStatusEffectID(value);
        }
    }
}