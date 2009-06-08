using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NetGore
{
    public abstract class TypeConverterBase : ExpandableObjectConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        protected abstract void AddDescriptors(List<SimpleMemberDescriptor> descriptors);

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            List<SimpleMemberDescriptor> descriptors = new List<SimpleMemberDescriptor>();
            AddDescriptors(descriptors);
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }
    }
}
