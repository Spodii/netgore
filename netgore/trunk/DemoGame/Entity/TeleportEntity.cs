using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Base of an Entity that, when used, will teleport the CharacterEntity that used it.
    /// </summary>
    public abstract class TeleportEntityBase : Entity, IUseableEntity
    {
        Vector2 _destination;
        ushort _destinationMap;

        /// <summary>
        /// Gets or sets the destination of CharacterEntities that touch the TeleportEntity.
        /// </summary>
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        /// <summary>
        /// Gets or sets the destination map index of CharacterEntities that touch the TeleportEntity.
        /// If this value is zero or equal to the map that the TeleportEntity is on, the CharacterEntity's
        /// map will not change.
        /// </summary>
        public ushort DestinationMap
        {
            get { return _destinationMap; }
            set { _destinationMap = value; }
        }

        /// <summary>
        /// TeleportEntity constructor
        /// </summary>
        protected TeleportEntityBase()
        {
            // Make sure we are set to CollisionType.Full
            CollisionType = CollisionType.Full;
        }

        /// <summary>
        /// TeleportEntity constructor
        /// </summary>
        /// <param name="position">Position of the teleporter</param>
        /// <param name="size">Size of the teleporter</param>
        /// <param name="destination">Initial destination of the teleporter</param>
        protected TeleportEntityBase(Vector2 position, Vector2 size, Vector2 destination) : base(position, size)
        {
            _destination = destination;
        }

        protected TeleportEntityBase(XmlReader r)
        {
            Load(this, r);
        }

        public static void Load(TeleportEntityBase teleportEntity, XmlReader r)
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

            r.MoveToAttribute("DestX");
            float destX = r.ReadContentAsFloat();

            r.MoveToAttribute("DestY");
            float destY = r.ReadContentAsFloat();

            r.MoveToAttribute("DestMap");
            ushort destMap = (ushort)r.ReadContentAsInt();

            r.Read();

            teleportEntity.Position = new Vector2(x, y);
            teleportEntity.Resize(new Vector2(w, h));
            teleportEntity.Destination = new Vector2(destX, destY);
            teleportEntity.CollisionType = ct;
            teleportEntity.DestinationMap = destMap;
        }

        public void Save(XmlWriter w)
        {
            w.WriteStartElement("Teleport");
            w.WriteAttributeString("X", CB.Min.X.ToString());
            w.WriteAttributeString("Y", CB.Min.Y.ToString());
            w.WriteAttributeString("W", CB.Width.ToString());
            w.WriteAttributeString("H", CB.Height.ToString());
            w.WriteAttributeString("CT", CollisionType.ToString());
            w.WriteAttributeString("DestX", Destination.X.ToString());
            w.WriteAttributeString("DestY", Destination.Y.ToString());
            w.WriteAttributeString("DestMap", DestinationMap.ToString());
            w.WriteEndElement();
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1} [map: {2}] [size: {3}x{4}]", Position, Destination, DestinationMap, CB.Size.X,
                                 CB.Size.Y);
        }

        #region IUseableEntity Members

        /// <summary>
        /// When overridden in the derived class, uses this <see cref="TeleportEntityBase"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="TeleportEntityBase"/></param>
        /// <returns>True if this <see cref="TeleportEntityBase"/> was successfully used, else false</returns>
        public abstract bool Use(CharacterEntity charEntity);

        /// <summary>
        /// When overridden in the derived class, checks if this <see cref="TeleportEntityBase"/> 
        /// can be used by the specified <paramref name="charEntity"/>, but does
        /// not actually use this <see cref="TeleportEntityBase"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="TeleportEntityBase"/></param>
        /// <returns>True if this <see cref="TeleportEntityBase"/> can be used, else false</returns>
        public abstract bool CanUse(CharacterEntity charEntity);

        /// <summary>
        /// When overridden in the derived class, notifies listeners that this <see cref="TeleportEntityBase"/> was used,
        /// and which <see cref="CharacterEntity"/> used it
        /// </summary>
        public abstract event EntityEventHandler<CharacterEntity> OnUse;

        #endregion
    }
}