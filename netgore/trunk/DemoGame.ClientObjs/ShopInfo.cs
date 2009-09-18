using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Client
{
    public class ShopInfo
    {
        readonly ItemInfo[] _items;
        readonly string _name;
        readonly bool _canBuy;

        /// <summary>
        /// Gets the items in the shop.
        /// </summary>
        public ItemInfo[] Items { get { return _items; } }
        
        /// <summary>
        /// Gets if the shop can buy stuff. If false, the shop can only sell items.
        /// </summary>
        public bool CanBuy { get { return _canBuy; } }

        /// <summary>
        /// Gets the name of the shop.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="canBuy">if set to <c>true</c> [can buy].</param>
        /// <param name="items">The items.</param>
        public ShopInfo(string name, bool canBuy, ItemInfo[] items)
        {
            _items = items;
            _name = name;
            _canBuy = canBuy;
        }
    }
}
