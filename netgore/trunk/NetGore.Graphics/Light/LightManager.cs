using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Collections;

namespace NetGore.Graphics
{
    /// <summary>
    /// Manages multiple <see cref="ILight"/>s.
    /// </summary>
    public class LightManager : VirtualList<ILight>, ILightManager
    {
        Color _ambient;
        Grh _defaultSprite;
        GraphicsDevice _gd;
        ResolveTexture2D _lightMap;
        ISpriteBatch _sb;

        #region ILightManager Members

        /// <summary>
        /// Gets or sets the ambient light color. The alpha value has no affect and will always be set to 255.
        /// </summary>
        public Color Ambient
        {
            get { return _ambient; }
            set { _ambient = new Color(value, 255); }
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
            get { return _lightMap != null; }
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

            base.Add(item);
        }

        /// <summary>
        /// Draws all of the lights in this <see cref="ILightManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Texture2D"/> containing the light map.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="ILightManager.IsInitialized"/> is false.</exception>
        public Texture2D Draw(ICamera2D camera)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("You must initialize the ILightManager before drawing.");

            // Clear the buffer with the ambient light color
            _gd.Clear(Ambient);

            // Don't waste time starting and stopping the SpriteBatch if there is nothing to draw
            if (Count > 0)
            {
                var rs = _gd.RenderState;

                // Store the previous render state values in interest
                var oldDestinationBlend = rs.DestinationBlend;
                var oldSourceBlend = rs.SourceBlend;
                var oldBlendFunction = rs.BlendFunction;
                var oldSABE = rs.SeparateAlphaBlendEnabled;

                // Start the SpriteBatch
                _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.Matrix);

                // Set the render state
                rs.DestinationBlend = Blend.One;
                rs.SourceBlend = Blend.DestinationAlpha;
                rs.BlendFunction = BlendFunction.Add;
                rs.SeparateAlphaBlendEnabled = true;

                // Draw the lights
                foreach (var light in this)
                {
                    light.Draw(_sb);
                }

                _sb.End();

                // Restore the render states
                rs.DestinationBlend = oldDestinationBlend;
                rs.SourceBlend = oldSourceBlend;
                rs.BlendFunction = oldBlendFunction;
                rs.SeparateAlphaBlendEnabled = oldSABE;
            }

            // Get and return the light map
            _gd.ResolveBackBuffer(_lightMap);
            return _lightMap;
        }

        /// <summary>
        /// Initializes the <see cref="ILightManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="ILight"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice"/> is null.</exception>
        public void Initialize(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");

            if (_lightMap != null && !_lightMap.IsDisposed)
                _lightMap.Dispose();

            if (_sb != null && !_sb.IsDisposed)
                _sb.Dispose();

            _gd = graphicsDevice;

            var pp = _gd.PresentationParameters;
            _lightMap = new ResolveTexture2D(_gd, pp.BackBufferWidth, pp.BackBufferHeight, 1, pp.BackBufferFormat);

            _sb = new RoundedXnaSpriteBatch(_gd);
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