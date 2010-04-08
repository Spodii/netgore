using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

using System.Reflection;

namespace SFML.Graphics.Design
{
    /// <summary>Provides a unified way of converting BoundingBox values to other types, as well as for accessing standard values and subproperties.</summary>
    public class BoundingBoxConverter : MathTypeConverter
    {
        /// <summary>Initializes a new instance of the BoundingBoxConverter class.</summary>
        public BoundingBoxConverter()
        {
            Type type = typeof(BoundingBox);
            base.propertyDescriptions =
                new PropertyDescriptorCollection(new PropertyDescriptor[]
                { new FieldPropertyDescriptor(type.GetField("Min")), new FieldPropertyDescriptor(type.GetField("Max")) }).Sort(
                    new string[] { "Min", "Max" });
            base.supportStringConvert = false;
        }

        /// <summary>Converts the given object to the type of this converter, using the specified context and culture information.</summary>
        /// <param name="context">The format context.</param>
        /// <param name="culture">The current culture.</param>
        /// <param name="value">The object to convert.</param>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>Converts the given value object to the specified type, using the specified context and culture information.</summary>
        /// <param name="context">The format context.</param>
        /// <param name="culture">The culture to use in the conversion.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="destinationType">The destination type.</param>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");
            if ((destinationType == typeof(InstanceDescriptor)) && (value is BoundingBox))
            {
                BoundingBox box = (BoundingBox)value;
                ConstructorInfo constructor = typeof(BoundingBox).GetConstructor(new Type[] { typeof(Vector3), typeof(Vector3) });
                if (constructor != null)
                    return new InstanceDescriptor(constructor, new object[] { box.Min, box.Max });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>Creates an instance of the type that this BoundingBoxConverter is associated with, using the specified context, given a set of property values for the object.</summary>
        /// <param name="context">The format context.</param>
        /// <param name="propertyValues">The new property values.</param>
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
                throw new ArgumentNullException("propertyValues", FrameworkResources.NullNotAllowed);
            return new BoundingBox((Vector3)propertyValues["Min"], (Vector3)propertyValues["Max"]);
        }
    }
}