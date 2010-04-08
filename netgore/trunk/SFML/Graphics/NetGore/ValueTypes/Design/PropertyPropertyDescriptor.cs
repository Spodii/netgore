using System;

using System.Reflection;

namespace SFML.Graphics.Design
{
    class PropertyPropertyDescriptor : MemberPropertyDescriptor
    {
        // Fields
        readonly PropertyInfo _property;

        // Methods
        public PropertyPropertyDescriptor(PropertyInfo property) : base(property)
        {
            _property = property;
        }

        public override Type PropertyType
        {
            get { return _property.PropertyType; }
        }

        public override object GetValue(object component)
        {
            return _property.GetValue(component, null);
        }

        public override void SetValue(object component, object value)
        {
            _property.SetValue(component, value, null);
            OnValueChanged(component, EventArgs.Empty);
        }

        // Properties
    }
}