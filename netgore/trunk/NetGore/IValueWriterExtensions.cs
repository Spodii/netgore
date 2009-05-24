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
        public static void Write(this IValueWriter writer, string name, Vector2 value)
        {
            writer.Write(name, value.X + "," + value.Y);
        }

        public static void Write(this IValueWriter writer, string name, CollisionType collisionType)
        {
            WriteEnum(writer, name, collisionType);
        }

        static void WriteEnum<T>(IValueWriter writer, string name, T value)
        {
            writer.Write(name, value.ToString());
        }
    }
}
