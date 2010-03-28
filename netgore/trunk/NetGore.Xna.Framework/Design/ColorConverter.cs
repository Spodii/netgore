using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Globalization;
using NetGore.Xna.Framework.Graphics;

namespace NetGore.Xna.Framework.Design
{
    public class ColorConverter : MathTypeConverter
    {
        // Methods
        public ColorConverter()
        {
            Type type = typeof(Color);
            base.propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new PropertyPropertyDescriptor(type.GetProperty("R")), new PropertyPropertyDescriptor(type.GetProperty("G")), new PropertyPropertyDescriptor(type.GetProperty("B")), new PropertyPropertyDescriptor(type.GetProperty("A")) }).Sort(new string[] { "R", "G", "B", "A" });
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            byte[] buffer = MathTypeConverter.ConvertToValues<byte>(context, culture, value, 4, new string[] { "R", "G", "B", "A" });
            if (buffer != null)
            {
                return new Color(buffer[0], buffer[1], buffer[2], buffer[3]);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is Color))
            {
                Color color2 = (Color)value;
                return MathTypeConverter.ConvertFromValues<byte>(context, culture, new byte[] { color2.R, color2.G, color2.B, color2.A });
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is Color))
            {
                Color color = (Color)value;
                ConstructorInfo constructor = typeof(Color).GetConstructor(new Type[] { typeof(byte), typeof(byte), typeof(byte), typeof(byte) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { color.R, color.G, color.B, color.A });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
            {
                throw new ArgumentNullException("propertyValues");
            }
            return new Color((byte)propertyValues["R"], (byte)propertyValues["G"], (byte)propertyValues["B"], (byte)propertyValues["A"]);
        }
    }
}
