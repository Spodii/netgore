using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Manages the preparation and tear-down for drawing.
    /// </summary>
    public class DrawingManager : IDrawingManager
    {

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Cache of <see cref="RenderImage.IsAvailable"/>. If false, we have to change the behavior a bit to render without
        /// using a <see cref="RenderImage"/>.
        /// </summary>
        static readonly bool _supportsRenderImage;

        readonly ILightManager _lightManager;
        readonly IRefractionManager _refractionManager;
        readonly SFML.Graphics.Sprite _lightMapSprite;
        readonly RenderWindow _rw;
        readonly ISpriteBatch _sb;

        Image _lightMap;
        DrawingManagerState _state = DrawingManagerState.Idle;

        /// <summary>
        /// Initializes the <see cref="DrawingManager"/> class.
        /// </summary>
        static DrawingManager()
        {
            // TODO: !! Add support for a DrawingManager that doesn't need RenderImage
            _supportsRenderImage = RenderImage.IsAvailable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingManager"/> class.
        /// </summary>
        /// <param name="rw">The <see cref="RenderWindow"/>.</param>
        public DrawingManager(RenderWindow rw)
        {
            ClearColor = new Color(100, 149, 237);

            _rw = rw;
            _sb = new RoundedSpriteBatch(_rw);

            // Set up the sprite used to draw the light map
            _lightMapSprite = new SFML.Graphics.Sprite
            { BlendMode = BlendMode.Multiply, Color = Color.White, Rotation = 0, Scale = Vector2.One, Origin = Vector2.Zero };

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _lightManager = CreateLightManager();
            _refractionManager = CreateRefractionManager();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            if (_lightManager != null)
                _lightManager.Initialize(_rw);

            if (_refractionManager != null)
                _refractionManager.Initialize(_rw);
        }

        /// <summary>
        /// Creates the <see cref="ILightManager"/> to use.
        /// </summary>
        /// <returns>The <see cref="ILightManager"/> to use.</returns>
        protected virtual ILightManager CreateLightManager()
        {
            return new LightManager();
        }

        /// <summary>
        /// Creates the <see cref="IRefractionManager"/> to use.
        /// </summary>
        /// <returns>The <see cref="IRefractionManager"/> to use.</returns>
        protected virtual IRefractionManager CreateRefractionManager()
        {
            return new RefractionManager();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to
        /// release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (!disposeManaged)
                return;

            GC.SuppressFinalize(this);

            if (_sb != null)
                _sb.Dispose();

            if (_lightManager != null)
                _lightManager.Dispose();

            if (_lightMapSprite != null)
                _lightMapSprite.Dispose();

            if (_refractionManager != null)
                _refractionManager.Dispose();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DrawingManager"/> is reclaimed by garbage collection.
        /// </summary>
        ~DrawingManager()
        {
            Dispose(false);
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
        /// Gets the <see cref="IRefractionManager"/> used by this <see cref="IDrawingManager"/>.
        /// </summary>
        public IRefractionManager RefractionManager
        {
            get { return _refractionManager; }
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
                _sb.Begin(BlendMode.Alpha);
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
                    _rw.Clear(ClearColor);

                // Start the SpriteBatch
                try
                {
                    _sb.Begin(BlendMode.Alpha, camera);
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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

            try
            {
                // We only have to go through all these extra steps if we are using a light map.
                if (_lightMap != null)
                {
                    // Draw the light map onto the screen
                    _lightMapSprite.Image = _lightMap;
                    _lightMapSprite.Width = _rw.CurrentView.Size.X;
                    _lightMapSprite.Height = _rw.CurrentView.Size.Y;
                    _lightMapSprite.Position = _rw.ConvertCoords(0, 0).Round();

                    _sb.Draw(_lightMapSprite);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to properly end drawing due to an unhandled exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
            finally
            {
                _sb.End();
            }
        }

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void Update(TickCount currentTime)
        {
            LightManager.Update(currentTime);
        }

        #endregion
    }
}