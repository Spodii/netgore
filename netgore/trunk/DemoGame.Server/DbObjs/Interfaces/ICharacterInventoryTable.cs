using System.Linq;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `character_inventory`.
    /// </summary>
    public interface ICharacterInventoryTable
    {
        /// <summary>
        /// Gets the value of the database column `character_id`.
        /// </summary>
        CharacterID CharacterID { get; }

        /// <summary>
        /// Gets the value of the database column `item_id`.
        /// </summary>
        ItemID ItemID { get; }

        /// <summary>
        /// Gets the value of the database column `slot`.
        /// </summary>
        InventorySlot Slot { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        ICharacterInventoryTable DeepCopy();
    }
}