using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a manager of multiple <see cref="ILight"/>s.
    /// </summary>
    public interface ILightManager : IList<ILight>, IDisposable
    {
        /// <summary>
        /// Gets or sets the ambient light color. The alpha value has no affect and will always be set to 255.
        /// </summary>
        Color Ambient { get; set; }

        /// <summary>
        /// Gets or sets the default sprite to use for all lights added to this <see cref="ILightManager"/>.
        /// When this value changes, all <see cref="ILight"/>s in this <see cref="ILightManager"/> who's
        /// <see cref="ILight.Sprite"/> is equal to the old value will have their sprite set to the new value.
        /// </summary>
        Grh DefaultSprite { get; set; }

        /// <summary>
        /// Gets if the <see cref="ILightManager"/> has been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Draws all of the lights in this <see cref="ILightManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Image"/> containing the light map. If the light map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="ILightManager.IsInitialized"/> is false.</exception>
        Image Draw(ICamera2D camera);

        /// <summary>
        /// Initializes the <see cref="ILightManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="ILight"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="renderWindow">The <see cref="RenderWindow"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="renderWindow"/> is null.</exception>
        void Initialize(RenderWindow renderWindow);

        /// <summary>
        /// Updates all of the lights in this <see cref="ILightManager"/>, along with the <see cref="ILightManager"/> itself.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void Update(TickCount currentTime);
    }
}