using System.ComponentModel;
using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// A <see cref="TypeConverter"/> for the <see cref="SoundID"/> type.
    /// </summary>
    public class SoundIDTypeConverter : SimpleTypeConverter<SoundID>
    {
        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to the output
        /// type.
        /// </summary>
        /// <param name="value">A string containing the value to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="value"/>.</returns>
        protected override SoundID ConvertFromString(string value, out bool wasConverted)
        {
            int i;
            wasConverted = int.TryParse(value, out i);

            if (wasConverted)
                return new SoundID(i);

            return new SoundID(1);
        }
    }
}