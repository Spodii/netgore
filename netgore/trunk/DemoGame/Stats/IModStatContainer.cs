using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Interface for an object that can contribute the modified Stat values.
    /// </summary>
    public interface IModStatContainer
    {
        /// <summary>
        /// Gets the modifier value for the given <paramref name="statType"/>, where a positive value adds to the
        /// mod stat value, a negative value subtracts from the mod stat value, and a value of 0 does not modify
        /// the mod stat value.
        /// </summary>
        /// <param name="statType">The StatType to get the modifier value for.</param>
        /// <returns>The modifier value for the given <paramref name="statType"/>.</returns>
        int GetStatModBonus(StatType statType);
    }
}