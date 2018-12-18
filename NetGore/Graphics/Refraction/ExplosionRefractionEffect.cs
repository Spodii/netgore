using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Properties;
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
        const float _defaultIntensity = 0.1f;
        const ushort _defaultLifeSpan = 2000;

        static readonly Shader _defaultShader;

        readonly Grh _explosionNoise;
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
            {
                const string msg =
                    "Unable to construct shader for `ExplosionRefractionEffect` - shaders are not supported on this system.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(msg);
                return;
            }

            // Try to create the default shader
            try
            {
                var code = Resources.ExplosionRefractionEffectShader;
                _defaultShader = ShaderExtensions.LoadFromMemory(code);
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
        /// <param name="explosionNoise">The sprite used to create the explosion's refraction map.</param>
        /// <param name="positionProvider">The <see cref="ISpatial"/> that provides the position of this
        /// <see cref="ExplosionRefractionEffect"/>.</param>
        /// <param name="lifeSpan">The life span in milliseconds. If 0, the <see cref="DefaultLifeSpan"/> will be used.</param>
        /// <param name="shader">The <see cref="Shader"/> to use to draw the explosion's refraction map. If null, the
        /// <see cref="ExplosionRefractionEffect.DefaultShader"/> will be used. If you provide your own shader, you must
        /// make sure that you either use the same effect parameters the default shader uses, or override this class
        /// so you can override the <see cref="ExplosionRefractionEffect.SetShaderParameters"/> method and set the parameters
        /// you require.</param>
        /// <exception cref="ArgumentNullException"><paramref name="explosionNoise"/> is null.</exception>
        /// <exception cref="NullReferenceException"><paramref name="positionProvider"/> is null.</exception>
        public ExplosionRefractionEffect(Grh explosionNoise, ISpatial positionProvider, ushort lifeSpan = (ushort)0,
                                         Shader shader = null) : this(explosionNoise, positionProvider.Center, lifeSpan, shader)
        {
            PositionProvider = positionProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionRefractionEffect"/> class.
        /// </summary>
        /// <param name="explosionNoise">The sprite used to create the explosion's refraction map.</param>
        /// <param name="center">The world position of the effect.</param>
        /// <param name="lifeSpan">The life span in milliseconds. If 0, the <see cref="DefaultLifeSpan"/> will be used.</param>
        /// <param name="shader">The <see cref="Shader"/> to use to draw the explosion's refraction map. If null, the
        /// <see cref="ExplosionRefractionEffect.DefaultShader"/> will be used. If you provide your own shader, you must
        /// make sure that you either use the same effect parameters the default shader uses, or override this class
        /// so you can override the <see cref="ExplosionRefractionEffect.SetShaderParameters"/> method and set the parameters
        /// you require.</param>
        /// <exception cref="ArgumentNullException"><paramref name="explosionNoise"/> is null.</exception>
        public ExplosionRefractionEffect(Grh explosionNoise, Vector2 center, ushort lifeSpan = (ushort)0, Shader shader = null)
        {
            if (explosionNoise == null)
                throw new ArgumentNullException("explosionNoise");

            _explosionNoise = explosionNoise;
            _startTime = TickCount.Now;
            _center = center;

            _shader = shader ?? DefaultShader;

            if (lifeSpan <= 0)
                _lifeSpan = DefaultLifeSpan;
            else
                _lifeSpan = 0;

            // Copy over the default values
            Intensity = DefaultIntensity;
            ExpansionRate = DefaultExpansionRate;
            IsEnabled = true;

            // Ensure we are able to use the effect
            if (_shader == null)
            {
                const string errmsg =
                    "Shaders not supported or unable to acquire a valid default shader. Expiring effect `{0}`...";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this);
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
        /// Gets or sets the global default intensity for all <see cref="ExplosionRefractionEffect"/>.
        /// Default value is 0.1.
        /// </summary>
        [DefaultValue(_defaultIntensity)]
        public static float DefaultIntensity { get; set; }

        /// <summary>
        /// Gets or sets the global default lifespan, in milliseconds, for all <see cref="ExplosionRefractionEffect"/>s.
        /// Default value is 2000 (2 seconds).
        /// </summary>
        [DefaultValue(_defaultLifeSpan)]
        public static ushort DefaultLifeSpan { get; set; }

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
        /// Gets or sets how fast the explosion effect expands in pixels per millisecond.
        /// </summary>
        public Vector2 ExpansionRate { get; set; }

        /// <summary>
        /// Gets the sprite used to create the explosion's refraction map.
        /// </summary>
        public Grh ExplosionNoise
        {
            get { return _explosionNoise; }
        }

        /// <summary>
        /// Gets or sets the intensity of the explosion. This value should be on the range of 0.0 to 1.0.
        /// </summary>
        public float Intensity { get; set; }

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
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{Vector2}"/> instance containing the event data.</param>
        void PositionProvider_Moved(ISpatial sender, EventArgs<Vector2> e)
        {
            // Avoid needless calculations on finding the position when there are no listeners for Moved
            if (Moved != null)
            {
                var oldValue = Position;
                _center = sender.Center;
                Moved.Raise(this, EventArgsHelper.Create(oldValue));
            }
            else
                _center = sender.Center;
        }

        /// <summary>
        /// Sets the parameters for the shader.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected virtual void SetShaderParameters(TickCount currentTime)
        {
            Shader.SetParameter("NoiseTexture", ExplosionNoise.CurrentGrhData.Texture);
            Shader.SetParameter("MaxAge", _lifeSpan);
            Shader.SetParameter("Intensity", Intensity);

            var age = (int)currentTime - (int)_startTime;
            Shader.SetParameter("Age", age);
        }

        #region IRefractionEffect Members

        /// <summary>
        /// Notifies listeners when this <see cref="IRefractionEffect"/> has moved.
        /// </summary>
        public event TypedEventHandler<ISpatial, EventArgs<Vector2>> Moved;

        /// <summary>
        /// Unused by <see cref="ExplosionRefractionEffect"/>.
        /// </summary>
        event TypedEventHandler<ISpatial, EventArgs<Vector2>> ISpatial.Resized
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Gets the center position of the <see cref="IRefractionEffect"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Center
        {
            get { return Position + Size / 2f; }
        }

        /// <summary>
        /// Gets the drawing priority of this <see cref="IRefractionEffect"/>. The value is relative to other
        /// <see cref="IRefractionEffect"/>s. <see cref="IRefractionEffect"/>s with lower values are drawn first.
        /// </summary>
        public virtual int DrawPriority
        {
            get { return 100000; }
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
                    Moved.Raise(this, EventArgsHelper.Create(oldValue));
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
                {
                    _positionProvider.Moved -= PositionProvider_Moved;
                }

                _positionProvider = value;

                if (_positionProvider != null)
                {
                    _positionProvider.Moved -= PositionProvider_Moved;
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
        /// Gets if this <see cref="ISpatial"/> can ever be moved with <see cref="ISpatial.TryMove"/>.
        /// </summary>
        bool ISpatial.SupportsMove
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this <see cref="ISpatial"/> can ever be resized with <see cref="ISpatial.TryResize"/>.
        /// </summary>
        bool ISpatial.SupportsResize
        {
            get { return false; }
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

            if (_positionProvider != null)
                _positionProvider.Moved -= PositionProvider_Moved;
        }

        /// <summary>
        /// Draws the <see cref="IRefractionEffect"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        public virtual void Draw(ISpriteBatch spriteBatch)
        {
            if (IsExpired || !IsEnabled)
                return;

            // Update the effect's parameters
            SetShaderParameters(_lastUpdateTime);

            // Draw
            var dest = ToRectangle();

            var oldBlendMode = spriteBatch.BlendMode;
            try
            {
                spriteBatch.BlendMode = BlendMode.Add;
                Shader.Bind();
                ExplosionNoise.Draw(spriteBatch, dest);
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
        /// Tries to move the <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="newPos">The new position.</param>
        /// <returns>True if the <see cref="ISpatial"/> was moved to the <paramref name="newPos"/>; otherwise false.</returns>
        bool ISpatial.TryMove(Vector2 newPos)
        {
            Position = newPos;
            return true;
        }

        /// <summary>
        /// Tries to resize the <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        /// <returns>True if the <see cref="ISpatial"/> was resized to the <paramref name="newSize"/>; otherwise false.</returns>
        bool ISpatial.TryResize(Vector2 newSize)
        {
            return false;
        }

        /// <summary>
        /// Updates the <see cref="IRefractionEffect"/>.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public virtual void Update(TickCount currentTime)
        {
            if (IsExpired)
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
            ExplosionNoise.Update(currentTime);
        }

        #endregion
    }
}