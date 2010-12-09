using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.Shop"/>.
    /// </summary>
    public enum ShopEventCounterType : byte
    {
        /// <summary>
        /// Number of times someone has purchased an item from this shop (each individual transaction).
        /// </summary>
        Buy = 1,

        /// <summary>
        /// Amount of items purchased from this shop (can be multiple per transaction).
        /// </summary>
        BuyAmount = 2,

        /// <summary>
        /// Value of items purchased from this shop.
        /// </summary>
        BuyValue = 3,

        /// <summary>
        /// Number of times someone has sold an item to this shop (each individual transaction).
        /// </summary>
        Sell = 4,

        /// <summary>
        /// Amount of items sold to this shop (can be multiple per transaction).
        /// </summary>
        SellAmount = 5,

        /// <summary>
        /// Value of all items sold to this shop.
        /// </summary>
        SellValue = 6,
    }
}