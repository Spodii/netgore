﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using NetGore;

namespace DemoGame.Server
{
    public class TeleportEntity : TeleportEntityBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Client: 
        ///     Checks if the Client's character can attempt to use the IUseableEntity. If false, the Client
        ///     wont even attempt to use the IUseableEntity. If true, the Client will attempt to use it, but
        ///     it is not guarenteed the Server will also allow it to be used.
        /// Server:
        ///     Checks if the specified Entity may use the IUseableEntity.
        /// </summary>
        /// <param name="charEntity">The CharacterEntity that is trying to use this IUsableEntity. For the Client,
        /// this will always be the User's Character. Can be null.</param>
        /// <returns>True if this IUsableEntity can be used by the <paramref name="charEntity"/>, else false.</returns>
        public override bool CanUse(DynamicEntity charEntity)
        {
            // Check that the base usage rules pass
            if (!base.CanUse(charEntity))
                return false;

            // Add custom rules here

            return true;
        }

        /// <summary>
        /// Client:
        ///     Handles any additional usage stuff. When this is called, it is to be assumed that the Server has recognized
        ///     the IUseableEntity as having been successfully used.
        /// Server:
        ///     Attempts to use this IUsableEntity on the <paramref name="charEntity"/>.
        /// </summary>
        /// <param name="charEntity">CharacterEntity that is trying to use this IUseableEntity. Can be null.</param>
        /// <returns>True if this IUseableEntity was successfully used, else false. On the Client, this is generally
        /// unused.</returns>
        public override bool Use(DynamicEntity charEntity)
        {
            // Check if we can use
            if (!CanUse(charEntity))
                return false;

            // Teleport to a new map
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

            // Successfully used
            return true;
        }

        /// <summary>
        /// Notifies the listeners when the IUseableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        public override event EntityEventHandler<DynamicEntity> OnUse;
    }
}