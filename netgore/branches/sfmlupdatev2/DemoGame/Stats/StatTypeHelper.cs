using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Helper for the <see cref="StatType"/> enum.
    /// </summary>
    public static class StatTypeHelper
    {
        /// <summary>
        /// Gets an IEnumerable of all of the <see cref="StatType"/>s who's base value can be raised by a Character.
        /// </summary>
        public static IEnumerable<StatType> RaisableStats
        {
            get { return EnumHelper<StatType>.Values; }
        }
    }
}