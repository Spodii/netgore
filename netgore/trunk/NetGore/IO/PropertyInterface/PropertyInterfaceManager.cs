using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetGore.Collections;

namespace NetGore.IO
{
    public class PropertyInterface<TObj, T> : ThreadSafeHashCache<PropertyInfo, IPropertyInterface<TObj, T>>
    {
        static readonly PropertyInterface<TObj, T> _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInterface{TObj, T}"/> class.
        /// </summary>
        static PropertyInterface()
        {
            _instance = new PropertyInterface<TObj, T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInterface{TObj, T}"/> class.
        /// </summary>
        PropertyInterface() : base(x => new ExpressionPropertyInterface(x))
        {
        }

        /// <summary>
        /// Gets the <see cref="PropertyInterface{T,U}"/> instance.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static PropertyInterface<TObj, T> Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets a <see cref="IPropertyInterface{T,U}"/> by the name of the property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The <see cref="IPropertyInterface{T,U}"/> for the property with the given <paramref name="propertyName"/>.</returns>
        /// <exception cref="ArgumentException">No property with the given <paramref name="propertyName"/> could be found.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is null or empty.</exception>
        public IPropertyInterface<TObj, T> GetByName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            var pi = typeof(TObj).GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (pi == null)
            {
                const string errmsg = "Unable to find the property with name `{0}` for `{1}`.";
                throw new ArgumentException(string.Format(errmsg, propertyName, this), "propertyName");
            }

            return this[pi];
        }

        class ExpressionPropertyInterface : IPropertyInterface<TObj, T>
        {
            readonly Func<TObj, T> _getter;
            readonly Action<TObj, T> _setter;

            internal ExpressionPropertyInterface(PropertyInfo propertyInfo)
            {
                _getter = CreateGetter(propertyInfo);
                _setter = CreateSetter(propertyInfo.GetSetMethod(true));
            }

            static Func<TObj, T> CreateGetter(PropertyInfo propertyInfo)
            {
                var instance = Expression.Parameter(typeof(TObj), "instance");

                var instanceCast = propertyInfo.GetGetMethod(true).IsStatic
                                       ? null : Expression.Convert(instance, propertyInfo.ReflectedType);

                var propertyAccess = Expression.Property(instanceCast, propertyInfo);
                var castPropertyValue = Expression.Convert(propertyAccess, typeof(T));
                var lambda = Expression.Lambda<Func<TObj, T>>(castPropertyValue, instance);

                return lambda.Compile();
            }

            static Action<TObj, T> CreateSetter(MethodInfo methodInfo)
            {
                // Get the instance object and parameter
                var instanceParameter = Expression.Parameter(typeof(TObj), "instance");
                var parametersParameter = Expression.Parameter(typeof(T), "parameters");

                // Cast the parameter to the needed type
                var paramInfos = methodInfo.GetParameters();
                var parameter = Expression.Convert(parametersParameter, paramInfos[0].ParameterType);

                // Check how we need to cast the instance object
                var instanceCast = methodInfo.IsStatic ? null : Expression.Convert(instanceParameter, methodInfo.ReflectedType);

                // Get the method invoke based on if it is static or instanced
                var methodCall = Expression.Call(instanceCast, methodInfo, parameter);

                // Build the expression and compile it
                var lambda = Expression.Lambda<Action<TObj, T>>(methodCall, instanceParameter, parametersParameter);

                return lambda.Compile();
            }

            #region IPropertyInterface<TObj,T> Members

            /// <summary>
            /// Gets the value of the property for the given <paramref name="obj"/> instance.
            /// </summary>
            /// <param name="obj">The object instance to get the property value for.</param>
            /// <returns>The value of the property for the given <paramref name="obj"/> instance.</returns>
            public T Get(TObj obj)
            {
                return _getter(obj);
            }

            /// <summary>
            /// Sets the value of the property for the given <paramref name="obj"/> instance.
            /// </summary>
            /// <param name="obj">The object instance to set the property value for.</param>
            /// <param name="value">The value to set the <paramref name="obj"/>'s property to.</param>
            public void Set(TObj obj, T value)
            {
                _setter(obj, value);
            }

            #endregion
        }
    }
}