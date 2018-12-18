using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;

namespace NetGore.Graphics
{
    public class DrawingManager : IDrawingManager
    {
        // FUTURE: !! Add support for a DrawingManager that doesn't need RenderImage

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The <see cref="Color"/> to use when clearing the GUI. This needs to have an alpha of 0 since we will
        /// be copying from the buffer onto the window, and if the world was drawn, we will be copying on top
        /// of the world.
        /// </summary>
        static readonly Color _clearGUIBufferColor = new Color(0, 0, 0, 0);

        readonly SFML.Graphics.Sprite _drawBufferToWindowSprite;
        readonly ILightManager _lightManager;
        readonly IRefractionManager _refractionManager;
        readonly ISpriteBatch _sb;
        readonly View _view = new View();

        RenderTexture _buffer;
        bool _isDisposed;

        /// <summary>
        /// Contains if the last draw was to the world. This way, we can make sure to not draw the GUI twice or world twice
        /// in a row without clearing. True for last drawing being to World, false for GUI.
        /// </summary>
        bool _lastDrawWasToWorld;

        RenderWindow _rw;

        DrawingManagerState _state = DrawingManagerState.Idle;
        ICamera2D _worldCamera;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingManager"/> class.
        /// </summary>
        public DrawingManager() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingManager"/> class.
        /// </summary>
        /// <param name="rw">The <see cref="RenderWindow"/>.</param>
        public DrawingManager(RenderWindow rw)
        {
            BackgroundColor = Color.Black;

            // Create the objects
            _sb = new RoundedSpriteBatch();
            _lightManager = CreateLightManager();
            _refractionManager = CreateRefractionManager();

            // Set up the sprite used to draw the light map
            _drawBufferToWindowSprite = new SFML.Graphics.Sprite
            {
                Color = Color.White,
                Rotation = 0,
                Scale = Vector2.One,
                Origin = Vector2.Zero,
                Position = Vector2.Zero
            };

            // Set the RenderWindow
            RenderWindow = rw;
        }

        /// <summary>
        /// Creates the <see cref="ILightManager"/> to use.
        /// </summary>
        /// <returns>The <see cref="ILightManager"/> to use. Cannot be null.</returns>
        protected virtual ILightManager CreateLightManager()
        {
            return new LightManager();
        }

        /// <summary>
        /// Creates the <see cref="IRefractionManager"/> to use.
        /// </summary>
        /// <returns>The <see cref="IRefractionManager"/> to use. Cannot be null.</returns>
        protected virtual IRefractionManager CreateRefractionManager()
        {
            return new RefractionManager();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (!disposeManaged)
                return;

            if (_lightManager != null)
                _lightManager.Dispose();

            if (_refractionManager != null)
                _refractionManager.Dispose();

            if (_sb != null && !_sb.IsDisposed)
                _sb.Dispose();

            if (_drawBufferToWindowSprite != null && !_drawBufferToWindowSprite.IsDisposed)
                _drawBufferToWindowSprite.Dispose();
        }

        /// <summary>
        /// Draws an <see cref="Texture"/> to the screen.
        /// </summary>
        /// <param name="buffer">The <see cref="Texture"/> to draw.</param>
        /// <param name="blendMode">The <see cref="BlendMode"/> to use.</param>
        void DrawBufferToScreen(Texture buffer, BlendMode blendMode)
        {
            var size = buffer.Size;

            _drawBufferToWindowSprite.Texture = buffer;
            _drawBufferToWindowSprite.TextureRect = new IntRect(0, 0, (int)size.X, (int)size.Y);

            _rw.Draw(_drawBufferToWindowSprite, new RenderStates(blendMode));
        }

        /// <summary>
        /// Gets if the <see cref="RenderWindow"/> is available. If it is not, we cannot draw.
        /// </summary>
        /// <returns>True if the <see cref="RenderWindow"/> is available; otherwise false.</returns>
        protected bool IsRenderWindowAvailable()
        {
            try
            {
                return !_rw.IsDisposed && _rw.IsOpen();
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception ex)
            {
                const string errmsg =
                    "Unexpected exception occured while trying to get the state of RenderWindow `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _rw, ex);
                Debug.Fail(string.Format(errmsg, _rw, ex));
                return false;
            }
        }

        /// <summary>
        /// Allows derived classes to handle when the <see cref="RenderWindow"/> changes.
        /// </summary>
        /// <param name="newRenderWindow">The new <see cref="RenderWindow"/>.</param>
        protected virtual void OnRenderWindowChanged(RenderWindow newRenderWindow)
        {
            if (_sb != null)
                _sb.RenderTarget = newRenderWindow;

            if (LightManager != null)
                LightManager.Initialize(newRenderWindow);

            if (RefractionManager != null)
                RefractionManager.Initialize(newRenderWindow);
        }

        /// <summary>
        /// Ends a SpriteBatch without throwing any <see cref="Exception"/>s.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to end.</param>
        void SafeEndSpriteBatch(ISpriteBatch sb)
        {
            try
            {
                sb.End();
            }
            catch (Exception ex)
            {
                const string errmsg = "Exception occured while calling End() the SpriteBatch `{0}` on `{1}`. Exception: {2}";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, sb, this, ex);
                Debug.Fail(string.Format(errmsg, sb, this, ex));
            }
        }

        #region IDrawingManager Members

        /// <summary>
        /// Gets or sets the background <see cref="Color"/>.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets if this <see cref="IDrawingManager"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

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
        /// Gets or sets the <see cref="RenderWindow"/> to draw to.
        /// </summary>
        public RenderWindow RenderWindow
        {
            get { return _rw; }
            set
            {
                if (_rw == value)
                    return;

                _rw = value;

                _sb.RenderTarget = _rw;

                if (_rw != null)
                {
                    LightManager.Initialize(_rw);
                    RefractionManager.Initialize(_rw);
                }

                OnRenderWindowChanged(_rw);
            }
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
        /// <param name="clearBuffer">When true, the buffer will be cleared before drawing. When false, the contents of the previous
        /// frame will remain in the buffer, only if the last draw was also to the GUI. When the last draw call was to the
        /// world, then this will have no affect. Useful for when you want to draw multiple GUI screens on top of one another.</param>
        /// <returns>
        /// The <see cref="ISpriteBatch"/> to use to draw the GUI, or null if an unexpected
        /// error was encountered when preparing the <see cref="ISpriteBatch"/>. When null, all drawing
        /// should be aborted completely instead of trying to draw with a different <see cref="ISpriteBatch"/>
        /// or manually recovering the error.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.Idle"/>.</exception>
        public ISpriteBatch BeginDrawGUI(bool clearBuffer = true)
        {
            if (State != DrawingManagerState.Idle)
                throw new InvalidOperationException("This method cannot be called while already busy drawing.");

            try
            {
                // Ensure the RenderWindow is available
                if (!IsRenderWindowAvailable())
                {
                    if (log.IsInfoEnabled)
                        log.Info("Skipping BeginDrawGUI() call - the RenderWindow is not available.");
                    _state = DrawingManagerState.Idle;
                    return null;
                }

                if (clearBuffer)
                {
                    // If the last draw was also to the GUI, clear the screen
                    if (!_lastDrawWasToWorld)
                        _rw.Clear(BackgroundColor);
                }

                _lastDrawWasToWorld = false;

                // Ensure the buffer is set up
                _buffer = _rw.CreateBufferRenderTexture(_buffer);
                _sb.RenderTarget = _buffer;

                if (_buffer == null)
                    return null;

                // Always clear the GUI with alpha = 0 since we will be copying it over the screen
                _buffer.Clear(_clearGUIBufferColor);

                // Start up the SpriteBatch
                _sb.Begin(BlendMode.Alpha);

                // Change the state
                _state = DrawingManagerState.DrawingGUI;
            }
            catch (AccessViolationException ex)
            {
                // More frequent and less concerning exception
                const string errmsg = "Failed to start drawing GUI on `{0}`. Device was probably lost. Exception: {1}";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, ex);
                _state = DrawingManagerState.Idle;
                SafeEndSpriteBatch(_sb);
                return null;
            }
            catch (Exception ex)
            {
                // Unexpected exception
                const string errmsg = "Failed to start drawing GUI on `{0}` due to unexpected exception. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));
                _state = DrawingManagerState.Idle;
                SafeEndSpriteBatch(_sb);
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
            if (State != DrawingManagerState.Idle)
                throw new InvalidOperationException("This method cannot be called while already busy drawing.");

            try
            {
                // Ensure the RenderWindow is available
                if (!IsRenderWindowAvailable())
                {
                    if (log.IsInfoEnabled)
                        log.Info("Skipping BeginDrawWorld() call - the RenderWindow is not available.");
                    _state = DrawingManagerState.Idle;
                    return null;
                }

                _worldCamera = camera;

                // No matter what the last draw was, we clear the screen when drawing the world since the world drawing
                // always comes first or not at all (makes no sense to draw the GUI then the world)
                _rw.Clear(BackgroundColor);
                
                _lastDrawWasToWorld = true;

                // Ensure the buffer is set up
                _buffer = _rw.CreateBufferRenderTexture(_buffer);
                _sb.RenderTarget = _buffer;

                if (_buffer == null)
                    return null;

                _buffer.Clear(BackgroundColor);

                // Start up the SpriteBatch
                _sb.Begin(BlendMode.Alpha, camera);

                // Change the state
                _state = DrawingManagerState.DrawingWorld;
            }
            catch (AccessViolationException ex)
            {
                // More frequent and less concerning exception
                const string errmsg = "Failed to start drawing world on `{0}`. Device was probably lost. Exception: {1}";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, ex);
                _state = DrawingManagerState.Idle;
                SafeEndSpriteBatch(_sb);
                return null;
            }
            catch (Exception ex)
            {
                // Unexpected exception
                const string errmsg = "Failed to start drawing world on `{0}` due to unexpected exception. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));
                _state = DrawingManagerState.Idle;
                SafeEndSpriteBatch(_sb);
                return null;
            }

            return _sb;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            Dispose(true);
        }

        /// <summary>
        /// Ends drawing the graphical user interface.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.DrawingGUI"/>.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BeginDrawGUI")]
        public void EndDrawGUI()
        {
            if (State != DrawingManagerState.DrawingGUI)
                throw new InvalidOperationException("This method can only be called after BeginDrawGUI.");

            try
            {
                _state = DrawingManagerState.Idle;

                // Ensure the RenderWindow is available
                if (!IsRenderWindowAvailable())
                {
                    if (log.IsInfoEnabled)
                        log.Info("Skipping EndDrawGUI() call - the RenderWindow is not available.");
                    return;
                }

                // Ensure the buffer is available
                if (_buffer == null || _buffer.IsDisposed)
                {
                    const string errmsg = "Skipping EndDrawWorld() call - the _buffer is not available.";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg);
                    return;
                }

                SafeEndSpriteBatch(_sb);

                // Copy the GUI to the screen
                _buffer.Display();

                DrawBufferToScreen(_buffer.Texture, BlendMode.Alpha);
            }
            catch (AccessViolationException ex)
            {
                // More frequently and less concerning exception
                const string errmsg =
                    "EndDrawGUI failed on `{0}`. Device was probably lost. The GUI will have to skip being drawn this frame. Exception: {1}";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, ex);
            }
            catch (Exception ex)
            {
                // Unexpected exception
                const string errmsg =
                    "EndDrawGUI failed on `{0}` due to unexpected exception. The GUI will have to skip being drawn this frame. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
            }
        }

        /// <summary>
        /// Ends drawing the world.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IDrawingManager.State"/> is not equal to
        /// <see cref="DrawingManagerState.DrawingWorld"/>.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BeginDrawWorld")]
        public void EndDrawWorld()
        {
            if (State != DrawingManagerState.DrawingWorld)
                throw new InvalidOperationException("This method can only be called after BeginDrawWorld.");

            try
            {
                _state = DrawingManagerState.Idle;

                // Ensure the RenderWindow is available
                if (!IsRenderWindowAvailable())
                {
                    if (log.IsInfoEnabled)
                        log.Info("Skipping EndDrawWorld() call - the RenderWindow is not available.");
                    return;
                }

                // Ensure the buffer is available
                if (_buffer == null || _buffer.IsDisposed)
                {
                    const string errmsg = "Skipping EndDrawWorld() call - the _buffer is not available.";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg);
                    return;
                }

                SafeEndSpriteBatch(_sb);

                // Draw the lights
                try
                {
                    if (LightManager.IsEnabled) 
                    {
                        // Copy the lights onto the buffer
                        LightManager.DrawToTarget(_worldCamera, _buffer);
                        _buffer.Display();
                    }
                    else
                    {
                        // Don't have to take any alternate drawing route since lights are just drawn on top of the screen buffer
                    }
                }
                catch (Exception ex)
                {
                    // Do not catch AccessViolationException - let that be handled by the outer block
                    if (ex is AccessViolationException)
                        throw;

                    const string errmsg =
                        "Error on `{0}` while trying to draw the LightManager `{1}`." +
                        " Lights will have to be skipped this frame. Exception: {2}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, LightManager, ex);
                }

                // Have to display the buffer since we will start referencing the texture for it
                _buffer.Display();

                // Draw the refractions
                try
                {
                    if (RefractionManager.IsEnabled)
                    {
                        // Pass the buffer to the refraction manager to draw to the screen
                        RefractionManager.DrawToTarget(_worldCamera, _rw, _buffer.Texture);
                    }
                    else
                    {
                        // Since the RefractionManager won't be handling copying to the screen for us, we will have to draw
                        // to the screen manually
                        DrawBufferToScreen(_buffer.Texture, BlendMode.None);
                    }
                }
                catch (Exception ex)
                {
                    // Do not catch AccessViolationException - let that be handled by the outer block
                    if (ex is AccessViolationException)
                        throw;

                    const string errmsg =
                        "Error on `{0}` while trying to draw the RefractionManager `{1}`." +
                        " Refractions will have to be skipped this frame. Exception: {2}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, RefractionManager, ex);
                }

                _view.Reset(new FloatRect(0, 0, _rw.Size.X, _rw.Size.Y));
                _rw.SetView(_view);
            }
            catch (AccessViolationException ex)
            {
                // More frequently and less concerning exception
                const string errmsg =
                    "EndDrawWorld failed on `{0}`. Device was probably lost. The world will have to skip being drawn this frame. Exception: {1}";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, ex);
            }
            catch (Exception ex)
            {
                // Unexpected exception
                const string errmsg =
                    "EndDrawWorld failed on `{0}` due to unexpected exception. The world will have to skip being drawn this frame. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
            }
        }

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void Update(TickCount currentTime)
        {
            LightManager.Update(currentTime);
            RefractionManager.Update(currentTime);
        }

        #endregion
    }
}