using System;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Interface for an object that can contribute the modified stat values.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public interface IModStatContainer<in TStatType> where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Gets the modifier value for the given <paramref name="statType"/>, where a positive value adds to the
        /// mod stat value, a negative value subtracts from the mod stat value, and a value of 0 does not modify
        /// the mod stat value.
        /// </summary>
        /// <param name="statType">The <typeparamref name="TStatType"/> to get the modifier value for.</param>
        /// <returns>The modifier value for the given <paramref name="statType"/>.</returns>
        int GetStatModBonus(TStatType statType);
    }
}