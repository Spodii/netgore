using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Base class for all <see cref="IParticleEmitter"/>s.
    /// </summary>
    /// <remarks>
    /// When passing the <see cref="ParticleEmitter"/> to higher levels of the application where the emitter should not be modified
    /// at all, pass using the <see cref="IParticleEmitter"/> interface. All <see cref="IParticleEmitter"/>s should derive from
    /// this class instead of trying to implement <see cref="IParticleEmitter"/> on their own.
    /// </remarks>
    public abstract class ParticleEmitter : IParticleEmitter
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The default name given to a <see cref="ParticleEmitter"/>.
        /// </summary>
        public const string DefaultName = "Unnamed";

        /// <summary>
        /// The maximum value allowed for the delta time for updating particles and emitters. If the real delta
        /// time is greater than this value, is will be reduced to this value. This is to prevent ugly side-effects
        /// from emitters trying to play too much catch-up. Recommended to keep this at the default value.
        /// </summary>
        public const int MaxDeltaTime = 100;

        const string _blendModeKeyName = "BlendMode";
        const string _budgetKeyName = "Budget";
        const string _customValuesNodeName = "CustomValues";
        const BlendMode _defaultBlendMode = BlendMode.Add;
        const int _defaultBudget = 5000;
        const string _emitterCategoryName = "Emitter";
        const string _emitterLifeKeyName = "EmitterLife";
        const string _emitterModifiersNodeName = "EmitterModifiers";
        const string _grhIndexKeyName = "Grh";

        /// <summary>
        /// The initial size of the particle array.
        /// </summary>
        const int _initialParticleArraySize = 64;

        const string _nameKeyName = "Name";
        const string _originKeyName = "Origin";
        const string _particleCategoryName = "Particle";
        const string _particleLifeKeyName = "ParticleLife";
        const string _particleModifiersNodeName = "ParticleModifiers";
        const string _releaseAmountKeyName = "ReleaseAmount";
        const string _releaseColorKeyName = "ReleaseColor";
        const string _releaseRateKeyName = "ReleaseRate";
        const string _releaseRotationKeyName = "ReleaseRotation";
        const string _releaseScaleKeyName = "ReleaseScale";
        const string _releaseSpeedKeyName = "ReleaseSpeed";

        static readonly IEnumerable<Type> _emitterTypes =
            TypeHelper.FindTypesThatInherit(typeof(ParticleEmitter), new Type[] { typeof(IParticleEffect) }).OrderBy(x => x.Name).
                ToCompact();

        readonly EmitterModifierCollection _emitterModifiers = new EmitterModifierCollection();
        readonly IParticleEffect _owner;
        readonly ParticleModifierCollection _particleModifiers = new ParticleModifierCollection();
        readonly Grh _sprite = new Grh();

        /// <summary>
        /// Holds the minimum x/y particle position (particle.Position). Gets set every update call.
        /// </summary>
        Vector2 _minParticlePosition = Vector2.Zero;

        /// <summary>
        /// Holds the maximum x/y particle position (particle.Position + (particle.Scale * sprite.Size)). Gets set every update call.
        /// </summary>
        Vector2 _maxParticlePosition = Vector2.Zero;

        int _budget;
        bool _isDisposed = false;

        /// <summary>
        /// The index for the <see cref="_particles"/> array of the last particle that is alive.
        /// </summary>
        int _lastAliveIndex = -1;

        TickCount _lastUpdateTime = TickCount.MinValue;
        int _life = -1;
        string _name;
        TickCount _nextReleaseTime = TickCount.MinValue;
        Vector2 _origin;
        Particle[] _particles;
        TickCount _timeCreated = TickCount.Now;
        bool _wasKilled;

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
        /// <param name="owner">The <see cref="IParticleEffect"/> that owns this <see cref="IParticleEmitter"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        protected ParticleEmitter(IParticleEffect owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            _owner = owner;

            ((ParticleEffect)owner).AddParticleEmitter(this);

            _budget = DefaultBudget;
            _particles = new Particle[_initialParticleArraySize];

            // Set some default values
            BlendMode = _defaultBlendMode;
            ParticleLife = new VariableInt(2000);
            ReleaseAmount = new VariableUShort(1);
            ReleaseColor = new VariableColor(Color.White);
            ReleaseRate = new VariableUShort(100);
            ReleaseRotation = new VariableFloat(0);
            ReleaseScale = new VariableFloat(1);
            ReleaseSpeed = new VariableFloat(50);
            Name = DefaultName;
        }

        /// <summary>
        /// Gets or sets the default budget to give to new <see cref="ParticleEmitter"/>s when no budget
        /// is explicitly given.
        /// </summary>
        public static int DefaultBudget { get; set; }

        /// <summary>
        /// Gets the <see cref="StringComparer"/> to use when comparing the name of <see cref="ParticleEmitter"/>s.
        /// </summary>
        public static StringComparer EmitterNameComparer
        {
            get { return StringComparer.OrdinalIgnoreCase; }
        }

        /// <summary>
        /// Gets the <see cref="StringComparison"/> to use when comparing the name of <see cref="ParticleEmitter"/>s.
        /// </summary>
        public static StringComparison EmitterNameComparison
        {
            get { return StringComparison.OrdinalIgnoreCase; }
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="Type"/>s of <see cref="ParticleEmitter"/>s.
        /// </summary>
        public static IEnumerable<Type> EmitterTypes
        {
            get { return _emitterTypes; }
        }

        /// <summary>
        /// Gets the approximate current time.
        /// </summary>
        [Browsable(false)]
        protected TickCount LastUpdateTime
        {
            get { return _lastUpdateTime; }
        }

        /// <summary>
        /// Performs the real name changing of this <see cref="ParticleEmitter"/>. Must only be invoked by <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="newName">The new name.</param>
        internal void ChangeName(string newName)
        {
            _name = newName;
        }

        /// <summary>
        /// Copies the values in this <see cref="ParticleEmitter"/> to another.
        /// </summary>
        /// <param name="destination">The <see cref="ParticleEmitter"/> to copy the values to.</param>
        public void CopyValuesTo(IParticleEmitter destination)
        {
            var d = (ParticleEmitter)destination;

            d.BlendMode = BlendMode;
            d.Budget = Budget;
            d.EmitterLife = EmitterLife;
            d.ParticleLife = ParticleLife;
            d.Origin = Origin;
            d.Name = Name;
            d.ReleaseAmount = ReleaseAmount;
            d.ReleaseColor = ReleaseColor;
            d.ReleaseRate = ReleaseRate;
            d.ReleaseRotation = ReleaseRotation;
            d.ReleaseScale = ReleaseScale;
            d.ReleaseSpeed = ReleaseSpeed;
            d.Sprite.SetGrh(Sprite.GrhData, Sprite.AnimType, Sprite.LastUpdated);

            d.ParticleModifiers.Clear();
            d.ParticleModifiers.AddRange(ParticleModifiers.Select(x => x.DeepCopy()));

            d.EmitterModifiers.Clear();
            d.EmitterModifiers.AddRange(EmitterModifiers.Select(x => x.DeepCopy()));
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="ParticleEmitter"/> instance.
        /// </summary>
        /// <param name="newOwner">The owner of the new <see cref="ParticleEmitter"/>.</param>
        /// <returns>
        /// A deep copy of this <see cref="ParticleEmitter"/> with the <paramref name="newOwner"/> set.
        /// </returns>
        public abstract ParticleEmitter DeepCopy(IParticleEffect newOwner);

        /// <summary>
        /// Draws the <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        internal void Draw(ISpriteBatch sb)
        {
            // Check if we can even draw anything
            if (Sprite == null || ActiveParticles <= 0)
                return;

            // Keep track of what the blend mode was originally so we can restore it when done
            var originalBlendMode = sb.BlendMode;

            // Set the blend mode
            sb.BlendMode = BlendMode;

            // Get the origin to use for the particles
            var origin = Sprite.Size / 2f;

            // Draw the live particles
            for (var i = 0; i < ActiveParticles; i++)
            {
                var p = _particles[i];
                Sprite.Draw(sb, Owner.Position + p.Position, p.Color, SpriteEffects.None, p.Rotation, origin, p.Scale);
            }

            // Restore the blend mode
            sb.BlendMode = originalBlendMode;
        }

        /// <summary>
        /// Expires a <see cref="Particle"/> in the <see cref="_particles"/> array, replacing the <paramref name="index"/>
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
        /// Gets the array of <see cref="Particle"/>s in this <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <returns>The array of <see cref="Particle"/>s in this <see cref="ParticleEmitter"/>.</returns>
        protected Particle[] GetParticles()
        {
            return _particles;
        }

        /// <summary>
        /// Gets an array of the <see cref="Particle"/>s handled by this <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <returns>An array of the <see cref="Particle"/>s handled by this <see cref="ParticleEmitter"/>.</returns>
        public Particle[] GetParticlesArray()
        {
            return _particles;
        }

        /// <summary>
        /// When overridden in the derived class, resets the variables for the <see cref="ParticleEmitter"/> in the derived
        /// class to make it like this instance is starting over from the start. This only resets state variables such as
        /// the time the effect was created and how long it has to live, not properties such as position and emitting style.
        /// </summary>
        protected virtual void HandleReset()
        {
        }

        /// <summary>
        /// Kills the <see cref="ParticleEmitter"/>, which stops it from emitting any more particles but keeps any
        /// existing particles alive.
        /// </summary>
        public void Kill()
        {
            _wasKilled = true;
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
        void ReleaseParticles(TickCount currentTime, int amount)
        {
            // Find how many we can actually release
            var lastIndex = Math.Min(Budget - 1, _lastAliveIndex + amount);

            // Ensure our particles array is large enough to fit the new particles.
            // When we resize the array, we use the "next power of two" sizing concept to reduce the
            // memory fragmentation (.NET internally does the same with most collections). To speed things up,
            // we just find the next power of two instead of looping until we have a large enough value.
            if (_particles.Length - 1 < lastIndex)
            {
                var newSize = BitOps.NextPowerOf2(lastIndex + 1);
                Debug.Assert(BitOps.IsPowerOf2(newSize),
                    "If this assert fails, something is probably wrong with BitOps.NextPowerOf2() or BitOps.IsPowerOf2().");
                Debug.Assert(newSize >= lastIndex + 1);
                Array.Resize(ref _particles, newSize);
            }

            // Start releasing the particles
            var hasReleaseModifiers = ParticleModifiers.HasReleaseModifiers;
            for (var i = _lastAliveIndex + 1; i <= lastIndex; i++)
            {
                var particle = _particles[i];
                if (particle == null)
                {
                    particle = Particle.Create();
                    _particles[i] = particle;
                }

                // Set up the particle
                particle.Momentum = Vector2.Zero;
                particle.LifeStart = currentTime;
                particle.LifeEnd = (TickCount)(currentTime + ParticleLife.GetNext());
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
        /// <param name="totalLife">The total life of the <see cref="ParticleEmitter"/> in milliseconds. If less
        /// than or equal to zero, the <see cref="ParticleEmitter"/> will never expire.</param>
        public void SetEmitterLife(int totalLife)
        {
            if (totalLife <= 0)
                _life = -1;
            else
                _life = (int)(TickCount.Now + totalLife);
        }

        /// <summary>
        /// Swaps two <see cref="Particle"/>s by index.
        /// </summary>
        /// <param name="aIndex">The first index.</param>
        /// <param name="bIndex">The second index.</param>
        void SwapParticles(int aIndex, int bIndex)
        {
            var tmp = _particles[aIndex];
            _particles[aIndex] = _particles[bIndex];
            _particles[bIndex] = tmp;
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
        internal void Update(TickCount currentTime)
        {
            var forceEmit = false;

            // Get the elapsed time
            // On the first update, just assume 33 ms have elapsed
            int elapsedTime;

            if (_lastUpdateTime == TickCount.MinValue)
            {
                // This is the very first update
                forceEmit = true;
                _nextReleaseTime = currentTime;
                elapsedTime = 33;
            }
            else
            {
                // Not the first update
                elapsedTime = (int)Math.Min(MaxDeltaTime, currentTime - _lastUpdateTime);
            }

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
            if (forceEmit || (RemainingLife != 0 && ReleaseAmount.Max > 0 && ReleaseRate.Max > 0))
            {
                // Do not allow the releasing catch-up time to exceed the _maxDeltaTime
                if (_nextReleaseTime < currentTime - MaxDeltaTime)
                    _nextReleaseTime = currentTime - MaxDeltaTime;

                // Keep calculating the releases until we catch up to the current time
                var amountToRelease = 0;
                while (_nextReleaseTime <= currentTime)
                {
                    amountToRelease += ReleaseAmount.GetNext();
                    _nextReleaseTime += ReleaseRate.GetNext();
                }

                // Release the particles, if there are any
                if (amountToRelease > 0)
                    ReleaseParticles(currentTime, amountToRelease);
            }
            else
            {
                // Set the next release time to now so that if the emitter starts releasing, it won't release a ton
                // as it catches back up
                _nextReleaseTime = currentTime - MaxDeltaTime;
            }

            // Update the particles
            var hasUpdateModifiers = ParticleModifiers.HasUpdateModifiers;
            var i = 0;
            Vector2 minParticlePosition = Vector2.Zero;
            Vector2 maxParticlePosition = Vector2.Zero;
            Vector2 spriteSize = Sprite.Size;
            while (i <= _lastAliveIndex)
            {
                var particle = _particles[i];

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

                // Update the min/max origin offsets
                Vector2 posMin = particle.Position;

                Vector2 particleSize;
                Vector2.Multiply(ref spriteSize, particle.Scale, out particleSize);

                Vector2 posMax;
                Vector2.Add(ref posMin, ref particleSize, out posMax);

                if (posMin.X < minParticlePosition.X) minParticlePosition.X = posMin.X;
                if (posMin.Y < minParticlePosition.Y) minParticlePosition.Y = posMin.Y;

                if (posMax.X > maxParticlePosition.X) maxParticlePosition.X = posMax.X;
                if (posMax.Y > maxParticlePosition.Y) maxParticlePosition.Y = posMax.Y;

                ++i;
            }

            _minParticlePosition = minParticlePosition;
            _maxParticlePosition = maxParticlePosition;

            EmitterModifiers.RestoreEmitter(this);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected abstract void WriteCustomValues(IValueWriter writer);

        #region IParticleEmitter Members

        /// <summary>
        /// Notifies listeners when this <see cref="IParticleEmitter"/> has bene disposed.
        /// </summary>
        public event TypedEventHandler<IParticleEmitter> Disposed;

        /// <summary>
        /// Gets the number of living <see cref="Particle"/>s.
        /// </summary>
        [Browsable(false)]
        public int ActiveParticles
        {
            get { return _lastAliveIndex + 1; }
        }

        /// <summary>
        /// Gets or sets the <see cref="BlendMode"/> to use when rendering the <see cref="Particle"/>s
        /// emitted by this <see cref="ParticleEmitter"/>.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The blending mode to use when rendering Particles from this emitter.")]
        [DisplayName("Blend Mode")]
        [DefaultValue(_defaultBlendMode)]
        public BlendMode BlendMode { get; set; }

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

                Array.Resize(ref _particles, _budget);

                if (_lastAliveIndex >= _particles.Length - 1)
                    _lastAliveIndex = _particles.Length - 1;
            }
        }

        /// <summary>
        /// Gets or sets how long, in milliseconds, the emitter lives after being created. If less than 0, it lives
        /// indefinitely.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("How long, in milliseconds, the emitter lives. If less than 0, it lives indefinitely.")]
        [DisplayName("Life")]
        [DefaultValue(-1)]
        public int EmitterLife
        {
            get { return _life; }
            set
            {
                if (_life == value)
                    return;

                _life = value;
            }
        }

        /// <summary>
        /// Gets the collection of modifiers to use on the <see cref="ParticleEmitter"/>.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("Collection of modifiers for the actual emitter.")]
        [DisplayName("Emitter Modifiers")]
        public EmitterModifierCollection EmitterModifiers
        {
            get { return _emitterModifiers; }
        }

        /// <summary>
        /// Gets if this <see cref="ParticleEmitter"/> has an infinite life span. If true,
        /// it will never expire automatically. If false, the amount of time remaining can be found
        /// from <see cref="RemainingLife"/>.
        /// </summary>
        [Browsable(false)]
        public bool HasInfiniteLife
        {
            get { return _timeCreated < 0; }
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
        /// Gets or sets the name of the <see cref="IParticleEmitter"/> that is unique to the <see cref="IParticleEmitter.Owner"/>.
        /// If set to a value that is not unique in the owner <see cref="IParticleEffect"/>, then a unique name will be generated.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The name of the particle emitter, unique to the particle effect.")]
        [DisplayName("Name")]
        [DefaultValue(DefaultName)]
        public string Name
        {
            get { return _name; }
            set
            {
                // This property does not actually change the name directly. Instead, it forwards the new name request to
                // IParticleEffect and then ChangeName() is invoked if successful.

                // Check for a new value
                if (EmitterNameComparer.Equals(_name, value))
                    return;

                // Get a name guaranteed to be unique
                var newName = Owner.GenerateUniqueEmitterName(value);

                // Let the owner know about the new name
                Owner.TryRenameEmitter(this, newName);
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
        /// Gets the <see cref="IParticleEffect"/> that owns this <see cref="IParticleEmitter"/>.
        /// </summary>
        [Browsable(false)]
        public IParticleEffect Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Gets or sets the life of each <see cref="Particle"/> emitted.
        /// </summary>
        [Category(_particleCategoryName)]
        [Description("How long in milliseconds each Particle emitted lives after being emitted.")]
        [DisplayName("Life")]
        [DefaultValue(typeof(VariableInt), "2000")]
        public VariableInt ParticleLife { get; set; }

        /// <summary>
        /// Gets the collection of modifiers to use on the <see cref="Particle"/>s from this
        /// <see cref="ParticleEmitter"/>.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("Collection of modifiers for individual particles.")]
        [DisplayName("Particle Modifiers")]
        public ParticleModifierCollection ParticleModifiers
        {
            get { return _particleModifiers; }
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
        /// automatically terminated. If less than zero, it will never be terminated automatically.
        /// If zero, this <see cref="IParticleEmitter"/> is no longer emitting particles.
        /// </summary>
        [Browsable(false)]
        public int RemainingLife
        {
            get
            {
                if (_wasKilled)
                    return 0;

                if (HasInfiniteLife)
                    return -1;

                if (_life < 0)
                    return -1;

                var ret = Math.Max(0, _life - ((int)TickCount.Now - (int)_timeCreated));
                return ret;
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
        /// Immediately terminates the <see cref="IParticleEmitter"/> and all <see cref="Particle"/>s in it.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            // Dispose the particles so they can be used again
            foreach (var particle in _particles)
            {
                if (particle != null && !particle.IsDisposed)
                    particle.Dispose();
            }

            if (Disposed != null)
                Disposed.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            // Read the primary values
            Name = reader.ReadString(_nameKeyName);
            BlendMode = reader.ReadEnum<BlendMode>(_blendModeKeyName);
            Budget = reader.ReadInt(_budgetKeyName);
            EmitterLife = reader.ReadInt(_emitterLifeKeyName);
            ParticleLife = reader.ReadVariableInt(_particleLifeKeyName);
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
        /// Forces the <see cref="IParticleEmitter"/> to be reset from the start. This only resets state variables such as
        /// the time the effect was created and how long it has to live, not properties such as position and emitting style.
        /// Has no effect when disposed.
        /// </summary>
        public void Reset()
        {
            if (IsDisposed)
            {
                const string errmsg = "Tried to reset a disposed ParticleEffect `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            _timeCreated = TickCount.Now;
            _lastUpdateTime = TickCount.MinValue;
            _nextReleaseTime = TickCount.MinValue;
            _wasKilled = false;
            _minParticlePosition = Vector2.Zero;
            _maxParticlePosition = Vector2.Zero;

            HandleReset();
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        public bool InView(ICamera2D camera)
        {
            // Check if in view by using the _min/_maxParticlePosition
            
            // If both are equal to 0, then it probably has not been updated yet, so return true to be safe
            if (_minParticlePosition == Vector2.Zero && _maxParticlePosition == Vector2.Zero)
                return true;

            // Create a rect of the world position covering the area
            Vector2 ownerPos = Owner.Position;
            Vector2 min = _minParticlePosition + ownerPos;
            Vector2 max = _maxParticlePosition + ownerPos;
            Vector2 size = max - min;
            Rectangle rect = new Rectangle(min.X, min.Y, size.X, size.Y);

            // Inflact the rect's size to cover more area, just to be safe
            rect.Inflate(96);

            return camera.InView(rect);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            // Write the primary values
            writer.Write(_nameKeyName, Name);
            writer.Write(_blendModeKeyName, BlendMode);
            writer.Write(_budgetKeyName, Budget);
            writer.Write(_emitterLifeKeyName, EmitterLife);
            writer.Write(_particleLifeKeyName, ParticleLife);
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

        #endregion
    }
}