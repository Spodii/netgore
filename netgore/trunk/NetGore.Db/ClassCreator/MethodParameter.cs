using System;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes a parameter on a method.
    /// </summary>
    public struct MethodParameter
    {
        static readonly MethodParameter[] _empty = new MethodParameter[0];

        /// <summary>
        /// The parameter name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The parameter type.
        /// </summary>
        public readonly string Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="formatter">The formatter.</param>
        public MethodParameter(string name, Type type, CodeFormatter formatter)
        {
            Name = name;
            Type = formatter.GetTypeString(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public MethodParameter(string name, string type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Gets an empty collection of <see cref="MethodParameter"/>s.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public static MethodParameter[] Empty
        {
            get { return _empty; }
        }
    }
}