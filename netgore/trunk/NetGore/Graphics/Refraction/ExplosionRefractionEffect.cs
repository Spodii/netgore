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
    /// A refraction effect that creates a refraction that looks like the shockwave for an explosion.
    /// </summary>
    public class ExplosionRefractionEffect : IRefractionEffect
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const ushort _defaultLifeSpan = 2000;
        const float _defaultIntensity = 0.1f;

        static readonly Shader _defaultShader;

        readonly Grh _explosionMap;
        readonly ushort _lifeSpan;
        readonly Shader _shader;
        readonly TickCount _startTime;

        Vector2 _center;

        /// <summary>
        /// We treat expiration and disposal the same, since when it expires, we might as well just dispose it, too.
        /// </summary>
        bool _isDisposed;

        TickCount _lastUpdateTime;

        ISpatial _positionProvider;
        Vector2 _size;

        /// <summary>
        /// Initializes the <see cref="ExplosionRefractionEffect"/> class.
        /// </summary>
        static ExplosionRefractionEffect()
        {
            // Set the default values
            DefaultLifeSpan = _defaultLifeSpan;
            DefaultExpansionRate = new Vector2(1.5f, 1.5f);
            DefaultIntensity = _defaultIntensity;

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

// The maximum alpha value allowed.
const float MaxAlpha = 1.00;

// The alpha multiplier. Greater values keep at MaxAlpha for longer, but fade out much faster.
// Too low of values will just end up not reaching MaxAlpha, or not reaching it for long.
const float AlphaMultiplier = 2.0;

// A multiplier used on the distortion vector values to determine the intensity. A value of 1.0f will result
// in using a distortion equal to that in the source noise image. Its recommended you manually edit the noise
// texture's R and G channels to alter the intensity, but this value can still be used to give different intensities
// to the same texture.
uniform float Intensity = 0.1;

// The maximum expected age of the effect. Unit does not matter as long as it is consistent.
uniform float MaxAge = 2000.0;

// The current age of the effect. Unit does not matter as long as it is consistent.
uniform float Age;

// The texture used to generate the noise.
uniform sampler2D NoiseTexture;

void main (void)
{
	vec4 noiseVec;
	vec2 noiseXYVec;
	float a;

	// Get the noise from the texture
	noiseVec = texture2D(NoiseTexture, gl_TexCoord[0].st);

	// Calculate the noise vector
	noiseXYVec = (noiseVec.xy / DistanceMultiplier) * Intensity;

	// Calculate the alpha, which will let us fade out smoothly before the effect ends
	a = clamp(noiseVec.a * AlphaMultiplier * ((MaxAge - Age) / MaxAge), 0.0f, MaxAlpha);

	// Apply the coloring, using the transformation directly and unaltered from the noise texture
	gl_FragColor = vec4(noiseXYVec.x, noiseXYVec.y, 0.0f, a);
}";

            // Try to create the default shader
            try
            {
                _defaultShader = ShaderHelper.LoadFromMemory(defaultShaderCode);
            }
            catch (LoadingFailedException ex)
            {
                const string errmsg = "Failed to load the default Shader for ExplosionRefractionEffect. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionRefractionEffect"/> class.
        /// </summary>
        /// <param name="explosionMap">The sprite used to create the explosion's refraction map.</param>
        /// <param name="positionProvider">The <see cref="ISpatial"/> that provides the position of this
        /// <see cref="ExplosionRefractionEffect"/>.</param>
        /// <param name="lifeSpan">The life span in milliseconds. If 0, the <see cref="DefaultLifeSpan"/> will be used.</param>
        /// <param name="shader">The <see cref="Shader"/> to use to draw the explosion's refraction map. If null, the
        /// <see cref="ExplosionRefractionEffect.DefaultShader"/> will be used. If you provide your own shader, you must
        /// make sure that you either use the same effect parameters the default shader uses, or override this class
        /// so you can override the <see cref="ExplosionRefractionEffect.SetShaderParameters"/> method and set the parameters
        /// you require.</param>
        /// <exception cref="ArgumentNullException"><paramref name="explosionMap"/> is null.</exception>
        public ExplosionRefractionEffect(Grh explosionMap, ISpatial positionProvider, ushort lifeSpan = (ushort)0,
            Shader shader = null) : this(explosionMap, 
            positionProvider.Center, lifeSpan, shader)
        {
            PositionProvider = positionProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionRefractionEffect"/> class.
        /// </summary>
        /// <param name="explosionMap">The sprite used to create the explosion's refraction map.</param>
        /// <param name="center">The world position of the effect.</param>
        /// <param name="lifeSpan">The life span in milliseconds. If 0, the <see cref="DefaultLifeSpan"/> will be used.</param>
        /// <param name="shader">The <see cref="Shader"/> to use to draw the explosion's refraction map. If null, the
        /// <see cref="ExplosionRefractionEffect.DefaultShader"/> will be used. If you provide your own shader, you must
        /// make sure that you either use the same effect parameters the default shader uses, or override this class
        /// so you can override the <see cref="ExplosionRefractionEffect.SetShaderParameters"/> method and set the parameters
        /// you require.</param>
        /// <exception cref="ArgumentNullException"><paramref name="explosionMap"/> is null.</exception>
        public ExplosionRefractionEffect(Grh explosionMap, Vector2 center, ushort lifeSpan = (ushort)0,Shader shader = null)
        {
            if (explosionMap == null)
                throw new ArgumentNullException("explosionMap");

            _explosionMap = explosionMap;
            _startTime = TickCount.Now;
            _center = center;

            if (lifeSpan <= 0)
                _lifeSpan = DefaultLifeSpan;
            else
                _lifeSpan = 0;

            if (shader == null)
                _shader = DefaultShader;
            else
                _shader = shader;

            // Copy over the default values
            Intensity = DefaultIntensity;
            ExpansionRate = DefaultExpansionRate;

            // Ensure we are able to use the effect
            if (_shader == null)
            {
                const string errmsg = "Shaders not supported or unable to acquire default shader. Expiring effect...";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                Dispose();
            }
        }

        /// <summary>
        /// Gets or sets the global default expansion rate for all <see cref="ExplosionRefractionEffect"/>s.
        /// Default value is (1.5, 1.5).
        /// </summary>
        [DefaultValue(typeof(Vector2), "{1.5, 1.5}")]
        public static Vector2 DefaultExpansionRate { get; set; }

        /// <summary>
        /// Gets or sets the global default lifespan, in milliseconds, for all <see cref="ExplosionRefractionEffect"/>s.
        /// Default value is 2000 (2 seconds).
        /// </summary>
        [DefaultValue(_defaultLifeSpan)]
        public static ushort DefaultLifeSpan { get; set; }

        /// <summary>
        /// Gets or sets the global default intensity for all <see cref="ExplosionRefractionEffect"/>.
        /// Default value is 0.1.
        /// </summary>
        [DefaultValue(_defaultIntensity)]
        public static float DefaultIntensity { get; set; }

        /// <summary>
        /// Gets the default <see cref="Shader"/> used for the <see cref="ExplosionRefractionEffect"/>.
        /// This is used to draw the explosion's refraction noise to the refraction map.
        /// When <see cref="SFML.Graphics.Shader.IsAvailable"/> is false, this value will always be null.
        /// It will also be null if the code for the default shader, for whatever reason, failed to load.
        /// </summary>
        public static Shader DefaultShader
        {
            get { return _defaultShader; }
        }

        /// <summary>
        /// Gets or sets the intensity of the explosion. This value should be on the range of 0.0 to 1.0.
        /// </summary>
        public float Intensity { get; set; }

        /// <summary>
        /// Gets or sets how fast the explosion effect expands in pixels per millisecond.
        /// </summary>
        public Vector2 ExpansionRate { get; set; }

        /// <summary>
        /// Gets the sprite used to create the explosion's refraction map.
        /// </summary>
        public Grh ExplosionMap
        {
            get { return _explosionMap; }
        }

        /// <summary>
        /// Gets the lifespan of this effect in milliseconds.
        /// </summary>
        public ushort Lifespan
        {
            get { return _lifeSpan; }
        }

        /// <summary>
        /// Gets the <see cref="Shader"/> being used by this effect to draw the explosion's refraction map.
        /// </summary>
        public Shader Shader
        {
            get { return _shader; }
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
                _center = sender.Center;
                Moved(this, oldValue);
            }
            else
            {
                _center = sender.Center;
            }
        }

        /// <summary>
        /// Sets the parameters for the shader.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected virtual void SetShaderParameters(TickCount currentTime)
        {
            Shader.SetTexture("NoiseTexture", ExplosionMap.CurrentGrhData.Texture);
            Shader.SetParameter("MaxAge", _lifeSpan);
            Shader.SetParameter("Intensity", Intensity);

            var age = (int)currentTime - (int)_startTime;
            Shader.SetParameter("Age", age);
        }

        #region IRefractionEffect Members

        /// <summary>
        /// Notifies listeners when this <see cref="IRefractionEffect"/> has moved.
        /// </summary>
        public event SpatialEventHandler<Vector2> Moved;

        /// <summary>
        /// Notifies listeners when this <see cref="IRefractionEffect"/> has been resized.
        /// </summary>
        public event SpatialEventHandler<Vector2> Resized;

        /// <summary>
        /// Gets the center position of the <see cref="IRefractionEffect"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Center
        {
            get { return Position + Size / 2f; }
        }

        /// <summary>
        /// Gets or sets if this reflection effect is enabled.
        /// </summary>
        [DisplayName("Enabled")]
        [Description("If this light is enabled.")]
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
        /// Gets the world coordinates of the bottom-right corner of this <see cref="IRefractionEffect"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Max
        {
            get { return Position + Size; }
        }

        /// <summary>
        /// Gets the world coordinates of the top-left corner of this <see cref="IRefractionEffect"/>.
        /// </summary>
        [DisplayName("Position")]
        [Description("The world position of the effect.")]
        [Browsable(true)]
        public Vector2 Position
        {
            get { return _center - (_size / 2.0f); }
            set
            {
                if (Position == value)
                    return;

                var oldValue = Position;
                _center = value - (_size / 2.0f);

                if (Moved != null)
                    Moved(this, oldValue);
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="ISpatial"/> that provides the position to use. If set, the
        /// <see cref="ISpatial.Position"/> value will automatically be acquired with the position of the
        /// <see cref="ISpatial"/> instead, and setting the position will have no affect.
        /// </summary>
        [Browsable(false)]
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
                    _center = _positionProvider.Center;
                }
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="IRefractionEffect"/>.
        /// </summary>
        [DisplayName("Size")]
        [Description("The size of the effect.")]
        [Browsable(true)]
        public Vector2 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets or sets an object that can be used to identify or store information about this <see cref="IRefractionEffect"/>.
        /// This property is purely optional.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
        }

        /// <summary>
        /// Draws the <see cref="IRefractionEffect"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        public virtual void Draw(ISpriteBatch spriteBatch)
        {
            if (_isDisposed)
                return;

            // Update the effect's parameters
            SetShaderParameters(_lastUpdateTime);

            // Draw
            var dest = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

            var oldBlendMode = spriteBatch.BlendMode;
            try
            {
                spriteBatch.BlendMode = BlendMode.Add;
                try
                {
                    Shader.Bind();

                    ExplosionMap.Draw(spriteBatch, dest);
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
            Position += offset;
        }

        /// <summary>
        /// Moves this <see cref="IRefractionEffect"/> to a new position.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        public void Teleport(Vector2 newPosition)
        {
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
        public virtual void Update(TickCount currentTime)
        {
            if (_isDisposed)
                return;

            _lastUpdateTime = currentTime;

            // Get the life of the effect
            var totalElapsedTime = (int)currentTime - _startTime;
            if (totalElapsedTime < 0)
                totalElapsedTime = 0;

            // Check if expired
            if (totalElapsedTime >= _lifeSpan)
            {
                Dispose();
                return;
            }

            // Update the size
            _size = ExpansionRate * totalElapsedTime;

            // Update the sprite
            ExplosionMap.Update(currentTime);
        }

        #endregion
    }
}