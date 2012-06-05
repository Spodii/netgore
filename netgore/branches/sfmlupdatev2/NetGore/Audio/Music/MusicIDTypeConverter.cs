using System.ComponentModel;
using System.Linq;

namespace NetGore.Audio
{
    /// <summary>
    /// A <see cref="MusicID"/> for the <see cref="TypeConverter"/> type.
    /// </summary>
    public class MusicIDTypeConverter : SimpleTypeConverter<MusicID>
    {
        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to the output
        /// type.
        /// </summary>
        /// <param name="value">A string containing the value to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="value"/>.</returns>
        protected override MusicID ConvertFromString(string value, out bool wasConverted)
        {
            int i;
            wasConverted = int.TryParse(value, out i);

            if (wasConverted)
                return new MusicID(i);

            return new MusicID(1);
        }
    }
}