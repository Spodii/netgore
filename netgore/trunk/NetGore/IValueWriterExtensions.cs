using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    public static class IValueWriterExtensions
    {
        public static void Write(this IValueWriter writer, Vector2 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
        }

        public static void Write(this INamedValueWriter writer, string name, Vector2 value)
        {
            writer.Write(name, value.X + "," + value.Y);
        }

        public static void Write(this IValueWriter writer, CollisionType collisionType)
        {
            writer.Write((byte)collisionType);
        }

        public static void Write(this INamedValueWriter writer, string name, CollisionType collisionType)
        {
            WriteEnum(writer, name, collisionType);
        }

        static void WriteEnum<T>(INamedValueWriter writer, string name, T value)
        {
            writer.Write(name, value.ToString());
        }
    }
}
