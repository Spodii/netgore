using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Platyform;
using Platyform.Extensions;

namespace DemoGame.Client
{
    public class TeleportEntity : TeleportEntityBase
    {
        public override event EntityEventHandler<CharacterEntity> OnUse
        {
            add { }
            remove { }
        }

        /// <summary>
        /// TeleportEntity constructor
        /// </summary>
        public TeleportEntity()
        {
        }

        /// <summary>
        /// TeleportEntity constructor
        /// </summary>
        /// <param name="position">Position of the teleporter</param>
        /// <param name="size">Size of the teleporter</param>
        /// <param name="destination">Initial destination of the teleporter</param>
        public TeleportEntity(Vector2 position, Vector2 size, Vector2 destination) : base(position, size, destination)
        {
        }

        public TeleportEntity(XmlReader r) : base(r)
        {
        }

        public override bool CanUse(CharacterEntity charEntity)
        {
            return true;
        }

        public override bool Use(CharacterEntity charEntity)
        {
            throw new MethodAccessException("The client may not actually an IUsableEntity, only send requests to use them.");
        }
    }
}