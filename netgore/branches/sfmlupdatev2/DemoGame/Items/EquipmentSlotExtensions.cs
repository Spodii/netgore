using System.Diagnostics;
using System.Linq;

namespace DemoGame
{
    public static class EquipmentSlotExtensions
    {
        /// <summary>
        /// Gets the integer value of the given <paramref name="equipmentSlot"/>. This value is unique for each
        /// individual EquipmentSlot.
        /// </summary>
        /// <param name="equipmentSlot">The EquipmentSlot to get the value for.</param>
        /// <returns>The integer value of the given <paramref name="equipmentSlot"/>.</returns>
        public static byte GetValue(this EquipmentSlot equipmentSlot)
        {
            Debug.Assert(((EquipmentSlot)((byte)equipmentSlot)) == equipmentSlot, "Conversion to byte results in data loss...");
            return (byte)equipmentSlot;
        }
    }
}