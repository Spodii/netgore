using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    public sealed class ItemTypeHelper : EnumIOHelper<ItemType>
    {
        static readonly ItemTypeHelper _instance;

        /// <summary>
        /// Initializes the <see cref="ItemTypeHelper"/> class.
        /// </summary>
        static ItemTypeHelper()
        {
            _instance = new ItemTypeHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTypeHelper"/> class.
        /// </summary>
        ItemTypeHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="ItemTypeHelper"/> instance.
        /// </summary>
        public static ItemTypeHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="ItemType"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="ItemType"/>.</returns>
        protected override ItemType FromInt(int value)
        {
            return (ItemType)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="ItemType"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(ItemType value)
        {
            return (int)value;
        }
    }
}