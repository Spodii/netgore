using System.ComponentModel;
using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// A <see cref="TypeConverter"/> for the <see cref="ItemID"/> type.
    /// </summary>
    public class ItemIDTypeConverter : SimpleTypeConverter<ItemID>
    {
        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to the output
        /// type.
        /// </summary>
        /// <param name="value">A string containing the value to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="value"/>.</returns>
        protected override ItemID ConvertFromString(string value, out bool wasConverted)
        {
            int i;
            wasConverted = int.TryParse(value, out i);

            if (wasConverted)
                return new ItemID(i);

            return new ItemID(1);
        }
    }
}