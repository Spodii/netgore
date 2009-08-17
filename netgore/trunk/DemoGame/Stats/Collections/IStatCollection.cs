using System.Collections.Generic;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Interface for a collection of IStats that provides access to the IStat's value through the StatType.
    /// </summary>
    public interface IStatCollection : IEnumerable<IStat>
    {
        /// <summary>
        /// Gets or sets the value of the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get or set the value for.</param>
        /// <returns>The value of the stat with the given <paramref name="statType"/>.</returns>
        int this[StatType statType] { get; set; }

        /// <summary>
        /// Gets the StatCollectionType that this collection is for.
        /// </summary>
        StatCollectionType StatCollectionType { get; }

        /// <summary>
        /// Checks if this collection contains the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType to check if exists in the collection.</param>
        /// <returns>True if this collection contains the stat with the given <paramref name="statType"/>;
        /// otherwise false.</returns>
        bool Contains(StatType statType);

        /// <summary>
        /// Gets the IStat for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <returns>The IStat for the stat of the given <paramref name="statType"/>.</returns>
        IStat GetStat(StatType statType);

        /// <summary>
        /// Tries to get the IStat for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <param name="stat">The IStat for the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be null.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        bool TryGetStat(StatType statType, out IStat stat);

        /// <summary>
        /// Tries to get the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <param name="value">The value of the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be 0.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        bool TryGetStatValue(StatType statType, out int value);
    }
}