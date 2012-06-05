using System;
using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// <see cref="EventArgs"/> for when a <see cref="Character"/> attacks.
    /// </summary>
    public class CharacterAttackEventArgs : EventArgs
    {
        readonly Character _attacked;
        readonly int _damage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterAttackEventArgs"/> class.
        /// </summary>
        /// <param name="attacked">The <see cref="Character"/> that was attacked.</param>
        /// <param name="damage">The amount of damage inflicted on the <paramref name="attacked"/> by
        /// the attacker.</param>
        public CharacterAttackEventArgs(Character attacked, int damage)
        {
            _attacked = attacked;
            _damage = damage;
        }

        /// <summary>
        /// Gets the <see cref="Character"/> that was attacked.
        /// </summary>
        public Character Attacked
        {
            get { return _attacked; }
        }

        /// <summary>
        /// Gets the amount of damage inflicted on the <see cref="Attacked"/> by the attacker.
        /// </summary>
        public int Damage
        {
            get { return _damage; }
        }
    }
}