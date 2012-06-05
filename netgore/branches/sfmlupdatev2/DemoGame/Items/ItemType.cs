using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Enum containing all of the different types of items.
    /// </summary>
    public enum ItemType : byte
    {
        // Each value is explicitly assigned to ensure the order does not change since these values are used
        // directly in the database.

        /// <summary>
        /// An item that can not be used at all in any way (ie items just for selling).
        /// </summary>
        Unusable = 0,

        /// <summary>
        /// An item that can be used, but only once (ie consumables).
        /// </summary>
        UseOnce = 1,

        /// <summary>
        /// A hand-held weapon.
        /// </summary>
        Weapon = 2,

        /// <summary>
        /// A helmet or other head-gear.
        /// </summary>
        Helmet = 3,

        /// <summary>
        /// Body armor or similar full-body gear.
        /// </summary>
        Body = 4
    }
}