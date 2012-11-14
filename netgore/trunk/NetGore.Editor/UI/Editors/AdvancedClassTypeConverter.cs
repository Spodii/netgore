using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// Provides access to the <see cref="AdvancedPropertyDescriptor"/> for a class's <see cref="TypeConverter"/>.
    /// This class is primarily intended for improving the experience when working with types in the
    /// <see cref="PropertyGrid"/> control.
    /// </summary>
    public class AdvancedClassTypeConverter : TypeConverter
    {
        static readonly Dictionary<Type, List<KeyValuePair<string, UITypeEditor>>> _forcedEditor =
            new Dictionary<Type, List<KeyValuePair<string, UITypeEditor>>>();

        static readonly object _forcedEditorSync = new object();
        static readonly Dictionary<Type, List<string>> _forcedReadOnly = new Dictionary<Type, List<string>>();
        static readonly object _forcedReadOnlySync = new object();

        static readonly Dictionary<Type, List<KeyValuePair<string, TypeConverter>>> _forcedTypeConverter =
            new Dictionary<Type, List<KeyValuePair<string, TypeConverter>>>();

        static readonly object _forcedTypeConverterSync = new object();

        /// <summary>
        /// Gets the <see cref="StringComparer"/> to use for comparing property names.
        /// </summary>
        static readonly StringComparer _propSC = StringComparer.Ordinal;

        /// <summary>
        /// Sets up multiple class <see cref="Type"/>s to use this <see cref="TypeConverter"/>.
        /// </summary>
        /// <param name="types">The <see cref="Type"/>s to set up to use this <see cref="TypeConverter"/>.</param>
        public static void AddTypes(params Type[] types)
        {
            foreach (var t in types)
            {
                TypeDescriptor.AddAttributes(t, new TypeConverterAttribute(typeof(AdvancedClassTypeConverter)));
            }
        }

        /// <summary>
        /// Returns a collection of properties for the type of array specified by the value parameter, using the specified
        /// context and attributes.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides
        /// a format context.</param>
        /// <param name="value">An <see cref="T:System.Object"/> that specifies the type of array for which to
        /// get properties.</param>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> with the properties that are exposed
        /// for this data type, or null if there are no properties.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
                                                                   Attribute[] attributes)
        {
            var ret = new List<AdvancedPropertyDescriptor>();
            var valueType = value.GetType();

            // Get the properties to force as read-only for this type
            List<string> fro;
            lock (_forcedReadOnlySync)
            {
                _forcedReadOnly.TryGetValue(valueType, out fro);
            }

            // Get the properties to force to use a specific type converter
            List<KeyValuePair<string, TypeConverter>> ftc;
            lock (_forcedTypeConverterSync)
            {
                _forcedTypeConverter.TryGetValue(valueType, out ftc);
            }

            // Get the properties to force to use a specific editor
            List<KeyValuePair<string, UITypeEditor>> fe;
            lock (_forcedEditorSync)
            {
                _forcedEditor.TryGetValue(valueType, out fe);
            }

            // Loop through all the properties to use our custom AdvancedPropertyDescriptor instead
            foreach (var p in TypeDescriptor.GetProperties(value, attributes).OfType<PropertyDescriptor>())
            {
                var pName = p.Name;
                var propertyDescriptor = new AdvancedPropertyDescriptor(p, value);

                // Apply the forced values
                if (fro != null && fro.Contains(propertyDescriptor.Name))
                    propertyDescriptor.ForceReadOnly = true;

                if (ftc != null)
                    propertyDescriptor.ForceTypeConverter = ftc.FirstOrDefault(x => _propSC.Equals(pName, x.Key)).Value;

                if (fe != null)
                    propertyDescriptor.ForceEditor = fe.FirstOrDefault(x => _propSC.Equals(pName, x.Key)).Value;

                // Add to the return list
                ret.Add(propertyDescriptor);
            }

            return new PropertyDescriptorCollection(ret.Cast<PropertyDescriptor>().ToArray());
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
            return true;
        }

        /// <summary>
        /// Sets a property of a certain <see cref="Type"/> to be forced to use a certain <see cref="UITypeEditor"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the class to set the properties on.</param>
        /// <param name="editors">The case-sensitive names of the properties and the <see cref="TypeConverter"/>
        /// instance to use.</param>
        public static void SetForceEditor(Type type, params KeyValuePair<string, UITypeEditor>[] editors)
        {
            lock (_forcedEditorSync)
            {
                List<KeyValuePair<string, UITypeEditor>> l;
                if (!_forcedEditor.TryGetValue(type, out l))
                {
                    l = new List<KeyValuePair<string, UITypeEditor>>();
                    _forcedEditor.Add(type, l);
                }

                foreach (var n in editors)
                {
                    var name = n.Key;

                    // Remove existing values for the property with this name (which should be either 0 or 1)
                    l.RemoveAll(x => _propSC.Equals(name, x.Key));

                    // Add the new value
                    l.Add(n);
                }
            }
        }

        /// <summary>
        /// Adds properties to be set as readonly for a given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the class to set the properties on.</param>
        /// <param name="propertyNames">The case-sensitive names of the properties to set as read-only.</param>
        public static void SetForceReadOnlyProperties(Type type, params string[] propertyNames)
        {
            lock (_forcedReadOnlySync)
            {
                List<string> l;
                if (!_forcedReadOnly.TryGetValue(type, out l))
                {
                    l = new List<string>();
                    _forcedReadOnly.Add(type, l);
                }

                foreach (var pn in propertyNames)
                {
                    if (!l.Contains(pn, _propSC))
                        l.Add(pn);
                }
            }
        }

        /// <summary>
        /// Sets a property of a certain <see cref="Type"/> to be forced to use a certain <see cref="TypeConverter"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the class to set the properties on.</param>
        /// <param name="typeConverters">The case-sensitive names of the properties and the <see cref="TypeConverter"/>
        /// instance to use.</param>
        public static void SetForceTypeConverter(Type type, params KeyValuePair<string, TypeConverter>[] typeConverters)
        {
            lock (_forcedTypeConverterSync)
            {
                List<KeyValuePair<string, TypeConverter>> l;
                if (!_forcedTypeConverter.TryGetValue(type, out l))
                {
                    l = new List<KeyValuePair<string, TypeConverter>>();
                    _forcedTypeConverter.Add(type, l);
                }

                foreach (var n in typeConverters)
                {
                    var name = n.Key;

                    // Remove existing values for the property with this name (which should be either 0 or 1)
                    l.RemoveAll(x => _propSC.Equals(name, x.Key));

                    // Add the new value
                    l.Add(n);
                }
            }
        }
    }
}