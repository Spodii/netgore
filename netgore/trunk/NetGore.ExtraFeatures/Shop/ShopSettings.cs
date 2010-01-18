using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    public class ShopSettings
    {
        static ShopSettings _instance;

        readonly byte _maxShopItems;

        public ShopSettings(byte maxShopItems)
        {
            _maxShopItems = maxShopItems;
        }

        public byte MaxShopItems { get { return _maxShopItems; } }

        public static void Initialize(ShopSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }

        /// <summary>
        /// Gets the <see cref="ShopSettings"/> instance.
        /// </summary>
        public static ShopSettings Instance
        {
            get { return _instance; }
        }
    }
}
