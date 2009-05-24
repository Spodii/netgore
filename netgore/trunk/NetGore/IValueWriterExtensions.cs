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
            if (writer.SupportsNameLookup)
            {
                // We are using name lookup, so we have to combine the values so we use only one name
                writer.Write(name, value.X + "," + value.Y);
            }
            else
            {
                // Not using name lookup, so just write them out
                writer.Write(null, value.X);
                writer.Write(null, value.Y);
            }
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
