using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    /// <summary>
    /// Same as <see cref="GameData"/>, but for information only available on the server.
    /// </summary>
    public static class ServerGameData
    {
        /// <summary>
        /// Gets the default amount of money a Character will pay for buying the given <paramref name="item"/> from
        /// a shop.
        /// </summary>
        /// <param name="item">The item to purchase.</param>
        /// <returns>the default amount of money a Character will pay for buying the given <paramref name="item"/>
        /// from a shop.</returns>
        public static int GetItemBuyValue(ItemEntity item)
        {
            return item.Value;
        }

        /// <summary>
        /// Gets the default amount of money a Character will get for selling the given <paramref name="item"/> to
        /// a shop.
        /// </summary>
        /// <param name="item">The item to sell.</param>
        /// <returns>the default amount of money a Character will get for selling the given <paramref name="item"/>
        /// to a shop.</returns>
        public static int GetItemSellValue(ItemEntity item)
        {
            return Math.Max(item.Value / 2, 1);
        }
    }
}
