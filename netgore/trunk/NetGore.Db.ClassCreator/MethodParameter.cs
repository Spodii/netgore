using System;
using System.Linq;
using NetGore;

namespace NetGore.Db.ClassCreator
{
    public struct MethodParameter
    {
        public static readonly MethodParameter[] Empty = new MethodParameter[0];

        public readonly string Name;
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
    }
}