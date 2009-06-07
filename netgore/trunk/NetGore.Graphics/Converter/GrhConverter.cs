using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NetGore.Graphics
{
    public class GrhConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return false;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return false;
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            string s = value as string;
            if (!string.IsNullOrEmpty(s))
            {
                GrhData grhData = TryGetGrhData(s);
                return grhData != null;
            }

            return false;
        }

        static GrhData TryGetGrhData(string categoryAndTitle)
        {
            GrhData grhData = null;

            try
            {
                grhData = GrhInfo.GetData(categoryAndTitle);
            }
            catch (ArgumentNullException)
            {
                // Ignore invalid argument error
            }

            return grhData;
        }

        static T GetPropertyValue<T>(ITypeDescriptorContext context) where T : class
        {
            PropertyDescriptor descriptor = context.PropertyDescriptor;
            string propertyName = descriptor.Name;
            var property = descriptor.ComponentType.GetProperty(propertyName);
            object value = property.GetValue(context.Instance, null);
            return value as T;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string s = value as string;
            if (s != null)
            {
                Grh grh = GetPropertyValue<Grh>(context);
                GrhData grhData = TryGetGrhData(s);
                if (grh != null && grhData != null)
                    grh.SetGrh(grhData);
                return grh;
            }

            return null;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Grh grh = value as Grh;
                if (grh == null || grh.GrhData == null)
                    return string.Empty;
                return grh.GrhData.Category + "." + grh.GrhData.Title;
            }

            return null;
        }
    }
}
