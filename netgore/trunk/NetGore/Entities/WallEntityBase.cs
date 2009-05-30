using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Base of all entities in the world which are not passable by other solid entities. WallEntities also do
    /// not ever change or move, allowing them to be stored in the map files. Despite the name,
    /// this refers to more than just walls such as a floor, platform, or even just an invisible
    /// plane of nothingness that you are just not allowed to go inside of for absolutely no reason.
    /// If you want a dynamic wall, you will want to use a DynamicEntity.
    /// </summary>
    public abstract class WallEntityBase : Entity
    {
        protected WallEntityBase(Vector2 position, Vector2 size)
            : this(position, size, CollisionType.Full)
        {
        }

        protected WallEntityBase(IValueReader r)
        {
            Read(r);
        }

        protected WallEntityBase(Vector2 position, Vector2 size, CollisionType collisionType)
            : base(position, size)
        {
            CollisionType = collisionType;
            Weight = 0.0f; // Walls have no weight
        }

        public void Write(IValueWriter w)
        {
            w.Write("Position", Position);
            w.Write("Size", Size);
            w.Write("CollisionType", CollisionType);
        }

        void Read(IValueReader r)
        {
            SetPositionRaw(r.ReadVector2("Position"));
            SetSizeRaw(r.ReadVector2("Size"));
            SetCollisionTypeRaw(r.ReadCollisionType("CollisionType"));
        }
    }
}