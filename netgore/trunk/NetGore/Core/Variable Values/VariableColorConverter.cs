using System.ComponentModel;
using System.Linq;
using SFML.Graphics;

namespace NetGore
{
    public class VariableColorConverter : IVariableValueConverter<Color>
    {
        static readonly TypeConverter _colorConverter = TypeDescriptor.GetConverter(typeof(Color));

        /// <summary>
        /// Converts the <paramref name="value"/> to string in a way that it can be parsed by <see cref="TryParse"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>
        /// The <paramref name="value"/> as a string.
        /// </returns>
        protected override string ConvertToString(Color value)
        {
            return "{" + _colorConverter.ConvertToString(value) + "}";
        }

        /// <summary>
        /// When overridden in the derived class, creates a new object of the given type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new object of the given type.</returns>
        protected override IVariableValue<Color> Create(Color value)
        {
            return new VariableColor(value);
        }

        /// <summary>
        /// When overridden in the derived class, creates a new object of the given type.
        /// </summary>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>A new object of the given type.</returns>
        protected override IVariableValue<Color> Create(Color min, Color max)
        {
            return new VariableColor(min, max);
        }

        /// <summary>
        /// When overridden in the derived class, tries to parse the given value from the string.
        /// </summary>
        /// <param name="parser">The <see cref="Parser"/> to use to parse the <paramref name="valueString"/>.</param>
        /// <param name="valueString">The string to parse.</param>
        /// <param name="value">The value parsed from the string.</param>
        /// <returns>
        /// True if parsed successfully; otherwise false.
        /// </returns>
        protected override bool TryParse(Parser parser, string valueString, out Color value)
        {
            value = (Color)_colorConverter.ConvertFromString(null, parser.CultureInfo, valueString);
            return true;
        }
    }
}