using System.Collections.Generic;
using System.Linq;
using NetGore.World;

namespace NetGore.Features.Shops
{
    public class ShopInfo<TItemInfo>
    {
        readonly bool _canBuy;
        readonly TItemInfo[] _items;
        readonly string _name;
        readonly DynamicEntity _shopOwner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopInfo{TItemInfo}"/> class.
        /// </summary>
        /// <param name="shopOwner">The shop owner. Can be null.</param>
        /// <param name="name">The name.</param>
        /// <param name="canBuy">if set to <c>true</c> [can buy].</param>
        /// <param name="items">The items.</param>
        public ShopInfo(DynamicEntity shopOwner, string name, bool canBuy, TItemInfo[] items)
        {
            _shopOwner = shopOwner;
            _items = items;
            _name = name;
            _canBuy = canBuy;
        }

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
        public ICollection<TItemInfo> Items
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
        /// Gets the item info at the given <paramref name="slot"/>.
        /// </summary>
        /// <param name="slot">The slot of the item.</param>
        /// <returns>The item info at the given <paramref name="slot"/>, or null if the
        /// <see cref="slot"/> was invalid or not item was in the specified slot.</returns>
        public TItemInfo GetItemInfo(ShopItemIndex slot)
        {
            if (_items == null)
                return default(TItemInfo);

            var i = slot.GetRawValue();
            if (i < 0 || i >= _items.Length)
                return default(TItemInfo);

            return _items[i];
        }
    }
}