using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics
{
    /// <summary>
    /// Base class for managing rendering multiple sprites to an off-screen buffer, then using that off-screen
    /// buffer to draw to the screen.
    /// </summary>
    public abstract class OffscreenRenderBufferManagerBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Color _defaultColor = new Color(0, 0, 0, 255);

        /// <summary>
        /// A <see cref="Sprite"/> used for drawing the buffer to a target.
        /// </summary>
        readonly SFML.Graphics.Sprite _drawToTargetSprite;

        /// <summary>
        /// A <see cref="ISpriteBatch"/> instance used by the <see cref="OffscreenRenderBufferManagerBase"/> for drawing
        /// the sprites onto the buffer.
        /// </summary>
        readonly ISpriteBatch _sb;

        readonly RenderStates _renderStates = RenderStates.Default;

        Color _bufferClearColor = _defaultColor;
        bool _isDisposed;
        RenderTexture _ri;
        Window _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="OffscreenRenderBufferManagerBase"/> class.
        /// </summary>
        protected OffscreenRenderBufferManagerBase()
        {
            _sb = CreateSpriteBatch();

            _drawToTargetSprite = new SFML.Graphics.Sprite
            {
                Color = Color.White,
                Rotation = 0,
                Scale = Vector2.One,
                Origin = Vector2.Zero,
                Position = Vector2.Zero
            };
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to use when clearing the buffer.
        /// </summary>
        [DefaultValue(typeof(Color), "{0, 0, 0, 255}")]
        protected Color BufferClearColor
        {
            get { return _bufferClearColor; }
            set { _bufferClearColor = value; }
        }

        /// <summary>
        /// Gets or sets if the buffer clearing should be bypassed. If the buffer is recreated, it will
        /// have to be cleared anyways. By default, this is false.
        /// </summary>
        [DefaultValue(false)]
        protected bool BypassClear { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Shader"/> to use when drawing the buffer to a <see cref="RenderTarget"/>.
        /// </summary>
        [DefaultValue(null)]
        public virtual Shader DrawToTargetShader { get; set; }

        /// <summary>
        /// Gets if this object has been initialized. If false, <see cref="InitializeRenderBuffer"/> must be called before drawing.
        /// </summary>
        protected bool IsBufferInitialized
        {
            get { return _window != null; }
        }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Creates the <see cref="ISpriteBatch"/> instance to use for drawing sprites to the buffer.
        /// </summary>
        /// <returns>The <see cref="ISpriteBatch"/> instance to use for drawing sprites to the buffer.</returns>
        protected virtual ISpriteBatch CreateSpriteBatch()
        {
            return new RoundedSpriteBatch(null);
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
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (!disposeManaged)
                return;

            // Dispose the RenderImage
            try
            {
                if (_ri != null && !_ri.IsDisposed)
                    _ri.Dispose();
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to dispose `{0}` in `{1}`. Exception: {2}";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, _ri, this, ex);
                Debug.Fail(string.Format(errmsg, _ri, this, ex));
            }

            // Dispose the SpriteBatch
            try
            {
                if (_sb != null && !_sb.IsDisposed)
                    _sb.Dispose();
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to dispose `{0}` in `{1}`. Exception: {2}";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, _ri, this, ex);
                Debug.Fail(string.Format(errmsg, _ri, this, ex));
            }
        }

        /// <summary>
        /// Gets the buffer then draws it to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="target">The <see cref="RenderTarget"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> to use when drawing.</param>
        /// <returns>True if the buffer was successfully drawn to the <paramref name="target"/>; otherwise false.</returns>
        protected bool DrawBufferToTarget(RenderTarget target, ICamera2D camera)
        {
            // Get the buffer
            var bufferImage = GetBuffer(camera);
            if (bufferImage == null)
                return false;

            // Set up the sprite
            _drawToTargetSprite.Texture = bufferImage;
            _drawToTargetSprite.Position = target.MapPixelToCoords(Vector2.Zero).Round();

            var bufferImageSize = bufferImage.Size;
            _drawToTargetSprite.TextureRect = new IntRect(0, 0, (int)bufferImageSize.X, (int)bufferImageSize.Y);

            // Reset RenderStates
            _renderStates.Shader = DrawToTargetShader;
            _renderStates.BlendMode = BlendMode.Alpha;
            _renderStates.Transform = Transform.Identity;
            _renderStates.Texture = null;

            // Draw to the target
            HandleDrawBufferToTarget(bufferImage, _drawToTargetSprite, target, camera, _renderStates);

            return true;
        }

        /// <summary>
        /// Throws an exception if <see cref="IsBufferInitialized"/> is false.
        /// </summary>
        /// <exception cref="InvalidOperationException">Cannot draw to or access the internal buffer when not initialized.</exception>
        void EnsureInitialized()
        {
            if (!IsBufferInitialized)
                throw new InvalidOperationException("Cannot draw to or access the internal buffer when not initialized.");
        }

        /// <summary>
        /// Gets the internal buffer and returns the <see cref="Texture"/> for it after drawing it.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to use when drawing.</param>
        /// <returns>
        /// The <see cref="Texture"/> for the internal offscreen buffer, or null if the buffer failed to be drawn.
        /// </returns>
        protected Texture GetBuffer(ICamera2D camera)
        {
            EnsureInitialized();

            try
            {
                // Get the RenderTarget
                var rt = GetRenderImage();
                _sb.RenderTarget = rt;

                if (rt == null)
                    return null;

                // Draw the buffer using the user code
                try
                {
                    if (!HandleDrawBuffer(rt, _sb, camera))
                        return null;
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to draw to offscreen buffer in `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, ex);
                    Debug.Fail(string.Format(errmsg, this, ex));
                    return null;
                }

                // Prepare and return the image
                rt.Display();
                return rt.Texture;
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to create and/or draw offscreen buffer in `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));
                return null;
            }
        }

        /// <summary>
        /// Makes sure the internal buffer is ready then returns a <see cref="RenderTexture"/> to draw to it.
        /// </summary>
        /// <returns>The <see cref="RenderTexture"/> to use, or null if the buffer could not be prepared.</returns>
        RenderTexture GetRenderImage()
        {
            var oldRI = _ri;
            _ri = _window.CreateBufferRenderTexture(_ri);

            if (_ri == null)
                return null;

            if (!BypassClear || _ri != oldRI)
                _ri.Clear(BufferClearColor);

            return _ri;
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the buffer.
        /// </summary>
        /// <param name="rt">The <see cref="RenderTarget"/> to draw to.</param>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw to the <paramref name="rt"/>. The derived class
        /// is required to handle making Begin()/End() calls on it.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> to use when drawing.</param>
        /// <returns>True if the drawing was successful; false if there were any errors while drawing.</returns>
        protected abstract bool HandleDrawBuffer(RenderTarget rt, ISpriteBatch sb, ICamera2D camera);

        /// <summary>
        /// Handles the actual drawing of the buffer to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="Texture"/> of the buffer that is to be drawn to the <paramref name="target"/>.</param>
        /// <param name="sprite">The <see cref="SFML.Graphics.Sprite"/> set up to draw the <paramref name="buffer"/>.</param>
        /// <param name="target">The <see cref="RenderTarget"/> to draw the <paramref name="buffer"/> to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that was used during the creation of the buffer.</param>
        protected virtual void HandleDrawBufferToTarget(Texture buffer, SFML.Graphics.Sprite sprite, RenderTarget target, ICamera2D camera, RenderStates renderStates)
        {
            target.Draw(sprite, renderStates);
        }

        /// <summary>
        /// Initializes the internal components of the buffer. This only needs to be called once.
        /// </summary>
        /// <param name="window">The <see cref="Window"/> to use.</param>
        protected void InitializeRenderBuffer(Window window)
        {
            if (_window == window)
                return;

            // Set the new Window
            _window = window;

            // Force the buffer to be rebuilt
            if (_ri != null)
            {
                // Dispose of the old buffer
                try
                {
                    if (!_ri.IsDisposed)
                        _ri.Display();
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to dispose RenderImage `{0}`. Exception: {1}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, _ri, ex);
                    Debug.Fail(string.Format(errmsg, _ri, ex));
                }

                // Set to null to force rebuild when needed
                _ri = null;
            }
        }

        /// <summary>
        /// Prepares the <see cref="SFML.Graphics.Sprite"/> used to draw to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="sprite">The <see cref="SFML.Graphics.Sprite"/> to prepare.</param>
        /// <param name="target">The <see cref="RenderTarget"/> begin drawn to.</param>
        protected virtual void PrepareDrawToTargetSprite(SFML.Graphics.Sprite sprite, RenderTarget target)
        {
        }
    }
}