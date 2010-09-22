using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.World;
using SFML;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A refraction effect that mirrors the image above it, optionally compresses or explodes it, and applies
    /// a moving noise map to simulate water.
    /// </summary>
    public class WaterRefractionEffect : IRefractionEffect
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly Shader _defaultShader;
        readonly Shader _shader;

        readonly Grh _waveNoise;
        bool _isDisposed;

        Vector2 _position;
        ISpatial _positionProvider;
        Vector2 _size;

        /// <summary>
        /// Initializes the <see cref="WaterRefractionEffect"/> class.
        /// </summary>
        static WaterRefractionEffect()
        {
            // Set the default values

            // Check if shaders are supported
            if (!Shader.IsAvailable)
                return;

            const string defaultShaderCode =
                @"
// The distance multiplier to apply to the values unpacked from channels to get the offset. This decreases our resolution,
// giving us a choppier image, but increases our range. Lower values give higher resolution but require smaller distances.
// This MUST be the same in all the refraction effects!
const float DistanceMultiplier = 2.0;

// The value of the reflection channels that will be used to not perform any reflection. Having this non-zero allows us to
// reflect in both directions instead of just borrowing pixels in one direction. Of course, this also halves our max distance.
// Logically, this value is 1/2. However, a slightly different number is used due to the behavior of floating-point numbers.
// This MUST be the same in all the refraction effects!
const float BaseOffset = 0.4981;

// The texture used to create the waves.
uniform sampler2D WaveNoiseTexture;

// The intensity of the waves. Greater value = higher intensity. Changing the wave noise texture itself is also
// a good way to go about altering the intensity and just the overall pattern of the waves.
uniform float WaveIntensity = 0.01;

// The current time in milliseconds.
uniform float Time;

// The wave speed multiplier. The greater the value, the faster the waves move.
uniform float WaveSpeedMultiplier = 0.5;

// The alpha value of the water. A lower alpha makes the water less transparent, making it harder to see the original
// graphics under it. This is just a modifier of the alpha of the wave noise texture, allowing you to give different
// alpha values without having to change the texture.
uniform float WaterAlphaModifier = 0.38;

void main (void)
{
	vec4 noiseVec;
	vec2 waveNoiseOffsetVec;
	vec2 newRG;

	// Get the noise vector for the waves, using the time so we can move, and mod to stay in the range of (0.0f, 1.0f).
	waveNoiseOffsetVec = mod(gl_TexCoord[0].st + (Time * 0.01f * WaveSpeedMultiplier), 1.0f);

	// Add the noise from the waves.
	noiseVec = texture2D(WaveNoiseTexture, waveNoiseOffsetVec).rgba;

	// Calculate the red and green channels in the same way. Simply, we take the base offset then add the noise from the water.
	newRG = BaseOffset + (noiseVec.rg * WaveIntensity);

	// For the vertical offset, we also need to add the texture offset, which allows us to properly reflect. Adding is used
	// because we reflect what is above us.
	newRG.y += gl_TexCoord[0].t / DistanceMultiplier;

	// Clamp it all into the range of (0.0f, 1.0f)
	newRG = clamp(newRG, 0.0f, 1.0f);

	// Apply the color.
	gl_FragColor = vec4(newRG.x, newRG.y, 0.0f, clamp(noiseVec.a - WaterAlphaModifier, 0.0f, 1.0f));
}";

            // Try to create the default shader
            try
            {
                _defaultShader = ShaderHelper.LoadFromMemory(defaultShaderCode);
            }
            catch (LoadingFailedException ex)
            {
                const string errmsg = "Failed to load the default Shader for WaterRefractionEffect. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaterRefractionEffect"/> class.
        /// </summary>
        /// <param name="waveNoise">The sprite to use for the wave noise.</param>
        /// <param name="position">The initial world position of the effect.</param>
        /// <param name="size">The size of the effect in pixels.</param>
        /// <param name="shader">The <see cref="Shader"/> to use to draw the water's refraction map. If null, the
        /// <see cref="WaterRefractionEffect.DefaultShader"/> will be used. If you provide your own shader, you must
        /// make sure that you either use the same effect parameters the default shader uses, or override this class
        /// so you can override the <see cref="WaterRefractionEffect.SetShaderParameters"/> method and set the parameters
        /// you require.</param>
        /// <exception cref="ArgumentNullException"><paramref name="waveNoise"/> is null.</exception>
        public WaterRefractionEffect(Grh waveNoise, Vector2 position, Vector2 size, Shader shader = null)
        {
            if (waveNoise == null)
                throw new ArgumentNullException("waveNoise");

            _waveNoise = waveNoise;
            _position = position;
            _size = size;

            _shader = shader ?? DefaultShader;

            // Copy over the default values
            // TODO:

            // Ensure we are able to use the effect
            if (_shader == null)
            {
                const string errmsg = "Shaders not supported or unable to acquire a valid default shader. Expiring effect...";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                Dispose();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaterRefractionEffect"/> class.
        /// </summary>
        /// <param name="waveNoise">The sprite to use for the wave noise.</param>
        /// <param name="positionProvider">The <see cref="ISpatial"/> that will provide the positioning.</param>
        /// <param name="size">The size of the effect in pixels.</param>
        /// <param name="shader">The <see cref="Shader"/> to use to draw the water's refraction map. If null, the
        /// <see cref="WaterRefractionEffect.DefaultShader"/> will be used. If you provide your own shader, you must
        /// make sure that you either use the same effect parameters the default shader uses, or override this class
        /// so you can override the <see cref="WaterRefractionEffect.SetShaderParameters"/> method and set the parameters
        /// you require.</param>
        /// <exception cref="ArgumentNullException"><paramref name="waveNoise"/> is null.</exception>
        /// <exception cref="NullReferenceException"><paramref name="positionProvider"/> is null.</exception>
        public WaterRefractionEffect(Grh waveNoise, ISpatial positionProvider, Vector2 size, Shader shader = null)
            : this(waveNoise, positionProvider.Position, size, shader)
        {
            PositionProvider = positionProvider;
        }

        /// <summary>
        /// Gets the default <see cref="Shader"/> used for the <see cref="WaterRefractionEffect"/>.
        /// This is used to draw the water's refraction noise to the refraction map.
        /// When <see cref="SFML.Graphics.Shader.IsAvailable"/> is false, this value will always be null.
        /// It will also be null if the code for the default shader, for whatever reason, failed to load.
        /// </summary>
        public static Shader DefaultShader
        {
            get { return _defaultShader; }
        }

        /// <summary>
        /// Gets the <see cref="Shader"/> being used by this effect to draw the water's refraction map.
        /// </summary>
        public Shader Shader
        {
            get { return _shader; }
        }

        /// <summary>
        /// Gets the sprite used to create the waves on the water's refraction map.
        /// </summary>
        public Grh WaveNoise
        {
            get { return _waveNoise; }
        }

        /// <summary>
        /// Handles when the <see cref="IRefractionEffect.PositionProvider"/> moves.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void PositionProvider_Moved(ISpatial sender, Vector2 e)
        {
            // Avoid needless calculations on finding the position when there are no listeners for Moved
            if (Moved != null)
            {
                var oldValue = Position;
                _position = sender.Position;
                Moved(this, oldValue);
            }
            else
                _position = sender.Position;
        }

        /// <summary>
        /// Sets the parameters for the shader.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected virtual void SetShaderParameters(TickCount currentTime)
        {
            Shader.SetTexture("WaveNoiseTexture", WaveNoise.CurrentGrhData.Texture);
            Shader.SetParameter("WaveIntensity", 0.01f); // TODO: Make param
            Shader.SetParameter("WaveSpeedMultiplier", 0.5f); // TODO: Make param
            Shader.SetParameter("WaterAlphaModifier", 0.38f); // TODO: Make param
            Shader.SetParameter("Time", currentTime);
        }

        #region IRefractionEffect Members

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        public event SpatialEventHandler<Vector2> Moved;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        public event SpatialEventHandler<Vector2> Resized;

        /// <summary>
        /// Gets the center position of the <see cref="ISpatial"/>.
        /// </summary>
        public Vector2 Center
        {
            get { return Position + (Size / 2f); }
        }

        /// <summary>
        /// Gets or sets if this refraction effect is enabled.
        /// </summary>
        [DisplayName("IsEnabled")]
        [Description("If this refraction effect is enabled.")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets if this effect has expired. Not all effects have to expire. This is not the same as <see cref="IRefractionEffect.IsEnabled"/>
        /// since expired effects are completely destroyed and removed, while disabled effects can still be enabled again. Once this
        /// value is true, it should remain true and the effect should cease being used.
        /// Disposing an <see cref="IRefractionEffect"/> will force it to be expired.
        /// </summary>
        public bool IsExpired
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        public Vector2 Max
        {
            get { return Position + Size; }
        }

        /// <summary>
        /// Gets or sets the world coordinates of the top-left corner of this <see cref="ISpatial"/>.
        /// Setting this value is valid only when the <see cref="PositionProvider"/> is null.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (PositionProvider != null)
                    return;

                _position = value;
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="ISpatial"/> that provides the position to use. If set, the
        /// <see cref="ISpatial.Position"/> value will automatically be acquired with the position of the
        /// <see cref="ISpatial"/> instead, and setting the position will have no affect.
        /// </summary>
        public ISpatial PositionProvider
        {
            get { return _positionProvider; }
            set
            {
                if (_positionProvider == value)
                    return;

                if (_positionProvider != null)
                    _positionProvider.Moved -= PositionProvider_Moved;

                _positionProvider = value;

                if (_positionProvider != null)
                {
                    _positionProvider.Moved += PositionProvider_Moved;
                    _position = _positionProvider.Position;
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref="ISpatial"/>.
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Gets or sets an object that can be used to identify or store information about this <see cref="IRefractionEffect"/>.
        /// This property is purely optional.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
        }

        /// <summary>
        /// Draws the <see cref="IRefractionEffect"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        public void Draw(ISpriteBatch spriteBatch)
        {
            if (IsExpired || !IsEnabled)
                return;

            // Draw
            var dest = ToRectangle();

            var oldBlendMode = spriteBatch.BlendMode;
            try
            {
                spriteBatch.BlendMode = BlendMode.Add;
                try
                {
                    Shader.Bind();

                    WaveNoise.Draw(spriteBatch, dest);
                }
                finally
                {
                    Shader.Unbind();
                }
            }
            finally
            {
                spriteBatch.BlendMode = oldBlendMode;
            }
        }

        /// <summary>
        /// Translates this <see cref="IRefractionEffect"/> relative to the current position.
        /// </summary>
        /// <param name="offset">The amount to move from the current position.</param>
        public void Move(Vector2 offset)
        {
            if (PositionProvider == null)
                Position += offset;
        }

        /// <summary>
        /// Moves this <see cref="IRefractionEffect"/> to a new position.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        public void Teleport(Vector2 newPosition)
        {
            if (PositionProvider == null)
                Position = newPosition;
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/> occupies.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/>
        /// occupies.</returns>
        public Rectangle ToRectangle()
        {
            return SpatialHelper.ToRectangle(this);
        }

        /// <summary>
        /// Updates the <see cref="IRefractionEffect"/>.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void Update(TickCount currentTime)
        {
            if (IsExpired)
                return;

            // Most all of the updating takes place in the shader itself, which is done right before drawing

            // Update the sprite
            WaveNoise.Update(currentTime);
        }

        #endregion
    }
}