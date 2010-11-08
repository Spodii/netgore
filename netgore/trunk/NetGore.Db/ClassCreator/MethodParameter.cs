using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes a parameter on a method.
    /// </summary>
    public struct MethodParameter
    {
        static readonly MethodParameter[] _empty = new MethodParameter[0];

        readonly string _name;
        readonly string _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="formatter">The formatter.</param>
        public MethodParameter(string name, Type type, CodeFormatter formatter)
        {
            _name = name;
            _type = formatter.GetTypeString(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public MethodParameter(string name, string type)
        {
            _name = name;
            _type = type;
        }

        /// <summary>
        /// Gets an empty collection of <see cref="MethodParameter"/>s.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public static MethodParameter[] Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
    }
}