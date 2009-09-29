using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Interface for an object that support drawing itself.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Notifies listeners that the object's <see cref="MapRenderLayer"/> has changed.
        /// </summary>
        event MapRenderLayerChange OnChangeRenderLayer;

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
        MapRenderLayer MapRenderLayer { get; }

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        void Draw(SpriteBatch sb);

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera"><see cref="Camera2D"/> to check if the object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        bool InView(Camera2D camera);
    }
}