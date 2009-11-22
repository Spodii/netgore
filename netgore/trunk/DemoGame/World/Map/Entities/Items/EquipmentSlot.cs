using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Enum of all the different types of equipment and their corresponding numeric identifier.
    /// </summary>
    public enum EquipmentSlot : byte
    {
        // Each value is explicitly assigned to ensure the order does not change since these values are used
        // directly in the database.

        Head = 0,
        Body = 1,
        RightHand = 2,
        LeftHand = 3
    }
}