using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame
{
    public static class StatTypeHelper
    {
        static readonly StatType[] _allStatTypes = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToArray();
        static readonly int _greatestValue = AllStatTypes.Select(x => x.GetValue()).Max();

        /// <summary>
        /// Gets an IEnumerable of all of the StatTypes.
        /// </summary>
        public static IEnumerable<StatType> AllStatTypes
        {
            get { return _allStatTypes; }
        }

        /// <summary>
        /// Gets an IEnumerable of all of the StatTypes who's base value can be raised by a Character.
        /// </summary>
        public static IEnumerable<StatType> RaisableStats
        {
            get { return AllStatTypes; }
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
