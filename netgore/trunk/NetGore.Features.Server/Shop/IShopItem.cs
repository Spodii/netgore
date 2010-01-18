using System.Linq;

namespace NetGore.Features.Shops
{
    /// <summary>
    /// Interface for an object that describes an item that a <see cref="Shop"/> has available.
    /// </summary>
    public interface IShopItem<TItemTemplate>
    {
        /// <summary>
        /// Gets the <see cref="ShopID"/> for the shop that this <see cref="ShopItem"/> belongs to.
        /// </summary>
        ShopID ShopID { get; }

        /// <summary>
        /// Gets the template for this shop item.
        /// </summary>
        TItemTemplate ItemTemplate { get; }
    }
}