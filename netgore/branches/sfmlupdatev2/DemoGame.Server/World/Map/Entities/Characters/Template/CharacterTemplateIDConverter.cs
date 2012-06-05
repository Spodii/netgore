using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Converter for the CharacterTemplateID.
    /// </summary>
    public class CharacterTemplateIDConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. 
        ///                 </param><param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from. 
        ///                 </param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string) || sourceType == typeof(int))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. 
        ///                 </param><param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to. 
        ///                 </param>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return false;
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed.</exception>
        /// <exception cref="InvalidCastException">The <paramref name="value"/> could not be converted to
        /// type <see cref="CharacterTemplateID"/>.</exception>
        /// <exception cref="ArgumentException">No <see cref="CharacterTemplate"/> exists for the specified
        /// <paramref name="value"/>.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    var id = Parser.Current.ParseCharacterTemplateID((string)value);
                    if (!id.TemplateExists())
                    {
                        const string errmsg = "No CharacterTemplate with ID `{0}`.";
                        throw new ArgumentException(string.Format(errmsg, id));
                    }
                    return id;
                }
                catch (Exception ex)
                {
                    throw new InvalidCastException(ex.ToString(), ex);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. 
        ///                 </param><param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed. 
        ///                 </param><param name="value">The <see cref="T:System.Object"/> to convert. 
        ///                 </param><param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to. 
        ///                 </param><exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is null. 
        ///                 </exception><exception cref="T:System.NotSupportedException">The conversion cannot be performed. 
        ///                 </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return value.ToString();

            return false;
        }
    }
}