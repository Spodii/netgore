using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="ShopItemIndex"/>.
    /// </summary>
    public static class ShopItemIndexExtensions
    {
        static readonly ShopSettings _shopSettings;

        /// <summary>
        /// Initializes the <see cref="ShopItemIndexExtensions"/> class.
        /// </summary>
        static ShopItemIndexExtensions()
        {
            _shopSettings = ShopSettings.Instance;
        }

        /// <summary>
        /// Checks if the given <see cref="ShopItemIndex"/> is in legal range.
        /// </summary>
        /// <param name="index">The ShopItemIndex.</param>
        /// <returns>True if the <paramref name="index"/> is in legal range; otherwise false.</returns>
        public static bool IsLegalValue(this ShopItemIndex index)
        {
            return index >= 0 && index < _shopSettings.MaxShopItems;
        }

        public static ShopItemIndex ReadShopItemIndex(this IValueReader reader, string name)
        {
            return new ShopItemIndex(reader.ReadByte(name));
        }

        public static void Write(this IValueWriter writer, string name, ShopItemIndex value)
        {
            writer.Write(name, value);
        }
    }
}