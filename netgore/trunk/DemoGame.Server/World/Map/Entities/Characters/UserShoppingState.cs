using System.Linq;
using NetGore;
using NetGore.Features.Shops;
using NetGore.World;

namespace DemoGame.Server
{
    public class UserShoppingState : CharacterShoppingState<User, Character, ShopItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserShoppingState"/> class.
        /// </summary>
        /// <param name="character">The character doing the shopping.</param>
        public UserShoppingState(User character) : base(character)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="IMap"/> for a shopper.
        /// </summary>
        /// <param name="character">The character to get the <see cref="IMap"/> for.</param>
        /// <returns>
        /// The <see cref="IMap"/> for the <paramref name="character"/>.
        /// </returns>
        protected override IMap GetCharacterMap(User character)
        {
            return character.Map;
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="IMap"/> for a shop owner.
        /// </summary>
        /// <param name="character">The character to get the <see cref="IMap"/> for.</param>
        /// <returns>
        /// The <see cref="IMap"/> for the <paramref name="character"/>.
        /// </returns>
        protected override IMap GetCharacterMap(Character character)
        {
            return character.Map;
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="IShop{TShopItem}"/> for a
        /// shop owner.
        /// </summary>
        /// <param name="shopkeeper">The shop owner to get the <see cref="IShop{TShopItem}"/> for.</param>
        /// <returns>
        /// The <paramref name="shopkeeper"/>'s <see cref="IShop{TShopItem}"/>.
        /// </returns>
        protected override IShop<ShopItem> GetCharacterShop(Character shopkeeper)
        {
            return shopkeeper.Shop;
        }

        /// <summary>
        /// When overridden in the derived class, makes the <paramref name="character"/> try to purchase
        /// <paramref name="amount"/> of the <paramref name="shopItem"/> from the shop.
        /// </summary>
        /// <param name="character">The character doing the buying.</param>
        /// <param name="shopItem">The item to purchase.</param>
        /// <param name="amount">The amount of the <paramref name="shopItem"/> to purchase.</param>
        /// <returns>
        /// True if the purchase was successful; otherwise false.
        /// </returns>
        protected override bool HandleBuyShopItem(User character, ShopItem shopItem, byte amount)
        {
            return character.TryBuyItem(shopItem.ItemTemplate, amount);
        }

        /// <summary>
        /// When overridden in the derived class, makes the <paramref name="character"/> try to sell
        /// <paramref name="amount"/> of their inventory at the given <paramref name="slot"/> to
        /// the <paramref name="shop"/>.
        /// </summary>
        /// <param name="character">The character doing the selling.</param>
        /// <param name="slot">The inventory slot to sell from.</param>
        /// <param name="amount">The amount of the inventory item to sell.</param>
        /// <param name="shop">The shop to sell to.</param>
        /// <returns>
        /// True if the sell was successful; otherwise false.
        /// </returns>
        protected override bool HandleSellInventoryItem(User character, InventorySlot slot, byte amount, IShop<ShopItem> shop)
        {
            return character.TrySellInventoryItem(slot, amount, ShoppingAt);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the <paramref name="character"/> is close enough to the
        /// <paramref name="shopKeeper"/> to buy and sell to and from their shop.
        /// </summary>
        /// <param name="character">The character doing the shopping.</param>
        /// <param name="shopKeeper">The owner of the shop.</param>
        /// <returns>
        /// True if the <paramref name="character"/> is close enough to the <see cref="shopKeeper"/>
        /// to buy and sell from their shop; otherwise false.
        /// </returns>
        protected override bool IsValidDistance(User character, Character shopKeeper)
        {
            return GameData.IsValidDistanceToShop(character, shopKeeper);
        }

        /// <summary>
        /// When overridden in the derived class, notifies the <paramref name="character"/> that they have
        /// started shopping at the given <paramref name="shop"/>.
        /// </summary>
        /// <param name="character">The character doing the shopping.</param>
        /// <param name="shop">The shop that the <paramref name="character"/> is shopping at..</param>
        protected override void SendStartShopping(User character, IShop<ShopItem> shop)
        {
            using (var pw = ServerPacket.StartShopping(ShopOwner.MapEntityIndex, shop))
            {
                character.Send(pw, ServerMessageType.GUI);
            }
        }

        /// <summary>
        /// When overridden in the derived class, notifies the <paramref name="character"/> that they have
        /// stopped shopping.
        /// </summary>
        /// <param name="character">The character that stopped shopping.</param>
        protected override void SendStopShopping(User character)
        {
            using (var pw = ServerPacket.StopShopping())
            {
                character.Send(pw, ServerMessageType.GUI);
            }
        }
    }
}