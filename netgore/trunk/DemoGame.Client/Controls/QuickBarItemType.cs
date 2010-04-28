using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains the different type of items that can be in the quick bar.
    /// </summary>
    public enum QuickBarItemType : byte
    {
        /// <summary>
        /// An empty slot.
        /// </summary>
        None,

        /// <summary>
        /// A slot containing an inventory item.
        /// </summary>
        Inventory,

        /// <summary>
        /// A slot containing a skill.
        /// </summary>
        Skill
    }
}