using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Interface for a class that contains the information on an item instance.
    /// </summary>
    public interface IItemInfo
    {
        /// <summary>
        /// Gets the <see cref="IStatCollection"/> containing the base stats.
        /// </summary>
        IStatCollection BaseStats { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the <see cref="GrhIndex"/>.
        /// </summary>
        GrhIndex GrhIndex { get; }

        /// <summary>
        /// Gets the amount of HP the item restores upon use.
        /// </summary>
        SPValueType HP { get; }

        /// <summary>
        /// Gets the amount of MP the item restores upon use.
        /// </summary>
        SPValueType MP { get; }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="IStatCollection"/> containing the required stats.
        /// </summary>
        IStatCollection ReqStats { get; }

        /// <summary>
        /// Gets the base value of the item.
        /// </summary>
        int Value { get; }
    }
}