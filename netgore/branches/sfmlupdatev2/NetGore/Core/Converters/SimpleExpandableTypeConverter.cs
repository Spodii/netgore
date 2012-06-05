using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetGore
{
    /// <summary>
    /// Wrapper for creating a <see cref="ExpandableObjectConverter"/> with minimal effort.
    /// </summary>
    /// <typeparam name="T">The type being converted.</typeparam>
    public abstract class SimpleExpandableTypeConverter<T> : ExpandableObjectConverter
    {
        static readonly Regex _groupingRegex = new Regex(@"\{(?<value>.+?)\}",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter,
        /// using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides
        /// a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to
        /// convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
        /// that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type"/> that represents the type
        /// you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(T))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides
        /// a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the
        /// current culture.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string s;
            if ((s = value as string) != null)
            {
                string[] values;

                // Check if we have explicit grouping defined by using {} brackets
                var matches = _groupingRegex.Matches(s);
                if (matches.Count > 0)
                {
                    // Grab the group values
                    values = new string[matches.Count];
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = matches[i].Groups["value"].Value;
                    }
                }
                else
                {
                    // No grouping so just separate the values by the separator
                    values = s.Split(new string[] { culture.TextInfo.ListSeparator }, StringSplitOptions.RemoveEmptyEntries);
                }

                if (value == null)
                    value = new string[0];

                // Convert
                var parser = Parser.FromCulture(culture);

                bool wasConverted;
                var converted = ConvertFromString(parser, values, out wasConverted);

                if (wasConverted)
                    return converted;
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="values"/> to the output
        /// type.
        /// </summary>
        /// <param name="parser">The <see cref="Parser"/> to use to parse the <paramref name="values"/>.</param>
        /// <param name="values">An array of strings containing the values to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="values"/>.</returns>
        protected abstract T ConvertFromString(Parser parser, string[] values, out bool wasConverted);

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture
        /// information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides
        /// a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the
        /// current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/>
        /// parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter
        /// is null.</exception>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed.</exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is T)
            {
                var valueAsT = (T)value;
                return ConvertToString(valueAsT, culture.TextInfo.ListSeparator + " ");
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to a string, separating
        /// each element with the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <param name="separator">The string to use to separate the values.</param>
        /// <returns>The <paramref name="value"/> as a string.</returns>
        protected abstract string ConvertToString(T value, string separator);
    }
}