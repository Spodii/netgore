using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace DemoGame
{
    [Obsolete]
    public static class StatTypeHelper
    {
        static readonly StatType[] _allValues = EnumHelper<StatType>.Values.ToArray();
        static readonly int _greatestValue = AllValues.Select(x => x.GetValue()).Max();

        /// <summary>
        /// Gets an IEnumerable of all of the StatTypes.
        /// </summary>
        public static IEnumerable<StatType> AllValues
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

        /// <summary>
        /// Gets an IEnumerable of all of the StatTypes who's base value can be raised by a Character.
        /// </summary>
        public static IEnumerable<StatType> RaisableStats
        {
            get { return AllValues; }
        }
    }
}