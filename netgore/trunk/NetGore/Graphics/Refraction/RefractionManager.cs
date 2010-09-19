using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics
{
    /// <summary>
    /// Manages multiple <see cref="IRefractionEffect"/>s and the generation of the refraction map.
    /// </summary>
    public class RefractionManager : VirtualList<IRefractionEffect>, IRefractionManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // TODO: !! Use RenderImage.IsAvailable to see if we can even use a RefractionManager

        /// <summary>
        /// The default refraction map requires be cleared with the RGB channels at 127 since that is the color used
        /// to indicate to the shader that no refraction
        /// </summary>
        static readonly Color _defaultClearColor = new Color(127, 127, 127, 0);

        static readonly Shader _defaultShader;

        /// <summary>
        /// A <see cref="Sprite"/> instance used by the <see cref="RefractionManager"/> for drawing the
        /// refraction map to a target <see cref="RenderTarget"/>.
        /// </summary>
        readonly SFML.Graphics.Sprite _sprite;

        Color _clearColor = _defaultClearColor;
        bool _isDisposed;
        bool _isEnabled;
        RenderImage _ri;

        /// <summary>
        /// A <see cref="ISpriteBatch"/> instance used by the <see cref="RefractionManager"/> for drawing
        /// the refraction sprites onto the <see cref="_ri"/>.
        /// </summary>
        ISpriteBatch _sb;

        Shader _shader;
        Window _window;

        /// <summary>
        /// Initializes the <see cref="RefractionManager"/> class.
        /// </summary>
        static RefractionManager()
        {
            // Check if shaders are supported
            if (!Shader.IsAvailable)
                return;

            const string defaultShaderCode =
                @"
/*
	This effect uses the R, G, and A channels to perform a reflection distortion. Color channels are used in the following ways:
		R: The amount to translate on the X axis (< BaseOffset for right, > BaseOffset for left).
		G: The amount to translate on the Y axis (< BaseOffset for down, > BaseOffset for up).
		A: The alpha value of the reflected image. 0.0 shows nothing, while 1.0 shows only the reflection.
*/

// The distance multiplier to apply to the values unpacked from channels to get the offset. This decreases our resolution,
// giving us a choppier image, but increases our range. Lower values give higher resolution but require smaller distances.
// This MUST be the same in all the refraction effects!
const float DistanceMultiplier = 2.0;

// The value of the reflection channels that will be used to not perform any reflection. Having this non-zero allows us to
// reflect in both directions instead of just borrowing pixels in one direction. Of course, this also halves our max distance.
// Logically, this value is 1/2. However, a slightly different number is used due to the behavior of floating-point numbers.
// This MUST be the same in all the refraction effects!
const float BaseOffset = 0.4981;

// The texture that contains the colors to use. Typically, a copy of the screen to be distorted.
uniform sampler2D ColorMap;

// The texture containing the noise that will be used to distort the ColorMap.
uniform sampler2D NoiseMap;

void main (void)
{
	vec4 noiseVec;
	vec4 colorReflected;
	vec4 colorOriginal;

	// Get the noise vector from the noise map.
	noiseVec = texture2D(NoiseMap, gl_TexCoord[0].st).rgba;

	// Get the original texel color, which is just the texel at the ColorMap at the same position as the NoiseMap.
	colorOriginal = texture2D(ColorMap, gl_TexCoord[0].st);

	// Using the noise vector to offset the position, find the corresponding reflected color on the color map.
	colorReflected = texture2D(ColorMap, gl_TexCoord[0].st + ((noiseVec.xy - BaseOffset) * DistanceMultiplier));

	// Mix the reflected and original texels together by the alpha of the noise vector to get the final pixel color.
	gl_FragColor = mix(colorOriginal, colorReflected, noiseVec.a);
}";
            // Try to create the default shader
            try
            {
                _defaultShader = ShaderHelper.LoadFromMemory(defaultShaderCode);
            }
            catch (LoadingFailedException ex)
            {
                const string errmsg = "Failed to load the default Shader for the RefractionManager. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefractionManager"/> class.
        /// </summary>
        public RefractionManager()
        {
            Shader = DefaultShader;

            // Try to enable
            IsEnabled = true;

            _sprite = new SFML.Graphics.Sprite
            { BlendMode = BlendMode.None, Color = Color.White, Position = Vector2.Zero, Scale = Vector2.One, Rotation = 0 };
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
        /// Gets the default <see cref="Shader"/> used by the <see cref="RefractionManager"/> for drawing the refraction map.
        /// </summary>
        public static Shader DefaultShader
        {
            get { return _defaultShader; }
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

            if (_ri != null && !_ri.IsDisposed)
                _ri.Dispose();

            if (_sprite != null && !_sprite.IsDisposed)
                _sprite.Dispose();
        }

        /// <summary>
        /// Draws all of the refraction effects in this <see cref="IRefractionManager"/>.
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

            try
            {
                // Ensure the light map is ready
                if (!PrepareRefractionMap())
                    return null;

                // Clear the buffer
                _ri.Clear(ClearColor);

                // Set up the SpriteBatch and draw the refraction effects
                _sb.Begin(BlendMode.Add, camera);
                _sb.RenderTarget = _ri;

                try
                {
                    foreach (var effect in this)
                    {
                        // TODO: Optimize by only drawing refraction effects actually in view
                        effect.Draw(_sb);
                    }
                }
                catch (Exception ex)
                {
                    const string errmsg = "Error while drawing IRefractionEffect for RefractionManager `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, ex);
                    Debug.Fail(string.Format(errmsg, this, ex));
                    return null;
                }
                finally
                {
                    _sb.End();
                }

                // Finalize the RenderImage
                _ri.Display();
            }
            catch (Exception ex)
            {
                const string errmsg = "Error while generating the refraction map for RefractionManager `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));
                return null;
            }

            return _ri.Image;
        }

        /// <summary>
        /// Ensures the <see cref="_ri"/> <see cref="Image"/> is all ready.
        /// </summary>
        /// <returns>True if the refraction map <see cref="Image"/> was prepared; false if there was some issue in preparing the
        /// <see cref="Image"/> and drawing cannot continue.</returns>
        bool PrepareRefractionMap()
        {
            // TODO: !! Use WindowHelper.CreateBufferRenderImage();
            // Ensure the refraction map is created and of the needed size
            var mustRecreateRefractionMap = false;
            try
            {
                if (_ri == null || _ri.IsDisposed || _ri.Width != _window.Width || _ri.Height != _window.Height)
                    mustRecreateRefractionMap = true;
            }
            catch (InvalidOperationException)
            {
                mustRecreateRefractionMap = true;
            }

            // Create the light map Image if needed
            if (mustRecreateRefractionMap)
            {
                // If there is an old Image, make sure to dispose of it... or at least try to
                if (_ri != null)
                {
                    try
                    {
                        if (!_ri.IsDisposed)
                            _ri.Dispose();
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
                    width = (int)_window.Width;
                    height = (int)_window.Height;
                }
                catch (InvalidOperationException ex)
                {
                    const string errmsg =
                        "Failed to create refraction map Image - failed to get Window width/height. Will attempt again next frame. Exception: {0}";
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
                            "Unable to recreate refraction map - invalid Width/Height ({0},{1}) returned from Window. Most likely, the form was minimized.",
                            width, height);
                    }
                    return false;
                }

                // Create the new Image
                try
                {
                    _ri = new RenderImage(_window.Width, _window.Height);
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

            // Update the sprite and SpriteBatch
            _sb.RenderTarget = _ri;
            _sprite.Width = _ri.Width;
            _sprite.Height = _ri.Height;

            return true;
        }

        #region IRefractionManager Members

        /// <summary>
        /// Gets or sets if this <see cref="IRefractionManager"/> is enabled.
        /// If <see cref="SFML.Graphics.Shader.IsAvailable"/> is false, this will always be false.
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
            get { return _window != null; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Shader"/> to use to draw the refraction map.
        /// </summary>
        public Shader Shader
        {
            get { return _shader; }
            set { _shader = value; }
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
        public virtual Image Draw(ICamera2D camera)
        {
            return DrawInternal(camera, 0);
        }

        /// <summary>
        /// Draws the refraction map to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <param name="target">The <see cref="RenderTarget"/> to draw the refraction map to.</param>
        /// <param name="refractionMap">The refraction map to draw. If null, one will be generated.</param>
        public virtual void DrawToTarget(ICamera2D camera, RenderTarget target, Image refractionMap = null)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("You must initialize the IRefractionManager before drawing.");

            // If no refraction map image provided, generate it
            if (refractionMap == null)
            {
                refractionMap = Draw(camera);

                // Check if it was properly generated
                if (refractionMap == null)
                    return;
            }

            // Draw to the target
            _sprite.Image = refractionMap;
            target.Draw(_sprite, Shader);
        }

        /// <summary>
        /// Initializes the <see cref="IRefractionManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="IRefractionEffect"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is null.</exception>
        public virtual void Initialize(Window window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (_ri != null && !_ri.IsDisposed)
                _ri.Dispose();

            if (_sb != null && !_sb.IsDisposed)
                _sb.Dispose();

            _window = window;

            _sb = new RoundedSpriteBatch(null);
        }

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public virtual void Update(TickCount currentTime)
        {
            foreach (var fx in this)
            {
                fx.Update(currentTime);
            }
        }

        #endregion
    }
}