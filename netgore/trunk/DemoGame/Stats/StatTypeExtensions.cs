using System;
using System.Linq;
using NetGore;

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
        /// <param name="statCollectionType">The StatCollectionType of the Stat to get the field for.</param>
        /// <returns>The Database field name for the <paramref name="statType"/>.</returns>
        public static string GetDatabaseField(this StatType statType, StatCollectionType statCollectionType)
        {
            switch (statCollectionType)
            {
                case StatCollectionType.Base:
                    return statType.ToString().ToLower();

                case StatCollectionType.Requirement:
                    return "req" + statType.GetDatabaseField(StatCollectionType.Base);

                case StatCollectionType.Modified:
                    throw new ArgumentException("StatCollectionType.Modified is not allowed in the database.",
                                                "statCollectionType");

                default:
                    throw new ArgumentOutOfRangeException("statCollectionType");
            }
        }

        /// <summary>
        /// Gets the integer value of the given <paramref name="statType"/>. This value is unique for each
        /// individual StatType.
        /// </summary>
        /// <param name="statType">The StatType to get the value for.</param>
        /// <returns>The integer value of the given <paramref name="statType"/>.</returns>
        public static byte GetValue(this StatType statType)
        {
            return (byte)statType;
        }

        /// <summary>
        /// Checks if a specified StatType value is defined by the StatType enum.
        /// </summary>
        /// <param name="statType">StatType value to check.</param>
        /// <returns>True if the <paramref name="statType"/> is defined in the StatType enum, else false.</returns>
        public static bool IsDefined(this StatType statType)
        {
            return EnumHelper.IsDefined(statType);
        }
    }
}