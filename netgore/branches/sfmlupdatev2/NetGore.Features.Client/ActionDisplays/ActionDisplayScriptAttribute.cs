using System;
using System.Linq;

namespace NetGore.Features.ActionDisplays
{
    /// <summary>
    /// Attribute used to denote a method used to handle how to display a <see cref="ActionDisplay"/>.
    /// Methods that implement this attribute must use the same signature as <see cref="ActionDisplayScriptHandler"/>, and
    /// should only be implemented on static methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ActionDisplayScriptAttribute : Attribute
    {
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDisplayScriptAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the script.</param>
        public ActionDisplayScriptAttribute(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the name of the script.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
    }
}