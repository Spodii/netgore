using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Wrapper for creating a <see cref="TypeConverter"/> with minimal effort.
    /// </summary>
    /// <typeparam name="T">The type being converted.</typeparam>
    public abstract class SimpleTypeConverter<T> : TypeConverter
    {
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
            if (sourceType == typeof(T))
                return true;

            if (sourceType == typeof(string))
                return true;

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

            if (destinationType == typeof(string))
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
                // Convert
                bool wasConverted;
                var converted = default(T);
                try
                {
                    converted = ConvertFromString(s, out wasConverted);
                }
                catch (FormatException)
                {
                    wasConverted = false;
                }
                catch (ArgumentNullException)
                {
                    wasConverted = false;
                }
                catch (OverflowException)
                {
                    wasConverted = false;
                }

                if (wasConverted)
                    return converted;
                else
                    return base.ConvertFrom(context, culture, value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to the output
        /// type.
        /// </summary>
        /// <param name="value">A string containing the value to parse.</param>
        /// <param name="wasConverted">Contains true if the parsing was successful, or false if the
        /// parsing failed.</param>
        /// <returns>The object parsed from the <paramref name="value"/>.</returns>
        protected abstract T ConvertFromString(string value, out bool wasConverted);

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
                return ConvertToString((T)value);
            else if (destinationType == typeof(T))
                return (T)value;

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Converts the <paramref name="value"/> to a string. By default, this just calls
        /// <see cref="System.Object.ToString"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>The <paramref name="value"/> converted to a string.</returns>
        protected virtual string ConvertToString(T value)
        {
            return value.ToString();
        }
    }
}