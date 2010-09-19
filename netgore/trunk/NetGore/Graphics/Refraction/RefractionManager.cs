using System;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using SFML;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Manages multiple <see cref="IRefractionEffect"/>s and the generation of the refraction map.
    /// </summary>
    public class RefractionManager : VirtualList<IRefractionEffect>, IRefractionManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The default refraction map requires be cleared with the RGB channels at 127 since that is the color used
        /// to indicate to the shader that no refraction
        /// </summary>
        static readonly Color _defaultClearColor = new Color(127, 127, 127, 0);

        Color _clearColor = _defaultClearColor;
        bool _isDisposed;
        bool _isEnabled;

        Image _refractionMap;
        RenderWindow _rw;
        ISpriteBatch _sb;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefractionManager"/> class.
        /// </summary>
        public RefractionManager()
        {
            // Try to enable
            IsEnabled = true;
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to use when clearing the refraction map.
        /// </summary>
        public Color ClearColor
        {
            get { return _clearColor; }
            set { _clearColor = value; }
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

            if (_refractionMap != null && !_refractionMap.IsDisposed)
                _refractionMap.Dispose();
        }

        /// <summary>
        /// Draws all of the lights in this <see cref="IRefractionManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <param name="recursionCount">The recursion count. When this number reaches its limit, any recursion
        /// this method may normally do will not be attempted. Should be initially set to 0.</param>
        /// <returns>
        /// The <see cref="Image"/> containing the refraction map. If the refraction map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IRefractionManager.IsInitialized"/> is false.</exception>
        Image DrawInternal(ICamera2D camera, int recursionCount)
        {
            // Check for too much recursion
            if (++recursionCount > 8)
                return null;

            if (!IsInitialized)
                throw new InvalidOperationException("You must initialize the IRefractionManager before drawing.");

            // Ensure the light map is ready
            if (!PrepareRefractionMap())
                return null;

            // Clear the buffer
            _rw.Clear(ClearColor);

            // Draw the lights
            _sb.Begin(BlendMode.Add, camera);

            foreach (var effect in this)
            {
                // TODO: Optimize by only drawing refraction effects actually in view
                effect.Draw(_sb);
            }

            _sb.End();

            // Copy the screen buffer onto the light map image
            if (!_refractionMap.CopyScreen(_rw))
                return null;

            return _refractionMap;
        }

        /// <summary>
        /// Ensures the <see cref="_refractionMap"/> <see cref="Image"/> is all ready.
        /// </summary>
        /// <returns>True if the refraction map <see cref="Image"/> was prepared; false if there was some issue in preparing the
        /// <see cref="Image"/> and drawing cannot continue.</returns>
        bool PrepareRefractionMap()
        {
            // Ensure the refraction map is created and of the needed size
            var mustRecreateRefractionMap = false;
            try
            {
                if (_refractionMap == null || _refractionMap.IsDisposed || _refractionMap.Width != _rw.Width ||
                    _refractionMap.Height != _rw.Height)
                    mustRecreateRefractionMap = true;
            }
            catch (InvalidOperationException)
            {
                mustRecreateRefractionMap = true;
            }

            // TODO: !! Use a RenderImage to draw the refraction noise onto instead of drawing to the screen then copying back

            // Create the light map Image if needed
            if (mustRecreateRefractionMap)
            {
                // If there is an old Image, make sure to dispose of it... or at least try to
                if (_refractionMap != null)
                {
                    try
                    {
                        if (!_refractionMap.IsDisposed)
                            _refractionMap.Dispose();
                    }
                    catch (InvalidOperationException)
                    {
                        // Ignore failure to dispose
                    }
                }

                // Get the size to make the light map (same size of the window)
                int width;
                int height;
                try
                {
                    width = (int)_rw.Width;
                    height = (int)_rw.Height;
                }
                catch (InvalidOperationException ex)
                {
                    const string errmsg =
                        "Failed to create refraction map Image - failed to get RenderWindow width/height. Will attempt again next frame. Exception: {0}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);
                    return false;
                }

                // Check for a valid RenderWindow size. These can be 0 when the RenderWindow has been minimized.
                if (width <= 0 || height <= 0)
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat(
                            "Unable to recreate refraction map - invalid Width/Height ({0},{1}) returned from RenderWindow. Most likely, the form was minimized.",
                            width, height);
                    }
                    return false;
                }

                // Create the new Image
                try
                {
                    _refractionMap = new Image(_rw.Width, _rw.Height) { Smooth = false };
                }
                catch (LoadingFailedException ex)
                {
                    const string errmsg =
                        "Failed to create refraction map Image - construction of Image failed. Will attempt again next frame. Exception: {0}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);
                    return false;
                }
            }

            return true;
        }

        #region IRefractionManager Members

        /// <summary>
        /// Gets or sets if this <see cref="IRefractionManager"/> is enabled.
        /// If <see cref="Shader.IsAvailable"/> is false, this will always be false.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                    return;

                // When going from disabled to enabled, make sure that shaders are supported
                if (value && !Shader.IsAvailable)
                {
                    const string errmsg = "Cannot enable IRefractionMap since Shader.IsAvailable returned false.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg);
                    return;
                }

                _isEnabled = value;
            }
        }

        /// <summary>
        /// Gets if the <see cref="IRefractionManager"/> has been initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return _rw != null; }
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
        /// Draws all of the reflection effects in this <see cref="IRefractionManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Image"/> containing the reflection map. If the reflection map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IRefractionManager.IsInitialized"/> is false.</exception>
        public Image Draw(ICamera2D camera)
        {
            return DrawInternal(camera, 0);
        }

        /// <summary>
        /// Initializes the <see cref="IRefractionManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="IRefractionEffect"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="renderWindow">The <see cref="RenderWindow"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="renderWindow"/> is null.</exception>
        public void Initialize(RenderWindow renderWindow)
        {
            if (renderWindow == null)
                throw new ArgumentNullException("renderWindow");

            if (_refractionMap != null && !_refractionMap.IsDisposed)
                _refractionMap.Dispose();

            if (_sb != null && !_sb.IsDisposed)
                _sb.Dispose();

            _rw = renderWindow;

            _sb = new RoundedSpriteBatch(renderWindow);
        }

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void Update(TickCount currentTime)
        {
            foreach (var fx in this)
            {
                fx.Update(currentTime);
            }
        }

        #endregion
    }
}