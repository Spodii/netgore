using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using log4net;
using NetGore.Collections;

namespace NetGore.IO
{
    public class PropertyInterface<TObj, T> : ThreadSafeSingletonFactory<PropertyInfo, IPropertyInterface<TObj, T>>
    {
        static readonly PropertyInterface<TObj, T> _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInterface&lt;TObj, T&gt;"/> class.
        /// </summary>
        static PropertyInterface()
        {
            _instance = new PropertyInterface<TObj, T>();
        }

        public IPropertyInterface<TObj, T> GetByName(string propertyName)
        {
            var pi = typeof(TObj).GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (pi == null)
                throw new ArgumentException("propertyName");

            return Get(pi);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInterface&lt;TObj, T&gt;"/> class.
        /// </summary>
        PropertyInterface()
        {
        }

        public static PropertyInterface<TObj, T> Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, creates the value for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to create the value for.</param>
        /// <returns></returns>
        protected override IPropertyInterface<TObj, T> CreateInstance(PropertyInfo key)
        {
            return new ExpressionPropertyInterface(key);
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