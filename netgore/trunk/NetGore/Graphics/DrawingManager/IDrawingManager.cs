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
        /// Gets or sets the <see cref="Color"/> to use when clearing the screen.
        /// </summary>
        Color ClearColor { get; set; }

        /// <summary>
        /// Gets the <see cref="ILightManager"/> used by this <see cref="IDrawingManager"/>.
        /// </summary>
        ILightManager LightManager { get; }

        /// <summary>
        /// Gets the <see cref="DrawingManagerState"/> describing the current drawing state.
        /// </summary>
        DrawingManagerState State { get; }

        /// <summary>
        /// Begins drawing the graphical user interface, which is not affected by the camera.
        /// </summary>
        /// <returns>The <see cref="ISpriteBatch"/> to use to draw the GUI, or null if an unexpected
        /// error was encountered when preparing the <see cref="ISpriteBatch"/>. When null, all drawing
        /// should be aborted completely instead of trying to draw with a different <see cref="ISpriteBatch"/>
        /// or manually recovering the error.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        ISpriteBatch BeginDrawGUI();

        /// <summary>
        /// Begins drawing of the world.
        /// </summary>
        /// <param name="camera">The camera describing the the current view of the world.</param>
        /// <returns>The <see cref="ISpriteBatch"/> to use to draw the world objects, or null if an unexpected
        /// error was encountered when preparing the <see cref="ISpriteBatch"/>. When null, all drawing
        /// should be aborted completely instead of trying to draw with a different <see cref="ISpriteBatch"/>
        /// or manually recovering the error.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        ISpriteBatch BeginDrawWorld(ICamera2D camera);

        /// <summary>
        /// Begins drawing of the world.
        /// </summary>
        /// <param name="camera">The camera describing the the current view of the world.</param>
        /// <param name="useLighting">Whether or not the <see cref="IDrawingManager.LightManager"/> is used to
        /// produce the world lighting.</param>
        /// <param name="bypassClear">If true, the backbuffer will not be cleared before the drawing starts,
        /// resulting in the new images being drawn on top of the previous frame instead of from a fresh screen.</param>
        /// <returns>
        /// The <see cref="ISpriteBatch"/> to use to draw the world objects, or null if an unexpected
        /// error was encountered when preparing the <see cref="ISpriteBatch"/>. When null, all drawing
        /// should be aborted completely instead of trying to draw with a different <see cref="ISpriteBatch"/>
        /// or manually recovering the error.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        ISpriteBatch BeginDrawWorld(ICamera2D camera, bool useLighting, bool bypassClear);

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
        void Update(int currentTime);
    }
}