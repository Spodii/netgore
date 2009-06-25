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
    [MapFileEntity]
    public class TeleportEntity : TeleportEntityBase, IDrawableEntity
    {
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
            if (OnUse != null)
                OnUse(this, charEntity);

            return true;
        }

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

        /// <summary>
        /// Notifies the listeners when the IUseableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        public override event EntityEventHandler<DynamicEntity> OnUse;
    }
}