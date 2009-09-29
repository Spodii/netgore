using System.Linq;
using NetGore.IO;

namespace NetGore.RPGComponents
{
    /// <summary>
    /// Extension methods for the <see cref="ShopItemIndex"/>.
    /// </summary>
    public static class ShopItemIndexExtensions
    {
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