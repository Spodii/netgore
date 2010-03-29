using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Globalization;

namespace Microsoft.Xna.Framework.Design
{
    public class Vector3Converter : MathTypeConverter
    {
       
        public Vector3Converter()
        {
            Type type = typeof(Vector3);
            base.propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("X")), new FieldPropertyDescriptor(type.GetField("Y")), new FieldPropertyDescriptor(type.GetField("Z")) }).Sort(new string[] { "X", "Y", "Z" });
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            float[] numArray = MathTypeConverter.ConvertToValues<float>(context, culture, value, 3, new string[] { "X", "Y", "Z" });
            if (numArray != null)
            {
                return new Vector3(numArray[0], numArray[1], numArray[2]);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is Vector3))
            {
                Vector3 vector2 = (Vector3)value;
                return MathTypeConverter.ConvertFromValues<float>(context, culture, new float[] { vector2.X, vector2.Y, vector2.Z });
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is Vector3))
            {
                Vector3 vector = (Vector3)value;
                ConstructorInfo constructor = typeof(Vector3).GetConstructor(new Type[] { typeof(float), typeof(float), typeof(float) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { vector.X, vector.Y, vector.Z });
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
            return new Vector3((float)propertyValues["X"], (float)propertyValues["Y"], (float)propertyValues["Z"]);
        }
    }


}
