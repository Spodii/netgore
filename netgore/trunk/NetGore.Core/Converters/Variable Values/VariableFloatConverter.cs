using System.Linq;

namespace NetGore
{
    public class VariableFloatConverter : IVariableValueConverter<float>
    {
        /// <summary>
        /// When overridden in the derived class, creates a new object of the given type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new object of the given type.</returns>
        protected override IVariableValue<float> Create(float value)
        {
            return new VariableFloat(value);
        }

        /// <summary>
        /// When overridden in the derived class, creates a new object of the given type.
        /// </summary>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>A new object of the given type.</returns>
        protected override IVariableValue<float> Create(float min, float max)
        {
            return new VariableFloat(min, max);
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
        protected override bool TryParse(Parser parser, string valueString, out float value)
        {
            return parser.TryParse(valueString, out value);
        }
    }
}