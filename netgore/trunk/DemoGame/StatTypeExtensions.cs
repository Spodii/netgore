using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Extensions for the DemoGame.StatType.
    /// </summary>
    public static class StatTypeExtensions
    {
        /// <summary>
        /// Gets the database field name for a given StatType.
        /// </summary>
        /// <param name="statType">StatType to get the database field name for.</param>
        /// <returns>The Database field name for the <paramref name="statType"/>.</returns>
        public static string GetDatabaseField(this StatType statType)
        {
            return statType.ToString().ToLower();
        }

        /// <summary>
        /// Checks if a specified StatType value is defined by the StatType enum.
        /// </summary>
        /// <param name="statType">StatType value to check.</param>
        /// <returns>True if the <paramref name="statType"/> is defined in the StatType enum, else false.</returns>
        public static bool IsDefined(this StatType statType)
        {
            return Enum.IsDefined(typeof(StatType), statType);
        }
    }
}