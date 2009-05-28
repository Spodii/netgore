﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        ushort _destinationMap;

        /// <summary>
        /// Gets or sets the map position that the Entity will be teleported to upon use.
        /// </summary>
        [SyncValue]
        public Vector2 Destination { get { return _destination; } set { _destination = value; } }

        /// <summary>
        /// Gets or sets the index of the map that the Entity will be teleported to upon use.
        /// </summary>
        [SyncValue]
        public ushort DestinationMap { get { return _destinationMap; } set { _destinationMap = value; } }

        protected TeleportEntityBase()
        {
            Weight = 0f;
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
