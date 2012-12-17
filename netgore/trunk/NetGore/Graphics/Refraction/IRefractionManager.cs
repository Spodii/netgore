using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an object that manages multiple <see cref="IRefractionEffect"/>s.
    /// </summary>
    public interface IRefractionManager : IList<IRefractionEffect>, IDisposable
    {
        /// <summary>
        /// Gets or sets the <see cref="Shader"/> to use to draw the refraction map.
        /// </summary>
        Shader DrawToTargetShader { get; set; }

        /// <summary>
        /// Gets or sets if this <see cref="IRefractionManager"/> is enabled.
        /// If <see cref="SFML.Graphics.Shader.IsAvailable"/> is false, this will always be false.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets if the <see cref="IRefractionManager"/> has been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Draws all of the reflection effects in this <see cref="IRefractionManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Texture"/> containing the reflection map. If the reflection map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IRefractionManager.IsInitialized"/> is false.</exception>
        Texture Draw(ICamera2D camera);

        /// <summary>
        /// Draws the refraction map to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <param name="target">The <see cref="RenderTarget"/> to draw the refraction map to.</param>
        /// <param name="colorMap">The <see cref="Texture"/> to get the colors from. Typically, this is an <see cref="Texture"/> of
        /// the fully drawn game scene to apply refractions to.</param>
        void DrawToTarget(ICamera2D camera, RenderTarget target, Texture colorMap);

        /// <summary>
        /// Initializes the <see cref="IRefractionManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="IRefractionEffect"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is null.</exception>
        void Initialize(Window window);

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void Update(TickCount currentTime);
    }
}