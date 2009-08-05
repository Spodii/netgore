using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// Enum of which type of form the ItemInfo came from.
    /// </summary>
    public enum ItemInfoSource
    {
        /// <summary>
        /// User's inventory.
        /// </summary>
        Inventory,

        /// <summary>
        /// User's equipped items.
        /// </summary>
        Equipped
    }
}