using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using IDrawable=NetGore.Graphics.IDrawable;

namespace DemoGame.Client
{
    [MapFileEntity]
    public class TeleportEntity : TeleportEntityBase, IDrawable
    {
        /// <summary>
        /// Notifies the listeners when the IUsableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        public override event EntityEventHandler<DynamicEntity> OnUse;

        /// <summary>
        /// Client:
        ///     Handles any additional usage stuff. When this is called, it is to be assumed that the Server has recognized
        ///     the IUsableEntity as having been successfully used.
        /// Server:
        ///     Attempts to use this IUsableEntity on the <paramref name="charEntity"/>.
        /// </summary>
        /// <param name="charEntity">CharacterEntity that is trying to use this IUsableEntity. Can be null.</param>
        /// <returns>True if this IUsableEntity was successfully used, else false. On the Client, this is generally
        /// unused.</returns>
        public override bool Use(DynamicEntity charEntity)
        {
            if (OnUse != null)
                OnUse(this, charEntity);

            return true;
        }

        #region IDrawable Members

        /// <summary>
        /// Notifies listeners that the object's <see cref="MapRenderLayer"/> has changed.
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer;

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
        /// <value></value>
        [Browsable(false)]
        public MapRenderLayer MapRenderLayer
        {
            get { return MapRenderLayer.SpriteForeground; }
        }

        /// <summary>
        /// Gets the depth of the object for the <see cref="IDrawable.MapRenderLayer"/> the object is on. A lower
        /// layer depth results in the object being drawn on top of (in front of) objects with a higher value.
        /// </summary>
        public byte LayerDepth
        {
            get { return 0; }
        }

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = ToRectangle();
            XNARectangle.Draw(sb, rect, new Color(255, 255, 255, 100), Color.Black);
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>
        /// True if the object is in view of the camera, else False.
        /// </returns>
        public bool InView(ICamera2D camera)
        {
            return camera.InView(this);
        }

        #endregion
    }
}