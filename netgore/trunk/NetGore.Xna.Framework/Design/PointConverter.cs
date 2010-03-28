using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Linq;
using System.Text;

namespace NetGore.Xna.Framework.Design
{
    public class PointConverter : MathTypeConverter
    {
        // Methods
        public PointConverter()
        {
            Type type = typeof(Point);
            base._propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("X")), new FieldPropertyDescriptor(type.GetField("Y")) }).Sort(new string[] { "X", "Y" });
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            int[] numArray = MathTypeConverter.ConvertToValues<int>(context, culture, value, 2, new string[] { "X", "Y" });
            if (numArray != null)
            {
                return new Point(numArray[0], numArray[1]);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is Point))
            {
                Point point2 = (Point)value;
                return MathTypeConverter.ConvertFromValues<int>(context, culture, new int[] { point2.X, point2.Y });
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is Point))
            {
                Point point = (Point)value;
                ConstructorInfo constructor = typeof(Point).GetConstructor(new Type[] { typeof(int), typeof(int) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { point.X, point.Y });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
            {
                throw new ArgumentNullException("propertyValues", FrameworkResources.NullNotAllowed);
            }
            return new Point((int)propertyValues["X"], (int)propertyValues["Y"]);
        }
    }


}
