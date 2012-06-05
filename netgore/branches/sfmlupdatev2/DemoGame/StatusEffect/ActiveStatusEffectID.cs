using System;
using System.Linq;
using NetGore.Features.StatusEffects;

namespace DemoGame
{
    /// <summary>
    /// Represents the unique ID of an active <see cref="StatusEffect{TStatType,TStatusEffectType}"/>.
    /// </summary>
    public struct ActiveStatusEffectID : IEquatable<ActiveStatusEffectID>
    {
        readonly int _value;

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ActiveStatusEffectID left, ActiveStatusEffectID right)
        {
            return left._value == right._value;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ActiveStatusEffectID left, ActiveStatusEffectID right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is ActiveStatusEffectID && this == (ActiveStatusEffectID)obj;
        }

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

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ActiveStatusEffectID other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}