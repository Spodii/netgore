using System.Linq;
using NetGore;
using NetGore.RPGComponents;

namespace NetGore.RPGComponents
{
    /// <summary>
    /// Represents the index of a ShopItem in a Shop.
    /// </summary>
    public struct ShopItemIndex
    {
        /// <summary>
        /// Represents the largest possible value of ShopItemIndex. This field is constant.
        /// </summary>
        public const byte MaxValue = byte.MaxValue;

        /// <summary>
        /// Represents the smallest possible value of ShopItemIndex. This field is constant.
        /// </summary>
        public const byte MinValue = byte.MinValue;

        readonly byte _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItemIndex"/> struct.
        /// </summary>
        /// <param name="id">The id.</param>
        public ShopItemIndex(byte id)
        {
            _value = id;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ShopItemIndex"/> to <see cref="System.Byte"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator byte(ShopItemIndex v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte"/> to <see cref="ShopItemIndex"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ShopItemIndex(byte v)
        {
            return new ShopItemIndex(v);
        }
    }
}