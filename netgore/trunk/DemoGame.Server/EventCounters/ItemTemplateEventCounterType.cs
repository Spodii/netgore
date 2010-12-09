namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.ItemTemplate"/>.
    /// </summary>
    public enum ItemTemplateEventCounterType:byte
    {
        /// <summary>
        /// Item created.
        /// </summary>
        Create = 0,

        /// <summary>
        /// Item consumed.
        /// </summary>
        Consume = 1,

        /// <summary>
        /// Item sold to a shop.
        /// </summary>
        SellToShop = 2,

        /// <summary>
        /// Item purchased from a shop.
        /// </summary>
        BuyFromShop = 3,
    }
}