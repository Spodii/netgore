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
            _lightManager.Initialize(_gd);
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
        public void EndDrawingGUI()
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
            if (State != DrawingManagerState.DrawingGUI)
                throw new InvalidOperationException("This method can only be called after BeginDrawWorld.");

            _state = DrawingManagerState.Idle;

            _sb.End();

            _sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            _gd.RenderState.SourceBlend = Blend.Zero;
            _gd.RenderState.DestinationBlend = Blend.SourceColor;
            _gd.RenderState.BlendFunction = BlendFunction.Add;
            _sb.Draw(_lightMap, Vector2.Zero, Color.White);
            _sb.End();
        }

        #endregion
    }
}