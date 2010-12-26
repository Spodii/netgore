using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.World;

namespace NetGore.Features.Shops
{
    /// <summary>
    /// Describes the current state of a shopper shopping at a shop.
    /// </summary>
    /// <typeparam name="TShopper">The type of shopper.</typeparam>
    /// <typeparam name="TShopOwner">The type of shop owner.</typeparam>
    /// <typeparam name="TShopItem">The type of shop item.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class CharacterShoppingState<TShopper, TShopOwner, TShopItem>
        where TShopper : DynamicEntity where TShopOwner : DynamicEntity
    {
        readonly TShopper _character;

        IMap _shopMap;
        TShopOwner _shopOwner;
        IShop<TShopItem> _shoppingAt;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterShoppingState{TShopper, TShopOwner, TShopItem}"/> class.
        /// </summary>
        /// <param name="character">The character doing the shopping.</param>
        /// <exception cref="ArgumentNullException"><paramref name="character" /> is <c>null</c>.</exception>
        protected CharacterShoppingState(TShopper character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
        }

        /// <summary>
        /// Gets the character doing the shopping.
        /// </summary>
        public TShopper Character
        {
            get { return _character; }
        }

        /// <summary>
        /// Gets the character that owns the shop that the
        /// <see cref="CharacterShoppingState{TShopper, TShopOwner, TShopItem}.Character"/> is shopping at.
        /// Will be null if not shopping.
        /// </summary>
        public TShopOwner ShopOwner
        {
            get { return _shopOwner; }
        }

        /// <summary>
        /// Gets the shop taht the
        /// <see cref="CharacterShoppingState{TShopper, TShopOwner, TShopItem}.Character"/> is shopping at.
        /// Will be null if not shopping.
        /// </summary>
        public IShop<TShopItem> ShoppingAt
        {
            get { return _shoppingAt; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="IMap"/> for a <typeparamref name="TShopper"/>.
        /// </summary>
        /// <param name="character">The character to get the <see cref="IMap"/> for.</param>
        /// <returns>The <see cref="IMap"/> for the <paramref name="character"/>.</returns>
        protected abstract IMap GetCharacterMap(TShopper character);

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="IMap"/> for a <typeparamref name="TShopOwner"/>.
        /// </summary>
        /// <param name="character">The character to get the <see cref="IMap"/> for.</param>
        /// <returns>The <see cref="IMap"/> for the <paramref name="character"/>.</returns>
        protected abstract IMap GetCharacterMap(TShopOwner character);

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="IShop{TShopItem}"/> for a
        /// <typeparamref name="TShopOwner"/>.
        /// </summary>
        /// <param name="shopkeeper">The shop owner to get the <see cref="IShop{TShopItem}"/> for.</param>
        /// <returns>The <paramref name="shopkeeper"/>'s <see cref="IShop{TShopItem}"/>.</returns>
        protected abstract IShop<TShopItem> GetCharacterShop(TShopOwner shopkeeper);

        /// <summary>
        /// When overridden in the derived class, makes the <paramref name="character"/> try to purchase
        /// <paramref name="amount"/> of the <paramref name="shopItem"/> from the shop.
        /// </summary>
        /// <param name="character">The character doing the buying.</param>
        /// <param name="shopItem">The item to purchase.</param>
        /// <param name="amount">The amount of the <paramref name="shopItem"/> to purchase.</param>
        /// <returns>True if the purchase was successful; otherwise false.</returns>
        protected abstract bool HandleBuyShopItem(TShopper character, TShopItem shopItem, byte amount);

        /// <summary>
        /// When overridden in the derived class, makes the <paramref name="character"/> try to sell
        /// <paramref name="amount"/> of their inventory at the given <paramref name="slot"/> to
        /// the <paramref name="shop"/>.
        /// </summary>
        /// <param name="character">The character doing the selling.</param>
        /// <param name="slot">The inventory slot to sell from.</param>
        /// <param name="amount">The amount of the inventory item to sell.</param>
        /// <param name="shop">The shop to sell to.</param>
        /// <returns>True if the sell was successful; otherwise false.</returns>
        protected abstract bool HandleSellInventoryItem(TShopper character, InventorySlot slot, byte amount, IShop<TShopItem> shop);

        /// <summary>
        /// When overridden in the derived class, gets if the <paramref name="character"/> is close enough to the
        /// <paramref name="shopKeeper"/> to buy and sell to and from their shop.
        /// </summary>
        /// <param name="character">The character doing the shopping.</param>
        /// <param name="shopKeeper">The owner of the shop.</param>
        /// <returns>True if the <paramref name="character"/> is close enough to the <see cref="shopKeeper"/>
        /// to buy and sell from their shop; otherwise false.</returns>
        protected abstract bool IsValidDistance(TShopper character, TShopOwner shopKeeper);

        /// <summary>
        /// When overridden in the derived class, notifies the <paramref name="character"/> that they have
        /// started shopping at the given <paramref name="shop"/>.
        /// </summary>
        /// <param name="character">The character doing the shopping.</param>
        /// <param name="shop">The shop that the <paramref name="character"/> is shopping at..</param>
        protected abstract void SendStartShopping(TShopper character, IShop<TShopItem> shop);

        /// <summary>
        /// When overridden in the derived class, notifies the <paramref name="character"/> that they have
        /// stopped shopping.
        /// </summary>
        /// <param name="character">The character that stopped shopping.</param>
        protected abstract void SendStopShopping(TShopper character);

        /// <summary>
        /// Attempts to purchase <paramref name="amount"/> items at the given <paramref name="slot"/> from the
        /// shop that the shopper is currently shopping at.
        /// </summary>
        /// <param name="slot">The index of the shop item to purchase.</param>
        /// <param name="amount">The amount of the item to purchase.</param>
        /// <returns>True if the purchase was successful; otherwise false.</returns>
        public bool TryPurchase(ShopItemIndex slot, byte amount)
        {
            ThreadAsserts.IsMainThread();

            if (!slot.IsLegalValue())
                return false;

            // Get the shop
            ValidateShop();
            if (_shoppingAt == null)
                return false;

            // Get and validate the item
            var shopItem = _shoppingAt.GetShopItem(slot);
            if (Equals(shopItem, null))
                return false;

            // Try to buy the item
            return HandleBuyShopItem(Character, shopItem, amount);
        }

        /// <summary>
        /// Attempts to sell <paramref name="amount"/> items from the given <paramref name="slot"/> to the
        /// shop that the shopper is currently shopping at.
        /// </summary>
        /// <param name="slot">The index of the inventory item to sell.</param>
        /// <param name="amount">The amount of the item to sell.</param>
        /// <returns>True if the sale was successful; otherwise false.</returns>
        public bool TrySellInventory(InventorySlot slot, byte amount)
        {
            ThreadAsserts.IsMainThread();

            ValidateShop();
            if (_shoppingAt == null)
                return false;

            // Make sure the shop buys stuff
            if (!_shoppingAt.CanBuy)
                return false;

            return HandleSellInventoryItem(Character, slot, amount, _shoppingAt);
        }

        /// <summary>
        /// Attempts to start shopping at a shop owned by the <paramref name="shopkeeper"/>.
        /// </summary>
        /// <param name="shopkeeper">The owner of the shop to try to start shopping at.</param>
        /// <returns>True if the shopping was successfully started at the <paramref name="shopkeeper"/>'s
        /// shop; otherwise false.</returns>
        public bool TryStartShopping(TShopOwner shopkeeper)
        {
            ThreadAsserts.IsMainThread();

            if (shopkeeper == null)
                return false;

            var shop = GetCharacterShop(shopkeeper);
            if (shop == null)
                return false;

            return TryStartShopping(shop, shopkeeper, GetCharacterMap(shopkeeper));
        }

        /// <summary>
        /// Attempts to start shopping at a shop owned by the <paramref name="shopkeeper"/>.
        /// </summary>
        /// <param name="shop">The shop to start shopping at.</param>
        /// <param name="shopkeeper">The owner of the shop to try to start shopping at.</param>
        /// <param name="entityMap">The map that the <paramref name="shopkeeper"/> is on.</param>
        /// <returns>
        /// True if the shopping was successfully started at the <paramref name="shopkeeper"/>'s
        /// shop; otherwise false.
        /// </returns>
        bool TryStartShopping(IShop<TShopItem> shop, TShopOwner shopkeeper, IMap entityMap)
        {
            ThreadAsserts.IsMainThread();

            if (shop == null || shopkeeper == null || entityMap == null)
                return false;

            if (GetCharacterMap(Character) != entityMap)
                return false;

            if (!IsValidDistance(Character, shopkeeper))
                return false;

            // If the User was already shopping somewhere else, stop that shopping
            if (_shoppingAt != null)
                SendStopShopping(Character);

            // Start the shopping
            _shoppingAt = shop;
            _shopOwner = shopkeeper;
            _shopMap = entityMap;

            SendStartShopping(Character, shop);

            return true;
        }

        /// <summary>
        /// Performs validation checks on the shop to ensure it is valid. If the shop is invalid,
        /// <see cref="ShoppingAt"/> and other values will be set to null.
        /// </summary>
        void ValidateShop()
        {
            ThreadAsserts.IsMainThread();

            // Check for a valid shop
            if (_shoppingAt == null || _shopOwner == null || _shopMap == null)
                return;

            // Check for a valid distance
            if (!IsValidDistance(Character, _shopOwner))
            {
                // Stop shopping
                SendStopShopping(Character);
                _shoppingAt = null;
                _shopOwner = null;
                _shopMap = null;
            }
        }
    }
}