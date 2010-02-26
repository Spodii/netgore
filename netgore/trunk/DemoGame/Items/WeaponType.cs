using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Describes what type of weapon an item is.
    /// </summary>
    public enum WeaponType : byte
    {
        /// <summary>
        /// An item that is either not a weapon, or is a weapon that cannot actually be used to attack.
        /// </summary>
        Unknown,

        /// <summary>
        /// A weapon that can only be used in melee (e.g. swords and axes).
        /// </summary>
        Melee,

        /// <summary>
        /// A weapon that fires projectiles (e.g. guns and bows).
        /// </summary>
        Gun,

        /// <summary>
        /// A weapon that is a thrown projectile (e.g. throwing knives and ninja stars).
        /// </summary>
        Projectile
    }
}