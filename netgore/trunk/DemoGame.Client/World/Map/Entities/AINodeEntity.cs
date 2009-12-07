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
    public class AINodeEntity : AINodeEntityBase, IDrawable
    {
        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        public override bool CollidesAgainstWalls
        {
            get { return false; }
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
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = CB.ToRectangle();
            XNARectangle.Draw(sb, rect, new Color(0, 0, 0, 0), Color.Black);
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera"><see cref="Camera2D"/> to check if the object is in view of.</param>
        /// <returns>
        /// True if the object is in view of the camera, else False.
        /// </returns>
        public bool InView(Camera2D camera)
        {
            return camera.InView(this);
        }

        #endregion
    }
}