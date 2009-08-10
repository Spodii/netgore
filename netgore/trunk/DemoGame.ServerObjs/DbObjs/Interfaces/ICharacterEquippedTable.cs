using System.Linq;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `character_equipped`.
    /// </summary>
    public interface ICharacterEquippedTable
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
        EquipmentSlot Slot { get; }
    }
}