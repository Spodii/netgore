using System.Linq;
using NetGore;

namespace DemoGame.Client
{
    public class ShopInfo
    {
        readonly bool _canBuy;
        readonly ItemInfo[] _items;
        readonly string _name;
        readonly DynamicEntity _shopOwner;

        /// <summary>
        /// Gets if the shop can buy stuff. If false, the shop can only sell items.
        /// </summary>
        public bool CanBuy
        {
            get { return _canBuy; }
        }

        /// <summary>
        /// Gets the items in the shop.
        /// </summary>
        public ItemInfo[] Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the name of the shop.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the shop owner. Can be null.
        /// </summary>
        public DynamicEntity ShopOwner
        {
            get { return _shopOwner; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopInfo"/> class.
        /// </summary>
        /// <param name="shopOwner">The shop owner. Can be null.</param>
        /// <param name="name">The name.</param>
        /// <param name="canBuy">if set to <c>true</c> [can buy].</param>
        /// <param name="items">The items.</param>
        public ShopInfo(DynamicEntity shopOwner, string name, bool canBuy, ItemInfo[] items)
        {
            _shopOwner = shopOwner;
            _items = items;
            _name = name;
            _canBuy = canBuy;
        }

        /// <summary>
        /// Gets the <see cref="ItemInfo"/> at the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">The slot of the item.</param>
        /// <returns>The <see cref="ItemInfo"/> at the given <paramref name="slot"/>, or null if the
        /// <see cref="slot"/> was invalid or not item was in the specified slot.</returns>
        public ItemInfo GetItemInfo(ShopItemIndex slot)
        {
            if (_items == null)
                return null;

            if (slot < 0 || slot >= _items.Length)
                return null;

            return _items[slot];
        }
    }
}