using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    public class ParticleEmitter
    {
        /// <summary>
        /// The array of <see cref="Particle"/>s.
        /// </summary>
        protected Particle[] particles;

        readonly ContentManager _contentManager;

        int _budget;

        /// <summary>
        /// The index for the <see cref="particles"/> array of the last particle that is alive.
        /// </summary>
        int _lastAliveIndex = -1;

        int _lastUpdateTime = int.MinValue;
        int _nextReleaseTime;
        Texture2D _particleTexture;
        TextureAssetName _textureAssetName;

        /// <summary>
        /// Initializes the <see cref="ParticleEmitter"/> class.
        /// </summary>
        static ParticleEmitter()
        {
            DefaultBudget = 1000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitter"/> class.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/> used to load assets.</param>
        public ParticleEmitter(ContentManager contentManager) : this(contentManager, DefaultBudget)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitter"/> class.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/> used to load assets.</param>
        /// <param name="budget">The initial particle budget.</param>
        public ParticleEmitter(ContentManager contentManager, int budget)
        {
            if (contentManager == null)
                throw new ArgumentNullException("contentManager");
            if (budget < 1)
                throw new ArgumentOutOfRangeException("budget", "budget must be greater than 0.");

            _contentManager = contentManager;

            _budget = budget;
            particles = new Particle[budget];

            // Set some default values
            Budget = budget;
            Life = new VariableInt(2000);
            ReleaseAmount = new VariableUShort(1);
            ReleaseColor = new VariableColor(new Color(0, 0, 0, 255), new Color(255,255,255,255));
            ReleaseRate = new VariableUShort(50);
            ReleaseRotation = new VariableFloat(0);
            ReleaseScale = new VariableFloat(32);
            ReleaseSpeed = new VariableFloat(50);
        }

        /// <summary>
        /// Gets the number of living <see cref="Particle"/>s.
        /// </summary>
        public int ActiveParticles
        {
            get { return _lastAliveIndex + 1; }
        }

        /// <summary>
        /// Gets or sets the maximum number of live particles this <see cref="ParticleEmitter"/> may create at
        /// any given time.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> less than or equal to zero.</exception>
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
        /// Gets or sets the life of each <see cref="Particle"/> emitted.
        /// </summary>
        public VariableInt Life { get; set; }

        /// <summary>
        /// Gets or sets the origin of this <see cref="ParticleEmitter"/>.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Gets the <see cref="Texture2D"/> to apply to the <see cref="Particle"/>s.
        /// </summary>
        public Texture2D ParticleTexture
        {
            get { return _particleTexture; }
        }

        public TextureAssetName ParticleTextureName
        {
            get { return _textureAssetName; }
            set
            {
                if (_textureAssetName == value)
                    return;

                _textureAssetName = value;

                // Update the texture
                if (!_textureAssetName.ContentExists())
                {
                    _particleTexture = null;
                    return;
                }

                try
                {
                    // HACK: Need a good way to remove the Grhs/ string constant
                    _particleTexture = _contentManager.Load<Texture2D>(_textureAssetName);
                }
                catch (ContentLoadException)
                {
                    _particleTexture = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of <see cref="Particle"/>s that are emitted at each release.
        /// </summary>
        public VariableUShort ReleaseAmount { get; set; }

        /// <summary>
        /// Gets or sets the initial color of the <see cref="Particle"/> when emitted.
        /// </summary>
        public VariableColor ReleaseColor { get; set; }

        /// <summary>
        /// Gets or sets the rate in milliseconds that <see cref="Particle"/>s are emitted.
        /// </summary>
        public VariableUShort ReleaseRate { get; set; }

        /// <summary>
        /// Gets or sets the initial rotation in radians of the <see cref="Particle"/> when emitted.
        /// </summary>
        public VariableFloat ReleaseRotation { get; set; }

        /// <summary>
        /// Gets or sets the initial scale of the <see cref="Particle"/> when emitted.
        /// </summary>
        public VariableFloat ReleaseScale { get; set; }

        /// <summary>
        /// Gets or sets the speed of <see cref="Particle"/>s when released.
        /// </summary>
        public VariableFloat ReleaseSpeed { get; set; }

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
        /// Gets an array of the <see cref="Particle"/>s handled by this <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <returns>An array of the <see cref="Particle"/>s handled by this <see cref="ParticleEmitter"/>.</returns>
        public Particle[] GetParticlesArray()
        {
            return particles;
        }

        /// <summary>
        /// Gets the velocity vector for a given direction and speed.
        /// </summary>
        /// <param name="direction">The direction in radians.</param>
        /// <param name="speed">The speed the particle is heading in the given <paramref name="direction"/>.</param>
        /// <param name="force">The force vector for the given <paramref name="direction"/>
        /// and <paramref name="speed"/>.</param>
        protected static void GetVelocity(float direction, float speed, out Vector2 force)
        {
            float radians = RandomHelper.NextFloat(direction);

            force = new Vector2((float)(Math.Sin(radians) * speed), (float)(Math.Cos(radians) * speed));
        }

        /// <summary>
        /// Initializes a <see cref="Particle"/>.
        /// </summary>
        /// <param name="particleIndex">Index of the particle to initialize.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="releasePosition">The position to release the particle.</param>
        /// <param name="releaseVelocity">The velocity vector to apply to the <see cref="Particle"/>.</param>
        protected virtual void InitializeParticle(int particleIndex, float speed, out Vector2 releasePosition,
                                                  out Vector2 releaseVelocity)
        {
            float radians = RandomHelper.NextFloat(MathHelper.TwoPi);

            releasePosition = Origin;
            GetVelocity(radians, speed, out releaseVelocity);
        }

        /// <summary>
        /// Releases particles.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="amount">The number of particles to release.</param>
        void ReleaseParticles(int currentTime, int amount)
        {
            // Find how many we can actually release
            int lastIndex = Math.Min(particles.Length - 1, _lastAliveIndex + amount);

            // Start releasing the particles
            for (int i = _lastAliveIndex + 1; i <= lastIndex; i++)
            {
                var particle = particles[i];

                // Set up the particle
                particle.Momentum = Vector2.Zero;
                particle.LifeStart = currentTime;
                particle.LifeEnd = currentTime + Life.GetNext();
                particle.Rotation = ReleaseRotation.GetNext();
                particle.Scale = ReleaseScale.GetNext();
                particle.Color = ReleaseColor.GetNextAsVector4();
                InitializeParticle(i, ReleaseSpeed.GetNext(), out particle.Position, out particle.Velocity);

                particles[i] = particle;
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

                // TODO: Run update modifiers on the particle

                // Update the particle
                particle.Update(elapsedTime);

                particles[i++] = particle;
            }
        }
    }
}