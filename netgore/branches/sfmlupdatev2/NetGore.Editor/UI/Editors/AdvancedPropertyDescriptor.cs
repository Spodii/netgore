using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// Provides a more advanced and useful <see cref="PropertyDescriptor"/>.
    /// </summary>
    public sealed class AdvancedPropertyDescriptor : PropertyDescriptor
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Dictionary<Type, Func<object, string>> _extraTextProviders = new Dictionary<Type, Func<object, string>>();
        static readonly object _extraTextProvidersSync = new object();

        readonly PropertyDescriptor _parent;

        TypeConverter _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="descr">A <see cref="T:System.ComponentModel.MemberDescriptor"/> that contains the name of the property
        /// and its attributes.</param>
        /// <param name="component">The component.</param>
        public AdvancedPropertyDescriptor(PropertyDescriptor descr, object component) : base(descr)
        {
            _parent = descr;

            OriginalValue = GetValue(component);
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
        /// Gets the type converter for this property.
        /// </summary>
        /// <returns>A <see cref="T:System.ComponentModel.TypeConverter"/> that is used to convert the <see cref="T:System.Type"/>
        /// of this property.</returns>
        public override TypeConverter Converter
        {
            get { return _converter ?? (_converter = TypeConverterWrapper.Create(ForceTypeConverter ?? base.Converter)); }
        }

        /// <summary>
        /// Gets or sets the <see cref="UITypeEditor"/> to force this property to use. If null, the default
        /// <see cref="UITypeEditor"/> will be used.
        /// </summary>
        public UITypeEditor ForceEditor { get; set; }

        /// <summary>
        /// Gets or sets if this property will be forced to be read-only.
        /// </summary>
        public bool ForceReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TypeConverter"/> to force this property to use. If null, the default
        /// <see cref="TypeConverter"/> will be used.
        /// </summary>
        public TypeConverter ForceTypeConverter { get; set; }

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
        /// Gets or sets the original value of this property.
        /// </summary>
        public object OriginalValue { get; set; }

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
        /// Gets an editor of the specified type.
        /// </summary>
        /// <param name="editorBaseType">The base type of editor, which is used to differentiate between multiple
        /// editors that a property supports.</param>
        /// <returns>
        /// An instance of the requested editor type, or null if an editor cannot be found.
        /// </returns>
        public override object GetEditor(Type editorBaseType)
        {
            return ForceEditor ?? base.GetEditor(editorBaseType);
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
            SetValue(component, OriginalValue);
        }

        /// <summary>
        /// Sets the <see cref="Func{T, U}"/> used to get the extra text for the type <typeparamref name="T"/>. Any
        /// existing provider will be overwritten.
        /// </summary>
        /// <typeparam name="T">The type to add the extra text for.</typeparam>
        /// <param name="provider">The <see cref="Func{T, U}"/> to get the extra text string. If null, then
        /// any existing provider for type <typeparamref name="T"/> will be removed.</param>
        public static void SetExtraTextProvider<T>(Func<T, string> provider)
        {
            lock (_extraTextProvidersSync)
            {
                if (provider == null)
                {
                    // Remove
                    _extraTextProviders.Remove(typeof(T));
                }
                else
                {
                    // Create a little wrapper since we can't implicitly cast Func<T, string> to Func<object, string>
                    Func<object, string> providerWithObj = x => provider((T)x);

                    // Add
                    if (_extraTextProviders.ContainsKey(typeof(T)))
                        _extraTextProviders[typeof(T)] = providerWithObj;
                    else
                        _extraTextProviders.Add(typeof(T), providerWithObj);
                }
            }
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
            var currentValue = GetValue(component);
            return !Equals(currentValue, OriginalValue);
        }

        /// <summary>
        /// A wrapper for the <see cref="TypeConverter"/> used by the <see cref="AdvancedPropertyDescriptor"/>
        /// to bind the extra text information stuff.
        /// </summary>
        class TypeConverterWrapper : TypeConverter
        {
            static readonly ThreadSafeHashCache<TypeConverter, TypeConverterWrapper> _typeConverterCache =
                new ThreadSafeHashCache<TypeConverter, TypeConverterWrapper>(x => new TypeConverterWrapper(x));

            readonly TypeConverter _baseConverter;

            /// <summary>
            /// Initializes a new instance of the <see cref="TypeConverterWrapper"/> class.
            /// </summary>
            /// <param name="baseConverter">The base converter.</param>
            TypeConverterWrapper(TypeConverter baseConverter)
            {
                _baseConverter = baseConverter;
            }

            /// <summary>
            /// Returns whether this converter can convert an object of the given type to the type of this
            /// converter, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
            /// that provides a format context.</param>
            /// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want
            /// to convert from.</param>
            /// <returns>
            /// true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return _baseConverter.CanConvertFrom(context, sourceType);
            }

            /// <summary>
            /// Returns whether this converter can convert the object to the specified type, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
            /// <param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to.</param>
            /// <returns>
            /// true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return _baseConverter.CanConvertTo(context, destinationType);
            }

            /// <summary>
            /// Converts the given object to the type of this converter, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that
            /// provides a format context.</param>
            /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the
            /// current culture.</param>
            /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
            /// <returns>
            /// An <see cref="T:System.Object"/> that represents the converted value.
            /// </returns>
            /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return _baseConverter.ConvertFrom(context, culture, value);
            }

            /// <summary>
            /// Converts the given value object to the specified type, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides
            /// a format context.</param>
            /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the
            /// current culture is assumed.</param>
            /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
            /// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/>
            /// parameter to.</param>
            /// <returns>
            /// An <see cref="T:System.Object"/> that represents the converted value.
            /// </returns>
            /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/>
            /// parameter is null. </exception>
            /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                             Type destinationType)
            {
                // Get the original text
                var ret = _baseConverter.ConvertTo(context, culture, value, destinationType);

                string extra = null;

                // Check if we will be showing the extra text
                if (value != null && destinationType == typeof(string) && context.ShowExtendedText())
                {
                    // Get the extra text provider func
                    Func<object, string> extraTextProvider;
                    lock (_extraTextProvidersSync)
                    {
                        _extraTextProviders.TryGetValue(value.GetType(), out extraTextProvider);
                    }

                    // Get the extra text if a provider was found
                    if (extraTextProvider != null)
                    {
                        try
                        {
                            extra = extraTextProvider(value);
                        }
                        catch (Exception ex)
                        {
                            // When there is an exception when trying to get the extra text, do not let it annoy the
                            // end-user since its just, well, extra text. We will be fine without it. However, show it
                            // in the logs and while Debug is defined so we can still debug it.
                            const string errmsg = "Failed to acquire the extra text for type `{0}` on instance `{1}`. Reason: {2}";
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, value.GetType(), value, ex);
                            Debug.Fail(string.Format(errmsg, value.GetType(), value, ex));

                            extra = null;
                        }
                    }
                }

                // Append the extra text if we have it
                if (!string.IsNullOrEmpty(extra))
                {
                    // Make sure the ret string is not null before appending to it
                    if (ret == null)
                        ret = string.Empty;

                    ret += " [" + extra + "]";
                }

                return ret;
            }

            /// <summary>
            /// Gets a <see cref="TypeConverterWrapper"/> for a <see cref="TypeConverter"/>.
            /// </summary>
            /// <param name="baseConverter">The base converter.</param>
            /// <returns></returns>
            public static TypeConverterWrapper Create(TypeConverter baseConverter)
            {
                return _typeConverterCache[baseConverter];
            }

            /// <summary>
            /// Creates an instance of the type that this <see cref="T:System.ComponentModel.TypeConverter"/> is
            /// associated with, using the specified context, given a set of property values for the object.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
            /// that provides a format context.</param>
            /// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary"/> of new property values.</param>
            /// <returns>
            /// An <see cref="T:System.Object"/> representing the given <see cref="T:System.Collections.IDictionary"/>,
            /// or null if the object cannot be created. This method always returns null.
            /// </returns>
            public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
            {
                return _baseConverter.CreateInstance(context, propertyValues);
            }

            /// <summary>
            /// Returns whether changing a value on this object requires a call to
            /// <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/> to create a
            /// new value, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
            /// format context.</param>
            /// <returns>
            /// true if changing a property on this object requires a call to
            /// <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/>
            /// to create a new value; otherwise, false.
            /// </returns>
            public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
            {
                return _baseConverter.GetCreateInstanceSupported(context);
            }

            /// <summary>
            /// Returns a collection of properties for the type of array specified by the value parameter, using
            /// the specified context and attributes.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that
            /// provides a format context.</param>
            /// <param name="value">An <see cref="T:System.Object"/> that specifies the type of array for which
            /// to get properties.</param>
            /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
            /// <returns>
            /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> with the properties that are
            /// exposed for this data type, or null if there are no properties.
            /// </returns>
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
                                                                       Attribute[] attributes)
            {
                return _baseConverter.GetProperties(context, value, attributes);
            }

            /// <summary>
            /// Returns whether this object supports properties, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
            /// format context.</param>
            /// <returns>
            /// true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"/> should be
            /// called to find the properties of this object; otherwise, false.
            /// </returns>
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return _baseConverter.GetPropertiesSupported(context);
            }

            /// <summary>
            /// Returns a collection of standard values for the data type this type converter is designed for when
            /// provided with a format context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides
            /// a format context that can be used to extract additional information about the environment from which
            /// this converter is invoked. This parameter or properties of this parameter can be null.</param>
            /// <returns>
            /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/> that holds a standard
            /// set of valid values, or null if the data type does not support a standard set of values.
            /// </returns>
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return _baseConverter.GetStandardValues(context);
            }

            /// <summary>
            /// Returns whether the collection of standard values returned from
            /// <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> is an exclusive list of possible values,
            /// using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/>
            /// that provides a format context.</param>
            /// <returns>
            /// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/>
            /// returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> is an exhaustive
            /// list of possible values; false if other values are possible.
            /// </returns>
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return _baseConverter.GetStandardValuesExclusive(context);
            }

            /// <summary>
            /// Returns whether this object supports a standard set of values that can be picked from a list, using the
            /// specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a
            /// format context.</param>
            /// <returns>
            /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> should be called to find
            /// a common set of values the object supports; otherwise, false.
            /// </returns>
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return _baseConverter.GetStandardValuesSupported(context);
            }

            /// <summary>
            /// Returns whether the given value object is valid for this type and for the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that
            /// provides a format context.</param>
            /// <param name="value">The <see cref="T:System.Object"/> to test for validity.</param>
            /// <returns>
            /// true if the specified value is valid for this object; otherwise, false.
            /// </returns>
            public override bool IsValid(ITypeDescriptorContext context, object value)
            {
                return _baseConverter.IsValid(context, value);
            }
        }
    }
}