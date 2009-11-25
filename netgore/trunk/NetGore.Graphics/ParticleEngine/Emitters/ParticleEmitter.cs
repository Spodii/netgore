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
        const SpriteBlendMode _defaultBlendMode = SpriteBlendMode.Additive;
        const int _defaultBudget = 5000;
        const string _emitterCategoryName = "Emitter";

        /// <summary>
        /// The initial size of the particle array.
        /// </summary>
        const int _initialParticleArraySize = 64;

        const string _particleCategoryName = "Particle";

        static readonly IEnumerable<Type> _emitterTypes =
            TypeHelper.FindTypesThatInherit(typeof(ParticleEmitter), Type.EmptyTypes, false).Concat(new Type[]
            { typeof(ParticleEmitter) }).Distinct().OrderBy(x => x.Name).ToCompact();

        /// <summary>
        /// The array of <see cref="Particle"/>s.
        /// </summary>
        protected Particle[] particles;

        int _budget;
        bool _isDisposed = false;

        /// <summary>
        /// The index for the <see cref="particles"/> array of the last particle that is alive.
        /// </summary>
        int _lastAliveIndex = -1;

        int _lastUpdateTime = int.MinValue;
        ParticleModifierCollection _modifiers = new ParticleModifierCollection();
        int _nextReleaseTime;
        Vector2 _origin;
        Grh _sprite;
        SpriteCategorization _spriteCategorization;

        /// <summary>
        /// Used to spread out sprite loading attempts when trying to load the sprite when it is null on Update().
        /// This way we suffer very little from an invalid Sprite, but still give the emitter a chance to reload
        /// the sprite.
        /// </summary>
        ushort _tryLoadSpriteTimeout = 0;

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
        /// Gets an IEnumerable of the <see cref="Type"/>s of <see cref="ParticleEmitter"/>s.
        /// </summary>
        public static IEnumerable<Type> EmitterTypes
        {
            get { return _emitterTypes; }
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
        /// Gets or sets the life of each <see cref="Particle"/> emitted.
        /// </summary>
        [Category(_particleCategoryName)]
        [Description("How long in milliseconds each Particle emitted lives after being emitted.")]
        [DisplayName("Life")]
        [DefaultValue(typeof(VariableInt), "2000")]
        public VariableInt Life { get; set; }

        /// <summary>
        /// Gets or sets the collection of modifiers to use on the <see cref="Particle"/>s from this
        /// <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        [Editor(EditorHelper.ParticleModifierCollectionEditorTypeName,
            EditorHelper.UITypeEditorTypeName)]
        [Category(_emitterCategoryName)]
        [Description("Collection of particle effect modifiers.")]
        [DisplayName("Modifiers")]
        public ParticleModifierCollection Modifiers
        {
            get { return _modifiers; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _modifiers = value;
            }
        }

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
        /// Gets the <see cref="ISprite"/> to draw the <see cref="Particle"/>s.
        /// </summary>
        [Browsable(false)]
        public Grh Sprite
        {
            get { return _sprite; }
        }

        /// <summary>
        /// Gets or sets the category for the <see cref="ISprite"/> used by this <see cref="ParticleEmitter"/>.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The sprite categorization that defines which sprite to use to draw the Particles.")]
        [DisplayName("Sprite")]
        public SpriteCategorization SpriteCategorization
        {
            get { return _spriteCategorization; }
            set
            {
                if (_spriteCategorization == value)
                    return;

                _spriteCategorization = value;
                TryLoadSprite();
            }
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
            destination.SpriteCategorization = SpriteCategorization;
            destination.Modifiers = Modifiers.DeepCopy();
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
            bool hasReleaseModifiers = Modifiers.HasReleaseModifiers;
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
                    Modifiers.ProcessReleasedParticle(this, particle);
            }

            // Increase the index of the last active particle
            _lastAliveIndex = lastIndex;
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
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected abstract void WriteCustomValues(IValueWriter writer);

        const string _blendModeKeyName = "BlendMode";
        const string _budgetKeyName = "Budget";
        const string _lifeKeyName = "Life";
        const string _originKeyName = "Origin";
        const string _releaseAmountKeyName = "ReleaseAmount";
        const string _releaseColorKeyName = "ReleaseColor";
        const string _releaseRateKeyName = "ReleaseRate";
        const string _releaseRotationKeyName = "ReleaseRotation";
        const string _releaseScaleKeyName = "ReleaseScale";
        const string _releaseSpeedKeyName = "ReleaseSpeed";
        const string _spriteCategorizationKeyName = "SpriteCategorization";
        const string _customValuesNodeName = "CustomValues";
        const string _modifiersNodeName = "Modifiers";

        /// <summary>
        /// Writes the <see cref="ParticleEmitter"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(IValueWriter writer)
        {
            // Write the primary values
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
            writer.Write(_spriteCategorizationKeyName, SpriteCategorization);

            // Write the custom values
            writer.WriteStartNode(_customValuesNodeName);
            {
                WriteCustomValues(writer);
            }
            writer.WriteEndNode(_customValuesNodeName);

            // Write the modifier collection
            Modifiers.Write(_modifiersNodeName, writer);
        }

        /// <summary>
        /// Tries to load the <see cref="_sprite"/> using the <see cref="SpriteCategorization"/>.
        /// </summary>
        void TryLoadSprite()
        {
            // Get the GrhData
            var grhData = GrhInfo.GetData(_spriteCategorization);
            if (grhData == null)
            {
                // Invalid GrhData...
                _sprite = null;
                return;
            }

            // Load the sprite
            _sprite = new Grh(grhData);

            _tryLoadSpriteTimeout = 0;
        }

        /// <summary>
        /// Updates the <see cref="ParticleEmitter"/> and all <see cref="Particle"/>s it has created.
        /// </summary>
        /// <param name="currentTime">The current time.</param>>
        public void Update(int currentTime)
        {
            // Check if the sprite is loaded
            if (Sprite == null)
            {
                // Try every update the first 5 times, then after that only try every 150 updates
                if (++_tryLoadSpriteTimeout < 5 || _tryLoadSpriteTimeout % 150 == 0)
                    TryLoadSprite();

                if (Sprite == null)
                    return;
            }

            // Update the current time on the modifiers
            Modifiers.UpdateCurrentTime(currentTime);

            // Update the sprite
            Sprite.Update(currentTime);

            // Get the elapsed time
            // On the first update, just assume 10 ms have elapsed
            int elapsedTime;

            if (_lastUpdateTime == int.MinValue)
            {
                _nextReleaseTime = currentTime;
                elapsedTime = 10;
            }
            else
                elapsedTime = currentTime - _lastUpdateTime;

            _lastUpdateTime = currentTime;

            // Check to spawn more particles
            int amountToRelease = 0;
            while (_nextReleaseTime < currentTime)
            {
                amountToRelease += ReleaseAmount.GetNext();
                _nextReleaseTime += ReleaseRate.GetNext();
            }

            if (amountToRelease > 0)
                ReleaseParticles(currentTime, amountToRelease);

            // Update the particles
            bool hasUpdateModifiers = Modifiers.HasUpdateModifiers;
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
                    Modifiers.ProcessUpdatedParticle(this, particle, elapsedTime);

                // Update the particle
                particle.Update(elapsedTime);

                ++i;
            }
        }

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