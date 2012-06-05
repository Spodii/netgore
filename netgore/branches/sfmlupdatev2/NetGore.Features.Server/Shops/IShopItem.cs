using System.Linq;

namespace NetGore.Features.Shops
{
    /// <summary>
    /// Interface for an object that describes an item that an <see cref="IShop{T}"/> has available.
    /// </summary>
    public interface IShopItem<out TItemTemplate>
    {
        /// <summary>
        /// Gets the template for this shop item.
        /// </summary>
        TItemTemplate ItemTemplate { get; }

        /// <summary>
        /// Gets the <see cref="ShopID"/> for the shop that this <see cref="IShopItem{T}"/> belongs to.
        /// </summary>
        ShopID ShopID { get; }
    }
}