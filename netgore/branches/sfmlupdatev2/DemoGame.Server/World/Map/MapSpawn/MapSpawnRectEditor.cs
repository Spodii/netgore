using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using SFML.Graphics.Design;

namespace DemoGame.Server
{
    /// <summary>
    /// Editor for the <see cref="MapSpawnRect"/>.
    /// </summary>
    public class MapSpawnRectEditor : MathTypeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnRectEditor"/> class.
        /// </summary>
        public MapSpawnRectEditor()
        {
            propertyDescriptions = TypeDescriptor.GetProperties(typeof(MapSpawnRect));
            supportStringConvert = false;
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
        /// format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the
        /// current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is null. </exception>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="destinationType" /> is <c>null</c>.</exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if ((destinationType == typeof(InstanceDescriptor)) && (value is MapSpawnRect))
            {
                var rectangle = (MapSpawnRect)value;
                var constructor =
                    typeof(MapSpawnRect).GetConstructor(new Type[]
                    { typeof(ushort?), typeof(ushort?), typeof(ushort?), typeof(ushort?) });
                if (constructor != null)
                    return new InstanceDescriptor(constructor,
                        new object[] { rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height });
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Creates an instance of the type that this <see cref="T:System.ComponentModel.TypeConverter"/> is associated with,
        /// using the specified context, given a set of property values for the object.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
        /// format context.</param>
        /// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary"/> of new property values.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> representing the given <see cref="T:System.Collections.IDictionary"/>,
        /// or null if the object cannot be created. This method always returns null.
        /// </returns>
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new MapSpawnRect((ushort?)propertyValues["X"], (ushort?)propertyValues["Y"], (ushort?)propertyValues["Width"],
                (ushort?)propertyValues["Height"]);
        }
    }
}