using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using DemoGame.Extensions;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    class TeleportEntity : TeleportEntityBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// When overridden in the derived class, notifies listeners that this <see cref="TeleportEntityBase"/> was used,
        /// and which <see cref="CharacterEntity"/> used it
        /// </summary>
        public override event EntityEventHandler<CharacterEntity> OnUse;

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

        /// <summary>
        /// When overridden in the derived class, checks if this <see cref="TeleportEntityBase"/> 
        /// can be used by the specified <paramref name="charEntity"/>, but does
        /// not actually use this <see cref="TeleportEntityBase"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="TeleportEntityBase"/></param>
        /// <returns>True if this <see cref="TeleportEntityBase"/> can be used, else false</returns>
        public override bool CanUse(CharacterEntity charEntity)
        {
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, uses this <see cref="TeleportEntityBase"/>.
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="TeleportEntityBase"/></param>
        /// <returns>True if this <see cref="TeleportEntityBase"/> was successfully used, else false</returns>
        public override bool Use(CharacterEntity charEntity)
        {
            // Check if we can use
            if (!CanUse(charEntity))
                return false;

            if (DestinationMap > 0)
            {
                Character c = (Character)charEntity;
                if (c.Map.Index != DestinationMap)
                {
                    Map newMap = c.World.GetMap(DestinationMap);
                    if (newMap == null)
                    {
                        const string errmsg = "Failed to teleport Character `{0}` - Invalid DestMap `{1}`.";
                        Debug.Fail(string.Format(errmsg, c, this));
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, c, this);
                        return false;
                    }
                    c.SetMap(newMap);
                }
            }

            // Teleport the CharacterEntity to our predefined location
            charEntity.Teleport(Destination);

            // Notify listeners
            if (OnUse != null)
                OnUse(this, charEntity);

            return true;
        }
    }
}