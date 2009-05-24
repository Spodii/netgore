using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    public static class IValueReaderExtensions
    {
        public static Vector2 ReadVector2(this IValueReader reader)
        {
            var x = reader.ReadFloat();
            var y = reader.ReadFloat();
            return new Vector2(x, y);
        }

        public static Vector2 ReadVector2(this INamedValueReader reader, string name)
        {
            string value = reader.ReadString(name);
            string[] split = value.Split(',');
            var x = float.Parse(split[0]);
            var y = float.Parse(split[1]);
            return new Vector2(x, y);
        }

        public static CollisionType ReadCollisionType(this IValueReader reader)
        {
            return (CollisionType)reader.ReadByte();
        }

        public static CollisionType ReadCollisionType(this INamedValueReader reader, string name)
        {
            return ReadEnum<CollisionType>(reader, name);
        }

        static T ReadEnum<T>(INamedValueReader reader, string name)
        {
            Type type = typeof(T);
            var str = reader.ReadString(name);
            var value = (T)Enum.Parse(type, str);
            return value;
        }
    }
}
