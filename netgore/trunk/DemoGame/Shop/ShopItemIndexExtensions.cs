using System.Linq;
using NetGore.IO;

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