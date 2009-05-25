using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    public abstract class NewTeleportEntityBase : DynamicEntity, IUseableEntity
    {
        public Vector2 Destination { get; set; }

        public ushort DestinationMap { get; set; }

        protected NewTeleportEntityBase()
        {
            Weight = 0f;
        }

        protected override void WriteCustomValues(NetGore.IO.IValueWriter writer)
        {
            writer.Write("Destination", Destination);
            writer.Write("DestinationMap", DestinationMap);
        }

        protected override void ReadCustomValues(NetGore.IO.IValueReader reader)
        {
            Destination = reader.ReadVector2("Destination");
            DestinationMap = reader.ReadUShort("DestinationMap");
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
