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
    /// Manages multiple <see cref="ILight"/>s and the generation of the light map.
    /// </summary>
    public class LightManager : VirtualList<ILight>, ILightManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Color _ambient;
        Grh _defaultSprite;
        Image _lightMap;
        RenderWindow _rw;
        ISpriteBatch _sb;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (!disposeManaged)
                return;

            if (_lightMap != null && !_lightMap.IsDisposed)
                _lightMap.Dispose();
        }

        /// <summary>
        /// Draws all of the lights in this <see cref="ILightManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <param name="recursionCount">The recursion count. When this number reaches its limit, any recursion
        /// this method may normally do will not be attempted. Should be initially set to 0.</param>
        /// <returns>
        /// The <see cref="Image"/> containing the light map. If the light map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="ILightManager.IsInitialized"/> is false.</exception>
        Image DrawInternal(ICamera2D camera, int recursionCount)
        {
            // Check for too much recursion
            if (++recursionCount > 8)
                return null;

            if (!IsInitialized)
                throw new InvalidOperationException("You must initialize the ILightManager before drawing.");

            // Ensure the light map is ready
            if (!PrepareLightMap())
                return null;

            // Clear the buffer with the ambient light color
            _rw.Clear(Ambient);

            // Draw the lights
            _sb.Begin(BlendMode.Add, camera);

            foreach (var light in this)
            {
                // TODO: Optimize by only drawing lights actually in view
                light.Draw(_sb);
            }

            _sb.End();

            // Copy the screen buffer onto the light map image
            if (!_lightMap.CopyScreen(_rw))
                return null;

            return _lightMap;
        }

        /// <summary>
        /// Ensures the <see cref="_lightMap"/> <see cref="Image"/> is all ready.
        /// </summary>
        /// <returns>True if the light map <see cref="Image"/> was prepared; false if there was some issue in preparing the
        /// <see cref="Image"/> and drawing cannot continue.</returns>
        bool PrepareLightMap()
        {
            // TODO: !! Use WindowHelper.CreateBufferRenderImage();
            // Ensure the light map is created and of the needed size
            var mustRecreateLightMap = false;
            try
            {
                if (_lightMap == null || _lightMap.IsDisposed || _lightMap.Width != _rw.Width || _lightMap.Height != _rw.Height)
                    mustRecreateLightMap = true;
            }
            catch (InvalidOperationException)
            {
                mustRecreateLightMap = true;
            }

            // TODO: !! Use a RenderImage to draw the lights onto instead of drawing to the screen then copying back

            // Create the light map Image if needed
            if (mustRecreateLightMap)
            {
                // If there is an old Image, make sure to dispose of it... or at least try to
                if (_lightMap != null)
                {
                    try
                    {
                        if (!_lightMap.IsDisposed)
                            _lightMap.Dispose();
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
                        "Failed to create light map Image - failed to get RenderWindow width/height. Will attempt again next frame. Exception: {0}";
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
                            "Unable to recreate LightMap - invalid Width/Height ({0},{1}) returned from RenderWindow. Most likely, the form was minimized.",
                            width, height);
                    }
                    return false;
                }

                // Create the new Image
                try
                {
                    _lightMap = new Image(_rw.Width, _rw.Height) { Smooth = false };
                }
                catch (LoadingFailedException ex)
                {
                    const string errmsg =
                        "Failed to create light map Image - construction of Image failed. Will attempt again next frame. Exception: {0}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);
                    return false;
                }
            }

            return true;
        }

        #region ILightManager Members

        /// <summary>
        /// Gets or sets the ambient light color. The alpha value has no affect and will always be set to 255.
        /// </summary>
        public Color Ambient
        {
            get { return _ambient; }
            set { _ambient = new Color(value.R, value.G, value.B, 255); }
        }

        /// <summary>
        /// Gets or sets the default sprite to use for all lights added to this <see cref="ILightManager"/>.
        /// When this value changes, all <see cref="ILight"/>s in this <see cref="ILightManager"/> who's
        /// <see cref="ILight.Sprite"/> is equal to the old value will have their sprite set to the new value.
        /// </summary>
        public Grh DefaultSprite
        {
            get { return _defaultSprite; }
            set
            {
                if (_defaultSprite == value)
                    return;

                var oldValue = _defaultSprite;
                _defaultSprite = value;

                foreach (var light in this)
                {
                    if (light.Sprite == oldValue)
                        light.Sprite = _defaultSprite;
                }
            }
        }

        /// <summary>
        /// Gets if the <see cref="ILightManager"/> has been initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return _rw != null; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public override void Add(ILight item)
        {
            if (item.Sprite == null)
                item.Sprite = DefaultSprite;

            if (!Contains(item))
                base.Add(item);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Draws all of the lights in this <see cref="ILightManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Image"/> containing the light map. If the light map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="ILightManager.IsInitialized"/> is false.</exception>
        public Image Draw(ICamera2D camera)
        {
            return DrawInternal(camera, 0);
        }

        /// <summary>
        /// Initializes the <see cref="ILightManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="ILight"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="renderWindow">The <see cref="RenderWindow"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="renderWindow"/> is null.</exception>
        public void Initialize(RenderWindow renderWindow)
        {
            if (renderWindow == null)
                throw new ArgumentNullException("renderWindow");

            if (_lightMap != null && !_lightMap.IsDisposed)
                _lightMap.Dispose();

            if (_sb != null && !_sb.IsDisposed)
                _sb.Dispose();

            _rw = renderWindow;

            _sb = new RoundedSpriteBatch(renderWindow);
        }

        /// <summary>
        /// Updates all of the lights in this <see cref="ILightManager"/>, along with the <see cref="ILightManager"/> itself.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void Update(TickCount currentTime)
        {
            foreach (var light in this)
            {
                light.Update(currentTime);
            }
        }

        #endregion
    }
}