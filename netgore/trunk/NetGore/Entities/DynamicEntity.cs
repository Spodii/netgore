using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    public abstract class DynamicEntity : Entity
    {
        public void Read(IValueReader reader)
        {
            Vector2 position = reader.ReadVector2("Position");
            Vector2 size = reader.ReadVector2("Size");
            Vector2 velocity = reader.ReadVector2("Velocity");
            float weight = reader.ReadFloat("Weight");
            CollisionType collisionType = reader.ReadCollisionType("CollisionType");

            LoadEntityValues(position, size, velocity, weight, collisionType);

            ReadCustomValues(reader);
        }

        protected static CollisionType ReadCollisionType(BitStream writer)
        {
            return (CollisionType)writer.ReadByte();
        }

        protected abstract void ReadCustomValues(IValueReader reader);

        public void Write(IValueWriter writer)
        {
            writer.Write("Position", Position);
            writer.Write("Size", Size);
            writer.Write("Velocity", Velocity);
            writer.Write("Weight", Weight);
            writer.Write("CollisionType", CollisionType);
            WriteCustomValues(writer);
        }

        protected abstract void WriteCustomValues(IValueWriter writer);
    }
}