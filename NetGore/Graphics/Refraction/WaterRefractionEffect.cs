using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using NetGore.Properties;
using NetGore.World;
using SFML;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A refraction effect that mirrors the image above it, optionally compresses or explodes it, and applies
    /// a moving noise map to simulate water.
    /// </summary>
    public class WaterRefractionEffect : IRefractionEffect, IPersistable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const float _defaultMagnification = 1.0f;

        const float _defaultWaterAlpha = 0.38f;
        const float _defaultWaveIntensity = 0.01f;
        const float _defaultWaveSpeed = 0.5f;
        const string _valueKeyWaveNoise = "WaveNoiseGrh";

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
            DefaultWaterAlpha = _defaultWaterAlpha;
            DefaultWaveSpeed = _defaultWaveSpeed;
            DefaultWaveIntensity = _defaultWaveIntensity;
            DefaultMagnification = _defaultMagnification;

            // Check if shaders are supported
            if (!Shader.IsAvailable)
            {
                const string msg =
                    "Unable to construct shader for `WaterRefractionEffect` - shaders are not supported on this system.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(msg);
                return;
            }

            // Try to create the default shader
            try
            {
                var code = Resources.WaterRefractionEffectShader;
                _defaultShader = ShaderExtensions.LoadFromMemory(code);
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
            WaveIntensity = DefaultWaveIntensity;
            WaveSpeed = DefaultWaveSpeed;
            WaterAlpha = DefaultWaterAlpha;
            Magnification = DefaultMagnification;
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
        /// Initializes a new instance of the <see cref="WaterRefractionEffect"/> class.
        /// </summary>
        protected WaterRefractionEffect() : this(new Grh(), Vector2.Zero, new Vector2(32))
        {
            // This constructor is only provided for the RefractionEffectFactory
        }

        /// <summary>
        /// Gets or sets the default <see cref="WaterRefractionEffect.Magnification"/> value for new instances.
        /// The default value is 1.0f.
        /// </summary>
        [DefaultValue(_defaultMagnification)]
        public static float DefaultMagnification { get; set; }

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
        /// Gets or sets the default <see cref="WaterRefractionEffect.WaterAlpha"/> value for new instances.
        /// The default value is 0.38f.
        /// </summary>
        [DefaultValue(_defaultWaterAlpha)]
        public static float DefaultWaterAlpha { get; set; }

        /// <summary>
        /// Gets or sets the default <see cref="WaterRefractionEffect.WaveIntensity"/> value for new instances.
        /// The default value is 0.01f.
        /// </summary>
        [DefaultValue(_defaultWaveIntensity)]
        public static float DefaultWaveIntensity { get; set; }

        /// <summary>
        /// Gets or sets the default <see cref="WaterRefractionEffect.WaveSpeed"/> value for new instances.
        /// The default value is 0.5f.
        /// </summary>
        [DefaultValue(_defaultWaveSpeed)]
        public static float DefaultWaveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the amount that the water magnifies the reflected image. Ignoring magnification resulting from the wave
        /// texture, a value of 1.0f would result in the objects in the reflected image being the same height as the original
        /// image. A value of 0.5f would result in the reflected images being twice the size of their original size, and as a
        /// result show only half the amount it normally would. Similarly, 1.5f would make the reflected images half their
        /// size and show twice as much.
        /// </summary>
        [SyncValue]
        public float Magnification { get; set; }

        /// <summary>
        /// Gets the <see cref="Shader"/> being used by this effect to draw the water's refraction map.
        /// </summary>
        [Browsable(false)]
        public Shader Shader
        {
            get { return _shader; }
        }

        /// <summary>
        /// Gets or sets the modifier for the water's alpha value. A lower alpha value results in less transparent water, making
        /// it harder to see what is under the water. This is just a modifier of the alpha of the wave noise texture,
        /// allowing you to give different alpha values without having to change the texture. This value should be between
        /// 0.0f and 1.0f, where 0.0f is completely opaque water (cannot see anything under it) and 1.0f is completely transparent
        /// water (resulting in nothing being shown).
        /// </summary>
        [SyncValue]
        public float WaterAlpha { get; set; }

        /// <summary>
        /// Gets or sets the refraction intensity of the waves. A greater value results in a higher intensity. This does not
        /// make the waves actually bigger, but makes them have a greater influence on the refraction, thus making them more
        /// prominent. A reasonable value is often a very small one (around 0.005f to 0.05f), but depends completely on the
        /// <see cref="WaveNoise"/> sprite used.
        /// </summary>
        [SyncValue]
        public float WaveIntensity { get; set; }

        /// <summary>
        /// Gets the sprite used to create the waves on the water's refraction map.
        /// </summary>
        public Grh WaveNoise
        {
            get { return _waveNoise; }
        }

        /// <summary>
        /// Gets or sets the speed of the waves. The greater the value, the faster the wave noise texture moves, making the
        /// waves look faster. A reasonable value is around 0.1f to 0.9f.
        /// </summary>
        [SyncValue]
        public float WaveSpeed { get; set; }

        /// <summary>
        /// Handles when the <see cref="IRefractionEffect.PositionProvider"/> moves.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void PositionProvider_Moved(ISpatial sender, EventArgs<Vector2> e)
        {
            // Avoid needless calculations on finding the position when there are no listeners for Moved
            if (Moved != null)
            {
                // Moved has listener(s)
                var oldValue = Position;
                _position = sender.Position;

                Moved.Raise(this, EventArgsHelper.Create(oldValue));
            }
            else
            {
                // No Moved listeners
                _position = sender.Position;
            }
        }

        /// <summary>
        /// Sets the parameters for the shader.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected virtual void SetShaderParameters(TickCount currentTime)
        {
            Shader.SetParameter("WaveNoiseTexture", WaveNoise.CurrentGrhData.Texture);
            Shader.SetParameter("WaveIntensity", WaveIntensity);
            Shader.SetParameter("WaveSpeedMultiplier", WaveSpeed);
            Shader.SetParameter("WaterAlphaModifier", WaterAlpha);
            Shader.SetParameter("DrawTextureHeightRatio", Size.Y / WaveNoise.CurrentGrhData.Texture.Size.Y);
            Shader.SetParameter("Magnification", Magnification);
            Shader.SetParameter("Time", currentTime);
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        void IPersistable.ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);

            // Manually handle the WaveNoise sprite
            var grhIndex = reader.ReadGrhIndex(_valueKeyWaveNoise);
            _waveNoise.SetGrh(grhIndex);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        void IPersistable.WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);

            // Manually handle the WaveNoise sprite
            writer.Write(_valueKeyWaveNoise, WaveNoise.GrhData.GrhIndex);
        }

        #endregion

        #region IRefractionEffect Members

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        public event TypedEventHandler<ISpatial, EventArgs<Vector2>> Moved;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        public event TypedEventHandler<ISpatial, EventArgs<Vector2>> Resized;

        /// <summary>
        /// Gets the center position of the <see cref="ISpatial"/>.
        /// </summary>
        public Vector2 Center
        {
            get { return Position + (Size / 2f); }
        }

        /// <summary>
        /// Gets the drawing priority of this <see cref="IRefractionEffect"/>. The value is relative to other
        /// <see cref="IRefractionEffect"/>s. <see cref="IRefractionEffect"/>s with lower values are drawn first.
        /// </summary>
        public virtual int DrawPriority
        {
            get { return -100000; }
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
        [Browsable(false)]
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
        [SyncValue]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (PositionProvider != null)
                    return;

                if (_position == value)
                    return;

                var oldPos = _position;
                _position = value;

                if (Moved != null)
                    Moved.Raise(this, EventArgsHelper.Create(oldPos));
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
                {
                    _positionProvider.Moved -= PositionProvider_Moved;
                }

                _positionProvider = value;

                if (_positionProvider != null)
                {
                    _positionProvider.Moved -= PositionProvider_Moved;
                    _positionProvider.Moved += PositionProvider_Moved;
                    _position = _positionProvider.Position;
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref="ISpatial"/>.
        /// </summary>
        [SyncValue]
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size == value)
                    return;

                var oldSize = _size;
                _size = value;

                if (Resized != null)
                    Resized.Raise(this, EventArgsHelper.Create(oldSize));
            }
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
            get { return true; }
        }

        /// <summary>
        /// Gets or sets an object that can be used to identify or store information about this <see cref="IRefractionEffect"/>.
        /// This property is purely optional.
        /// </summary>
        [Browsable(false)]
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

            SetShaderParameters(TickCount.Now);

            // Draw
            var dest = ToRectangle();

            var oldBlendMode = spriteBatch.BlendMode;
            try
            {
                spriteBatch.BlendMode = BlendMode.None;
                Shader.Bind();
                WaveNoise.Draw(spriteBatch, dest);
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
            Size = newSize;
            return true;
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