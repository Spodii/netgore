using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Base class for all emitters of <see cref="Particle"/>s.
    /// </summary>
    public abstract class ParticleEmitter : IDisposable
    {
        /// <summary>
        /// The maximum value allowed for the delta time for updating particles and emitters. If the real delta
        /// time is greater than this value, is will be reduced to this value. This is to prevent ugly side-effects
        /// from emitters trying to play too much catch-up. Recommended to keep this at the default value.
        /// </summary>
        public const int MaxDeltaTime = 200;

        const string _blendModeKeyName = "BlendMode";
        const string _budgetKeyName = "Budget";
        const string _customValuesNodeName = "CustomValues";
        const SpriteBlendMode _defaultBlendMode = SpriteBlendMode.Additive;
        const int _defaultBudget = 5000;
        const string _defaultName = "Unnamed";
        const string _emitterCategoryName = "Emitter";
        const string _emitterModifiersNodeName = "EmitterModifiers";
        const string _grhIndexKeyName = "Grh";

        /// <summary>
        /// The initial size of the particle array.
        /// </summary>
        const int _initialParticleArraySize = 64;

        const string _lifeKeyName = "Life";
        const string _nameKeyName = "Name";
        const string _originKeyName = "Origin";
        const string _particleCategoryName = "Particle";
        const string _particleModifiersNodeName = "ParticleModifiers";
        const string _releaseAmountKeyName = "ReleaseAmount";
        const string _releaseColorKeyName = "ReleaseColor";
        const string _releaseRateKeyName = "ReleaseRate";
        const string _releaseRotationKeyName = "ReleaseRotation";
        const string _releaseScaleKeyName = "ReleaseScale";
        const string _releaseSpeedKeyName = "ReleaseSpeed";

        static readonly IEnumerable<Type> _emitterTypes =
            TypeHelper.FindTypesThatInherit(typeof(ParticleEmitter), Type.EmptyTypes, false).OrderBy(x => x.Name).ToCompact();

        /// <summary>
        /// The array of <see cref="Particle"/>s.
        /// </summary>
        protected Particle[] particles;

        readonly Grh _sprite = new Grh();

        int _budget;
        EmitterModifierCollection _emitterModifiers = new EmitterModifierCollection();
        int _expirationTime = int.MaxValue;
        bool _isDisposed = false;

        /// <summary>
        /// The index for the <see cref="particles"/> array of the last particle that is alive.
        /// </summary>
        int _lastAliveIndex = -1;

        int _lastUpdateTime = int.MinValue;
        int _nextReleaseTime;
        Vector2 _origin;
        ParticleModifierCollection _particleModifiers = new ParticleModifierCollection();

        /// <summary>
        /// Initializes the <see cref="ParticleEmitter"/> class.
        /// </summary>
        static ParticleEmitter()
        {
            DefaultBudget = _defaultBudget;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitter"/> class.
        /// </summary>
        protected ParticleEmitter()
        {
            _budget = DefaultBudget;
            particles = new Particle[_initialParticleArraySize];

            // Set some default values
            Name = _defaultName;
            BlendMode = _defaultBlendMode;
            Life = new VariableInt(2000);
            ReleaseAmount = new VariableUShort(1);
            ReleaseColor = new VariableColor(Color.White);
            ReleaseRate = new VariableUShort(100);
            ReleaseRotation = new VariableFloat(0);
            ReleaseScale = new VariableFloat(1);
            ReleaseSpeed = new VariableFloat(50);
        }

        /// <summary>
        /// Gets the number of living <see cref="Particle"/>s.
        /// </summary>
        [Browsable(false)]
        public int ActiveParticles
        {
            get { return _lastAliveIndex + 1; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SpriteBlendMode"/> to use when rendering the <see cref="Particle"/>s
        /// emitted by this <see cref="ParticleEmitter"/>.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The blending mode to use when rendering Particles from this emitter.")]
        [DisplayName("Blend Mode")]
        [DefaultValue(_defaultBlendMode)]
        public SpriteBlendMode BlendMode { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of live particles this <see cref="ParticleEmitter"/> may create at
        /// any given time.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> less than or equal to zero.</exception>
        [Category(_emitterCategoryName)]
        [Description("The maximum number of live Particles that this emitter may have out at once.")]
        [DisplayName("Budget")]
        [DefaultValue(_defaultBudget)]
        public int Budget
        {
            get { return _budget; }
            set
            {
                if (_budget == value)
                    return;

                if (_budget < 1)
                    throw new ArgumentOutOfRangeException("value", "Value must be greater than 0.");

                _budget = value;

                Array.Resize(ref particles, _budget);

                if (_lastAliveIndex >= particles.Length - 1)
                    _lastAliveIndex = particles.Length - 1;
            }
        }

        /// <summary>
        /// Gets the approximate current time.
        /// </summary>
        [Browsable(false)]
        protected int CurrentTime
        {
            get { return _lastUpdateTime; }
        }

        /// <summary>
        /// Gets or sets the default budget to give to new <see cref="ParticleEmitter"/>s when no budget
        /// is explicitly given.
        /// </summary>
        public static int DefaultBudget { get; set; }

        /// <summary>
        /// Gets or sets the collection of modifiers to use on the <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        [Category(_emitterCategoryName)]
        [Description("Collection of modifiers for the actual emitter.")]
        [DisplayName("Emitter Modifiers")]
        public EmitterModifierCollection EmitterModifiers
        {
            get { return _emitterModifiers; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _emitterModifiers = value;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="Type"/>s of <see cref="ParticleEmitter"/>s.
        /// </summary>
        public static IEnumerable<Type> EmitterTypes
        {
            get { return _emitterTypes; }
        }

        /// <summary>
        /// Gets if this <see cref="ParticleEmitter"/> has an infinite life span. If true,
        /// it will never expire automatically. If false, the amount of time remaining can be found
        /// from <see cref="RemainingLife"/>.
        /// </summary>
        [Browsable(false)]
        public bool HasInfiniteLife
        {
            get { return _expirationTime == int.MaxValue; }
        }

        /// <summary>
        /// Gets if this <see cref="ParticleEmitter"/> has been disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets if the <see cref="ParticleEmitter"/> is expired and all <see cref="Particle"/>s it has spawned
        /// have expired.
        /// </summary>
        /// <returns>True if the <see cref="ParticleEmitter"/> is ready to be disposed; otherwise false.</returns>
        [Browsable(false)]
        public bool IsExpired
        {
            get
            {
                if (RemainingLife != 0)
                    return false;

                if (ActiveParticles > 0)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Gets or sets the life of each <see cref="Particle"/> emitted.
        /// </summary>
        [Category(_particleCategoryName)]
        [Description("How long in milliseconds each Particle emitted lives after being emitted.")]
        [DisplayName("Life")]
        [DefaultValue(typeof(VariableInt), "2000")]
        public VariableInt Life { get; set; }

        /// <summary>
        /// Gets or sets the unique name of the particle effect.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The unique name of the particle effect.")]
        [DisplayName("Name")]
        [DefaultValue(_defaultName)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the origin of this <see cref="ParticleEmitter"/>.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The origin of the emitter.")]
        [DisplayName("Origin")]
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Gets or sets the collection of modifiers to use on the <see cref="Particle"/>s from this
        /// <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        [Category(_emitterCategoryName)]
        [Description("Collection of modifiers for individual particles.")]
        [DisplayName("Particle Modifiers")]
        public ParticleModifierCollection ParticleModifiers
        {
            get { return _particleModifiers; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _particleModifiers = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of <see cref="Particle"/>s that are emitted at each release.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("How many Particles are emitted on each release (see Release Rate).")]
        [DisplayName("Amount")]
        [DefaultValue(typeof(VariableUShort), "1")]
        public VariableUShort ReleaseAmount { get; set; }

        /// <summary>
        /// Gets or sets the initial color of the <see cref="Particle"/> when emitted.
        /// </summary>
        [Category(_particleCategoryName)]
        [Description("The color of a Particle when it is released.")]
        [DisplayName("Color")]
        [DefaultValue(typeof(VariableColor), "{255, 255, 255, 255}")]
        public VariableColor ReleaseColor { get; set; }

        /// <summary>
        /// Gets or sets the rate in milliseconds that <see cref="Particle"/>s are emitted.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The rate in milliseconds that Particles are released from this emitter.")]
        [DisplayName("Rate")]
        [DefaultValue(typeof(VariableUShort), "100")]
        public VariableUShort ReleaseRate { get; set; }

        /// <summary>
        /// Gets or sets the initial rotation in radians of the <see cref="Particle"/> when emitted.
        /// </summary>
        [Category(_particleCategoryName)]
        [Description("The angle in radians released Particles will be facing.")]
        [DisplayName("Rotation")]
        [DefaultValue(typeof(VariableFloat), "0")]
        public VariableFloat ReleaseRotation { get; set; }

        /// <summary>
        /// Gets or sets the initial scale of the <see cref="Particle"/> when emitted.
        /// </summary>
        [Category(_particleCategoryName)]
        [Description("The magnification multiplier of released Particles, where 1.0 is the normal sprite size.")]
        [DisplayName("Scale")]
        [DefaultValue(typeof(VariableFloat), "1")]
        public VariableFloat ReleaseScale { get; set; }

        /// <summary>
        /// Gets or sets the speed of <see cref="Particle"/>s when released.
        /// </summary>
        [Category(_particleCategoryName)]
        [Description("The speed released Particles will be moving, where 0.0 is no movement.")]
        [DisplayName("Speed")]
        [DefaultValue(typeof(VariableFloat), "50")]
        public VariableFloat ReleaseSpeed { get; set; }

        /// <summary>
        /// Gets the amount of time remaining for the <see cref="ParticleEmitter"/> before it is
        /// automatically terminated.
        /// </summary>
        /// <returns>The number of milliseconds remaining in the <see cref="ParticleEmitter"/>'s life, or
        /// zero if the emitter has already expired, or -1 if the emitter does not expire.</returns>
        [Browsable(false)]
        public int RemainingLife
        {
            get
            {
                if (HasInfiniteLife)
                    return -1;

                return Math.Max(0, _expirationTime - _lastUpdateTime);
            }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> to draw the <see cref="Particle"/>s.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Browsable(true)]
        [DisplayName("GrhData")]
        [Description("The GrhData that is drawn for the emitted particles.")]
        public Grh Sprite
        {
            get { return _sprite; }
        }

        /// <summary>
        /// Copies the values in this <see cref="ParticleEmitter"/> to another.
        /// </summary>
        /// <param name="destination">The <see cref="ParticleEmitter"/> to copy the values to.</param>
        public void CopyValuesTo(ParticleEmitter destination)
        {
            destination.BlendMode = BlendMode;
            destination.Budget = Budget;
            destination.Life = Life;
            destination.Origin = Origin;
            destination.ReleaseAmount = ReleaseAmount;
            destination.ReleaseColor = ReleaseColor;
            destination.ReleaseRate = ReleaseRate;
            destination.ReleaseRotation = ReleaseRotation;
            destination.ReleaseScale = ReleaseScale;
            destination.ReleaseSpeed = ReleaseSpeed;
            destination.Sprite.SetGrh(Sprite.GrhData, Sprite.AnimType, Sprite.LastUpdated);
            destination.ParticleModifiers = ParticleModifiers.DeepCopy();
        }

        /// <summary>
        /// Expires a <see cref="Particle"/> in the <see cref="particles"/> array, replacing the <paramref name="index"/>
        /// with a living particle, unless the <paramref name="index"/> is the last living particle.
        /// </summary>
        /// <param name="index">Index of the <see cref="Particle"/> to expire.</param>
        void ExpireParticle(int index)
        {
            SwapParticles(index, _lastAliveIndex--);
        }

        /// <summary>
        /// When overridden in the derived class, generates the offset and normalized force vectors to
        /// release the <see cref="Particle"/> at.
        /// </summary>
        /// <param name="particle">The <see cref="Particle"/> that the values are being generated for.</param>
        /// <param name="offset">The offset vector.</param>
        /// <param name="force">The normalized force vector.</param>
        protected abstract void GenerateParticleOffsetAndForce(Particle particle, out Vector2 offset, out Vector2 force);

        /// <summary>
        /// Gets the absolute position from a relative position and the origin of this emitter.
        /// </summary>
        /// <param name="relativePosition">The relative position.</param>
        /// <param name="absolutePosition">The resulting absolute position.</param>
        public void GetAbsoultePosition(ref Vector2 relativePosition, out Vector2 absolutePosition)
        {
            Vector2.Add(ref relativePosition, ref _origin, out absolutePosition);
        }

        /// <summary>
        /// Calculates the normalized force vector for a given direction.
        /// </summary>
        /// <param name="direction">The direction in radians to get the force vector for.</param>
        /// <param name="force">The force vector for the given <paramref name="direction"/>.</param>
        protected static void GetForce(float direction, out Vector2 force)
        {
            force = new Vector2((float)Math.Sin(direction), (float)Math.Cos(direction));
        }

        /// <summary>
        /// Gets an array of the <see cref="Particle"/>s handled by this <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <returns>An array of the <see cref="Particle"/>s handled by this <see cref="ParticleEmitter"/>.</returns>
        public Particle[] GetParticlesArray()
        {
            return particles;
        }

        /// <summary>
        /// Reads the <see cref="ParticleEmitter"/> settings from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        public void Read(IValueReader reader)
        {
            // Read the primary values
            Name = reader.ReadString(_nameKeyName);
            BlendMode = reader.ReadEnum<SpriteBlendMode>(_blendModeKeyName);
            Budget = reader.ReadInt(_budgetKeyName);
            Life = reader.ReadVariableInt(_lifeKeyName);
            Origin = reader.ReadVector2(_originKeyName);
            ReleaseAmount = reader.ReadVariableUShort(_releaseAmountKeyName);
            ReleaseColor = reader.ReadVariableColor(_releaseColorKeyName);
            ReleaseRate = reader.ReadVariableUShort(_releaseRateKeyName);
            ReleaseRotation = reader.ReadVariableFloat(_releaseRotationKeyName);
            ReleaseScale = reader.ReadVariableFloat(_releaseScaleKeyName);
            ReleaseSpeed = reader.ReadVariableFloat(_releaseSpeedKeyName);
            Sprite.SetGrh(reader.ReadGrhIndex(_grhIndexKeyName));

            // Read the custom values
            var customValuesReader = reader.ReadNode(_customValuesNodeName);
            ReadCustomValues(customValuesReader);

            // Read the modifier collection
            ParticleModifiers.Read(_particleModifiersNodeName, reader);
            EmitterModifiers.Read(_emitterModifiersNodeName, reader);
        }

        /// <summary>
        /// When overridden in the derived class, reads all custom state values from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the state values from.</param>
        protected abstract void ReadCustomValues(IValueReader reader);

        /// <summary>
        /// Releases one or more particles.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="amount">The number of particles to release.</param>
        void ReleaseParticles(int currentTime, int amount)
        {
            // Find how many we can actually release
            int lastIndex = Math.Min(Budget - 1, _lastAliveIndex + amount);

            // Ensure our particles array is large enough to fit the new particles.
            // When we resize the array, we use the "next power of two" sizing concept to reduce the
            // memory fragmentation (.NET internally does the same with most collections). To speed things up,
            // we just find the next power of two instead of looping until we have a large enough value.
            if (particles.Length - 1 < lastIndex)
            {
                int newSize = BitOps.NextPowerOf2(lastIndex + 1);
                Debug.Assert(BitOps.IsPowerOf2(newSize),
                             "If this assert fails, something is probably wrong with BitOps.NextPowerOf2() or BitOps.IsPowerOf2().");
                Debug.Assert(newSize >= lastIndex + 1);
                Array.Resize(ref particles, newSize);
            }

            // Start releasing the particles
            bool hasReleaseModifiers = ParticleModifiers.HasReleaseModifiers;
            for (int i = _lastAliveIndex + 1; i <= lastIndex; i++)
            {
                var particle = particles[i];
                if (particle == null)
                {
                    particle = Particle.Create();
                    particles[i] = particle;
                }

                // Set up the particle
                particle.Momentum = Vector2.Zero;
                particle.LifeStart = currentTime;
                particle.LifeEnd = currentTime + Life.GetNext();
                particle.Rotation = ReleaseRotation.GetNext();
                particle.Scale = ReleaseScale.GetNext();
                ReleaseColor.GetNext(ref particle.Color);

                // Get the offset and force
                Vector2 offset;
                Vector2 force;
                GenerateParticleOffsetAndForce(particle, out offset, out force);

                // Set the position
                Vector2.Add(ref _origin, ref offset, out particle.Position);

                // Set the velocity
                Vector2.Multiply(ref force, ReleaseSpeed.GetNext(), out particle.Velocity);

                if (hasReleaseModifiers)
                    ParticleModifiers.ProcessReleasedParticle(this, particle);
            }

            // Increase the index of the last active particle
            _lastAliveIndex = lastIndex;
        }

        /// <summary>
        /// Sets the life of the <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="totalLife">The total life of the <see cref="ParticleEmitter"/> in milliseconds. If less
        /// than or equal to zero, the <see cref="ParticleEmitter"/> will never expire.</param>
        public void SetEmitterLife(int currentTime, int totalLife)
        {
            if (totalLife <= 0)
                _expirationTime = int.MaxValue;
            else
                _expirationTime = currentTime + totalLife;
        }

        /// <summary>
        /// Swaps two <see cref="Particle"/>s by index.
        /// </summary>
        /// <param name="aIndex">The first index.</param>
        /// <param name="bIndex">The second index.</param>
        void SwapParticles(int aIndex, int bIndex)
        {
            var tmp = particles[aIndex];
            particles[aIndex] = particles[bIndex];
            particles[bIndex] = tmp;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} [{1}] - {2}", Name, Origin, GetType().Name);
        }

        /// <summary>
        /// Updates the <see cref="ParticleEmitter"/> and all <see cref="Particle"/>s it has created.
        /// </summary>
        /// <param name="currentTime">The current time.</param>>
        public void Update(int currentTime)
        {
            // Get the elapsed time
            // On the first update, just assume 10 ms have elapsed
            int elapsedTime;

            if (_lastUpdateTime == int.MinValue)
            {
                _nextReleaseTime = currentTime;
                elapsedTime = 10;
            }
            else
                elapsedTime = Math.Min(MaxDeltaTime, currentTime - _lastUpdateTime);

            _lastUpdateTime = currentTime;

            // Update the emitter modifiers
            EmitterModifiers.ProcessEmitter(this, elapsedTime);

            // Check if the sprite is loaded
            if (Sprite == null || Sprite.GrhData == null)
            {
                EmitterModifiers.RestoreEmitter(this);
                return;
            }

            // Update the current time on the modifiers
            ParticleModifiers.UpdateCurrentTime(currentTime);

            // Update the sprite
            Sprite.Update(currentTime);

            // Check to spawn more particles
            if (RemainingLife != 0)
            {
                // Do not allow the releasing catch-up time to exceed the _maxDeltaTime
                if (_nextReleaseTime < currentTime - MaxDeltaTime)
                    _nextReleaseTime = currentTime - MaxDeltaTime;

                // Keep calculating the releases until we catch up to the current time
                int amountToRelease = 0;
                while (_nextReleaseTime < currentTime)
                {
                    amountToRelease += ReleaseAmount.GetNext();
                    _nextReleaseTime += ReleaseRate.GetNext();
                }

                // Release the particles, if there are any
                if (amountToRelease > 0)
                    ReleaseParticles(currentTime, amountToRelease);
            }

            // Update the particles
            bool hasUpdateModifiers = ParticleModifiers.HasUpdateModifiers;
            int i = 0;
            while (i <= _lastAliveIndex)
            {
                var particle = particles[i];

                // Check if the particle has expired
                if (particle.LifeEnd <= currentTime)
                {
                    // We do NOT increment because once we expire the particle, this slot will become in use by
                    // another particle, so we will have to update at this index again
                    ExpireParticle(i);
                    continue;
                }

                // Process the particle with the modifiers
                if (hasUpdateModifiers)
                    ParticleModifiers.ProcessUpdatedParticle(this, particle, elapsedTime);

                // Update the particle
                particle.Update(elapsedTime);

                ++i;
            }

            EmitterModifiers.RestoreEmitter(this);
        }

        /// <summary>
        /// Writes the <see cref="ParticleEmitter"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(IValueWriter writer)
        {
            // Write the primary values
            writer.Write(_nameKeyName, Name);
            writer.Write(_blendModeKeyName, BlendMode);
            writer.Write(_budgetKeyName, Budget);
            writer.Write(_lifeKeyName, Life);
            writer.Write(_originKeyName, Origin);
            writer.Write(_releaseAmountKeyName, ReleaseAmount);
            writer.Write(_releaseColorKeyName, ReleaseColor);
            writer.Write(_releaseRateKeyName, ReleaseRate);
            writer.Write(_releaseRotationKeyName, ReleaseRotation);
            writer.Write(_releaseScaleKeyName, ReleaseScale);
            writer.Write(_releaseSpeedKeyName, ReleaseSpeed);
            writer.Write(_grhIndexKeyName, Sprite.GrhData != null ? Sprite.GrhData.GrhIndex : GrhIndex.Invalid);

            // Write the custom values
            writer.WriteStartNode(_customValuesNodeName);
            {
                WriteCustomValues(writer);
            }
            writer.WriteEndNode(_customValuesNodeName);

            // Write the modifier collection
            ParticleModifiers.Write(_particleModifiersNodeName, writer);
            EmitterModifiers.Write(_emitterModifiersNodeName, writer);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected abstract void WriteCustomValues(IValueWriter writer);

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            // Dispose the particles so they can be used again
            foreach (var particle in particles)
            {
                if (particle != null && !particle.IsDisposed)
                    particle.Dispose();
            }
        }

        #endregion
    }
}