using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics
{
    public class GrhConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter,
        /// using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
        /// format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert
        /// from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return false;
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
        /// format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to
        /// convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return false;
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
        /// format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the
        /// current culture.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The conversion cannot be performed.
        /// </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                var grh = GetPropertyValue<Grh>(context);
                var grhData = TryGetGrhData(s);
                if (grh != null && grhData != null)
                    grh.SetGrh(grhData);
                return grh;
            }

            return null;
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
        /// format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current
        /// culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/>
        /// parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="destinationType"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The conversion cannot be performed.
        /// </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var grh = value as Grh;
                if (grh == null || grh.GrhData == null)
                    return string.Empty;

                return grh.GrhData.Categorization.ToString();
            }

            return null;
        }

        static T GetPropertyValue<T>(ITypeDescriptorContext context) where T : class
        {
            var descriptor = context.PropertyDescriptor;
            var propertyName = descriptor.Name;
            var property = descriptor.ComponentType.GetProperty(propertyName);
            var value = property.GetValue(context.Instance, null);
            return value as T;
        }

        /// <summary>
        /// Returns whether the given value object is valid for this type and for the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
        /// that provides a format context.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to test for validity.</param>
        /// <returns>
        /// true if the specified value is valid for this object; otherwise, false.
        /// </returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            var s = value as string;
            if (!string.IsNullOrEmpty(s))
            {
                var grhData = TryGetGrhData(s);
                return grhData != null;
            }

            return false;
        }

        static GrhData TryGetGrhData(string categoryAndTitle)
        {
            GrhData grhData = null;

            try
            {
                grhData = GrhInfo.GetData(new SpriteCategorization(categoryAndTitle));
            }
            catch (ArgumentNullException)
            {
                // Ignore invalid argument error
            }

            return grhData;
        }
    }
}