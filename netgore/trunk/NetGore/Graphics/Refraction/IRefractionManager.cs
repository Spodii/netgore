using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an object that manages multiple <see cref="IRefractionEffect"/>s.
    /// </summary>
    public interface IRefractionManager : IList<IRefractionEffect>, IDisposable
    {
        /// <summary>
        /// Gets or sets if this <see cref="IRefractionManager"/> is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets if the <see cref="IRefractionManager"/> has been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Initializes the <see cref="IRefractionManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="IRefractionEffect"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="renderWindow">The <see cref="RenderWindow"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="renderWindow"/> is null.</exception>
        void Initialize(RenderWindow renderWindow);

        /// <summary>
        /// Draws all of the reflection effects in this <see cref="IRefractionManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Image"/> containing the reflection map. If the reflection map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IRefractionManager.IsInitialized"/> is false.</exception>
        Image Draw(ICamera2D camera);

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void Update(TickCount currentTime);
    }
}
