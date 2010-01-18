using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.IO;

namespace NetGore.Features.Shop
{
    public interface IShop<TShopItem>
    {
        /// <summary>
        /// Gets an IEnumerable of the items in this <see cref="Shop"/>.
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
        /// Gets if this shop can buy items instead of just sell them.
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
        /// Writes the information describing the shop items to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        void WriteShopItems(IValueWriter writer);
    }
}