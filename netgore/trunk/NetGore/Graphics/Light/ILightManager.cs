using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a manager of multiple <see cref="ILight"/>s and the generation of the light map.
    /// </summary>
    public interface ILightManager : IList<ILight>, IDisposable
    {
        /// <summary>
        /// Gets or sets the ambient light color. The alpha value has no affect and will always be set to 255.
        /// </summary>
        Color Ambient { get; set; }

        /// <summary>
        /// Gets or sets the default sprite to use for all lights added to this <see cref="ILightManager"/>
        /// that do not have a sprite set. Only sprites added that do not already have a <see cref="ILight.Sprite"/>
        /// set will have this sprite set on them.
        /// </summary>
        Grh DefaultSprite { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Shader"/> to use to draw the light map.
        /// </summary>
        Shader DrawToTargetShader { get; set; }

        /// <summary>
        /// Gets or sets if this <see cref="ILightManager"/> is enabled.
        /// If <see cref="SFML.Graphics.Shader.IsAvailable"/> is false, this will always be false.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets if the <see cref="ILightManager"/> has been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Draws all of the lights in this <see cref="ILightManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Texture"/> containing the light map. If the light map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="ILightManager.IsInitialized"/> is false.</exception>
        Texture Draw(ICamera2D camera);

        /// <summary>
        /// Draws the light map to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <param name="target">The <see cref="RenderTarget"/> to draw the light map to.</param>
        void DrawToTarget(ICamera2D camera, RenderTarget target);

        /// <summary>
        /// Initializes the <see cref="ILightManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="ILight"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        void Initialize(Window window);

        /// <summary>
        /// Updates all of the lights in this <see cref="ILightManager"/>, along with the <see cref="ILightManager"/> itself.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void Update(TickCount currentTime);
    }
}