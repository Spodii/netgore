using System;
using System.ComponentModel;
using System.Text;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Provides a more advanced and useful <see cref="PropertyDescriptor"/>.
    /// </summary>
    public sealed class AdvancedPropertyDescriptor : PropertyDescriptor
    {
        readonly PropertyDescriptor _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="descr">A <see cref="T:System.ComponentModel.MemberDescriptor"/> that contains the name of the property
        /// and its attributes.</param>
        /// <param name="component">The component.</param>
        public AdvancedPropertyDescriptor(PropertyDescriptor descr, object component)
            : base(descr)
        {
            _parent = descr;

            OriginalValue = GetValue(component);
        }

        /// <summary>
        /// Gets or sets the original value of this property.
        /// </summary>
        public object OriginalValue { get; set; }

        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object changes its value.
        /// </summary>
        /// <returns>
        /// true if resetting the component changes its value; otherwise, false.
        /// </returns>
        /// <param name="component">The component to test for reset capability. </param>
        public override bool CanResetValue(object component)
        {
            return _parent.CanResetValue(component);
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <returns>
        /// The value of a property for a given component.
        /// </returns>
        /// <param name="component">The component with the property for which to retrieve the value. </param>
        public override object GetValue(object component)
        {
            return _parent.GetValue(component);
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value. </param>
        public override void ResetValue(object component)
        {
            _parent.ResetValue(component);
        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value)
        {
            _parent.SetValue(component, value);
        }

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs
        /// to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>
        /// true if the property should be persisted; otherwise, false.
        /// </returns>
        public override bool ShouldSerializeValue(object component)
        {
            return !Equals(GetValue(component), OriginalValue);
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of component this property is bound to
        /// When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"/> or
        /// <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"/>
        /// methods are invoked, the object specified might be an instance of this type.
        /// </returns>
        public override Type ComponentType
        {
            get { return _parent.ComponentType; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <returns>
        /// true if the property is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly
        {
            get { return ForceReadOnly || _parent.IsReadOnly; }
        }

        /// <summary>
        /// Gets or sets if this property will be forced to be read-only.
        /// </summary>
        public bool ForceReadOnly { get; set; }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the property.
        /// </returns>
        public override Type PropertyType
        {
            get { return _parent.PropertyType; }
        }
    }
}
