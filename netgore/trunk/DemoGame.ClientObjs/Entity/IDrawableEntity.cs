using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Interface for an Entity that support drawing itself.
    /// </summary>
    public interface IDrawableEntity
    {
        /// <summary>
        /// Notifies listeners that the Entity's MapRenderLayer has changed.
        /// </summary>
        event MapRenderLayerChange OnChangeRenderLayer;

        /// <summary>
        /// Gets the MapRenderLayer that this Entity is rendered on.
        /// </summary>
        MapRenderLayer MapRenderLayer { get; }

        /// <summary>
        /// Makes the Entity draw itself.
        /// </summary>
        /// <param name="sb">SpriteBatch the entity can use to draw itself with.</param>
        void Draw(SpriteBatch sb);

        /// <summary>
        /// Checks if in the Entity is in view of the specified camera.
        /// </summary>
        /// <param name="camera">Camera to check if the Entity is in view of.</param>
        /// <returns>True if the Entity is in view of the camera, else False.</returns>
        bool InView(Camera2D camera);
    }
}