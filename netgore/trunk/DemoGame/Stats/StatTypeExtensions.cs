using System;
using System.Linq;
using NetGore.Stats;

namespace DemoGame
{
    /// <summary>
    /// Extensions for the <see cref="StatType"/>.
    /// </summary>
    public static class StatTypeExtensions
    {
        /// <summary>
        /// Gets the database field name for a given <see cref="StatType"/>.
        /// </summary>
        /// <param name="statType"><see cref="StatType"/> to get the database field name for.</param>
        /// <param name="statCollectionType">The <see cref="StatCollectionType"/> of the <see cref="StatType"/> to get
        /// the field for.</param>
        /// <returns>The database field name for the <paramref name="statType"/>.</returns>
        /// <exception cref="ArgumentException">StatCollectionType.Modified is not allowed in the database.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statCollectionType"/> is not a valid
        /// <see cref="StatCollectionType"/> enum value.</exception>
        public static string GetDatabaseField(this StatType statType, StatCollectionType statCollectionType)
        {
            switch (statCollectionType)
            {
                case StatCollectionType.Base:
                    return "stat_" + statType.ToString().ToLowerInvariant();

                case StatCollectionType.Requirement:
                    return "stat_req_" + statType.ToString().ToLowerInvariant();

                case StatCollectionType.Modified:
                    throw new ArgumentException("StatCollectionType.Modified is not allowed in the database.", "statCollectionType");

                default:
                    throw new ArgumentOutOfRangeException("statCollectionType");
            }
        }

        /// <summary>
        /// Gets the integer value of the given <paramref name="statType"/>. This value is unique for each
        /// individual <see cref="StatType"/>.
        /// </summary>
        /// <param name="statType">The <see cref="StatType"/> to get the value for.</param>
        /// <returns>The integer value of the given <paramref name="statType"/>.</returns>
        public static byte GetValue(this StatType statType)
        {
            return (byte)statType;
        }
    }
}