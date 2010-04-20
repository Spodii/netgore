using System;
using System.Linq;
using NetGore.Collections;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Manages multiple <see cref="ILight"/>s.
    /// </summary>
    public class LightManager : VirtualList<ILight>, ILightManager
    {
        Color _ambient;
        Grh _defaultSprite;
        Image _lightMap;
        RenderWindow _rw;
        ISpriteBatch _sb;

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

            // Ensure the light map is created and of the needed size
            if (_lightMap == null || _lightMap.IsDisposed() || _lightMap.Width != _rw.Width || _lightMap.Height != _rw.Height)
            {
                if (_lightMap != null && !_lightMap.IsDisposed())
                    _lightMap.Dispose();

                _lightMap = new Image(_rw.Width, _rw.Height);
            }

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
            _lightMap.CopyScreen(_rw);

            return _lightMap;
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

            if (_lightMap != null && !_lightMap.IsDisposed())
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
        public void Update(int currentTime)
        {
            foreach (var light in this)
            {
                light.Update(currentTime);
            }
        }

        #endregion
    }
}