using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    public class DrawingManager : IDrawingManager
    {
        readonly GraphicsDevice _gd;
        readonly ILightManager _lightManager;
        readonly SpriteBatch _sb;

        Texture2D _lightMap;
        DrawingManagerState _state = DrawingManagerState.Idle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingManager"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        public DrawingManager(GraphicsDevice graphicsDevice)
        {
            _gd = graphicsDevice;
            _sb = new SpriteBatch(_gd);

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _lightManager = CreateLightManager();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            _lightManager.Initialize(_gd);
        }

        /// <summary>
        /// Creates the <see cref="ILightManager"/> to use.
        /// </summary>
        /// <returns>The <see cref="ILightManager"/> to use.</returns>
        protected virtual ILightManager CreateLightManager()
        {
            return new LightManager();
        }

        #region IDrawingManager Members

        /// <summary>
        /// Gets the <see cref="ILightManager"/> used by this <see cref="IDrawingManager"/>.
        /// </summary>
        public ILightManager LightManager
        {
            get { return _lightManager; }
        }

        /// <summary>
        /// Gets the <see cref="DrawingManagerState"/> describing the current drawing state.
        /// </summary>
        public DrawingManagerState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Begins drawing the graphical user interface, which is not affected by the camera.
        /// </summary>
        /// <returns>The <see cref="SpriteBatch"/> to use to draw the GUI.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        public SpriteBatch BeginDrawGUI()
        {
            if (State != DrawingManagerState.Idle)
                throw new InvalidOperationException("This method cannot be called while already busy drawing.");

            _state = DrawingManagerState.DrawingGUI;

            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            return _sb;
        }

        /// <summary>
        /// Begins drawing of the world.
        /// </summary>
        /// <param name="camera">The camera describing the the current view of the world.</param>
        /// <returns>The <see cref="SpriteBatch"/> to use to draw the world objects.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        public SpriteBatch BeginDrawWorld(ICamera2D camera)
        {
            if (State != DrawingManagerState.Idle)
                throw new InvalidOperationException("This method cannot be called while already busy drawing.");

            _state = DrawingManagerState.DrawingWorld;

            _lightMap = _lightManager.Draw(camera);

            _gd.Clear(Color.CornflowerBlue);
            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.Matrix);

            return _sb;
        }

        /// <summary>
        /// Ends drawing the graphical user interface.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.DrawingGUI"/>.</exception>
        public void EndDrawGUI()
        {
            if (State != DrawingManagerState.DrawingGUI)
                throw new InvalidOperationException("This method can only be called after BeginDrawGUI.");

            _state = DrawingManagerState.Idle;

            _sb.End();
        }

        /// <summary>
        /// Ends drawing the world.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.DrawingWorld"/>.</exception>
        public void EndDrawWorld()
        {
            if (State != DrawingManagerState.DrawingWorld)
                throw new InvalidOperationException("This method can only be called after BeginDrawWorld.");

            _state = DrawingManagerState.Idle;

            _sb.End();

            var rs = _gd.RenderState;

            // Store the old state values
            var oldSourceBlend = rs.SourceBlend;
            var oldDestinationBlend = rs.DestinationBlend;
            var oldBlendFunction = rs.BlendFunction;

            // Start drawing
            _sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            // Set the blending mode
            rs.SourceBlend = Blend.Zero;
            rs.DestinationBlend = Blend.SourceColor;
            rs.BlendFunction = BlendFunction.Add;

            // Draw the light map
            _sb.Draw(_lightMap, Vector2.Zero, Color.White);
            _sb.End();

            // Restore the old blend mode
            rs.SourceBlend = oldSourceBlend;
            rs.DestinationBlend = oldDestinationBlend;
            rs.BlendFunction = oldBlendFunction;
        }

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void Update(int currentTime)
        {
            LightManager.Update(currentTime);
        }

        #endregion
    }
}