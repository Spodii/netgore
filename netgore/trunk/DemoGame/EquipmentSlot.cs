using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public static class EquipmentSlotExtensions
    {
        static readonly int _highestSlotIndex = Enum.GetValues(typeof(EquipmentSlot)).Cast<byte>().Max();

        public static int HighestIndex
        {
            get { return _highestSlotIndex; }
        }

        public static byte GetIndex(this EquipmentSlot slot)
        {
            Debug.Assert(((EquipmentSlot)((byte)slot)) == slot, "Conversion to byte results in data loss...");
            return (byte)slot;
        }

        /// <summary>
        /// Checks if a specified EquipmentSlot value is defined by the EquipmentSlot enum.
        /// </summary>
        /// <param name="slot">EquipmentSlot value to check.</param>
        /// <returns>True if the <paramref name="slot"/> is defined in the EquipmentSlot enum, else false.</returns>
        public static bool IsDefined(this EquipmentSlot slot)
        {
            return Enum.IsDefined(typeof(EquipmentSlot), slot);
        }
    }
}