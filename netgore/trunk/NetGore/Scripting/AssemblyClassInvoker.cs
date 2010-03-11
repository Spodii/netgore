using System;
using System.Linq;
using System.Reflection;

namespace NetGore
{
    /// <summary>
    /// Helper for invoking the methods of an <see cref="Assembly"/>.
    /// </summary>
    public class AssemblyClassInvoker
    {
        readonly string _className;
        readonly Assembly _assembly;
        readonly Type _classType;
        readonly object _classInstance;

        /// <summary>
        /// Gets the <see cref="Assembly"/> that this <see cref="AssemblyClassInvoker"/> invokes.
        /// </summary>
        public Assembly Assembly { get { return _assembly; } }

        /// <summary>
        /// Gets the name of the class that this <see cref="AssemblyClassInvoker"/> invokes.
        /// </summary>
        public string ClassName { get { return _className; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyClassInvoker"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="className">The name of the class to invoke the methods of.</param>
        public AssemblyClassInvoker(Assembly assembly, string className)
        {
            _assembly = assembly;
            _className = className;

            _classType = assembly.GetType(className);
            _classInstance = Activator.CreateInstance(_classType);
        }

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The method arguments.</param>
        /// <returns>The returned value from the invoked method.</returns>
        public object Invoke(string method, params object[] args)
        {
            var result = _classType.InvokeMember(method, BindingFlags.InvokeMethod, null, _classInstance, args );
            return result;
        }

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The method arguments.</param>
        /// <returns>The returned value from the invoked method as a string, or null if there was no returned
        /// value.</returns>
        public string InvokeAsString(string method, params object[] args)
        {
            var ret = Invoke(method, args);

            if (ret == null)
                return null;
            else
                return ret.ToString();
        }
    }
}