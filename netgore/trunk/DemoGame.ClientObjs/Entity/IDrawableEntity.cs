using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Interface for entities which support drawing itself
    /// </summary>
    public interface IDrawableEntity
    {
        /// <summary>
        /// Notifies that the Entity's MapRenderLayer has changed
        /// </summary>
        event MapRenderLayerChange OnChangeRenderLayer;

        /// <summary>
        /// Gets the layer that this entity is rendered at
        /// </summary>
        MapRenderLayer MapRenderLayer { get; }

        /// <summary>
        /// Tells the entity to draw itself
        /// </summary>
        /// <param name="sb">SpriteBatch the entity can use to draw itself with</param>
        void Draw(SpriteBatch sb);

        /// <summary>
        /// Checks if in view of the specified camera
        /// </summary>
        /// <param name="camera">Camera to check if in view of</param>
        /// <returns>True if in view of the camera, else false</returns>
        bool InView(Camera2D camera);
    }
}