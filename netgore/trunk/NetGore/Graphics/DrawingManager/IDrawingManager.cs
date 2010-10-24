using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a component that manages the preparation and tear-down for drawing.
    /// </summary>
    public interface IDrawingManager : IDisposable
    {
        /// <summary>
        /// Gets or sets the background <see cref="Color"/>.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets if this <see cref="IDrawingManager"/> has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets the <see cref="ILightManager"/> used by this <see cref="IDrawingManager"/>.
        /// </summary>
        ILightManager LightManager { get; }

        /// <summary>
        /// Gets the <see cref="IRefractionManager"/> used by this <see cref="IDrawingManager"/>.
        /// </summary>
        IRefractionManager RefractionManager { get; }

        /// <summary>
        /// Gets or sets the <see cref="RenderWindow"/> to draw to.
        /// </summary>
        RenderWindow RenderWindow { get; set; }

        /// <summary>
        /// Gets the <see cref="DrawingManagerState"/> describing the current drawing state.
        /// </summary>
        DrawingManagerState State { get; }

        /// <summary>
        /// Begins drawing the graphical user interface, which is not affected by the camera.
        /// </summary>
        /// <param name="clearBuffer">When true, the buffer will be cleared before drawing. When false, the contents of the previous
        /// frame will remain in the buffer, only if the last draw was also to the GUI. When the last draw call was to the
        /// world, then this will have no affect. Useful for when you want to draw multiple GUI screens on top of one another.</param>
        /// <returns>The <see cref="ISpriteBatch"/> to use to draw the GUI, or null if an unexpected
        /// error was encountered when preparing the <see cref="ISpriteBatch"/>. When null, all drawing
        /// should be aborted completely instead of trying to draw with a different <see cref="ISpriteBatch"/>
        /// or manually recovering the error.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        ISpriteBatch BeginDrawGUI(bool clearBuffer = true);

        /// <summary>
        /// Begins drawing of the world.
        /// </summary>
        /// <param name="camera">The camera describing the the current view of the world.</param>
        /// <returns>
        /// The <see cref="ISpriteBatch"/> to use to draw the world objects, or null if an unexpected
        /// error was encountered when preparing the <see cref="ISpriteBatch"/>. When null, all drawing
        /// should be aborted completely instead of trying to draw with a different <see cref="ISpriteBatch"/>
        /// or manually recovering the error.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        ISpriteBatch BeginDrawWorld(ICamera2D camera);

        /// <summary>
        /// Ends drawing the graphical user interface.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.DrawingGUI"/>.</exception>
        void EndDrawGUI();

        /// <summary>
        /// Ends drawing the world.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.DrawingWorld"/>.</exception>
        void EndDrawWorld();

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void Update(TickCount currentTime);
    }
}