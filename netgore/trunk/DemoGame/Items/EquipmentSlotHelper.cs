using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoGame
{
    public static class EquipmentSlotHelper
    {
        static readonly EquipmentSlot[] _allEquipmentSlots = Enum.GetValues(typeof(EquipmentSlot)).Cast<EquipmentSlot>().ToArray();
        static readonly int _greatestValue = AllEquipmentSlots.Select(x => x.GetValue()).Max();

        /// <summary>
        /// Gets an IEnumerable of all of the EquipmentSlots.
        /// </summary>
        public static IEnumerable<EquipmentSlot> AllEquipmentSlots
        {
            get { return _allEquipmentSlots; }
        }

        /// <summary>
        /// Gets the greatest value from GetValue() from each EquipmentSlot.
        /// </summary>
        public static int GreatestValue
        {
            get { return _greatestValue; }
        }
    }
}