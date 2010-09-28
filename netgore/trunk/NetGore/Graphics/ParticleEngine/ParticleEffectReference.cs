using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// An instance for a particle effect that is referenced by name and displayed at a custom position. This provides an easy
    /// way to reference and persist a particle effect by name instead of having to persist the a deep copy.
    /// </summary>
    public class ParticleEffectReference : IPersistable
    {
        const string _categoryName = "Map Particle Effect";

        string _particleEffectName;
        ParticleEmitter _particleEmitter;
        Vector2 _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectReference"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public ParticleEffectReference(IValueReader reader)
        {
            ReadState(reader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectReference"/> class.
        /// </summary>
        /// <param name="particleEffectName">The name of the particle effect.</param>
        public ParticleEffectReference(string particleEffectName)
        {
            ParticleEffectName = particleEffectName;
        }

        /// <summary>
        /// Gets or sets the name of the particle effect to display. When the name is not a valid particle effect, the effect
        /// will be set to null.
        /// </summary>
        [SyncValue]
        [Category(_categoryName)]
        [DisplayName("Particle Effect Name")]
        [Browsable(true)]
        public string ParticleEffectName
        {
            get { return _particleEffectName; }
            set
            {
                if (_particleEffectName == value)
                    return;

                _particleEffectName = value;

                ParticleEmitter = ParticleEmitterFactory.LoadEmitter(ContentPaths.Build, _particleEffectName);

                if (ParticleEmitter != null)
                    ParticleEmitter.Origin = Position;
            }
        }

        /// <summary>
        /// Gets the <see cref="ParticleEmitter"/> instance. Can be null.
        /// </summary>
        [Browsable(false)]
        protected ParticleEmitter ParticleEmitter
        {
            get { return _particleEmitter; }
            private set
            {
                if (_particleEmitter == value)
                    return;

                _particleEmitter = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the particle effect.
        /// </summary>
        [SyncValue]
        [Category(_categoryName)]
        [DisplayName("Position")]
        [Browsable(true)]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position == value)
                    return;

                _position = value;

                if (ParticleEmitter != null)
                    ParticleEmitter.Origin = Position;
            }
        }

        /// <summary>
        /// Draws the <see cref="ParticleEffectReference"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public void Draw(ISpriteBatch sb)
        {
            if (ParticleEmitter == null)
                return;

            ParticleEmitter.Draw(sb);
        }

        /// <summary>
        /// Updates the <see cref="ParticleEffectReference"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(TickCount currentTime)
        {
            if (ParticleEmitter == null)
                return;

            ParticleEmitter.Update(currentTime);

            if (ParticleEmitter.IsExpired)
                ParticleEmitter.Reset();
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}