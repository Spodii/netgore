using System;
using System.Linq;

namespace NetGore.IO
{
    public sealed class SpriteCategoryConverter : SimpleTypeConverter<SpriteCategory>
    {
        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to the output
        /// type.
        /// </summary>
        /// <param name="value">A string containing the value to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="value"/>.</returns>
        protected override SpriteCategory ConvertFromString(string value, out bool wasConverted)
        {
            try
            {
                wasConverted = true;
                return new SpriteCategory(value);
            }
            catch (ArgumentException)
            {
                wasConverted = false;
                return null;
            }
        }
    }
}