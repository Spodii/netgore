using System.ComponentModel;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// A <see cref="TypeConverter"/> for the <see cref="InventorySlot"/> type.
    /// </summary>
    public class InventorySlotTypeConverter : SimpleTypeConverter<InventorySlot>
    {
        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to the output
        /// type.
        /// </summary>
        /// <param name="value">A string containing the value to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="value"/>.</returns>
        protected override InventorySlot ConvertFromString(string value, out bool wasConverted)
        {
            int i;
            wasConverted = int.TryParse(value, out i);

            if (wasConverted)
                return new InventorySlot(i);

            return new InventorySlot(1);
        }
    }
}