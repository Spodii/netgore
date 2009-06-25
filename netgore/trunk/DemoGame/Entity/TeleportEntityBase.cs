using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// An Entity that can teleport another Entity to a new position and map upon use.
    /// </summary>
    public abstract class TeleportEntityBase : DynamicEntity, IUseableEntity
    {
        Vector2 _destination;
        MapIndex _destinationMap;

        /// <summary>
        /// Gets or sets the map position that the Entity will be teleported to upon use.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        [Category("Teleport")]
        [DisplayName("Destination")]
        [Description("Position to teleport the Entity that uses this Teleport to.")]
        [Browsable(true)]
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        /// <summary>
        /// Gets or sets the index of the map that the Entity will be teleported to upon use.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        [Category("Teleport")]
        [DisplayName("DestinationMap")]
        [Description("Map to teleport the Entity that uses this Teleport to.")]
        [Browsable(true)]
        public MapIndex DestinationMap
        {
            get { return _destinationMap; }
            set { _destinationMap = value; }
        }

        protected TeleportEntityBase()
        {
            Weight = 0f;
        }

        #region IUseableEntity Members

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
        public abstract bool Use(DynamicEntity charEntity);

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
        public virtual bool CanUse(DynamicEntity charEntity)
        {
            if (charEntity == null)
                return false;

            return true;
        }

        /// <summary>
        /// Gets if the Client should be notified when this IUseableEntity is used. If true, when this IUsableEntity is
        /// used on the Server, every Client in the Map will be notified of the usage. As a result, Use() will be called
        /// on each Client. If false, this message will never be sent. Only set to true if any code is placed in
        /// Use() on the Client implementation of the IUseableEntity, or there are expected to be listeners to OnUse.
        /// </summary>
        public bool NotifyClientsOfUsage
        {
            get { return false; }
        }

        /// <summary>
        /// Notifies the listeners when the IUseableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        public abstract event EntityEventHandler<DynamicEntity> OnUse;

        #endregion
    }
}