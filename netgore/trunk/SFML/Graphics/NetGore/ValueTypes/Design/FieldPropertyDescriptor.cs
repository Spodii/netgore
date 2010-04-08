using System;

using System.Reflection;

namespace SFML.Graphics.Design
{
    class FieldPropertyDescriptor : MemberPropertyDescriptor
    {
        readonly FieldInfo _field;

        public FieldPropertyDescriptor(FieldInfo field) : base(field)
        {
            _field = field;
        }

        public override Type PropertyType
        {
            get { return _field.FieldType; }
        }

        public override object GetValue(object component)
        {
            return _field.GetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            _field.SetValue(component, value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }
}