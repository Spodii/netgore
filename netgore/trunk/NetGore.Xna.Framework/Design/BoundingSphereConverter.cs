using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Globalization;

namespace NetGore.Xna.Framework.Design
{
    public class BoundingSphereConverter : MathTypeConverter
    {
        // Methods
        public BoundingSphereConverter()
        {
            Type type = typeof(BoundingSphere);
            base._propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("Center")), new FieldPropertyDescriptor(type.GetField("Radius")) }).Sort(new string[] { "Center", "Radius" });
            base._supportStringConvert = false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is BoundingSphere))
            {
                BoundingSphere sphere = (BoundingSphere)value;
                ConstructorInfo constructor = typeof(BoundingSphere).GetConstructor(new Type[] { typeof(Vector3), typeof(float) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { sphere.Center, sphere.Radius });
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
            return new BoundingSphere((Vector3)propertyValues["Center"], (float)propertyValues["Radius"]);
        }
    }


}
