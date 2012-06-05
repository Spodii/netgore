using System;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.Shops
{
    /// <summary>
    /// Contains the settings for shops.
    /// </summary>
    public class ShopSettings
    {
        /// <summary>
        /// The settings instance.
        /// </summary>
        static ShopSettings _instance;

        readonly byte _maxShopItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopSettings"/> class.
        /// </summary>
        /// <param name="maxShopItems">The max number of items in a single shop.</param>
        public ShopSettings(byte maxShopItems)
        {
            _maxShopItems = maxShopItems;
        }

        /// <summary>
        /// Gets the <see cref="ShopSettings"/> instance.
        /// </summary>
        public static ShopSettings Instance
        {
            get
            {
                Debug.Assert(_instance != null, "The settings instance should not be null!");
                return _instance;
            }
        }

        /// <summary>
        /// Gets the max number of items in a single shop.
        /// </summary>
        public byte MaxShopItems
        {
            get { return _maxShopItems; }
        }

        /// <summary>
        /// Initializes the <see cref="ShopSettings"/>. This must only be called once and called as early as possible.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is <c>null</c>.</exception>
        /// <exception cref="MethodAccessException">This method must be called once and only once.</exception>
        public static void Initialize(ShopSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }
    }
}