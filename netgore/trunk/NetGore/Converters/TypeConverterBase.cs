using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NetGore
{
    public abstract class TypeConverterBase : ExpandableObjectConverter
    {
        protected abstract void AddDescriptors(List<SimpleMemberDescriptor> descriptors);

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
                                                                   Attribute[] attributes)
        {
            var descriptors = new List<SimpleMemberDescriptor>();
            AddDescriptors(descriptors);
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}