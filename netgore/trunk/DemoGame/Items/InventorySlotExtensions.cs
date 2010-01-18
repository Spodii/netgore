using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="InventorySlot"/>.
    /// </summary>
    public static class InventorySlotExtensions
    {
        /// <summary>
        /// Checks if the given <see cref="InventorySlot"/> is in legal range.
        /// </summary>
        /// <param name="slot">The InventorySlot.</param>
        /// <returns>True if the <paramref name="slot"/> is in legal range; otherwise false.</returns>
        public static bool IsLegalValue(this InventorySlot slot)
        {
            return slot >= 0 && slot < GameData.MaxInventorySize;
        }
    }
}