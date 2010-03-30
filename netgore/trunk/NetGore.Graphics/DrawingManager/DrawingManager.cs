using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    public class DrawingManager : IDrawingManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly GraphicsDevice _gd;
        readonly ILightManager _lightManager;
        readonly ISpriteBatch _sb;

        Texture2D _lightMap;
        DrawingManagerState _state = DrawingManagerState.Idle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingManager"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        public DrawingManager(GraphicsDevice graphicsDevice)
        {
            ClearColor = Color.CornflowerBlue;

            _gd = graphicsDevice;
            _sb = new RoundedXnaSpriteBatch(_gd);

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
        /// Gets or sets the <see cref="Color"/> to use when clearing the screen.
        /// </summary>
        public Color ClearColor { get; set; }

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
        /// <returns>
        /// The <see cref="ISpriteBatch"/> to use to draw the GUI, or null if an unexpected
        /// error was encountered when preparing the <see cref="ISpriteBatch"/>. When null, all drawing
        /// should be aborted completely instead of trying to draw with a different <see cref="ISpriteBatch"/>
        /// or manually recovering the error.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        public ISpriteBatch BeginDrawGUI()
        {
            if (State != DrawingManagerState.Idle)
                throw new InvalidOperationException("This method cannot be called while already busy drawing.");

            _state = DrawingManagerState.DrawingGUI;

            try
            {
                _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            }
            catch (InvalidOperationException ex)
            {
                // Failed to Begin() the SpriteBatch - abort!
                const string errmsg = "SpriteBatch.Begin() failed. Aborting drawing. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);

                _state = DrawingManagerState.Idle;
                return null;
            }

            return _sb;
        }

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
        public ISpriteBatch BeginDrawWorld(ICamera2D camera)
        {
            return BeginDrawWorld(camera, true, false);
        }

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
        public ISpriteBatch BeginDrawWorld(ICamera2D camera, bool useLighting, bool bypassClear)
        {
            if (State != DrawingManagerState.Idle)
                throw new InvalidOperationException("This method cannot be called while already busy drawing.");

            _state = DrawingManagerState.DrawingWorld;

            _lightMap = null;

            try
            {
                // If using lighting, grab the light map
                if (useLighting)
                {
                    try
                    {
                        _lightMap = _lightManager.Draw(camera);
                    }
                    catch (InvalidOperationException ex)
                    {
                        const string errmsg = "Failed to create light map. Exception: {0}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, ex);
                    }
                }

                // Clear the buffer
                if (!bypassClear)
                    _gd.Clear(ClearColor);

                // Start the SpriteBatch
                try
                {
                    _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.Matrix);
                }
                catch (InvalidOperationException ex)
                {
                    // Failed to Begin() the SpriteBatch - abort!
                    const string errmsg = "SpriteBatch.Begin() failed. Aborting drawing. Exception: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);

                    _state = DrawingManagerState.Idle;
                    return null;
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to properly begin drawing due to an unhandled exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));

                _state = DrawingManagerState.Idle;
                return null;
            }

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

            try
            {
                _sb.End();
            }
            catch (InvalidOperationException ex)
            {
                const string errmsg = "SpriteBatch.End() failed. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                return;
            }
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

            try
            {
                try
                {
                    _sb.End();
                }
                catch (InvalidOperationException ex)
                {
                    const string errmsg = "SpriteBatch.End() failed. Exception: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                    return;
                }

                // We only have to go through all these extra steps if we are using a light map.
                if (_lightMap != null)
                {
                    var rs = _gd.RenderState;

                    // Store the old state values
                    var oldSourceBlend = rs.SourceBlend;
                    var oldDestinationBlend = rs.DestinationBlend;
                    var oldBlendFunction = rs.BlendFunction;

                    // Start drawing
                    try
                    {
                        _sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                    }
                    catch (InvalidOperationException ex)
                    {
                        const string errmsg = "SpriteBatch.Begin() failed. Exception: {0}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, ex);
                        return;
                    }

                    // Set the blending mode
                    rs.SourceBlend = Blend.Zero;
                    rs.DestinationBlend = Blend.SourceColor;
                    rs.BlendFunction = BlendFunction.Add;

                    try
                    {
                        // Draw the light map
                        _sb.Draw(_lightMap, Vector2.Zero, Color.White);
                    }
                    finally
                    {
                        _sb.End();

                        // Restore the old blend mode
                        rs.SourceBlend = oldSourceBlend;
                        rs.DestinationBlend = oldDestinationBlend;
                        rs.BlendFunction = oldBlendFunction;
                    }
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to properly end drawing due to an unhandled exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
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