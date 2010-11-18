using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.Shops
{
    /// <summary>
    /// Interface for a shop.
    /// </summary>
    /// <typeparam name="TShopItem">The type of shop item.</typeparam>
    public interface IShop<out TShopItem>
    {
        /// <summary>
        /// Gets if this shop can buy items from shoppers instead of just sell items to them.
        /// </summary>
        bool CanBuy { get; }

        /// <summary>
        /// Gets the ID of the shop.
        /// </summary>
        ShopID ID { get; }

        /// <summary>
        /// Gets the name of the shop.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the items in this shop.
        /// </summary>
        IEnumerable<TShopItem> ShopItems { get; }

        /// <summary>
        /// Gets the item at the specified <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">The slot of the shop item.</param>
        /// <returns>The shop item at the specified <paramref name="slot"/>, or null if
        /// the slot was invalid or contains no item.</returns>
        TShopItem GetShopItem(ShopItemIndex slot);

        /// <summary>
        /// Writes the information describing the shop items to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        void WriteShopItems(IValueWriter writer);
    }
}