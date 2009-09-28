using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace DemoGame
{
    [Obsolete]
    public static class EquipmentSlotHelper
    {
        static readonly EquipmentSlot[] _allValues = EnumHelper<EquipmentSlot>.Values.ToArray();
        static readonly int _greatestValue = AllValues.Select(x => x.GetValue()).Max();

        /// <summary>
        /// Gets an IEnumerable of all of the EquipmentSlots.
        /// </summary>
        public static IEnumerable<EquipmentSlot> AllValues
        {
            get { return _allValues; }
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