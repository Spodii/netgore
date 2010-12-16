using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.ItemTemplate"/>.
    /// </summary>
    public enum ItemTemplateEventCounterType : byte
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

        /// <summary>
        /// Item was used to by a character to kill another character. Only valid for weapons.
        /// </summary>
        CharactersKilled = 4,

        /// <summary>
        /// Item was equipped by a character when they died. Only valid for items that can be equipped.
        /// </summary>
        CharacterDeaths = 5,

        /// <summary>
        /// Item was dropped as loot from a NPC after it was killed.
        /// </summary>
        DroppedAsLoot = 6,

        /// <summary>
        /// Item was traded in a peer trading session.
        /// </summary>
        PeerTraded = 7,

        /// <summary>
        /// Item expired while sitting on the map.
        /// </summary>
        ExpiredOnMap = 8,
    }
}