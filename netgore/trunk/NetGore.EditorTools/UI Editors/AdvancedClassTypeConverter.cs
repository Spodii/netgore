using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Provides access to the <see cref="AdvancedPropertyDescriptor"/> for a class's <see cref="TypeConverter"/>.
    /// </summary>
    public class AdvancedClassTypeConverter : TypeConverter
    {
        static readonly Dictionary<Type, List<string>> _forcedReadOnly = new Dictionary<Type, List<string>>();
        static readonly object _forcedReadOnlySync = new object();

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

            List<string> fro;
            lock (_forcedReadOnlySync)
            {
                _forcedReadOnly.TryGetValue(value.GetType(), out fro);
            }

            foreach (var p in TypeDescriptor.GetProperties(value.GetType()).OfType<PropertyDescriptor>())
            {
                var propertyDescriptor = new AdvancedPropertyDescriptor(p, value);

                if (fro != null && fro.Contains(propertyDescriptor.Name))
                    propertyDescriptor.ForceReadOnly = true;

                ret.Add(propertyDescriptor);
            }

            return new PropertyDescriptorCollection(ret.ToArray());
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
                    if (!l.Contains(pn))
                        l.Add(pn);
                }
            }
        }
    }
}