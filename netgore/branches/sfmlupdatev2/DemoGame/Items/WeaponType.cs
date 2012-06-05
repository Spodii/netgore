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
        /// A weapon that can be used from a range (e.g. guns and bows).
        /// </summary>
        Ranged,

        /// <summary>
        /// A weapon that is a thrown projectile (e.g. throwing knives and rocks).
        /// </summary>
        Projectile
    }
}