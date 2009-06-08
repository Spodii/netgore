using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    [MapFileEntityAttribute]
    public class TeleportEntity : TeleportEntityBase, IDrawableEntity
    {
        /// <summary>
        /// When overridden in the derived class, notifies listeners that this <see cref="TeleportEntityBase"/> was used,
        /// and which <see cref="CharacterEntity"/> used it
        /// </summary>
        public override event EntityEventHandler<CharacterEntity> OnUse
        {
            add { }
            remove { }
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
            throw new MethodAccessException("The client may not actually an IUsableEntity, only send requests to use them.");
        }

        #region IDrawableEntity Members

        /// <summary>
        /// Notifies listeners that the Entity's MapRenderLayer has changed.
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer;

        /// <summary>
        /// Gets the MapRenderLayer that this entity is rendered on.
        /// </summary>
        [Browsable(false)]
        public MapRenderLayer MapRenderLayer
        {
            get { return MapRenderLayer.Foreground; }
        }

        /// <summary>
        /// Makes the Entity draw itself.
        /// </summary>
        /// <param name="sb">SpriteBatch the entity can use to draw itself with.</param>
        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = CB.ToRectangle();
            XNARectangle.Draw(sb, rect, new Color(255, 255, 255, 100), Color.Black);
        }

        /// <summary>
        /// Checks if in the Entity is in view of the specified camera.
        /// </summary>
        /// <param name="camera">Camera to check if the Entity is in view of.</param>
        /// <returns>True if the Entity is in view of the camera, else False.</returns>
        public bool InView(Camera2D camera)
        {
            return camera.InView(this);
        }

        #endregion
    }
}