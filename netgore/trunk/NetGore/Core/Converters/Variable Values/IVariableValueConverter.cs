using System.Linq;

namespace NetGore
{
    /// <summary>
    /// A simple wrapper for a SimpleExpandableTypeConverter for a IVariableValue.
    /// </summary>
    /// <typeparam name="T">The internal value type of the IVariableValue.</typeparam>
    public abstract class IVariableValueConverter<T> : SimpleExpandableTypeConverter<IVariableValue<T>>
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
        protected override IVariableValue<T> ConvertFromString(Parser parser, string[] values, out bool wasConverted)
        {
            wasConverted = true;

            switch (values.Length)
            {
                case 1:
                    T value;
                    if (TryParse(parser, values[0], out value))
                        return Create(value);

                    break;

                case 2:
                    T min;
                    T max;

                    if (TryParse(parser, values[0], out min) && TryParse(parser, values[1], out max))
                        return Create(min, max);

                    break;
            }

            wasConverted = false;
            return default(IVariableValue<T>);
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to a string, separating
        /// each element with the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <param name="separator">The string to use to separate the values.</param>
        /// <returns>The <paramref name="value"/> as a string.</returns>
        protected override string ConvertToString(IVariableValue<T> value, string separator)
        {
            var minStr = ConvertToString(value.Min);
            var maxStr = ConvertToString(value.Max);
            return string.Format("{1}{0}{2}", separator, minStr, maxStr);
        }

        /// <summary>
        /// Converts the <paramref name="value"/> to string in a way that it can be parsed by <see cref="TryParse"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>The <paramref name="value"/> as a string.</returns>
        protected virtual string ConvertToString(T value)
        {
            return value.ToString();
        }

        /// <summary>
        /// When overridden in the derived class, creates a new object of the given type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new object of the given type.</returns>
        protected abstract IVariableValue<T> Create(T value);

        /// <summary>
        /// When overridden in the derived class, creates a new object of the given type.
        /// </summary>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>A new object of the given type.</returns>
        protected abstract IVariableValue<T> Create(T min, T max);

        /// <summary>
        /// When overridden in the derived class, tries to parse the given value from the string.
        /// </summary>
        /// <param name="parser">The <see cref="Parser"/> to use to parse the <paramref name="valueString"/>.</param>
        /// <param name="valueString">The string to parse.</param>
        /// <param name="value">The value parsed from the string.</param>
        /// <returns>True if parsed successfully; otherwise false.</returns>
        protected abstract bool TryParse(Parser parser, string valueString, out T value);
    }
}