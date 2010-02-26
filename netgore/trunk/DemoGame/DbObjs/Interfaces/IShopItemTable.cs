using System.Linq;
using NetGore.Features.Shops;

namespace DemoGame.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `shop_item`.
    /// </summary>
    public interface IShopItemTable
    {
        /// <summary>
        /// Gets the value of the database column `item_template_id`.
        /// </summary>
        ItemTemplateID ItemTemplateID { get; }

        /// <summary>
        /// Gets the value of the database column `shop_id`.
        /// </summary>
        ShopID ShopID { get; }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IShopItemTable DeepCopy();
    }
}