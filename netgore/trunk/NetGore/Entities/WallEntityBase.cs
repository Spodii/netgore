using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;

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
        /// <summary>
        /// WallEntity constructor
        /// </summary>
        protected WallEntityBase(Vector2 position, Vector2 size) : base(position, size)
        {
            Weight = 0.0f; // Walls have no weight
        }

        /// <summary>
        /// WallEntity constructor
        /// </summary>
        protected WallEntityBase()
        {
        }

        public static TWall Load<TWall>(XmlReader r) where TWall : WallEntityBase, new()
        {
            r.MoveToContent();

            r.MoveToAttribute("X");
            float x = r.ReadContentAsFloat();

            r.MoveToAttribute("Y");
            float y = r.ReadContentAsFloat();

            r.MoveToAttribute("W");
            float w = r.ReadContentAsFloat();

            r.MoveToAttribute("H");
            float h = r.ReadContentAsFloat();

            r.MoveToAttribute("CT");
            CollisionType ct = (CollisionType)Enum.Parse(typeof(CollisionType), r.ReadContentAsString());

            r.Read();

            TWall wall = Create<TWall>(new Vector2(x, y), w, h);
            wall.CollisionType = ct;
            return wall;
        }

        public void Save(XmlWriter w)
        {
            w.WriteStartElement("Wall");
            w.WriteAttributeString("X", CB.Min.X.ToString());
            w.WriteAttributeString("Y", CB.Min.Y.ToString());
            w.WriteAttributeString("W", CB.Width.ToString());
            w.WriteAttributeString("H", CB.Height.ToString());
            w.WriteAttributeString("CT", CollisionType.ToString());
            w.WriteEndElement();
        }
    }
}