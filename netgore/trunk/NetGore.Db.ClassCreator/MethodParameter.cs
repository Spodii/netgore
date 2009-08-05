using System;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    public struct MethodParameter
    {
        public static readonly MethodParameter[] Empty = new MethodParameter[0];

        public readonly string Name;
        public readonly string Type;

        public MethodParameter(string name, Type type, CodeFormatter formatter)
        {
            Name = name;
            Type = formatter.GetTypeString(type);
        }

        public MethodParameter(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}