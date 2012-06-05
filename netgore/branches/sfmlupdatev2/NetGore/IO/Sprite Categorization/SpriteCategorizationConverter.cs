using System.Linq;

namespace NetGore.IO
{
    public sealed class SpriteCategorizationConverter : SimpleExpandableTypeConverter<SpriteCategorization>
    {
        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="values"/> to the output
        /// type.
        /// </summary>
        /// <param name="parser">The <see cref="Parser"/> to use to parse the <paramref name="values"/>.</param>
        /// <param name="values">An array of strings containing the values to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="values"/>.</returns>
        protected override SpriteCategorization ConvertFromString(Parser parser, string[] values, out bool wasConverted)
        {
            wasConverted = true;

            switch (values.Length)
            {
                case 1:
                    return new SpriteCategorization(values[0]);

                case 2:
                    return new SpriteCategorization(values[0], values[1]);
            }

            wasConverted = false;
            return null;
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to a string, separating
        /// each element with the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <param name="separator">The string to use to separate the values.</param>
        /// <returns>The <paramref name="value"/> as a string.</returns>
        protected override string ConvertToString(SpriteCategorization value, string separator)
        {
            return value.ToString();
        }
    }
}