using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetGore.Xna.Framework.Design
{
    internal class PropertyPropertyDescriptor : MemberPropertyDescriptor
    {
        // Fields
        private PropertyInfo _property;

        // Methods
        public PropertyPropertyDescriptor(PropertyInfo property)
            : base(property)
        {
            this._property = property;
        }

        public override object GetValue(object component)
        {
            return this._property.GetValue(component, null);
        }

        public override void SetValue(object component, object value)
        {
            this._property.SetValue(component, value, null);
            this.OnValueChanged(component, EventArgs.Empty);
        }

        // Properties
        public override Type PropertyType
        {
            get
            {
                return this._property.PropertyType;
            }
        }
    }


}
