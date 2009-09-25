using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="ShopItemIndex"/>.
    /// </summary>
    public static class ShopItemIndexExtensions
    {
        /// <summary>
        /// Checks if the given <see cref="ShopItemIndex"/> is in legal range.
        /// </summary>
        /// <param name="index">The ShopItemIndex.</param>
        /// <returns>True if the <paramref name="index"/> is in legal range; otherwise false.</returns>
        public static bool IsLegalValue(this ShopItemIndex index)
        {
            return index >= 0 && index < GameData.MaxShopItems;
        }
    }
}
